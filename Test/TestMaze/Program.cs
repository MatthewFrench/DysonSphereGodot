using Godot;
using System;
using System.Collections.Generic;
using Test.MazeCreation;

namespace TestMaze
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Hello World!");

            var maze = new Maze(new List<Vector2>() { new Vector2(-10, -10), new Vector2(10, -10), new Vector2(10, 10), new Vector2(-10, 10) }, new Vector2(-21, -21), new Vector2(21, 21));
        }
    }
}
