using System;
using System.Collections.Generic;
namespace Test
{
    public class Face
    {
        static int _faceCount = 0;
        public int id;
        public List<Point> points;
        Point centroid = null;

        public Face(Point point1, Point point2, Point point3, bool register = true)
        {
            this.id = _faceCount++;

            this.points = new List<Point>{
                point1,
                point2,
                point3
            };
            if (register)
            {
                point1.registerFace(this);
                point2.registerFace(this);
                point3.registerFace(this);
            }
        }

        public List<Point> getOtherPoints(Point point1)
        {
            var other = new List<Point>();
            for (var i = 0; i < this.points.Count; i++)
            {
                if (this.points[i].toString().Equals(point1.toString()) == false)
                {
                    other.Add(this.points[i]);
                }
            }
            return other;
        }

        public Point findThirdPoint(Point point1, Point point2)
        {
            for (var i = 0; i < this.points.Count; i++)
            {
                if (this.points[i].toString().Equals(point1.toString()) == false && this.points[i].toString().Equals(point2.toString()) == false)
                {
                    return this.points[i];
                }
            }
            return null;
        }

        public bool isAdjacentTo(Face face2)
        {
            // adjacent if 2 of the points are the same

            var count = 0;
            for (var i = 0; i < this.points.Count; i++)
            {
                for (var j = 0; j < face2.points.Count; j++)
                {
                    if (this.points[i].toString() == face2.points[j].toString())
                    {
                        count++;

                    }
                }
            }

            return (count == 2);
        }

        public Point getCentroid(bool clear = false)
        {
            if (this.centroid != null && !clear)
            {
                return this.centroid;
            }

            var x = (this.points[0].x + this.points[1].x + this.points[2].x) / 3;
            var y = (this.points[0].y + this.points[1].y + this.points[2].y) / 3;
            var z = (this.points[0].z + this.points[1].z + this.points[2].z) / 3;

            this.centroid = new Point(x, y, z);

            return this.centroid;

        }
    }
}