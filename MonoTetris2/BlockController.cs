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
        private int _count;
        private const int FallSpeed = 1;
        private int _fallSpeed;
        static public float TotalCountDuration = 1.0f;
        static public float CurrentCountDuration = TotalCountDuration;
        private float _countDuration = CurrentCountDuration;
        private float _currentTime;
        private bool _keyHasBeenPressed;

        private Texture2D sprite;
        private ActiveBlock _currentBlockPattern;

        // There needs to be a singular static grid to check things on.
        public BlockController(Texture2D sprite, ActiveBlock currentBlockPattern)
        {
            this.sprite = sprite;
            _count = 0;
            _currentBlockPattern = currentBlockPattern;
        }
        
        public bool SetNewBlock(ActiveBlock newBlock)
        {
            foreach (var ele in newBlock.GetPos())
            {
                if (IsValidMove(ele)) continue;
                return false;
            }
            
            _currentBlockPattern = newBlock;
            _count = 0;
            _active = true;
            return true;
        }

        public List<Vector2> GetPos()
        {
            return _currentBlockPattern.GetPos();
        }
        
        public Color GetColor()
        {
            return _currentBlockPattern.GetColor();
        }

        private bool IsValidMove(Vector2 toCheck)
        {
            if (toCheck.X >= MonoTetris2.Game1.WindowBounds.Right || toCheck.X < MonoTetris2.Game1.WindowBounds.Left 
                                                || toCheck.Y + sprite.Height > MonoTetris2.Game1.WindowBounds.Bottom)
                return false;
            foreach (List<Block> list in MonoTetris2.Game1.grid)
            {
                if (list.Count > 0)
                {
                    // If the block isn't in the same row then don't bother checking it
                    if ((int)list[0].GetPos().Y != (int)toCheck.Y)
                        continue;
                }
                foreach (Block blk in list)
                {
                    if (toCheck == blk.GetPos())
                        return false;
                }
                
            }

            return true;

        }
        public void Move(Vector2 toMove)
        {
            foreach (var ele in _currentBlockPattern.GetPos())
            {
                if (IsValidMove(ele + toMove)) continue;
                if (!(_currentTime < _countDuration)) _count++;
                return;
            }
            _count = 0;
            for(int i = 0; i < _currentBlockPattern.GetPos().Count; i++)
            {
                _currentBlockPattern.GetPos()[i] += toMove;
            }
        }

        // Draws the active block
        public void Draw(SpriteBatch spriteBatch, Texture2D sprite)
        {
            _currentBlockPattern.Draw(spriteBatch, sprite);
        }
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                if (!_keyHasBeenPressed)
                {
                    _keyHasBeenPressed = true;
                    _currentTime = 0;
                }

                _fallSpeed = 6;
            }
            else
            {
                _countDuration = CurrentCountDuration;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                if (!_keyHasBeenPressed)
                {
                    _keyHasBeenPressed = true;
                    _currentTime = 0;
                }

                _fallSpeed = 200;
            }
            else
            {
                _countDuration = CurrentCountDuration;
            }
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
            else if (Keyboard.GetState().IsKeyDown(Keys.Up) && !_keyHasBeenPressed)
            {
                List<Vector2> rotations = _currentBlockPattern.Rotate();
                List<Vector2> kickbacks = new List<Vector2>
                {
                    new Vector2(0, 0),
                    new Vector2(0, -sprite.Height),
                    new Vector2(sprite.Width, 0),
                    new Vector2(-sprite.Width, 0),
                    new Vector2(sprite.Width * 2, 0),
                    new Vector2(-sprite.Width * 2, 0)
                };

                // Tries rotating the block in place and if not possible tries one place to the left, to the right
                // etc (the kickbacks list) until it finds a valid place or not.
                foreach (Vector2 kickback in kickbacks)
                {
                    List<Vector2> validRotation = new List<Vector2>();
                    foreach (Vector2 rotation in rotations)
                    {
                        if (IsValidMove(rotation + kickback))
                        {
                            validRotation.Add(rotation + kickback);
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (validRotation.Count == 4)
                    {
                        _currentBlockPattern.SetPos(validRotation);
                        _keyHasBeenPressed = true;
                        return;
                    }
                }
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right) 
               && Keyboard.GetState().IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyUp(Keys.Down)
               && Keyboard.GetState().IsKeyUp(Keys.X))
            {
                _keyHasBeenPressed = false;
                _fallSpeed = FallSpeed;
            }
            _countDuration = CurrentCountDuration / _fallSpeed;
            _currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 
            if (!(_currentTime > _countDuration)) return;
            
            Move(new Vector2(0f, sprite.Height));
            _active = _count < 2;
            _currentTime -= _countDuration;
        }

        public bool IsActive()
        {
            return _active;
        }
    }
}
