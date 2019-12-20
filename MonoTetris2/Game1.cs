using System;
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
        private Texture2D sprite;
        private List<List<List<Vector2>>> _blocks = new List<List<List<Vector2>>>();
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
        
        private List<List<Vector2>> GetRandomBlock()
        {
            Random random = new Random();  
            int num = random.Next(_blocks.Count);
            List<List<Vector2>> newList = new List<List<Vector2>>(_blocks[0]);
            return newList;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sprite = this.Content.Load<Texture2D>("block");
            List<List<Vector2>> lineBlock = new List<List<Vector2>>();
            List<List<Vector2>> tBlock = new List<List<Vector2>>();
            List<List<Vector2>> sBlockR = new List<List<Vector2>>();
            List<List<Vector2>> sBlockL = new List<List<Vector2>>();
            List<List<Vector2>> squareBlock = new List<List<Vector2>>();
            lineBlock.Add(new List<Vector2> {
                new Vector2(sprite.Width * 1, 0), 
                new Vector2(sprite.Width * 1, sprite.Height * 1), 
                new Vector2(sprite.Width * 1, sprite.Height * 2), 
                new Vector2(sprite.Width * 1, sprite.Height * 3) 
            });
            
            lineBlock.Add(new List<Vector2> {
                new Vector2(0, sprite.Height * 1), 
                new Vector2(sprite.Width * 1, sprite.Height * 1), 
                new Vector2(sprite.Width * 2, sprite.Height * 1), 
                new Vector2(sprite.Width * 3, sprite.Height * 1) 
            });
            
            tBlock.Add(new List<Vector2> {
                new Vector2(0, sprite.Height * 1), 
                new Vector2(sprite.Width * 1, 0), 
                new Vector2(sprite.Width * 1, sprite.Height * 1), 
                new Vector2(sprite.Width * 1, sprite.Height * 2) 
            });
            
            tBlock.Add(new List<Vector2> {
                new Vector2(sprite.Width * 1, 0),
                new Vector2(0, sprite.Height * 1),
                new Vector2(sprite.Width * 1, sprite.Height * 1),
                new Vector2(sprite.Width * 2, sprite.Height * 1)
            });
            
            tBlock.Add(new List<Vector2> {
                new Vector2(sprite.Width * 2, sprite.Height * 1), 
                new Vector2(sprite.Width * 1, 0), 
                new Vector2(sprite.Width * 1, sprite.Height * 1), 
                new Vector2(sprite.Width * 1, sprite.Height * 2) 
            });
            
            tBlock.Add(new List<Vector2> {
                new Vector2(sprite.Width * 1, sprite.Height * 2),
                new Vector2(0, sprite.Height * 1),
                new Vector2(sprite.Width * 1, sprite.Height * 1),
                new Vector2(sprite.Width * 2, sprite.Height * 1)
            });
            
            squareBlock.Add(new List<Vector2> {
                new Vector2(0, 0), 
                new Vector2(0, sprite.Height * 1), 
                new Vector2(sprite.Width * 1, 0), 
                new Vector2(sprite.Width * 1, sprite.Height * 1) 
            });
            
            sBlockR.Add(new List<Vector2> {
                new Vector2(0, sprite.Height * 2), 
                new Vector2(0, sprite.Height * 1), 
                new Vector2(sprite.Width * 1, 0), 
                new Vector2(sprite.Width * 1, sprite.Height * 1) 
            });
            
            sBlockR.Add(new List<Vector2> {
                new Vector2(0, 0), 
                new Vector2(sprite.Width * 1, 0), 
                new Vector2(sprite.Width * 1, sprite.Height * 1), 
                new Vector2(sprite.Width * 2, sprite.Height * 1) 
            });
            
            sBlockL.Add(new List<Vector2> {
                new Vector2(sprite.Width * 2, 0), 
                new Vector2(sprite.Width * 1, 0), 
                new Vector2(0, sprite.Height * 1), 
                new Vector2(sprite.Width * 1, sprite.Height * 1) 
            });
            
            sBlockL.Add(new List<Vector2> {
                new Vector2(sprite.Width * 1, sprite.Height * 1), 
                new Vector2(sprite.Width * 1, sprite.Height * 2), 
                new Vector2(0, 0), 
                new Vector2(0, sprite.Height * 1) 
            });

            _blocks.Add(squareBlock);
            _blocks.Add(lineBlock);
            _blocks.Add(sBlockL);
            _blocks.Add(sBlockR);
            _blocks.Add(tBlock);
            block = new Block(sprite, GetRandomBlock());
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
                sprite = this.Content.Load<Texture2D>("block");
                block = new Block(sprite, GetRandomBlock());
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