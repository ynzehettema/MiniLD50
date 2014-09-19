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
    class GamePlayState : GameState
    {
        public RayCaster rayCaster;
        public Sprite hudBar;
        public Sprite CurrentWeapon;
        public Sprite CurrentFace;
        public static bool startedMusic = false;


        public GamePlayState()
        {
            //HUD
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/hudbar.png", "hudbar");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/pistol.png", "pistol");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/pistolhud.png", "pistolhud");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/machinehud.png", "machinehud");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/minihud.png", "minihud");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/pistolfire.png", "pistolfire");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/machine.png", "machinegun");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/machinefire.png", "machinegunfire");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/mini.png", "mini");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/minifire.png", "minifire");

            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/face0.png", "face0");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/face1.png", "face1");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/face2.png", "face2");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/face3.png", "face3");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/hud/face4.png", "face4");

            //SPRITES
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/enemy1.png", "enemy1");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/enemy11.png", "enemy11");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/enemy12.png", "enemy12");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/enemy13.png", "enemy13");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/enemy2.png", "enemy2");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/enemy21.png", "enemy21");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/enemy22.png", "enemy22");


            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/clip.png", "clip");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/firstaid.png", "firstaid");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/lamp.png", "lamp");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/machinegun.png", "machinegun2");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/chaingun.png", "chaingun");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/sprites/pillar.png", "pillar");

            //AUDIO
            ContentManager.LoadSound(GameUtils.GetAppPath() + "/content/audio/pistol.wav", "pistol");
            ContentManager.LoadSound(GameUtils.GetAppPath() + "/content/audio/machine.wav", "machine");
            ContentManager.LoadSound(GameUtils.GetAppPath() + "/content/audio/mini.wav", "mini");
            ContentManager.LoadSound(GameUtils.GetAppPath() + "/content/audio/open.wav", "open");
            ContentManager.LoadSound(GameUtils.GetAppPath() + "/content/audio/close.wav", "close");
            ContentManager.LoadSound(GameUtils.GetAppPath() + "/content/audio/die.wav", "die");
            ContentManager.LoadSound(GameUtils.GetAppPath() + "/content/audio/hurt.wav", "hurt");
            ContentManager.LoadSound(GameUtils.GetAppPath() + "/content/audio/clip.wav", "clip");
            ContentManager.LoadSound(GameUtils.GetAppPath() + "/content/audio/health.wav", "health");
            ContentManager.LoadSound(GameUtils.GetAppPath() + "/content/audio/weaponpickup.wav", "weaponpickup");

            hudBar = new Sprite("hudbar", new Vector2(0, 480 * 0.85f));
            CurrentWeapon = new Sprite("pistolhud", new Vector2(550, 422.5f));
            CurrentFace = new Sprite("face1", new Vector2(640 / 2 - 40, 480 - 67.5f));
            ScreenBuffer.Initialize();
            Player.Initialize();
            rayCaster = new RayCaster();

        }

        public GamePlayState(bool restart)
        {
            hudBar = new Sprite("hudbar", new Vector2(0, 480 * 0.85f));
            CurrentWeapon = new Sprite("pistolhud", new Vector2(550, 422.5f));
            CurrentFace = new Sprite("face1", new Vector2(640 / 2 - 40, 480 - 67.5f));
            ScreenBuffer.Initialize();
            Player.Initialize();
            rayCaster = new RayCaster(true);
        }



        public override void Update(GameLoop gameLoop)
        {
            if (Player.Health > 75)
            {
                CurrentFace.textureName = "face1";
            }
            else
            {
                if (Player.Health > 50)
                {
                    CurrentFace.textureName = "face2";
                }
                else
                {
                    if (Player.Health > 25)
                    {
                        CurrentFace.textureName = "face3";
                    }
                    else
                    {
                        if (Player.Health > 0)
                        {
                            CurrentFace.textureName = "face4";
                        }
                        else
                        {
                            CurrentFace.textureName = "face0";
                        }
                    }
                }
            }
            if (!startedMusic && Program.Music != null)
            {
                Program.Music.Gain = 0.0f;
                Program.Music.Play();
                startedMusic = true;
            }
            if (startedMusic && Program.Music.Gain < 0.25)
            {
                Program.Music.Gain += 0.00025f;
                if (Program.MenuMusic.Gain > 0)
                {
                    Program.MenuMusic.Gain -= 0.00025f;
                }
            }
            Program.TotalTime += (float)Glfw.GetTime();
            double delta = Glfw.GetTime();
            if (Program.TotalTime > 1f)
            {
                Glfw.SetTime(0.0);
            }
            Player.Update(delta);
            rayCaster.Update(delta);
            if (rayCaster.newMap)
            {
                gameLoop.ActiveGameState = new InterMissionState(this);
            }
            if (Input.GetState(0).Keyboard[Key.Escape])
            {
                gameLoop.ActiveGameState = new MenuState(this);
            }
        }

        public override void Draw()
        {

            rayCaster.Render();
            Player.Draw();
            ScreenBuffer.InsertSprite(hudBar);
            ScreenBuffer.InsertSprite(CurrentWeapon);
            ScreenBuffer.InsertSprite(CurrentFace);
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

            Font.Draw();
            ScreenBuffer.Draw();
            ScreenBuffer.Clear();
        }

    }
}
