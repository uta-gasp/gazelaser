using System;
using System.Collections.Generic;
using System.Drawing;

namespace GazeLaser.Processor
{
    [Serializable]
    internal class TwoLevelLowPassFilter
    {
        /*
        //----------------------------------------------------------------------------
        // _    SUM(X[i]), if TimeWindow <= (Time[i] - Time[n]) < 2*TimeWindow
        // Xb = --------------------------------------------------------------
        //        SUM(1), if TimeWindow <= (Time[i] - Time[n]) < 2*TimeWindow
        //
        // _    SUM(X[i]), if 0 <= (Time[i]- Time[n]) < TimeWindow
        // Xa = --------------------------------------------------
        //       SUM(1), if 0 <= (Time[i] - Time[n]) < TimeWindow
        //
        //    where X[i] - X of a sample from the buffer
        //    where Time[i] - time of a sample from the buffer
        //    where Time[n] - time of the last sample
        //                 __________________________
        // Dist(Pb, Pa) = V(Xb - Xa)^2 + (Yb - Ya)^2
        //
        //     | TLow, if Dist(Pb, Pa) < Threshold
        // T = |
        //     | THigh, otherwise
        //
        // Alpha = T / SampleInterval
        //
        //      X[n] + Alpha * XMprev
        // XM = ---------------------
        //            1 + Alpha
        //
         * */

        #region Constants

        private const double T_LOW = 300;
        private const double T_HIGH = 10;
        private const int WINDOW_SIZE = 150;
        private const int FIXATION_THRESHOLD = 50;

        #endregion

        #region Internal members

        private double iX = Double.NaN;
        private double iY = Double.NaN;
        private double iT = 0;
        private int iInterval = 0;

        private Queue<GazePoint> iBuffer = new Queue<GazePoint>();
        private bool iEnoughPoints = false;

        #endregion

        #region Properties

        public double TLow { get; set; }
        public double THigh { get; set; }
        public int WindowSize { get; set; }
        public int FixationThreshold { get; set; }

        #endregion

        #region Public methods

        public TwoLevelLowPassFilter()
        {
            TLow = T_LOW;
            THigh = T_HIGH;
            WindowSize = WINDOW_SIZE;
            FixationThreshold = FIXATION_THRESHOLD;
        }

        public GazePoint feed(GazePoint aGazePoint)
        {
            if (Double.IsNaN(iX) || Double.IsNaN(iY) || iT == 0)
            {
                iX = aGazePoint.X;
                iY = aGazePoint.Y;
                iT = TLow;
            }

            bool enoughPoints = UpdateBuffer(aGazePoint);
            iT = EstimateCurrentT(aGazePoint);
            
            if (!iEnoughPoints && enoughPoints)
            {
                iEnoughPoints = true;
                if (iInterval == 0)
                {
                    iInterval = EstimateInterval();
                }
            }

            if (iEnoughPoints)
            {
                double alfa = ((double)iT) / iInterval;
                iX = (aGazePoint.X + alfa * iX) / (1.0 + alfa);
                iY = (aGazePoint.Y + alfa * iY) / (1.0 + alfa);
            }

            GazePoint result = new GazePoint(aGazePoint.Timestamp, new Point((int)iX, (int)iY));
            Console.WriteLine(String.Format("{0}\t{1}\t{2}\t{3}", aGazePoint.X, aGazePoint.Y, iX, iY));
            return result;
        }

        public void reset(int aInterval = 0)
        {
            iBuffer.Clear();
            iX = Double.NaN;
            iY = Double.NaN;
            iT = 0;
            iInterval = aInterval;
            iEnoughPoints = false;
        }

        #endregion

        #region Internal methods

        private bool UpdateBuffer(GazePoint aGazePoint)
        {
            bool isFilterValid = false;
            iBuffer.Enqueue(aGazePoint);
            while (aGazePoint.Timestamp - iBuffer.Peek().Timestamp > (2 * WindowSize))
            {
                iBuffer.Dequeue();
                isFilterValid = true;
            }
            return isFilterValid && iBuffer.Count > 1;
        }

        private double EstimateCurrentT(GazePoint aGazePoint)
        {
            double result = iT;

            int avgXB = 0;
            int avgYB = 0;
            int avgXA = 0;
            int avgYA = 0;
            int ptsBeforeCount = 0;
            int ptsAfterCount = 0;

            foreach (GazePoint gp in iBuffer)
            {
                long dt = aGazePoint.Timestamp - gp.Timestamp;
                if (dt > WindowSize)
                {
                    avgXB += gp.X;
                    avgYB += gp.Y;
                    ptsBeforeCount++;
                }
                else
                {
                    avgXA += gp.X;
                    avgYA += gp.Y;
                    ptsAfterCount++;
                }
            }

            if (ptsBeforeCount > 0 && ptsAfterCount > 0)
            {
                avgXB = avgXB / ptsBeforeCount;
                avgYB = avgYB / ptsBeforeCount;
                avgXA = avgXA / ptsAfterCount;
                avgYA = avgYA / ptsAfterCount;

                double dx = avgXB - avgXA;
                double dy = avgYB - avgYA;
                double dist = Math.Sqrt(dx * dx + dy * dy);

                result = dist > FixationThreshold ? THigh : TLow;
            }

            return result;
        }

        private int EstimateInterval()
        {
            int avgDT = 0;
            GazePoint refGP = null;
            foreach (GazePoint gp in iBuffer)
            {
                if (refGP != null)
                {
                    avgDT += gp.Timestamp - refGP.Timestamp;
                }
                refGP = gp;
            }
            return avgDT / (iBuffer.Count - 1);
        }

        #endregion
    }
}