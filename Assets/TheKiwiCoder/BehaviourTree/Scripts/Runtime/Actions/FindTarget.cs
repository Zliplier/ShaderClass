using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class FindTarget : ActionNode
{
    public float searchRadius = 30f;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        
        // Find all health components in the scene
        Health[] allHealths = Object.FindObjectsOfType<Health>();
        
        Health closestHealth = null;
        float closestDistance = Mathf.Infinity;

        foreach (var healthObj in allHealths)
        {
            // Don't target ourselves!
            if (healthObj.gameObject == context.gameObject) continue;
            
            // Don't target dead things
            if (healthObj.currentHealth <= 0) continue;

            float distance = Vector3.Distance(context.transform.position, healthObj.transform.position);
            
            // Must be within search radius
            if (distance <= searchRadius && distance < closestDistance)
            {
                closestHealth = healthObj;
                closestDistance = distance;
            }
        }

        if (closestHealth != null)
        {
            blackboard.target = closestHealth.gameObject;
            return State.Success;
        }

        return State.Failure;
    }
}
