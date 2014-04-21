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

        PlayablePersonnage jacket; // mon perso
        Cursor cursor;

        Map gameMap;
        FPSCounter FPS;

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

            Geometric.Init(GraphicsDevice);
            Options.Init(graphics);

            camera = new Camera2D(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            jacket = new PlayablePersonnage("Jacket", new Vector2(-100,-100), 8, Color.White, Keys.Z, Keys.Q, Keys.S, Keys.D); // New bonhomme (jacket)
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
            jacket.LoadTextures(Content,"persoMapV2"); // Load la texture de jacket
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
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            camera.focus = ((3 * jacket.mid + cursor.globalMid) / 4);
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift)) camera.focus = cursor.globalMid;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl)) jacket.pos = cursor.globalMid - (new Vector2(jacket.largeur,jacket.hauteur))/2;

            camera.Update(gameTime);
            jacket.Update(gameTime, cursor.globalMid, gameMap, camera); // Jacket s'update
            cursor.Update(gameTime, camera);
            Options.GetOptions(graphics);
            particleEngine.Update(gameTime, jacket.mid);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Gray);
            
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, camera.transform);
                gameMap.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, null, null, null, null, camera.transform);
                jacket.Draw(spriteBatch); // Jacket se dessine
                particleEngine.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
                cursor.Draw(spriteBatch); // On affiche le curseur
            spriteBatch.End();


            base.Draw(gameTime);

            FPS.UpdateFPS(gameTime.TotalGameTime.Milliseconds);
            FPS.ShowFPS();
        }
    }
}
