using System.Drawing;

namespace GazeLaser.Processor
{
    internal class GazePoint
    {
        public int Timestamp;
        public int X;
        public int Y;
        public GazePoint(int aTimestamp, Point aPoint)
        {
            Timestamp = aTimestamp;
            X = aPoint.X;
            Y = aPoint.Y;
        }
    }
}
