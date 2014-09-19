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
    class Enemy : Actor
    {
        private double fireDelay = 0;
        private double fireState = 1.0;
        private bool isShooting = false;
        private bool hasSeenPlayer = false;

        public Enemy(Vector2 position)
            : base(position)
        {
            Sprite = new Sprite("enemy1", Vector2.Zero);
            ShootAble = true;
            Solid = true;
        }

        public override void Update(double delta)
        {
            if (fireDelay > 0)
            {
                fireDelay -= delta;
            }
            if (!isShooting && fireDelay <= 0 && HitScanRay.CanSeePlayer(Position))
            {
                isShooting = true;
                Random rand = new Random();
                fireDelay = 1 + rand.NextDouble() * 1.75;
                fireState = 1.0;
            }
            if (hasSeenPlayer)
            {
                if (isShooting)
                {
                    if (fireState > 0.8)
                    {
                        if (Sprite.textureName != "enemy11")
                        {
                            Sprite.textureName = "enemy11";
                        }
                    }
                    else
                    {
                        if (fireState > 0.7)
                        {
                            if (Sprite.textureName != "enemy12")
                            {
                                Sprite.textureName = "enemy12";
                                Random rand = new Random();
                                if (rand.Next(0, 100) < 77)
                                {
                                    int damage = (int)(20 - (GameUtils.GetDistance(RayCaster.Position, Position)));
                                    if (damage > 0)
                                    {
                                        Player.Health -= damage;
                                    }
                                }
                                ContentManager.GetSound("pistol").Play();
                            }
                        }
                        else
                        {
                            if (fireState > 0.4)
                            {
                                if (Sprite.textureName != "enemy11")
                                {
                                    Sprite.textureName = "enemy11";
                                }
                            }
                            else
                            {
                                if (Sprite.textureName != "enemy1")
                                {
                                    Sprite.textureName = "enemy1";
                                }
                            }
                        }
                    }
                    if (fireState > 0)
                    {
                        fireState -= delta;
                    }
                    else
                    {
                        isShooting = false;
                    }
                }
                if (health <= 0)
                {
                    Sprite = new Sprite("enemy13", Vector2.Zero);
                    RayCaster.ActorList.Add(new Clip(Position));
                    Dead = true;
                    Solid = false;
                    ContentManager.GetSound("die").Play();
                }
                if (GameUtils.GetDistance(Position, RayCaster.Position) > 1.5)
                {
                    Vector2 collisionPos = GameUtils.MoveTowards(Position, RayCaster.Position, 0.25f);
                    Vector2 nextPos = GameUtils.MoveTowards(Position, RayCaster.Position, 0.025f);
                    if (RayCaster.WorldMap[(int)collisionPos.X, (int)collisionPos.Y] == ' ')
                    {
                        bool col = false;
                        foreach (Actor a in RayCaster.ActorList)
                        {
                            if (a.Solid && GameUtils.GetDistance(new Vector2((int)collisionPos.X, (int)collisionPos.Y), a.Position) < 0.5)
                            {
                                col = true;
                                break;
                            }
                        }
                        if (!col)
                            Position = nextPos;
                    }
                    else
                    {
                        if (RayCaster.WorldMap[(int)collisionPos.X, (int)Position.Y] == ' ')
                        {
                            bool col = false;
                            foreach (Actor a in RayCaster.ActorList)
                            {
                                if (a.Solid && GameUtils.GetDistance(new Vector2((int)collisionPos.X, (int)Position.Y), a.Position) < 0.5)
                                {
                                    col = true;
                                    break;
                                }
                            }
                            if (!col)
                                Position.X = nextPos.X;
                        }
                        else
                        {
                            if (RayCaster.WorldMap[(int)Position.X, (int)collisionPos.Y] == ' ')
                            {
                                bool col = false;
                                foreach (Actor a in RayCaster.ActorList)
                                {
                                    if (a.Solid && GameUtils.GetDistance(new Vector2((int)Position.X, (int)collisionPos.Y), a.Position) < 0.5)
                                    {
                                        col = true;
                                        break;
                                    }
                                }
                                if (!col)
                                    Position.Y = nextPos.Y;
                            }
                        }
                    }
                }
            }
            else
            {
                hasSeenPlayer = HitScanRay.CanSeePlayer(Position);
            }
        }
    }
}
