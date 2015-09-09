using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;

namespace MonoGame2D.Game
{
    enum PlayerSteps
    { 
        Right,
        Left,
        Up,
        Down,
        CrossRightUp,
        CrossRightDown,
        CrossLeftUp,
        CrossLeftDown,
        None,
    }

	public class Player
	{
        private Texture2D _characterTexture;
        private ContentManager _content;
        private SpriteBatch  _spriteBatch;
        private int _baseScreenWidth;
        private int _baseScreenHeight;
        private GameTime _gameTime;
        private HelperStruct _helperStruct;

        private Animation _walkDown;
        private Animation _walkUp;
        private Animation _walkLeft;
        private Animation _walkRight;

        private Animation _standDown;
        private Animation _standUp;
        private Animation _standLeft;
        private Animation _standRight;

        private Animation _currentAnimation;
        public bool IsAnimationContinue { get { return _isAnimationContinue; } }
        private bool _isAnimationContinue = false;

        public const int Width = 80;
        public const int Height = 80;
        public int PositionX { get { return _positionX; } }
        private int _positionX;
        public int PositionY { get { return _positionY; } }
        private int _positionY;

        private int _positionDestinationX;
        private int _positionDestinationY;

        public bool IsPlayerInMove { get { return _isPlayerInMove; } set { _isPlayerInMove = value; } }
        private bool _isPlayerInMove = false;

        private int _stepIndex = 0;
        public int NumberAvaliableSteps { get { return _numberAvaliableSteps;  } }
        private int _numberAvaliableSteps = 3;

		public float X
		{
			get;
			set;
		}

		public float Y
		{
			get;
			set;
		}

        public int PlayerPositionInMapX { get { return _playerPositionInMapX; } set { _playerPositionInMapX = value; } }
        private int _playerPositionInMapX;
        public int PlayerPositionInMapY { get { return _playerPositionInMapY; } set { _playerPositionInMapY = value; } }
        private int _playerPositionInMapY;
        public int PlayerPositionInMapPreX { get { return _playerPositionInMapPreX; } set { _playerPositionInMapPreX = value; } }
        private int _playerPositionInMapPreX;
        public int PlayerPositionInMapPreY { get { return _playerPositionInMapPreY; } set { _playerPositionInMapPreY = value; } }
        private int _playerPositionInMapPreY;

