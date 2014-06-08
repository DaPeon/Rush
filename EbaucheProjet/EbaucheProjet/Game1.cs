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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Camera2D camera; // La cam

        PlayablePersonnage player1; // mon perso

        List<Personnage> personnages;

        Cursor cursor;

        Map gameMap;
        FPSCounter FPS;
        HUD Hud;

        ParticleEngine particleEngine;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            
            //HARDCORE
            //IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferMultiSampling = true;

            graphics.ApplyChanges();

            Geometric.Init(GraphicsDevice);
            Options.Init(graphics);

            camera = new Camera2D(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            Hud = new HUD();

            player1 = new PlayablePersonnage("player1", 100, new Vector2(64, 64), 8, Color.White, Keys.Z, Keys.Q, Keys.S, Keys.D); // New bonhomme (jacket)
            personnages = new List<Personnage>();
            personnages.Add(player1);
            personnages.Add(new Bot("bot1", 100, new Vector2(64 * 11, 64 * 2), Color.Red, 8));


            cursor = new Cursor();

            particleEngine = new ParticleEngine(new Vector2(0, 0), 0);

            FPS = new FPSCounter();

            gameMap = new Map();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            // TODO: use this.Content to load your game content here
            
            //player1.LoadTextures(Content, "persoMapV2"); // Load la texture de jacket
            foreach (Personnage p in personnages) p.LoadTextures(Content, "persoMapV2");

            Hud.LoadTextures(Content);
            cursor.LoadTextures(Content,"CursorsW"); // Load les textures de la souris

            ParticleTextures.LoadTextures(Content);
            gameMap.Load(Content);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            #region Touches

            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) this.Exit(); // Exit

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) camera.focus = cursor.globalMid;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl)) personnages[0].pos = cursor.globalMid - (new Vector2(player1.largeur, player1.hauteur)) / 2;
            if (Keyboard.GetState().IsKeyDown(Keys.RightControl)) personnages[1].pos = cursor.globalMid - (new Vector2(player1.largeur, player1.hauteur)) / 2;

            if (Mouse.GetState().MiddleButton == ButtonState.Pressed) particleEngine.on = true;
            if (Keyboard.GetState().IsKeyDown(Keys.D0)) particleEngine.on = false;
            if (Keyboard.GetState().IsKeyDown(Keys.D1)) particleEngine.type = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.D2)) particleEngine.type = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.D3)) particleEngine.type = 2;
            if (Keyboard.GetState().IsKeyDown(Keys.D4)) particleEngine.type = 3;
            if (Keyboard.GetState().IsKeyDown(Keys.D5)) particleEngine.type = 4;
            if (Keyboard.GetState().IsKeyDown(Keys.D6)) particleEngine.type = 5;
            if (Keyboard.GetState().IsKeyDown(Keys.D7)) particleEngine.type = 6;

            #endregion Touches

            camera.focus = ((2 * player1.mid + cursor.globalMid) / 3);
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) camera.focus = cursor.globalMid;

            camera.Update(gameTime);
            cursor.Update(gameTime, camera);

            if (Keyboard.GetState().IsKeyDown(Keys.P)) return; // Pause

            foreach (Personnage p in personnages) p.Update(gameTime, cursor.globalMid, gameMap, camera, personnages);

            Hud.Update(FPS.GetFPS());
            Options.GetOptions(graphics);
            particleEngine.Update(gameMap, player1.mid);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should dr'aw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.transform);
                gameMap.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, null, null, null, null, camera.transform);
                foreach (Personnage p in personnages) p.Draw(spriteBatch);
                particleEngine.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            Hud.Draw(spriteBatch);
                cursor.Draw(spriteBatch); // On affiche le curseur
            spriteBatch.End();


            base.Draw(gameTime);

            FPS.UpdateFPS(gameTime.TotalGameTime.Milliseconds);
        }
    }
}
