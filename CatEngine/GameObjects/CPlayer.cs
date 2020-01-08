using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using CatEngine.Content;

namespace CatEngine
{
    public class CPlayer : CGameObject
    {
        public float fAimDirection = 0.0f;

        private float fHInput = 0.0f;
        private float fVInput = 0.0f;

        public float fDir = 0;

        private float fMoveDir = 0.0f;

        float fMaxSpeed = 0.35f;
        private float fAcceleration = 0.05f;
        private float fFriction = 0.075f;

        float fJumpSpeed = 1.0f;

        private float fMinHeight = 0.0f;
        private float fHeightBufferZone = 0.6f;

        private float fZSpeed = 0.0f;

        private float fGravity = 0.05f;

        private bool bLanded = false;

        private float fPlayerHeight = 0.0f;
        
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

            if(!CSettings.Instance.bGamepadEnabled)
                MovementKeyboard(keyboardState);
            else
                MovementGamepad(gamepadState);

            PlayerPhysics();
            PlayerCollision(fHeightBufferZone, 4.0f);
        }

        public override void Render()
        {
            //CSprite.Instance.Render("sprPlayer", x+8, y+8, iAnimFrame % 4, false, -(float)(iDir*(Math.PI/2)), 1.0f, Color.White);
            //CRender.Instance.DrawModel("textured_cube", new Vector3(x, z, y), fDir);

            String anime = "Monni";

            if (fHorSpeed != 0 || fVerSpeed != 0)
                anime = "Monni";
            else
                anime = "Monni_standby";

            CRender.Instance.DrawSkinnedModel("Monni", anime, new Vector3(x, z, y), fDir+((float)Math.PI/2));
            //CRender.Instance.DrawBillBoard(new Vector3(10.0f, 10.0f, 10.0f), new Vector2(5, 5), new Vector2(2.5f, 2.5f), "grasstop");
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
                fZSpeed = fJumpSpeed;
                //bLanded = false;
            }

            //CGameObject col = CollisionRectangle(new Rectangle((int)x + (int)distDirX(16, degToRad(iNextDir * 90)), (int)y + (int)distDirY(16, degToRad(iNextDir * 90)), 16, 16), typeof(CWall), true);

            //if (col == null)
        }

        private void MovementGamepad(GamePadState gamepadState)
        {
            fHInput = gamepadState.ThumbSticks.Left.X;
            fVInput = gamepadState.ThumbSticks.Left.Y;

            if (gamepadState.IsButtonDown(Buttons.A) && bLanded)
            {
                fZSpeed = fJumpSpeed;
                //bLanded = false;
            }
        }

        private void PlayerPhysics()
        {
            //handling movement

            //temp I guess
            CCamera camera = (CCamera)FindInstance(typeof(CCamera));
            camera.SetTarget(this);
            float fCamDir = degToRad(camera.GetCameraDirection() + 90.0f);

            float fInputDir = PointDirection(0, 0, fHInput, fVInput);
            float fInputDist = PointDistance(0, 0, fHInput, fVInput);
            float fSpeed = 0.5f;

            if (fHInput != 0 || fVInput != 0)
            {
                fDir = fCamDir + fInputDir;
                fMoveDir = fCamDir;

                fHorSpeed += distDirX(fAcceleration * 2, fInputDir);
                fVerSpeed += distDirY(fAcceleration * 2, fInputDir);
            }

            //friction is only applied when inputs are not detected to avoid slowing the player down too much
            if (fHInput == 0)
            {
                if (fHorSpeed > fFriction)
                {
                    fHorSpeed -= fFriction;
                }
                else if (fHorSpeed < -fFriction)
                {
                    fHorSpeed += fFriction;
                }
                else
                {
                    fHorSpeed = 0;
                }
            }

            if (fVInput == 0)
            {
                if (fVerSpeed > (fFriction))
                {
                    fVerSpeed -= fFriction;
                }
                else if (fVerSpeed < -(fFriction))
                {
                    fVerSpeed += fFriction;
                }
                else
                {
                    fVerSpeed = 0;
                }
            }

            //capping the speed
            if (fHorSpeed > fMaxSpeed)
            {
                fHorSpeed = fMaxSpeed;
            }
            else if (fHorSpeed < -fMaxSpeed)
            {
                fHorSpeed = -fMaxSpeed;
            }

            if (fVerSpeed > fMaxSpeed)
            {
                fVerSpeed = fMaxSpeed;
            }
            else if (fVerSpeed < -fMaxSpeed)
            {
                fVerSpeed = -fMaxSpeed;
            }

            //CConsole.Instance.Print("player hsp " + fHorSpeed + " vsp " + fVerSpeed + " dir " + radToDeg(fDir) % 360.0f + " inputdir " + fInputDir);

            //handling gravity
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

                    CParticleManager.Instance.CreateParticle(new Vector3(x, z, y), new Vector3(0, 0, 0));
                }

                z = (float)Math.Floor(z);
                while (z < fMinHeight-fGravity*2)
                {
                    z += fGravity;
                }
            }

            z += fZSpeed;
        }

        private void PlayerCollision(float bufferZone, float colBoxSize)
        {
            float hsp = distDirX(fHorSpeed, fMoveDir) + distDirX(fVerSpeed, fMoveDir - (float)(Math.PI / 2));
            float vsp = distDirY(fHorSpeed, fMoveDir) + distDirY(fVerSpeed, fMoveDir - (float)(Math.PI / 2));

            float heightPoint = CLevel.Instance.GetMapHeightAt(x + colBoxSize*Math.Sign(hsp), y);
            if (heightPoint > z + bufferZone)
            {
                hsp = 0;
                //CConsole.Instance.Print("player x y " + x + " " + y + " col x y " + x+hsp*2 + " " + y + " height " + heightPoint);
            }

            heightPoint = CLevel.Instance.GetMapHeightAt(x, y + colBoxSize * Math.Sign(vsp));
            if (heightPoint > z + bufferZone)
            {
                vsp = 0;
                //CConsole.Instance.Print("player x y " + x + " " + y + " col x y " + x + " " + y+vsp*2 + " height " + heightPoint);
            }

            x += hsp;
            y += vsp;
        }
    }
}
