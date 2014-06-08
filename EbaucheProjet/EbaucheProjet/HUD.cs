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
    class HUD
    {
        public SpriteFont font1;

        public int width;
        public int height;

        public Vector2 lifePos;


        public Vector2 FPSPos;
        public string FPS;
        public bool showFPS;

        public HUD()
        {
            height = 1280;
            width = 720;


            FPSPos = new Vector2(height - 50, 0);
            lifePos = new Vector2(18 , width - 50);
        }

        public void LoadTextures(ContentManager cm)
        {
            font1 = cm.Load<SpriteFont>("font1");
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(font1, "100%", lifePos, Color.Red);
            sb.DrawString(font1, FPS.ToString(), FPSPos, Color.Yellow);
        }

        public void Update(string FPS)
        {
            this.FPS = FPS;
            showFPS = true;
        }
    }
}
