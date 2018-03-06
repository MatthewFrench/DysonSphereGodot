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
        maze = new Maze(ShapeGeometry.MakePolygon(6, 21, (float)Math.PI/2), 
                        new Vector2(-21, -21), new Vector2(21, 21));
        //maze = new Maze(ShapeGeometry.MakeStar(12, 21, (float)Math.PI / 2), new Vector2(-21, -21), new Vector2(21, 21));

        //Draw cells to single mesh and apply to screen
        var surfTool = MeshCreation.CreateSurfaceTool();

        var walls = maze.GetWalls();
        foreach (var wall in walls) {
            if (!wall.KnockedDown) {
                MeshCreation.AddWall(surfTool, new Vector3(wall.GetPoint1X(), 0.0f, wall.GetPoint1Y()),
                                     new Vector3(wall.GetPoint2X(), 0.0f, wall.GetPoint2Y()), 0.1f, 2.0f);
            }
        }

        this.AddChild(MeshCreation.CreateMeshInstanceFromMesh(MeshCreation.CreateMeshFromSurfaceTool(surfTool)));

        //Put players at start
        KinematicBody player = (KinematicBody)this.GetParent().GetNode("player");
        var startingCell = maze.GetStartingCell();
        player.SetTranslation(new Vector3(startingCell.X, 0.0f, startingCell.Y));

        //Put flag at end
        Spatial flag = (Spatial)this.GetParent().GetNode("Flag");
        var endingCell = maze.GetEndingCell();
        flag.SetTranslation(new Vector3(endingCell.X, 0.0f, endingCell.Y));
    }
}