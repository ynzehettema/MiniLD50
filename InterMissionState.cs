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
    class InterMissionState : GameState
    {
        private GameState parent;
        private int totalKills, totalPotentionalKills, currentKillPercentage;
        private double percentageDelay = 0.025, percentageCounter;

        public InterMissionState(GameState parent)
        {
            this.parent = parent;

            totalKills = RayCaster.ActorList.Where(x => (x.ShootAble && x.Dead)).ToList().Count;
            totalPotentionalKills = RayCaster.ActorList.Where(x => x.ShootAble).ToList().Count;
        }

        public override void Update(GameLoop gameLoop)
        {
            Program.TotalTime += (float)Glfw.GetTime();
            double delta = Glfw.GetTime();
            if (Program.TotalTime > 1f)
            {
                Glfw.SetTime(0.0);
            }
            if (currentKillPercentage < (int)(((double)totalKills / (double)totalPotentionalKills) * 100.0))
            {
                percentageCounter += delta;
                if (percentageCounter > percentageDelay)
                {
                    percentageCounter = 0;

                    currentKillPercentage++;
                }
            }
            else
            {
                if (Input.GetState(0).Keyboard[Key.Enter] && !Input.GetState(1).Keyboard[Key.Enter])
                {
                    ((GamePlayState)parent).rayCaster = new RayCaster();
                    gameLoop.ActiveGameState = parent;

                }
            }

        }

        public override void Draw()
        {

            for (int x = 0; x < 640; x++)
            {
                for (int y = 0; y < 480 - 72; y++)
                {
                    ScreenBuffer.screenBuffer[y * 640 + x] = ScreenBuffer.GetUintFromARGB(255, 120, 120, 120);
                }
            }

            ScreenBuffer.InsertSprite(((GamePlayState)parent).hudBar);
            ScreenBuffer.InsertSprite(((GamePlayState)parent).CurrentWeapon);
            ScreenBuffer.InsertSprite(((GamePlayState)parent).CurrentFace);

            Font.TextList = new List<Text>();
            Font.AddText(new Text("Floor", "floor", new Vector2(15, 422.5f), 0.55f, true));
            Font.AddText(new Text(RayCaster.Floor.ToString(), "floorval", new Vector2(35f, 450f), 0.65f, true));

            Font.AddText(new Text("Score", "score", new Vector2(110, 422.5f), 0.55f, true));
            Font.AddText(new Text("1337", "scoreval", new Vector2(120, 450f), 0.65f, true));

            Font.AddText(new Text("Lives", "lives", new Vector2(200, 422.5f), 0.55f, true));
            Font.AddText(new Text("1", "livesval", new Vector2(225, 450f), 0.65f, true));

            Font.AddText(new Text("Health", "health", new Vector2(350f, 422.5f), 0.55f, true));
            Font.AddText(new Text(Player.Health + "%", "healthval", new Vector2(355f, 450f), 0.65f, true));

            Font.AddText(new Text("Ammo", "ammo", new Vector2(450f, 422.5f), 0.55f, true));
            Font.AddText(new Text(Player.Ammo.ToString(), "ammoval", new Vector2(462.5f, 450f), 0.65f, true));
            Font.AddText(new Text("Floor: " + RayCaster.Floor + " Completed!", "floorcomplete", new Vector2(50, 75), 1f));
            Font.AddText(new Text("Time: " + Math.Round(RayCaster.CurrentTime, 2) + " seconds.", "time", new Vector2(50, 125), 1f));
            Font.AddText(new Text("Kill Ratio: " + currentKillPercentage + "%", "killpercentage", new Vector2(50, 175), 1f));

            Font.AddText(new Text("Press Enter!", "entertext", new Vector2(50, 225), 1f));

            Font.Draw();

            ScreenBuffer.Draw();
            ScreenBuffer.Clear();
        }
    }
}
