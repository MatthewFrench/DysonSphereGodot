using System;
using System.Collections.Generic;
using System.Drawing;
namespace Test
{
    public class Cell
    {
        public bool NorthWall = true;
        public bool SouthWall = true;
        public bool WestWall = true;
        public bool EastWall = true;
        public string id;
        public Dictionary<string, Cell> Cells;
        public int Column;
        public int Row;
        public string NeighborNorthID;
        public string NeighborSouthID;
        public string NeighborEastID;
        public string NeighborWestID;
        public bool Visited = false;
        public Stack<Cell> Stack;
        Random rnd = new Random();
        public Cell(Point location, Size size, ref Dictionary<string, Cell> cellList, int r, int c, int maxR, int maxC)
        {
            this.Column = c;
            this.Row = r;
            this.id = "c" + c + "r" + r;
            int rowNort = r - 1;
            int rowSout = r + 1;
            int colEast = c + 1;
            int colWest = c - 1;
            NeighborNorthID = "c" + c + "r" + rowNort;
            NeighborSouthID = "c" + c + "r" + rowSout;
            NeighborEastID = "c" + colEast + "r" + r;
            NeighborWestID = "c" + colWest + "r" + r;
            if (rowNort < 0) NeighborNorthID = "none";
            if (rowSout > maxR) NeighborSouthID = "none";
            if (colEast > maxC) NeighborEastID = "none";
            if (colWest < 0) NeighborWestID = "none";
            this.Cells = cellList;
            this.Cells.Add(this.id, this);
        }
        public Cell getNeighbor()
        {
            List<Cell> c = new List<Cell>();
            if (!(NeighborNorthID == "none") && Cells[NeighborNorthID].Visited == false) c.Add(Cells[NeighborNorthID]);
            if (!(NeighborSouthID == "none") && Cells[NeighborSouthID].Visited == false) c.Add(Cells[NeighborSouthID]);
            if (!(NeighborEastID == "none") && Cells[NeighborEastID].Visited == false) c.Add(Cells[NeighborEastID]);
            if (!(NeighborWestID == "none") && Cells[NeighborWestID].Visited == false) c.Add(Cells[NeighborWestID]);
            int max = c.Count;
            Cell currentCell = null;
            if (c.Count > 0)
            {
                int index = (int)(this.rnd.NextDouble() * c.Count);
                currentCell = c[index];
            }
            return currentCell;
        }
        public Cell Dig(ref Stack<Cell> stack)
        {
            this.Stack = stack;
            Cell nextCell = getNeighbor();
            if ((nextCell != null))
            {
                stack.Push(nextCell);
                if (nextCell.id == this.NeighborNorthID)
                {
                    this.NorthWall = false;
                    nextCell.SouthWall = false;
                }
                else if (nextCell.id == this.NeighborSouthID)
                {
                    this.SouthWall = false;
                    nextCell.NorthWall = false;
                }
                else if (nextCell.id == this.NeighborEastID)
                {
                    this.EastWall = false;
                    nextCell.WestWall = false;
                }
                else if (nextCell.id == this.NeighborWestID)
                {
                    this.WestWall = false;
                    nextCell.EastWall = false;
                }
            }
            else if (!(stack.Count == 0))
            {
                nextCell = stack.Pop();
            }
            else
            {
                return null;
            }
            return nextCell;
        }
    }
}
