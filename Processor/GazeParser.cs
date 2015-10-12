using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GazeLaser.Processor
{
    public class GazeParser
    {
        #region Consts

        public static int SAMPLE_INTERVAL { get { return 30; } }

        #endregion

        #region Internal members

        private PointF iLastPoint = PointF.Empty;
        private TwoLevelLowPassFilter iFilter = new TwoLevelLowPassFilter();

        private Queue<GazePoint> iPointBuffer = new Queue<GazePoint>();
        private System.Windows.Forms.Timer iPointsTimer = new System.Windows.Forms.Timer();

        #endregion

        #region Events

        public class NewGazePointArgs : EventArgs
        {
            public Point Location { get; private set; }
            public NewGazePointArgs(Point aLocation)
            {
                Location = aLocation;
            }
        }
        public delegate void NewGazePointHandler(object aSender, NewGazePointArgs aArgs);
        public event NewGazePointHandler OnNewGazePoint = delegate { };

        #endregion

        #region Public methods

        public GazeParser()
        {
            iPointsTimer.Interval = SAMPLE_INTERVAL;
            iPointsTimer.Tick += PointsTimer_Tick;
        }

        public void start()
        {
            iLastPoint = PointF.Empty;
            iFilter.reset(SAMPLE_INTERVAL);

            iPointBuffer.Clear();
            iPointsTimer.Start();
        }

        public void stop()
        {
            iPointsTimer.Stop();
        }

        public void feed(int aTimestamp, int aX, int aY)
        {
            Point gazePoint = new Point(aX, aY);
            lock (iPointBuffer)
            {
                iPointBuffer.Enqueue(new GazePoint(aTimestamp, gazePoint));
            }
        }

        #endregion

        #region Internal methods

        private void PointsTimer_Tick(object sender, EventArgs e)
        {
            int timestamp = 0;
            Point point = new Point(0, 0);
            int bufferSize = 0;

            lock (iPointBuffer)
            {
                while (iPointBuffer.Count > 0)
                {
                    GazePoint gp = iPointBuffer.Dequeue();
                    timestamp = gp.Timestamp;
                    point.X += gp.X;
                    point.Y += gp.Y;
                    bufferSize++;
                }
            }

            if (bufferSize > 0)
            {
                point.X /= bufferSize;
                point.Y /= bufferSize;

                GazePoint gazePoint = iFilter.feed(new GazePoint(timestamp, point));
                OnNewGazePoint(this, new NewGazePointArgs(new Point(gazePoint.X, gazePoint.Y)));
            }
        }

        #endregion
    }
}
