using UnityEngine;
using System;
using UnityRandom = UnityEngine.Random;

namespace SnakeWorld
{
	[RequireComponent (typeof(CircleCollider2D), typeof(SpriteRenderer))]
	sealed class SnakeFood : MonoBehaviour
	{
		SpriteRenderer _view;

		static int _layer;

		public static int layer {
			get {
				if (_layer == 0) {
					_layer = LayerMask.NameToLayer ("Food");
				}
				return _layer;
			}
		}

        public float fatness { get { return transform.localScale.x * 10; } }

        public Color color { get { return _view.color; } }

        public static event Action EatUp;

		public static event EventHandler<FoodArgs> EatMe;

		void Start ()
		{            
            gameObject.layer = layer;
			transform.localScale = Vector3.one * (1 + UnityRandom.value);
			_view = GetComponent<SpriteRenderer> ();
			_view.color = SnakeGame.GetRandomColor ();
			CircleCollider2D collider2d = GetComponent<CircleCollider2D> ();
			collider2d.isTrigger = true;
			collider2d.radius = _view.sprite.bounds.extents.x;
		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.gameObject.layer == Snake.layer) {
				if (EatMe != null) {
					EatMe (this, new FoodArgs (other));
				}
			}
		}

		void OnDisable ()
		{
			if (EatUp != null) {
				EatUp ();
			}
		}
	}

	sealed class FoodArgs : EventArgs
	{
		public Collider2D snakeHeadCollider { get; private set; }

		public FoodArgs (Collider2D snakeHead)
		{			
			snakeHeadCollider = snakeHead;
		}
	}
}
