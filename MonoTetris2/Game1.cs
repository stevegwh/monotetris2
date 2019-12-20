using System.Collections.Generic;
using Game1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoTetris2
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Rectangle windowBounds;
        SpriteBatch spriteBatch;
        Block block;
        List<Block> grid = new List<Block>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 48 * 14;
            graphics.PreferredBackBufferWidth = 48 * 7;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            windowBounds = graphics.GraphicsDevice.Viewport.Bounds;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            block = new Block(this);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (block.IsActive())
            {
                block.Update(gameTime, windowBounds);
            }
            else
            {
                grid.Add(block);
                block = new Block(this);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            block.Draw(gameTime, spriteBatch);
            foreach (Block ele in grid)
            {
                ele.Draw(gameTime, spriteBatch);
            }

            base.Draw(gameTime);
        }
    }
}