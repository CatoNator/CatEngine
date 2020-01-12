using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using CatEngine.Content;

namespace CatEngine
{
    class CCollidable : CGameObject
    {
        //floor heights
        /*
        0---1
        |   |
        |   |
        2---3
        */
        public float[] fFloorHeights = new float[4];
        //ceiling heights
        public float[] fCeilHeights = new float[4];

        public Vector2 fSize = new Vector2(5, 5);

        public override void InstanceSpawn()
        {
            for (int i = 0; i < 4; i++)
            {
                fFloorHeights[i] = z + 2.5f;
                fCeilHeights[i] = z - 2.5f;
            }
        }

        public override void Render()
        {
            CRender.Instance.DrawRectangle(new Vector3(x, fFloorHeights[2], y + fSize.Y),
                    new Vector3(x + fSize.X, fFloorHeights[3], y + fSize.Y),
                    new Vector3(x, fFloorHeights[0], y),
                    new Vector3(x + fSize.X, fFloorHeights[1], y), "grasstop", false, 1.0f);

            //rendering the south wall
            CRender.Instance.DrawRectangle(new Vector3(x , fCeilHeights[2], y+fSize.Y ),
                new Vector3(x+fSize.X , fCeilHeights[3], y+fSize.Y ),
                new Vector3(x , fFloorHeights[2], y+fSize.Y ),
                new Vector3(x+fSize.X , fFloorHeights[3], y+fSize.Y ), "grasstop", false, 1.0f);

            //and the wall to the right of us
            CRender.Instance.DrawRectangle(new Vector3(x+fSize.X , fFloorHeights[1], y ),
                new Vector3(x+fSize.X , fFloorHeights[3], y+fSize.Y ),
                new Vector3(x+fSize.X , fCeilHeights[1], y ),
                new Vector3(x+fSize.X , fCeilHeights[3], y+fSize.Y ), "grasstop", false, 1.0f);

            //if we're on the leftmost row, let's render y left wall
            CRender.Instance.DrawRectangle(new Vector3(x, fCeilHeights[0], y ),
                new Vector3(x, fCeilHeights[2], y+fSize.Y ),
                new Vector3(x, fFloorHeights[0], y ),
                new Vector3(x, fFloorHeights[2], y+fSize.Y ), "grasstop", true, 1.0f);

            //if we're on the top row, let's draw y top wall
            CRender.Instance.DrawRectangle(new Vector3(x , fFloorHeights[0], y),
                new Vector3(x+fSize.X , fFloorHeights[1], y),
                new Vector3(x , 0, y),
                new Vector3(x+fSize.X , 0, y), "grasstop", false, 1.0f);
        }
    }
}
