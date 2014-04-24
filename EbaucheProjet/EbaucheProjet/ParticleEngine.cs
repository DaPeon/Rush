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
    public class ParticleEngine<T>
    {
        #region Vars

        public Vector2 pos;
        public int type;
        public List<Particle> particles;
        public Random r;

        public bool on;

        public int particlesPerSec;

        #endregion Vars

        public ParticleEngine(Vector2 pos, int type) : this(pos, type, 100, true) { }

        public ParticleEngine(Vector2 pos, int type, int particlesPerSec, bool on)
        {
            this.pos = pos;
            this.type = type;
            this.particlesPerSec = particlesPerSec;

            this.on = on;

            r = new Random();
            particles = new List<T>();
        }

        public void Update() { Update(pos); }
        public void Update(Vector2 pos)
        {
            this.pos = pos;

            if(on) for (int i = 0; i < particlesPerSec; i++) particles.Add(NewParticle());

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update();
                if (!particles[i].alive) particles.Remove(particles[i]);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (T p in particles) p.Draw(sb);
        }

        public Particle NewParticle()
        {
            Vector2 position = pos;
            Vector2 dir = Vector2.Normalize(new Vector2((float)(r.NextDouble() * 2 - 1), (float)(r.NextDouble() * 2 - 1)));
            float speed = (float)r.NextDouble() * 0.2f + 1.2f;
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(r.NextDouble() * 2 - 1);
            Color color = new Color((float)r.NextDouble(), (float)r.NextDouble(), (float)r.NextDouble());
            float size = (float)r.NextDouble() * 0.7f + 0.3f;
            int ttl = 100 + r.Next(50);

            return new T(type, position, dir, speed, angle, angularVelocity, color, size, ttl);
        }
    }
}