        public Player(IServiceProvider serviceProvider, SpriteBatch spriteBatch, Vector2 baseScreenSize, HelperStruct helperStruct)
        {
            _content = new ContentManager(serviceProvider, "Content");
            _spriteBatch = spriteBatch;
            _baseScreenWidth = (int)baseScreenSize.X;
            _baseScreenHeight = (int)baseScreenSize.Y;

            _helperStruct = helperStruct;
  
			if (_characterTexture == null)
                _characterTexture = _content.Load<Texture2D>("Sprites/Player/Player");

            _walkDown = new Animation();
            _walkDown.AddFrame(new Rectangle(0,   160, 80, 80), TimeSpan.FromSeconds(.25));
            _walkDown.AddFrame(new Rectangle(80,  160, 80, 80), TimeSpan.FromSeconds(.25));
            _walkDown.AddFrame(new Rectangle(160, 160, 80, 80), TimeSpan.FromSeconds(.25));
            _walkDown.AddFrame(new Rectangle(240, 160, 80, 80), TimeSpan.FromSeconds(.25));
            _walkDown.AddFrame(new Rectangle(320, 160, 80, 80), TimeSpan.FromSeconds(.25));
            _walkDown.AddFrame(new Rectangle(400, 160, 80, 80), TimeSpan.FromSeconds(.25));
            _walkDown.AddFrame(new Rectangle(480, 160, 80, 80), TimeSpan.FromSeconds(.25));
            _walkDown.AddFrame(new Rectangle(560, 160, 80, 80), TimeSpan.FromSeconds(.25));
            _walkDown.AddFrame(new Rectangle(640, 160, 80, 80), TimeSpan.FromSeconds(.25));

            _walkUp = new Animation();
            _walkUp.AddFrame(new Rectangle(0, 0, 80, 80), TimeSpan.FromSeconds(.25));
            _walkUp.AddFrame(new Rectangle(80, 0, 80, 80), TimeSpan.FromSeconds(.25));
            _walkUp.AddFrame(new Rectangle(160, 0, 80, 80), TimeSpan.FromSeconds(.25));
            _walkUp.AddFrame(new Rectangle(240, 0, 80, 80), TimeSpan.FromSeconds(.25));
            _walkUp.AddFrame(new Rectangle(320, 0, 80, 80), TimeSpan.FromSeconds(.25));
            _walkUp.AddFrame(new Rectangle(400, 0, 80, 80), TimeSpan.FromSeconds(.25));
            _walkUp.AddFrame(new Rectangle(480, 0, 80, 80), TimeSpan.FromSeconds(.25));
            _walkUp.AddFrame(new Rectangle(560, 0, 80, 80), TimeSpan.FromSeconds(.25));
            _walkUp.AddFrame(new Rectangle(640, 0, 80, 80), TimeSpan.FromSeconds(.25));

            _walkLeft = new Animation();
            _walkLeft.AddFrame(new Rectangle(0,   80, 80, 80), TimeSpan.FromSeconds(.25));
            _walkLeft.AddFrame(new Rectangle(80,  80, 80, 80), TimeSpan.FromSeconds(.25));
            _walkLeft.AddFrame(new Rectangle(160, 80, 80, 80), TimeSpan.FromSeconds(.25));
            _walkLeft.AddFrame(new Rectangle(240, 80, 80, 80), TimeSpan.FromSeconds(.25));
            _walkLeft.AddFrame(new Rectangle(320, 80, 80, 80), TimeSpan.FromSeconds(.25));
            _walkLeft.AddFrame(new Rectangle(400, 80, 80, 80), TimeSpan.FromSeconds(.25));
            _walkLeft.AddFrame(new Rectangle(480, 80, 80, 80), TimeSpan.FromSeconds(.25));
            _walkLeft.AddFrame(new Rectangle(560, 80, 80, 80), TimeSpan.FromSeconds(.25));
            _walkLeft.AddFrame(new Rectangle(640, 80, 80, 80), TimeSpan.FromSeconds(.25));

            _walkRight = new Animation();
            _walkRight.AddFrame(new Rectangle(0,   240, 80, 80), TimeSpan.FromSeconds(.25));
            _walkRight.AddFrame(new Rectangle(80,  240, 80, 80), TimeSpan.FromSeconds(.25));
            _walkRight.AddFrame(new Rectangle(160, 240, 80, 80), TimeSpan.FromSeconds(.25));
            _walkRight.AddFrame(new Rectangle(240, 240, 80, 80), TimeSpan.FromSeconds(.25));
            _walkRight.AddFrame(new Rectangle(320, 240, 80, 80), TimeSpan.FromSeconds(.25));
            _walkRight.AddFrame(new Rectangle(400, 240, 80, 80), TimeSpan.FromSeconds(.25));
            _walkRight.AddFrame(new Rectangle(480, 240, 80, 80), TimeSpan.FromSeconds(.25));
            _walkRight.AddFrame(new Rectangle(560, 240, 80, 80), TimeSpan.FromSeconds(.25));
            _walkRight.AddFrame(new Rectangle(640, 240, 80, 80), TimeSpan.FromSeconds(.25));

			// Standing animations only have a single frame of animation:
			_standDown = new Animation ();
            _standDown.AddFrame(new Rectangle(0, 160, 80, 80), TimeSpan.FromSeconds(.25));

			_standUp = new Animation ();
            _standUp.AddFrame(new Rectangle(0, 0, 80, 80), TimeSpan.FromSeconds(.25));

			_standLeft = new Animation ();
            _standLeft.AddFrame(new Rectangle(0, 80, 80, 80), TimeSpan.FromSeconds(.25));

			_standRight = new Animation ();
            _standRight.AddFrame(new Rectangle(0, 240, 80, 80), TimeSpan.FromSeconds(.25));

            _currentAnimation = new Animation();
            _currentAnimation = _walkRight;
		}

