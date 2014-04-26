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
        public Bullet(int type, Vector2 position, Vector2 dir, float speed)
            : base(type, position, dir, speed, 0f, 0f, Color.White, 1f, 1000)
        { }
    }

    public class BulletGenerator
    {
        public bool add;
        public Vector2 pos;
        List<Bullet> bullets;
        public float speed;
        public int type;

        public BulletGenerator(int type, float speed)
        {
            pos = Vector2.Zero;
            bullets = new List<Bullet>();

            this.speed = speed;
            this.type = type;
        }

        public void Update(Vector2 pos, Vector2 dir, Map map)
        {
            this.pos = pos;

            foreach (Bullet b in bullets)
            {
                b.Update(map);

                if (b.TTL <= 0) bullets.Remove(b);
            }

            if (add)
            {
                bullets.Add(NewBullet(dir));
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Bullet b in bullets)
            {
                b.Draw(sb);
            }
        }

        public Bullet NewBullet(Vector2 dir)
        {
            return new Bullet(type ,pos, dir, speed);
        }
    }

    public class Weapon
    {
        public BulletGenerator bg;
        public float speed;
        public int type;
        public bool shoot;

        public Weapon(int type, float speed)
        {
            this.speed = speed;
            this.type = type;
            shoot = false;

            bg = new BulletGenerator(type,speed);
        }

        public void Update(Vector2 pos, Vector2 dir, Map map)
        {
            if (shoot)
            {
                bg.add = true;
                shoot = false;
            }

            bg.Update(pos, dir, map);
        }

        public void Draw(SpriteBatch sb)
        {
            bg.Draw(sb);
        }

        public void Shoot() { shoot = true; }

    }
}
