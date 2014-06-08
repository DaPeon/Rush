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
    public class Bot : Personnage
    {
        public List<Color> colorlist;

        public Bot(string name, gamePersonnage type, int life, Vector2 pos, int nbPhases) 
            : base(name, type, 100, pos, nbPhases)
        {
            largeur = 32;
            hauteur = 32;

            List<Color> colorlist = new List<Color>();
            //AddColors();

            Random r = new Random();

            color = new Color(233, 150, 122);//colorlist[r.Next(colorlist.Count)];
        }

        public void AddColors()
        {
            colorlist.Add(new Color(255, 228, 196));
            colorlist.Add(new Color(165, 42, 42));
            colorlist.Add(new Color(210, 105, 30));
            colorlist.Add(new Color(255, 127, 80));
            colorlist.Add(new Color(184, 134, 11));
            colorlist.Add(new Color(0, 100, 0));
            colorlist.Add(new Color(233, 150, 122));
            colorlist.Add(new Color(178, 34, 34));
            colorlist.Add(new Color(218, 165, 32));
            colorlist.Add(new Color(240, 230, 140));
            colorlist.Add(new Color(225, 228, 181));
            colorlist.Add(new Color(255, 165, 0));
            colorlist.Add(new Color(139, 69, 19));
            colorlist.Add(new Color(160, 82, 45));
            colorlist.Add(new Color(210, 180, 140));
            colorlist.Add(new Color(255, 99, 71));
            colorlist.Add(new Color(154, 205, 50));
            colorlist.Add(new Color(245, 222, 179));



            
        }

        public override void Update(GameTime gt, Vector2 lookedPoint, Map map, Camera2D cam, List<Personnage> personnages)
        {
            Vector2 toLook = new Vector2();

            foreach (Personnage p in personnages) if (p.name == "player1") toLook = p.mid;

            base.Update(gt, toLook, map, cam, personnages);
        }

        public new void Mouv()
        {
            mov = new Vector2(1, 0);
        }
    }
}
