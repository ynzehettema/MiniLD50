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
    class Player
    {
        public static Sprite Weapon;
        public static bool IsMoving = false;
        public static bool IsFireing = false;
        private static double fireSpriteDelay = 0;
        private static double fireDelay = 0;

        public static int Ammo = 8;
        public static int Health = 100;

        public static bool HasMachineGun = false;
        public static bool HasChainGun = false;

        public static void Initialize()
        {
            Weapon = new Sprite("pistol", new Vector2(320, 480));
            Weapon.Position.X -= Weapon.Texture.Width / 2;
            Weapon.Position.Y -= 40;
            Weapon.Position.Y -= Weapon.Height;
        }

        public static void Update(double delta)
        {

            if (IsMoving)
            {

            }
            else
            {
                if (!IsFireing)
                {
                    Weapon.Position = new Vector2(320, 480);
                    Weapon.Position.Y -= 40;
                    Weapon.Position.Y -= Weapon.Height;

                }
            }
            Weapon.Position.X = 320 - Weapon.Texture.Width / 2;
            if (IsFireing && fireDelay <= 0 && Ammo > 0)
            {
                if (!Weapon.textureName.Contains("fire"))
                {
                    Ammo--;
                    HitScanRay.Fire(RayCaster.Position, GameUtils.GetRotation(Vector2.Zero, new Vector2((float)RayCaster.dirX, (float)RayCaster.dirY)));
                    Weapon.textureName += "fire";

                    if (Weapon.textureName.Contains("pistol"))
                    {
                        fireSpriteDelay = 0.17;
                        fireDelay = 0.50;
                        ContentManager.GetSound("pistol").Play();
                    }
                    if (Weapon.textureName.Contains("machine"))
                    {
                        fireSpriteDelay = 0.1;
                        fireDelay = 0.2;
                        ContentManager.GetSound("machine").Play();
                    }
                    if (Weapon.textureName.Contains("mini"))
                    {
                        fireSpriteDelay = 0.05;
                        fireDelay = 0.1;
                        ContentManager.GetSound("mini").Play();
                    }
                    Weapon.Position.Y = 480 - 80 - Weapon.Height;
                }
            }

            if (fireSpriteDelay > 0)
            {
                fireSpriteDelay -= delta;
                if (fireSpriteDelay <= 0)
                    Weapon.textureName = Weapon.textureName.Replace("fire", string.Empty);
            }
            if (fireDelay > 0)
            {
                fireDelay -= delta;

            }
            if (Weapon.Position.Y < 480 - 40 - Weapon.Height)
            {
                Weapon.Position.Y += 250f * (float)delta;
            }

        }

        public static void Draw()
        {
            ScreenBuffer.InsertSprite(Weapon);
        }
    }


    public class HitScanRay
    {
        public static void Fire(Vector2 initialPos, double angle)
        {
            while (true)
            {
                initialPos = GameUtils.MoveAlongAngle((float)angle, initialPos, 0.01f);
                foreach (Actor a in RayCaster.ActorList)
                {
                    if (a.ShootAble && !a.Dead)
                    {
                        if (GameUtils.GetDistance(initialPos, a.Position) < 0.25)
                        {
                            int damage = (int)(60 - (GameUtils.GetDistance(RayCaster.Position, a.Position) * 2.5));
                            if (damage > 0)
                            {
                                a.health -= damage;
                                ContentManager.GetSound("hurt").Play();
                            }
                            return;
                        }
                    }
                }
                if (RayCaster.WorldMap[(int)initialPos.X, (int)initialPos.Y] != ' ')
                {
                    break;
                }
            }
        }

        public static bool CanSeePlayer(Vector2 initialPos)
        {
            while (true)
            {
                initialPos = GameUtils.MoveTowards(initialPos, RayCaster.Position, 0.01f);
                if (GameUtils.GetDistance(initialPos, RayCaster.Position) < 0.25)
                {
                    return true;
                }
                if (RayCaster.WorldMap[(int)initialPos.X, (int)initialPos.Y] != ' ')
                {
                    break;
                }
            }
            return false;
        }
    }
}
