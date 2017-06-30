using UnityEngine;


namespace SnakeWorld
{
	abstract class SnakePart
	{
		public readonly SpriteRenderer view;

		public Vector3 lastPosition { get; protected set; }

		public Transform transform { get { return view != null ? view.transform : null; } }

		protected SnakePart (Transform root, Sprite sprite)
		{
			GameObject obj = new GameObject ();
			obj.layer = root.gameObject.layer;
			obj.transform.SetParent (root);
			obj.transform.localPosition = Vector3.zero;
			view = obj.AddComponent<SpriteRenderer> ();
			view.sprite = sprite;
		}

		public abstract void Move(Vector3 translation);
	}
} 