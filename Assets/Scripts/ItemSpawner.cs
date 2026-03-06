using UnityEngine;
using UnityEngine.AI;

namespace TheKiwiCoder {
    public class ItemSpawner : MonoBehaviour
    {
        public GameObject itemPrefab;
        public float spawnInterval = 10f;
        public int maxItemsOnMap = 5;
        public Vector2 spawnAreaMin = new Vector2(-20, -20);
        public Vector2 spawnAreaMax = new Vector2(20, 20);
        
        private float lastSpawnTime;

        void Update()
        {
            if (Time.time >= lastSpawnTime + spawnInterval)
            {
                lastSpawnTime = Time.time;
                SpawnItem();
            }
        }

        void SpawnItem()
        {
            // Don't spawn if we hit the limit
            HealingItem[] existingItems = FindObjectsOfType<HealingItem>();
            if (existingItems.Length >= maxItemsOnMap) return;

            // Generate random position
            Vector3 randomPos = new Vector3(
                Random.Range(spawnAreaMin.x, spawnAreaMax.x),
                0f,
                Random.Range(spawnAreaMin.y, spawnAreaMax.y)
            );

            // Ensure it's on the ground or NavMesh
            if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 10f, NavMesh.AllAreas))
            {
                randomPos = hit.position;
            }
            
            // Slightly elevate to prevent clipping
            randomPos.y += 0.5f;

            if (itemPrefab != null)
            {
                Instantiate(itemPrefab, randomPos, Quaternion.identity);
            }
            else
            {
                // Fallback primitive if no prefab is assigned
                GameObject fallbackItem = GameObject.CreatePrimitive(PrimitiveType.Cube);
                fallbackItem.transform.position = randomPos;
                fallbackItem.transform.localScale = Vector3.one * 0.5f;
                // Make it green for health
                fallbackItem.GetComponent<Renderer>().material.color = Color.green;
                // Needs to be a trigger
                fallbackItem.GetComponent<Collider>().isTrigger = true;
                // Add the logic
                fallbackItem.AddComponent<HealingItem>();
            }
        }
    }
}
