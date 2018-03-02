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

        //Define sphere properties
        float radius = 151;
        float scale = radius;
        int subdivisionCount = 5;

        //h is used for creating full size tiles
        Hexasphere h = new Hexasphere((decimal)radius, subdivisionCount, 1);
        //h2 is for creating the line mesh
        Hexasphere h2 = new Hexasphere((decimal)(radius*0.99), subdivisionCount, 1);

        GD.Print("Number of Tiles: " + h.GetTiles().Count);
        //Load the instances to put in the scene
        var meshInstance = (PackedScene)ResourceLoader.Load("res://MeshInstance.tscn");
        var sphereMeshScene = (PackedScene)ResourceLoader.Load("res://SphereMesh.tscn");
        var greenSphereMeshScene = (PackedScene)ResourceLoader.Load("res://GreenSphereMesh.tscn");
        var redSphereMeshScene = (PackedScene)ResourceLoader.Load("res://RedSphereMesh.tscn");
        var hexagonTestScene = (PackedScene)ResourceLoader.Load("res://MattLevel/Hexagon.tscn");
        var pentagonTestScene = (PackedScene)ResourceLoader.Load("res://MattLevel/Main.tscn"); //Pentagon, Main
        var numberOfTiles = h.GetTiles().Count;
        //Create all the tiles
        foreach (var tile in h.GetTiles()) {
            CreateInstance(tile, hexagonTestScene, pentagonTestScene, redSphereMeshScene, radius / numberOfTiles);
        }
        //Create the line mesh
        foreach (var tile in h2.GetTiles())
        {
            CreateMesh(tile, sphereMeshScene, greenSphereMeshScene, h2.GetTiles().Count, radius*0.99f);
        }
    }

    public void CreateInstance(Tile tile, PackedScene hexagonTestScene, PackedScene pentagonTestScene, PackedScene redSphereMeshScene, float sphereScale) {
        decimal tileCenterX = 0;
        decimal tileCenterY = 0;
        decimal tileCenterZ = 0;
        decimal polygonRadius = 0;

        List<Point> points = tile.boundary;

        //Calculate the average center point and polygon side length.
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

        //Create the center point
        var tileCenterPoint = new Vector3((float)tileCenterX, (float)tileCenterY, (float)tileCenterZ);
        var firstPoint = new Vector3((float)points[0].x, (float)points[0].y, (float)points[0].z);

        //Get the average polygon radius from center to each point.
        foreach (Point point in points)
        {
            var vector = new Vector3((float)point.x, (float)point.y, (float)point.z);
            polygonRadius += (decimal)vector.DistanceTo(tileCenterPoint);
        }
        polygonRadius = polygonRadius / points.Count;

        var sphereCenterPoint = new Vector3(0f, 0f, 0f);

        //Create the tile instance
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
            //Add ground to world
            this.AddChild(groundTest);
            this.hexInstances.Add(groundTest);

            //Create and convert local point to world point
            var localOriginalFirstPoint = new Vector3(0, 0, (float)polygonRadius);
            var worldOriginalFirstPoint = groundTest.ToGlobal(localOriginalFirstPoint);

            var worldGeneratedFirstPoint = firstPoint;
            var localGeneratedFirstPoint = groundTest.ToLocal(worldGeneratedFirstPoint);

            MeshInstance sphere = (MeshInstance)redSphereMeshScene.Instance();
            sphere.SetTranslation(worldOriginalFirstPoint);
            sphere.SetScale(new Vector3(sphereScale, sphereScale, sphereScale));
            this.AddChild(sphere);
            //World Original First Point works and is correct
            //Now is there a way I can get the two points and match them up?
            var angleCalcOriginalPoint = worldOriginalFirstPoint - tileCenterPoint;
            var angleCalcFirstPoint = firstPoint - tileCenterPoint;

            var angle = angleCalcFirstPoint.AngleTo(angleCalcOriginalPoint);
            var angle2 = new Vector2(localGeneratedFirstPoint.x, localGeneratedFirstPoint.z).AngleTo(new Vector2(localOriginalFirstPoint.x, localOriginalFirstPoint.z));

            var axis3 = groundTest.GetTransform().basis.Xform(new Vector3(0, 1, 0));
            if (points.Count == 5) {
                angle2 += (float)Math.PI / 4;
            }
            //angleCalcFirstPoint.
            //Rotate it to match up with other parts of the sphere
            groundTest.Rotate(axis3, angle2);


            var newWorldPoint = groundTest.ToGlobal(localOriginalFirstPoint);
            GD.Print("Difference: " + (worldGeneratedFirstPoint - newWorldPoint));
        }
    }

    public void CreateMesh(Tile tile, PackedScene sphereMeshScene, PackedScene greenSphereMeshScene, int numberOfTiles, float radius) {
        var surfTool = new SurfaceTool();
        var mesh = new ArrayMesh();
        var material = new SpatialMaterial();
        material.SetEmission(new Color(1.0f, 0.0f, 0.0f));
        material.SetAlbedo(new Color(1.0f, 0.0f, 0.0f));
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

        MeshInstance sphere2 = (MeshInstance)greenSphereMeshScene.Instance();
        sphere2.SetTranslation(firstPoint);
        sphere2.SetScale(new Vector3(sphereScale, sphereScale, sphereScale));
        this.AddChild(sphere2);

        MeshInstance sphere3 = (MeshInstance)greenSphereMeshScene.Instance();
        sphere3.SetTranslation((firstPoint - tileCenterPoint) / 2 + tileCenterPoint);
        sphere3.SetScale(new Vector3(sphereScale, sphereScale, sphereScale));
        this.AddChild(sphere3);

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
