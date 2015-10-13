using System.Drawing;

namespace GazeLaser.Processor
{
    public class GazePoint
    {
        public int Timestamp { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }

        public GazePoint(int aTimestamp, Point aPoint)
        {
            Timestamp = aTimestamp;
            X = aPoint.X;
            Y = aPoint.Y;
        }
    }
}
