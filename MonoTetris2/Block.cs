using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    class Block
    {
        
        private Vector2 _vec = Vector2.Zero;
        private Texture2D sprite;
        private int _count;
        private float _countDuration = 1.0f;
        private float _currentTime;

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
            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 
            if (_currentTime > _countDuration)
            {
                //_count++;
                if (_vec.Y + sprite.Height < windowBounds.Bottom)
                {
                    Move(new Vector2(0f, sprite.Height));
                }
                _currentTime -= _countDuration;
            }
        }
    }
}
