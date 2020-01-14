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

        private float fHInput2 = 0.0f;
        private float fVInput2 = 0.0f;

        public float fDir = 0;

        private float fMoveDir = 0.0f;

        private const float fMaxSpeed = 0.6f;
        private const float fAcceleration = 0.1f;
        private const float fFriction = 0.075f;

        private const float fJumpSpeed = 1.0f;

        private float fMinHeight = 0.0f;
        private const float fHeightBufferZone = 3.0f;

        private float fZSpeed = 0.0f;

        private const float fGravity = 0.05f;

        private bool bLanded = false;

        private const float fPlayerHeight = 0.0f;

        private float fAnimFrame = 0.0f;
        
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

            CLevelTest.Instance.UpdateActiveCell(x, y);
        }

        public override void Render()
        {
            //CSprite.Instance.Render("sprPlayer", x+8, y+8, iAnimFrame % 4, false, -(float)(iDir*(Math.PI/2)), 1.0f, Color.White);
            //CRender.Instance.DrawModel("textured_cube", new Vector3(x, z, y), fDir);

            String anime = "player_tposebones";

            if (fHorSpeed != 0 || fVerSpeed != 0)
            {
                anime = "player_walkcyclebones";
                float animSp = (float)(Math.Abs(PointDistance(0, 0, fHorSpeed, fVerSpeed))/fMaxSpeed)*2;
                fAnimFrame += animSp;
                CConsole.Instance.debugString2 = "animframe";
                CConsole.Instance.debugValue2 = animSp;
            }
            else
            {
                anime = "player_tposebones";
                fAnimFrame = 0.0f;
            }
                

            double spSp = (double)(Math.Abs(PointDistance(0, 0, fHorSpeed, fVerSpeed)) / fMaxSpeed);
            //CConsole.Instance.debugString2 = "animSp";
            //CConsole.Instance.debugValue2 = (float)spSp;

            CRender.Instance.DrawPlayer(anime, new Vector3(x, z, y), fDir+((float)Math.PI/2), fAnimFrame);

            float shadowSize = 2.5f;

            CRender.Instance.DrawModel("textured_cube", new Vector3(x, fMinHeight, y), 0.0f);

            /*CRender.Instance.DrawRectangle(new Vector3(x - shadowSize, fMinHeight + 0.1f, y + shadowSize),
                    new Vector3(x + shadowSize, fMinHeight + 0.1f, y + shadowSize),
                    new Vector3(x - shadowSize, fMinHeight + 0.1f, y - shadowSize),
                    new Vector3(x + shadowSize, fMinHeight + 0.1f, y - shadowSize), "shadow", false, 0.5f);*/
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
            fHInput2 = gamepadState.ThumbSticks.Right.X;
            fVInput2 = gamepadState.ThumbSticks.Right.Y;

            if (gamepadState.IsButtonDown(Buttons.A) && bLanded)
            {
                fZSpeed = fJumpSpeed;
                //bLanded = false;
            }
        }

        private float PDPositive(float x1, float y1, float x2, float y2)
        {

            float val = (float)Math.Atan2((double)(y1 - y2), (double)(x2 - x1));

            if (val < 0)
                val += (float)Math.PI*2;

            return val;
        }

        private void PlayerPhysics()
        {
            CCamera camera = (CCamera)FindInstance(typeof(CCamera));
            camera.SetTarget(this);
            float fCamDir = degToRad(camera.GetCameraDirection() + 90.0f);

            //handling movement
            float inputDir = 0;
            float inputSp = 0;

            if ((fHInput != 0 || fVInput != 0) && (fHInput2 == 0 && fVInput2 == 0))
            {
                inputDir = inputDir = PointDirection(0, 0, fHInput, fVInput);
                inputSp = PointDistance(0, 0, fHInput, fVInput);
                fDir = inputDir+fCamDir;
                //fMoveDir = fCamDir;
            }
            else if (fHInput2 != 0 || fVInput2 != 0)
            {
                float inputDirR = PointDirection(0, 0, fHInput2, fVInput2);
                fDir = inputDirR + fCamDir;
            }

            fHorSpeed += distDirX(inputSp * fAcceleration, fDir);
            fVerSpeed += distDirY(inputSp * fAcceleration, fDir);

            float spDir = PDPositive(0, 0, fHorSpeed, fVerSpeed);
            float spSp = PointDistance(0, 0, fHorSpeed, fVerSpeed);

            if (spSp > fFriction)
            {
                fHorSpeed += distDirX(-fFriction, spDir);
                fVerSpeed += distDirY(-fFriction, spDir);
            }

            if (spSp <= fFriction)
            {
                fHorSpeed = 0;
                fVerSpeed = 0;
            }

            if (spSp > fMaxSpeed*inputSp)
            {
                fHorSpeed = distDirX(fMaxSpeed * inputSp, spDir);
                fVerSpeed = distDirY(fMaxSpeed * inputSp, spDir);
            }

            float angleMeasureDist = spSp;
            float heightPFront = CLevel.Instance.GetMapHeightAt(x + distDirX(angleMeasureDist, fDir), y + distDirY(angleMeasureDist, fDir));
            float heightPBack = CLevel.Instance.GetMapHeightAt(x + distDirX(-angleMeasureDist, fDir), y + distDirY(-angleMeasureDist, fDir));

            float angle = radToDeg(PointDirection(0, heightPBack, spSp, heightPFront));
            float angleSpeed = (angle / 90.0f) * 4.0f;

            //CConsole.Instance.debugString = "angle";
            //CConsole.Instance.debugValue = angle;

            if (angle < 0 && bLanded && angle > -72.0f)
            {
                z -= heightPBack - heightPFront;
                //CConsole.Instance.Print("angle " + angle + " zdiff " + (heightPBack - heightPFront));
            }
                

            //CConsole.Instance.debugString = "floor angle";
            //CConsole.Instance.debugValue = angle;

            //handling gravity
            fMinHeight = CLevelTest.Instance.GetHeightAt(x, y) + fPlayerHeight;

            z = fMinHeight;

            /*if (z > fMinHeight)
            {
                bLanded = false;
                fZSpeed -= fGravity;

                /*if (z < fMinHeight+fHeightBufferZone*2)
                {
                    while (z > fMinHeight )
                    {
                        z += fGravity;
                    }
                }
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

            z += fZSpeed;*/
        }

        private void PlayerCollision(float bufferZone, float colBoxSize)
        {
            float hsp = distDirX(fHorSpeed, fMoveDir) + distDirX(fVerSpeed, fMoveDir - (float)(Math.PI / 2));
            float vsp = distDirY(fHorSpeed, fMoveDir) + distDirY(fVerSpeed, fMoveDir - (float)(Math.PI / 2));

            /*float heightPoint = CLevel.Instance.GetMapHeightAt(x + colBoxSize*Math.Sign(hsp), y);
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
            }*/

            x += hsp;
            y += vsp;
        }
    }
}
