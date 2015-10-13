using System;
using System.Windows.Forms;
using ETUDriver;

namespace GazeLaser
{
    public class GazeLaser : IDisposable
    {
        public enum TrackingState
        {
            NotAvailable,
            Disconnected,
            Connected,
            Calibrating,
            Calibrated,
            Tracking
        }

        #region Internal members

        private CoETUDriver iETUDriver;
        private Processor.GazeParser iGazeParser;
        private Pointer iPointer;
        private Menu iMenu;
        private Options iOptions;
        private NotifyIcon iTrayIcon;

        private bool iExitAfterTrackingStopped = false;
        private bool iDisposed = false;

        #endregion

        #region Properties

        public AutoStarter AutoStarter { get; private set; }
        public TrackingState State
        {
            get
            {
                GazeLaser.TrackingState result = TrackingState.NotAvailable;
                if (iETUDriver.Active != 0)
                    result = GazeLaser.TrackingState.Tracking;
                else if (iETUDriver.Calibrated != 0)
                    result = GazeLaser.TrackingState.Calibrated;
                else switch (iETUDriver.Ready)
                    {
                        case 1: result = GazeLaser.TrackingState.Disconnected; break;
                        case 2: result = GazeLaser.TrackingState.Connected; break;
                        case 3: result = GazeLaser.TrackingState.Calibrating; break;
                    }
                return result;
            }
        }

        #endregion

        #region Public methods

        public GazeLaser()
        {
            AutoStarter = Utils.ObjectStorage<AutoStarter>.load();

            iETUDriver = new CoETUDriver();
            iETUDriver.OptionsFile = Application.StartupPath + "\\etudriver.ini";
            iETUDriver.OnRecordingStart += ETUDriver_OnRecordingStart;
            iETUDriver.OnRecordingStop += ETUDriver_OnRecordingStop;
            iETUDriver.OnCalibrated += ETUDriver_OnCalibrated;
            iETUDriver.OnDataEvent += ETUDriver_OnDataEvent;

            iGazeParser = new Processor.GazeParser();
            iGazeParser.OnNewGazePoint += GazeParser_OnNewGazePoint;

            iPointer = new Pointer();

            iMenu = new Menu();
            iMenu.OnShowOptions += showOptions;
            iMenu.OnShowETUDOptions += showETUDOptions;
            iMenu.OnCalibrate += calibrate;
            iMenu.OnToggleTracking += toggleTracking;
            iMenu.OnExit += Menu_Exit;

            iOptions = new Options();

            iTrayIcon = new NotifyIcon();
            iTrayIcon.Icon = iOptions.Icon;
            iTrayIcon.ContextMenuStrip = iMenu.Strip;
            iTrayIcon.Text = "GazeLaser";
            iTrayIcon.Visible = true;

            Utils.GlobalShortcut.add(new Utils.Shortcut("Pointer", new Action(Shortcut_TogglePointer), Keys.Pause));
            Utils.GlobalShortcut.add(new Utils.Shortcut("Tracking", new Action(Shortcut_TrackingNext), Keys.PrintScreen, Keys.Control));
            Utils.GlobalShortcut.init();

            UpdateMenu(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void showOptions()
        {
            UpdateMenu(true);
            iOptions.load(iPointer, iGazeParser.Filter, AutoStarter);
            bool acceptChanges = iOptions.ShowDialog() == DialogResult.OK;
            iOptions.save(acceptChanges);

            UpdateMenu(false);
        }

        public void showETUDOptions()
        {
            UpdateMenu(true);
            iETUDriver.showRecordingOptions();
            UpdateMenu(false);
        }

        public void calibrate()
        {
            UpdateMenu(true);
            iETUDriver.calibrate();
            UpdateMenu(false);
        }

        public void toggleTracking()
        {
            if (iETUDriver.Active == 0)
            {
                iGazeParser.start();
                iETUDriver.startTracking();
            }
            else
            {
                iETUDriver.stopTracking();
                iGazeParser.stop();
            }
        }

        #endregion

        #region Internal methods

        protected virtual void Dispose(bool aDisposing)
        {
            if (iDisposed)
                return;

            if (aDisposing)
            {
                // Free any other managed objects here.
                iPointer.Dispose();
                iGazeParser.Dispose();
                Utils.ObjectStorage<AutoStarter>.save(AutoStarter);
                Utils.GlobalShortcut.close();
            }

            // Free any unmanaged objects here.
            iETUDriver = null;
            
            iDisposed = true;
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
            Application.Exit();
        }

        #endregion

        #region ETUD event handlers

        private void ETUDriver_OnCalibrated()
        {
            UpdateMenu(false);
        }

        private void ETUDriver_OnRecordingStart()
        {
            UpdateMenu(false);
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
                iGazeParser.feed(smp.Time, (int)smp.X[0], (int)smp.Y[0]);
            }
        }

        #endregion

        #region Other event handlers

        private void Menu_Exit()
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

        private void GazeParser_OnNewGazePoint(object aSender, Processor.GazeParser.NewGazePointArgs aArgs)
        {
            iPointer.moveTo(aArgs.Location);
        }

        private void Shortcut_TogglePointer()
        {
            if (iPointer.Visible)
                iPointer.hide();
            else
                iPointer.show();
        }

        private void Shortcut_TrackingNext()
        {
            if (State == TrackingState.Disconnected)
                showETUDOptions();
            else if (State == TrackingState.Connected)
                calibrate();
            else if (State == TrackingState.Calibrated)
                toggleTracking();
            else if (State == TrackingState.Tracking)
                toggleTracking();
        }

        #endregion
    }
}
