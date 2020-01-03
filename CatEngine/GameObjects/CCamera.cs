﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CatEngine
{
    class CCamera : CGameObject
    {
        private float fCameraRotation = 90.0f;
        private float fCameraVRotation = 20.0f;
        private float fCameraDistance = 20.0f;
        private float fTargetHeight = 10.0f;
        private float fCameraBufferRange = 5.0f;

        private float fCameraRotationSpeed = 2.0f;
        private float fCameraVRotationSpeed = 2.0f;

        private CGameObject oTarget;

        private Vector3 TargetVector = new Vector3(0, 0, 0);

        private enum CameraStates
        {
            PlayerControlled,
            LevelStart
        };

        private CameraStates cameraState = CameraStates.LevelStart;

        public override void Update()
        {
            int iCameraRot = 0;

            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(CSettings.Instance.kCRotateCamRight) || gamepadState.IsButtonDown(Buttons.DPadRight))
            {
                iCameraRot = -1;
            }
            else if (keyboardState.IsKeyDown(CSettings.Instance.kCRotateCamLeft) || gamepadState.IsButtonDown(Buttons.DPadLeft))
            {
                iCameraRot = 1;
            }

            int iCameraVRot = 0;

            if (keyboardState.IsKeyDown(CSettings.Instance.kCRotateCamUp) || gamepadState.IsButtonDown(Buttons.DPadUp))
            {
                iCameraVRot = 1;
            }
            else if (keyboardState.IsKeyDown(CSettings.Instance.kCRotateCamDown) || gamepadState.IsButtonDown(Buttons.DPadDown))
            {
                iCameraVRot = -1;
            }

            if (keyboardState.IsKeyDown(Keys.Enter) && cameraState == CameraStates.LevelStart)
            {
                cameraState = CameraStates.PlayerControlled;
            }

            //PlayerCamera(iCameraRot, iCameraVRot);

            if (cameraState == CameraStates.LevelStart)
                LevelCamera();
            else if (cameraState == CameraStates.PlayerControlled)
            {
                if (iCameraRot != 0 || iCameraVRot != 0)
                    PlayerCamera(iCameraRot, iCameraVRot);
                else
                    AutoCamera();

                if (keyboardState.IsKeyDown(Keys.X))
                {
                    fCameraRotation = ((CPlayer)oTarget).fDir;
                }
            }

            Vector3 lerpVector = Lerp(
                new Vector3(x + distDirX(fCameraDistance, degToRad(fCameraRotation)), z + -distDirY(fCameraDistance, degToRad(fCameraVRotation)), y + distDirY(fCameraDistance, degToRad(fCameraRotation))),
                CRender.Instance.GetCameraPosition(), 0.9f);

            CRender.Instance.SetCameraTarget(TargetVector);
            CRender.Instance.SetCameraPosition(lerpVector);
            //CConsole.Instance.Print("camera rotation " + fCameraVRotation + " rot " + iCameraVRot);
        }

        public void SetTarget(CGameObject targetObject)
        {
           oTarget = targetObject;
        }

        public float GetCameraDirection()
        {
            return fCameraRotation;
        }

        private void PlayerCamera(int rotDir, int rotDirV)
        {
            fCameraRotation += (float)rotDir * fCameraRotationSpeed;

            if (fCameraRotation > 180.0f)
                fCameraRotation -= 360.0f;
            else if (fCameraRotation < -180.0f)
                fCameraRotation += 360.0f;

            fCameraVRotation += (float)rotDirV * fCameraVRotationSpeed;

            if (fCameraVRotation > 80.0f)
                fCameraVRotation = 80.0f;
            else if (fCameraVRotation < 5.0f)
                fCameraVRotation = 5.0f;

            Vector3 targetPos = new Vector3(0.0f, 0.0f, 0.0f);

            if (oTarget != null)
                targetPos = new Vector3(oTarget.x, oTarget.z + fTargetHeight, oTarget.y);

            Vector3 cameraPos = new Vector3(x, z, y);

            float camDist = PointDistance(cameraPos.X, cameraPos.Z, targetPos.X, targetPos.Z);

            x = targetPos.X + distDirX(fCameraDistance, degToRad(fCameraRotation));
            y = targetPos.Z + distDirY(fCameraDistance, degToRad(fCameraRotation));
            z = targetPos.Y + -distDirY(fCameraDistance, degToRad(fCameraVRotation));

            TargetVector = targetPos;
        }

        private void AutoCamera()
        {
            Vector3 targetPos = new Vector3(0.0f, 0.0f, 0.0f);

            if (oTarget != null)
                targetPos = new Vector3(oTarget.x, oTarget.z + fTargetHeight, oTarget.y);

            Vector3 cameraPos = new Vector3(x, 10.0f, y);

            fCameraRotation = -radToDeg(PointDirection(cameraPos.X, cameraPos.Z, targetPos.X, targetPos.Z)) + 180.0f;
            float camDist = PointDistance(cameraPos.X, cameraPos.Z, targetPos.X, targetPos.Z);

            if (camDist > fCameraDistance || camDist < fCameraDistance)
            {
                x = targetPos.X + distDirX(fCameraDistance, degToRad(fCameraRotation));
                y = targetPos.Z + distDirY(fCameraDistance, degToRad(fCameraRotation));
                z = targetPos.Y + -distDirY(fCameraDistance, degToRad(fCameraVRotation));
            }

            TargetVector = targetPos;

            //CRender.Instance.SetCameraTarget(new Vector3(distDirX(30.0f, degToRad(fCameraRotation)), 0.0f, distDirY(30.0f, degToRad(fCameraRotation))));
        }

        private void LevelCamera()
        {
            int LevelW = CLevel.Instance.iLevelWidth;
            int LevelH = CLevel.Instance.iLevelHeight;
            int TileS = CLevel.Instance.iTileSize;

            Vector3 targetPos = new Vector3((LevelW / 2 ) * TileS, 10.0f, (LevelH / 2) * TileS);

            float fCameraRotationSpeed = 1.0f;
            float fLevelCamDist = LevelW * TileS + 30.0f;

            fCameraRotation += fCameraRotationSpeed;
            fCameraVRotation = 80.0f;

            x = targetPos.X + distDirX(fLevelCamDist, degToRad(fCameraRotation));
            y = targetPos.Z + distDirY(fLevelCamDist, degToRad(fCameraRotation));
            z = targetPos.Y + -distDirY(fLevelCamDist, degToRad(fCameraVRotation));

            TargetVector = targetPos;
        }
    }
}
