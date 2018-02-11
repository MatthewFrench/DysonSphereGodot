using System;
using System.Collections.Generic;
using Test;

namespace Test
{
    public class Hexasphere
    {
        public decimal radius;
        public List<Tile> tiles;
        public Dictionary<String, Tile> tileLookup;
        public Hexasphere(decimal radius, int numDivisions, double _hexSize)
        {
            decimal hexSize = (decimal)_hexSize;
            this.radius = radius;
            decimal tao = 1.61803399M;
            var corners = new List<Point>{
                new Point(1000, tao * 1000, 0),
                new Point(-1000, tao * 1000, 0),
                new Point(1000, -tao * 1000, 0),
                new Point(-1000, -tao * 1000, 0),
                new Point(0, 1000, tao * 1000),
                new Point(0, -1000, tao * 1000),
                new Point(0, 1000, -tao * 1000),
                new Point(0, -1000, -tao * 1000),
                new Point(tao * 1000, 0, 1000),
                new Point(-tao * 1000, 0, 1000),
                new Point(tao * 1000, 0, -1000),
                new Point(-tao * 1000, 0, -1000)
            };

            var points = new Dictionary<String, Point>();

            for (var i = 0; i < corners.Count; i++)
            {
                points[corners[i].toString()] = corners[i];
            }

            var faces = new List<Face> {
                new Face(corners[0], corners[1], corners[4], false),
                new Face(corners[1], corners[9], corners[4], false),
                new Face(corners[4], corners[9], corners[5], false),
                new Face(corners[5], corners[9], corners[3], false),
                new Face(corners[2], corners[3], corners[7], false),
                new Face(corners[3], corners[2], corners[5], false),
                new Face(corners[7], corners[10], corners[2], false),
                new Face(corners[0], corners[8], corners[10], false),
                new Face(corners[0], corners[4], corners[8], false),
                new Face(corners[8], corners[2], corners[10], false),
                new Face(corners[8], corners[4], corners[5], false),
                new Face(corners[8], corners[5], corners[2], false),
                new Face(corners[1], corners[0], corners[6], false),
                new Face(corners[11], corners[1], corners[6], false),
                new Face(corners[3], corners[9], corners[11], false),
                new Face(corners[6], corners[10], corners[7], false),
                new Face(corners[3], corners[11], corners[7], false),
                new Face(corners[11], corners[6], corners[7], false),
                new Face(corners[6], corners[0], corners[10], false),
                new Face(corners[9], corners[1], corners[11], false)
            };

            Func<Point, Point> getPointIfExists = (point) => {
                if (points.ContainsKey(point.toString()))
                {
                    // console.log("EXISTING!");
                    return points[point.toString()];
                }
                else
                {
                    // console.log("NOT EXISTING!");
                    points[point.toString()] = point;
                    return point;
                }
            };


            var newFaces = new List<Face>();

            for (var f = 0; f < faces.Count; f++)
            {
                // console.log("-0---");
                List<Point> prev = null;
                List<Point> bottom = new List<Point> { faces[f].points[0] };
                List<Point> left = faces[f].points[0].subdivide(faces[f].points[1], numDivisions, getPointIfExists);
                List<Point> right = faces[f].points[0].subdivide(faces[f].points[2], numDivisions, getPointIfExists);
                for (var i = 1; i <= numDivisions; i++)
                {
                    prev = bottom;
                    bottom = left[i].subdivide(right[i], i, getPointIfExists);
                    for (var j = 0; j < i; j++)
                    {
                        var nf = new Face(prev[j], bottom[j], bottom[j + 1]);
                        newFaces.Add(nf);

                        if (j > 0)
                        {
                            nf = new Face(prev[j - 1], prev[j], bottom[j]);
                            newFaces.Add(nf);
                        }
                    }
                }
            }

            faces = newFaces;

            Dictionary<String, Point> newPoints = new Dictionary<String, Point>();
            foreach (String p in points.Keys)
            {
                var np = points[p].project(radius);
                newPoints[np.toString()] = np;
            }

            points = newPoints;

            this.tiles = new List<Tile>();
            this.tileLookup = new Dictionary<String, Tile>();

            // create tiles and store in a lookup for references
            foreach(var p in points.Keys)
            {
                var newTile = new Tile(points[p], hexSize);
                this.tiles.Add(newTile);
                this.tileLookup[newTile.toString()] = newTile;
            }

            // resolve neighbor references now that all have been created
            /*
            foreach (Tile t in this.tiles)
            {
                t.neighbors = t.neighborIds.ConvertAll((Point item)=> _this.tileLookup[item]);
            }
            */
        }



        public Dictionary<string, dynamic> toJson()
        {
            var dictionary = new Dictionary<string, dynamic>();
            dictionary["radius"] = this.radius;
            dictionary["tiles"] = this.tiles.ConvertAll(tile => tile.toJson()).ToArray();
            return dictionary;
        }
    public List<Tile> GetTiles() {
        return this.tiles;
    }
    }
}
