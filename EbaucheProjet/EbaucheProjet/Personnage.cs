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
    public enum gamePersonnage
	{
        player,
        alien
	}

    public class Personnage : Sprite
    {

        #region Vars

        public gamePersonnage type;

        public float speed; // Vitesse en pixel
        public Vector2 mov; // Mouvement a venir
        public Vector2 mid; // Centre du personnage ( sert d'axe pour la rotation)
        public Vector2 dir; // Direction du personnage

        public int life;
        public bool alive;

        public bool shootLeft; public Vector2 leftPos;
        public bool shootRight; public Vector2 RightPos;
        public Weapon weaponLeft;
        public Weapon weaponRight;

        public List<Bleeding> bleedingList;

        #endregion Vars


        public virtual void Mouv() // Fct mouvement
        {
            mov = Vector2.Zero;
        }

        public new void Draw(SpriteBatch sb)
        {
            weaponLeft.Draw(sb);
            weaponRight.Draw(sb);
            sb.Draw(texture, mid, new Rectangle(phase * largeur, 0, largeur, hauteur), color, rotation, new Vector2(largeur / 2, hauteur / 2), scale, SpriteEffects.None, 0);

            foreach (Bleeding b in bleedingList) b.Draw(sb);
        }
        
        public override void SetHitbox()
        {
            hitbox = new List<Rectangle>();
            switch (type)
            {
                case gamePersonnage.player: hitbox.Add(new Rectangle((int)pos.X + 19, (int)pos.Y + 19, largeur - 2 * 19, hauteur - 2 * 19));
                    break;
                case gamePersonnage.alien: hitbox.Add(new Rectangle((int)pos.X, (int)pos.Y,  largeur, hauteur));
                    break;
                default:
                    break;
            }
        }

        public void MouvWithHitBoxes(Map map)
        {
            bool intersect = false;

            // On sépare le mouvement en X et Y
            // En X
            float x = 0;
            while (x != (int)mov.X && !intersect)
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

                x += ((int)mov.X>0)?1:-1;
            }
            // Et en Y
            intersect = false;
            float y= 0;
            while (y != (int)mov.Y && !intersect)
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

                y += ((int)mov.Y > 0) ? 1 : -1;
            }
            // Fin des hitbox check
        }

        public virtual void GetActions()
        { }

        public void takeDamage(int damage, Vector2 hitPos, Vector2 hitDir)
        {
            life -= damage;
            if (life <= 0)
            { alive = false; }

            bleedingList.Add(new Bleeding(hitPos, -hitDir, color));
        }

        public virtual void Update(GameTime gt, Vector2 lookedPoint, Map map, Camera2D cam, List<Personnage> personnages) // Update position
        {
            Mouv();
            
            MouvWithHitBoxes(map);

            mid = new Vector2(pos.X + (largeur / 2), pos.Y + (hauteur / 2)); // On definit le milieu du perso, vu qu'on a sa largeur/hauteur

            dir = lookedPoint - mid; // On calcule la direction du regard, par rapport au point regardé
            dir.Normalize(); // On le normalise pour pas exploser

            rotation = (float)(Math.Atan2((double)dir.Y, (double)dir.X)) + MathHelper.Pi / 2;


            Vector2 dirL = new Vector2(); Vector2 dirR = new Vector2();
            dirL = lookedPoint - leftPos;
            dirL.Normalize();
            dirR = lookedPoint - RightPos;
            dirR.Normalize();
            
            GetActions();

            if (shootLeft) weaponLeft.Shoot();
            weaponLeft.Update(gt, mid + dir * (largeur / 2), dir, map, this, personnages);
            
            if (shootRight) weaponRight.Shoot();
            weaponRight.Update(gt, mid + dir * (largeur / 2), dir, map, this, personnages);

            foreach (Bleeding b in bleedingList) b.Update(map);

            if (mov == Vector2.Zero)
                nbPhases = 0;
            else
                nbPhases = nbPhasesTotal;

            base.Update(gt);
        }

        public void Update(GameTime gt, Map map, Camera2D cam, List<Personnage> personnages) // Update position
        {
            this.Update(gt, pos, map, cam, personnages);
        }

        public string GetTextureName(gamePersonnage type)
        {
            switch (type)
            {
                case gamePersonnage.player: return "persoMapV2";
                case gamePersonnage.alien: return "ennemy";
                default: return "persoMapV2";
            }
        }

        public void LoadTextures(ContentManager cm) // Load texture
        {
            base.LoadTextures(cm, GetTextureName(type));

            mid = new Vector2(pos.X + largeur / 2, pos.Y + hauteur / 2);// On definit le milieu du perso, vu qu'on a sa largeur/hauteur
        }


        public Personnage(string name, gamePersonnage type, int life, Vector2 pos, int nbPhases) : base(name, pos, nbPhases, Color.White) // Constructeur (ne pas oublier de load la texture)
        {
            speed = 3;

            this.life = life;
            alive = true;

            weaponLeft = new Balle();
            weaponRight = new LanceBoule();

            shootLeft = false;
            shootRight = false;

            mov = new Vector2(0, 0);

            leftPos = new Vector2(0, 0);
            RightPos = new Vector2(0, 0);

            this.type = type;

            bleedingList = new List<Bleeding>();
        }
    }
}
