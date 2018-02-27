using System;
using System.Collections.Generic;
using System.Drawing;
using Godot;
namespace Test
{
    public class Maze
    {
        int Rows;
        int Columns;
        int cellWidth;
        int cellHeight;
        int Width;
        int Height;
        Dictionary<string, Cell> cells = new Dictionary<string, Cell>();
        Stack<Cell> stack = new Stack<Cell>();

        public void Generate()
        {
            int c = 0;
            int r = 0;
            for (int y = 0; y <= Height; y += cellHeight)
            {
                for (int x = 0; x <= Width; x += cellWidth)
                {
                    Cell cell = new Cell(new Point(x, y), new Size(cellWidth, cellHeight), ref cells, r, c, (Rows - 1), (Columns - 1));
                    c += 1;
                }
                c = 0;
                r += 1;
            }

            Dig();
            GD.Print("Finished the maze generation");
        }
        public Dictionary<string, Cell> GetCells() {
            return cells;
        }
        private void Dig()
        {
            int r = 0;
            int c = 0;
            string key = "c" + 5 + "r" + 5;
            Cell startCell = cells[key];
            stack.Clear();
            startCell.Visited = true;
            while ((startCell != null))
            {
                startCell = startCell.Dig(ref stack);
                if (startCell != null)
                {
                    startCell.Visited = true;
                }
            }
            stack.Clear();
        }
        public Maze(int rows, int columns, int cellWidth, int cellHeight)
        {
            this.Rows = rows;
            this.Columns = columns;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            this.Width = (this.Columns * this.cellWidth) + 1;
            this.Height = (this.Rows * this.cellHeight) + 1;
        }
    }
}
