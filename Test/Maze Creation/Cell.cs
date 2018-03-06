using System;
using Godot;
using System.Collections.Generic;
namespace Test.MazeCreation
{
    
    public class Cell
    {
        Maze maze = null;
        List<Vector2> polygonShape = new List<Vector2>();

        //Public variables
        public Cell LeftCell = null, RightCell = null, TopCell = null, BottomCell = null;
        public Wall LeftWall = null, RightWall = null, TopWall = null, BottomWall = null;
        public int X = 0, Y = 0;
        public bool IsInPath = false;
        public int DistanceFromEnd = -1;
        public bool HasPathToEnd = false;
        //Used for distance, creates chains to track for pathing
        /*
        public bool LeftFlowsDeeper = null;
        public bool PastRightCell = null;
        public bool PastTopCell = null;
        public bool PastBottomCell = null;
        public bool NextLeftCell = null;
        public bool NextRightCell = null;
        public bool NextTopCell = null;
        public bool NextBottomCell = null;
        */
        public Cell(Maze maze, int x, int y)
        {
            this.maze = maze;
            this.X = x;
            this.Y = y;
            this.polygonShape.Add(new Vector2(this.X - 0.5f, this.Y - 0.5f));
            this.polygonShape.Add(new Vector2(this.X - 0.5f, this.Y + 0.5f));
            this.polygonShape.Add(new Vector2(this.X + 0.5f, this.Y + 0.5f));
            this.polygonShape.Add(new Vector2(this.X + 0.5f, this.Y - 0.5f));
        }
        public List<Vector2> getPolygon() {
            return polygonShape;
        }
        public void SetLeftCell(Cell cell) {
            LeftCell = cell;
            if (LeftCell != null && LeftCell.RightCell != this) {
                LeftCell.SetRightCell(this);
            }
        }
        public void SetRightCell(Cell cell)
        {
            RightCell = cell;
            if (RightCell != null && RightCell.LeftCell != this)
            {
                RightCell.SetLeftCell(this);
            }
        }
        public void SetTopCell(Cell cell)
        {
            TopCell = cell;
            if (TopCell != null && TopCell.BottomCell != this)
            {
                TopCell.SetBottomCell(this);
            }
        }
        public void SetBottomCell(Cell cell)
        {
            BottomCell = cell;
            if (BottomCell != null && BottomCell.TopCell != this)
            {
                BottomCell.SetTopCell(this);
            }
        }
        public void SetWallsFromNeighbors() {
            //Set left wall
            if (LeftCell == null) {
                LeftWall = new Wall(WallDirection.Vertical, null, this, this.X - 0.5f, this.Y + 0.5f, this.X - 0.5f, this.Y - 0.5f);
            } else {
                LeftWall = LeftCell.RightWall;
            }
            //Set right wall
            if (RightCell == null)
            {
                RightWall = new Wall(WallDirection.Vertical, this, null, this.X + 0.5f, this.Y + 0.5f, this.X + 0.5f, this.Y - 0.5f);
            }
            else
            {
                RightWall = RightCell.LeftWall;
            }
            //Set top wall
            if (TopCell == null)
            {
                TopWall = new Wall(WallDirection.Horizontal, null, this, this.X - 0.5f, this.Y + 0.5f, this.X + 0.5f, this.Y + 0.5f);
            }
            else
            {
                TopWall = TopCell.BottomWall;
            }
            //Set bottom wall
            if (BottomCell == null)
            {
                BottomWall = new Wall(WallDirection.Horizontal, null, this, this.X - 0.5f, this.Y - 0.5f, this.X + 0.5f, this.Y - 0.5f);
            }
            else
            {
                BottomWall = BottomCell.TopWall;
            }
        }
    }
}
