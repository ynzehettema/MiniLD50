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
    class EndState : GameState
    {
        private GameState parent;
        public EndState(GameState parent)
        {
            this.parent = parent;
        }

        public override void Update(GameLoop gameLoop)
        {
            for (int x = 0; x < 640; x++)
            {
                for (int y = 0; y < 480 - 72; y++)
                {
                    ScreenBuffer.screenBuffer[y * 640 + x] = ScreenBuffer.GetUintFromARGB(255, 120, 120, 120);
                }
            }
            if (Input.GetState(0).Keyboard[Key.Enter] && !Input.GetState(1).Keyboard[Key.Enter])
            {
                Loader.CurrentMap = "Map00";
                Loader.NextMap = "Map01";
                gameLoop.ActiveGameState = new MenuState();
            }
        }

        public override void Draw()
        {
            ScreenBuffer.InsertSprite(((GamePlayState)parent).hudBar);
            ScreenBuffer.InsertSprite(((GamePlayState)parent).CurrentWeapon);
            ScreenBuffer.InsertSprite(((GamePlayState)parent).CurrentFace);
            Font.AddText(new Text("Congratulations!", "yay1", new Vector2(50, 75), 1f));
            Font.AddText(new Text("You made it", "yay2", new Vector2(50, 125), 1f));
            Font.AddText(new Text("Total time: " + Math.Round(Program.TotalTime, 2) + " seconds.", "time", new Vector2(50, 175), 1f));
            Font.AddText(new Text("Thank you for playing!", "yay3", new Vector2(50, 225), 1f));
            Font.AddText(new Text("By: Metaldemon", "yay4", new Vector2(50, 275), 1f));

            Font.AddText(new Text("Press Enter to return to menu.", "yay5", new Vector2(50, 350), 0.825f));
            Font.Draw();
            ScreenBuffer.Draw();
            ScreenBuffer.Clear();
        }
    }
}
