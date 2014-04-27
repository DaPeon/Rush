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
        public ParticleEngine pEngine;
        public bool toRemove;
        public bool lastGenHappened;
        Weapons w;

        public Bullet(int type, Vector2 position, Vector2 dir, float speed, float size, Color color, int TTL, Weapons w)
            : base(type, position, dir, speed, 0f, 0f, color, size, TTL)
        {
            this.w = w;
            pEngine = Type.GetTypeOfEngine(w);
            toRemove = false;
            lastGenHappened = false;
        }

        
        public void Update(Map map)
        {
            if (!alive && !lastGenHappened)
            {
                pEngine.Impact();
                lastGenHappened = true;
            }

            pEngine.Update(map, position);

            if (!alive && lastGenHappened) pEngine.Off();

            if(alive) base.Update(map);

            if (pEngine.particles.Count == 0) toRemove = true;
        }

        public void Draw(SpriteBatch sb)
        {
            pEngine.Draw(sb);
            if (alive) base.Draw(sb);
        }
    }

    public class BulletGenerator
    {
        public bool add;
        public Vector2 pos;
        public List<Bullet> bullets;
        public List<Particle> impacts;
        public float speed;
        public int type;
        public int TTL;
        public Color color;
        public float size;
        public Weapons w;

        public BulletGenerator(int type, float speed, Color color, float size, int TTL, Weapons w)
        {
            pos = Vector2.Zero;
            bullets = new List<Bullet>();

            this.TTL = TTL;

            this.speed = speed;
            this.type = type;
            this.color = color;
            this.size = size;
            this.w = w;
        }

        public void Update(Vector2 pos, Vector2 dir, Map map)
        {
            this.pos = pos;

            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update(map);


                if (bullets[i].impact)
                {
                    // TODO
                    // Gérer des impacts
                }

                if (bullets[i].toRemove)
                {
                    bullets.Remove(bullets[i]);
                }
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
            return new Bullet(type ,pos, dir, speed, size, color, TTL, w);
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
        public Weapons w;

        public int bulletsInterval;
        public int lastBullet;


        public Weapon(int type, float speed, Color color, int bulletsInterval, float size, int TTL, Weapons w)
        {
            this.speed = speed;
            this.type = type;
            this.color = color;
            this.size = size;
            this.TTL = TTL;
            this.w = w;

            this.bulletsInterval = bulletsInterval;
            shoot = false;
            lastBullet = 0;

            bg = new BulletGenerator(type, speed, color,size, TTL, w);
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


    public enum Weapons
    {
        LanceBoule,
        Lazer
    }

    public static class Type
    {
        public static ParticleEngine GetTypeOfEngine(Weapons w)
        {
            switch (w)
            {
                case Weapons.LanceBoule: return (new LanceBouleParticle(Vector2.Zero));
                    break;
                case Weapons.Lazer: return (new LanceLazerParticle(Vector2.Zero));
                default: return null;
                    break;
            }
        }
    }

    // Weapon : BulletType, BulletSpeed, BulletColor, 1000/BulletsPerSecond, BulletSize, BulletLifeTime

    public class LanceBoule : Weapon
    {
        public LanceBoule() : base(4, 10, Color.Red, 1000 / 2, 1.3f, 500, Weapons.LanceBoule) { }
    }

    public class LanceBouleParticle : ParticleEngine
    {
        public LanceBouleParticle(Vector2 pos)
            : base(pos, 1, 25, true) { }

        public override Particle NewParticle()
        {
            Vector2 position = pos;
            Vector2 dir = Vector2.Normalize(new Vector2((float)(r.NextDouble() * 2 - 1), (float)(r.NextDouble() * 2 - 1)));
            float speed = (float)r.NextDouble() * 6f + 0f;
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(r.NextDouble() * 2 - 1);
            Color color = new Color((float)r.NextDouble() * 0.3f + 0.7f, (float)r.NextDouble() * 0.2f + 0.0f, 0f);
            float size = (float)r.NextDouble() * 0.7f + 0.3f;
            int ttl = 5 + r.Next(5);

            return new Particle(type, position, dir, speed, angle, angularVelocity, color, size, ttl);
        }

        public override void Impact()
        {
            particlesPerUp = 1000;
        }
    }

    public class LanceLazer : Weapon
    {
        public LanceLazer() : base(0, 10, Color.Blue, 1000 / 1, 1f, 500, Weapons.Lazer) { }
    }

    public class LanceLazerParticle : ParticleEngine
    {
        public LanceLazerParticle(Vector2 pos)
            : base(pos, 0, 5, true) { }

        public override Particle NewParticle()
        {
            Vector2 position = pos;
            Vector2 dir = Vector2.Normalize(new Vector2((float)(r.NextDouble() * 2 - 1), (float)(r.NextDouble() * 2 - 1)));
            float speed = (float)r.NextDouble() * 0.1f + 0f;
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(r.NextDouble() * 2 - 1);
            Color color = new Color(0, 0, (float)r.NextDouble() * 0.5f + 0.5f);
            float size = 1f;
            int ttl = 10 + r.Next(10);

            return new Particle(type, position, dir, speed, angle, angularVelocity, color, size, ttl);
        }

        public override void Impact()
        {
            particlesPerUp = 100;
        }
    }
}
