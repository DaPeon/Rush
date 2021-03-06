﻿using System;
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
        #region Vars

        public Vector2 pos;
        public int type;
        public List<Particle> particles;
        public Random r;

        public bool on;

        public int particlesPerUp;

        #endregion Vars

        public ParticleEngine(Vector2 pos, int type) : this(pos, type, 10, false) { }

        public ParticleEngine(Vector2 pos, int type, int particlesPerUp, bool on)
        {
            this.pos = pos;
            this.type = type;
            this.particlesPerUp = particlesPerUp;

            this.on = on;

            r = new Random();
            particles = new List<Particle>();
        }

        public void Update(Map map) { Update(map, pos); }
        public void Update(Map map, Vector2 pos)
        {
            this.pos = pos;

            if(on) for (int i = 0; i < particlesPerUp; i++) particles.Add(NewParticle());

            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Update(map);
                if (!particles[i].alive) particles.Remove(particles[i]);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Particle p in particles) p.Draw(sb);
        }

        public void On() { on = true; }
        public void Off() { on = false; }

        public virtual Particle NewParticle()
        {
            Vector2 position = pos;
            Vector2 dir = Vector2.Normalize(new Vector2((float)(r.NextDouble() * 2 - 1), (float)(r.NextDouble() * 2 - 1)));
            float speed = (float)r.NextDouble() * 1.5f + 1f;
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(r.NextDouble() * 2 - 1);
            Color color = new Color((float)r.NextDouble() * 1f, (float)r.NextDouble() * 1f, (float)r.NextDouble() * 1f);
            float size = (float)r.NextDouble() * 1f + 0.3f;
            int ttl = 200 + r.Next(100);

            return new Particle(type, position, dir, speed, angle, angularVelocity, color, size, 10, ttl, 1);
        }

        public virtual void Impact()
        { }
    }
}