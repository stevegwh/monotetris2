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
        private List<List<List<Vector2>>> _blockData = new List<List<List<Vector2>>>();
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
            // Line block
            _blockData.Add(new List<List<Vector2>> { 
                new List<Vector2> {
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(sprite.Width * 1, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, sprite.Height * 2), 
                    new Vector2(sprite.Width * 1, sprite.Height * 3) 
                },
                new List<Vector2> {
                    new Vector2(0, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, sprite.Height * 1), 
                    new Vector2(sprite.Width * 2, sprite.Height * 1), 
                    new Vector2(sprite.Width * 3, sprite.Height * 1) 
                }
            });
            
            //tblock
            _blockData.Add(new List<List<Vector2>> { 
                new List<Vector2> {
                    new Vector2(0, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(sprite.Width * 1, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, sprite.Height * 2) 
                },
                 new List<Vector2> {
                    new Vector2(sprite.Width * 1, 0),
                    new Vector2(0, sprite.Height * 1),
                    new Vector2(sprite.Width * 1, sprite.Height * 1),
                    new Vector2(sprite.Width * 2, sprite.Height * 1)
                },
                new List<Vector2> {
                    new Vector2(sprite.Width * 2, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(sprite.Width * 1, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, sprite.Height * 2) 
                },
                new List<Vector2> {
                    new Vector2(sprite.Width * 1, sprite.Height * 2),
                    new Vector2(0, sprite.Height * 1),
                    new Vector2(sprite.Width * 1, sprite.Height * 1),
                    new Vector2(sprite.Width * 2, sprite.Height * 1)
                }
            });
            
            // Square block
            _blockData.Add(new List<List<Vector2>> { 
                new List<Vector2> {
                    new Vector2(0, 0), 
                    new Vector2(0, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(sprite.Width * 1, sprite.Height * 1) 
                }
            });
            
            // sBlockR
            _blockData.Add(new List<List<Vector2>> { 
                new List<Vector2> {
                    new Vector2(0, sprite.Height * 2), 
                    new Vector2(0, sprite.Height * 1), 
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(sprite.Width * 1, sprite.Height * 1) 
                },
                new List<Vector2> {
                    new Vector2(0, 0), 
                    new Vector2(sprite.Width * 1, 0), 
                    new Vector2(sprite.Width * 1, sprite.Height * 1), 
                    new Vector2(sprite.Width * 2, sprite.Height * 1) 
                }
            });

            // sBlockL
            _blockData.Add(new List<List<Vector2>>
            {
                new List<Vector2>
                {
                    new Vector2(sprite.Width * 2, 0),
                    new Vector2(sprite.Width * 1, 0),
                    new Vector2(0, sprite.Height * 1),
                    new Vector2(sprite.Width * 1, sprite.Height * 1)
                },
                new List<Vector2>
                {
                    new Vector2(sprite.Width * 1, sprite.Height * 1),
                    new Vector2(sprite.Width * 1, sprite.Height * 2),
                    new Vector2(0, 0),
                    new Vector2(0, sprite.Height * 1)
                }
            });
            _blockController = new BlockController(sprite, GetRandomBlock());
            InitGrid();
        }
        
        // Must return by value
        private List<List<Vector2>> GetRandomBlock()
        {
            Random random = new Random();  
            int num = random.Next(_blockData.Count);
            List<List<Vector2>> newList = new List<List<Vector2>>();
            // Make deep copy
            foreach (List<Vector2> list in _blockData[num])
            {
                List<Vector2> copy = new List<Vector2>();
                foreach (Vector2 vec in list)
                {
                    copy.Add(new Vector2(vec.X, vec.Y));
                }
                newList.Add(copy);
            }
            return newList;
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

            _blockController.Draw(gameTime, spriteBatch);
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