using System;
using System.Windows.Forms;

namespace GazeLaser.Utils
{
    public class DelayedAction
    {
        private Timer iTimer = new Timer();
        private Action iAction;

        public DelayedAction(Action aAction, int aDelay)
        {
            iAction = aAction;
            iTimer.Interval = aDelay;

            iTimer.Tick += Timer_Tick;
            iTimer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
 	        iTimer.Stop();
            iAction();
        }
    }
}
