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
    class CBall : CGameObject
    {
        private bool bStopped = true;

        private int iStopTimer = 60;
        
        public override void InstanceSpawn()
        {
            vCollisionOrigin = new Vector2(4, 4);
            rCollisionRectangle = new Rectangle(0, 0, 8, 8);
            UpdateCollision();

            fHorSpeed = 2.0f - (float)(myRandom.NextDouble()*4.0);
            fVerSpeed = -2.0f;
        }

        public override void Update()
        {
            UpdateCollision();

            //floor collision
            CGameObject collision = CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y + rCollisionRectangle.Height, rCollisionRectangle.Width, 1), typeof(CWall), true);
            
            //collisions to the left and right
            if (testHorCollision())
                fHorSpeed = -fHorSpeed;

            //collisions above and below
            if (testVerCollision())
                fVerSpeed = -fVerSpeed;

            if (!bStopped)
            {
                x += fHorSpeed;
                y += fVerSpeed;
            }
            else
            {
                iStopTimer--;
                if (iStopTimer <= 0)
                    bStopped = false;
            }

            if (y > 210)
            {
                CObjectManager.Instance.DestroyInstance(iIndex);
                CGameManager.Instance.iLives--;
            }
        }

        private bool testHorCollision()
        {
            int collisionSafeZone = 2;

            CGameObject horBrickCol = CollisionRectangle(new Rectangle(rCollisionRectangle.X - collisionSafeZone, rCollisionRectangle.Y, collisionSafeZone, rCollisionRectangle.Height), typeof(CWall), true);
            CGameObject verBrickCol = CollisionRectangle(new Rectangle(rCollisionRectangle.X + rCollisionRectangle.Width, rCollisionRectangle.Y, collisionSafeZone, rCollisionRectangle.Height), typeof(CWall), true);

            if (horBrickCol != null)
            {
                CObjectManager.Instance.DestroyInstance(horBrickCol.iIndex);
            }

            if (verBrickCol != null)
            {
                CObjectManager.Instance.DestroyInstance(verBrickCol.iIndex);
            }

            bool brickCollision = ((horBrickCol != null && fHorSpeed < 0) || (verBrickCol != null && fHorSpeed > 0));
            
            bool playerCollision = ((CollisionRectangle(new Rectangle(rCollisionRectangle.X - collisionSafeZone, rCollisionRectangle.Y, collisionSafeZone, rCollisionRectangle.Height), typeof(CPlayer), true) != null && fHorSpeed < 0) ||
              (CollisionRectangle(new Rectangle(rCollisionRectangle.X + rCollisionRectangle.Width, rCollisionRectangle.Y, collisionSafeZone, rCollisionRectangle.Height), typeof(CPlayer), true) != null && fHorSpeed > 0));
            
            bool outOfScreen = (x <= 0 || x >= 416);

            return brickCollision || playerCollision || outOfScreen;
        }

        private bool testVerCollision()
        {
            int collisionSafeZone = 2;

            CGameObject horBrickCol = CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y - collisionSafeZone, rCollisionRectangle.Width, collisionSafeZone), typeof(CWall), true);
            CGameObject verBrickCol = CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y + rCollisionRectangle.Height, rCollisionRectangle.Width, collisionSafeZone), typeof(CWall), true);

            if (horBrickCol != null)
            {
                CObjectManager.Instance.DestroyInstance(horBrickCol.iIndex);
            }

            if (verBrickCol != null)
            {
                CObjectManager.Instance.DestroyInstance(horBrickCol.iIndex);
            }

            bool brickCollision = ((horBrickCol != null && fVerSpeed < 0) || (verBrickCol != null && fVerSpeed > 0));

            bool playerCollision = (CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y - collisionSafeZone, rCollisionRectangle.Width, collisionSafeZone), typeof(CPlayer), true) != null && fVerSpeed < 0) ||
                (CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y + rCollisionRectangle.Height, rCollisionRectangle.Width, collisionSafeZone), typeof(CPlayer), true) != null && fVerSpeed > 0);

            bool outOfScreen = (y <= 0 || y >= 240);

            return brickCollision || playerCollision || outOfScreen;
        }

        public override void Render()
        {
            CSprite.Instance.DrawRect(rCollisionRectangle, Color.Green);

            //CSprite.Instance.Render("sprPlayer", x, y, 0, false, 0, 1.0f, Color.White);
        }
    }
}
