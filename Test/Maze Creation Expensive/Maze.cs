using System;
using System.Collections.Generic;
using Godot;
using Test.Utility;
namespace Test.MazeCreationExpensive
{
    public class Maze
    {
        Dictionary<Tuple<int, int>, Cell> cellMap = new Dictionary<Tuple<int, int>, Cell>();
        List<Cell> cells = new List<Cell>();
        List<Vector2> limitPolygon;
        Cell startCell = null, endCell = null;
        Random random = new Random();
        public Maze(List<Vector2> limitPolygon, Vector2 startPosition, Vector2 endPosition)
        {
            this.limitPolygon = limitPolygon;
            Grow();
            //Set the start and end cells
            startCell = GetClosestCellToPosition(startPosition);
            endCell = GetClosestCellToPosition(endPosition);
            CreatePathThroughMaze(startCell, endCell);
            CreateRandomMazePaths();
            //Open the end and start of the maze
            if (startCell.LeftCell == null) startCell.LeftWall.setKnockedDown(true);
            if (startCell.RightCell == null) startCell.RightWall.setKnockedDown(true);
            if (startCell.TopCell == null) startCell.TopWall.setKnockedDown(true);
            if (startCell.BottomCell == null) startCell.BottomWall.setKnockedDown(true);
            if (endCell.LeftCell == null) endCell.LeftWall.setKnockedDown(true);
            if (endCell.RightCell == null) endCell.RightWall.setKnockedDown(true);
            if (endCell.TopCell == null) endCell.TopWall.setKnockedDown(true);
            if (endCell.BottomCell == null) endCell.BottomWall.setKnockedDown(true);
        }

        private void CreateRandomMazePaths() {
            //Make a list of non-pathed cells
            List<Cell> nonPathedCells = new List<Cell>();
            List<Cell> pathedCells = new List<Cell>();
            foreach (Cell cell in cells) {
                if (!cell.IsInPath) {
                    nonPathedCells.Add(cell);
                } else {
                    pathedCells.Add(cell);
                }
            }
            //Loop through cells and loop each non-pathed to a pathed
            while (nonPathedCells.Count > 0) {
                var availableTargets = new List<Cell>(pathedCells);
                var cell = nonPathedCells[nonPathedCells.Count - 1];
                nonPathedCells.RemoveAt(nonPathedCells.Count - 1);
                pathedCells.Add(cell);
                var gotPath = false;
                while (!gotPath && availableTargets.Count > 0) {
                    var randomTarget = availableTargets[random.Next(availableTargets.Count)];
                    availableTargets.Remove(randomTarget);
                    gotPath = CreatePathThroughMaze(cell, randomTarget);
                }

                TransferPathedCellsToNonPathed(nonPathedCells, pathedCells);
            }
        }

        private void TransferPathedCellsToNonPathed(List<Cell> nonPathedCells, List<Cell> pathedCells) {
            for (var index = 0; index < nonPathedCells.Count; index++) {
                var cell = nonPathedCells[index];
                if (cell.IsInPath) {
                    pathedCells.Add(cell);
                    nonPathedCells.RemoveAt(index);
                    index--;
                }
            }
        }

