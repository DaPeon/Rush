using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace EbaucheProjet
{
    public class Bullet : Particle
    {
        ParticleEngine pEngine;

        public Bullet(int type, Vector2 position, Vector2 dir, float speed, float size, Color color ,int TTL)
            : base(type, position, dir, speed, 0f, 0f, color, size, TTL)
        {
            //pEngine = 
        }


        public void Update(Map map)
        {
            base.Update(map);
        }

        public void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }

    public class BulletGenerator
    {
        public bool add;
        public Vector2 pos;
        public List<Bullet> bullets;
        public float speed;
        public int type;
        public int TTL;
        public Color color;
        public float size;

        public BulletGenerator(int type, float speed, Color color, float size, int TTL)
        {
            pos = Vector2.Zero;
            bullets = new List<Bullet>();

            this.TTL = TTL;

            this.speed = speed;
            this.type = type;
            this.color = color;
            this.size = size;
        }

        public void Update(Vector2 pos, Vector2 dir, Map map)
        {
            this.pos = pos;

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(map);

                if (!bullets[i].alive) bullets.Remove(bullets[i]);
            }

            if (add)
            {
                bullets.Add(NewBullet(dir));
            }
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < bullets.Count; i++) bullets[i].Draw(sb);
        }

        public Bullet NewBullet(Vector2 dir)
        {
            return new Bullet(type ,pos, dir, speed, size, color, TTL);
        }
    }

    public class Weapon
    {
        public BulletGenerator bg;
        public float speed;
        public int type;
        public bool shoot;
        public int TTL;
        public Color color;
        public float size;

        public int bulletsInterval;
        public int lastBullet;

        public Weapon(int type, float speed, Color color, int bulletsInterval, float size, int TTL)
        {
            this.speed = speed;
            this.type = type;
            this.color = color;
            this.size = size;
            this.TTL = TTL;
            this.bulletsInterval = bulletsInterval;
            shoot = false;
            lastBullet = 0;

            bg = new BulletGenerator(type, speed, color,size, TTL);
        }

        public void Update(GameTime gt, Vector2 pos, Vector2 dir, Map map)
        {
            bg.add = false;

            if ((int)gt.TotalGameTime.TotalMilliseconds - lastBullet >= bulletsInterval && shoot)
            {
                lastBullet = (int)gt.TotalGameTime.TotalMilliseconds;
                shoot = false;
                bg.add = true;
            }
            else
                shoot = false;

            bg.Update(pos, dir, map);
        }

        public void Draw(SpriteBatch sb)
        {
            bg.Draw(sb);
        }

        public void Shoot() { shoot = true; }

    }

    // Weapon : BulletType, BulletSpeed, BulletColor, 1000/BulletsPerSecond, BulletSize, BulletLife

    public class LanceBoule : Weapon
    {
        public LanceBoule() : base(4, 50, Color.Blue, 1000/10, 1.5f, 100) { }
    }
}
