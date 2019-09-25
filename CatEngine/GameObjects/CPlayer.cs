﻿using System;
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
        
        public override void InstanceSpawn()
        {
            vCollisionOrigin = new Vector2(8, 8);
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

            //capping the player rotation to [0.0f, 360.0f]
            fAimDirection = fAimDirection % 360;

            //Debug.Print("player aim dir " + fAimDirection);

            //fHorSpeed = (float)distDirX((float)iFSpeed, degToRad(fAimDirection)) + (float)distDirX((float)iSSpeed, degToRad(fAimDirection+90.0f));
            //fVerSpeed = (float)distDirY((float)iFSpeed, degToRad(fAimDirection)) + (float)distDirY((float)iSSpeed, degToRad(fAimDirection+90.0f));

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
            float dir = (float)PointDirection(0, 0, fHorSpeed, fVerSpeed);

            CSprite.Instance.Render("sprTest", x, y, 0, false, dir, 1.0f, Color.White);
        }

        public void MovementKeyboard(KeyboardState keyboardState)
        {
            float maxSpeed = 2.0f;
            
            //moving
            //up
            if (keyboardState.IsKeyDown(CSettings.Instance.kPMoveForward))
            {
                fHorSpeed = 0;
                fVerSpeed = -maxSpeed;
            }
            //down
            else if (keyboardState.IsKeyDown(CSettings.Instance.kPMoveBackward))
            {
                fHorSpeed = 0;
                fVerSpeed = maxSpeed;
            }
            //left
            else if(keyboardState.IsKeyDown(CSettings.Instance.kPTurnLeft))
            {
                fHorSpeed = -maxSpeed;
                fVerSpeed = 0;
            }
            //right
            else if (keyboardState.IsKeyDown(CSettings.Instance.kPTurnRight))
            {
                fHorSpeed = maxSpeed;
                fVerSpeed = 0;
            }
            //DEBUG!!!!!!!!!!!!!
            else
            {
                fHorSpeed = 0;
                fVerSpeed = 0;
            }
        }
    }
}
