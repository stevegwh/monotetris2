using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoTetris2;

namespace Game1
{
   // TODO: Make this intantiate once and just change what block is the current one. 
    public class BlockController
    {
        private bool _active = true;
        private int _count = 0;
        private const float CountDuration = 1.0f;
        private float _countDuration = CountDuration;
        private float _currentTime;
        private bool _keyHasBeenPressed = false;

        private Texture2D sprite;
        private List<List<Vector2>> _currentBlockPattern;
        private int _currentBlockPatternIdx;
        

        // There needs to be a singular static grid to check things on.
        public BlockController(Texture2D sprite, List<List<Vector2>> currentBlockPattern)
        {
            //_grid = grid;
            this.sprite = sprite;
            _count = 0;
            _currentBlockPatternIdx = 0;
            _currentBlockPattern = currentBlockPattern;
        }

        public List<Vector2> GetPos()
        {
            return _currentBlockPattern[_currentBlockPatternIdx];
        }

        private bool IsValidMove(Vector2 toCheck, Rectangle windowBounds)
        {
            if (toCheck.X >= windowBounds.Right || toCheck.X < windowBounds.Left 
                                                || toCheck.Y + sprite.Height > windowBounds.Bottom)
                return false;
            foreach (List<Block> list in MonoTetris2.Game1.grid)
            {
                foreach (Block blk in list)
                {
                    if (toCheck == blk.GetPos())
                        return false;
                }
                
            }

            return true;

        }
        // Updates all blocks with the new position
        public void Move(Vector2 toMove, Rectangle windowBounds)
        {
            foreach (var ele in _currentBlockPattern[_currentBlockPatternIdx])
            {
                if (!IsValidMove(ele + toMove, windowBounds))
                {
                    if (!(_currentTime < _countDuration)) _count++;
                    return;
                }
            }
            _count = 0;
            foreach (var ele in _currentBlockPattern)
            {
                for(int i=0;i<ele.Count;i++)
                {
                    ele[i] += toMove;
                }
            }
        }
        // Draws the active block
        // Could replace this by having the _blockData store Blocks not Vector2
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var ele in _currentBlockPattern[_currentBlockPatternIdx])
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
                int toMoveIdx = (_currentBlockPatternIdx + 1) % _currentBlockPattern.Count;
                foreach (var ele in _currentBlockPattern[toMoveIdx])
                {
                    if (!IsValidMove(ele,  windowBounds)) return;
                }

                _currentBlockPatternIdx = toMoveIdx;
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