		public void Draw()
		{
			Vector2 topLeftOfSprite = new Vector2 (this.X, this.Y);
			Color tintColor = Color.White;
			var sourceRectangle = _currentAnimation.CurrentRectangle;

			_spriteBatch.Draw(_characterTexture, topLeftOfSprite, sourceRectangle, Color.White);
		}

		public void Update(GameTime gameTime, int x, int y, int positionX, int positionY, bool isInputPressed)
		{
            _positionX = positionX;
            _positionY = positionY;
            _gameTime = gameTime;
		}

        Vector2 GetDesiredVelocityFromInput(int x, int y)
        {
            Vector2 desiredVelocity = new Vector2();

            var howCloseDestinationTileX = _positionX - _positionDestinationX;
            howCloseDestinationTileX = (howCloseDestinationTileX > 0) ? howCloseDestinationTileX : howCloseDestinationTileX * -1;
            var howCloseDestinationTileY = _positionY - _positionDestinationY;
            howCloseDestinationTileY = (howCloseDestinationTileY > 0) ? howCloseDestinationTileY : howCloseDestinationTileY * -1;

            desiredVelocity.X = x - this.X;
            desiredVelocity.Y = y - this.Y;

            if (desiredVelocity.X != 0 || desiredVelocity.Y != 0)
            {
                desiredVelocity.Normalize();
                Vector2 desiredSpeedClose;
                desiredSpeedClose.X = (8 * howCloseDestinationTileX);
                desiredSpeedClose.Y = (8 * howCloseDestinationTileY);
                desiredVelocity *= desiredSpeedClose;
            }

            return desiredVelocity;
        }

