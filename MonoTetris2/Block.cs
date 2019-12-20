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
        private int _count = 0;
        private const float CountDuration = 1.0f;
        private float _countDuration = CountDuration;
        private float _currentTime;
        private bool _keyHasBeenPressed = false;

        private Texture2D sprite;
        private List<List<Vector2>> _currentBlock;
        private int _currentBlockIdx;
        

        public Block(Texture2D sprite, List<List<Vector2>> currentBlock)
        {
            this.sprite = sprite;
            _count = 0;
            _currentBlockIdx = 0;
            _currentBlock = currentBlock;
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
