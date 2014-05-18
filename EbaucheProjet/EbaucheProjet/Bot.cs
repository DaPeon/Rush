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
        public Bot(string name, int life, Vector2 pos, Color couleur, int nbPhases) 
            : base(name, 100, pos, nbPhases)
        {
            largeur = 64;
            hauteur = 64;

            this.color = couleur;
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
