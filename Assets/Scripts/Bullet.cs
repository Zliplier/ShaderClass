using UnityEngine;

namespace TheKiwiCoder {
    public class Bullet : MonoBehaviour
    {
        public float speed = 15f;
        public float damage = 20f;
        public float lifetime = 5f;
        
        [HideInInspector]
        public GameObject shooter;

        private Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            rb.useGravity = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // Destroy bullet after a few seconds to prevent clutter
            Destroy(gameObject, lifetime);
        }

        void FixedUpdate()
        {
            rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        }

        void OnTriggerEnter(Collider other)
        {
            HandleCollision(other.gameObject);
        }

        void OnCollisionEnter(Collision collision)
        {
            HandleCollision(collision.gameObject);
        }

        private void HandleCollision(GameObject hitObject)
        {
            // Ignore collision with the shooter
            if (hitObject == shooter) return;

            // Attempt to get health
            Health health = hitObject.GetComponentInChildren<Health>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }

            // Destroy the bullet on any collision (except with shooter)
            Destroy(gameObject);
        }
    }
}
