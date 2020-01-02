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

        private int iDir = 0;

        private float fHInput = 0.0f;
        private float fVInput = 0.0f;

        private float fDir = 0;

        float fMaxSpeed = 1.0f;
        private float fAcceleration = 0.025f;

        private float fMinHeight = 0.0f;
        private float fHeightBufferZone = 0.6f;

        private float fZSpeed = 0.0f;

        private float fGravity = 0.05f;

        private bool bLanded = false;

        private float fPlayerHeight = 1.0f;

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
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            MovementKeyboard(keyboardState);
            //MovementGamepad(gamepadState);
            //CameraDebug(keyboardState);

            CCamera camera = (CCamera)FindInstance(typeof(CCamera));
            camera.SetTarget(this);
            float fCameraRotation = camera.GetCameraDirection();

            float fInputDir = PointDirection(0, 0, fHInput, fVInput);
            float fSpeed = 0.5f;

            if (fHInput != 0 || fVInput != 0)
            {
                fDir = degToRad(fCameraRotation + 90.0f) + fInputDir;

                x += distDirX(fSpeed, fDir);
                y += distDirY(fSpeed, fDir);
            }

            PlayerPhysics();
            //PlayerCollision(0.5f);
        }

        public override void Render()
        {
            //CSprite.Instance.Render("sprPlayer", x+8, y+8, iAnimFrame % 4, false, -(float)(iDir*(Math.PI/2)), 1.0f, Color.White);
            CRender.Instance.DrawModel("textured_cube", new Vector3(x, z, y), fDir);
        }

        public void MovementKeyboard(KeyboardState keyboardState)
        {
            //moving
            //right
            if (keyboardState.IsKeyDown(CSettings.Instance.kPTurnRight))
            {
                fHInput = 1.0f;
            }
            //left
            else if (keyboardState.IsKeyDown(CSettings.Instance.kPTurnLeft))
            {
                fHInput = -1.0f;
            }
            else
            {
                fHInput = 0.0f;
            }

            //up
            if (keyboardState.IsKeyDown(CSettings.Instance.kPMoveForward))
            {
                fVInput = 1.0f;
            }
            //down
            else if (keyboardState.IsKeyDown(CSettings.Instance.kPMoveBackward))
            {
                fVInput = -1.0f;
            }
            else
            {
                fVInput = 0.0f;
            }

            //yump
            if (keyboardState.IsKeyDown(CSettings.Instance.kPJump) && bLanded)
            {
                fZSpeed = 0.6f;
                //bLanded = false;
            }

            //CGameObject col = CollisionRectangle(new Rectangle((int)x + (int)distDirX(16, degToRad(iNextDir * 90)), (int)y + (int)distDirY(16, degToRad(iNextDir * 90)), 16, 16), typeof(CWall), true);

            //if (col == null)
        }

        private void PlayerPhysics()
        {
            fMinHeight = CLevel.Instance.GetMapHeightAt(x, y) + fPlayerHeight;

            if (z > fMinHeight)
            {
                bLanded = false;
                fZSpeed -= fGravity;
            }
            else
            {
                if (!bLanded)
                {
                    bLanded = true;
                    fZSpeed = 0f;
                }

                z = (float)Math.Floor(z);
                while (z < fMinHeight-fGravity*2)
                {
                    z += fGravity;
                }
            }

            z += fZSpeed;
        }

        private void MovementGamepad(GamePadState gamepadState)
        {
            fHInput = gamepadState.ThumbSticks.Left.X;
            fVInput = gamepadState.ThumbSticks.Left.Y;
        }

        private void PlayerCollision(float bufferZone)
        {
            //set up like 4 to 8 points around the player checking for the heightmap height
            //if the heightmap is higher than player's height +- buffer zone, push the player towards his own center axis until this is no longer the case

            //this is kind of fucked. I should do collision detection on tile edge

            int playerCollisionPoints = 4; //square
            float collisionSize = 1.0f;

            for (int i = 0; i < playerCollisionPoints; i++)
            {
                float collisionX = x + distDirX(collisionSize, 45 + (90 * i));
                float collisionY = y + distDirY(collisionSize, 45 + (90 * i));

                float heightPoint = CLevel.Instance.GetMapHeightAt(collisionX, collisionY) + fPlayerHeight;


                if (heightPoint > fMinHeight+bufferZone)
                {
                    x += distDirX(0.1f, PointDirection(collisionX, collisionY, x, y));
                    y += distDirY(0.1f, PointDirection(collisionX, collisionY, x, y));
                }

                //CConsole.Instance.Print("player x y " + x + " " + y + " col x y "+collisionX + " " + collisionY + " height " +heightPoint);
            }
        }
    }
}
