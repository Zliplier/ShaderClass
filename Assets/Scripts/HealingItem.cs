using UnityEngine;

namespace TheKiwiCoder {
    public class HealingItem : MonoBehaviour
    {
        public float healAmount = 50f;
        public float rotationSpeed = 90f;

        void Start()
        {
            // Ensure a Rigidbody exists for Trigger events to fire! Unity requires at least one Rigidbody.
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            rb.isKinematic = true; // Don't fall through floor
            rb.useGravity = false;

            // Ensure there is a generously sized trigger so agents can easily grab it
            // even if their NavMesh stopping distance makes them halt early.
            SphereCollider grabRadius = gameObject.AddComponent<SphereCollider>();
            grabRadius.isTrigger = true;
            grabRadius.radius = 1.5f; // Generous 1.5 meter grab radius!
        }

        void Update()
        {
            // Simple idle animation
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            // The safest way to find components on complex prefabs is to go to the very top (root) and search downwards
            Health health = other.transform.root.GetComponentInChildren<Health>();

            // Only heal if they have a health component and are actually wounded
            if (health != null && health.currentHealth > 0 && health.currentHealth < health.maxHealth)
            {
                health.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
