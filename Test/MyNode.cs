using Godot;
using System;
using Test;
using System.Collections.Generic;

public class MyNode : Node
{
    // Member variables here, example:
    // private int a = 2;
    // private string b = "textvar";

    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        GD.Print("Hello from the Node CS Script Init");

        float radius = 6;
        float scale = radius;
        int subdivisionCount = 3;

        Hexasphere h = new Hexasphere((decimal)radius, subdivisionCount, 0.95);

        GD.Print("Number of Tiles: " + h.GetTiles().Count);
        var meshInstance = (PackedScene)ResourceLoader.Load("res://MeshInstance.tscn");
        var sphereMeshScene = (PackedScene)ResourceLoader.Load("res://SphereMesh.tscn");
        var groundTestScene = (PackedScene)ResourceLoader.Load("res://Ground Test.tscn");
        foreach (var tile in h.GetTiles()) {
            //CreateInstanceForTile(tile, meshInstance, scale, radius);
            CreateMesh(tile, sphereMeshScene, h.GetTiles().Count, radius, groundTestScene);
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

    public void CreateMesh(Tile tile, PackedScene sphereMeshScene, int numberOfTiles, float radius, PackedScene groundTestScene) {
        var surfTool = new SurfaceTool();
        var mesh = new ArrayMesh();
        var material = new SpatialMaterial();
        material.SetEmission(new Color(1.0f, 0.0f, 0.0f, 0.5f));
        surfTool.SetMaterial(material);
        surfTool.Begin(Mesh.PrimitiveType.TriangleFan);
        decimal tileCenterX = 0;
        decimal tileCenterY = 0;
        decimal tileCenterZ = 0;

        List<Point> points = tile.boundary;
        foreach (Point point in points) {
            surfTool.AddUv(new Vector2(0, 0));
            surfTool.AddVertex(new Vector3((float)point.x, (float)point.y, (float)point.z));
            tileCenterX += point.x / points.Count;
            tileCenterY += point.y / points.Count;
            tileCenterZ += point.z / points.Count;
        }

        var tileCenterPoint = new Vector3((float)tileCenterX, (float)tileCenterY, (float)tileCenterZ);
        var firstPoint = new Vector3((float)points[0].x, (float)points[0].y, (float)points[0].z);

        var polygonRadius = tileCenterPoint.DistanceTo(firstPoint);

        var sphereScale = radius / numberOfTiles;


        MeshInstance sphere = (MeshInstance)sphereMeshScene.Instance();
        sphere.SetTranslation(tileCenterPoint);
        sphere.SetScale(new Vector3(sphereScale, sphereScale, sphereScale));
        this.AddChild(sphere);

        
        var sphereCenterPoint = new Vector3(0f, 0f, 0f);

        Spatial groundTest = (Spatial)groundTestScene.Instance();
        groundTest.LookAtFromPosition(tileCenterPoint, sphereCenterPoint, new Vector3(0f, -1f, 0f));
		//Add ground to world
        this.AddChild(groundTest);


        //Scale ground to fit polygon size
        //groundTest.SetScale(new Vector3(polygonRadius * 2, 1.0f, polygonRadius * 2));
        //Move it to the center of the tile
        //groundTest.SetTranslation(tileCenterPoint);
        //Rotate it to face the center (0,0,0)
        //groundTest.LookAt(sphereCenterPoint, new Vector3(0f, 1f, 0f));

        /*
        groundTest.SetTransform(
            groundTest.GetTransform()
                //.Scaled(new Vector3(polygonRadius * 2, 1.0f, polygonRadius * 2))
                .Rotated(tileCenterPoint.Normalized(), tileCenterPoint.AngleTo(sphereCenterPoint))
            //.Translated(tileCenterPoint)
        );
        */
        //groundTest.SetScale(new Vector3(polygonRadius * 2, 1.0f, polygonRadius * 2));
        //groundTest.RotateZ(50);
        //groundTest.Translate(tileCenterPoint);
        //groundTest.S(new Vector3(polygonRadius * 2, 1.0f, polygonRadius * 2));
        //groundTest.RotateX((float)Math.PI);

        //groundTest.LookAt(tileCenterPoint, new Vector3(0f, 1f, 0f));
        //groundTest.Rotate(tileCenterPoint.Normalized(), tileCenterPoint.AngleTo(sphereCenterPoint));
        //groundTest.LookAtFromPosition(tileCenterPoint, sphereCenterPoint, new Vector3(0f, 1f, 0f));
        //groundTest.Translate(tileCenterPoint);
        //groundTest.SetTranslation(tileCenterPoint);

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
