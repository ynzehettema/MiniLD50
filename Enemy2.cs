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
    class Enemy2 : Actor
    {
        private double fireDelay = 0;
        private double fireState = 1.0;
        private bool isShooting = false;
        private bool hasSeenPlayer = false;

        public Enemy2(Vector2 position)
            : base(position)
        {
            Sprite = new Sprite("enemy1", Vector2.Zero);
            ShootAble = true;
            Solid = true;
            health = 200;
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
                fireDelay = 0.25;
                fireState = 0.25;
            }
            if (hasSeenPlayer)
            {
                if (isShooting)
                {
                    if (fireState > 0.2)
                    {
                        if (Sprite.textureName != "enemy21")
                        {
                            Sprite.textureName = "enemy21";
                        }
                    }
                    else
                    {
                        if (fireState > 0.1)
                        {
                            if (Sprite.textureName != "enemy22")
                            {
                                Sprite.textureName = "enemy22";
                                Random rand = new Random();
                                if (rand.Next(0, 100) < 77)
                                {
                                    int damage = (int)(12.5 - (GameUtils.GetDistance(RayCaster.Position, Position)));
                                    if (damage > 0)
                                    {
                                        Player.Health -= damage;
                                    }
                                }
                                ContentManager.GetSound("machine").Play();
                            }
                        }
                        else
                        {
                            if (fireState > 0)
                            {
                                if (Sprite.textureName != "enemy21")
                                {
                                    Sprite.textureName = "enemy21";
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
                else
                {
                    if (Sprite.textureName != "enemy2")
                    {
                        Sprite.textureName = "enemy2";
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
                if (!isShooting && GameUtils.GetDistance(Position, RayCaster.Position) > 1.5)
                {
                    Vector2 collisionPos = GameUtils.MoveTowards(Position, RayCaster.Position, 0.25f);
                    Vector2 nextPos = GameUtils.MoveTowards(Position, RayCaster.Position, 0.025f);
                    if (RayCaster.WorldMap[(int)collisionPos.X, (int)collisionPos.Y] == ' ')
                    {
                        Position = nextPos;
                    }
                    else
                    {
                        if (RayCaster.WorldMap[(int)collisionPos.X, (int)Position.Y] == ' ')
                        {
                            Position.X = nextPos.X;
                        }
                        else
                        {
                            if (RayCaster.WorldMap[(int)Position.X, (int)collisionPos.Y] == ' ')
                            {
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
