using UnityEngine;
using UnityEngine.UI;

namespace TheKiwiCoder {
    public class HealthBarUI : MonoBehaviour
    {
        public Health healthComponent;
        public Image healthFillImage;
        public Transform cameraTransform;

        void Start()
        {
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }

            if (healthComponent != null)
            {
                healthComponent.onHealthChanged.AddListener(UpdateHealthBar);
                UpdateHealthBar(healthComponent.currentHealth, healthComponent.maxHealth);
            }
        }

        void UpdateHealthBar(float currentHealth, float maxHealth)
        {
            if (healthFillImage != null)
            {
                healthFillImage.fillAmount = currentHealth / maxHealth;
            }
        }

        void LateUpdate()
        {
            if (cameraTransform != null)
            {
                // Billboard the health bar to always face the camera
                transform.LookAt(transform.position + cameraTransform.rotation * Vector3.forward,
                    cameraTransform.rotation * Vector3.up);
            }
        }

        void OnDestroy()
        {
            if (healthComponent != null)
            {
                healthComponent.onHealthChanged.RemoveListener(UpdateHealthBar);
            }
        }
    }
}
