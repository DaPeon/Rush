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

        public Bullet(int type, Vector2 position, Vector2 dir, float speed, float size, Color color, int bumpin, int TTL, Weapons w)
            : base(type, position, dir, speed, 0f, 0f, color, size, bumpin, TTL, 5)
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
        public int bumpin;
        public int TTL;
        public Color color;
        public float size;
        public Weapons w;

        public BulletGenerator(int type, float speed, Color color, float size,int bumpin, int TTL, Weapons w)
        {
            pos = Vector2.Zero;
            bullets = new List<Bullet>();

            this.TTL = TTL;

            this.speed = speed;
            this.type = type;
            this.color = color;
            this.size = size;
            this.bumpin = bumpin;
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
            return new Bullet(type ,pos, dir, speed, size, color, bumpin, TTL, w);
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


        public Weapon(int type, float speed, Color color, int bulletsInterval, float size, int bumpin, int TTL, Weapons w)
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

            bg = new BulletGenerator(type, speed, color,size, bumpin, TTL, w);
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
        Lazer,
        Caillou,
        Balle
    }

    public static class Type
    {
        public static ParticleEngine GetTypeOfEngine(Weapons w)
        {
            switch (w)
            {
                case Weapons.LanceBoule: return (new LanceBouleParticle(Vector2.Zero));break;
                case Weapons.Lazer: return (new LanceLazerParticle(Vector2.Zero));
                case Weapons.Caillou: return (new CaillouParticle(Vector2.Zero));
                case Weapons.Balle: return (new BalleParticle(Vector2.Zero));
                default: return null; break;
            }
        }
    }

    // Weapon : BulletType, BulletSpeed, BulletColor, 1000/BulletsPerSecond, BulletSize, BulletBumps ,BulletLifeTime, WeaponType

    public class LanceBoule : Weapon
    {
        public LanceBoule() : base(4, 10, Color.Red, 1000 / 1, 1.3f, 3, 500, Weapons.LanceBoule) { }
    }

    public class LanceBouleParticle : ParticleEngine
    {
        public LanceBouleParticle(Vector2 pos)
            : base(pos, 2, 25, true) { }

        public override Particle NewParticle()
        {
            Vector2 position = pos;
            Vector2 dir = Vector2.Normalize(new Vector2((float)(r.NextDouble() * 2 - 1), (float)(r.NextDouble() * 2 - 1)));
            float speed = (float)r.NextDouble() * 2f + 0f;
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(r.NextDouble() * 2 - 1);
            Color color = new Color((float)r.NextDouble() * 0.3f + 0.7f, (float)r.NextDouble() * 0.2f + 0.0f, 0f);
            float size = (float)r.NextDouble() * 0.7f + 0.3f;
            int ttl = 5 + r.Next(10);

            return new Particle(type, position, dir, speed, angle, angularVelocity, color, size, 0, ttl, 1);
        }

        public override void Impact()
        {
            particlesPerUp = 1000;
        }
    }


    public class Lazer : Weapon
    {
        public Lazer() : base(0, 10, Color.Blue, 1000 / 1000, 1f, 5, 500, Weapons.Lazer) { }
    }

    public class LanceLazerParticle : ParticleEngine
    {
        public float speedFactor;

        public LanceLazerParticle(Vector2 pos)
            : base(pos, 0, 5, true)
        {
            speedFactor = 0.4f;
        }

        public override Particle NewParticle()
        {
            Vector2 position = pos;
            Vector2 dir = Vector2.Normalize(new Vector2((float)(r.NextDouble() * 2 - 1), (float)(r.NextDouble() * 2 - 1)));
            float speed = (float)r.NextDouble() * speedFactor + 0f;
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(r.NextDouble() * 2 - 1);
            Color color = new Color(0, 0, (float)r.NextDouble() * 0.5f + 0.5f);
            float size = 1f;
            int ttl = 5 + r.Next(5);

            return new Particle(type, position, dir, speed, angle, angularVelocity, color, size, 0, ttl, 1);
        }

        public override void Impact()
        {
            particlesPerUp = 25;
            speedFactor = 4f;
        }
    }


    public class Caillou : Weapon
    {
        public Caillou() : base(5, 6, new Color(0.1f, 0.1f, 0.1f), 1000 / 4, 1f, 1, 500, Weapons.Caillou) { }
    }

    public class CaillouParticle : ParticleEngine
    {
        public CaillouParticle(Vector2 pos)
            : base(pos, 5, 1, true) { }

        public override Particle NewParticle()
        {
            Vector2 position = pos;
            Vector2 dir = Vector2.Normalize(new Vector2((float)(r.NextDouble() * 2 - 1), (float)(r.NextDouble() * 2 - 1)));
            float speed = (float)r.NextDouble() * 1.5f + 0.5f;
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(r.NextDouble() * 2 - 1);
            float temp = (float)r.NextDouble() * 0.2f;
            Color color = new Color(temp, temp, temp);
            float size = (float)r.NextDouble() * 0.3f + 0.1f;
            int ttl = 5 + r.Next(5);

            return new Particle(type, position, dir, speed, angle, angularVelocity, color, size, 0, ttl, 1);
        }

        public override void Impact()
        {
            particlesPerUp = 1000;
        }
    }


    public class Balle : Weapon
    {
        public Balle() : base(2, 15, Color.Black, 1000 / 4, 1f, 0, 500, Weapons.Balle) { }
    }

    public class BalleParticle : ParticleEngine
    {
        public bool smoke;

        public BalleParticle(Vector2 pos)
            : base(pos, 3, 10, true) { smoke = true; }

        public override Particle NewParticle()
        {
            Vector2 position = pos;
            Vector2 dir = Vector2.Normalize(new Vector2((float)(r.NextDouble() * 2 - 1), (float)(r.NextDouble() * 2 - 1)));
            float speed = (float)r.NextDouble() * 0.5f + 0.0f;
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(r.NextDouble() * 2 - 1);
            Color color;
            if (smoke) { float temp = (float)r.NextDouble() * 0.5f + 0.2f; color = new Color(temp, temp, temp); }
            else { color = new Color(1f, (float)r.NextDouble() * 0.5f + 0.2f, (float)r.NextDouble() * 0.2f); }
            float size = (float)r.NextDouble() * 2f + 0.5f;
            int ttl = 15 + r.Next(15);

            return new Particle(type, position, dir, speed, angle, angularVelocity, color, size, 0, ttl, 1);
        }

        public override void Impact()
        {
            particlesPerUp = 100;
            smoke = false;
        }
    }
}
