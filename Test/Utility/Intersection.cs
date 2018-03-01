using System;
using Godot;
using System.Collections.Generic;

namespace Test.Utility
{
    public static class Intersection
    {
        //Returns true if polygons intersect.
        public static bool DoPolygonsIntersect(List<Vector2> polygon1, List<Vector2> polygon2) {
            for (var polygon1Index = 0; polygon1Index < polygon1.Count; polygon1Index++) {
                var polygon1Point1 = polygon1[polygon1Index];
                Vector2 polygon1Point2;
                if (polygon1Index > 0) {
                    polygon1Point2 = polygon1[polygon1Index - 1];
                } else {
                    polygon1Point2 = polygon1[polygon1.Count - 1];
                }

                for (var polygon2Index = 0; polygon2Index < polygon2.Count; polygon2Index++)
                {
                    var polygon2Point1 = polygon2[polygon2Index];
                    Vector2 polygon2Point2;
                    if (polygon2Index > 0)
                    {
                        polygon2Point2 = polygon2[polygon2Index - 1];
                    }
                    else
                    {
                        polygon2Point2 = polygon2[polygon2.Count - 1];
                    }
                    if (DoLineSegmentsIntersection(polygon1Point1, polygon1Point2, polygon2Point1, polygon2Point2)) {
                        return true;
                    }
                }

            }
            return false;
        }

        //Returns true if the lines intersect.
        public static bool DoLineSegmentsIntersection(Vector2 point1, Vector2 point2, Vector2 point3, Vector2 point4)
        {
            return DoLineSegmentsIntersection(point1.x, point1.y, point2.x, point2.y,
                                              point3.x, point3.y, point4.x, point4.y);
        }

        //https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect
        //Returns true if the lines intersect.
        public static bool DoLineSegmentsIntersection(float p0_x, float p0_y, float p1_x, float p1_y,
            float p2_x, float p2_y, float p3_x, float p3_y)
        {
            //float i_x;
            //float i_y;
            float s1_x, s1_y, s2_x, s2_y;
            s1_x = p1_x - p0_x; s1_y = p1_y - p0_y;
            s2_x = p3_x - p2_x; s2_y = p3_y - p2_y;

            float s, t;
            s = (-s1_y * (p0_x - p2_x) + s1_x * (p0_y - p2_y)) / (-s2_x * s1_y + s1_x * s2_y);
            t = (s2_x * (p0_y - p2_y) - s2_y * (p0_x - p2_x)) / (-s2_x * s1_y + s1_x * s2_y);

            if (s >= 0 && s <= 1 && t >= 0 && t <= 1)
            {
                // Collision detected
                //if (i_x != NULL)
                //    i_x = p0_x + (t * s1_x);
                //if (i_y != NULL)
                //    i_y = p0_y + (t * s1_y);
                return true;
            }

            return false; // No collision
        }
    }
}