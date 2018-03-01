using System;
using Godot;
using System.Collections.Generic;
namespace Test.MazeCreation
{
    public class Cell
    {
        Maze maze = null;
        Cell leftCell = null, rightCell = null, topCell = null, bottomCell = null;
        Wall leftWall = null, rightWall = null, topWall = null, bottomWall = null;
        int x = 0, y = 0;
        List<Vector2> polygonShape = new List<Vector2>();
        public Cell(Maze maze, int x, int y)
        {
            this.maze = maze;
            this.x = x;
            this.y = y;
            this.polygonShape.Add(new Vector2(this.x - 0.5f, this.y - 0.5f));
            this.polygonShape.Add(new Vector2(this.x - 0.5f, this.y + 0.5f));
            this.polygonShape.Add(new Vector2(this.x + 0.5f, this.y + 0.5f));
            this.polygonShape.Add(new Vector2(this.x + 0.5f, this.y - 0.5f));
        }
        public List<Vector2> getPolygon() {
            return polygonShape;
        }
        public void SetLeftCell(Cell cell) {
            leftCell = cell;
            if (leftCell != null && leftCell.GetRightCell() != this) {
                leftCell.SetRightCell(this);
            }
        }
        public void SetRightCell(Cell cell)
        {
            rightCell = cell;
            if (rightCell != null && rightCell.GetLeftCell() != this)
            {
                rightCell.SetLeftCell(this);
            }
        }
        public void SetTopCell(Cell cell)
        {
            topCell = cell;
            if (topCell != null && topCell.GetBottomCell() != this)
            {
                topCell.SetBottomCell(this);
            }
        }
        public void SetBottomCell(Cell cell)
        {
            bottomCell = cell;
            if (bottomCell != null && bottomCell.GetTopCell() != this)
            {
                bottomCell.SetTopCell(this);
            }
        }
        public void SetWallsFromNeighbors() {
            //Set left wall
            if (leftCell == null) {
                leftWall = new Wall(WallDirection.Vertical, null, this, this.x - 0.5f, this.y + 0.5f, this.x - 0.5f, this.y - 0.5f);
            } else {
                leftWall = leftCell.GetRightWall();
            }
            //Set right wall
            if (rightCell == null)
            {
                rightWall = new Wall(WallDirection.Vertical, this, null, this.x + 0.5f, this.y + 0.5f, this.x + 0.5f, this.y - 0.5f);
            }
            else
            {
                rightWall = rightCell.GetLeftWall();
            }
            //Set top wall
            if (topCell == null)
            {
                topWall = new Wall(WallDirection.Horizontal, null, this, this.x - 0.5f, this.y + 0.5f, this.x + 0.5f, this.y + 0.5f);
            }
            else
            {
                topWall = topCell.GetBottomWall();
            }
            //Set bottom wall
            if (bottomCell == null)
            {
                bottomWall = new Wall(WallDirection.Horizontal, null, this, this.x - 0.5f, this.y - 0.5f, this.x + 0.5f, this.y - 0.5f);
            }
            else
            {
                bottomWall = bottomCell.GetTopWall();
            }
        }
        public Wall GetLeftWall() {
            return leftWall;
        }
        public Wall GetRightWall()
        {
            return rightWall;
        }
        public Wall GetTopWall()
        {
            return topWall;
        }
        public Wall GetBottomWall()
        {
            return bottomWall;
        }
        public Cell GetLeftCell() {
            return leftCell;
        }
        public Cell GetRightCell() {
            return rightCell;
        }
        public Cell GetTopCell() {
            return topCell;
        }
        public Cell GetBottomCell() {
            return bottomCell;
        }
        public int GetX() {
            return x;
        }
        public int GetY() {
            return y;
        }
    }
}
