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
    public class PlayablePersonnage : Personnage
    {

        #region Vars

        public Keys h;
        public Keys g;
        public Keys b;
        public Keys d;

        public bool canShootLeft;
        public bool canShootRight;

        public float speedInterval;

        #endregion Vars


        public override void Mouv() // Fct mouvement
        {

            if (Keyboard.GetState().IsKeyDown(d) && mov.X < speed) mov.X += speedInterval; if (mov.X > 0 && !Keyboard.GetState().IsKeyDown(d)) mov.X -= speedInterval;
            if (Keyboard.GetState().IsKeyDown(g) && mov.X > -speed) mov.X -= speedInterval; if (mov.X < 0 && !Keyboard.GetState().IsKeyDown(g)) mov.X += speedInterval;
            if (Keyboard.GetState().IsKeyDown(b) && mov.Y < speed) mov.Y += speedInterval; if (mov.Y > 0 && !Keyboard.GetState().IsKeyDown(b)) mov.Y -= speedInterval;
            if (Keyboard.GetState().IsKeyDown(h) && mov.Y > -speed) mov.Y -= speedInterval; if (mov.Y < 0 && !Keyboard.GetState().IsKeyDown(h)) mov.Y += speedInterval;
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

        public override void Update(GameTime gt, Vector2 lookedPoint, Map map, Camera2D cam, List<Personnage> personnages)
        {
            base.Update(gt, lookedPoint, map, cam, personnages);
            if (Mouse.GetState().LeftButton == ButtonState.Pressed || Mouse.GetState().RightButton == ButtonState.Pressed) phase = nbPhasesTotal;
        }

        public PlayablePersonnage(string name, gamePersonnage type, int life, Vector2 pos, int nbPhases, Color couleur, Keys h, Keys g, Keys b, Keys d)
            : base(name, type, life, pos, nbPhases)
        {
            largeur = 64;
            hauteur = 64;
            
            this.color = couleur;

            this.h = h;
            this.g = g;
            this.b = b;
            this.d = d;

            speedInterval = 0.5f;
        }
    }
}
