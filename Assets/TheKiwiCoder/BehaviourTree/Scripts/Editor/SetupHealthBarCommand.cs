using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;

namespace TheKiwiCoder {
    public class SetupHealthBarCommand : EditorWindow
    {
        [MenuItem("TheKiwiCoder/Setup Agents (Health & Death)")]
        public static void SetupHealthBars()
        {
            // Find all BehaviourTreeRunners in the current scene to identify the agents
            var agents = FindObjectsOfType<BehaviourTreeRunner>();

            if (agents.Length == 0)
            {
                Debug.LogWarning("No BehaviourTreeRunners found in the scene.");
                return;
            }

            int count = 0;

            foreach (var agent in agents)
            {
                GameObject agentGo = agent.gameObject;

                // 1. Add Health component
                Health health = agentGo.GetComponent<Health>();
                if (health == null)
                {
                    health = agentGo.AddComponent<Health>();
                    health.maxHealth = 100f;
                    health.currentHealth = 100f;
                    Undo.RegisterCreatedObjectUndo(health, "Add Health");
                }

                // Check if HealthBar is already set up
                Transform existingCanvas = agentGo.transform.Find("HealthBarCanvas");
                if (existingCanvas != null)
                {
                    continue; // Skip if already set up
                }

                // 2. Create Canvas GameObject
                GameObject canvasGo = new GameObject("HealthBarCanvas");
                canvasGo.transform.SetParent(agentGo.transform);
                canvasGo.transform.localPosition = new Vector3(0, 2.5f, 0); // Above the agent
                Undo.RegisterCreatedObjectUndo(canvasGo, "Create Health Bar Canvas");

                Canvas canvas = canvasGo.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.WorldSpace;
                
                RectTransform canvasRect = canvasGo.GetComponent<RectTransform>();
                canvasRect.sizeDelta = new Vector2(2f, 0.3f);
                canvasRect.localScale = Vector3.one;

                // 3. Create Background Image
                GameObject bgGo = new GameObject("Background");
                bgGo.transform.SetParent(canvasGo.transform, false);
                Image bgImage = bgGo.AddComponent<Image>();
                bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                
                RectTransform bgRect = bgGo.GetComponent<RectTransform>();
                bgRect.anchorMin = Vector2.zero;
                bgRect.anchorMax = Vector2.one;
                bgRect.offsetMin = Vector2.zero;
                bgRect.offsetMax = Vector2.zero;

                // 4. Create Foreground Image
                GameObject fgGo = new GameObject("Foreground");
                fgGo.transform.SetParent(canvasGo.transform, false);
                Image fgImage = fgGo.AddComponent<Image>();
                fgImage.color = Color.green;
                fgImage.type = Image.Type.Filled;
                fgImage.fillMethod = Image.FillMethod.Horizontal;
                fgImage.fillOrigin = (int)Image.OriginHorizontal.Left;
                fgImage.fillAmount = 1f;

                RectTransform fgRect = fgGo.GetComponent<RectTransform>();
                fgRect.anchorMin = Vector2.zero;
                fgRect.anchorMax = Vector2.one;
                fgRect.offsetMin = Vector2.zero;
                fgRect.offsetMax = Vector2.zero;

                // 5. Add HealthBarUI component
                HealthBarUI healthBarUI = canvasGo.AddComponent<HealthBarUI>();
                healthBarUI.healthComponent = health;
                healthBarUI.healthFillImage = fgImage;
                
                if (Camera.main != null)
                {
                    healthBarUI.cameraTransform = Camera.main.transform;
                }
                
                // 6. Add AgentDeath script
                AgentDeath agentDeath = agentGo.GetComponent<AgentDeath>();
                if (agentDeath == null)
                {
                    agentDeath = agentGo.AddComponent<AgentDeath>();
                    Undo.RegisterCreatedObjectUndo(agentDeath, "Add Agent Death");
                }

                count++;
            }

            Debug.Log($"Successfully setup health bars and death states on {count} agents in the scene.");
        }
    }
}
