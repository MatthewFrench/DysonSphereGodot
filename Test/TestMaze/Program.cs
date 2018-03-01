using Godot;
using System;
using Test;
using System.Collections.Generic;
using Test.MeshUtilities;
using Test.MazeCreation;

namespace TestMaze
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var maze = new Maze(new List<Vector2>() { new Vector2(-10, -10), new Vector2(10, -10), new Vector2(10, 10), new Vector2(-10, 10) });
        }
    }
}
