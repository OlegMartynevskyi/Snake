using UnityEngine;


namespace SnakeWorld
{
	[RequireComponent (typeof(Camera))]
	public class SnakeGame : MonoBehaviour
	{
		[SerializeField]
		SnakeSetup _snakeSettings = null;
		[SerializeField]
		TerrariumSetup _terrariumSettings = null;
		[SerializeField]
		FeederSetup _feederSettings = null;
		[SerializeField]
		int _colorsCount = 20;

		Snake _snake;
		Feeder _feeder;		        
		Vector3 _cameraOffset;        

        private static Color[] _gameColors;

        public static Camera mainCamera { get; private set; }

        void Awake ()
		{			
            mainCamera = GetComponent<Camera> ();
            GenerateColors();
        }

		void Start ()
		{
            Terrarium terrarium = new Terrarium (mainCamera.transform.forward, _terrariumSettings);
            _feeder = new Feeder(terrarium, _feederSettings);
			_snake = new Snake (_snakeSettings);

            MoveCameraToSnake();
			SnakeFood.EatMe += _snake.EatFood;
			SnakeFood.EatUp += _feeder.UpdateFoodCount;
			_snake.onDie = GameOver;
            _feeder.StartMakeFood(this);
		}

        void MoveCameraToSnake()
        {
            mainCamera.transform.position = new Vector3(_snake.transform.position.x, _snake.transform.position.y, mainCamera.transform.position.z);
            _cameraOffset = _snake.transform.position - mainCamera.transform.position;
        }

		void FixedUpdate ()
		{
			if (_snake != null)
				_snake.Move ();
		}

		void LateUpdate ()
		{
			if (_snake != null) {
                mainCamera.transform.position = _snake.head.transform.position - _cameraOffset;
			}
		}

		void GameOver ()
		{
			SnakeFood.EatMe -= _snake.EatFood;
			SnakeFood.EatUp -= _feeder.UpdateFoodCount;
            _feeder.StopMakeFood();
			_snake = null;
		}

		void GenerateColors ()
		{
			_gameColors = new Color[_colorsCount];
			for (int i = 0; i < _colorsCount; ++i) {
				_gameColors [i] = Random.ColorHSV (0f, 1f, 1f, 1f, 0.5f, 1f);
			}
		}

		public static Color GetRandomColor ()
		{
			return _gameColors [Random.Range (0, _gameColors.Length)];
		}
	}
}