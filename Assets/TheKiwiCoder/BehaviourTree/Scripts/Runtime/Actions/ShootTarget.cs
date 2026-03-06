using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class ShootTarget : ActionNode
{
    public GameObject bulletPrefab;
    public float fireCooldown = 1.0f;
    public float fireForce = 15f;
    public float damage = 20f;
    public float verticalOffset = 1.0f;
    
    private float lastFireTime = -9999f;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        
        // Return failure if target is invalid or dead
        if (blackboard.target == null) return State.Failure;
        
        Health targetHealth = blackboard.target.GetComponent<Health>();
        if (targetHealth == null || targetHealth.currentHealth <= 0)
        {
            blackboard.target = null;
            return State.Failure;
        }
        
        // Target velocity for leading (prediction)
        UnityEngine.AI.NavMeshAgent targetNav = blackboard.target.GetComponent<UnityEngine.AI.NavMeshAgent>();
        Vector3 targetVelocity = targetNav != null ? targetNav.velocity : Vector3.zero;

        Vector3 targetCenter = blackboard.target.transform.position + Vector3.up * verticalOffset;
        float distanceToTarget = Vector3.Distance(context.transform.position, targetCenter);
        float leadTime = distanceToTarget / fireForce;
        Vector3 predictedTargetPos = targetCenter + targetVelocity * leadTime;

        // Aim at predicted target
        Vector3 aimDirection = (predictedTargetPos - context.transform.position).normalized;
        aimDirection.y = 0; // Ignore height for body rotation
        
        if (aimDirection != Vector3.zero)
        {
            // Smoothly rotate towards predicted position
            context.transform.rotation = Quaternion.Slerp(context.transform.rotation, Quaternion.LookRotation(aimDirection), Time.deltaTime * 15f);
        }

        // Check if cooldown has expired
        if (Time.time >= lastFireTime + fireCooldown)
        {
            lastFireTime = Time.time;
            
            // If bullet prefab isn't set, we create a fallback crude visual sphere
            GameObject bulletObj = null;
            if (bulletPrefab != null)
            {
                bulletObj = Object.Instantiate(bulletPrefab);
            }
            else
            {
                bulletObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bulletObj.transform.localScale = Vector3.one * 0.2f;
                // Add yellow material so it looks like a crude laser/bullet
                bulletObj.GetComponent<Renderer>().material.color = Color.yellow;
            }

            // Position it slightly in front of the agent (and up)
            bulletObj.transform.position = context.transform.position + Vector3.up * verticalOffset + context.transform.forward * 1.0f;
            
            // Calculate precise predicted position from the bullet's actual spawn point
            float bulletDist = Vector3.Distance(bulletObj.transform.position, targetCenter);
            float bulletLeadTime = bulletDist / fireForce;
            Vector3 precisePredictedPos = targetCenter + targetVelocity * bulletLeadTime;

            // Important to look towards predicted target so bullet local forward is correct
            Vector3 fireDir = precisePredictedPos - bulletObj.transform.position;
            bulletObj.transform.rotation = Quaternion.LookRotation(fireDir.normalized);

            // Add bullet logic
            Bullet bulletComp = bulletObj.GetComponent<Bullet>();
            if (bulletComp == null)
            {
                bulletComp = bulletObj.AddComponent<Bullet>();
                bulletComp.speed = fireForce;
                bulletComp.damage = damage;
            }
            
            bulletComp.shooter = context.gameObject;
            
            // Optional: return Running if we want to stay in this node longer, 
            // but success means we fired and let the tree decide what to do next.
            return State.Success;
        }

        return State.Running;
    }
}
