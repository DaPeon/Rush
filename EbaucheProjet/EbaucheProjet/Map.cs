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
    public class Map
    {
        public Tile[,] terrain;

        public int largeur;
        public int hauteur;

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    terrain[i, j].Draw(sb);
                }
            }
        }

        public void Load(ContentManager cm)
        {
            Textures.Load(cm);
        }

        public Map()
        {
            int [,] toLoad = 
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,1,0,0,0,0,0,0,0,0,1},
                {1,0,1,0,1,0,1,0,1,1,1,1,0,1},
                {1,0,1,0,1,0,1,0,1,0,0,0,0,1},
                {1,0,1,0,0,0,1,0,1,0,1,1,0,1},
                {1,0,0,0,0,1,1,0,1,0,0,1,0,1},
                {1,0,0,0,0,0,0,0,1,0,0,1,0,1},
                {1,0,0,0,0,1,0,0,0,0,0,1,0,1},
                {1,1,0,1,1,1,1,1,1,0,1,1,0,1},
                {1,0,0,1,0,0,0,0,1,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,1,0,0,0,0,1},
                {1,0,0,1,1,1,0,1,1,0,0,1,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1}
            };
            
            largeur = toLoad.GetLength(0);
            hauteur = toLoad.GetLength(1);

            terrain = new Tile[largeur, hauteur];
            
            for (int i = 0; i < largeur; i++)
            {
                for (int j = 0; j < hauteur; j++)
                {
                    terrain[i, j] = new Tile(i, j, toLoad[i, j]);
                }
            }
        }
    }

    public class Tile
    {
        public int x;
        public int y;

        public int largeur;
        public int hauteur;

        public int type;

        public List<Rectangle> hitbox;


        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Textures.GetTexture(type), (new Rectangle(x * largeur, y * hauteur, largeur, hauteur)), null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.1f);
        }

        public Tile(int x, int y, int type)
        {
            this.x = x;
            this.y = y;

            largeur = 64;
            hauteur = 64;

            hitbox = new List<Rectangle>();

            this.type = type;

            SetHitbox();
        }

        public void SetHitbox()
        {
            if(type == 1) hitbox.Add(new Rectangle(x * largeur, y * hauteur, largeur, hauteur));
        }
    }

    public static class Textures
    {
        #region Vars

        public static Texture2D ground;             // 0
        public static Texture2D full;               // 1
        public static Texture2D wallV;              // 2
        public static Texture2D wallH;              // 3

        public static Texture2D upleftcorner;       // 4
        public static Texture2D downleftcorner;     // 5
        public static Texture2D downrightcorner;    // 6
        public static Texture2D uprightcorner;      // 7

        #endregion Vars


        public static Texture2D GetTexture(int n)
        {
            switch (n)
            {
                case 0: return ground; break;
                case 1: return full; break;
                case 2: return wallV; break;
                case 3: return wallH; break;
                case 4: return uprightcorner; break;
                case 5: return downrightcorner; break;
                case 6: return downleftcorner; break;
                case 7: return upleftcorner; break;

                default: return ground; break;
            }
        }

        public static void Load(ContentManager cm)
        {
            ground = cm.Load<Texture2D>("tiles/ground");
            full = cm.Load<Texture2D>("tiles/full");
            wallV = cm.Load<Texture2D>("tiles/wallV");
            wallH = cm.Load<Texture2D>("tiles/wallH");

            upleftcorner = cm.Load<Texture2D>("tiles/upleftcorner");
            downleftcorner = cm.Load<Texture2D>("tiles/downleftcorner");
            downrightcorner = cm.Load<Texture2D>("tiles/downrightcorner");
            uprightcorner = cm.Load<Texture2D>("tiles/uprightcorner");
        }
    }
}
