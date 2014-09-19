using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetalLib;
using MetalLib.GameStructure;
using MetalLib.GameWorld;
using MetalLib.Pencil.Gaming;

using Pencil.Gaming;
using Pencil.Gaming.Audio;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using System.Threading;

namespace MiniLD50
{
    class Program
    {
        public static GameLoop MainGameLoop;
        public static double TotalTime = 0.0;
        public static int ScreenWidth = 640, ScreenHeight = 480;
        public static Sound Music;
        public static Sound MenuMusic;



        static void Main(string[] args)
        {
            Window2D window = new Window2D(ScreenWidth, ScreenHeight, true, "Pew Pew - VikingStein 2.5D - By Metaldemon", ScreenWidth, ScreenHeight);
            MainGameLoop = new GameLoop(new LoadState());

            

            Glfw.SwapInterval(false);

            new Thread(_LoadMusic).Start();
            MainGameLoop.Start(window);

            ContentManager.DisposeAll();
        }

        private static void _LoadMusic()
        {
            MenuMusic = new Sound(GameUtils.GetAppPath() + "/content/audio/menu2.ogg");
            Music = new Sound(GameUtils.GetAppPath() + "/content/audio/music2.ogg");
            Music.Looping = true;
            MenuMusic.Looping = true;
        }
    }
}
