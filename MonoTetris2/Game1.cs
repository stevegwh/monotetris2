﻿using System;
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
        private bool _gameOver;
        GraphicsDeviceManager graphics;
        static public Rectangle WindowBounds;
        private Rectangle _panelBounds;
        SpriteBatch spriteBatch;
        private int _level;
        private int _lines;
        private int _score;
        private Texture2D _sprite;
        private Texture2D _border;
        private SpriteFont _gameText;
        private List<ActiveBlock> _blockData = new List<ActiveBlock>();
        private List<ActiveBlock> _randomBlockBag = new List<ActiveBlock>();
        private BlockController _blockController;
        static public List<List<Block>> grid = new List<List<Block>>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = (48 * 20);
            graphics.PreferredBackBufferWidth = (48 * 15);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _sprite = Content.Load<Texture2D>("block");
            _border = Content.Load<Texture2D>("border");
            var win = graphics.GraphicsDevice.Viewport.Bounds;
            int offset = 12;
            WindowBounds = new Rectangle(win.Left, win.Top, (48 * 10), (48 * 20));
            _panelBounds = new Rectangle(WindowBounds.Right, WindowBounds.Top, 
                win.Width - WindowBounds.Width, win.Height - WindowBounds.Height);
            
            _gameText = Content.Load<SpriteFont>("gametext");
            BlockI lineBlock = new BlockI(
                new List<Vector2> {
                    new Vector2(_sprite.Width * 1, 0), 
                    new Vector2(_sprite.Width * 2, 0), 
                    new Vector2(_sprite.Width * 3, 0), 
                    new Vector2(_sprite.Width * 4, 0) 
                },
                1,
                _sprite,
                new Color(Color.Cyan, 1.0f)
            );
            
            ActiveBlock pBlock = new ActiveBlock(
                new List<Vector2> {
                    new Vector2(0, 0) ,
                    new Vector2(0, _sprite.Height * 1), 
                    new Vector2(_sprite.Width * 1, _sprite.Height * 1), 
                    new Vector2(_sprite.Width * 2, _sprite.Height * 1), 
                },
                2,
                _sprite,
                new Color(Color.Blue, 1.0f)
            );
            
            ActiveBlock sevenBlock = new ActiveBlock(
                new List<Vector2> {
                    new Vector2(_sprite.Width * 2, 0) ,
                    new Vector2(0, _sprite.Height * 1), 
                    new Vector2(_sprite.Width * 1, _sprite.Height * 1), 
                    new Vector2(_sprite.Width * 2, _sprite.Height * 1), 
                },
                2,
                _sprite,
                new Color(Color.Orange, 1.0f)
            );
            
            ActiveBlock tBlock = new ActiveBlock(
                new List<Vector2> {
                    new Vector2(_sprite.Width * 1, 0), 
                    new Vector2(0, _sprite.Height * 1), 
                    new Vector2(_sprite.Width * 1, _sprite.Height * 1), 
                    new Vector2(_sprite.Width * 2, _sprite.Height * 1) ,
                },
                2,
                _sprite,
                new Color(Color.Purple, 1.0f)
            );

            ActiveBlock squareBlock = new ActiveBlock(
                new List<Vector2> {
                    new Vector2(0, 0), 
                    new Vector2(0, _sprite.Height * 1), 
                    new Vector2(_sprite.Width * 1, 0), 
                    new Vector2(_sprite.Width * 1, _sprite.Height * 1) 
                },
                -1,
                _sprite,
                new Color(Color.Yellow, 1.0f)
            );
            
            BlockI sBlockR = new BlockI(
                new List<Vector2> {
                    new Vector2(_sprite.Width * 2, 0),
                    new Vector2(_sprite.Width * 1, 0),
                    new Vector2(0, _sprite.Height * 1),
                    new Vector2(_sprite.Width * 1, _sprite.Height * 1)
                },
                3,
                _sprite,
                new Color(Color.Lime, 1.0f)
            );

            BlockI sBlockL = new BlockI(
                new List<Vector2> {
                    new Vector2(0, 0),
                    new Vector2(_sprite.Width * 1, 0),
                    new Vector2(_sprite.Width * 1, _sprite.Height * 1),
                    new Vector2(_sprite.Width * 2, _sprite.Height * 1)
                },
                2,
                _sprite,
                new Color(Color.Red, 1.0f)
            );
            
            _blockData.Add(sevenBlock);
            _blockData.Add(pBlock);
            _blockData.Add(squareBlock);
            _blockData.Add(lineBlock);
            _blockData.Add(sBlockL);
            _blockData.Add(sBlockR);
            _blockData.Add(tBlock);
            

            GenerateRandomBag();
            _blockController = new BlockController(_sprite, GetRandomBlock());
            InitGrid();
        }

        void GenerateRandomBag()
        {
            foreach (var blk in _blockData)
            {
                _randomBlockBag.Add(blk);
            }
        }
        
        // Must return by value
        private ActiveBlock GetRandomBlock()
        {
            if (_randomBlockBag.Count <= 0) GenerateRandomBag();
            Random random = new Random();  
            int num = random.Next(_randomBlockBag.Count);
            var blk = _randomBlockBag[num];
            int orig = blk.GetOrig();
            List<Vector2> positions = new List<Vector2>(blk.GetPos());
            _randomBlockBag.RemoveAt(num);
            if (blk is BlockI)
            {
                return new BlockI(positions, orig, blk.GetSprite(), blk.GetColor());
            }
            else
            {
                return new ActiveBlock(positions, orig, blk.GetSprite(), blk.GetColor());
            }
        }

        private void InitGrid()
        {
            for (int i = 0; i < WindowBounds.Height/_sprite.Height; i++)
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
                if (list.Count == WindowBounds.Right / _sprite.Width)
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
                grid.IndexOf(e) * _sprite.Height))));

            if (toRemove.Count > 0)
            {
                UpdateScore(toRemove.Count);
            }
        }

        private void UpdateScore(int total)
        {
            _lines += total;
            int fours = total / 4;
            _score += 1200 * (_level + 1) * fours;
            total -= fours * 4;
            if (total == 3)
            {
                _score += 300 * (_level + 1);
            }
            else if (total == 2)
            {
                _score += 100 * (_level + 1);
            }
            else if (total == 1)
            {
                _score += 40 * (_level + 1);
            }

            var oldLevel = _level;
            _level = _lines > 10 ? _lines / 10 : 0;
            if (oldLevel < _level) _blockController.SetBaseFallSpeed(_blockController.GetBaseFallSpeed() + 1.0f);
        }

        protected override void Update(GameTime gameTime)
        {
            if (_gameOver) return;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_blockController.IsActive())
            {
                _blockController.Update(gameTime);
            }
            else
            {
                // Add all blocks to the grid
                foreach (Vector2 pos in _blockController.GetPos())
                {
                    int idx = (int) (pos.Y / _sprite.Height);
                    grid[idx].Add(new Block(pos: pos, sprite: _sprite, spriteBatch: spriteBatch, color: _blockController.GetColor()));
                }
                ClearLines();
                if (!_blockController.SetNewBlock(GetRandomBlock()))
                {
                    _gameOver = true;
                    Console.WriteLine("Game over!");
                }
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(_border, new Vector2(0, 0));
            spriteBatch.End();

            _blockController.Draw(spriteBatch, _sprite);
            foreach (List<Block> ele in grid)
            {
                foreach (Block blk in ele)
                {
                    blk.Draw();
                }
            }
            spriteBatch.Begin();
            spriteBatch.DrawString(_gameText, "Score: " + _score, 
                new Vector2(_panelBounds.Left + (float)_panelBounds.Width / 2, 48 * 3), Color.White);
            spriteBatch.DrawString(_gameText, "Level " + _level, 
                new Vector2(_panelBounds.Left + (float)_panelBounds.Width / 2, 48 * 4), Color.White);
            spriteBatch.DrawString(_gameText, "Lines: " + _lines, 
                new Vector2(_panelBounds.Left +(float)_panelBounds.Width / 2, 48 * 5), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}