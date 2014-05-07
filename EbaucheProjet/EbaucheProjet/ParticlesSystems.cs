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
        public static Texture2D star; // 2
        public static Texture2D dot; // 3
        public static Texture2D bigCircle; // 4
        public static Texture2D rock; // 5
        public static Texture2D doubleCircle; // 5

        public static Texture2D GetTexture(int n)
        {
            switch (n)
            {
                case 0: return air; break;
                case 1: return circle; break;
                case 2: return star; break;
                case 3: return dot; break;
                case 4: return bigCircle; break;
                case 5: return rock; break;
                case 6: return doubleCircle; break;
                default: return air; break;
            }
        }

        public static void LoadTextures(ContentManager cm)
        {
            air = cm.Load<Texture2D>("particles/air");
            circle = cm.Load<Texture2D>("particles/circle");
            star = cm.Load<Texture2D>("particles/star");
            dot = cm.Load<Texture2D>("particles/dot");
            bigCircle = cm.Load<Texture2D>("particles/bigCircle");
            rock = cm.Load<Texture2D>("particles/rock");
            doubleCircle = cm.Load<Texture2D>("particles/doubleCircle");
        }
    }

    public class Particle
    {
        #region Vars

        public bool alive;
        public bool impact;

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
        public int bumpin;
        public int TTL;
        public int bumpPrecision;

        public Rectangle hitbox;

        #endregion Vars

        public Particle(int type, Vector2 position, Vector2 dir, float speed, float angle, float angularVelocity, Color color, float size, int bumpin, int ttl, int bumpPrecision)
        {
            this.alive = true;
            this.impact = false;
            
            this.type = type;
            width = ParticleTextures.GetTexture(type).Height;
            height = ParticleTextures.GetTexture(type).Width;

            this.position = position;
            this.dir = dir;
            this.speed = speed;
            this.angle = angle;
            this.angularVelocity = angularVelocity;
            this.color = color;
            this.size = size;
            this.bumpin = bumpin;
            this.TTL = ttl;
            this.bumpPrecision = bumpPrecision;

            hitbox = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);
        }

        public void MouvWithCollisions(Map map)
        {
            // Opti pour faire moins de tests de collision
            int imin, imax;
            imin = (int)position.X / map.terrain[0, 0].largeur - 2;
            imax = (int)position.X / map.terrain[0, 0].largeur + 2;

            if (imin < 0) imin = 0;
            if (imin > map.largeur) imin = map.largeur;

            if (imax < 0) imax = 0;
            if (imax > map.largeur) imax = map.largeur;


            int jmin, jmax;
            jmin = (int)position.Y / map.terrain[0, 0].largeur - 2;
            jmax = (int)position.Y / map.terrain[0, 0].largeur + 2;

            if (jmin < 0) jmin = 0;
            if (jmin > map.hauteur) jmin = map.hauteur;

            if (jmax < 0) jmax = 0;
            if (jmax > map.hauteur) jmax = map.hauteur;


            // Debut des smooth collisions
            // X
            float intervalle = speed / bumpPrecision;
            float s = 0;
            bool collision = false;

            while (s <= speed && !collision)
            {
                position.X += dir.X * intervalle;

                for (int i = imin; i < imax; i++)
                    for (int j = jmin; j < jmax; j++)
                        foreach (Rectangle r in map.terrain[i, j].hitbox)
                            if (r.Intersects(new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height))) collision = true;

                s += intervalle;
            }
            if (collision)
            {
                if (bumpin <= 0) { Impact(); position -= dir * intervalle; }
                if (bumpin > 0) { bumpin--; position -= dir * intervalle; dir.X = -dir.X; }
            }


            // Y
            s = 0;
            collision = false;

            while (s <= speed && !collision)
            {
                position.Y += dir.Y * intervalle;

                for (int i = imin; i < imax; i++)
                    for (int j = jmin; j < jmax; j++)
                        foreach (Rectangle r in map.terrain[i, j].hitbox)
                            if (r.Intersects(new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height))) collision = true;
                s += intervalle;
            }
            if (collision)
            {
                if (bumpin <= 0) { Impact(); position -= dir * intervalle; }
                if (bumpin > 0) { bumpin--; position -= dir * intervalle; dir.Y = -dir.Y; }
            }


            hitbox = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width, height);
        }

        public void Update(Map map)
        {
            TTL--;
            if (TTL < 0) Die();

            MouvWithCollisions(map);

            angle += angularVelocity;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(ParticleTextures.GetTexture(type), position, new Rectangle(0, 0, width, height), color, angle, new Vector2(width / 2, height / 2), size, SpriteEffects.None, 0f);
        }

        public void Die() { alive = false; }

        public void Impact()
        { 
            impact = true;
            Die();
        }
    }

    // Particles Ideas
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
}
