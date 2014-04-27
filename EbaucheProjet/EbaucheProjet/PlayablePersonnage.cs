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
    public class PlayablePersonnage : Personnage
    {

        #region Vars

        public Keys h;
        public Keys g;
        public Keys b;
        public Keys d;

        public bool canShootLeft;
        public bool canShootRight;

        #endregion Vars


        public override void Mouv() // Fct mouvement
        {
            //speed = (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) ? defaultSpeed*2: defaultSpeed;

            mov = Vector2.Zero; // Vecteur mouvement a 0

            mov.X += (Keyboard.GetState().IsKeyDown(d) ? speed : 0); //Droite
            mov.X -= (Keyboard.GetState().IsKeyDown(g) ? speed : 0); //Gauche
            mov.Y += (Keyboard.GetState().IsKeyDown(b) ? speed : 0); //Bas
            mov.Y -= (Keyboard.GetState().IsKeyDown(h) ? speed : 0); //Haut

            // Deplacement a la souris 
            /*
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
                mov += dir * speed;*/
        }

        public override void GetActions()
        {
            shootLeft = false;
            if (Mouse.GetState().LeftButton == ButtonState.Pressed && canShootLeft)
            {
                shootLeft = true;
                canShootLeft = false;
            }
            if (Mouse.GetState().LeftButton == ButtonState.Released && !canShootLeft) canShootLeft = true;


            shootRight = false;
            if (Mouse.GetState().RightButton == ButtonState.Pressed && canShootRight)
            {
                shootRight = true;
                canShootRight = false;
            }
            if (Mouse.GetState().RightButton == ButtonState.Released && !canShootRight) canShootRight = true;
        }

        public PlayablePersonnage(string name, Vector2 pos, int nbPhases, Color couleur, Keys h, Keys g, Keys b, Keys d) : base(name, pos, nbPhases)
        {
            largeur = 64;
            hauteur = 64;
            
            this.color = couleur;

            this.h = h;
            this.g = g;
            this.b = b;
            this.d = d;
        }
    }
}
