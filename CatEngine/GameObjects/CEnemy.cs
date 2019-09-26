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

        private int iShotCooldown = 120;

        private int iShotTimer = 0;

        private int timer = 120;

        int iDir = 0;

        public override void InstanceSpawn()
        {
            vCollisionOrigin = new Vector2(0, 0);
            rCollisionRectangle = new Rectangle(0, 0, 16, 16);
            vTarget = new Vector2(this.x, this.y);
        }

        public override void Update()
        {
            UpdateCollision();

            //wall collision
            //CGameObject collision = CollisionRectangle(new Rectangle(rCollisionRectangle.X, rCollisionRectangle.Y + rCollisionRectangle.Height, rCollisionRectangle.Width, 1), typeof(CWall), true);

            CGameObject player = FindInstance(typeof(CPlayer));

            bool playerInSight = FreeSightline(new Vector2(player.x, player.y));

            EnemyMovement(1, new Vector2(player.x, player.y));

            /*if (timer <= 0)
            {
                EnemyMovement(1, new Vector2(player.x, player.y));

                Debug.Print("moved to location (" + x + ", " + y +")");

                timer = iShotCooldown;
            }
            else
            {
                timer--;
            }*/

            //if the ghost aligns with the grid

            //fHorSpeed = (float)distDirX(0.1f, fAimDirection);
            //fVerSpeed = (float)distDirY(0.1f, fAimDirection);

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

            /*CGameObject coll = CollisionCircle(typeof(CWall), 16);

            while (coll != null)
            {
                x += (float)distDirX((float)2, -(float)PointDirection(this.x, this.y, coll.x, coll.y));
                y += (float)distDirY((float)2, -(float)PointDirection(this.x, this.y, coll.x, coll.y));
                coll = CollisionCircle(typeof(CWall), 16);
            }*/

            x += fHorSpeed;
            y += fVerSpeed;
        }

        public override void Render()
        {
            CSprite.Instance.DrawRect(rCollisionRectangle, Color.Red);

            //CSprite.Instance.Render("sprEnemyTest", x, y, 0, false, -fAimDirection, 1.0f, Color.White);
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

        private int PickDirection(Vector2 target)
        {
            int dir = 0; //returns dir between 0-3, dir*90 = movemnt direction
            int gridX = (int)x / 16;
            int gridY = (int)y / 16;

            bool[] canMoveInDir = new bool[4];

            for (int i = 0; i <= 3; i++)
            {
                CGameObject col = CollisionRectangle(new Rectangle((int)x + (int)distDirX(16, degToRad(i * 90)), (int)y + (int)distDirY(16, degToRad(i * 90)), 16, 16), typeof(CWall), true);

                //check if grid tile ahead is free
                if (col == null && i != (iDir + 2) % 4)
                {
                    //it is free, so calculate the distance from said tile
                    canMoveInDir[i] = true;
                }
                else
                {
                    if (col != null)
                        Debug.Print("did not move in dir " + i + ", col was not null");
                    else if (i == (iDir + 2) % 4)
                        Debug.Print("did not move in dir " + i + ", it was backtracking");
                    else
                        Debug.Print("lol what the fuck");
                }
            }

            float min = 666666666;
            for (int i = 0; i <= 3; i++)
            {
                if (canMoveInDir[i])
                {
                    
                    float dist = (float)(Math.Pow((target.X - (x + distDirX(16, degToRad(i * 90)))), 2) + Math.Pow((target.Y - (y + distDirY(16, degToRad(i * 90)))), 2));

                    if (dist < min)
                    {
                        min = dist;
                        Debug.Print("dir " + i + " dist " + dist);
                        dir = i;
                    }
                }
            }

            return dir;
        }

        private void EnemyMovement(int spd, Vector2 Target)
        {
            if ((int)x % 16 == 0 && (int)y % 16 == 0)
            {
                //new direction is picked
                iDir = PickDirection(new Vector2(Target.X, Target.Y));
                Debug.Print("Picked new direction, " + iDir);
            }
            /*x += 16 * Math.Sign(distDirX(10, fAimDirection));
            y += 16 * Math.Sign(distDirY(10, fAimDirection));*/
            if (iDir == 0)
            {
                x += spd;
            }
            else if (iDir == 1)
            {
                y -= spd;
            }
            else if (iDir == 2)
            {
                x -= spd;
            }
            else if (iDir == 3)
            {
                y += spd;
            }
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
