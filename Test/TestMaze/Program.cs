using Godot;
using System;
using System.Collections.Generic;
using Test.MazeCreation;
using Test.Utility;
using Test.MeshUtilities;
using System.Diagnostics;

namespace TestMaze
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Maze Benchmark!");

            Stopwatch benchmark = new Stopwatch();
            benchmark.Start();
            Maze maze;
            for (var i = 0; i < 1; i++) {
                maze = new Maze(ShapeGeometry.MakePolygon(6, 21, (float)Math.PI / 2), new Vector2(-21, -21), new Vector2(21, 21));
            }
            benchmark.Stop();
            Console.WriteLine("Elapsed time {0} ms", benchmark.ElapsedMilliseconds);

            //var maze = new Maze(new List<Vector2>() { new Vector2(-10, -10), new Vector2(10, -10), new Vector2(10, 10), new Vector2(-10, 10) }, new Vector2(-21, -21), new Vector2(21, 21));
        }
    }
}
