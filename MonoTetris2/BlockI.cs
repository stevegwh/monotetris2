using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTetris2
{
    public class BlockI : ActiveBlock
    {
        private bool _horizontal;
        public override List<Vector2> Rotate()
        {
            List<Vector2> normalizedPositions = Normalize();
            Vector2 center = normalizedPositions[_origin];
            List<Vector2> toReturn = new List<Vector2>();
            foreach (Vector2 normalizedPosition in normalizedPositions)
            {
                float x2;
                float y2;
                if (_horizontal)
                {
                    x2 = center.X + center.Y - normalizedPosition.Y;
                    y2 = normalizedPosition.X + center.Y - center.X;
                }
                else
                {
                    x2 = (normalizedPosition.Y + center.X - center.Y);
                    y2 = (center.X + center.Y - normalizedPosition.X);
                    
                }
                
                toReturn.Add(new Vector2(x2 * _sprite.Width, y2 * _sprite.Height));
            }
            _horizontal = !_horizontal;
            return toReturn;
        }


        public BlockI(List<Vector2> positions, int origin, Texture2D sprite, Color color)
            : base(positions, origin, sprite, color)
        { }
    }
}