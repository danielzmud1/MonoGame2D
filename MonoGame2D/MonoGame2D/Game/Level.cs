using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace MonoGame2D.Game
{
    class Level
    {
        public ContentManager Content
        {
            get { return _content; }
        }
        private ContentManager _content;
        private SpriteBatch _spriteBatch;
        private HelperStruct _helperStruct;
        private int _baseScreenWidth;
        private int _baseScreenHeight;
        private int _numberOfTilesWidth;
        private int _numberOfTilesHeight;

        private Tile[,] _tiles1;
        private Tile[,] _tiles2;
        private int _tilesHeight;
        private int _tilesWidth;
        private char _act;
        private int _level;
        private bool _inputPressedRelease = false;
        private bool _inputPressedReleasePre = false;
        private bool _isInputPressedRelease = false;
        //private bool _isInputPressedRelease = true;
        // TODO: _isInputPressedRelease = false;

        private Player _player;
        private int _playerPositionX = 160;
        private int _playerPositionY = 160;
        List<PlayerSteps> _listSteps = new List<PlayerSteps>();
        private int _posX;
        private int _posY;
        private int _posPreX;
        private int _posPreY;



        private int _inputX;
        private int _inputY;

        private Texture2D _availableTile;
        private Texture2D _unAvailableTile;
        private Texture2D _pathTile;
        private Texture2D _targetTile;


        public Level(IServiceProvider serviceProvider, SpriteBatch spriteBatch, Vector2 baseScreenSize)
        {
            _baseScreenWidth = (int)baseScreenSize.X;
            _baseScreenHeight = (int)baseScreenSize.Y;
            _numberOfTilesWidth = _baseScreenWidth / Tile.Width;
            _numberOfTilesHeight = _baseScreenHeight / Tile.Height;



            _helperStruct = new HelperStruct();
            _helperStruct.NumberOfTilesWidth = _numberOfTilesWidth;
            _helperStruct.NumberOfTilesHeight = _numberOfTilesHeight;
            _helperStruct.isPlayerMoveRight = false;
            _helperStruct.isPlayerMoveLeft = false;
            _helperStruct.isPlayerMoveUp = false;
            _helperStruct.isPlayerMoveDown = false;



            _content = new ContentManager(serviceProvider, "Content");
            _spriteBatch = spriteBatch;

            _availableTile = Content.Load<Texture2D>("Tiles/AvailableTile");
            _unAvailableTile = Content.Load<Texture2D>("Tiles/UnAvailableTile");
            _pathTile = Content.Load<Texture2D>("Tiles/PathTile");
            _targetTile = Content.Load<Texture2D>("Tiles/TargetTile");

            _player = new Player(serviceProvider, spriteBatch, baseScreenSize, _helperStruct);
        }

        public void LoadLevel(char act, int level, GameTime gameTime)
        {
            _act = act;
            _level = level;

            string levelPath;
            Stream fileStream;
            List<string> lines;
            // load textures
            // TO DO: check level and act must agree with this in level folder

            // first render, background
            levelPath = string.Format("Content/Levels/{0}{1}_level_1.txt", act, level);
            fileStream = TitleContainer.OpenStream(levelPath);
            lines = LoadTileFromTxt(fileStream);
            _tiles1 = new Tile[_tilesWidth, _tilesHeight];
            LoadTile(lines, _tiles1, new DelegateLoadTile(LoadTile1));

            // secound render
            levelPath = string.Format("Content/Levels/{0}{1}_level_2.txt", act, level);
            fileStream = TitleContainer.OpenStream(levelPath);
            lines = LoadTileFromTxt(fileStream);
            _tiles2 = new Tile[_tilesWidth, _tilesHeight];
            LoadTile(lines, _tiles2, new DelegateLoadTile(LoadTile2));

            _helperStruct.NumberOfTilesInMapX = _tilesWidth;
            _helperStruct.NumberOfTilesInMapY = _tilesHeight;

            Update(gameTime, 0, 0, false);


        }

        public void Update(GameTime gameTime, int x, int y, bool isInputPressed)
        {
            _inputX = x;
            _inputY = y;

            _player.Update(gameTime, _inputX, _inputY, _playerPositionX, _playerPositionY, isInputPressed);
        }

        #region [Load Tile]

        private List<string> LoadTileFromTxt(Stream fileStream)
        {
            int width;
            List<string> lines = new List<string>();
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The file with level dont have equal langth od line"));
                    line = reader.ReadLine();
                }

                _tilesWidth = width;
                _tilesHeight = lines.Count;

                return lines;
            }
        }

        private void LoadTile(List<string> lines, Tile[,] tiles, DelegateLoadTile LoadTile)
        {
            for (int y = 0; y < _tilesHeight; ++y)
                for (int x = 0; x < _tilesWidth; ++x)
                {
                    char tileType = lines[y][x];
                    tiles[x, y] = LoadTile(tileType, x, y);
                }
        }

        private delegate Tile DelegateLoadTile(char Type, int x, int y);

        private Tile LoadTile1(char tileType, int x, int y)
        {
            switch (tileType)
            {
                case '*':
                    return new Tile(null, TileCollision.cos);
                case '.':
                    return LoadTile("Block1", TileCollision.Tile);
                default:
                    throw new NotSupportedException(String.Format("Not Parrsed character in level.txt"));
            }
        }

        private Tile LoadTile2(char tileType, int x, int y)
        {
            switch (tileType)
            {
                case '*':
                    return new Tile(null, TileCollision.cos);
                case 'p':
                    _player.X = x * Tile.Width;
                    _player.Y = y * Tile.Width;
                    _player.PlayerPositionInMapX = (int)_player.X;
                    _player.PlayerPositionInMapY = (int)_player.Y;
                    _player.PlayerPositionInMapPreX = _player.PlayerPositionInMapX;
                    _player.PlayerPositionInMapPreY = _player.PlayerPositionInMapY;

                    //_posX = (int)_player.X;
                    //_posY = (int)_player.Y;
                    //_posPreX = _posX;
                    //_posPreY = _posY;


                    return new Tile(null, TileCollision.cos);
                //case 'x':
                //    return LoadTile("Exit1", TileCollision.Tile);

                case 'q':
                    return LoadTile("Rock1", TileCollision.Tile);
                case 'w':
                    return LoadTile("Rock2", TileCollision.Tile);
                case 'e':
                    return LoadTile("Busches1", TileCollision.Tile);
                case 'r':
                    return LoadTile("Wood1", TileCollision.Tile);

                case '6':
                    return LoadTile("WallRight1", TileCollision.Tile);
                case '4':
                    return LoadTile("WallLeft1", TileCollision.Tile);
                case '8':
                    return LoadTile("WallUp1", TileCollision.Tile);
                case '2':
                    return LoadTile("WallDown1", TileCollision.Tile);

                case '7':
                    return LoadTile("Wall1", TileCollision.Tile);
                case '9':
                    return LoadTile("Wall2", TileCollision.Tile);
                case '3':
                    return LoadTile("Wall3", TileCollision.Tile);
                case '1':
                    return LoadTile("Wall4", TileCollision.Tile);

                default:
                    throw new NotSupportedException(String.Format("Not Parrsed character in level.txt"));
            }
        }

        private Tile LoadTile(string name, TileCollision collision)
        {
            return new Tile(Content.Load<Texture2D>("Tiles/" + _act + "_" + name), collision);
        }

        #endregion

        public void Draw(int x, int y, bool isInputPressed, GameTime gameTime)
        {
            
            DrawTiles(_tiles1);
            DrawTiles(_tiles2);    

            _helperStruct.isPlayerMoveRight = false;
            _helperStruct.isPlayerMoveDown = false;


            // Change status pressed - use for release click
            _inputPressedReleasePre = _inputPressedRelease;
            _inputPressedRelease = isInputPressed;
            if (_inputPressedRelease != _inputPressedReleasePre)
                _isInputPressedRelease = !_isInputPressedRelease;



            if (_isInputPressedRelease && !_player.IsPlayerInMove)
            {
                DrawPath(x, y, isInputPressed, _player.NumberAvaliableSteps);
            }           
            else
                if (_listSteps.Count != 0)
                    _player.WalkAlongPath(_listSteps, _player.NumberAvaliableSteps);

            

           

            _player.Draw();

        }

        private void DrawTiles(Tile[,] tiles)
        {
            _helperStruct.isPlayerStartX = false;
            _helperStruct.isPlayerEndX = false;
            _helperStruct.isPlayerStartY = false;
            _helperStruct.isPlayerEndY = false;

            
            int startX = 0;
            int startY = 0;
            int endX = _numberOfTilesWidth;
            int endY = _numberOfTilesHeight;

            var tmpX = (int)_numberOfTilesWidth / 2;
            var tmpY = (int)_numberOfTilesHeight / 2;
       

            _posPreX = _posX;
            _posPreY = _posY;

            if (_player.IsAnimationContinue)
                _posX = _player.PlayerPositionInMapPreX / Tile.Width;
            else _posX = _player.PlayerPositionInMapX / Tile.Width;

            if (_player.IsAnimationContinue)
                _posY = _player.PlayerPositionInMapPreY / Tile.Height;
            else _posY = _player.PlayerPositionInMapY / Tile.Height;

            // player is not near on start and end axis X
            startX = _posX - tmpX;          
            // player is near 0 index in axis X
            if (_posX < tmpX)
            {
                startX = 0;
                _helperStruct.isPlayerStartX = true;
            }                
            // player is near end of map in axis X
            if (_tilesWidth - _posX < tmpX)
            {
                startX = _tilesWidth - _numberOfTilesWidth;
                _helperStruct.isPlayerEndX = true;
            }                
            
            // Axis Y
            // player is not near on start and end axis X
            startY = _posY - tmpY;
            // player is near 0 index in axis X
            if (_posY < tmpY)
            {
                startY = 0;
                _helperStruct.isPlayerStartY = true;
            }                
            // player is near end of map in axis X
            if (_tilesHeight - _posY < tmpY)
            {
                startY = _tilesHeight - _numberOfTilesHeight;
                _helperStruct.isPlayerEndY = true;
            }                
            
            endX = startX + _numberOfTilesWidth;
            endY = startY + _numberOfTilesHeight;
            for (int y = startY, i = 0; y < endY; ++y, ++i)
            {
                for (int x = startX, j = 0; x < endX; ++x, ++j)
                {
                    Texture2D texture = tiles[x, y].Texture;
                    if (texture != null)
                    {
                        Vector2 position = new Vector2(j, i) * Tile.Size;
                        _spriteBatch.Draw(texture, position, Color.White);
                    }
                }
            }


        }

        private void DrawPath(int x, int y, bool isInputPressed, int numberAvaliableSteps)
        {
            _listSteps.Clear();

            // TO DO: x index change to variable - numberAvaliableSteps
            int[,] recTab = new int[numberAvaliableSteps, 2];
            int recIndex = 0;

            var tileWidth = Tile.Width;
            var tileHeight = Tile.Height;

            int tileX = smallestMultiples(x, Tile.Width);
            int tileY = smallestMultiples(y, Tile.Height);

            var posX = (int)_player.X;
            var posY = (int)_player.Y;

            var distanceX = posX - tileX;
            distanceX = (distanceX > 0) ? distanceX : distanceX * -1;
            var distanceY = posY - tileY;
            distanceY = (distanceY > 0) ? distanceY : distanceY * -1;

            if (distanceX <= numberAvaliableSteps * tileWidth && distanceY <= numberAvaliableSteps * tileHeight)
            {
                while (((posX - tileX) != 0) || ((posY - tileY) != 0))
                {
                    recTab[recIndex, 0] = posX;
                    recTab[recIndex, 1] = posY;
                    recIndex++;

                    if ((posX - tileX < 0) && (posY - tileY < 0))
                    {
                        _listSteps.Add(PlayerSteps.CrossRightDown);
                        posX += tileWidth;
                        posY += tileHeight;
                        continue;
                    }

                    if ((posX - tileX > 0) && (posY - tileY < 0))
                    {
                        _listSteps.Add(PlayerSteps.CrossLeftDown);
                        posX -= tileWidth;
                        posY += tileHeight;
                        continue;
                    }

                    if ((posX - tileX > 0) && (posY - tileY > 0))
                    {
                        _listSteps.Add(PlayerSteps.CrossLeftUp);
                        posX -= tileWidth;
                        posY -= tileHeight;
                        continue;
                    }

                    if ((posX - tileX < 0) && (posY - tileY > 0))
                    {
                        _listSteps.Add(PlayerSteps.CrossRightUp);
                        posX += tileWidth;
                        posY -= tileHeight;
                        continue;
                    }

                    if (posX - tileX < 0)
                    {
                        _listSteps.Add(PlayerSteps.Right);
                        posX += tileWidth;
                        continue;
                    }

                    if (posX - tileX > 0)
                    {
                        _listSteps.Add(PlayerSteps.Left);
                        posX -= tileWidth;
                        continue;
                    }

                    if (posY - tileY < 0)
                    {
                        _listSteps.Add(PlayerSteps.Down);
                        posY += tileHeight;
                        continue;
                    }

                    if (posY - tileY > 0)
                    {
                        _listSteps.Add(PlayerSteps.Up);
                        posY -= tileHeight;
                        continue;
                    }
                }
                _listSteps.Add(PlayerSteps.None);

                for (int i = 0; i < recIndex; i++)
                    _spriteBatch.Draw(_pathTile, new Rectangle(recTab[i, 0], recTab[i, 1], tileWidth, tileHeight), Color.White);

                var rec = new Rectangle(tileX, tileY, Tile.Width, Tile.Height);
                _spriteBatch.Draw(_targetTile, rec, Color.White);

            }
        }

        private void DrawAvaliableTile(int x, int y, bool isInputPressed, int numberAvaliableSteps)
        { }

        #region [Helper functions]

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