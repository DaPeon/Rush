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

        public int particlesPerSec;

        public ParticleEngine(Vector2 pos, int type) : this(pos, type, 25) { }

        public ParticleEngine(Vector2 pos, int type, int particlesPerSec)
        {
            this.pos = pos;
            this.type = type;
            this.particlesPerSec = particlesPerSec;

            particles = new List<Particle>();
        }

        public void Update() { Update(pos); }
        public void Update(Vector2 pos)
        {
            this.pos = pos;

            for (int i = 0; i < particlesPerSec; i++) particles.Add(NewParticle());

            /*
            foreach(Particle p in particles)
            {
                p.Update();
                if (p.TTL <= 0) particles.Remove(p);
            }*/

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update();
                if (particles[i].TTL <= 0) particles.Remove(particles[i]);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Particle p in particles)
            {
                p.Draw(sb);
            }
        }

        public Particle NewParticle()
        {
            Random r = new Random();

            return new Particle(type, // Type
                                pos, // Position
                                Vector2.Normalize(new Vector2((float)(r.NextDouble()*2 - 1), (float)(r.NextDouble()*2 - 1))), // Direction
                                (float)r.NextDouble()*3,
                                MathHelper.Pi, // Angle
                                0.1f * (float)(r.NextDouble() * 2 - 1), // Vitesse angulaire
                                new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble()), // Couleur
                                (float)r.NextDouble(), // Taille
                                20 + r.Next(40) // Temps de vie
                                );
        }
    }

    public class Particle
    {
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

        public Particle(int type, Vector2 position, Vector2 dir,float speed,
            float angle, float angularVelocity, Color color, float size, int ttl)
        {
            this.type = type;
            if (type == 1) width = 5; height = 5;

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
        public static Texture2D circle; // 1

        public static Texture2D GetTexture(int n)
        {
            switch (n)
            {
                case 1: return circle; break;
                default: return circle; break;
            }
        }

        public static void LoadTextures(ContentManager cm)
        {
            circle = cm.Load<Texture2D>("particles/circle");
        }
    }
}