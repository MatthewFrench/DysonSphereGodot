using System;
using System.Collections.Generic;
using Godot;
using Test.Utility;
namespace Test.MazeCreation
{
    public class Maze
    {
        Dictionary<Tuple<int, int>, Cell> cellMap = new Dictionary<Tuple<int, int>, Cell>();
        List<Cell> cells = new List<Cell>();
        List<Vector2> limitPolygon;
        Cell startCell = null, endCell = null;
        public Maze(List<Vector2> limitPolygon, Vector2 startPosition, Vector2 endPosition)
        {
            this.limitPolygon = limitPolygon;
            Grow();
            //Set the start and end cells
            startCell = GetClosestCellToPosition(startPosition);
            endCell = GetClosestCellToPosition(startPosition);
            CreatePathThroughMaze();
        }

        private void CreatePathThroughMaze() {
            
        }

        //Sets a seed at 0,0 and grows it until it hits boundaries
        private void Grow() {
            //Place a tile in the center and grow it until it hits the limit polygon.
            var centerCell = CreateNewCellAtIndex(0, 0);
            if (centerCell == null) {
                return;
            }
            var completedQueue = new HashSet<Cell>();
            var growQueue = new List<Cell>();
            growQueue.Add(centerCell);
            while (growQueue.Count > 0) {
                var cell = growQueue[0];
                growQueue.RemoveAt(0);
                completedQueue.Add(cell);
                var x = cell.GetX();
                var y = cell.GetY();
                if (cell.GetLeftCell() == null) {
                    var leftCell = CreateNewCellAtIndex(x - 1, y);
                    if (leftCell != null) {
                        growQueue.Add(leftCell);
                    }
                }
                if (cell.GetRightCell() == null)
                {
                    var rightCell = CreateNewCellAtIndex(x + 1, y);
                    if (rightCell != null)
                    {
                        growQueue.Add(rightCell);
                    }
                }
                if (cell.GetTopCell() == null)
                {
                    var topCell = CreateNewCellAtIndex(x, y + 1);
                    if (topCell != null)
                    {
                        growQueue.Add(topCell);
                    }
                }
                if (cell.GetBottomCell() == null)
                {
                    var bottomCell = CreateNewCellAtIndex(x, y - 1);
                    if (bottomCell != null)
                    {
                        growQueue.Add(bottomCell);
                    }
                }
            }
        }

        //Returns a cell if it exists or is created
        //Exists null if the cell cannot be created because of boundaries
        public Cell CreateNewCellAtIndex(int x, int y) {
            if (GetCellAtPosition(x, y) != null) {
                return GetCellAtPosition(x, y);
            }
            var cell = new Cell(this, x, y);
            if (Intersection.DoPolygonsIntersect(this.limitPolygon, cell.getPolygon())) {
                return null;
            }
            cell.SetLeftCell(GetCellAtPosition(x - 1, y));
            cell.SetRightCell(GetCellAtPosition(x + 1, y));
            cell.SetTopCell(GetCellAtPosition(x, y + 1));
            cell.SetBottomCell(GetCellAtPosition(x, y - 1));
            cell.SetWallsFromNeighbors();
            cells.Add(cell);
            cellMap.Add(new Tuple<int, int>(x, y), cell);
            return cell;
        }

        //Gets a cell at an exact position
        public Cell GetCellAtPosition(int x, int y) {
            var key = new Tuple<int, int>(x, y);
            if (cellMap.ContainsKey(key)) {
                return cellMap[key];
            }
            return null;
        }

        //Gets a cell at the closest position
        public Cell GetClosestCellToPosition(Vector2 position) {
            float distance = float.MaxValue;
            Cell chosenCell = null;
            foreach (var cell in cells) {
                var x = cell.GetX() - position.x;
                var y = cell.GetY() - position.y;
                float cellDistance = (float)Math.Sqrt(x * x + y * y);
                if (distance > cellDistance) {
                    distance = cellDistance;
                    chosenCell = cell;
                } 
            }
            return chosenCell;
        }

        //Returns the list of walls in the maze.
        public List<Wall> GetWalls() {
            var walls = new HashSet<Wall>();
            foreach (var cell in cells) {
                if (cell.GetLeftWall() != null) {
                    walls.Add(cell.GetLeftWall());
                }
                if (cell.GetRightWall() != null)
                {
                    walls.Add(cell.GetRightWall());
                }
                if (cell.GetTopWall() != null)
                {
                    walls.Add(cell.GetTopWall());
                }
                if (cell.GetBottomWall() != null)
                {
                    walls.Add(cell.GetBottomWall());
                }
            }
            return new List<Wall>(walls);
        }
    }
}
