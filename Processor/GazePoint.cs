using System.Drawing;

namespace GazeLaser.Processor
{
    public class GazePoint
    {
        public int Timestamp { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }
        public PointF Point { get { return new PointF(X, Y); } }

        public GazePoint(int aTimestamp, PointF aPoint)
        {
            Timestamp = aTimestamp;
            X = aPoint.X;
            Y = aPoint.Y;
        }
    }
}
