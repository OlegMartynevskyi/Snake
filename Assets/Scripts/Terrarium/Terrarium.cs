using UnityEngine;


namespace SnakeWorld
{
	sealed class Terrarium
	{
		public readonly Transform transform;
		public readonly Rect size;

		public Terrarium (Vector3 position, TerrariumSetup setup)
		{
			GameObject gameObject = new GameObject ("Terrarium");
            gameObject.layer = Snake.layer;
			transform = gameObject.transform;
			transform.position = position;
			Rect backgroundRect = CreateBackground (setup);
			size = CreateBorder (gameObject, backgroundRect);
		}

		Rect CreateBackground (TerrariumSetup setup)
		{
			Vector2 spriteSize = setup.background.bounds.size;
			Vector2 center = spriteSize * setup.size * 0.5f - spriteSize * 0.5f;
			for (int i = 0; i < setup.size; ++i) {
				for (int j = 0; j < setup.size; ++j) {
					GameObject backgroundPart = new GameObject ("Background_" + i + j);
					SpriteRenderer backgroundView = backgroundPart.AddComponent<SpriteRenderer> ();
					backgroundView.sprite = setup.background;
					backgroundView.sortingOrder = -10;
					backgroundPart.transform.SetParent (transform);
					Vector3 position = Vector3.zero;
					position.x = center.x - spriteSize.x * i;
					position.y = center.y - spriteSize.y * j;
					backgroundPart.transform.localPosition = position;
				}
			}
			Rect background = new Rect ();
			background.center = -spriteSize * setup.size * 0.5f;
			background.size = spriteSize * setup.size;
			return background;
		}

		Rect CreateBorder (GameObject gameObject, Rect backgroundRect)
		{
			Vector3 min = SnakeGame.mainCamera.ViewportToWorldPoint (Vector3.zero);
			Vector3 max = SnakeGame.mainCamera.ViewportToWorldPoint (Vector3.one);
			Vector2 cameraSize = max - min;
            Rect borderRect = new Rect(backgroundRect);
            borderRect.size -= cameraSize;
            borderRect.center += cameraSize * 0.5f;
            EdgeCollider2D border = gameObject.AddComponent<EdgeCollider2D>();
            border.isTrigger = true;
            border.points = new Vector2[] {borderRect.min, new Vector2(borderRect.xMin, borderRect.yMax), borderRect.max, new Vector2(borderRect.xMax, borderRect.yMin), borderRect.min };						
			return borderRect;
		}	
	}
}