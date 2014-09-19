using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetalLib;
using MetalLib.GameStructure;
using MetalLib.GameWorld;
using MetalLib.Pencil.Gaming;

using Pencil.Gaming;
using Pencil.Gaming.Graphics;
using Pencil.Gaming.MathUtils;
using Pencil.Gaming.Audio;

namespace MiniLD50
{
    class LoadState : GameState
    {
        private Sprite ratingScreen;
        private float alpha = 1f;
        private bool begin = false;

        public LoadState()
        {
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/states/rating.png", "rating");
            ratingScreen = new Sprite("rating", new Vector2(640 / 2, 480 / 2));
        }

        public override void Update(GameLoop gameLoop)
        {
            if (begin)
            {
                alpha += 0.0005f;
                if (alpha >= 1)
                {
                    gameLoop.ActiveGameState = new MenuState();
                }
            }
            else
            {
                alpha -= 0.0005f;
                begin = (alpha <= 0);
            }
        }

        public override void Draw()
        {
            ratingScreen.Draw();
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Begin(BeginMode.Quads);

            GL.Color4(0, 0, 0, alpha);
            

            GL.Vertex2(0, 0);
            GL.Vertex2(640, 0);
            GL.Vertex2(640, 480);
            GL.Vertex2(0, 480);

            GL.End();
            GL.Disable(EnableCap.Blend);

        }
    }
}
