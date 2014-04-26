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
        public Bullet(Vector2 position, Vector2 dir, float speed)
            : base(2, position, dir, speed, 0f, 0f, Color.White, 1f, 1000)
        { }
    }

    public class BulletGenerator
    {
        public bool shoot;
        public Vector2 pos;
        List<Bullet> bullets;
        public float speed;

        public BulletGenerator()
        {
            shoot = false;
            pos = Vector2.Zero;
            bullets = new List<Bullet>();

            speed = 100;
        }

        public void Update(Vector2 pos, Vector2 dir, Map map)
        {
            foreach (Bullet b in bullets)
            {
                b.Update(map);
            }

            if (shoot)
            {
                shoot = false;
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
            return new Bullet(pos, dir, speed);
        }
    }
}
