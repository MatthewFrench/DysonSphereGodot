using Godot;
using System;
using Test;
using System.Collections.Generic;

namespace Test.MeshUtilities
{
    public class MeshCreation
    {
        //Creates a surface tool for mesh creation
        public static SurfaceTool CreateSurfaceTool(SpatialMaterial material = null) {
            var surfTool = new SurfaceTool();
            surfTool.Begin(Mesh.PrimitiveType.Triangles);
            if (material == null) {
                material = new SpatialMaterial();
                //material.SetEmission(new Color(1.0f, 0.0f, 0.0f));
                //material.SetEmissionEnergy(0.5f);
                material.SetAlbedo(new Color(0.5f, 0.0f, 0.0f));
                //material.SetMetallic(0.5f);
                material.SetCullMode(SpatialMaterial.CullMode.Back);
            }
            surfTool.SetMaterial(material);
            return surfTool;
        }

        //Creates a mesh from a surface tool
        public static ArrayMesh CreateMeshFromSurfaceTool(SurfaceTool surfTool)
        {
            var mesh = new ArrayMesh();
            surfTool.GenerateNormals();
            surfTool.Index();
            surfTool.Commit(mesh);
            return mesh;
        }

        //Creates a mesh instance from a mesh.
        public static MeshInstance CreateMeshInstanceFromMesh(ArrayMesh mesh) {
            var meshInstance = new MeshInstance();
            meshInstance.SetMesh(mesh);
            //meshInstance.SetCastShadowsSetting(GeometryInstance.ShadowCastingSetting.On);

            meshInstance.CreateTrimeshCollision();

            return meshInstance;
        }

        //Adds a wall to the surface tool.
        public static void AddWall(SurfaceTool surfTool, Vector3 point1, Vector3 point2, float width, float height)
        {
            //Creates 8 points and 6 quads from the given 2 points, width and height. Y axis is up.
            var rotationBetweenPoints = Math.Atan2(point2.z - point1.z, point2.x - point1.x);
            var leftRotation = rotationBetweenPoints - Math.PI / 2;
            var rightRotation = rotationBetweenPoints + Math.PI / 2;
            //Make the left point relative to either point
            var leftPoint = new Vector2(width / 2, 0).Rotated((float)leftRotation);
            //Make the right point relative to either point
            var rightPoint = new Vector2(width / 2, 0).Rotated((float)rightRotation);

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
            AddQuad(surfTool,
                    wallPoint1LeftBottom,
                    wallPoint1LeftTop,
                    wallPoint1RightTop,
                    wallPoint1RightBottom, true);

            //Add bottom wall
            AddQuad(surfTool,
                    wallPoint1LeftBottom,
                    wallPoint2LeftBottom,
                    wallPoint2RightBottom,
                    wallPoint1RightBottom);

            //Add top wall
            AddQuad(surfTool,
                    wallPoint1LeftTop,
                    wallPoint2LeftTop,
                    wallPoint2RightTop,
                    wallPoint1RightTop, true);

            //Add left wall
            AddQuad(surfTool,
                    wallPoint1LeftBottom,
                    wallPoint1LeftTop,
                    wallPoint2LeftTop,
                    wallPoint2LeftBottom, false);

            //Add right wall
            AddQuad(surfTool,
                    wallPoint1RightBottom,
                    wallPoint1RightTop,
                    wallPoint2RightTop,
                    wallPoint2RightBottom, true);

            //Add point 2 back wall
            AddQuad(surfTool,
                    wallPoint2LeftBottom,
                    wallPoint2LeftTop,
                    wallPoint2RightTop,
                    wallPoint2RightBottom);
        }

        //Adds a quad to the surface tool.
        public static void AddQuad(SurfaceTool surfTool, Vector3 point1, Vector3 point2, Vector3 point3, Vector3 point4, bool reverse = false)
        {
            AddTriangle(surfTool, point4, point1, point2, reverse);
            AddTriangle(surfTool, point2, point3, point4, reverse);
        }

        //Adds a triangle to a surface tool.
        public static void AddTriangle(SurfaceTool surfTool, Vector3 point1, Vector3 point2, Vector3 point3, bool reverse = false)
        {
            if (reverse) {
                surfTool.AddUv(new Vector2(0, 0));
                surfTool.AddVertex(point1);

                surfTool.AddUv(new Vector2(0, 0));
                surfTool.AddVertex(point2);

                surfTool.AddUv(new Vector2(0, 0));
                surfTool.AddVertex(point3);
            } else {
                surfTool.AddUv(new Vector2(0, 0));
                surfTool.AddVertex(point3);

                surfTool.AddUv(new Vector2(0, 0));
                surfTool.AddVertex(point2);

                surfTool.AddUv(new Vector2(0, 0));
                surfTool.AddVertex(point1);
            }
        }
    }
}