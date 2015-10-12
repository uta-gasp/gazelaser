using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using ETUDriver;

namespace GazeLaser
{
    public class GazeLaser
    {
        private class Menu
        {
            public struct TrackerState
            {
                public bool IsShowingOptions;
                public bool HasDevices;
                public bool IsConnected;
                public bool IsCalibrated;
                public bool IsTracking;
            }

            private ToolStripMenuItem tsmiOptions;
            private ToolStripMenuItem tsmiETUDOptions;
            private ToolStripMenuItem tsmiETUDCalibrate;
            private ToolStripMenuItem tsmiETUDToggleTracking;
            private ToolStripMenuItem tsmiExit;
            private ContextMenuStrip cmsMenu;

            public event EventHandler OnShowOptions = delegate { };
            public event EventHandler OnShowETUDOptions = delegate { };
            public event EventHandler OnCalibrate = delegate { };
            public event EventHandler OnToggleTracking = delegate { };
            public event EventHandler OnExit = delegate { };

            public ContextMenuStrip Strip { get { return cmsMenu; } }

            public Menu()
            {
                tsmiOptions = new ToolStripMenuItem("Options");
                tsmiOptions.Click += tsmiOptions_Click;

                tsmiETUDOptions = new ToolStripMenuItem("ETU-Driver");
                tsmiETUDOptions.Click += tsmiETUDOptions_Click;

                tsmiETUDCalibrate = new ToolStripMenuItem("Calibrate");
                tsmiETUDCalibrate.Click += tsmiETUDCalibrate_Click;

                tsmiETUDToggleTracking = new ToolStripMenuItem("Start");
                tsmiETUDToggleTracking.Click += tsmiETUDToggleTracking_Click;

                tsmiExit = new ToolStripMenuItem("Exit");
                tsmiExit.Click += tsmiExit_Click;

                cmsMenu = new ContextMenuStrip();

                cmsMenu.Items.Add(tsmiOptions);
                cmsMenu.Items.Add("-");
                cmsMenu.Items.Add(tsmiETUDOptions);
                cmsMenu.Items.Add(tsmiETUDCalibrate);
                cmsMenu.Items.Add(tsmiETUDToggleTracking);
                cmsMenu.Items.Add("-");
                cmsMenu.Items.Add(tsmiExit);
            }

            public void update(TrackerState aTrackerState)
            {
                tsmiOptions.Enabled = !aTrackerState.IsShowingOptions && !aTrackerState.IsTracking;
                tsmiETUDOptions.Enabled = !aTrackerState.IsShowingOptions && aTrackerState.HasDevices && !aTrackerState.IsTracking;
                tsmiETUDCalibrate.Enabled = !aTrackerState.IsShowingOptions && aTrackerState.IsConnected && !aTrackerState.IsTracking;
                tsmiETUDToggleTracking.Enabled = !aTrackerState.IsShowingOptions && aTrackerState.IsConnected && aTrackerState.IsCalibrated;
                tsmiETUDToggleTracking.Text = aTrackerState.IsTracking ? "Stop" : "Start";
                tsmiExit.Enabled = !aTrackerState.IsShowingOptions;
            }

            #region Event handlers

            void tsmiExit_Click(object sender, EventArgs e)
            {
                OnExit(this, new EventArgs());
            }

            void tsmiETUDToggleTracking_Click(object sender, EventArgs e)
            {
                OnToggleTracking(this, new EventArgs());
            }

            void tsmiETUDCalibrate_Click(object sender, EventArgs e)
            {
                OnCalibrate(this, new EventArgs());
            }

            void tsmiETUDOptions_Click(object sender, EventArgs e)
            {
                OnShowETUDOptions(this, new EventArgs());
            }

            void tsmiOptions_Click(object sender, EventArgs e)
            {
                OnShowOptions(this, new EventArgs());
            }

            #endregion
        }

        private CoETUDriver iETUDriver;
        private Pointer iPointer;
        private Menu iMenu;
        private Options iOptions;
        private NotifyIcon iTrayIcon;

        private bool iExitAfterTrackingStopped = false;

        public GazeLaser()
        {
            iETUDriver = new CoETUDriver();
            iETUDriver.OptionsFile = Application.StartupPath + "\\etudriver.ini";
            iETUDriver.OnRecordingStart += ETUDriver_OnRecordingStart;
            iETUDriver.OnRecordingStop += ETUDriver_OnRecordingStop;
            iETUDriver.OnCalibrated += ETUDriver_OnCalibrated;
            iETUDriver.OnDataEvent += ETUDriver_OnDataEvent;

            iPointer = new Pointer();
            iPointer.show();

            iMenu = new Menu();
            iMenu.OnShowOptions += Menu_OnShowOptions;
            iMenu.OnShowETUDOptions += Menu_OnShowETUDOptions;
            iMenu.OnCalibrate += Menu_OnCalibrate;
            iMenu.OnToggleTracking += Menu_OnToggleTracking;
            iMenu.OnExit += Menu_OnExit;

            iOptions = new Options();

            iTrayIcon = new NotifyIcon();
            iTrayIcon.Icon = iOptions.Icon;
            iTrayIcon.ContextMenuStrip = iMenu.Strip;
            iTrayIcon.Text = "GazeLaser";
            iTrayIcon.Visible = true;

            UpdateMenu(false);
        }

        private void UpdateMenu(bool aIsShowingDialog)
        {
            Menu.TrackerState trackerState = new Menu.TrackerState();
            trackerState.IsShowingOptions = aIsShowingDialog;
            trackerState.HasDevices = iETUDriver.DeviceCount > 0;
            trackerState.IsConnected = iETUDriver.Ready != 0;
            trackerState.IsCalibrated = iETUDriver.Calibrated != 0;
            trackerState.IsTracking = iETUDriver.Active != 0;
            iMenu.update(trackerState);
        }

        private void Exit()
        {
            iTrayIcon.Visible = false;
            iETUDriver = null;
            GC.Collect();
            Application.Exit();
        }

        #region ETUD event handlers

        private void ETUDriver_OnCalibrated()
        {
            UpdateMenu(false);
        }

        private void ETUDriver_OnRecordingStart()
        {
            UpdateMenu(false);
            iPointer.show();
        }

        private void ETUDriver_OnRecordingStop()
        {
            iPointer.hide();
            UpdateMenu(false);

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

        #endregion

        #region Menu

        private void Menu_OnShowOptions(object aSender, EventArgs aArgs)
        {
            UpdateMenu(true);
            iOptions.load(iPointer);
            if (iOptions.ShowDialog() == DialogResult.OK)
            {
                iOptions.save(iPointer);
            }
            UpdateMenu(false);
        }

        private void Menu_OnShowETUDOptions(object aSender, EventArgs aArgs)
        {
            UpdateMenu(true);
            iETUDriver.showRecordingOptions();
            UpdateMenu(false);
        }

        private void Menu_OnCalibrate(object aSender, EventArgs aArgs)
        {
            UpdateMenu(true);
            iETUDriver.calibrate();
            UpdateMenu(false);
        }

        private void Menu_OnToggleTracking(object aSender, EventArgs aArgs)
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

        private void Menu_OnExit(object aSender, EventArgs aArgs)
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

        #endregion
    }
}
