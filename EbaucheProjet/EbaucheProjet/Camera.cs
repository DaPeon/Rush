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
using System.Text;

namespace EbaucheProjet
{
    public class Camera
    {
        public Matrix transform; // Matrix Transform

        public Vector2 pos; // Camera Position

        public Vector2 mov;

        public int speed;

        public float rotation;

        public float zoom; // Camera Zoom

        public Camera()
        {
            zoom = 1.0f;
            rotation = 0.0f;
            pos = Vector2.Zero;

            speed = 3;
            mov = Vector2.Zero;
        }



        public Matrix Transformation()
        {/*
            return      Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 0)) *
                        Matrix.CreateRotationZ(rotation) *
                        Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                        Matrix.CreateTranslation(new Vector3(gd.Viewport.Width * 0.5f, gd.Viewport.Height * 0.5f, 0));*/

            return     Matrix.CreateTranslation(new Vector3(pos, 0.0f)) *
                       Matrix.CreateRotationZ(rotation) *
                       Matrix.CreateScale(zoom, zoom, 1.0f);// *
                       Matrix.CreateTranslation(new Vector3(Vector2.Zero,0.0f));
        }

        public void Update(GameTime gt, PlayablePersonnage perso)
        {
            mov = Vector2.Zero;
            
            mov.X -= (Keyboard.GetState().IsKeyDown(Keys.NumPad6) ? speed : 0); //Droite
            mov.X += (Keyboard.GetState().IsKeyDown(Keys.NumPad4) ? speed : 0); //Gauche
            mov.Y -= (Keyboard.GetState().IsKeyDown(Keys.NumPad2) ? speed : 0); //Bas
            mov.Y += (Keyboard.GetState().IsKeyDown(Keys.NumPad8) ? speed : 0); //Haut

            rotation += (Keyboard.GetState().IsKeyDown(Keys.NumPad9)) ? 0.05f : 0; // Rotation horaire
            rotation -= (Keyboard.GetState().IsKeyDown(Keys.NumPad7)) ? 0.05f : 0; // Anti-horaire

            zoom += (Keyboard.GetState().IsKeyDown(Keys.NumPad3)) ? 0.05f : 0; // Zoom
            zoom -= (Keyboard.GetState().IsKeyDown(Keys.NumPad1)) ? 0.05f : 0; // Dézoom

            pos += mov;

            transform = Transformation();
        }

    }

    public class Camera2D // Camera utilisée
    {
        #region Vars

        public Vector2 pos;
        public Vector2 mov;
        public Vector2 focus;
        public Vector2 screenCenter;
        public float height;
        public float width;
        public Vector2 origin;

        public float rotation;
        public float scale;
        public float speed;
        
        public Matrix transform;

        #endregion Vars

        public Camera2D(int width, int height) 
        {
            this.width = width;
            this.height = height;

            Initialize();
        }

        public void Initialize()
        {
            screenCenter = new Vector2(width / 2, height / 2);
            origin = screenCenter;
            focus = origin;

            rotation = 0f;

            scale = 1;
            speed = 5;
        }

        public bool Mouv()
        {

            Vector2 mov = Vector2.Zero;

            mov.X += (Keyboard.GetState().IsKeyDown(Keys.NumPad6) ? 2*speed : 0); //Droite
            mov.X -= (Keyboard.GetState().IsKeyDown(Keys.NumPad4) ? 2*speed : 0); //Gauche
            mov.Y += (Keyboard.GetState().IsKeyDown(Keys.NumPad2) ? 2*speed : 0); //Bas
            mov.Y -= (Keyboard.GetState().IsKeyDown(Keys.NumPad8) ? 2*speed : 0); //Haut

            pos += mov;

            rotation += (Keyboard.GetState().IsKeyDown(Keys.NumPad9)) ? 0.05f : 0; // Rotation horaire
            rotation -= (Keyboard.GetState().IsKeyDown(Keys.NumPad7)) ? 0.05f : 0; // Anti-horaire

            scale += (Keyboard.GetState().IsKeyDown(Keys.NumPad3)) ? 0.05f : 0; // Zoom
            scale -= (Keyboard.GetState().IsKeyDown(Keys.NumPad1)) ? 0.05f : 0; // Dézoom

            if (Keyboard.GetState().IsKeyDown(Keys.Multiply)) Initialize();

            if (
                Keyboard.GetState().IsKeyDown(Keys.NumPad1) || 
                Keyboard.GetState().IsKeyDown(Keys.NumPad2) || 
                Keyboard.GetState().IsKeyDown(Keys.NumPad3) || 
                Keyboard.GetState().IsKeyDown(Keys.NumPad4) || 
                Keyboard.GetState().IsKeyDown(Keys.NumPad5) || 
                Keyboard.GetState().IsKeyDown(Keys.NumPad6) || 
                Keyboard.GetState().IsKeyDown(Keys.NumPad7) ||
                Keyboard.GetState().IsKeyDown(Keys.NumPad8) ||
                Keyboard.GetState().IsKeyDown(Keys.Multiply)||
                Keyboard.GetState().IsKeyDown(Keys.LeftControl) ||
                Keyboard.GetState().IsKeyDown(Keys.NumPad9))
                {return true;}
            else
                return false;
        }

        public void Update(GameTime gameTime)
        {
            transform = Matrix.Identity *
                        Matrix.CreateTranslation(-pos.X, -pos.Y, 0) *
                        Matrix.CreateRotationZ(rotation) *
                        Matrix.CreateTranslation(origin.X, origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(scale, scale, scale));

            origin = screenCenter / scale;

            if (!Mouv())
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                pos.X += (focus.X - pos.X) * speed * delta;
                pos.Y += (focus.Y - pos.Y) * speed * delta;
            }
        }
    }
}
