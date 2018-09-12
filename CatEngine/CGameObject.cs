using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CatEngine
{
    public class CGameObject
    {
        public bool bActive = true;

        public float x;
        public float y;

        public int iIndex;

        public float fHorSpeed;
        public float fVerSpeed;

        public float fDirection;
        public float fVelocity;

        public Vector2 vCollisionOrigin;
        public Rectangle rCollisionRectangle;

        public Random myRandom = new Random();

        public CGameObject()
        {
        }

        //creating the instance
        public void Spawn(float t_x, float t_y, int index)
        {
            this.x = t_x;
            this.y = t_y;
            this.fHorSpeed = 0;
            this.fVerSpeed = 0;

            this.iIndex = index;

            this.fDirection = 0;

            this.fVelocity = 0;

            InstanceSpawn();
        }

        //object-specific spawning code, called from Spawn() and only from Spawn()
        public virtual void InstanceSpawn()
        {
        }

        //call this code when destroying the instance
        public virtual void OnDestruction()
        {
        }

        //trig math
        public float degToRad(float degrees)
        {
            return degrees * ((float)Math.PI) / 180.0f;
        }

        public double distDirX(float dist, float dir)
        {
            return (Math.Cos(dir) * dist);
        }

        public double distDirY(float dist, float dir)
        {
            return (-Math.Sin(dir) * dist);
        }

        public double pointDirection(float x1, float y1, float x2, float y2)
        {
            return Math.Atan2((double)(y2 - y1), (double)(x2 - x1));
        }

        //how many instances of a certain type are there
        public int InstanceNumber(Type instanceType)
        {
            int instances = 0;

            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                //if the other object type is the one we're looking for
                if ((CObjectManager.Instance.pGameObjectList[i] != null)
                    && Object.ReferenceEquals(instanceType, CObjectManager.Instance.pGameObjectList[i].GetType()))
                    instances++;
            }

            return instances;
        }

        //deactivating instances (stops updating but not rendering)
        public void DeactivateInstances(bool notMe)
        {
            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                //if the other object type is the one we're looking for
                if (CObjectManager.Instance.pGameObjectList[i] != null)
                {
                    if (notMe && i != iIndex)
                        CObjectManager.Instance.pGameObjectList[i].bActive = false;
                }
            }
        }

        //reactivating the instances
        public void ActivateInstances()
        {
            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                //if the other object type is the one we're looking for
                if (CObjectManager.Instance.pGameObjectList[i] != null)
                {
                    CObjectManager.Instance.pGameObjectList[i].bActive = true;
                }
            }
        }

        //the step code called once per frame
        public virtual void Update()
        {
            /*if (iLifeTime <= 0)
                CObjectManager.Instance.DestroyInstance(iIndex);
            else
                iLifeTime--;*/

            //Console.WriteLine("Player objects: " + InstanceNumber(typeof(CPlayer)));
        }

        //does a specific type exist?
        public bool InstanceExists(Type instanceType)
        {
            bool success = false;

            //getting the object to collide with

            //looping through the object list
            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                //if the other object type is the one we're looking for
                if ((CObjectManager.Instance.pGameObjectList[i] != null)
                    && Object.ReferenceEquals(instanceType, CObjectManager.Instance.pGameObjectList[i].GetType()))
                {
                    //getting the reference
                    success = true;
                }
            }

            return success;
        }

        //updating the collision
        public void UpdateCollision()
        {
            this.rCollisionRectangle.X = (int)this.x-(int)vCollisionOrigin.X;
            this.rCollisionRectangle.Y = (int)this.y - (int)vCollisionOrigin.Y;
        }

        //circle collider from Super Starblasters
        public CGameObject CollisionCircle(Type instanceType, double collisionDistance)
        {
            CGameObject collidedInstance = null;
            CGameObject otherInstance = null;

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

                    //measuring the distance
                    double dist = (float)Math.Sqrt(Math.Pow(this.x - otherInstance.x, 2) + Math.Pow(this.y - otherInstance.y, 2));

                    //if distance is smaller than the collision radiuses
                    if (dist <= collisionDistance)
                        collidedInstance = otherInstance;
                }
            }

            return collidedInstance;
        }

        //bounding box collision
        public CGameObject BboxMeeting(Type instanceType, bool notMe)
        {
            CGameObject collidedInstance = null;
            CGameObject otherInstance = null;

            //looping through the gameobjectlist
            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                //if the other object type is the one we're looking for
                if ((CObjectManager.Instance.pGameObjectList[i] != null) &&
                    (notMe && CObjectManager.Instance.pGameObjectList[i] != this) &&
                    Object.ReferenceEquals(instanceType, CObjectManager.Instance.pGameObjectList[i].GetType()))
                {
                    //getting the reference
                    otherInstance = CObjectManager.Instance.pGameObjectList[i];

                    if (this.rCollisionRectangle.Intersects(otherInstance.rCollisionRectangle) ||
                        this.rCollisionRectangle.Contains(otherInstance.rCollisionRectangle))
                        collidedInstance = otherInstance;
                }
            }

            return collidedInstance;
        }

        public CGameObject CollisionRectangle(Rectangle rectangle, Type instanceType, bool notMe)
        {
            CGameObject collidedInstance = null;
            CGameObject otherInstance = null;

            //looping through the gameobjectlist
            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                //if the other object type is the one we're looking for
                if ((CObjectManager.Instance.pGameObjectList[i] != null) &&
                    (notMe && CObjectManager.Instance.pGameObjectList[i] != this) &&
                    Object.ReferenceEquals(instanceType, CObjectManager.Instance.pGameObjectList[i].GetType()))
                {
                    //getting the reference
                    otherInstance = CObjectManager.Instance.pGameObjectList[i];

                    //checking if the rectangles overlap
                    if (rectangle.Intersects(otherInstance.rCollisionRectangle) ||
                        rectangle.Contains(otherInstance.rCollisionRectangle))
                        collidedInstance = otherInstance;
                }
            }

            return collidedInstance;
        }

        //raycast collider
        public Vector2 RaycastCollider(int startX, int startY, int destX, int destY, Type instanceType)
        {
            bool hasHit = false;
            Vector2 hitCoords = new Vector2(destX, destY);

            //measuring the distance between the two points
            double dist = (float)Math.Sqrt(Math.Pow(destX - startX, 2) + Math.Pow(destY - startY, 2));
            float dir = 0;

            //looping through the gameobject list
            for (int e = 0; e < CObjectManager.MAX_INSTANCES; e++)
            {
                //if the other object type is the one we're looking for and exists
                if (CObjectManager.Instance.pGameObjectList[e] != null
                    && Object.ReferenceEquals(instanceType, CObjectManager.Instance.pGameObjectList[e].GetType()))
                {
                    //looping through all the positions on the ray
                    for (int i = 0; i < dist; i++)
                    {
                        float xpos = (float)startX + (float)distDirX((float)dist, dir);
                        float ypos = (float)startY + (float)distDirY((float)dist, dir);
                        
                        //if a position on the ray is inside the object's rectangle collider
                        if (CObjectManager.Instance.pGameObjectList[e].rCollisionRectangle.Contains(xpos, ypos) && !hasHit)
                        {
                            //return the collision position
                            hitCoords = new Vector2(xpos, ypos);
                            hasHit = true;
                        }
                    }
                }
            }

            return hitCoords;
        }

        //point collider
        public CGameObject PointCollider(int collX, int collY, Type instanceType)
        {
            CGameObject collidedInstance = null;
            CGameObject otherInstance = null;

            //looping through the gameobject list
            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                //if the other object type is the one we're looking for and exists
                if (CObjectManager.Instance.pGameObjectList[i] != null
                    && Object.ReferenceEquals(instanceType, CObjectManager.Instance.pGameObjectList[i].GetType()))
                {
                    otherInstance = CObjectManager.Instance.pGameObjectList[i];

                    //if the point is inside the collision rectangle
                    if (otherInstance.rCollisionRectangle.Contains(collX, collY))
                        collidedInstance = otherInstance;
                }
            }

            return collidedInstance;
        }

        //the rendering code called once per frame
        public virtual void Render()
        {
        }
    }
}