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
    static class Geometric
    {

        public static Texture2D px;

        public static void Init(GraphicsDevice gd) // Init la texture pixel
        {
            px = new Texture2D(gd, 1, 1);
        }

        public static void SetPixel(SpriteBatch sb, GraphicsDevice gd, Vector2 pos, Color color) // Set pixel
        {
            px.SetData(new Color[] { color });
            sb.Draw(px, pos, color);
        }

        public static int Min(float[] f)
        {
            float min = f[0];
            int minI = 0;

            for (int i = 0; i < f.Length; i++)
            {
                if ((f[i] < min))
                {
                    min = f[i];
                    minI = i;
                }
            }

            return minI;
        }
    }
}
