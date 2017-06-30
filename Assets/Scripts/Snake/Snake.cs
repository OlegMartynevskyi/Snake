using UnityEngine;
using System.Collections.Generic;

namespace SnakeWorld
{
	sealed class Snake
	{
		public Transform transform;
		public readonly SnakeHead head;

		readonly SnakeSetup settings;
		readonly LinkedList<SnakePart> body;

		float movementSpeed;
		float rotationSpeed;
		float maxMomementSpeed;
		float rotationRatio;
		float tailLenght;

		SnakePart tail { get { return body.Last != null ? body.Last.Value : null; } }

		static int _layer;

		public static int layer {
			get {
				if (_layer == 0) {
					_layer = LayerMask.NameToLayer ("Snake");
				}
				return _layer;
			}
		}

		public System.Action onDie;

		public Snake (SnakeSetup setup)
		{	
			GameObject gameObject = new GameObject ("Snake");
			gameObject.layer = layer;
			transform = gameObject.transform;
			body = new LinkedList<SnakePart> ();
			head = new SnakeHead (transform, setup.bodySprite);
			head.hitTrigger.onHit = OnHeadImpact;
			body.AddFirst (head);
			for (int i = 1; i < setup.bodyLenght; ++i) {
				SnakeTail snakeTail = new SnakeTail (transform, setup.bodySprite);
				snakeTail.view.color = head.view.color;
				body.AddLast (snakeTail);
			}
			UpdateRenderOrder ();
			movementSpeed = setup.bodySprite.bounds.size.x;
			maxMomementSpeed = setup.moveSpeed * 2;
			rotationRatio = setup.rotationSpeed / movementSpeed;
			tailLenght = setup.bodyLenght;
			settings = setup;
		}

		public void Move ()
		{
			if (body.Count > 0) {				
				RotateHead (Input.GetAxis ("Horizontal"));
				MoveBody ();
				if (Input.GetButton ("Jump")) {
					Acceleration ();
				} else if (movementSpeed < settings.moveSpeed) {
					SpeedUp ();
				} else if (movementSpeed > settings.moveSpeed) {
					SlowDown ();
				}
			}
		}

		public void EatFood (object sender, FoodArgs foodData)
		{
			if (foodData.snakeHeadCollider.Equals (head.collider)) {
				SnakeFood food = (SnakeFood)sender;
				if (head.view.color == food.color) {					
					tailLenght += food.fatness;
				} else {					
					tailLenght -= food.fatness;
					head.view.color = food.color;
				}
				if (Mathf.CeilToInt (tailLenght) > body.Count) {					
					Grow (settings.growSpeed);
				} else {
					Shrink (settings.shrinkSpeed);
				}
				Object.Destroy (food.gameObject);
			}
		}

		void UpdateRenderOrder ()
		{
			int sortingOrder = 0;
			var bodyPart = body.Last;
			if (bodyPart != null) {
				bodyPart.Value.view.sortingOrder = sortingOrder;
				while (bodyPart.Previous != null) {				
					++sortingOrder;
					bodyPart.Previous.Value.view.sortingOrder = sortingOrder;
					bodyPart = bodyPart.Previous;
				}
			}
		}

		void RotateHead (float rotation)
		{			
			rotationSpeed = Mathf.Lerp (settings.rotationSpeed, movementSpeed * rotationRatio, Time.fixedDeltaTime);
			head.Rotate (rotation * rotationSpeed * Time.fixedDeltaTime);
		}

		void MoveBody ()
		{
			head.Move (head.transform.right * movementSpeed * Time.fixedDeltaTime);
			LinkedListNode<SnakePart> bodyPart = body.First.Next;
			while (bodyPart != null) {
				Vector3 direction = bodyPart.Previous.Value.lastPosition - bodyPart.Value.transform.position;
				bodyPart.Value.Move (direction);
				bodyPart = bodyPart.Next;
			}
		}

		void Acceleration ()
		{
			movementSpeed += Time.fixedDeltaTime * settings.acceleration;
			if (movementSpeed > maxMomementSpeed) {
				movementSpeed = maxMomementSpeed;
			}
		}

		void SpeedUp ()
		{
			movementSpeed += Time.fixedDeltaTime;
			if (movementSpeed > settings.moveSpeed) {
				movementSpeed = settings.moveSpeed;
			}
		}

		void SlowDown ()
		{
			movementSpeed -= Time.fixedDeltaTime * settings.acceleration;
			if (movementSpeed < settings.moveSpeed) {
				movementSpeed = settings.moveSpeed;
			}
		}

		void Grow (int size)
		{
			for (int i = 0; i < size; ++i) {
				SnakeTail snakeTail = new SnakeTail (transform, settings.bodySprite);
				snakeTail.transform.position = tail.transform.position;
				snakeTail.view.color = tail.view.color;
				body.AddLast (snakeTail);
			}
			UpdateRenderOrder ();
		}

		void Shrink (int size)
		{
			for (int i = 0; i < size; ++i) {
				if (body.Count > 1) {					
					Object.Destroy (tail.transform.gameObject);
					body.RemoveLast ();
				} else {
					KillSelf ();
					break;
				}
			}
		}

		void OnHeadImpact (Collider2D other)
		{
			if (other.gameObject.layer == layer) {				
				KillSelf ();
			}
		}

		void KillSelf ()
		{
			Object.Destroy (transform.gameObject);
			if (onDie != null) {
				onDie ();
			}				
		}
	}
}