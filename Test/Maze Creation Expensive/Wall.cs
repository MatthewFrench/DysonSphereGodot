using System;
namespace Test.MazeCreationExpensive
{
    public enum WallDirection { Horizontal, Vertical };
    public class Wall
    {
        WallDirection direction = WallDirection.Horizontal;
        Cell topOrLeftCell = null;
        Cell bottomOrRightCell = null;
        float point1X = 0, point1Y = 0, point2X = 0, point2Y = 0;
        bool knockedDown = false;
        public Wall(WallDirection direction, Cell topOrLeftCell, Cell bottomOrRightCell, float point1X, float point1Y, float point2X, float point2Y)
        {
            this.direction = direction;
            this.topOrLeftCell = topOrLeftCell;
            this.bottomOrRightCell = bottomOrRightCell;
            this.point1X = point1X;
            this.point1Y = point1Y;
            this.point2X = point2X;
            this.point2Y = point2Y;
        }
        public void setKnockedDown(bool value) {
            knockedDown = value;
        }
        public bool isKnockedDown() {
            return knockedDown;
        }
        public void setTopOrLeftCell(Cell cell) {
            topOrLeftCell = cell;
        }
        public void setBottomOrRightCell(Cell cell) {
            bottomOrRightCell = cell;
        }
        public float GetPoint1X() {
            return point1X;
        }
        public float GetPoint1Y()
        {
            return point1Y;
        }
        public float GetPoint2X()
        {
            return point2X;
        }
        public float GetPoint2Y()
        {
            return point2Y;
        }
    }
}
