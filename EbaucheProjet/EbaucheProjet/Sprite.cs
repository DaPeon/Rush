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
    public abstract class Sprite
    {

        #region Vars

        public string name; // nom du sprite
        public Texture2D texture; // texture
        public Vector2 pos; // point haut gauche dans le jeu
        public Color color; // Couleur du calque
        public List<Rectangle> hitbox;

        public int phase; // phase courante
        public int nbPhasesTotal; // nb de phases total
        public int nbPhases; // nb de phases 
        public int actualisation; // Tous les cb de temps on changera de phase (en ms)

        public float scale; // Scale, pour le redimentionnement
        public float rotation; // Rotation du sprite

        public int largeur;
        public int hauteur;

        #endregion Vars


        public void Update(GameTime gt) // Update position
        {
            phase = (((int)(gt.TotalGameTime.TotalMilliseconds / 100)) % (nbPhases-1) );
            SetHitbox();
        }

        public void SetHitbox()
        {
            hitbox = new List<Rectangle>();
            hitbox.Add(new Rectangle((int)pos.X, (int)pos.Y, largeur, hauteur));
        }

        public void LoadTextures(ContentManager cm, string textureName)
        {
            texture = cm.Load<Texture2D>(textureName);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, pos, new Rectangle(phase * largeur, 0, largeur, hauteur), color, rotation, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public Sprite(string name,Vector2 pos, int nbPhases, Color color)
        {
            this.name = name;
            this.pos = pos;

            this.color = color;

            hitbox = new List<Rectangle>();

            phase = 0;
            this.nbPhases = nbPhases;
            this.nbPhasesTotal = nbPhases;
            actualisation = 100;
            
            scale = 1;
            rotation = 0;
        }
        public Sprite() : this("", Vector2.Zero, 0, Color.White) { }
        public Sprite(Vector2 pos) : this("", pos, 0, Color.White) { }
    }
}
