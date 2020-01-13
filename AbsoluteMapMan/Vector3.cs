using System;
using System.Collections.Generic;
using System.Text;

namespace AbsoluteMapMan
{
    class Vector3
    {
        public double X;
        public double Y;
        public double Z;

        public Vector3 (double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override String ToString()
        {
            return X + ", " + Y + ", " + Z;
        }
    }
}
