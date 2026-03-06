using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TheKiwiCoder;

namespace TheKiwiCoder {
    public class AgentDeath : MonoBehaviour
    {
        private Health health;
        private bool isDead = false;

        void Start()
        {
            health = GetComponent<Health>();
            if (health != null)
            {
                health.onHealthChanged.AddListener(OnHealthChanged);
            }
        }

        void OnHealthChanged(float current, float max)
        {
            if (!isDead && current <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            isDead = true;

            // 1. Disable Behaviour Tree (Stops AI logic)
            var bTreeRunner = GetComponent<BehaviourTreeRunner>();
            if (bTreeRunner != null) bTreeRunner.enabled = false;

            // 2. Disable NavMeshAgent (Stops physics movement)
            var navAgent = GetComponent<NavMeshAgent>();
            if (navAgent != null) navAgent.enabled = false;

            // 3. Disable all Colliders (Bullets pass through dead bodies)
            var colliders = GetComponentsInChildren<Collider>();
            foreach(var col in colliders)
            {
                col.enabled = false;
            }

            // 4. Disable UI Health Bar Canvas
            var canvas = GetComponentInChildren<Canvas>();
            if (canvas != null) canvas.enabled = false;

            // 5. Visually Fall Over (Simulate death animation)
            // A quick and dirty way to show they died without rigging a proper animation!
            StartCoroutine(FallOverRoutine());
        }

        IEnumerator FallOverRoutine()
        {
            Quaternion startRot = transform.rotation;
            // Rotate 90 degrees backward on the X axis to 'fall over'
            Quaternion targetRot = startRot * Quaternion.Euler(-90f, 0, 0);
            
            float duration = 0.5f;
            float timePassed = 0f;

            while (timePassed < duration)
            {
                transform.rotation = Quaternion.Slerp(startRot, targetRot, timePassed / duration);
                
                // Slowly drop them to the floor level so they don't float
                transform.position = Vector3.Lerp(transform.position, 
                                                   new Vector3(transform.position.x, 0f, transform.position.z), 
                                                   timePassed / duration);
                
                timePassed += Time.deltaTime;
                yield return null;
            }
            
            // Ensure final transform is perfectly set
            transform.rotation = targetRot;
        }

        void OnDestroy()
        {
            if (health != null)
            {
                health.onHealthChanged.RemoveListener(OnHealthChanged);
            }
        }
    }
}
