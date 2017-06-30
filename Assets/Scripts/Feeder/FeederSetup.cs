using UnityEngine;


namespace SnakeWorld
{
	[System.Serializable]
	sealed class FeederSetup
	{
		[SerializeField]
		SnakeFood _foodPrefab = null;
		[SerializeField]
		int _capacity = 1;
		[SerializeField]
		float _spawnFoodDelay = 1f;

		public int capacity { get { return _capacity; } }

		public float spawnDelay { get { return _spawnFoodDelay; } }

		public SnakeFood foodPrefab { get { return _foodPrefab; } }
	}
}