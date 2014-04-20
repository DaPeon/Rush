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
    static class Options
    {

        public static void Init(GraphicsDeviceManager g)
        { 
            largeur = g.PreferredBackBufferWidth;
            hauteur = g.PreferredBackBufferHeight;

            dotLargeur = largeur / 10;
            dotHauteur = hauteur / 10;
        }

        public static int largeur;
        public static int hauteur;

        public static int dotLargeur;
        public static int dotHauteur;

        public static void PrintInfos(GraphicsDeviceManager g)
        {
            Console.WriteLine("# New resolution : (" + g.PreferredBackBufferWidth + ", " + g.PreferredBackBufferHeight + ")" );
        }

        public static void GetOptions(GraphicsDeviceManager g)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F12))
                g.ToggleFullScreen();

            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                g.PreferredBackBufferWidth = largeur;
                g.PreferredBackBufferHeight = hauteur;

                Console.WriteLine("# Default resolution");
                PrintInfos(g);
            }

            g.ApplyChanges();
        }
    }
}
