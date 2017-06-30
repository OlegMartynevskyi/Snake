using UnityEngine;
using System.Collections;

namespace SnakeWorld
{
    sealed class Feeder
    {
        public readonly Transform transform;

        readonly FeederSetup settings;
        readonly float spawnRadius;
        int foodCount;

        Coroutine makeFoodRoutine;
        MonoBehaviour foodMaker;

        public Feeder(Terrarium terrarium, FeederSetup setup)
        {
            GameObject gameObject = new GameObject("Feeder");
            transform = gameObject.transform;
            transform.SetParent(terrarium.transform);
            spawnRadius = Mathf.Min(terrarium.size.x, terrarium.size.y);
            settings = setup;
            while (foodCount < setup.capacity)
            {
                InstantiateFood();
            }
        }

        public void UpdateFoodCount()
        {
            --foodCount;
        }

        public void StartMakeFood(MonoBehaviour behaviour)
        {
            if (foodMaker != null)
                return;
            foodMaker = behaviour;
            makeFoodRoutine = foodMaker.StartCoroutine(CreateFood());                
        }

        public void StopMakeFood()
        {
            if (foodMaker != null && makeFoodRoutine != null)
            {
                foodMaker.StopCoroutine(makeFoodRoutine);
            }
        }

        IEnumerator CreateFood()
        {
            while (true)
            {                
                if (foodCount < settings.capacity)
                {
                    InstantiateFood();
                }
                yield return new WaitForSeconds(settings.spawnDelay);
            }
        }

        void InstantiateFood()
        {
            Vector3 position = Vector3.zero;
            position.x = Random.Range(-spawnRadius, spawnRadius);
            position.y = Random.Range(-spawnRadius, spawnRadius);
            Object.Instantiate(settings.foodPrefab, position, Quaternion.identity, transform);
            ++foodCount;
        }
    }
}