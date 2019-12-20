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
        private float _countDuration = 1.0f;
        private float _currentTime;
        private bool _keyHasBeenPressed = false;

        public Block(MonoTetris2.Game1 game)
        {
            sprite = game.Content.Load<Texture2D>("block");
            _count = 0;
        }

        public Texture2D GetSprite()
        {
            return sprite;
        }

        public Vector2 GetPos()
        {
            return _vec;
        }

        public void SetVec(Vector2 newVec)
        {
            _vec = newVec;
        }

        public void Move(Vector2 toMove)
        {
            _vec += toMove;
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(sprite, _vec);
            spriteBatch.End();
        }

        public void Update(GameTime gameTime, Rectangle windowBounds)
        {
            if (!_active) return;
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && !_keyHasBeenPressed)
            {
                Move(new Vector2(sprite.Width, 0));
                _keyHasBeenPressed = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Left) && !_keyHasBeenPressed)
            {
                Move(new Vector2(-sprite.Width, 0));
                _keyHasBeenPressed = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                _keyHasBeenPressed = false;
            }
                
            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 
            if (!(_currentTime > _countDuration)) return;
            //_count++;
            if (_vec.Y + sprite.Height < windowBounds.Bottom)
            {
                Move(new Vector2(0f, sprite.Height));
            }
            else
            {
                _count++;
            }

            _active = _count < 1;
            _currentTime -= _countDuration;
        }
    }
}
