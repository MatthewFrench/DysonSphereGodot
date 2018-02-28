using Godot;
using System;
using Test;
using System.Collections.Generic;
using Test.MeshUtilities;

public class MazePuzzleNode : Node
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    Maze maze;

    public override void _Ready()
    {
        var rows = 16;
        var columns = 16;
        maze = new Maze(rows, columns, 1, 1);
        maze.Generate();

        //Draw cells to single mesh and apply to screen
        var surfTool = MeshCreation.CreateSurfaceTool();
        var cells = maze.GetCells();
        if (cells.Count > 0)
        {
            for (var r = 0; r <= rows - 1; r++)
            {
                for (var c = 0; c <= columns - 1; c++)
                {
                    Cell cell = cells["c" + c + "r" + r];
                    CreateCellWalls(surfTool, cell, r, c);
                }
            }
        }
        this.AddChild(MeshCreation.CreateMeshInstanceFromMesh(MeshCreation.CreateMeshFromSurfaceTool(surfTool)));
    }

    public void CreateCellWalls(SurfaceTool surfTool, Cell cell, int row, int column)
    {
        var size = 1;
        if (cell.NorthWall) {
            MeshCreation.AddWall(surfTool, new Vector3(row, 0.6f, column + size), new Vector3(row + size, 0.6f, column + size), 0.1f, 1.0f);
        }
        if (cell.SouthWall)
        {
            MeshCreation.AddWall(surfTool, new Vector3(row, 0.6f, column), new Vector3(row + size, 0.6f, column), 0.1f, 1.0f);
        }
        if (cell.EastWall)
        {
            MeshCreation.AddWall(surfTool, new Vector3(row + size, 0.6f, column), new Vector3(row + size, 0.6f, column + size), 0.1f, 1.0f);
        }
        if (cell.WestWall)
        {
            MeshCreation.AddWall(surfTool, new Vector3(row, 0.6f, column), new Vector3(row, 0.6f, column + size), 0.1f, 1.0f);
        }
    }
}