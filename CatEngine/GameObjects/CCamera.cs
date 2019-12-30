using System;
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
        private float fCameraDistance = 30.0f;
        private float fCameraBufferRange = 5.0f;

        private CGameObject oTarget;

        private enum CameraState
        {
            PlayerControlled,
            Automatic
        };

        public override void Update()
        {
            int iCameraRot = 0;

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(CSettings.Instance.kCRotateCamRight))
            {
                iCameraRot = 1;
            }
            else if (keyboardState.IsKeyDown(CSettings.Instance.kCRotateCamLeft))
            {
                iCameraRot = -1;
            }
            else
            {
                iCameraRot = 0;
            }

            //AutoCamera();

            if (iCameraRot != 0)
                PlayerCamera(iCameraRot);
            else
                AutoCamera();

            //fCameraRotation %= 360.0f;

            Vector3 targetPos = new Vector3(0.0f, 0.0f, 0.0f);

            if (oTarget != null)
                targetPos = new Vector3(oTarget.x, oTarget.z, oTarget.y);

            CRender.Instance.SetCameraTarget(targetPos);
            CRender.Instance.SetCameraPosition(new Vector3(x, 10.0f, y));
            CConsole.Instance.Print("camera rotation " + fCameraRotation + " x "+x+" y "+y);
        }

        public void SetTarget(CGameObject targetObject)
        {
           oTarget = targetObject;
        }

        public float GetCameraDirection()
        {
            return fCameraRotation;
        }

        private void PlayerCamera(int rotDir)
        {
            float fCameraRotationSpeed = 4.0f;

            fCameraRotation++;//(float)rotDir * fCameraRotationSpeed;

            Vector3 targetPos = new Vector3(0.0f, 0.0f, 0.0f);

            if (oTarget != null)
                targetPos = new Vector3(oTarget.x, oTarget.z, oTarget.y);

            Vector3 cameraPos = new Vector3(x, 10.0f, y);

            float camDist = PointDistance(cameraPos.X, cameraPos.Z, targetPos.X, targetPos.Z);

            x = targetPos.X + distDirX(camDist, degToRad(fCameraRotation));
            y = targetPos.Z + distDirY(camDist, degToRad(fCameraRotation));
        }

        private void AutoCamera()
        {
            Vector3 targetPos = new Vector3(0.0f, 0.0f, 0.0f);

            if (oTarget != null)
                targetPos = new Vector3(oTarget.x, oTarget.z, oTarget.y);

            Vector3 cameraPos = new Vector3(x, 10.0f, y);

            fCameraRotation = radToDeg(PointDirection(cameraPos.X, cameraPos.Z, targetPos.X, targetPos.Z));
            float camDist = PointDistance(cameraPos.X, cameraPos.Z, targetPos.X, targetPos.Z);

            if (camDist > fCameraDistance)
            {
                x = targetPos.X + distDirX(fCameraDistance, -degToRad(fCameraRotation));
                y = targetPos.Z + distDirY(fCameraDistance, -degToRad(fCameraRotation));
            }

            //CRender.Instance.SetCameraTarget(new Vector3(distDirX(30.0f, degToRad(fCameraRotation)), 0.0f, distDirY(30.0f, degToRad(fCameraRotation))));
        }
    }
}
