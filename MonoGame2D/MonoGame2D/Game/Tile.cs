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
using Microsoft.Xna.Framework;

namespace MonoGame2D.Game
{

    enum TileCollision
    {
        // Tile use for move
        Tile = 0,

        cos = 1,
    }

    struct Tile
    {
        public Texture2D Texture;
        public TileCollision Collision;

        public const int Width = 80;
        public const int Height = 80;

        public static readonly Vector2 Size = new Vector2 (Width, Height);

        public Tile(Texture2D texture, TileCollision collision)
        {
            Texture = texture;
            Collision  = collision;
        }
    
    }
}