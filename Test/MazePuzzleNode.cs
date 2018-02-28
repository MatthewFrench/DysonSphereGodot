using Godot;
using System;
using Test;
using System.Collections.Generic;

public class MazePuzzleNode : Node
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";
    Maze maze;

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        CreateAndAddWallMeshToScene(new Vector3(0.0f, 3.0f, 0.0f), 
                                    new Vector3(0.0f, 3.0f, 1.0f), 
                                    1.0f, 1.0f);
        
        var rows = 16;
        var columns = 16;
        maze = new Maze(rows, columns, 1, 1);
        maze.Generate();
        var cells = maze.GetCells();
        if (cells.Count > 0)
        {
            for (var r = 0; r <= rows - 1; r++)
            {
                for (var c = 0; c <= columns - 1; c++)
                {
                    Cell cell = cells["c" + c + "r" + r];
                    CreateCellWalls(cell, r, c);
                }
            }
        }
    }


    public void CreateCellWalls(Cell cell, int row, int column)
    {
        var size = 1;
        if (cell.NorthWall) {
            CreateAndAddWallMeshToScene(new Vector3(row, 0.6f, column + size), new Vector3(row + size, 0.6f, column + size), 0.1f, 1.0f);
        }
        if (cell.SouthWall)
        {
            CreateAndAddWallMeshToScene(new Vector3(row, 0.6f, column), new Vector3(row + size, 0.6f, column), 0.1f, 1.0f);
        }
        if (cell.EastWall)
        {
            CreateAndAddWallMeshToScene(new Vector3(row + size, 0.6f, column), new Vector3(row + size, 0.6f, column + size), 0.1f, 1.0f);
        }
        if (cell.WestWall)
        {
            CreateAndAddWallMeshToScene(new Vector3(row, 0.6f, column), new Vector3(row, 0.6f, column + size), 0.1f, 1.0f);
        }
    }

    //Makes a wall from a line given a width and height. Each point is in the center of that respective end.
    public void CreateAndAddWallMeshToScene(Vector3 point1, Vector3 point2, float width, float height) {
        //Setup
        var surfTool = new SurfaceTool();
        var mesh = new ArrayMesh();
        var material = new SpatialMaterial();
        material.SetEmission(new Color(1.0f, 0.0f, 0.0f));
        material.SetEmissionEnergy(0.5f);
        material.SetAlbedo(new Color(0.5f, 0.0f, 0.0f));
        material.SetMetallic(0.5f);
        material.SetCullMode(SpatialMaterial.CullMode.Disabled);
        surfTool.Begin(Mesh.PrimitiveType.Triangles);
        surfTool.SetMaterial(material);

        //Add geometry
        AddWall(surfTool, material, point1, point2, width, height);

        //Clean up
        surfTool.GenerateNormals();
        surfTool.Index();
        surfTool.Commit(mesh);
        var meshInstance = new MeshInstance();
        meshInstance.SetMesh(mesh);
        meshInstance.SetCastShadowsSetting(GeometryInstance.ShadowCastingSetting.DoubleSided);
        this.AddChild(meshInstance);
    }

    //Adds a wall to the surface tool.
    public void AddWall(SurfaceTool surfTool, SpatialMaterial material, Vector3 point1, Vector3 point2, float width, float height)
    {
        //Creates 8 points and 6 quads from the given 2 points, width and height. Y axis is up.
        var rotationBetweenPoints = Math.Atan2(point2.z - point1.z, point2.x - point1.x);
        var leftRotation = rotationBetweenPoints - Math.PI / 2;
        var rightRotation = rotationBetweenPoints + Math.PI / 2;
        //Make the left point relative to either point
        var leftPoint = new Vector2(width/2, 0).Rotated((float)leftRotation);
        //Make the right point relative to either point
        var rightPoint = new Vector2(width/2, 0).Rotated((float)rightRotation);

        var point1Left = new Vector2(leftPoint.x + point1.x, leftPoint.y + point1.z);
        var point1Right = new Vector2(rightPoint.x + point1.x, rightPoint.y + point1.z);
        var point2Left = new Vector2(leftPoint.x + point2.x, leftPoint.y + point2.z);
        var point2Right = new Vector2(rightPoint.x + point2.x, rightPoint.y + point2.z);
        var point1Top = point1.y + height;
        var point2Top = point2.y + height;

        //Point 1 side
        //  Left
        //      Bottom
        var wallPoint1LeftBottom = new Vector3(point1Left.x, point1.y, point1Left.y);
        //      Top
        var wallPoint1LeftTop = new Vector3(point1Left.x, point1Top, point1Left.y);
        //  Right
        //      Top
        var wallPoint1RightTop = new Vector3(point1Right.x, point1Top, point1Right.y);
        //      Bottom
        var wallPoint1RightBottom = new Vector3(point1Right.x, point1.y, point1Right.y);
        //Point 2 side
        //  Left
        //      Bottom
        var wallPoint2LeftBottom = new Vector3(point2Left.x, point2.y, point2Left.y);
        //      Top
        var wallPoint2LeftTop = new Vector3(point2Left.x, point2Top, point2Left.y);
        //  Right
        //      Top
        var wallPoint2RightTop = new Vector3(point2Right.x, point2Top, point2Right.y);
        //      Bottom
        var wallPoint2RightBottom = new Vector3(point2Right.x, point2.y, point2Right.y);

        //Add point 1 back wall
        AddQuad(surfTool, material,
                wallPoint1LeftBottom,
                wallPoint1LeftTop,
                wallPoint1RightTop,
                wallPoint1RightBottom);

        //Add bottom wall
        AddQuad(surfTool, material,
                wallPoint1LeftBottom,
                wallPoint2LeftBottom,
                wallPoint2RightBottom,
                wallPoint1RightBottom);

        //Add top wall
        AddQuad(surfTool, material,
                wallPoint1LeftTop,
                wallPoint2LeftTop,
                wallPoint2RightTop,
                wallPoint1RightTop);

        //Add left wall
        AddQuad(surfTool, material,
                wallPoint1LeftBottom,
                wallPoint1LeftTop,
                wallPoint2LeftTop,
                wallPoint2LeftBottom);

        //Add right wall
        AddQuad(surfTool, material,
                wallPoint1RightBottom,
                wallPoint1RightTop,
                wallPoint2RightTop,
                wallPoint2RightBottom);

        //Add point 2 back wall
        AddQuad(surfTool, material,
                wallPoint2LeftBottom,
                wallPoint2LeftTop,
                wallPoint2RightTop,
                wallPoint2RightBottom);
    }

    //Adds a quad to the surface tool.
    public void AddQuad(SurfaceTool surfTool, SpatialMaterial material, Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4) {
        AddTriangle(surfTool, material, point4, point1, point2);
        AddTriangle(surfTool, material, point2, point3, point4);
    }

    //Adds a triangle to a surface tool.
    public void AddTriangle(SurfaceTool surfTool, SpatialMaterial material, Vector3 point1, Vector3 point2, Vector3 point3) {
        //surfTool.SetMaterial(material);
        surfTool.AddUv(new Vector2(0, 0));
        surfTool.AddVertex(point1);

        //surfTool.SetMaterial(material);
        surfTool.AddUv(new Vector2(0, 0));
        surfTool.AddVertex(point2);

        //surfTool.SetMaterial(material);
        surfTool.AddUv(new Vector2(0, 0));
        surfTool.AddVertex(point3);
    }
}
