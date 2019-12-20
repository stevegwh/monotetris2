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
        private Vector2 _vec = new Vector2(1f, 1f);
        private Texture2D sprite;

        public Block(Texture2D sprite)
        {
            this.sprite = sprite;
        }

        public Texture2D getSprite()
        {
            return sprite;
        }

        public Vector2 getPos()
        {
            return _vec;
        }

        public void setVec(Vector2 newVec)
        {
            _vec = newVec;
        }

        public void move(Vector2 toMove)
        {
            _vec += toMove;
        }

    }
}
