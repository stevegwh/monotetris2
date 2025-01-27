using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoTetris2;

namespace Game1
{
   // TODO: Make this intantiate once and just change what block is the current one. 
    public class BlockController
    {
        private bool _dropResetNeeded;
        private bool _rotateKeyDown;
        private bool _moveKeyDown;
        
        private bool _moving = true;
        private int _blockStickTimer; // Time until a block 'sticks' after reaching a destination
        private const int BLOCK_STICK_THRESHOLD = 2;
        private const int FALL_SPEED = 1;
        private int _fallSpeed;
        private const float FALL_TIME_THRESHOLD = 1.0f;
        private const float MOVE_TIME_THRESHOLD = 0.15f;
        private float _fallTimer;
        private float _moveTimer;

        private Texture2D _sprite;
        private ActiveBlock _currentBlockPattern;

        // There needs to be a singular static grid to check things on.
        public BlockController(Texture2D sprite, ActiveBlock currentBlockPattern)
        {
            _sprite = sprite;
            _blockStickTimer = 0;
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
            _blockStickTimer = 0;
            _moving = true;
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
                                                || toCheck.Y + _sprite.Height > MonoTetris2.Game1.WindowBounds.Bottom)
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
        
        private void LateralMove(Vector2 toMove)
        {
            foreach (var ele in _currentBlockPattern.GetPos())
            {
                if (!IsValidMove(ele + toMove)) return;
            }
            for(int i = 0; i < _currentBlockPattern.GetPos().Count; i++)
            {
                _currentBlockPattern.GetPos()[i] += toMove;
            }
        }
        
        private void Drop(Vector2 toMove)
        {
            foreach (var ele in _currentBlockPattern.GetPos())
            {
                if (IsValidMove(ele + toMove)) continue;
                if (!( _fallTimer < FALL_TIME_THRESHOLD)) _blockStickTimer++;
                return;
            }
            _blockStickTimer = 0;
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
            var keyboardState = Keyboard.GetState();
            var fallKeyPressed = false;

            if (keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.X))
                _dropResetNeeded = false;

            if (!_dropResetNeeded)
            {
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    HandleKeyPress(6);
                    fallKeyPressed = true;
                }

                if (keyboardState.IsKeyDown(Keys.X))
                {
                    HandleKeyPress(200);
                    fallKeyPressed = true;
                }
            }

            _moveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.Up) && !_rotateKeyDown)
            {
                TryRotateBlock();
                _rotateKeyDown = true;
            }

            if (keyboardState.IsKeyUp(Keys.Up))
                _rotateKeyDown = false;

            if (_moveTimer > MOVE_TIME_THRESHOLD || !_moveKeyDown)
            {
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    LateralMove(new Vector2(_sprite.Width, 0));
                }
                else if (keyboardState.IsKeyDown(Keys.Left))
                {
                    LateralMove(new Vector2(-_sprite.Width, 0));
                }
                _moveTimer = 0;
                _moveKeyDown = true;
            }
            
            if (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.Left))
                _moveKeyDown = false;

            
            if (!fallKeyPressed)
            {
                _fallSpeed = FALL_SPEED;
            }
            
            _fallTimer += (float)gameTime.ElapsedGameTime.TotalSeconds * _fallSpeed;

            if (_fallTimer < FALL_TIME_THRESHOLD) return;
            _fallSpeed = FALL_SPEED;
            Drop(new Vector2(0f, _sprite.Height));
            _moving = _blockStickTimer < BLOCK_STICK_THRESHOLD;
            _fallTimer = 0;
            // Reset key input here
        }

        private void HandleKeyPress(int fallSpeed)
        {
            _fallSpeed = fallSpeed;
        }

        private void TryRotateBlock()
        {
            var rotations = _currentBlockPattern.Rotate();
            var kickbacks = new List<Vector2>
            {
                new (0, 0),
                new (0, -_sprite.Height),
                new (_sprite.Width, 0),
                new (-_sprite.Width, 0),
                new (_sprite.Width * 2, 0),
                new (-_sprite.Width * 2, 0)
            };

            foreach (var kickback in kickbacks)
            {
                var validRotation = new List<Vector2>();

                foreach (var rotation in rotations)
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
                    return;
                }
            }
        }


        public bool IsActive()
        {
            return _moving;
        }
    }
}
