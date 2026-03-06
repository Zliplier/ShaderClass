using UnityEngine;
using UnityEngine.Events;

namespace TheKiwiCoder {
    public class Health : MonoBehaviour
    {
        public float maxHealth = 100f;
        public float currentHealth;
        
        [System.Serializable]
        public class HealthChangeEvent : UnityEvent<float, float> { }
        
        public HealthChangeEvent onHealthChanged = new HealthChangeEvent();

        void Start()
        {
            currentHealth = maxHealth;
            onHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            onHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void Heal(float amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            onHealthChanged?.Invoke(currentHealth, maxHealth);
        }
    }
}
