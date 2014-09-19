using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    class Loader
    {
        public static string NextMap = "map01";
        public static string CurrentMap = string.Empty;

        public static void LoadMap(string file, out int w, out int h, out Vector2 playerpos, out char[,] map, bool restart = false)
        {
            w = h = 0;
            playerpos = new Vector2(1.5f, 1.5f);
            StreamReader sr = new StreamReader(file);
            w = int.Parse(sr.ReadLine());
            h = int.Parse(sr.ReadLine());
            if (!restart)
            {
                CurrentMap = NextMap;
                NextMap = sr.ReadLine();
            }
            else
            {
                sr.ReadLine();
            }
            map = new char[w, h];
            int y = 0;

            string line = string.Empty;

            while ((line = sr.ReadLine()) != null)
            {
                for (int x = 0; x < line.Length - 1; ++x)
                {
                    switch (line[x])
                    {
                        case 'W':
                            playerpos = new Vector2(x + 0.5f, y + 0.5f);
                            map[x, y] = ' ';
                            RayCaster.planeX = 0.66;
                            RayCaster.planeY = 0.0;
                            RayCaster.dirX = 0.0;
                            RayCaster.dirY = -1.0;
                            break;
                        case 'S':
                            playerpos = new Vector2(x + 0.5f, y + 0.5f);
                            map[x, y] = ' ';
                            RayCaster.planeX = -0.66;
                            RayCaster.planeY = 0.0;
                            RayCaster.dirX = 0.0;
                            RayCaster.dirY = 1.0;
                            break;
                        case 'A':
                            playerpos = new Vector2(x + 0.5f, y + 0.5f);
                            map[x, y] = ' ';
                            RayCaster.dirX = -1.0;
                            RayCaster.planeY = -0.66;
                            break;
                        case 'D':
                            playerpos = new Vector2(x + 0.5f, y + 0.5f);
                            map[x, y] = ' ';
                            RayCaster.dirX = 1.0;
                            RayCaster.planeY = 0.66;
                            break;
                        case 'E':
                            RayCaster.ActorList.Add(new Enemy(new Vector2(x + 0.5f, y + 0.5f)));
                            map[x, y] = ' ';
                            break;
                        case 'R':
                            RayCaster.ActorList.Add(new Enemy2(new Vector2(x + 0.5f, y + 0.5f)));
                            map[x, y] = ' ';
                            break;
                        case 'T':
                            RayCaster.ActorList.Add(new DeadEnemy(new Vector2(x + 0.5f, y + 0.5f)));
                            map[x, y] = ' ';
                            break;
                        case 'P':
                            RayCaster.ActorList.Add(new Pillar(new Vector2(x + 0.5f, y + 0.5f)));
                            map[x, y] = ' ';
                            break;
                        case 'H':
                            RayCaster.ActorList.Add(new FirstAid(new Vector2(x + 0.5f, y + 0.5f)));
                            map[x, y] = ' ';
                            break;
                        case 'C':
                            RayCaster.ActorList.Add(new Clip(new Vector2(x + 0.5f, y + 0.5f)));
                            map[x, y] = ' ';
                            break;
                        case 'L':
                            RayCaster.ActorList.Add(new Lamp(new Vector2(x + 0.5f, y + 0.5f)));
                            map[x, y] = ' ';
                            break;
                        case 'M':
                            RayCaster.ActorList.Add(new MachineGun(new Vector2(x + 0.5f, y + 0.5f)));
                            map[x, y] = ' ';
                            break;
                        case 'G':
                            RayCaster.ActorList.Add(new ChainGun(new Vector2(x + 0.5f, y + 0.5f)));
                            map[x, y] = ' ';
                            break;
                        default:
                            map[x, y] = line[x];
                            break;
                    }
                }
                y++;
            }
            sr.Close();
        }


    }
}