        internal void WalkAlongPath(System.Collections.Generic.List<PlayerSteps> listSteps, int _numberAvaliableSteps)
        {
            //int aa = 0;
            //int bb = 0;

            _isPlayerInMove = true;
            _positionX = (int)Math.Floor((double)X);
            _positionY = (int)Math.Floor((double)Y);

            var howCloseDestinationTileX = _positionX - _positionDestinationX;
            howCloseDestinationTileX = (howCloseDestinationTileX > 0) ? howCloseDestinationTileX : howCloseDestinationTileX * -1;
            var howCloseDestinationTileY = _positionY - _positionDestinationY;
            howCloseDestinationTileY = (howCloseDestinationTileY > 0) ? howCloseDestinationTileY : howCloseDestinationTileY * -1;

            if (!_isAnimationContinue && _stepIndex <= (_numberAvaliableSteps))
            {
                _isAnimationContinue = true;
                var a = listSteps[_stepIndex];
                switch (a)
                {
                    case PlayerSteps.Right:
                        _currentAnimation = _walkRight;
                        _positionDestinationX = _positionX + Tile.Width;
                        _positionDestinationY = _positionY;
                        _playerPositionInMapX += Tile.Width;
                        //aa += Tile.Width;
                        break;
                    case PlayerSteps.Left:
                        _currentAnimation = _walkLeft;
                        _positionDestinationX = _positionX - Tile.Width;
                        _positionDestinationY = _positionY;
                        _playerPositionInMapX -= Tile.Width;
                        //aa -= Tile.Width;
                        break;
                    case PlayerSteps.Up:
                        _currentAnimation = _walkUp;
                        _positionDestinationX = _positionX;
                        _positionDestinationY = _positionY - Tile.Width;
                        _playerPositionInMapY -= Tile.Height;
                       // bb -= Tile.Height;
                        break;
                    case PlayerSteps.Down:
                        _currentAnimation = _walkDown;
                        _positionDestinationX = _positionX;
                        _positionDestinationY = _positionY + Tile.Width;
                        _playerPositionInMapY += Tile.Height;
                        //bb += Tile.Height;
                        break;
                    case PlayerSteps.CrossRightUp:
                        _currentAnimation = _walkRight;
                        _positionDestinationX = _positionX + Tile.Width;
                        _positionDestinationY = _positionY - Tile.Height;
                        _playerPositionInMapX += Tile.Width;
                        _playerPositionInMapY -= Tile.Height;
                        //aa += Tile.Width;
                       // bb -= Tile.Height;
                        break;
                    case PlayerSteps.CrossRightDown:
                        _currentAnimation = _walkRight;
                        _positionDestinationX = _positionX + Tile.Width;
                        _positionDestinationY = _positionY + Tile.Width;
                        _playerPositionInMapX += Tile.Width;
                        _playerPositionInMapY += Tile.Height;
                       // aa += Tile.Width;
                       // bb += Tile.Height;
                        break;
                    case PlayerSteps.CrossLeftUp:
                        _currentAnimation = _walkLeft;
                        _positionDestinationX = _positionX - Tile.Width;
                        _positionDestinationY = _positionY - Tile.Width;
                        _playerPositionInMapX -= Tile.Width;
                        _playerPositionInMapY -= Tile.Height;
                       // aa -= Tile.Width;
                       // bb -= Tile.Height;
                        break;
                    case PlayerSteps.CrossLeftDown:
                        _currentAnimation = _walkLeft;
                        _positionDestinationX = _positionX - Tile.Width;
                        _positionDestinationY = _positionY + Tile.Height;
                        _playerPositionInMapX -= Tile.Width;
                        _playerPositionInMapY += Tile.Height;
                        //aa -= Tile.Width;
                       // bb += Tile.Height;
                        break;
                    case PlayerSteps.None:
                        _currentAnimation = _standDown;
                        _isPlayerInMove = false;
                        _isAnimationContinue = false;
                        break;
                    default:
                        break;
                }
            }
            else 
                if (howCloseDestinationTileX < 3 && howCloseDestinationTileY < 3)
                {
                    X = roundToTileSize((int)X, Tile.Width, 5);
                    Y = roundToTileSize((int)Y, Tile.Width, 5);

                    if (!_helperStruct.isPlayerStartX && !_helperStruct.isPlayerEndX)
                    {
                        X = (_helperStruct.NumberOfTilesWidth / 2) * Tile.Width;
                        _positionDestinationX = (int)X;
                        //_helperStruct.isPlayerMoveRight = true;
                    }

                    if (!_helperStruct.isPlayerStartY && !_helperStruct.isPlayerEndY)
                    {
                        Y = (_helperStruct.NumberOfTilesHeight / 2) * Tile.Height;
                        _positionDestinationY = (int)Y;
                        //_helperStruct.isPlayerMoveDown = true;
                    }

                    _playerPositionInMapPreX = _playerPositionInMapX;
                    _playerPositionInMapPreY = _playerPositionInMapY;

                    Draw();

                    _stepIndex++;
                    _isAnimationContinue = false;
                }

            if (_isPlayerInMove)
            {

                //_playerPositionInMapX += aa;
                //_playerPositionInMapY += bb;

                WalkTo(_gameTime, _positionDestinationX, _positionDestinationY);
            }
            else
            {
                _stepIndex = 0;
                listSteps.Clear();
            }

        }

        private void WalkTo(GameTime gameTime, int x, int y)
        {
            var velocity = GetDesiredVelocityFromInput(x, y);

            this.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _currentAnimation.Update(gameTime);
        }

        #region [Helper functions]

        private int roundToTileSize(int number, int tileSize, int range)
        {
            var helper = smallestMultiples(number, tileSize);
            var tmp = number - helper;

            if (tmp >= 0 && tmp <= range)
                return helper;

            return helper + tileSize;
        }

        private int smallestMultiples(int number, int multiple)
        {
            int helper = 0;
            for (; helper <= number; helper += multiple) ;
            helper = helper - multiple;
            return helper;
        }

        #endregion


    }
}