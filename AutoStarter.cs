using System;

namespace GazeLaser
{
    [Serializable]
    public class AutoStarter
    {
        private Utils.DelayedAction iDelayedAction;
        private GazeLaser iGazeLaser;
        private bool iFinished = true;

        public bool Enabled { get; set; }

        public AutoStarter()
        {
            Enabled = false;
        }

        public void run(GazeLaser aGazeLaser)
        {
            if (!iFinished)
                return;

            iGazeLaser = aGazeLaser;
            iFinished = false;

            iDelayedAction = new Utils.DelayedAction(() => Next(true), 1000);
        }

        private void Next(bool aCanRunCalibration)
        {
            if (iGazeLaser.State == GazeLaser.TrackingState.NotAvailable ||
                iGazeLaser.State == GazeLaser.TrackingState.Tracking)
            {
                return;
            }
            else if (iGazeLaser.State == GazeLaser.TrackingState.Disconnected)
            {
                iDelayedAction = new Utils.DelayedAction(iGazeLaser.showETUDOptions, 500);
            }
            else
            {
                if (iGazeLaser.State == GazeLaser.TrackingState.Connected && aCanRunCalibration)
                {
                    iDelayedAction = new Utils.DelayedAction(() =>
                    {
                        iGazeLaser.calibrate();
                        iDelayedAction = new Utils.DelayedAction(() => Next(false), 1000);
                    }, 500);
                }
                else if (iGazeLaser.State == GazeLaser.TrackingState.Calibrated)
                {
                    iDelayedAction = new Utils.DelayedAction(iGazeLaser.toggleTracking, 500);
                }
            }
        }
    }
}
