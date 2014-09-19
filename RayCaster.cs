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
    class Actor
    {
        public Vector2 Position;
        public int letsPrentendThisIsZ = 0;
        public int Scale = 1;
        public bool ShootAble = false;
        public int health = 100;
        public bool Solid = false;
        public Sprite Sprite;

        public bool Dead = false;
        public bool Remove = false;

        public Actor(Vector2 position)
        {
            Position = position;

        }

        public virtual void Update(double delta)
        {

        }

    }

    class Pillar : Actor
    {
        public Pillar(Vector2 position)
            : base(position)
        {
            Sprite = new Sprite("pillar", Vector2.Zero);
            Solid = true;
        }
    }

    class DeadEnemy : Actor
    {
        public DeadEnemy(Vector2 position)
            : base(position)
        {
            Sprite = new Sprite("enemy13", Vector2.Zero);
        }
    }

    class MachineGun : Actor
    {
        public MachineGun(Vector2 position)
            : base(position)
        {
            Sprite = new Sprite("machinegun2", Vector2.Zero);
            Scale = 2;
            letsPrentendThisIsZ = (int)(Math.Pow(4, 4) * 0.75);
        }

        public override void Update(double delta)
        {
            if (GameUtils.GetDistance(Position, RayCaster.Position) < 0.75)
            {
                if (!Player.HasMachineGun)
                {
                    Player.Weapon.textureName = "machinegun";
                    ((GamePlayState)Program.MainGameLoop.ActiveGameState).CurrentWeapon.textureName = "machinehud";
                }
                Player.HasMachineGun = true;

                ContentManager.GetSound("weaponpickup").Play();
                for (int a = 0; a < 8; ++a)
                {
                    if (Player.Ammo < 100)
                    {
                        Player.Ammo++;
                    }
                    else
                    {
                        break;
                    }
                }
                Remove = true;
            }
        }
    }
    class ChainGun : Actor
    {
        public ChainGun(Vector2 position)
            : base(position)
        {
            Sprite = new Sprite("chaingun", Vector2.Zero);
            Scale = 2;
            letsPrentendThisIsZ = (int)(Math.Pow(4, 4) * 0.75);
        }

        public override void Update(double delta)
        {
            if (GameUtils.GetDistance(Position, RayCaster.Position) < 0.75)
            {
                if (!Player.HasChainGun)
                {
                    Player.Weapon.textureName = "mini";
                    ((GamePlayState)Program.MainGameLoop.ActiveGameState).CurrentWeapon.textureName = "minihud";
                }
                Player.HasChainGun = true;

                ContentManager.GetSound("weaponpickup").Play();
                for (int a = 0; a < 8; ++a)
                {
                    if (Player.Ammo < 100)
                    {
                        Player.Ammo++;
                    }
                    else
                    {
                        break;
                    }
                }
                Remove = true;
            }
        }
    }

    class Clip : Actor
    {
        public Clip(Vector2 position)
            : base(position)
        {
            Sprite = new Sprite("clip", Vector2.Zero);
            Scale = 4;
            letsPrentendThisIsZ = (int)(Math.Pow(Scale, Scale) * 0.75);
        }

        public override void Update(double delta)
        {
            if (GameUtils.GetDistance(Position, RayCaster.Position) < 0.5)
            {
                ContentManager.GetSound("clip").Play();
                for (int a = 0; a < 8; ++a)
                {
                    if (Player.Ammo < 100)
                    {
                        Player.Ammo++;
                    }
                    else
                    {
                        break;
                    }
                }
                Remove = true;
            }
        }
    }

    class FirstAid : Actor
    {
        public FirstAid(Vector2 position)
            : base(position)
        {
            Sprite = new Sprite("firstaid", Vector2.Zero);
            Scale = 3;
            letsPrentendThisIsZ = (int)(Math.Pow(4, 4) * 0.75);
        }

        public override void Update(double delta)
        {
            if (GameUtils.GetDistance(Position, RayCaster.Position) < 0.5)
            {
                ContentManager.GetSound("health").Play();
                for (int a = 0; a < 25; ++a)
                {
                    if (Player.Health < 100)
                    {
                        Player.Health++;
                    }
                    else
                    {
                        break;
                    }
                }
                Remove = true;
            }
        }
    }

    class Lamp : Actor
    {
        public Lamp(Vector2 position)
            : base(position)
        {
            Sprite = new Sprite("lamp", Vector2.Zero);

        }

    }

    class RayCaster
    {
        private static readonly int detailLevel = 1;
        public static int MapWidth, MapHeight;
        public static char[,] WorldMap;
        public static Vector2 Position = new Vector2(1.5f, 1.5f);
        public static double dirX = -1.0, dirY = 0.0, planeX = 0.0, planeY = -0.66;
        private static int texWidth = 16, texHeight = 16;
        public static List<WallTexture> wallTextures = new List<WallTexture>();

        public static List<Actor> ActorList = new List<Actor>();
        public bool newMap = false;

        public static int Floor = 0;
        public static double CurrentTime = 0;


        public RayCaster()
        {
            dirX = -1.0;
            dirY = 0.0;
            planeX = 0.0;
            planeY = -0.66;
            ActorList = new List<Actor>();
            Floor++;
            if (Loader.NextMap != "Are you just poking around in the map files, or do you wish there was an automap feature? :)")
            {
                Loader.LoadMap(GameUtils.GetAppPath() + "/content/maps/" + Loader.NextMap + ".txt", out MapWidth, out MapHeight, out Position, out WorldMap);
            }

            if (wallTextures.Count == 0)
            {
                for (int a = 0; a < 9; a++)
                {
                    wallTextures.Add(new WallTexture(GameUtils.GetAppPath() + "/content/walltextures/wall" + a + ".png"));
                }
            }

            CurrentTime = 0;
        }

        public RayCaster(bool restart)
        {
            dirX = -1.0;
            dirY = 0.0;
            planeX = 0.0;
            planeY = -0.66;
            ActorList = new List<Actor>();
            Loader.LoadMap(GameUtils.GetAppPath() + "/content/maps/" + Loader.CurrentMap + ".txt", out MapWidth, out MapHeight, out Position, out WorldMap, true);
            if (wallTextures.Count == 0)
            {
                for (int a = 0; a < 9; a++)
                {
                    wallTextures.Add(new WallTexture(GameUtils.GetAppPath() + "/content/walltextures/wall" + a + ".png"));
                }
            }
            CurrentTime = 0;

        }

        public void Update(double delta)
        {
            CurrentTime += delta;
            ActorList = ActorList.Where(x => !x.Remove).ToList();
            for (int i = 0; i < ActorList.Count; i++)
            {
                if (!ActorList[i].Dead)
                {
                    ActorList[i].Update(delta);
                }
            }
            double moveSpeed = delta * 5;
            double rotateSpeed = delta * 2.0;
            Player.IsMoving = false;
            Player.IsFireing = false;
            if (Input.GetState(0).Keyboard[Key.LeftShift] || Input.GetState(0).Keyboard[Key.RightShift])
            {
                moveSpeed *= 1.5;
                rotateSpeed *= 1.5;
            }
            if (Input.GetState(0).Keyboard['1'])
            {
                Player.Weapon.textureName = "pistol";
                ((GamePlayState)Program.MainGameLoop.ActiveGameState).CurrentWeapon.textureName = "pistolhud";
            }
            if (Input.GetState(0).Keyboard['2'] && Player.HasMachineGun)
            {
                Player.Weapon.textureName = "machinegun";
                ((GamePlayState)Program.MainGameLoop.ActiveGameState).CurrentWeapon.textureName = "machinehud";
            }
            if (Input.GetState(0).Keyboard['3'] && Player.HasChainGun)
            {
                Player.Weapon.textureName = "mini";
                ((GamePlayState)Program.MainGameLoop.ActiveGameState).CurrentWeapon.textureName = "minihud";
            }
            if (Input.GetState(0).Keyboard[Key.RightControl] || Input.GetState(0).Mouse.LeftButton)
            {
                Player.IsFireing = true;
            }
            if (Input.GetState(0).Keyboard[Key.Space])
            {
                int x = (int)(Position.X + dirX * (1)), y = (int)(Position.Y + dirY * (1));
                if (WorldMap[x, y] == '5' || WorldMap[x, y] == '8')
                {
                    WorldMap[x, y] = ' ';
                    ContentManager.GetSound("open").Play();
                }
                if (WorldMap[x, y] == '7')
                {
                    if (Loader.NextMap != "Are you just poking around in the map files, or do you wish there was an automap feature? :)")
                    {
                        newMap = true;
                    }
                    else
                    {
                        Program.MainGameLoop.ActiveGameState = new EndState(Program.MainGameLoop.ActiveGameState);
                    }
                }
            }
            if (Input.GetState(0).Keyboard['W'] || Input.GetState(0).Keyboard[Key.Up])
            {
                if (WorldMap[(int)(Position.X + dirX * (1)), (int)Position.Y] == ' ')
                {
                    bool col = false;
                    foreach (Actor a in ActorList)
                    {
                        if (a.Solid && GameUtils.GetDistance(new Vector2(Position.X + (float)dirX, Position.Y), a.Position) < 0.5)
                        {
                            col = true;
                            break;
                        }
                    }
                    if (!col)
                        Position.X += (float)(dirX * moveSpeed);
                }
                if (WorldMap[(int)(Position.X), (int)(Position.Y + dirY)] == ' ')
                {
                    bool col = false;
                    foreach (Actor a in ActorList)
                    {
                        if (a.Solid && GameUtils.GetDistance(new Vector2(Position.X, Position.Y + (float)dirY), a.Position) < 0.5)
                        {
                            col = true;
                            break;
                        }
                    }
                    if (!col)
                        Position.Y += (float)(dirY * moveSpeed);
                }
                Player.IsMoving = true;
            }
            if (Input.GetState(0).Keyboard['S'] || Input.GetState(0).Keyboard[Key.Down])
            {
                if (WorldMap[(int)(Position.X - dirX), (int)Position.Y] == ' ')
                {
                    bool col = false;
                    foreach (Actor a in ActorList)
                    {
                        if (a.Solid && GameUtils.GetDistance(new Vector2(Position.X - (float)dirX, Position.Y), a.Position) < 0.5)
                        {
                            col = true;
                            break;
                        }
                    }
                    if (!col)
                        Position.X -= (float)(dirX * moveSpeed);
                }
                if (WorldMap[(int)(Position.X), (int)(Position.Y - dirY)] == ' ')
                {
                    bool col = false;
                    foreach (Actor a in ActorList)
                    {
                        if (a.Solid && GameUtils.GetDistance(new Vector2(Position.X, Position.Y - (float)dirY), a.Position) < 0.5)
                        {
                            col = true;
                            break;
                        }
                    }
                    if (!col)
                        Position.Y -= (float)(dirY * moveSpeed);
                }
                Player.IsMoving = true;
            }
            if (Input.GetState(0).Keyboard['A'] || Input.GetState(0).Keyboard[Key.Left])
            {
                double oldDirX = dirX;
                dirX = dirX * Math.Cos(-rotateSpeed) - dirY * Math.Sin(-rotateSpeed);
                dirY = oldDirX * Math.Sin(-rotateSpeed) + dirY * Math.Cos(-rotateSpeed);
                double oldPlaneX = planeX;
                planeX = planeX * Math.Cos(-rotateSpeed) - planeY * Math.Sin(-rotateSpeed);
                planeY = oldPlaneX * Math.Sin(-rotateSpeed) + planeY * Math.Cos(-rotateSpeed);
            }
            if (Input.GetState(0).Keyboard['D'] || Input.GetState(0).Keyboard[Key.Right])
            {
                double oldDirX = dirX;
                dirX = dirX * Math.Cos(rotateSpeed) - dirY * Math.Sin(rotateSpeed);
                dirY = oldDirX * Math.Sin(rotateSpeed) + dirY * Math.Cos(rotateSpeed);
                double oldPlaneX = planeX;
                planeX = planeX * Math.Cos(rotateSpeed) - planeY * Math.Sin(rotateSpeed);
                planeY = oldPlaneX * Math.Sin(rotateSpeed) + planeY * Math.Cos(rotateSpeed);
            }



        }

        private double bufferMouseThing = 0;

        public List<int> SortSprites()
        {
            List<int> sorted = new List<int>();
            for (int i = 0; i < ActorList.Count; i++)
            {
                int lowest = -1;
                double currentDist = double.MaxValue;
                for (int j = 0; j < ActorList.Count; j++)
                {
                    if (!sorted.Contains(j))
                    {
                        double dist = GameUtils.GetDistance(Position, ActorList[j].Position);
                        if (dist < currentDist)
                        {
                            lowest = j;
                            currentDist = dist;
                        }
                    }
                }
                sorted.Add(lowest);
            }

            return sorted;
        }

        public void Render()
        {
            float ceilingHeight = 2f, floorHeight = 2f;
            double[] zBuffer = new double[Program.ScreenWidth];
            for (int x = 0; x < Program.ScreenWidth; x += detailLevel)
            {
                double cameraX = 2 * x / (double)Program.ScreenWidth - 1;
                double rayPosX = Position.X, rayPosY = Position.Y;
                double rayDirX = dirX + planeX * cameraX,
                       rayDirY = dirY + planeY * cameraX;
                int mapX = (int)rayPosX, mapY = (int)rayPosY;
                double sideDistX, sideDistY;
                double deltaDistX = Math.Sqrt(1 + (rayDirY * rayDirY) / (rayDirX * rayDirX)), deltaDistY = Math.Sqrt(1 + (rayDirX * rayDirX) / (rayDirY * rayDirY));
                int stepX, stepY;
                byte side = 0;
                bool hit = false;
                if (rayDirX < 0) { stepX = -1; sideDistX = (rayPosX - mapX) * deltaDistX; } else { stepX = 1; sideDistX = (mapX + 1.0 - rayPosX) * deltaDistX; }
                if (rayDirY < 0) { stepY = -1; sideDistY = (rayPosY - mapY) * deltaDistY; } else { stepY = 1; sideDistY = (mapY + 1.0 - rayPosY) * deltaDistY; }
                while (!hit)
                {
                    if (sideDistX < sideDistY) { sideDistX += deltaDistX; mapX += stepX; side = 0; }
                    else { sideDistY += deltaDistY; mapY += stepY; side = 1; }
                    if (WorldMap[mapX, mapY] != ' ') hit = true;
                }
                double wallDist = 0;
                if (side == 0)
                    wallDist = Math.Abs((mapX - rayPosX + (1 - stepX) / 2) / rayDirX);
                else
                    wallDist = Math.Abs((mapY - rayPosY + (1 - stepY) / 2) / rayDirY);
                zBuffer[x] = wallDist;
                int lineHeight = 1;
                if (wallDist != 0)
                    lineHeight = Math.Abs((int)((Program.ScreenHeight - 72) / wallDist));
                int drawStart = -(int)((float)lineHeight / ceilingHeight) + (Program.ScreenHeight - 72) / 2;
                if (drawStart < 0) drawStart = 0;
                int drawEnd = (int)((float)lineHeight / floorHeight) + (Program.ScreenHeight - 72) / 2;
                if (drawEnd >= (Program.ScreenHeight - 72)) drawEnd = (Program.ScreenHeight - 72) - 1;
                int textureIndex = 0;
                if (char.IsNumber(WorldMap[mapX, mapY]))
                    textureIndex = int.Parse(WorldMap[mapX, mapY].ToString());
                double wallX;
                if (side == 1) wallX = rayPosX + ((mapY - rayPosY + (1 - stepY) / 2) / rayDirY) * rayDirX;
                else wallX = rayPosY + ((mapX - rayPosX + (1 - stepX) / 2) / rayDirX) * rayDirY;
                wallX -= Math.Floor((wallX));
                int texX = (int)(wallX * (double)(texWidth));
                if (side == 0 && rayDirX > 0) texX = texWidth - texX - 1;
                if (side == 1 && rayDirY < 0) texX = texWidth - texX - 1;
                for (int y = drawStart; y < drawEnd; y += detailLevel)
                {
                    int d = y * 256 - (Program.ScreenHeight - 72) * 128 + lineHeight * 128;
                    int texY = ((d * texHeight) / lineHeight) / 256;
                    if (texX < 0) texX = 0;
                    if (texY < 0) texY = 0;
                    UInt32 color = wallTextures[textureIndex].Pixels[wallTextures[textureIndex].Width - 1 - texX, texY];
                    if (side == 1) color = (color >> 1) & 8355711;
                    for (int u = 0; u < detailLevel; u++)
                        for (int j = 0; j < detailLevel; j++)
                            ScreenBuffer.SetPixel(x + u, y + j, color);
                }
            }
            int h = (Program.ScreenHeight - 72), w = Program.ScreenWidth;
            List<int> sortedList = SortSprites();
            for (int i = sortedList.Count - 1; i >= 0; i--)
            {
                if (!ActorList[sortedList[i]].Remove)
                {
                    double spriteX = ActorList[sortedList[i]].Position.X - Position.X, spriteY = ActorList[sortedList[i]].Position.Y - Position.Y;
                    double invDet = 1.0 / (planeX * dirY - dirX * planeY);
                    double transformX = invDet * (dirY * spriteX - dirX * spriteY);
                    double transformY = invDet * (-planeY * spriteX + planeX * spriteY);
                    if (transformY > 0)
                    {
                        int screenOffset = (int)(ActorList[sortedList[i]].letsPrentendThisIsZ / transformY);
                        int spriteScreenX = (int)((Program.ScreenWidth / 2) * (1 + transformX / transformY));
                        int spriteHeight = Math.Abs((int)((h / transformY))) / ActorList[sortedList[i]].Scale;
                        int drawStartY = (-spriteHeight / 2 + h / 2) + screenOffset;
                        if (drawStartY < 0) drawStartY = 0;
                        int drawEndY = spriteHeight / 2 + h / 2 + screenOffset;
                        if (drawEndY >= h) drawEndY = h - 1;
                        int spriteWidth = Math.Abs((int)(h / (transformY))) / ActorList[sortedList[i]].Scale;
                        int drawStartX = (-spriteWidth / 2 + spriteScreenX);
                        if (drawStartX < 0) drawStartX = 0;
                        int drawEndX = spriteWidth / 2 + spriteScreenX;
                        if (drawEndX >= w) drawEndX = w - 1;
                        for (int stripe = drawStartX; stripe < drawEndX; stripe++)
                        {
                            int texX = (int)(256 * (stripe - (-spriteWidth / 2 + spriteScreenX)) * texWidth / spriteWidth) / 256;
                            if (transformY > 0 && stripe > 0 && stripe < w && transformY < zBuffer[stripe])
                            {
                                for (int y = drawStartY; y < drawEndY; ++y)
                                {
                                    int d = (y - screenOffset) * 256 - h * 128 + spriteHeight * 128;
                                    int texY = ((d * texHeight) / spriteHeight) / 256;
                                    uint color = ActorList[sortedList[i]].Sprite.Texture.Pixels[Math.Abs(texX), Math.Abs(texY)];
                                    if (color != 4294967295)
                                        for (int u = 0; u < detailLevel; u++)
                                            for (int j = 0; j < detailLevel; j++)
                                                ScreenBuffer.SetPixel(stripe + u, y + j, color);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    class WallTexture
    {
        public UInt32[,] Pixels;
        public int GLTexture = 0;
        public int Width, Height;
        private Bitmap bitMap;

        public WallTexture(string file)
        {


            GL.BindTexture(TextureTarget.Texture2D, (GLTexture = GL.Utils.LoadImage(file, true)));

            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureWidth, out Width);
            GL.GetTexLevelParameter(TextureTarget.Texture2D, 0, GetTextureParameter.TextureHeight, out Height);

            bitMap = new Bitmap(file);

            Pixels = new UInt32[Width, Height];

            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    Color c = bitMap.GetPixel(x, y);
                    Pixels[x, y] = UintFromColor(0, c.R, c.G, c.B);
                }
            }
        }

        private uint UintFromColor(byte a, byte r, byte g, byte b)
        {
            return (uint)((a << 24) | (r << 16) | (g << 8) | b);
        }
    }
}
