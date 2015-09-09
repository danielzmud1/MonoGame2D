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

namespace MonoGame2D.Game
{
    public class HelperStruct
    {
        public bool isPlayerStartX;
        public bool isPlayerEndX;
        public bool isPlayerStartY;
        public bool isPlayerEndY;

        public bool isPlayerMoveRight;
        public bool isPlayerMoveLeft;
        public bool isPlayerMoveUp;
        public bool isPlayerMoveDown;

        public int NumberOfTilesWidth;
        public int NumberOfTilesHeight;

        public int NumberOfTilesInMapX;
        public int NumberOfTilesInMapY;
    }
}