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
    class CEnemy : CGameObject
    {
        public float fAimDirection = 0.0f;

        private int iFSpeed = 0;

        private Vector2 vTarget;

        private bool bChasing = false;

        private int iShotCooldown = 20;

        private int iShotTimer = 0;

        private int timer = 120;

        public override void InstanceSpawn()
        {
            vCollisionOrigin = new Vector2(16, 16);
            rCollisionRectangle = new Rectangle(0, 0, 32, 32);
            vTarget = new Vector2(this.x, this.y);
        }

        public override void Update()
        {
            UpdateCollision();

            //wall collision
            //CGameObject collision = CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y + rCollisionRectangle.Height, rCollisionRectangle.Width, 1), typeof(CWall), true);

            CGameObject player = FindInstance(typeof(CPlayer));

            bool playerInSight = FreeSightline(new Vector2(player.x, player.y));


            if (timer <= 0)
            {
                if (playerInSight)
                {
                    //fire gun
                    if (iShotTimer >= iShotCooldown)
                    {
                        CEnemyBullet bullet = (CEnemyBullet)CObjectManager.Instance.CreateInstance(typeof(CEnemyBullet), this.x, this.y);
                        bullet.SetAimdir(fAimDirection);
                        iShotTimer = 0;
                    }
                    else
                        iShotTimer++;

                    iFSpeed = 0;
                    vTarget = new Vector2(player.x, player.y);
                    Debug.Print("set target to " + vTarget.X + " " + vTarget.Y);
                    fAimDirection = -(float)PointDirection(this.x, this.y, vTarget.X, vTarget.Y);
                    bChasing = true;
                }
                else
                {

                    if (bChasing)
                    {
                        ChaseTarget(vTarget, 1);

                        Rectangle rect = new Rectangle((int)vTarget.X - 2, (int)vTarget.Y - 2, 4, 4);

                        if (rect.Contains((int)x, (int)y))
                        {
                            iFSpeed = 0;
                            bChasing = false;
                            Debug.Print("stopped chasing");
                        }
                    }

                }
            }
            else
            {
                timer--;
            }

            fHorSpeed = (float)distDirX((float)iFSpeed, fAimDirection);
            fVerSpeed = (float)distDirY((float)iFSpeed, fAimDirection);

            //note! current collision model only supports recantular collisions, no pixel perfect shapes
            //collision always gets stuck, needs adjusting

            //int collisionSafeZone = 4;

            //collisions to the left and right
            /*if ((CollisionRectangle(new Rectangle(rCollisionRectangle.X - collisionSafeZone, rCollisionRectangle.Y, collisionSafeZone, rCollisionRectangle.Height), typeof(CWall), true) != null && fHorSpeed < 0) ||
              (CollisionRectangle(new Rectangle(rCollisionRectangle.X + rCollisionRectangle.Width, rCollisionRectangle.Y, collisionSafeZone, rCollisionRectangle.Height), typeof(CWall), true) != null && fHorSpeed > 0))
                fHorSpeed = 0;

            //collisions above and below
            if ((CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y - collisionSafeZone, rCollisionRectangle.Width, collisionSafeZone), typeof(CWall), true) != null && fVerSpeed < 0) ||
                (CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y + rCollisionRectangle.Height, rCollisionRectangle.Width, collisionSafeZone), typeof(CWall), true) != null && fVerSpeed > 0))
                fVerSpeed = 0;*/

            CGameObject coll = CollisionCircle(typeof(CWall), 16);

            while (coll != null)
            {
                x += (float)distDirX((float)2, -(float)PointDirection(this.x, this.y, coll.x, coll.y));
                y += (float)distDirY((float)2, -(float)PointDirection(this.x, this.y, coll.x, coll.y));
                coll = CollisionCircle(typeof(CWall), 16);
            }
            
            x += fHorSpeed;
            y += fVerSpeed;
        }

        public override void Render()
        {
            //CSprite.Instance.DrawRect(rCollisionRectangle, Color.Green);

            CSprite.Instance.Render("sprEnemyTest", x, y, 0, false, -fAimDirection, 1.0f, Color.White);
        }

        private void ChaseTarget(Vector2 target, int speed)
        {
            fAimDirection = -(float)PointDirection(this.x, this.y, target.X, target.Y);
            //Debug.Print("atan angle in deg " + (fAimDirection * 180 / Math.PI));
            iFSpeed = speed;
        }

        private bool FreeSightline(Vector2 target)
        {
            bool success = !CollisionLine((int)this.x, (int)this.y, (int)target.X, (int)target.Y, typeof(CWall));
            return success;
        }

        private Vector2 EnemyCollision(Type instanceType, double collisionRadius)
        {
            CGameObject collidedInstance = null;
            CGameObject otherInstance = null;

            Vector2 collisionLocation = new Vector2(this.x, this.y);

            //getting the object to collide with

            //looping through the object list
            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                //if the other object type is the one we're looking for
                if ((CObjectManager.Instance.pGameObjectList[i] != null)
                    && Object.ReferenceEquals(instanceType, CObjectManager.Instance.pGameObjectList[i].GetType()))
                {
                    //getting the reference
                    otherInstance = CObjectManager.Instance.pGameObjectList[i];

                    for (int e = 0; e <= 360; e++)
                    {
                        int pointx = (int)this.x + (int)distDirX((float)collisionRadius, (float)e);
                        int pointy = (int)this.y + (int)distDirY((float)collisionRadius, (float)e);

                        if (otherInstance.rCollisionRectangle.Contains(pointx, pointy))
                        {
                            collisionLocation = new Vector2((float)pointx, (float)pointy);
                            break;
                        }
                    }

                }
            }

            return collisionLocation;
        }
    }
}
