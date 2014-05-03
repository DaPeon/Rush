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

        #region Vars

        public int speed; // Vitesse en pixel
        public int defaultSpeed; // Vitesse de base
        public Vector2 mov; // Mouvement a venir
        public Vector2 mid; // Centre du personnage ( sert d'axe pour la rotation)
        public Vector2 dir; // Direction du personnage

        public bool shootLeft; public Vector2 leftPos;
        public bool shootRight; public Vector2 RightPos;
        public Weapon weaponLeft;
        public Weapon weaponRight;

        #endregion Vars


        public virtual void Mouv() // Fct mouvement
        {
            mov = Vector2.Zero;
        }

        public new void Draw(SpriteBatch sb)
        {
            weaponLeft.Draw(sb);
            weaponRight.Draw(sb);
            sb.Draw(texture, mid, new Rectangle(phase * largeur, 0, largeur, hauteur), Color.White, rotation, new Vector2(largeur / 2, hauteur / 2), scale, SpriteEffects.None, 0);
        }

        public void SetHitbox()
        {
            hitbox = new List<Rectangle>();
            hitbox.Add(new Rectangle((int)pos.X + 19, (int)pos.Y + 19, 43 - 19, 43 - 19));
        }

        public void MouvWithHitBoxes(Map map)
        {
            bool intersect = false;

            // On sépare le mouvement en X et Y
            // En X
            float x = 0;
            while (x != mov.X && !intersect)
            {
                pos.X += x; // On bouge
                SetHitbox();

                foreach (Rectangle r in hitbox)
                    for (int i = 0; i < map.largeur; i++)
                        for (int j = 0; j < map.hauteur; j++)
                            foreach (Rectangle r2 in map.terrain[i, j].hitbox)
                                if (r.Intersects(r2)) { intersect = true; } // Si ça nous fait rentrer dans le mur

                if (intersect == true) pos.X -= x; // On annule
                SetHitbox();

                x += (mov.X>0)?1:-1;
            }
            // Et en Y
            intersect = false;
            float y= 0;
            while (y != mov.Y && !intersect)
            {
                pos.Y += y; // On bouge
                SetHitbox();

                foreach (Rectangle r in hitbox)
                    for (int i = 0; i < map.largeur; i++)
                        for (int j = 0; j < map.hauteur; j++)
                            foreach (Rectangle r2 in map.terrain[i, j].hitbox)
                                if (r.Intersects(r2)) { intersect = true; } // Si ça nous fait rentrer dans le mur

                if (intersect == true) pos.Y -= y; // On annule
                SetHitbox();

                y += (mov.Y > 0) ? 1 : -1;
            }
            // Fin des hitbox check
        }

        public virtual void GetActions()
        { }

        public virtual void Update(GameTime gt, Vector2 lookedPoint, Map map, Camera2D cam) // Update position
        {
            Mouv();
            
            MouvWithHitBoxes(map);

            mid = new Vector2(pos.X + (largeur / 2), pos.Y + (hauteur / 2)); // On definit le milieu du perso, vu qu'on a sa largeur/hauteur

            dir = lookedPoint - mid; // On calcule la direction du regard, par rapport au point regardé
            dir.Normalize(); // On le normalise pour pas exploser

            rotation = (float)(Math.Atan2((double)dir.Y, (double)dir.X)) + MathHelper.Pi / 2;


            leftPos = mid - (new Vector2(20, 0));
            RightPos = mid + (new Vector2(20, 0));
            Vector2 dirL = new Vector2(); Vector2 dirR = new Vector2();

            dirL = lookedPoint - leftPos;
            dirL.Normalize();
            dirR = lookedPoint - RightPos;
            dirR.Normalize();
            
            GetActions();

            if (shootLeft) weaponLeft.Shoot();
            weaponLeft.Update(gt, mid, dir, map);
            
            if (shootRight) weaponRight.Shoot();
            weaponRight.Update(gt, mid, dir, map);



            if (mov == Vector2.Zero)
                nbPhases = 0;
            else
                nbPhases = nbPhasesTotal;

            base.Update(gt);
        }

        public void Update(GameTime gt, Map map, Camera2D cam) // Update position
        {
            this.Update(gt, pos, map, cam);
        }
        
        public void LoadTextures(ContentManager cm, string textureName) // Load texture
        {
            base.LoadTextures(cm, textureName);

            mid = new Vector2(pos.X + largeur / 2, pos.Y + hauteur / 2);// On definit le milieu du perso, vu qu'on a sa largeur/hauteur
        }


        public Personnage(string name, Vector2 pos, int nbPhases) : base(name, pos, nbPhases, Color.White) // Constructeur (ne pas oublier de load la texture)
        {
            speed = 3;
            defaultSpeed = speed;

            weaponLeft = new Balle();
            weaponRight = new LanceBoule(); 

            shootLeft = false;
            shootRight = false;

            mov = new Vector2(0, 0);

            leftPos = new Vector2(0, 0);
            RightPos = new Vector2(0, 0);
        }
    }
}
