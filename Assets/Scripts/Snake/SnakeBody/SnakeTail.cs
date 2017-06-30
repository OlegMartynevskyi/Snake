using UnityEngine;


namespace SnakeWorld
{
	sealed class SnakeTail : SnakePart {

		public SnakeTail(Transform root, Sprite sprite)
			:base(root, sprite)
		{
			transform.gameObject.name = "Tail";
		}

		public override void Move (Vector3 translation)
		{
			lastPosition = view.transform.position;
			view.transform.position += translation;
		}
	}
}