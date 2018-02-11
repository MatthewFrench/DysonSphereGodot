using System;
using System.Collections.Generic;

namespace Test
{
    public class Tile
    {
        public Point centerPoint;
        public List<Face> faces;
        public List<Point> boundary;
        public List<Point> neighborIds;
        public List<Point> neighbors;

        public Tile(Point centerPoint, decimal hexSize = 1)
        {
            if (hexSize < 0.01M) {
                hexSize = 0.01M;
            }
            if (hexSize > 1M)
            {
                hexSize = 1M;
            }

            this.centerPoint = centerPoint;
            this.faces = centerPoint.getOrderedFaces();
            this.boundary = new List<Point>();
            this.neighborIds = new List<Point>(); // this holds the centerpoints, will resolve to references after
            this.neighbors = new List<Point>(); // this is filled in after all the tiles have been created

            var neighborHash = new Dictionary<Point, int>();
            for (var f = 0; f < this.faces.Count; f++)
            {
                // build boundary
                this.boundary.Add(this.faces[f].getCentroid().segment(this.centerPoint, hexSize));

                // get neighboring tiles
                var otherPoints = this.faces[f].getOtherPoints(this.centerPoint);
                for (var o = 0; o < 2; o++)
                {
                    neighborHash[otherPoints[o]] = 1;
                }

            }

            this.neighborIds = new List<Point>(neighborHash.Keys);

            // Some of the faces are pointing in the wrong direction
            // Fix this.  Should be a better way of handling it
            // than flipping them around afterwards

            var normal = Vector.CalculateSurfaceNormal(this.boundary[1], this.boundary[2], this.boundary[3]);

            if (!Vector.PointingAwayFromOrigin(this.centerPoint, normal))
            {
                this.boundary.Reverse();
            }
        }

        public Tuple<decimal, decimal> getLatLon(decimal radius, int boundaryNum = -1)
        {
            var point = this.centerPoint;
            if (boundaryNum >= 0 && boundaryNum < this.boundary.Count)
            {
                point = this.boundary[boundaryNum];
            }
            decimal phi = (decimal)Math.Acos((double)(point.y / radius)); //lat 
            decimal theta = (decimal)((Math.Atan2((double)point.x, (double)point.z) + Math.PI + Math.PI / 2) % (Math.PI * 2) - Math.PI); // lon

            // theta is a hack, since I want to rotate by Math.PI/2 to start.  sorryyyyyyyyyyy
            return new Tuple<decimal, decimal>(
                (decimal)(180 * (double)phi / Math.PI - 90),
                (decimal)(180 * (double)theta / Math.PI)
            );
        }

        public List<Point> scaledBoundary(decimal scale)
        {

            scale = Math.Max(0, Math.Min(1, scale));

            var ret = new List<Point>();
            for (var i = 0; i < this.boundary.Count; i++)
            {
                ret.Add(this.centerPoint.segment(this.boundary[i], 1 - scale));
            }

            return ret;
        }

        public Dictionary<string, dynamic> toJson()
        {
            // this.centerPoint = centerPoint;
            // this.faces = centerPoint.getOrderedFaces();
            // this.boundary = [];
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["centerPoint"] = this.centerPoint.toJson();
            dictionary["boundary"] = this.boundary.ConvertAll(point => point.toJson()).ToArray();
            return dictionary;
        }

        public String toString()
        {
            return this.centerPoint.toString();
        }
    }
}