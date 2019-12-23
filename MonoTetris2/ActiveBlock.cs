using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace MonoTetris2
{
    public class ActiveBlock
    {
        private List<Vector2> _positions;
        // The index of the point of origin in the array
        private int _origin;
        private Texture2D _sprite;

        public List<Vector2> GetPos()
        {
            return _positions;
        }
        
        public Texture2D GetSprite()
        {
            return _sprite;
        }

        public int GetOrig()
        {
            return _origin;
        }

        
        // Returns the block after it has been rotated. BlockController will check if this is a valid move
        // and if so it will then call "SetPos"
        public List<Vector2> Rotate()
        {
            // TODO: Straight line rotation is off
            if (_origin == -1) return _positions;
            List<Vector2> normalizedPositions = Normalize();
            Vector2 center = normalizedPositions[_origin];
            // Remove rotation point
            //normalizedPositions.RemoveAt(_origin);
            /*
            Vector2 center = (normalizedPositions[0] + normalizedPositions[1] + normalizedPositions[2] + normalizedPositions[3] )/4;
            center.X = (float)Math.Round(center.X);
            center.Y = (float)Math.Round(center.Y);
            */
            List<Vector2> toReturn = new List<Vector2>();
            foreach (Vector2 normalizedPosition in normalizedPositions)
            {
                // Do not bother rotating the pivot point
                if (normalizedPosition == center)
                {
                    toReturn.Add(_positions[_origin]);
                    continue;
                }
                
                Vector2 transformed = new Vector2();

                double angle = -45;
                double vertical = -Math.Tan(angle * 0.5f);
            
                // First shear.
                transformed.Y = (float)Math.Round(normalizedPosition.Y + vertical * (normalizedPosition.X - center.X));
            
                // Second shear.
                transformed.X = (float)Math.Round(normalizedPosition.X + Math.Sin(angle) * (transformed.Y - center.Y));
            
                // Third shear
                transformed.Y = (float)Math.Round(transformed.Y + vertical * (transformed.X - center.X));
            
                // Denormallize
                toReturn.Add(new Vector2(transformed.X * _sprite.Width, transformed.Y * _sprite.Height));
            }

            // Add center back in and de-normalize it
            /*
            toReturn.Insert(_origin,
                new Vector2(center.X * _sprite.Width, center.Y * _sprite.Height));
                */
            
            return toReturn;
        }

        // Changes exact screen position to grid numbers e.g. 1, 3 
        private List<Vector2> Normalize()
        {
            List<Vector2> toReturn = new List<Vector2>();
            foreach (Vector2 vec in _positions)
            {
                toReturn.Add(new Vector2(vec.X / _sprite.Width, vec.Y / _sprite.Height));
            }

            return toReturn;
        }

        public void SetPos(List<Vector2> newPos)
        {
            _positions = newPos;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D sprite)
        {
            spriteBatch.Begin();
            foreach (Vector2 vec in _positions)
            {
                spriteBatch.Draw(sprite, vec);
            }
            spriteBatch.End();
        }

        public ActiveBlock(List<Vector2> positions, int origin, Texture2D sprite)
        {
            _origin = origin;
            _positions = positions;
            _sprite = sprite;
        }
    }
}