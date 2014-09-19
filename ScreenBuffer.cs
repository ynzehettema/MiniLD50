using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

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
    class ScreenBuffer
    {
        public static int Width, Height;
        public static UInt32[] screenBuffer;
        public static UInt32[] deathBuffer;
        private static int screenBufferTexture;
        private static Color4 floorColor = new Color4(113, 113, 113, 255), ceilingColor = new Color4(56, 56, 56, 255);


        public static void Initialize()
        {
            GL.GenTextures(1, out screenBufferTexture);
            Width = Program.ScreenWidth;
            Height = Program.ScreenHeight;
            screenBuffer = new UInt32[Width * Height];
            deathBuffer = new UInt32[Width * Height];
        }

        public static void SetPixel(int x, int y, UInt32 color)
        {
            if (y < Height)
            {
                screenBuffer[y * Width + x] = color;
            }
        }

        public static uint GetUintFromARGB(byte a, byte r, byte g, byte b)
        {
            return (uint)((a << 24) | (r << 16) | (g << 8) | b);
        }

        public static void Clear()
        {
            screenBuffer = new UInt32[Width * Height];
            for (int y = 0; y < Height; ++y)
                for (int x = 0; x < Width; ++x)
                {
                    if (y < (Height - 72) / 2)
                    {
                        SetPixel(x, y, GetUintFromARGB(255, (byte)(255f * ceilingColor.R), (byte)(255f * ceilingColor.G), (byte)(255f * ceilingColor.B)));
                    }
                    else
                    {
                        SetPixel(x, y, GetUintFromARGB(255, (byte)(255f * floorColor.R), (byte)(255f * floorColor.G), (byte)(255f * floorColor.B)));
                    }
                }
        }

        public static void Draw()
        {
            if (Player.Health <= 0 && deathBuffer.Contains((uint)0))
            {
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (deathBuffer[y * Width + x] != 0)
                        {
                            screenBuffer[y * Width + x] = deathBuffer[y * Width + x];
                        }
                    }
                }
                Random rand = new Random();
                for (int a = 0; a < 5000; a++)
                {
                    if (deathBuffer.Contains((uint)0))
                    {
                        int tries = 0;
                        while (true) // jam code
                        {
                            tries++;
                            if (tries > 500)
                                break;
                            int x = rand.Next(0, Width), y = rand.Next(0, Height - 72);
                            if (deathBuffer[y * Width + x] == 0)
                            {
                                deathBuffer[y * Width + x] = GetUintFromARGB(255, 255, 0, 0);
                                break;
                            }
                        }
                        if (tries > 500)
                        {
                            for (int x = 0; x < Width; x++)
                            {
                                for (int y = 0; y < Height; y++)
                                {
                                    deathBuffer[y * Width + x] = GetUintFromARGB(255, 255, 0, 0);
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                if (!deathBuffer.Contains((uint)0))
                {
                    deathBuffer = new UInt32[Width * Height];
                    Program.MainGameLoop.ActiveGameState = new GamePlayState(true);
                    Player.Health = 100;
                    Player.Ammo = 8;
                    Player.HasMachineGun = false;
                    Player.HasChainGun = false;

                }
            }
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.TextureRectangle, screenBufferTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, screenBuffer);

            GL.Begin(BeginMode.Quads);
            {
                GL.TexCoord2(0f, 0f);
                GL.Vertex2(0f, 0f);

                GL.TexCoord2(1f, 0f);
                GL.Vertex2(Width, 0f);

                GL.TexCoord2(1f, 1f);
                GL.Vertex2(Width, Height);

                GL.TexCoord2(0f, 1f);
                GL.Vertex2(0f, Height);
            }

            GL.End();
            GL.Disable(EnableCap.Texture2D);


        }

        public static void InsertSprite(Sprite s)
        {
            int x = (int)s.Position.X, y = (int)s.Position.Y;

            for (int spriteY = 0; spriteY < s.Texture.Height; spriteY++)
            {
                for (int spriteX = 0; spriteX < s.Texture.Width; spriteX++)
                {
                    if (s.Texture.Pixels[spriteX, spriteY] != 4294967295)
                        SetPixel(x + spriteX, y, s.GetPixel(spriteX, spriteY));
                }
                y++;
            }
        }
    }
}
