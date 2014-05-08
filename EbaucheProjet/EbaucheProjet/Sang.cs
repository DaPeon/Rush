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
    public class BloodParticle : Particle
    {
        public int timeToMove;
        public bool moving;

        public BloodParticle(int type, Vector2 position, Vector2 dir, float speed, float angle, float angularVelocity, Color color, float size, int bumpin, int ttl, int timeToMove, int bumpPrecision)
            : base(type, position, dir, speed, angle, angularVelocity, color, size, bumpin, ttl, bumpPrecision)
        {
            this.timeToMove = timeToMove;
            this.moving = true;
        }

        public override void Update(Map map)
        {
            TTL--;
            if (TTL < 0) Die();

            if (moving)
            {
                MouvWithCollisions(map);
                angle += angularVelocity;
            }
        }
    }

    public class Bleeding : ParticleEngine
    {
        public int timeToBleed;
        public bool makeBlood;
        public Vector2 bloodDir;

        public Bleeding(Vector2 pos, Vector2 bloodDir)
            : base(pos, 2, 25, true)
        {
            timeToBleed = 5;
            makeBlood = true;
            this.bloodDir = bloodDir;
        }

        public new void Update(Map map)
        {
            timeToBleed--;
            if (timeToBleed <= 0 && makeBlood) StopBleeding();
            foreach (BloodParticle b in particles) 
            {
                b.timeToMove--;
                if (b.timeToMove <= 0) b.moving = false;
            }
            base.Update(map);
        }

        public void StopBleeding()
        {
            makeBlood = false;
            particlesPerUp = 0;
        }


        public override Particle NewParticle()
        {
            Vector2 position = pos;
            Vector2 dir = -bloodDir + new Vector2((float)r.NextDouble()*0.4f - 0.2f, (float)r.NextDouble()*0.4f - 0.2f) * 1.0f;
            dir.Normalize();
            position += dir * 25f;
            float speed = (float)r.NextDouble() * 8f + 0f;
            float angle = 0f;
            float angularVelocity = 0.1f * (float)(r.NextDouble() * 2 - 1);
            Color color = new Color(0f, 0f, (float)r.NextDouble() * 0.2f + 0.8f);
            float size = (float)r.NextDouble() * 0.7f + 0.3f;
            int ttl = 5000 + r.Next(5000);
            int timeToMove = 5;

            return new BloodParticle(type, position, dir, speed, angle, angularVelocity, color, size, 0, ttl, timeToMove, 1);
        }
    }
}
