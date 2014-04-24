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

    public class Particle
    {
        #region Vars
        public bool alive;
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

        public Particle(int type, Vector2 position, Vector2 dir, float speed, float angle, float angularVelocity, Color color, float size, int ttl)
        {
            alive = true;
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
            if (TTL < 0) Die();

            position += dir * speed;

            angle += angularVelocity;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(ParticleTextures.GetTexture(type), position, new Rectangle(0, 0, width, height), color, angle, new Vector2(width / 2, height / 2), size, SpriteEffects.None, 0f);
        }

        public void Die() { alive = false; }
    }

    // ADD PARTICLES SYSTEMS
    //
    // Blood (sweat & tears)
    // Fire
    // Explosions
    // Smoke
    // Rain
    // Dust
    // Bullets
    // Teleportation particles
    // Electro
    // Much much more
    
    public class GravityParticle : Particle
    {
        #region Vars
        
        public bool gravityOn;
        public bool dissapearOnPoint;
        public Vector2 gravityPoint;
        public float gravityValue;

        #endregion Vars

        public GravityParticle(int type, Vector2 position, Vector2 dir, float speed, float angle, float angularVelocity, Color color, float size, int ttl,
            Vector2 gravityPoint, float gravityValue, bool gravityOn, bool dissapearOnPoint)
            : base(type, position, dir, speed, angle, angularVelocity, color, size, ttl)
        {
            this.gravityPoint = gravityPoint;
            this.gravityValue = gravityValue;
            this.gravityOn = gravityOn;
            this.dissapearOnPoint = dissapearOnPoint;
        }

        public void Update()
        {
            base.Update();

            if (position == gravityPoint && dissapearOnPoint) Die();

            Vector2 gravityDir = new Vector2(); gravityDir = gravityPoint; gravityDir.Normalize();
            if(gravityOn) position += gravityDir * gravityValue;
        }

    }
}
