using Godot;
using System;
using System.Collections.Generic;
using Test.MazeCreation;
using Test.Utility;
using Test.MeshUtilities;

namespace TestMaze
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");


            var maze = new Maze(ShapeGeometry.MakePolygon(6, 21, (float)Math.PI / 2), new Vector2(-21, -21), new Vector2(21, 21));

            //var maze = new Maze(new List<Vector2>() { new Vector2(-10, -10), new Vector2(10, -10), new Vector2(10, 10), new Vector2(-10, 10) }, new Vector2(-21, -21), new Vector2(21, 21));
        }
    }
}
