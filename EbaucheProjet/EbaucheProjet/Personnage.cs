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
    public class Personnage : Sprite
    {
        public int speed; // Vitesse en pixel

        public int defaultSpeed; // Vitesse de base

        public Vector2 mov; // Mouvement a venir

        public Vector2 mid; // Centre du personnage ( sert d'axe pour la rotation)

        public Vector2 dir; // Direction du personnage

        public new void SetHitbox()
        {
            hitbox = new Rectangle((int)pos.X, (int)pos.Y, largeur , hauteur);            
        }

        public virtual void Mouv() // Fct mouvement
        {
            mov = Vector2.Zero;
        }

        public new void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, mid, new Rectangle(phase * largeur, 0, largeur, hauteur), Color.White, rotation, new Vector2(largeur / 2, hauteur / 2), scale, SpriteEffects.None, 0);
        }

        /*
        public void Update(GameTime gt, Vector2 lookedPoint) // Update position
        {
            base.Update(gt);

            mov = Mouv();
            

            pos += mov;
            mid = new Vector2(pos.X + (largeur / 2), pos.Y + (hauteur / 2)); // On definit le milieu du perso, vu qu'on a sa largeur/hauteur

            dir = lookedPoint; // On calcule la direction du regard, par rapport au point regardé
            dir.Normalize(); // On le normalise pour pas exploser

            rotation = (float)(Math.Atan2((double)dir.Y, (double)dir.X)) + MathHelper.Pi/2;


            if (mov == Vector2.Zero)
                nbPhases = 0;
            else
                nbPhases = nbPhasesTotal;
        }*/

        public new void Update(GameTime gt, Vector2 lookedPoint, Map map) // Update position
        {
            bool intersect = false;

            Mouv();

            // On sépare le mouvement en X et Y
            // En X
            pos.X += mov.X; // On bouge
            SetHitbox();

            for (int i = 0; i < map.largeur; i++)
                for (int j = 0; j < map.hauteur; j++)
                    if (hitbox.Intersects(map.terrain[i, j].hitbox)) { intersect = true; } // Si ça nous fait rentrer dans le mur

            if (intersect == true) pos.X -= mov.X; // On annule
            SetHitbox();

            // Et en Y
            pos.Y += mov.Y; // On bouge
            SetHitbox();

            for (int i = 0; i < map.largeur; i++)
                for (int j = 0; j < map.hauteur; j++)
                    if (hitbox.Intersects(map.terrain[i, j].hitbox)) { intersect = true; } // Si ça nous fait rentrer dans le mur

            if (intersect == true) pos.Y -= mov.Y; // On annule
            SetHitbox();

            // Fin des hitbox check


            mid = new Vector2(pos.X + (largeur / 2), pos.Y + (hauteur / 2)); // On definit le milieu du perso, vu qu'on a sa largeur/hauteur

            dir = lookedPoint - mid; // On calcule la direction du regard, par rapport au point regardé
            dir.Normalize(); // On le normalise pour pas exploser

            rotation = (float)(Math.Atan2((double)dir.Y, (double)dir.X)) + MathHelper.Pi / 2;

            if (mov == Vector2.Zero)
                nbPhases = 0;
            else
                nbPhases = nbPhasesTotal;
            
            base.Update(gt);
        }

        public new void Update(GameTime gt, Map map) // Update position
        {
            this.Update(gt, pos, map);
        }
        
        public new void LoadTextures(ContentManager cm, string textureName) // Load texture
        {
            base.LoadTextures(cm, textureName);

            mid = new Vector2(pos.X + largeur / 2, pos.Y + hauteur / 2);// On definit le milieu du perso, vu qu'on a sa largeur/hauteur
        }


        public Personnage(string name, Vector2 pos, int nbPhases) : base(name, pos, nbPhases, Color.White) // Constructeur (ne pas oublier de load la texture)
        {
            speed = 5;
            defaultSpeed = 5;

            mov = new Vector2(0, 0);
        }
    }
}
