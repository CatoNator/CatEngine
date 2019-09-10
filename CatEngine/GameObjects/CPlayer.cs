using System;
using System.Collections.Generic;
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
        
        public override void InstanceSpawn()
        {
            vCollisionOrigin = new Vector2(16, 16);
            rCollisionRectangle = new Rectangle(0, 0, 32, 32);
        }
        
        public override void Update()
        {
            UpdateCollision();

            //floor collision
            CGameObject collision = CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y + rCollisionRectangle.Height, rCollisionRectangle.Width, 1), typeof(CWall), true);

            //input
            KeyboardState keyboardState = Keyboard.GetState();

            MovementKeyboard(keyboardState);

            //capping the player rotation to [0.0f, 360.0f]
            if (fAimDirection >= 360)
                fAimDirection -= 360;
            else if (fAimDirection <= 0)
                fAimDirection += 360;

            fHorSpeed = (float)distDirX((float)iFSpeed, degToRad(fAimDirection)) + (float)distDirX((float)iSSpeed, degToRad(fAimDirection+90.0f));
            fVerSpeed = (float)distDirY((float)iFSpeed, degToRad(fAimDirection)) + (float)distDirY((float)iSSpeed, degToRad(fAimDirection+90.0f));

            //note! current collision model only supports recantular collisions, no pixel perfect shapes
            //collision always gets stuck, needs adjusting

            int collisionSafeZone = 4;

            //collisions to the left and right
            if ((CollisionRectangle(new Rectangle(rCollisionRectangle.X - collisionSafeZone, rCollisionRectangle.Y, collisionSafeZone, rCollisionRectangle.Height), typeof(CWall), true) != null && fHorSpeed < 0) || 
              (CollisionRectangle(new Rectangle(rCollisionRectangle.X + rCollisionRectangle.Width, rCollisionRectangle.Y, collisionSafeZone, rCollisionRectangle.Height), typeof(CWall), true) != null && fHorSpeed > 0))
                fHorSpeed = 0;

            //collisions above and below
            if ((CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y - collisionSafeZone, rCollisionRectangle.Width, collisionSafeZone), typeof(CWall), true) != null && fVerSpeed < 0)||
                (CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y + rCollisionRectangle.Height, rCollisionRectangle.Width, collisionSafeZone), typeof(CWall), true) != null && fVerSpeed > 0))
                fVerSpeed = 0;

            x += fHorSpeed;
            y += fVerSpeed;
        }

        public override void Render()
        {
            //CSprite.Instance.DrawRect(rCollisionRectangle, Color.Green);

            CSprite.Instance.Render("sprTest", x, y, 0, false, -degToRad(fAimDirection), 1.0f, Color.White);
        }

        public void MovementKeyboard(KeyboardState keyboardState)
        {
            //rename this var
            int maxSpeed = 2;

            //turning
            if (keyboardState.IsKeyDown(CSettings.Instance.kPTurnLeft))
                fAimDirection += 2.0f;
            else if (keyboardState.IsKeyDown(CSettings.Instance.kPTurnRight))
                fAimDirection -= 2.0f;

            //moving forward/backward
            if (keyboardState.IsKeyDown(CSettings.Instance.kPMoveForward))
                iFSpeed = maxSpeed;
            else if (keyboardState.IsKeyDown(CSettings.Instance.kPMoveBackward))
                iFSpeed = -maxSpeed;
            else
                iFSpeed = 0;

            //strafing??
            if (keyboardState.IsKeyDown(CSettings.Instance.kPStrafeLeft))
                iSSpeed = maxSpeed;
            else if (keyboardState.IsKeyDown(CSettings.Instance.kPStrafeRight))
                iSSpeed = -maxSpeed;
            else
                iSSpeed = 0;
        }
    }
}
