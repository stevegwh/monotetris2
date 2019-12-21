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
    public class Block
    {

        private bool _active = true;
        private int _count = 0;
        private const float CountDuration = 1.0f;
        private float _countDuration = CountDuration;
        private float _currentTime;
        private bool _keyHasBeenPressed = false;

        private Texture2D sprite;
        private List<List<Vector2>> _currentBlock;
        private int _currentBlockIdx;
        

        // There needs to be a singular static grid to check things on.
        public Block(Texture2D sprite, List<List<Vector2>> currentBlock)
        {
            //_grid = grid;
            this.sprite = sprite;
            _count = 0;
            _currentBlockIdx = 0;
            _currentBlock = currentBlock;
        }

        public List<Vector2> GetPos()
        {
            return _currentBlock[_currentBlockIdx];
        }

        private bool IsValidMove(Vector2 toCheck, Rectangle windowBounds)
        {
            if (toCheck.X >= windowBounds.Right || toCheck.X < windowBounds.Left 
                                                || toCheck.Y + sprite.Height > windowBounds.Bottom)
                return false;
            foreach (Block blk in MonoTetris2.Game1.grid)
            {
                foreach (Vector2 pos in blk.GetPos())
                {
                    if (toCheck == pos)
                        return false;
                }
                
            }

            return true;

        }
        // Updates all blocks with the new position
        public void Move(Vector2 toMove, Rectangle windowBounds)
        {
            foreach (var ele in _currentBlock[_currentBlockIdx])
            {
                if (!IsValidMove(ele + toMove, windowBounds))
                {
                    if (!(_currentTime < _countDuration)) _count++;
                    return;
                }
            }
            _count = 0;
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
        public void Update(GameTime gameTime, Rectangle windowBounds)
        {
            _countDuration = Keyboard.GetState().IsKeyDown(Keys.Down) ? CountDuration / 4 : CountDuration;
            
                
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !_keyHasBeenPressed)
            {
                Move(new Vector2(sprite.Width, 0), windowBounds);
                _keyHasBeenPressed = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !_keyHasBeenPressed)
            {
                Move(new Vector2(-sprite.Width, 0), windowBounds);
                _keyHasBeenPressed = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_keyHasBeenPressed)
            {
                int toMoveIdx = (_currentBlockIdx + 1) % _currentBlock.Count;
                foreach (var ele in _currentBlock[toMoveIdx])
                {
                    if (!IsValidMove(ele,  windowBounds)) return;
                }

                _currentBlockIdx = toMoveIdx;
                _keyHasBeenPressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                _keyHasBeenPressed = false;
            }
                
            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 
            if (!(_currentTime > _countDuration)) return;
            
            Move(new Vector2(0f, sprite.Height), windowBounds);
            _active = _count < 2;
            _currentTime -= _countDuration;
        }

        public bool IsActive()
        {
            return _active;
        }
    }
}
