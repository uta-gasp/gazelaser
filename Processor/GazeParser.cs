using System;
using System.Collections.Generic;
using System.Drawing;

namespace GazeLaser.Processor
{
    public class GazeParser : IDisposable
    {
        #region Consts

        public static int SAMPLE_INTERVAL { get { return 30; } }

        #endregion

        #region Internal members

        private PointF iLastPoint = PointF.Empty;
        private Queue<GazePoint> iPointBuffer = new Queue<GazePoint>();
        private System.Windows.Forms.Timer iPointsTimer = new System.Windows.Forms.Timer();
        private bool iDisposed = false;

        #endregion

        #region Events

        public class NewGazePointArgs : EventArgs
        {
            public PointF Location { get; private set; }
            public NewGazePointArgs(PointF aLocation)
            {
                Location = aLocation;
            }
        }
        public delegate void NewGazePointHandler(object aSender, NewGazePointArgs aArgs);
        public event NewGazePointHandler OnNewGazePoint = delegate { };

        #endregion

        #region Properties

        public TwoLevelLowPassFilter Filter { get; private set; }

        #endregion

        #region Public methods

        public GazeParser()
        {
            Filter = Utils.Storage<TwoLevelLowPassFilter>.load();

            iPointsTimer.Interval = SAMPLE_INTERVAL;
            iPointsTimer.Tick += PointsTimer_Tick;
        }

        public void start()
        {
            iLastPoint = PointF.Empty;
            Filter.reset(SAMPLE_INTERVAL);

            iPointBuffer.Clear();
            iPointsTimer.Start();
        }

        public void stop()
        {
            iPointsTimer.Stop();
        }

        public void feed(int aTimestamp, PointF aGazePoint)
        {
            if (aGazePoint.X <= 0 && aGazePoint.Y <= 0)
                return;

            lock (iPointBuffer)
            {
                iPointBuffer.Enqueue(new GazePoint(aTimestamp, aGazePoint));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Internal methods

        protected virtual void Dispose(bool aDisposing)
        {
            if (iDisposed)
                return;

            if (aDisposing)
            {
                Utils.Storage<TwoLevelLowPassFilter>.save(Filter);
            }

            iDisposed = true;
        }

        private void PointsTimer_Tick(object sender, EventArgs e)
        {
            int timestamp = 0;
            PointF point = new PointF(0, 0);
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

                GazePoint rawGazePoint = new GazePoint(timestamp, point);
                GazePoint smoothed = Filter.feed(rawGazePoint);
                OnNewGazePoint(this, new NewGazePointArgs(smoothed.Point));
            }
        }

        #endregion
    }
}
