using UnityEngine;


namespace SnakeWorld
{
	[System.Serializable]
	sealed class TerrariumSetup
	{
		[SerializeField]
		Sprite _background = null;
		[SerializeField]
		int _size = 1;

		public Sprite background { get { return _background; } }

		public int size { get { return _size; } }
	}
}