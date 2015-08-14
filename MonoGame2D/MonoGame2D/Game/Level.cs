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
        private int _baseScreenWidth;
        private int _baseScreenHeight;

        private Tile[,] _tiles1;
        private Tile[,] _tiles2;
        private int _tilesHeight;
        private int _tilesWidth;
        private Texture2D[] _layers;
        private char _act;
        private int _level;
        
        

        public int Width { get { return _tiles1.GetLength(0); } }
        public int Height { get { return _tiles1.GetLength(1); } }

        

        public Level(IServiceProvider serviceProvider, SpriteBatch spriteBatch, Vector2 baseScreenSize)
        {
            _baseScreenWidth = (int)baseScreenSize.X;
            _baseScreenHeight = (int)baseScreenSize.Y;
            _content = new ContentManager(serviceProvider, "Content");
            _spriteBatch = spriteBatch;
        }

        public void LoadLevel(char act, int level)
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
            fileStream = TitleContainer.OpenStream(levelPath) ;
            lines = LoadTileFromTxt(fileStream);
            _tiles1 = new Tile[_tilesWidth, _tilesHeight];
            LoadTile(lines, _tiles1, new DelegateLoadTile(LoadTile1));

            // secound render
            levelPath = string.Format("Content/Levels/{0}{1}_level_2.txt", act, level);
            fileStream = TitleContainer.OpenStream(levelPath);
            lines = LoadTileFromTxt(fileStream);
            _tiles2 = new Tile[_tilesWidth, _tilesHeight];
            LoadTile(lines, _tiles2, new DelegateLoadTile(LoadTile2));
        
        }

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
                case '1':
                    return LoadTile("Rock1", TileCollision.Tile);
                case '2':
                    return LoadTile("Rock2", TileCollision.Tile);
                case '3':
                    return LoadTile("Busches1", TileCollision.Tile);
                case '4':
                    return LoadTile("Wood1", TileCollision.Tile);
                default:
                    throw new NotSupportedException(String.Format("Not Parrsed character in level.txt"));
            }
        }

        

        private Tile LoadTile(string name, TileCollision collision)
        {
            return new Tile(Content.Load<Texture2D>("Tiles/" + _act + "_" + name), collision);
        }




        public void Draw()
        {
            DrawTiles(_tiles1);
            DrawTiles(_tiles2);
        
        }

        private void DrawTiles(Tile[,] tiles)
        {
            for (int y = 0; y < _tilesHeight; ++y)
            {
                for (int x = 0; x < _tilesWidth; ++x)
                {
                    Texture2D texture = tiles[x, y].Texture;
                    if (texture != null)
                    {
                        Vector2 position = new Vector2(x, y) * Tile.Size;
                        _spriteBatch.Draw(texture, position, Color.White);
                    }
                }
            }
        }






    }
}