        private bool CreatePathThroughMaze(Cell chosenStartCell, Cell chosenEndCell) {
            if (!DoesAvailablePathExistBetweenCells(chosenStartCell, chosenEndCell)) {
                return false;
            }
            startCell.IsInPath = true;
            //Start at the start cell, pick a random direction, test if it can be moved to without blocking the path
            //Move to that direction and repeat
            var currentStartCell = chosenStartCell;
            var targetEndCell = chosenEndCell;
            var directionsAvailable = new List<int>() { 1, 2, 3, 4 };
            var directionsTried = new List<int>();
            while (currentStartCell != targetEndCell) {
                if (directionsAvailable.Count == 0) {
                    //No paths to the end available, exit
                    return false;
                }
                //Choose a direction at random
                var randomDirection = directionsAvailable[random.Next(directionsAvailable.Count)];
                directionsTried.Add(randomDirection);
                directionsAvailable.Remove(randomDirection);
                var directionX = 0;
                var directionY = 0;
                if (randomDirection == 1) directionX = -1;
                if (randomDirection == 2) directionX = 1;
                if (randomDirection == 3) directionY = -1;
                if (randomDirection == 4) directionY = 1;
                var randomCell = GetCellAtPosition(currentStartCell.X + directionX, currentStartCell.Y + directionY);
                if (randomCell != null && DoesAvailablePathExistBetweenCells(randomCell, targetEndCell)) {
                    //Knock down wall between current start cell and random cell, mark it as a path
                    if (directionX == -1) currentStartCell.LeftWall.setKnockedDown(true);
                    if (directionX == 1) currentStartCell.RightWall.setKnockedDown(true);
                    if (directionY == -1) currentStartCell.BottomWall.setKnockedDown(true);
                    if (directionY == 1) currentStartCell.TopWall.setKnockedDown(true);
                    randomCell.IsInPath = true;
                    //Set the new cell as the start cell and reset the directions
                    currentStartCell = randomCell;
                    //TODO Replace directions list with boolean to increase speed
                    directionsAvailable = new List<int>() { 1, 2, 3, 4 };
                    directionsTried = new List<int>();
                }
            }
            return true;
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
                var x = cell.X;
                var y = cell.Y;
                if (cell.LeftCell == null) {
                    var leftCell = CreateNewCellAtIndex(x - 1, y);
                    if (leftCell != null) {
                        growQueue.Add(leftCell);
                    }
                }
                if (cell.RightCell == null)
                {
                    var rightCell = CreateNewCellAtIndex(x + 1, y);
                    if (rightCell != null)
                    {
                        growQueue.Add(rightCell);
                    }
                }
                if (cell.TopCell == null)
                {
                    var topCell = CreateNewCellAtIndex(x, y + 1);
                    if (topCell != null)
                    {
                        growQueue.Add(topCell);
                    }
                }
                if (cell.BottomCell == null)
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
                var x = cell.X - position.x;
                var y = cell.Y - position.y;
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
                if (cell.LeftWall != null) {
                    walls.Add(cell.LeftWall);
                }
                if (cell.RightWall != null)
                {
                    walls.Add(cell.RightWall);
                }
                if (cell.TopWall != null)
                {
                    walls.Add(cell.TopWall);
                }
                if (cell.BottomWall != null)
                {
                    walls.Add(cell.BottomWall);
                }
            }
            return new List<Wall>(walls);
        }

        public Cell GetStartingCell() {
            return startCell;
        }

        public Cell GetEndingCell()
        {
            return endCell;
        }
        public static int floodFills = 0;
        public bool DoesAvailablePathExistBetweenCells(Cell cell1, Cell cell2) {
            floodFills++;
            if (cell1 == cell2)
            {
                return true;
            }
            //Do flood fill algorithm on all cells that aren't in a path
            HashSet<Cell> travelledCells = new HashSet<Cell>();
            List<Cell> cellsToCheck = new List<Cell>();
            cellsToCheck.Add(cell1);
            travelledCells.Add(cell1);
            while (cellsToCheck.Count > 0) {
                //Exit if we hit the end
                var cell = cellsToCheck[cellsToCheck.Count - 1];
                cellsToCheck.RemoveAt(cellsToCheck.Count - 1);
                //Add the neighbors if the neighbors are not on a path
                var topCell = cell.TopCell;
                var bottomCell = cell.BottomCell;
                var leftCell = cell.LeftCell;
                var rightCell = cell.RightCell;
                if (topCell != null && !topCell.IsInPath && !travelledCells.Contains(topCell)) {
                    if (topCell == cell2)
                    {
                        return true;
                    }
                    cellsToCheck.Add(topCell);
                    travelledCells.Add(topCell);
                }
                if (bottomCell != null && !bottomCell.IsInPath && !travelledCells.Contains(bottomCell))
                {
                    if (bottomCell == cell2)
                    {
                        return true;
                    }
                    cellsToCheck.Add(bottomCell);
                    travelledCells.Add(bottomCell);
                }
                if (leftCell != null && !leftCell.IsInPath && !travelledCells.Contains(leftCell))
                {
                    if (leftCell == cell2)
                    {
                        return true;
                    }
                    cellsToCheck.Add(leftCell);
                    travelledCells.Add(leftCell);
                }
                if (rightCell != null && !rightCell.IsInPath && !travelledCells.Contains(rightCell))
                {
                    if (rightCell == cell2)
                    {
                        return true;
                    }
                    cellsToCheck.Add(rightCell);
                    travelledCells.Add(rightCell);
                }
            }

            return false;
        }
    }
}
