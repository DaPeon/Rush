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
        public Bullet(int type, Vector2 position, Vector2 dir, float speed, Color color ,int TTL)
            : base(type, position, dir, speed, 0f, 0f, color, 1f, TTL)
        { }
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

        public BulletGenerator(int type, float speed, Color color ,int TTL)
        {
            pos = Vector2.Zero;
            bullets = new List<Bullet>();

            this.TTL = TTL;

            this.speed = speed;
            this.type = type;
            this.color = color;
        }

        public void Update(Vector2 pos, Vector2 dir, Map map)
        {
            this.pos = pos;

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(map);

                if (bullets[i].TTL <= 0) bullets.Remove(bullets[i]);
            }

            if (add)
            {
                bullets.Add(NewBullet(dir));
            }
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(sb);
            }
        }

        public Bullet NewBullet(Vector2 dir)
        {
            return new Bullet(type ,pos, dir, speed, color, TTL);
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

        public Weapon(int type, float speed, Color color, int TTL)
        {
            this.speed = speed;
            this.type = type;
            this.color = color;
            this.TTL = TTL;
            shoot = false;

            bg = new BulletGenerator(type, speed, color, TTL);
        }

        public void Update(Vector2 pos, Vector2 dir, Map map)
        {
            Console.WriteLine(bg.bullets.Count); // REMOVE

            bg.add = shoot;
            if (shoot)
            {
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

    public class LanceBoule : Weapon
    {
        public LanceBoule() : base(2, 8, Color.Red, 100) { }
    }
}
