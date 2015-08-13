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
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace MonoGame2D.Game
{
    class Menu
    {
        
        
        public ContentManager Content
        {
            get { return _content; }
        }
        private ContentManager _content;

        private SpriteBatch _spriteBatch;
        private int _baseScreenWidth;
        private int _baseScreenHeight;

        // Textures
        private Texture2D _background;
        private Texture2D _buttonPlay;
        private Texture2D _buttonOptions;

        // Rectangle
        private Rectangle _recBackground;
        private Rectangle _recPlay;
        private Rectangle _recOptions;



        public Menu(IServiceProvider serviceProvider, SpriteBatch spriteBatch, Vector2 baseScreenSize)
        {
            _content = new ContentManager(serviceProvider, "Content");
            _spriteBatch = spriteBatch;
            _baseScreenWidth  = (int)baseScreenSize.X;
            _baseScreenHeight = (int)baseScreenSize.Y;

            _recBackground = new Rectangle(0, 0, _baseScreenWidth, _baseScreenHeight);
            _recPlay = new Rectangle(400, 100, 200, 100);
            _recOptions = new Rectangle(400, 300, 200, 100);

            LoadTextures();
        }

        public GameState HandleInput(int x, int y, bool isInputPressed)
        {
            if (isInputPressed)
            {
                if (_recPlay.Contains(x, y))
                {
                    OnClick();
                    return GameState.Game;
                }  
                if (_recOptions.Contains(x, y))
                {
                    OnClick();
                    return GameState.Options;
                }
            }
            return GameState.Menu;
        }

        private void OnClick()
        {
            System.Diagnostics.Debug.WriteLine("_________________________");
        }

        private void LoadTextures()
        {
            _background = Content.Load<Texture2D>("Menu/Background");
            _buttonPlay = Content.Load<Texture2D>("Menu/ButtonPlay");
            _buttonOptions = Content.Load<Texture2D>("Menu/ButtonOptions");
        }

        public void Draw() 
        { 
            _spriteBatch.Draw(_background, _recBackground, Color.White);
            _spriteBatch.Draw(_buttonPlay, _recPlay, Color.White);
            _spriteBatch.Draw(_buttonOptions, _recOptions, Color.White);
        }

 

    }
}