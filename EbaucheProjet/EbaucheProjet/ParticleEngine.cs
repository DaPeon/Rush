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
    public class ParticleEngine
    {
        public Vector2 pos;
        public int type;
        public List<Particle> particles;
        public Random r;

        public bool on;

        public int particlesPerSec;

        public ParticleEngine(Vector2 pos, int type) : this(pos, type, 10, true) { }

        public ParticleEngine(Vector2 pos, int type, int particlesPerSec, bool on)
        {
            this.pos = pos;
            this.type = type;
            this.particlesPerSec = particlesPerSec;

            this.on = on;

            r = new Random();
            particles = new List<Particle>();
        }

        public void Update() { Update(pos); }
        public void Update(Vector2 pos)
        {
            this.pos = pos;

            if(on) for (int i = 0; i < particlesPerSec; i++) particles.Add(NewParticle());

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update();
                if (particles[i].TTL <= 0) particles.Remove(particles[i]);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Particle p in particles) p.Draw(sb);
        }

        public Particle NewParticle()
        {
            Vector2 position = pos;
            Vector2 dir = Vector2.Normalize(new Vector2((float)(r.NextDouble() * 2 - 1), (float)(r.NextDouble() * 2 - 1)));
            float speed = (float)r.NextDouble() * 3f + 0.2f;
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(r.NextDouble() * 2 - 1);
            Color color = new Color((float)r.NextDouble() * 255, (float)r.NextDouble() * 255, (float)r.NextDouble() * 255) * 0.5f;
            float size = (float)r.NextDouble() * 0.7f + 0.5f;
            int ttl = 100 + r.Next(50);

            return new Particle(type, position, dir, speed, angle, angularVelocity, color, size, ttl);
        }
    }

    public class Particle
    {
        #region Vars
        public int type;
        public int width;
        public int height;
        public Vector2 position;
        public Vector2 dir;
        public float speed;
        public float angle;
        public float angularVelocity;
        public Color color;
        public float size;
        public int TTL;
        #endregion Vars

        public Particle(int type, Vector2 position, Vector2 dir,float speed, float angle, float angularVelocity, Color color, float size, int ttl)
        {
            this.type = type;
            width = 5; height = 5;

            this.position = position;
            this.dir = dir;
            this.speed = speed;
            this.angle = angle;
            this.angularVelocity = angularVelocity;
            this.color = color;
            this.size = size;
            this.TTL = ttl;
        }

        public void Update()
        {
            TTL--;
            position += dir * speed;

            angle += angularVelocity;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(ParticleTextures.GetTexture(type), position, new Rectangle(0, 0, width, height), color, angle, new Vector2(width / 2, height / 2), size, SpriteEffects.None, 0f);
        }
    }

    public static class ParticleTextures
    {
        public static Texture2D air; // 0
        public static Texture2D circle; // 1
        public static Texture2D blood; // 2

        public static Texture2D GetTexture(int n)
        {
            switch (n)
            {
                case 0: return air; break;
                case 1: return circle; break;
                case 2: return blood; break;
                default: return air; break;
            }
        }

        public static void LoadTextures(ContentManager cm)
        {
            air = cm.Load<Texture2D>("particles/air");
            circle = cm.Load<Texture2D>("particles/circle");
            blood = cm.Load<Texture2D>("particles/blood");
        }
    }
}