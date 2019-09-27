using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CatEngine
{
    public class CPlayer : CGameObject
    {
        public float fAimDirection = 0.0f;

        private int iFSpeed = 0;
        private int iSSpeed = 0;

        private int iDir = 0;

        private int iNextDir = 0;

        private int iAnimCoolDown = 5;

        private int iAnimTimer = 0;

        private int iAnimFrame = 0;

        private bool bCanMove = false;
        
        public override void InstanceSpawn()
        {
            vCollisionOrigin = new Vector2(0, 0);
            rCollisionRectangle = new Rectangle(0, 0, 16, 16);
        }
        
        public override void Update()
        {
            UpdateCollision();

            //floor collision
            CGameObject collision = CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y + rCollisionRectangle.Height, rCollisionRectangle.Width, 1), typeof(CWall), true);

            //input
            KeyboardState keyboardState = Keyboard.GetState();

            MovementKeyboard(keyboardState);

            //check for collision on next tile
            if ((int)x % 16 == 0 && (int)y % 16 == 0)
            {
                CGameObject col = CollisionRectangle(new Rectangle((int)x + (int)distDirX(16, degToRad(iDir * 90)), (int)y + (int)distDirY(16, degToRad(iDir * 90)), 16, 16), typeof(CWall), true);

                if (col == null)
                    bCanMove = true;
                else
                    bCanMove = false;
            }

            
            if (bCanMove)
            {
                int spd = 1;

                if (iDir == 0)
                {
                    x += spd;
                }
                else if (iDir == 1)
                {
                    y -= spd;
                }
                else if (iDir == 2)
                {
                    x -= spd;
                }
                else if (iDir == 3)
                {
                    y += spd;
                }

                if (iAnimTimer <= 0)
                {
                    iAnimFrame++;

                    iAnimTimer = iAnimCoolDown;
                }
                else
                {
                    iAnimTimer--;
                }

                iAnimFrame %= 5;
            }
        }

        public override void Render()
        {
            CSprite.Instance.Render("sprPlayer", x+8, y+8, iAnimFrame % 4, false, -(float)(iDir*(Math.PI/2)), 1.0f, Color.White);
        }

        public void MovementKeyboard(KeyboardState keyboardState)
        {
            //moving
            //right
            if (keyboardState.IsKeyDown(CSettings.Instance.kPTurnRight))
            {
                iNextDir = 0;
            }
            //up
            else if (keyboardState.IsKeyDown(CSettings.Instance.kPMoveForward))
            {
                iNextDir = 1;
            }
            //left
            else if (keyboardState.IsKeyDown(CSettings.Instance.kPTurnLeft))
            {
                iNextDir = 2;
            }
            //down
            else if (keyboardState.IsKeyDown(CSettings.Instance.kPMoveBackward))
            {
                iNextDir = 3;
            }

            CGameObject col = CollisionRectangle(new Rectangle((int)x + (int)distDirX(16, degToRad(iNextDir * 90)), (int)y + (int)distDirY(16, degToRad(iNextDir * 90)), 16, 16), typeof(CWall), true);

            if (col == null)
                iDir = iNextDir;
        }
    }
}
