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

        private Tile[,] _tiles;
        private Texture2D[] _layers;
        private char _act;
        private int _level;
        
        

        public int Width { get { return _tiles.GetLength(0); } }
        public int Height { get { return _tiles.GetLength(1); } }

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
            // load textures
            // TO DO: check level and act must agree with this in level folder
            string levelPath = string.Format("Content/Levels/{0}_level{1}.txt", act, level);
            Stream fileStream = TitleContainer.OpenStream(levelPath) ;
            LoadTiles(fileStream);
        
        }

        private void LoadTiles(Stream fileStream)
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

                _tiles = new Tile[width, lines.Count];
                for (int y = 0; y < Height; ++y)
                {
                    for (int x = 0; x < Width; ++x)
                    {
                        char tileType = lines[y][x];
                        _tiles[x, y] = LoadTile(tileType, x, y);
                    }
                }

            
            
            }
        }

        private Tile LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                case '.':
                    return LoadTile("Block1", TileCollision.Tile);

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
            DrawTiles();
        
        }

        private void DrawTiles()
        {
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    Texture2D texture = _tiles[x, y].Texture;
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