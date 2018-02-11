using System;
namespace Test
{
    public class Vector
    {
        public decimal x;
        public decimal y;
        public decimal z;
        public Vector(decimal localX, decimal localY, decimal localZ)
        {
            x = localX;
            y = localY;
            z = localZ;
        }


        public static Vector FromPoints(Point p1, Point p2)
        {
            return new Vector(
                p2.x - p1.x,
                p2.y - p1.y,
                p2.z - p1.z
            );
        }

        // https://www.khronos.org/opengl/wiki/Calculating_a_Surface_Normal
        // Set Vector U to (Triangle.p2 minus Triangle.p1)
        // Set Vector V to (Triangle.p3 minus Triangle.p1)
        // Set Normal.x to (multiply U.y by V.z) minus (multiply U.z by V.y)
        // Set Normal.y to (multiply U.z by V.x) minus (multiply U.x by V.z)
        // Set Normal.z to (multiply U.x by V.y) minus (multiply U.y by V.x)
        public static Vector CalculateSurfaceNormal(Point p1, Point p2, Point p3)
        {
            var U = FromPoints(p1, p2);
            var V = FromPoints(p1, p3);

            var N = new Vector(
                    U.y * V.z - U.z * V.y,
                    U.z * V.x - U.x * V.z,
                    U.x * V.y - U.y * V.x
                );

            return N;
        }


        public static bool PointingAwayFromOrigin(Point p, Vector v)
        {
            return ((p.x * v.x) >= 0) && ((p.y * v.y) >= 0) && ((p.z * v.z) >= 0);
        }

        public static Vector NormalizeVector(Vector v)
        {
            var m = (decimal)Math.Sqrt((double)((v.x * v.x) + (v.y * v.y) + (v.z * v.z)));

            return new Vector(
                (v.x / m),
                (v.y / m),
                (v.z / m)
            );
        }
    }
}
