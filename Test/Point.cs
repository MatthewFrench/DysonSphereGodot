using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

namespace Test
{
    public class Point
    {
        // Member variables here, example:
        // private int a = 2;
        // private string b = "textvar";
        // GD.Print("Hello from the Point CS Script Init");
        public decimal x, y, z;
        public List<Face> faces;

        public Point(decimal x = 0, decimal y = 0, decimal z = 0)
        {
            /*
            this.x = limitDecimals(x, 3);
            this.y = limitDecimals(y, 3);
            this.z = limitDecimals(z, 3);
            */
            this.x = x;
            this.y = y;
            this.z = z;
            this.faces = new List<Face>();
        }

        public static decimal limitDecimals(decimal x, int numberOfDecimals)
        {
            decimal trimAmount = (decimal)Math.Pow(10, numberOfDecimals);
            return Math.Truncate(x * trimAmount) / trimAmount;
        }

        public List<Point> subdivide(Point point, decimal count, Func<Point, Point> checkPoint)
        {

            var segments = new List<Point>();
            segments.Add(this);

            for (var i = 1; i < count; i++)
            {
                var np = new Point(this.x * (1 - (i / count)) + point.x * (i / count),
                    this.y * (1 - (i / count)) + point.y * (i / count),
                    this.z * (1 - (i / count)) + point.z * (i / count));
                np = checkPoint(np);
                segments.Add(np);
            }

            segments.Add(point);

            return segments;

        }

        public Point segment(Point point, decimal percent)
        {
            if (percent < 0.01M) { percent = 0.01M;}
            if (percent > 1M) { percent = 1M; }

            decimal localX = point.x * (1 - percent) + this.x * percent;
            decimal localY = point.y * (1 - percent) + this.y * percent;
            decimal localZ = point.z * (1 - percent) + this.z * percent;

            var newPoint = new Point(localX, localY, localZ);
            return newPoint;

        }
        /* Doesn't seem to be used.
        public void midpoint(Point point, location)
        {
            return this.segment(point, .5);
        }
        */


        public Point project(decimal radius, decimal percent = 1.0M)
        {
            percent = Math.Max(0, Math.Min(1, percent));
            //var yx = this.y / this.x;
            //var zx = this.z / this.x;
            //var yz = this.z / this.y;

            var mag = (decimal)Math.Sqrt((double)(this.x*this.x + this.y*this.y + this.z*this.z));
            var ratio = radius / mag;

            this.x = this.x * ratio * percent;
            this.y = this.y * ratio * percent;
            this.z = this.z * ratio * percent;
            return this;

        }

        public void registerFace(Face face)
        {
            this.faces.Add(face);
        }

        public List<Face> getOrderedFaces()
        {
            var workingArray = new List<Face>(this.faces);
            var ret = new List<Face>();

            var i = 0;
            while (i < this.faces.Count)
            {
                if (i == 0)
                {
                    ret.Add(workingArray[i]);
                    workingArray.RemoveAt(i);
                }
                else
                {
                    var hit = false;
                    var j = 0;
                    while (j < workingArray.Count && !hit)
                    {
                        if (workingArray[j].isAdjacentTo(ret[i - 1]))
                        {
                            hit = true;
                            ret.Add(workingArray[j]);
                            workingArray.RemoveAt(j);
                        }
                        j++;
                    }
                }
                i++;
            }

            return ret;
        }

        public Face findCommonFace(Point other, Face notThisFace)
        {
            for (var i = 0; i < this.faces.Count; i++)
            {
                for (var j = 0; j < other.faces.Count; j++)
                {
                    if (this.faces[i].id == other.faces[j].id && this.faces[i].id != notThisFace.id)
                    {
                        return this.faces[i];
                    }
                }
            }

            return null;
        }

        public Dictionary<string, decimal> toJson()
        {
            return new Dictionary<string, decimal>
            {
                { "x", this.x},
                { "y", this.y},
                { "z", this.z}
            };
        }

        public String toString()
        {
            return "" + this.x + "," + this.y + "," + this.z;

            //return "" + limitDecimals(this.x, 3) + "," + limitDecimals(this.y, 3) + "," + limitDecimals(this.z, 3);
        }
    }
}