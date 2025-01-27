using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoTetris2
{
    public class ActiveBlock
    {
        protected List<Vector2> Positions;
        // The index of the point of origin in the array
        protected int _origin;
        protected Texture2D _sprite;
        private Color _color;

        public List<Vector2> GetPos()
        {
            return Positions;
        }
        
        public Texture2D GetSprite()
        {
            return _sprite;
        }
        
        public Color GetColor()
        {
            return _color;
        }

        public int GetOrig()
        {
            return _origin;
        }

        // Returns the block after it has been rotated. BlockController will check if this is a valid move
        // and if so it will then call "SetPos"
        public virtual List<Vector2> Rotate()
        {
            // -1 is a flag to not rotate, square uses this.
            if (_origin == -1) return Positions;
            List<Vector2> normalizedPositions = Normalize();
            Vector2 center = normalizedPositions[_origin];
            List<Vector2> toReturn = new List<Vector2>();
            foreach (Vector2 normalizedPosition in normalizedPositions)
            {
                float x2 = (normalizedPosition.Y + center.X - center.Y);
                float y2 = (center.X + center.Y - normalizedPosition.X);
                
                toReturn.Add(new Vector2(x2 * _sprite.Width, y2 * _sprite.Height));
            }
            return toReturn;
        }

        // Changes exact screen position to grid numbers e.g. 1, 3 
        protected List<Vector2> Normalize()
        {
            List<Vector2> toReturn = new List<Vector2>();
            foreach (Vector2 vec in Positions)
            {
                toReturn.Add(new Vector2(vec.X / _sprite.Width, vec.Y / _sprite.Height));
            }

            return toReturn;
        }

        public void SetPos(List<Vector2> newPos)
        {
            Positions = newPos;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D sprite)
        {
            spriteBatch.Begin();
            foreach (Vector2 vec in Positions)
            {
                spriteBatch.Draw(sprite, vec, _color);
            }
            spriteBatch.End();
        }

        public ActiveBlock(List<Vector2> positions, int origin, Texture2D sprite, Color color)
        {
            _origin = origin;
            Positions = positions;
            _sprite = sprite;
            _color = color;
        }
    }
}