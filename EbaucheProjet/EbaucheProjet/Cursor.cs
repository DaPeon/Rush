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
    class Cursor // Curseur cool
    {
        #region Vars

        public Texture2D texture; // texture

        public Vector2 pos; // point haut gauche dans la fenetre
        
        public Vector2 globalMid; // point milieu dans le jeu

        public Color color; // Couleur du calque

        public int phase; // phase courante
        public int nbPhasesTotal; // nb de phases total
        public int nbPhases; // nb de phases 

        public int actualisation; // Tous les cb de temps on changera de phase (en ms)

        public float scale; // Scale, pour le redimentionnement
        public float rotation; // Rotation du sprite
        
        public int largeur;
        public int hauteur;
        
        public Vector2 mid;
        
        #endregion Vars


        public void Update(GameTime gt, Camera2D c)
        {
            phase = (((int)(gt.TotalGameTime.TotalMilliseconds / 100)) % (nbPhases - 1));

            mid.X = Mouse.GetState().X;
            mid.Y = Mouse.GetState().Y;

            pos.X = mid.X - largeur / 2;
            pos.Y = mid.Y - hauteur / 2;

            globalMid = Vector2.Transform(mid, Matrix.Invert(c.transform));

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
                color = Color.Blue;
            else if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                color = Color.Red;
            else
                color = Color.White;

        }

        public void LoadTextures(ContentManager cm, string textureName)
        {
            texture = cm.Load<Texture2D>(textureName);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, pos, new Rectangle(phase * largeur, 0, largeur, hauteur), color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public Cursor(Vector2 pos, int nbPhases, int size)
        {
            this.pos = pos;

            color = Color.White;

            phase = 1;
            this.nbPhases = nbPhases;
            this.nbPhasesTotal = nbPhases;
            actualisation = 100;

            scale = 1;
            rotation = 0;

            largeur = size;
            hauteur = size;
        }

        public Cursor() : this(Vector2.Zero, 6, 32) { }
        
    }
}
