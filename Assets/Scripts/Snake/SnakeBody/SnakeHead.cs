using UnityEngine;


namespace SnakeWorld
{
	sealed class SnakeHead : SnakePart {

		readonly Rigidbody2D rigidbody2D;
		public readonly CircleCollider2D collider;
		public readonly HitTrigger hitTrigger;

		public SnakeHead(Transform root, Sprite sprite)
			:base(root, sprite)
		{
			transform.gameObject.name = "Head";
			collider =  transform.gameObject.AddComponent<CircleCollider2D> ();
			rigidbody2D = transform.gameObject.AddComponent<Rigidbody2D> ();
			rigidbody2D.gravityScale = 0;
			hitTrigger = transform.gameObject.AddComponent<HitTrigger> ();
			view.color = SnakeGame.GetRandomColor ();
		}

		public override void Move (Vector3 translation)
		{
			lastPosition = transform.position;
			rigidbody2D.MovePosition (rigidbody2D.position + (Vector2)translation);
		}

		public void Rotate(float rotation)
		{
			rigidbody2D.MoveRotation (rigidbody2D.rotation + rotation);
		}
	}	
}