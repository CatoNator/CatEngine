using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CatEd
{
    class Mathf
    {
        public static float degToRad(float degrees)
        {
            return degrees * ((float)Math.PI / 180.0f);
        }

        public static float radToDeg(float radians)
        {
            return radians * (180.0f / (float)Math.PI);
        }

        public static float distDirX(float dist, float dir)
        {
            return (float)(Math.Cos(dir) * dist);
        }

        public static float distDirY(float dist, float dir)
        {
            return (float)(-Math.Sin(dir) * dist);
        }

        public static float PointDirection(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Atan2((double)(y2 - y1), (double)(x2 - x1));
        }

        public static float PointDistance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow((double)(x2 - x1), 2) + Math.Pow((double)(y2 - y1), 2));
        }

        public static float Lerp(float f1, float f2, float amount)
        {
            return f1 + (f2 - f1) * amount;
        }

        public static Vector2 Lerp(Vector2 V1, Vector2 V2, float amount)
        {
            float retX = Lerp(V1.X, V2.X, amount);
            float retY = Lerp(V1.Y, V2.Y, amount);
            return new Vector2(retX, retY);
        }

        public Vector3 Lerp(Vector3 V1, Vector3 V2, float amount)
        {
            float retX = Lerp(V1.X, V2.X, amount);
            float retY = Lerp(V1.Y, V2.Y, amount);
            float retZ = Lerp(V1.Z, V2.Z, amount);
            return new Vector3(retX, retY, retZ);
        }
    }
}
