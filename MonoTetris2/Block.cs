using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTetris2
{
    public struct Block
    {
        private Texture2D _sprite;
        private Vector2 _pos;
        private SpriteBatch _spriteBatch;

        public Vector2 GetPos()
        {
            return _pos;
        }
            
        public void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(_sprite, _pos);
            _spriteBatch.End();
        }

        public Block(Texture2D sprite, Vector2 pos, SpriteBatch spriteBatch)
        {
            _sprite = sprite;
            _pos = pos;
            _spriteBatch = spriteBatch;
        }
    }
}