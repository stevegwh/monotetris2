using System;
using System.Collections.Generic;
using System.Linq;
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
        private List<ActiveBlock> _blockData = new List<ActiveBlock>();
        private BlockController _blockController;
        //static public List<Block> grid = new List<Block>();
        static public List<List<Block>> grid = new List<List<Block>>();

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
            windowBounds = graphics.GraphicsDevice.Viewport.Bounds;
            base.Initialize();
        }
        

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sprite = this.Content.Load<Texture2D>("block");
            ActiveBlock lineBlock = new ActiveBlock(
                new List<Vector2> {
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(sprite.Width * 1, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, sprite.Height * 2), 
                    new Vector2(sprite.Width * 1, sprite.Height * 3) 
                },
                2,
                sprite
            );
            
            ActiveBlock pBlock = new ActiveBlock(
                new List<Vector2> {
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(sprite.Width * 2, 0) ,
                    new Vector2(sprite.Width * 1, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, sprite.Height * 2), 
                },
                2,
                sprite
            );
            
            ActiveBlock sevenBlock = new ActiveBlock(
                new List<Vector2> {
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(0, 0) ,
                    new Vector2(sprite.Width * 1, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, sprite.Height * 2), 
                },
                2,
                sprite
            );
            
            ActiveBlock tBlock = new ActiveBlock(
                new List<Vector2> {
                    new Vector2(0, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(sprite.Width * 1, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, sprite.Height * 2) ,
                },
                2,
                sprite
            );

            ActiveBlock squareBlock = new ActiveBlock(
                new List<Vector2> {
                    new Vector2(0, 0), 
                    new Vector2(0, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(sprite.Width * 1, sprite.Height * 1) 
                },
                -1,
                sprite
            );
            
            ActiveBlock sBlockR = new ActiveBlock(
                new List<Vector2> {
                    new Vector2(0, sprite.Height * 2), 
                    new Vector2(0, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(sprite.Width * 1, sprite.Height * 1) 
                },
                1,
                sprite
            );

            ActiveBlock sBlockL = new ActiveBlock(
                new List<Vector2> {
                    new Vector2(sprite.Width * 2, 0),
                    new Vector2(sprite.Width * 1, 0),
                    new Vector2(0, sprite.Height * 1),
                    new Vector2(sprite.Width * 1, sprite.Height * 1)
                },
                1,
                sprite
            );
            
            _blockData.Add(lineBlock);
            /*
            _blockData.Add(pBlock);
            _blockData.Add(sevenBlock);
            _blockData.Add(tBlock);
            _blockData.Add(squareBlock);
            _blockData.Add(sBlockL);
            _blockData.Add(sBlockR);
            */

            _blockController = new BlockController(sprite, GetRandomBlock());
            InitGrid();
        }
        
        // Must return by value
        private ActiveBlock GetRandomBlock()
        {
            Random random = new Random();  
            int num = random.Next(_blockData.Count);
            ActiveBlock randomBlock = _blockData[num];
            int orig = randomBlock.GetOrig();
            List<Vector2> positions = new List<Vector2>(randomBlock.GetPos());
            ActiveBlock newBlock = new ActiveBlock(positions, orig, randomBlock.GetSprite());
            return newBlock;
        }

        private void InitGrid()
        {
            for (int i = 0; i < windowBounds.Bottom/sprite.Height; i++)
            {
                grid.Add(new List<Block>());
            }
        }

        private void ClearLines()
        {
            List<int> toRemove = new List<int>();
            foreach (List<Block> list in grid)
            {
                // Console.WriteLine(grid[key].Count == windowBounds.Right / sprite.Width);
                if (list.Count == windowBounds.Right / sprite.Width)
                {
                    toRemove.Add(grid.IndexOf(list));
                }
            }

            foreach (int idx in toRemove)
            {
                grid.RemoveAt(idx);
                grid.Insert(0, new List<Block>());
            }
            
            // Update Y position of every block
            grid.ForEach(e=> e.ForEach(f=> f.UpdatePos( new Vector2(f.GetPos().X,
                grid.IndexOf(e) * sprite.Height))));

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_blockController.IsActive())
            {
                _blockController.Update(gameTime, windowBounds);
            }
            else
            {
                // Add all blocks to the grid
                foreach (Vector2 pos in _blockController.GetPos())
                {
                    int idx = (int) (pos.Y / sprite.Height);
                    grid[idx].Add(new Block(pos: pos, sprite: sprite, spriteBatch: spriteBatch));
                }
                ClearLines();
                _blockController = new BlockController(sprite, GetRandomBlock());
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _blockController.Draw(spriteBatch, sprite);
            foreach (List<Block> ele in grid)
            {
                foreach (Block blk in ele)
                {
                    blk.Draw();
                }
            }

            base.Draw(gameTime);
        }
    }
}