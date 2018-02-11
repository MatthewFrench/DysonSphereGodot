using Godot;
using System;
using Test;
using System.Collections.Generic;

public class MyNode : Node
{
    List<Spatial> hexInstances = new List<Spatial>();
    public override void _Ready()
    {
        // Called every time the node is added to the scene.
        // Initialization here
        GD.Print("Hello from the Node CS Script Init");

        float radius = 1000;
        float scale = radius;
        int subdivisionCount = 3;

        Hexasphere h = new Hexasphere((decimal)radius, subdivisionCount, 1);
        Hexasphere h2 = new Hexasphere((decimal)(radius*0.99), subdivisionCount, 1);

        GD.Print("Number of Tiles: " + h.GetTiles().Count);
        var meshInstance = (PackedScene)ResourceLoader.Load("res://MeshInstance.tscn");
        var sphereMeshScene = (PackedScene)ResourceLoader.Load("res://SphereMesh.tscn");
        var hexagonTestScene = (PackedScene)ResourceLoader.Load("res://Hexagon Test.tscn");
        var pentagonTestScene = (PackedScene)ResourceLoader.Load("res://Pentagon Test.tscn");
        foreach (var tile in h.GetTiles()) {
            CreateInstance(tile, hexagonTestScene, pentagonTestScene);
        }

        foreach (var tile in h2.GetTiles())
        {
            CreateMesh(tile, sphereMeshScene, h2.GetTiles().Count, radius*0.99f);
        }
    }

    public void CreateInstance(Tile tile, PackedScene hexagonTestScene, PackedScene pentagonTestScene) {
        decimal tileCenterX = 0;
        decimal tileCenterY = 0;
        decimal tileCenterZ = 0;
        decimal polygonRadius = 0;

        List<Point> points = tile.boundary;
        var lastPoint = points[points.Count - 1];
        Vector3 lastPointVector = new Vector3((float)lastPoint.x, (float)lastPoint.y, (float)lastPoint.z);
        decimal polygonSideLength = 0;
        foreach (Point point in points)
        {
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
            var vector = new Vector3((float)point.x, (float)point.y, (float)point.z);
            polygonRadius += (decimal)vector.DistanceTo(tileCenterPoint);
        }
        polygonRadius = polygonRadius / points.Count;

        var polygonRotation = firstPoint.AngleTo(tileCenterPoint);
        GD.Print("First Point Radians: " + polygonRotation);
        GD.Print("First Point Degrees: " + Math.Round(polygonRotation * 180 / Math.PI));


        var sphereCenterPoint = new Vector3(0f, 0f, 0f);

        Spatial groundTest = null;
        if (points.Count == 5)
        {
            GD.Print("Pentagon Radius: " + polygonRadius + ", side length: " + polygonSideLength);

            //Create instance
            groundTest = (Spatial)pentagonTestScene.Instance();
        }
        else if (points.Count == 6)
        {
            GD.Print("Hexagon Radius: " + polygonRadius + ", side length: " + polygonSideLength);

            //Create instance
            groundTest = (Spatial)hexagonTestScene.Instance();
        }
        else
        {
            GD.Print("Unknown Radius: " + polygonRadius + ", Count: " + points.Count + ", Side Length: " + polygonSideLength);
        }
        if (groundTest != null)
        {
            //Put the instance on the tile and make it face the center of the sphere
            //This should be all that is needed but the sections are rotated perpendicular
            groundTest.LookAtFromPosition(tileCenterPoint, sphereCenterPoint, new Vector3(0f, 1f, 0f));
            //Get the local axis of the instance
            var axis = groundTest.GetTransform().basis.Xform(new Vector3(-1, 0, 0)).Normalized();
            //Rotate it to face the center (I have no idea why this is necessary)
            groundTest.Rotate(axis, (float)(Math.PI / 2.0));
            //Get the local axis of the instance
            var axis2 = groundTest.GetTransform().basis.Xform(new Vector3(0, 1, 0)).Normalized();
            //Rotate it to match up with other parts of the sphere
            //groundTest.Rotate(axis2, (float)(30.0 / 180.0 * Math.PI + polygonRotation));
            groundTest.Rotate(axis2, -polygonRotation);
            //Add ground to world
            this.AddChild(groundTest);
            this.hexInstances.Add(groundTest);
        }
    }

    public void CreateMesh(Tile tile, PackedScene sphereMeshScene, int numberOfTiles, float radius) {
        var surfTool = new SurfaceTool();
        var mesh = new ArrayMesh();
        var material = new SpatialMaterial();
        material.SetEmission(new Color(1.0f, 0.0f, 0.0f, 0.5f));
        surfTool.SetMaterial(material);
        surfTool.Begin(Mesh.PrimitiveType.LineLoop);
        decimal tileCenterX = 0;
        decimal tileCenterY = 0;
        decimal tileCenterZ = 0;
        decimal polygonRadius = 0;

        List<Point> points = tile.boundary;
        var lastPoint = points[points.Count - 1];
        Vector3 lastPointVector = new Vector3((float)lastPoint.x, (float)lastPoint.y, (float)lastPoint.z);
        decimal polygonSideLength = 0;
        foreach (Point point in points) {
            surfTool.AddUv(new Vector2(0, 0));
            surfTool.AddVertex(new Vector3((float)point.x, (float)point.y, (float)point.z));

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

        var polygonRotation = firstPoint.AngleTo(tileCenterPoint);


        var sphereCenterPoint = new Vector3(0f, 0f, 0f);

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

    public override void _Process(float delta)
    {
        // Called every frame. Delta is time since last frame.
        // Update game logic here.
        /*
        foreach (Spatial node in hexInstances) {
            //Get the local axis of the instance
            var axis = node.GetTransform().basis.Xform(new Vector3(0, 1, 0)).Normalized();
            //Rotate it to match up with other parts of the sphere
            node.Rotate(axis, (float)(1.0 / 180 * Math.PI));
        }
        */
    }
}
