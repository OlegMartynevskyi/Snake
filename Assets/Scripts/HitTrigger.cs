using UnityEngine;


namespace SnakeWorld
{
	sealed class HitTrigger : MonoBehaviour
	{
		public System.Action<Collider2D> onHit = null;

		void OnTriggerEnter2D (Collider2D other)
		{
			if (onHit != null) {
				onHit (other);
			}
		}
	}
}