using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Block
    {

        private bool _active = true;
        private Vector2 _vec = Vector2.Zero;
        private Texture2D sprite;
        private int _count = 0;
        private const float CountDuration = 1.0f;
        private float _countDuration = CountDuration;
        private float _currentTime;
        private bool _keyHasBeenPressed = false;

        private List<List<Vector2>> _currentBlock;
        private List<List<List<Vector2>>> _blocks;
        private int _currentBlockIdx;
        

        public Block(MonoTetris2.Game1 game)
        {
            List<List<Vector2>> lineBlock = new List<List<Vector2>>();
            List<List<Vector2>> tBlock = new List<List<Vector2>>();
            List<List<Vector2>> sBlockR = new List<List<Vector2>>();
            List<List<Vector2>> sBlockL = new List<List<Vector2>>();
            List<List<Vector2>> squareBlock = new List<List<Vector2>>();
            sprite = game.Content.Load<Texture2D>("block");
            _count = 0;
            _currentBlockIdx = 0;
            
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

            /*
            _blocks.Add(squareBlock);
            _blocks.Add(lineBlock);
            _blocks.Add(tBlock);
            _blocks.Add(sBlockL);
            _blocks.Add(sBlockR);
            */

            _currentBlock = sBlockL;
        }

        private List<List<Vector2>> GetRandomBlock()
        {
            Random random = new Random();  
            int num = random.Next(_blocks.Count);
            return _blocks[num];
        }

        // Updates all blocks with the new position
        public void Move(Vector2 toMove)
        {
            foreach (var ele in _currentBlock)
            {
                for(int i=0;i<ele.Count;i++)
                {
                    ele[i] += toMove;
                }
            }
            
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var ele in _currentBlock[_currentBlockIdx])
            {
                spriteBatch.Draw(sprite, ele);
            }
            spriteBatch.End();
        }

        private bool ValidDownMove(Rectangle windowBounds)
        {
            foreach (var ele in _currentBlock[_currentBlockIdx])
            {
                if (ele.Y + sprite.Height >= windowBounds.Bottom)
                    return false;
            }

            return true;
        }
        
        private bool ValidSideMove(Rectangle windowBounds, int move)
        {
            foreach (var ele in _currentBlock[_currentBlockIdx])
            {
                if (ele.X + move >= windowBounds.Right || ele.X + move < windowBounds.Left)
                    return false;
            }

            return true;
        }

        public void Update(GameTime gameTime, Rectangle windowBounds)
        {
            _countDuration = Keyboard.GetState().IsKeyDown(Keys.Down) ? CountDuration / 4 : CountDuration;
            
                
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !_keyHasBeenPressed)
            {
                if (ValidSideMove(windowBounds, sprite.Width))
                    Move(new Vector2(sprite.Width, 0));
                _keyHasBeenPressed = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !_keyHasBeenPressed)
            {
                if(ValidSideMove(windowBounds, -sprite.Width))
                    Move(new Vector2(-sprite.Width, 0));
                _keyHasBeenPressed = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_keyHasBeenPressed)
            {
                _currentBlockIdx = (_currentBlockIdx + 1) % _currentBlock.Count;
                _keyHasBeenPressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                _keyHasBeenPressed = false;
            }
                
            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 
            if (!(_currentTime > _countDuration)) return;
            if (ValidDownMove(windowBounds))
            {
                Move(new Vector2(0f, sprite.Height));
                _count = 0;
            }
            else
            {
                _count++;
            }

            _active = _count < 2;
            _currentTime -= _countDuration;
        }

        public bool IsActive()
        {
            return _active;
        }
    }
}
