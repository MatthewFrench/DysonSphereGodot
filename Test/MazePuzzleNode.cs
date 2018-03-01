using Godot;
using System;
using Test;
using System.Collections.Generic;
using Test.MeshUtilities;
using Test.MazeCreation;
using Test.Utility;

public class MazePuzzleNode : Node
{
    Maze maze;

    public override void _Ready()
    {
        maze = new Maze(ShapeGeometry.MakePolygon(6, 21, (float)Math.PI/2), new Vector2(-21, -21), new Vector2(21, 21));

        //Draw cells to single mesh and apply to screen
        var surfTool = MeshCreation.CreateSurfaceTool();

        var walls = maze.GetWalls();
        foreach (var wall in walls) {
            if (!wall.isKnockedDown()) {
                MeshCreation.AddWall(surfTool, new Vector3(wall.GetPoint1X(), 0.6f, wall.GetPoint1Y()),
                                     new Vector3(wall.GetPoint2X(), 0.6f, wall.GetPoint2Y()), 0.1f, 1.0f);
            }
        }

        this.AddChild(MeshCreation.CreateMeshInstanceFromMesh(MeshCreation.CreateMeshFromSurfaceTool(surfTool)));
    }
}