using UnityEngine;


namespace SnakeWorld
{
	[System.Serializable]
	sealed class SnakeSetup
	{
		[SerializeField]	
		Sprite _bodySprite = null;
		[SerializeField, Range (1, 10000)]
		int _bodyLenght = 100;
		[SerializeField]
		float _moveSpeed = 0.5f;
		[SerializeField]
		float _rotationSpeed = 250.0f;
		[SerializeField]
		float _acceleration = 2.0f;
		[SerializeField]
		int _growSpeed = 1;
		[SerializeField]
		int _shrinkSpeed = 10;

		public Sprite bodySprite { get { return _bodySprite; } }

		public int bodyLenght { get { return _bodyLenght; } }

		public float moveSpeed { get { return _moveSpeed; } }

		public float rotationSpeed { get { return _rotationSpeed; } }

		public float acceleration { get { return _acceleration; } }

		public int growSpeed { get { return _growSpeed; } }

		public int shrinkSpeed { get { return _shrinkSpeed; } }
	}
}