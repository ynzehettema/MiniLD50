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
    class MenuState : GameState
    {
        private Sprite menuScreen;
        private Sprite arrow;
        private float alpha = 1f;
        private bool begin = false;
        private double psycheTime = 1f;
        private string quitMessage = "";
        private bool showQuitMessage;
        private bool startedMusic = false;

        private GamePlayState gameplayState;

        public MenuState()
        {
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/states/menu.png", "menu");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/states/menu2.png", "menu2");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/states/arrow.png", "arrow");
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/states/getpsyched.png", "getpsyched");
            ContentManager.LoadSound(GameUtils.GetAppPath() + "/content/audio/select.wav", "select");

            //FONT
            ContentManager.LoadTexture(GameUtils.GetAppPath() + "/content/font2.png", "font");

            menuScreen = new Sprite("menu", new Vector2(640 / 2, 480 / 2));
            arrow = new Sprite("arrow", new Vector2(168, 228));

        }

        public MenuState(GamePlayState parent)
        {
            ContentManager.GetSound("select").Play();
            menuScreen = new Sprite("menu2", new Vector2(640 / 2, 480 / 2));
            arrow = new Sprite("arrow", new Vector2(168, 228));
            gameplayState = parent;
        }

        public override void Update(GameLoop gameLoop)
        {
            if (!startedMusic && Program.MenuMusic != null)
            {
                Program.MenuMusic.Gain = 0;
                Program.MenuMusic.Play();
                startedMusic = true;
            }
            
            if (!begin && alpha > 0)
            {
                alpha -= 0.001f;
                if (Program.MenuMusic != null && Program.MenuMusic.Gain < 0.25)
                {
                    Program.MenuMusic.Gain += 0.00025f;
                }
                if (Program.Music != null && Program.Music.Gain > 0)
                {
                    Program.Music.Gain -= 0.00025f;
                }
            }
            if (alpha <= 0 && !begin)
            {
                if (Program.MenuMusic != null && Program.MenuMusic.Gain < 0.25)
                {
                    Program.MenuMusic.Gain += 0.00025f;
                }
                if (Program.Music != null && Program.Music.Gain > 0)
                {
                    Program.Music.Gain -= 0.00025f;
                }
                if (!showQuitMessage)
                {
                    if (Input.GetState(0).Keyboard[Key.Up] && arrow.Position.Y != 228)
                    {
                        ContentManager.GetSound("select").Play();
                        arrow.Position.Y = 228;
                    }
                    if (Input.GetState(0).Keyboard[Key.Down] && arrow.Position.Y != 228 + 45)
                    {
                        ContentManager.GetSound("select").Play();
                        arrow.Position.Y = 228 + 45;
                    }
                    if (Input.GetState(0).Keyboard[Key.Enter] && !Input.GetState(1).Keyboard[Key.Enter])
                    {
                        ContentManager.GetSound("select").Play();
                        if (arrow.Position.Y == 228)
                            begin = true;
                        else
                        {
                            showQuitMessage = true;
                            Random rand = new Random();
                            switch (rand.Next(0, 8))
                            {
                                case 0:
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("For guns and glory, press N ", "quit1", new Vector2(640 / 2, 480 / 2 - 25), 0.5f, false));
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("For work and worry, press Y", "quit2", new Vector2(640 / 2, 480 / 2 + 25), 0.5f, false));
                                    break;
                                case 1:
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("Press N if you are brave", "quit1", new Vector2(640 / 2, 480 / 2 - 25), 0.5f, false));
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("Press Y to cower in shame", "quit2", new Vector2(640 / 2, 480 / 2 + 25), 0.5f, false));
                                    break;
                                case 2:
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("You are at an intersection", "quit1", new Vector2(640 / 2, 480 / 2 - 25), 0.5f, false));
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("A sign says, `press Y to quit.`", "quit2", new Vector2(640 / 2, 480 / 2 + 25), 0.5f, false));
                                    break;
                                case 3:
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("Press N to save the world", "quit1", new Vector2(640 / 2, 480 / 2 - 25), 0.5f, false));
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("Press Y to abandon it in its hour of need", "quit2", new Vector2(640 / 2, 480 / 2 + 25), 0.5f, false));
                                    break;
                                case 4:
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("Press N for more carnage", "quit1", new Vector2(640 / 2, 480 / 2 - 25), 0.5f, false));
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("Press Y to be a weenie", "quit2", new Vector2(640 / 2, 480 / 2 + 25), 0.5f, false));
                                    break;
                                case 5:
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("Heroes, press N", "quit1", new Vector2(640 / 2, 480 / 2 - 25), 0.5f, false));
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("Whimps, press Y", "quit2", new Vector2(640 / 2, 480 / 2 + 25), 0.5f, false));
                                    break;
                                case 6:
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("Chickening out...", "quit1", new Vector2(640 / 2, 480 / 2 - 25), 0.5f, false));
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("already?", "quit2", new Vector2(640 / 2, 480 / 2 + 25), 0.5f, false));
                                    break;
                                default:
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("Dost thou wish to leave with ", "quit1", new Vector2(640 / 2, 480 / 2 - 25), 0.5f, false));
                                    FontHandler.AddText(new MetalLib.Pencil.Gaming.Text("such hasty abandon? Y / N", "quit2", new Vector2(640 / 2, 480 / 2 + 25), 0.5f, false));
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    if (Input.GetState(0).Keyboard['Y'])
                    {
                        ContentManager.GetSound("select").Play();
                        gameLoop.Run = false;
                    }
                    else if (Input.GetState(0).Keyboard['N'])
                    {
                        ContentManager.GetSound("select").Play();
                        showQuitMessage = false;
                        FontHandler.TextList = new List<MetalLib.Pencil.Gaming.Text>();
                    }
                }

            }
            if (begin)
            {
                if (Program.MenuMusic.Gain > 0.0)
                {
                    Program.MenuMusic.Gain -= 0.00025f;
                }
                if (alpha < 1f)
                {
                    alpha += 0.001f;
                    Glfw.SetTime(0.0);
                }
                else
                {
                    Program.TotalTime += (float)Glfw.GetTime();
                    double delta = Glfw.GetTime();
                    if (Program.TotalTime > 1f)
                    {
                        Glfw.SetTime(0.0);
                    }
                    psycheTime -= delta;
                    if (psycheTime <= 0)
                    {
                        GL.Color4(Color4.White);
                        if (gameplayState == null)
                        {
                            gameLoop.ActiveGameState = new GamePlayState();
                        }
                        else
                        {
                            gameLoop.ActiveGameState = gameplayState;
                        }
                    }
                }
            }
        }

        public override void Draw()
        {
            if (begin && alpha >= 1f)
            {
                GL.Enable(EnableCap.Blend);
                GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.OneMinusSrcAlpha);
                GL.Begin(BeginMode.Quads);

                GL.Color4(0, 0, 0, 0.5f);


                GL.Vertex2(0, 0);
                GL.Vertex2(640, 0);
                GL.Vertex2(640, 480);
                GL.Vertex2(0, 480);

                GL.End();
                GL.Disable(EnableCap.Blend);

                menuScreen = new Sprite("getpsyched", new Vector2(640 / 2, 480 / 2));
                menuScreen.Draw();

                GL.LineWidth(4f);
                GL.Begin(BeginMode.Lines);

                GL.Color4(Color4.DarkRed);
                GL.Vertex2((640 / 2) - menuScreen.Texture.Width / 2, (480 / 2) + 38);
                GL.Color4(Color4.Red);
                GL.Vertex2(((640 / 2) + menuScreen.Texture.Width / 2) - menuScreen.Texture.Width * psycheTime, (480 / 2) + 38);
                GL.End();

            }
            else
            {
                menuScreen.Draw();
                arrow.Draw();
                if (showQuitMessage)
                {
                    GL.Color4(Color4.DarkGray);
                    GL.Begin(BeginMode.Quads);

                    GL.Vertex2(640 / 2 - 240, 480 / 2 - 70);
                    GL.Vertex2(640 / 2 + 240, 480 / 2 - 70);
                    GL.Vertex2(640 / 2 + 240, 480 / 2 + 70);
                    GL.Vertex2(640 / 2 - 240, 480 / 2 + 70);

                    GL.End();
                    FontHandler.Draw();
                }
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
}
