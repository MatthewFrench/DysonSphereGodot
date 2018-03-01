using System;
using Godot;
using System.Collections.Generic;

namespace Test.Utility
{
    public static class ShapeGeometry
    {
        public static List<Vector2> MakePolygon(int sides, float radius, float initialRotation = 0) {
            var points = new List<Vector2>();
            for (var index = 0; index < sides; index++) {
                var x = radius * Math.Cos(2 * Math.PI * index / sides + initialRotation);
                var y = radius * Math.Sin(2 * Math.PI * index / sides + initialRotation);
                points.Add(new Vector2((float)x, (float)y));
            }
            return points;
        }
    }
}
