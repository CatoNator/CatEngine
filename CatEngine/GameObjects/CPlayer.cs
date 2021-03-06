﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using CatEngine.Content;
using CatEngine.Input;

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
        public float fAimDir = 0;
        private float fBodyDir = 0;
        private float fFeetDir = 0;

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

        private float fAnimFade = 0.0f;

        private float fLightDistance = 20.0f;
        private const float fLightMaxDistance = 40.0f;
        private const float fLightMinDistance = 10.0f;

        public Vector3 vLightTarget = Vector3.Zero;

        String sCurrentAnimation = "player_tpose";
        String sSecondaryAnimation = null;

        private int iShotCooldown = 0;
        private const int iShotCooldownMax = 5;

        private bool bIsShooting = false;

        float testframe = 0;


        public override void InstanceSpawn()
        {
            vCollisionOrigin = new Vector2(0, 0);
            rCollisionRectangle = new Rectangle(0, 0, 16, 16);

            hitCylinder = new CylinderCollider(new Vector3(x, z, y), 10, 2.5f);
        }
        
        public override void Update()
        {
            UpdateCollision();
            hitCylinder.SetPos(new Vector3(x, z, y));

            //floor collision
            CGameObject natsaCollision = CollisionCylinder(typeof(CNatsa));

            if (natsaCollision != null)
            {
                CGame.Instance.CollectNatsa(1);
                CObjectManager.Instance.DestroyInstance(natsaCollision.iIndex);
            }

            //input
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);


            if (!CSettings.Instance.bGamepadEnabled)
                MovementKeyboard(keyboardState);
            else
                MovementGamepad(gamepadState);

            PlayerPhysics();
            PlayerCollision(fHeightBufferZone, 4.0f);

            if (fHInput2 != 0)
            {
                fAimDir -= (fHInput2 * 0.2f)/(float)Math.PI;
            }

            if (Math.Abs(fVInput2) >= 0.35)
            {
                fLightDistance -= (fVInput2 * 1.5f) / (float)Math.PI;

                if (fLightDistance > fLightMaxDistance)
                    fLightDistance = fLightMaxDistance;
                else if (fLightDistance < fLightMinDistance)
                    fLightDistance = fLightMinDistance;
            }

            //shoob
            if (CInputManager.ButtonDown(CSettings.Instance.gFire))
            {
                if (iShotCooldown <= 0)
                {
                    float xoffs = distDirX(5, fAimDir);
                    float yoffs = distDirY(5, fAimDir);

                    Vector3 bulletSpawnPoint = new Vector3(x + xoffs, z + 7, y + yoffs);

                    CAudioManager.Instance.PlaySound("temp_gunshot");

                    CPlayerBullet bullet = (CPlayerBullet)CObjectManager.Instance.CreateInstance(typeof(CPlayerBullet), bulletSpawnPoint.X, bulletSpawnPoint.Y, bulletSpawnPoint.Z);
                    bullet.SetProperties(fAimDir, 5);

                    CParticleManager.Instance.CreateParticle("part_muzzleflash", bulletSpawnPoint);
                    CParticleManager.Instance.CreateParticle("part_gunsmoke", bulletSpawnPoint);
                    CParticleManager.Instance.CreateParticle("part_bulletcasing", bulletSpawnPoint, new Vector3(distDirX(0.3f, fAimDir - (float)(Math.PI/2)), 0, distDirY(0.3f, fAimDir - (float)(Math.PI / 2))));

                    iShotCooldown = iShotCooldownMax;
                }

                testframe += 0.25f;
            }

            iShotCooldown--;

            //debug
                CLevel.Instance.UpdateActiveCell(x, y);

            //we don't want to update the animation in Render() because it'll animate while paused
            UpdateAnimation();

            CGame.Instance.UpdatePlayer(new Vector2(x, y));

            vLightTarget = new Vector3(x + distDirX(fLightDistance, fAimDir), z, y + distDirY(fLightDistance, fAimDir));

            CRender.Instance.SetLightPosition(new Vector3(x, z + 10, y), vLightTarget);
        }

        public override void Render(string technique)
        {
            if (technique != "ShadowMap")
            {
                //CRender.Instance.DrawPlayer(technique, sCurrentAnimation, sSecondaryAnimation, new Vector3(x, z, y), fDir+((float)Math.PI/2), fAnimFrame, fAnimFade);

                float psize = 0.5f;

                //CRender.Instance.Draw3DSprite(technique, "p_legs_" + sCurrentAnimation, (int)fAnimFrame, new Vector3(x, z + 2f, y), psize, new Vector3(0, fDir, 0), 1f);
                CRender.Instance.RenderSkeletalSprite("legs", "legs_anim", new Vector3(x, z + 2f, y) / psize, fAnimFrame, fDir, psize);
                CRender.Instance.RenderSkeletalSprite("player_ak", "ak_fire", new Vector3(x, z + 4f, y)/psize, testframe, fAimDir, psize);

                //legs
                /*CRender.Instance.Draw3DSprite(technique, "p_legs_" + sCurrentAnimation, (int)fAnimFrame, new Vector3(x, z + 2f, y), psize, new Vector3(0, fDir, 0), 1f);
                //body
                CRender.Instance.Draw3DSprite(technique, "p_body_" + sCurrentAnimation, (int)fAnimFrame, new Vector3(x, z + 4f, y), psize, new Vector3(0, fDir, 0), 1f);
                //torso
                CRender.Instance.Draw3DSprite(technique, "p_torso_" + sCurrentAnimation, (int)fAnimFrame, new Vector3(x, z + 6f, y), psize, new Vector3(0, fDir, 0), 1f);
                //head
                CRender.Instance.Draw3DSprite(technique, "p_head_" + sCurrentAnimation, (int)fAnimFrame, new Vector3(x, z + 7f, y), psize, new Vector3(0, fDir, 0), 1f);*/

                CRender.Instance.DrawShadow(new Vector3(x, z + 2.5f, y), 2.5f);
                //CRender.Instance.DrawPlayerShadow(new Vector3(x, z + 2.5f, y), 2.5f, sCurrentAnimation, fDir, fAnimFrame);

                if (CDebug.Instance.ShowHitBoxes)
                    CRender.Instance.DrawHitBox(hitCylinder.Position, hitCylinder.Height, hitCylinder.Radius);
            }
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
                CAudioManager.Instance.PlaySound("footstep_solid1");
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

            if (CInputManager.ButtonPressed(CSettings.Instance.gPJump) && bLanded)
            {
                fZSpeed = fJumpSpeed;
                CAudioManager.Instance.PlaySound("footstep_solid1");
                //bLanded = false;
            }
        }

        private float AngleDiff(float angle1, float angle2)
        {
            float pi = (float)Math.PI;
            return ((((angle1 - angle2) % pi) + (pi*1.5f)) % pi) - (pi/2);
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

            if ((fHInput != 0 || fVInput != 0))
            {
                inputDir = inputDir = PointDirection(0, 0, fHInput, fVInput);
                inputSp = 1.0f;//PointDistance(0, 0, fHInput, fVInput);
                fDir = inputDir+fCamDir;
                //fMoveDir = fCamDir;
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
            float heightPFront = HeightAtPoint(new Vector3(x + distDirX(angleMeasureDist, fDir), y + distDirY(angleMeasureDist, fDir), z + 10.0f));//CLevel.Instance.GetHeightAt(x + distDirX(angleMeasureDist, fDir), y + distDirY(angleMeasureDist, fDir), z+10.0f);
            float heightPBack = HeightAtPoint(new Vector3(x + distDirX(-angleMeasureDist, fDir), y + distDirY(-angleMeasureDist, fDir), z + 10.0f));//CLevel.Instance.GetHeightAt(x + distDirX(-angleMeasureDist, fDir), y + distDirY(-angleMeasureDist, fDir), z + 10.0f);

            float angle = radToDeg(PointDirection(0, heightPBack, spSp, heightPFront));
            float angleSpeed = (angle / 90.0f) * 4.0f;

            //Console.WriteLine(heightPFront + " " + heightPBack + " " + angle);

            if (angle < 0 && bLanded && angle > -72.0f)
            {
                z -= heightPBack - heightPFront;
                //CConsole.Instance.Print("angle " + angle + " zdiff " + (heightPBack - heightPFront));
            }


            //CConsole.Instance.debugString = "floor angle";
            //CConsole.Instance.debugValue = angle;

            //handling gravity

            float groundH = CLevel.Instance.GetHeightAt(x, y, z);
            
            Tuple<CCollidable, float> col = GetObjectCollision(new Vector3(x, y, z));

            float objH = col.Item2;

            if (groundH > objH)
                fMinHeight = groundH + fPlayerHeight;
            else
            {
                fMinHeight = objH + fPlayerHeight;//(float)Math.Min(z-CLevel.Instance.GetHeightAt(x, y, z) + fPlayerHeight, z-GetObjectCollision()+ fPlayerHeight);
                if (bLanded && col.Item1 != null)
                {
                    x += col.Item1.Speed.X;
                    y += col.Item1.Speed.Y;
                    z += col.Item1.Speed.Z;
                }
            }

            //z = fMinHeight;

            CConsole.Instance.debugString = "minh";
            CConsole.Instance.debugValue = fMinHeight;

            //CConsole.Instance.debugString2 = "z";
            //CConsole.Instance.debugValue2 = z;

            if (z > fMinHeight)
            {
                bLanded = false;
                fZSpeed -= fGravity;

                /*if (z < fMinHeight+fHeightBufferZone*2)
                {
                    while (z > fMinHeight )
                    {
                        z += fGravity;
                    }
                }*/
            }
            else
            {
                if (!bLanded)
                {
                    bLanded = true;
                    fZSpeed = 0f;

                    CAudioManager.Instance.PlaySound("footstep_solid1");

                    CParticleManager.Instance.CreateParticle("part_dustcloud", new Vector3(x, z, y));
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

            //test if the player would be in a wall, did they continue walking forward

            Vector3 snap = WallCollisionAt(new Vector3(x, z, y), hitCylinder.Radius, hitCylinder.Height);

            while (!snap.Equals(new Vector3(0, 0, 0)))
            {
                x += 0.1f * Math.Sign(snap.X);
                y += 0.1f * Math.Sign(snap.Z);

                snap = WallCollisionAt(new Vector3(x, z, y), hitCylinder.Radius, hitCylinder.Height);
            }

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

        private void UpdateAnimation()
        {
            if (!bLanded)
            {
                sCurrentAnimation = "walk";
                fAnimFrame = 0;
            }
            else
            {
                if (fHorSpeed != 0 || fVerSpeed != 0)
                {
                    sCurrentAnimation = "walk";
                    float animSp = (float)(Math.Abs(PointDistance(0, 0, fHorSpeed, fVerSpeed)) / fMaxSpeed) * 0.125f;
                    
                    /*CConsole.Instance.debugString2 = "animframe";
                    CConsole.Instance.debugValue2 = animSp;*/

                    if (bIsShooting)
                    {
                        int diff = (int)((fDir - fAimDir) / (float)Math.PI/2);
                        Console.WriteLine(diff);

                        if (diff >= 2)
                        {
                            fAnimFrame -= animSp;
                            if (fAnimFrame < 0)
                                fAnimFrame = 8;
                        }
                        else
                        {
                            fAnimFrame += animSp;
                            fAnimFrame %= 8;
                        }
                    }
                    else
                    {
                        sCurrentAnimation = "walk";
                        fAnimFrame += animSp;
                        fAnimFrame %= 8;
                    }

                    if ((fAnimFrame >= 1 && fAnimFrame <= 3) || (fAnimFrame >= 45 && fAnimFrame <= 47))
                    {
                        CAudioManager.Instance.PlaySound("footstep_solid1");
                    }


                }
                else
                {
                    sCurrentAnimation = "stand";
                    fAnimFrame = 0;
                }
            }

            fFeetDir = fDir;
            fBodyDir = AngleDiff(fAimDir, fFeetDir) / 2;

            if (bIsShooting)
            {
                sSecondaryAnimation = "stand";
            }
            else
                sSecondaryAnimation = null;

            CRender.Instance.PlayerSetAdditionalRotation("upperback", new Vector3(0, fBodyDir, 0));

            CRender.Instance.PlayerSetAdditionalRotation("neck", new Vector3(0, fAimDir-fBodyDir, 0));
        }

        private Tuple<CCollidable, float> GetObjectCollision(Vector3 point)
        {
            float Height = 0.0f;
            List<Tuple<CCollidable, float>> heights = new List<Tuple<CCollidable, float>>();

            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                if ((CObjectManager.Instance.pGameObjectList[i] != null)
                    && Object.ReferenceEquals(typeof(CCollidable), CObjectManager.Instance.pGameObjectList[i].GetType()))
                {
                    //getting the reference
                    CCollidable otherInstance = (CCollidable)CObjectManager.Instance.pGameObjectList[i];

                    heights.Add(new Tuple<CCollidable, float>(otherInstance, otherInstance.GetHeightAt(point.X, point.Y, point.Z)));
                }
            }

            int ind = 0;

            if (heights.Count > 0)
            {
                //remove the ones above the player
                float min = z;

                //foreach (float f in possibleHeights)
                for (int i = 0; i < heights.Count; i++)
                {
                    float f = heights[i].Item2;
                    float diff = z - f;

                    //Console.WriteLine(f + " " + z + " " + diff);

                    if (diff >= -2.0f && diff < min) //
                    {
                        Height = f;
                        min = diff;
                        ind = i;
                    }
                }

                //Height = min;
            }

            if (Height > 0)
                return heights[ind];
            else
                return new Tuple<CCollidable, float>(null, 0.0f);
        }

        private float HeightAtPoint(Vector3 point)
        {
            float h = 0.0f;

            float groundH = CLevel.Instance.GetHeightAt(point.X, point.Y, point.Z);

            float objH = GetObjectCollision(point).Item2;

            if (groundH > objH)
                h = groundH;
            else
                h = objH;//(float)Math.Min(z-CLevel.Instance.GetHeightAt(x, y, z) + fPlayerHeight, z-GetObjectCollision()+ fPlayerHeight);

            return h;
        }

        private Vector3 WallCollisionAt(Vector3 point, float rad, float height)
        {
            Vector3 objectCollision = new Vector3(0, 0, 0);

            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                if ((CObjectManager.Instance.pGameObjectList[i] != null)
                    && Object.ReferenceEquals(typeof(CCollidable), CObjectManager.Instance.pGameObjectList[i].GetType()))
                {
                    //getting the reference
                    CCollidable otherInstance = (CCollidable)CObjectManager.Instance.pGameObjectList[i];

                    Vector3 col = otherInstance.PointInWall(point, rad, height);

                    if (!col.Equals(new Vector3(0, 0, 0)))
                        objectCollision = col;
                }
            }

            return objectCollision.Equals(new Vector3(0, 0, 0)) ? CLevel.Instance.PointInWall(point, rad, height) : objectCollision;
        }
    }
}
