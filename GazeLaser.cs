using System;
using System.Windows.Forms;
using ETUDriver;

namespace GazeLaser
{
    public partial class GazeLaser : Form
    {
        private CoETUDriver iETUDriver;
        private bool iExitAfterTrackingStopped = false;
        
        public GazeLaser()
        {
            InitializeComponent();

            iETUDriver = new CoETUDriver();
            iETUDriver.OptionsFile = Application.StartupPath + "\\etudriver.ini";
            iETUDriver.OnRecordingStart += ETUDriver_OnRecordingStart;
            iETUDriver.OnRecordingStop += ETUDriver_OnRecordingStop;
            iETUDriver.OnCalibrated += ETUDriver_OnCalibrated;
            iETUDriver.OnDataEvent += ETUDriver_OnDataEvent;

            EnabledMenuButtons();
        }

        private void EnabledMenuButtons()
        {
            tsmOptions.Enabled = iETUDriver.DeviceCount > 0 && iETUDriver.Active == 0;
            tsmCalibrate.Enabled = iETUDriver.Ready != 0 && iETUDriver.Active == 0;
            tsmToggleTracking.Enabled = iETUDriver.Ready != 0 && iETUDriver.Calibrated != 0;
        }

        private void Exit()
        {
            iETUDriver = null;
            this.Close();
        }

        private void ETUDriver_OnCalibrated()
        {
            EnabledMenuButtons();
        }

        private void ETUDriver_OnRecordingStart()
        {
            EnabledMenuButtons();
            tsmToggleTracking.Text = "Stop";
        }

        private void ETUDriver_OnRecordingStop()
        {
            EnabledMenuButtons();
            tsmToggleTracking.Text = "Start";

            if (iExitAfterTrackingStopped)
            {
                Exit();
            }
        }

        private void ETUDriver_OnDataEvent(EiETUDGazeEvent aEventID, ref int aData, ref int aResult)
        {
            if (aEventID == EiETUDGazeEvent.geSample)
            {
                SiETUDSample smp = iETUDriver.LastSample;
                //Point pt = pcbControl.PointToClient(new Point((int)smp.X[0], (int)smp.Y[0]));
                //iParser.feed(smp.Time, pt);
            }
        }

        private void tsmOptions_Click(object sender, EventArgs e)
        {
            iETUDriver.showRecordingOptions();
        }

        private void tsmCalibrate_Click(object sender, EventArgs e)
        {
            iETUDriver.calibrate();
        }

        private void tsmToggleTracking_Click(object sender, EventArgs e)
        {
            if (iETUDriver.Active == 0)
            {
                iETUDriver.startTracking();
            }
            else
            {
                iETUDriver.stopTracking();
            }
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            if (iETUDriver.Active != 0)
            {
                iExitAfterTrackingStopped = true;
                iETUDriver.stopTracking();
            }
            else
            {
                Exit();
            }
        }
    }
}
