using Godot;
using System;
using Test;
using System.Collections.Generic;

public class MyNode : Node
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    private bool Show_Hex_Mesh = false;

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        GD.Print("Hello from the Node CS Script Init");

        float radius = 1000;
        float scale = radius;
        int subdivisionCount = 3;

        Hexasphere h = new Hexasphere((decimal)radius, subdivisionCount, 1);

        GD.Print("Number of Tiles: " + h.GetTiles().Count);
        var meshInstance = (PackedScene)ResourceLoader.Load("res://MeshInstance.tscn");
        var sphereMeshScene = (PackedScene)ResourceLoader.Load("res://SphereMesh.tscn");
        var hexagonTestScene = (PackedScene)ResourceLoader.Load("res://Hexagon Test.tscn");
        var pentagonTestScene = (PackedScene)ResourceLoader.Load("res://Pentagon Test.tscn");
        foreach (var tile in h.GetTiles()) {
            //CreateInstanceForTile(tile, meshInstance, scale, radius);
            CreateMesh(tile, sphereMeshScene, h.GetTiles().Count, radius, hexagonTestScene, pentagonTestScene);
        }
		
		/*
		for i in range(0, h.GetTiles().Count):
			var tile = h.GetTiles()[i];
			var centerPoint = tile.centerPoint;
			GD.Print("Tile " + i);
			GD.Print("X: " + centerPoint.x);
			*/
    }

    public void CreateInstanceForTile(Tile tile, PackedScene meshInstance, float scale, float radius) {
        var centerPoint = tile.centerPoint;
        var x = (float)centerPoint.x;
        var y = (float)centerPoint.y;
        var z = (float)centerPoint.z;
        MeshInstance mesh = (MeshInstance)meshInstance.Instance();
        mesh.SetTransform(mesh.Transform.Scaled(new Vector3(scale, scale, scale)));
        mesh.SetTransform(mesh.Transform.Translated(new Vector3(x, y, z)));
        //mesh.SetTranslation(new Vector3(x, y, z));
        mesh.SetTransform(mesh.Transform.LookingAt(new Vector3(radius, radius, radius), new Vector3(0, 1, 0)));
        this.AddChild(mesh);
    }

    public void CreateMesh(Tile tile, PackedScene sphereMeshScene, int numberOfTiles, float radius, PackedScene hexagonTestScene, PackedScene pentagonTestScene) {
        var surfTool = new SurfaceTool();
        var mesh = new ArrayMesh();
        var material = new SpatialMaterial();
        material.SetEmission(new Color(1.0f, 0.0f, 0.0f, 0.5f));
        surfTool.SetMaterial(material);
        surfTool.Begin(Mesh.PrimitiveType.TriangleFan);
        decimal tileCenterX = 0;
        decimal tileCenterY = 0;
        decimal tileCenterZ = 0;
        decimal polygonRadius = 0;

        List<Point> points = tile.boundary;
        var lastPoint = points[points.Count - 1];
        Vector3 lastPointVector = new Vector3((float)lastPoint.x, (float)lastPoint.y, (float)lastPoint.z);
        decimal polygonSideLength = 0;
        foreach (Point point in points) {
            if (Show_Hex_Mesh) {
                surfTool.AddUv(new Vector2(0, 0));
                surfTool.AddVertex(new Vector3((float)point.x, (float)point.y, (float)point.z));
            }
            tileCenterX += point.x / points.Count;
            tileCenterY += point.y / points.Count;
            tileCenterZ += point.z / points.Count;
            Vector3 currentVector = new Vector3((float)point.x, (float)point.y, (float)point.z); 
            polygonSideLength += (decimal)currentVector.DistanceTo(lastPointVector);
            lastPointVector = currentVector;
        }
        polygonSideLength = polygonSideLength / points.Count;

        var tileCenterPoint = new Vector3((float)tileCenterX, (float)tileCenterY, (float)tileCenterZ);
        var firstPoint = new Vector3((float)points[0].x, (float)points[0].y, (float)points[0].z);

        foreach (Point point in points)
        {
            var vector = new Vector3((float) point.x, (float)point.y, (float)point.z);
            polygonRadius += (decimal)vector.DistanceTo(tileCenterPoint);
        }
        polygonRadius = polygonRadius / points.Count;


        var sphereCenterPoint = new Vector3(0f, 0f, 0f);

        if (points.Count == 5) {
            GD.Print("Pentagon Radius: " + polygonRadius + ", side length: " + polygonSideLength);

            //Create instance
            Spatial groundTest = (Spatial)pentagonTestScene.Instance();
            //Put the instance on the tile and make it face the center of the sphere
            //This should be all that is needed but the sections are rotated perpendicular
            groundTest.LookAtFromPosition(tileCenterPoint, sphereCenterPoint, new Vector3(0f, 1f, 0f));
            //Get the local axis of the instance
            var axis = groundTest.GetTransform().basis.Xform(new Vector3(-1, 0, 0)).Normalized();
            //Rotate it to face the center (I have no idea why this is necessary)
            groundTest.Rotate(axis, (float)(Math.PI / 2.0));
            //Scale the ground to match the size of the tile
            //groundTest.SetTransform(groundTest.GetTransform().Scaled(new Vector3(polygonRadius * 2, 1.0f, polygonRadius * 2)));
            //groundTest.SetScale(new Vector3(polygonRadius * 2, 1.0f, polygonRadius * 2));
            //Add ground to world
            this.AddChild(groundTest);
        } else if (points.Count == 6) {
            GD.Print("Hexagon Radius: " + polygonRadius + ", side length: " + polygonSideLength);

            //Create instance
            Spatial groundTest = (Spatial)hexagonTestScene.Instance();
            //Put the instance on the tile and make it face the center of the sphere
            //This should be all that is needed but the sections are rotated perpendicular
            groundTest.LookAtFromPosition(tileCenterPoint, sphereCenterPoint, new Vector3(0f, 1f, 0f));
            //Get the local axis of the instance
            var axis = groundTest.GetTransform().basis.Xform(new Vector3(-1, 0, 0)).Normalized();
            //Rotate it to face the center (I have no idea why this is necessary)
            groundTest.Rotate(axis, (float)(Math.PI / 2.0));
            //Scale the ground to match the size of the tile
            //groundTest.SetTransform(groundTest.GetTransform().Scaled(new Vector3(polygonRadius * 2, 1.0f, polygonRadius * 2)));
            //groundTest.SetScale(new Vector3(polygonRadius * 2, 1.0f, polygonRadius * 2));
            //Add ground to world
            this.AddChild(groundTest);
        } else {
            GD.Print("Unknown Radius: " + polygonRadius + ", Count: " + points.Count + ", Side Length: " + polygonSideLength);
        }

        var sphereScale = radius / numberOfTiles;
        MeshInstance sphere = (MeshInstance)sphereMeshScene.Instance();
        sphere.SetTranslation(tileCenterPoint);
        sphere.SetScale(new Vector3(sphereScale, sphereScale, sphereScale));
        this.AddChild(sphere);



        surfTool.GenerateNormals();
        surfTool.Index();
        surfTool.Commit(mesh);
        var meshInstance = new MeshInstance();
        meshInstance.SetMesh(mesh);
        this.AddChild(meshInstance);
    }

//    public override void _Process(float delta)
//    {
//        // Called every frame. Delta is time since last frame.
//        // Update game logic here.
//        
//    }
}
