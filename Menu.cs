using System;
using System.Windows.Forms;

namespace GazeLaser
{
    public class Menu
    {
        #region Declarations

        public struct TrackerState
        {
            public bool IsShowingOptions;
            public bool HasDevices;
            public bool IsConnected;
            public bool IsCalibrated;
            public bool IsTracking;
        }

        #endregion

        #region Internal members

        private ToolStripMenuItem tsmiOptions;
        private ToolStripMenuItem tsmiETUDOptions;
        private ToolStripMenuItem tsmiETUDCalibrate;
        private ToolStripMenuItem tsmiETUDToggleTracking;
        private ToolStripMenuItem tsmiExit;
        private ContextMenuStrip cmsMenu;

        #endregion

        #region Events

        public event Action OnShowOptions = delegate { };
        public event Action OnShowETUDOptions = delegate { };
        public event Action OnCalibrate = delegate { };
        public event Action OnToggleTracking = delegate { };
        public event Action OnExit = delegate { };

        #endregion

        #region Properties

        public ContextMenuStrip Strip { get { return cmsMenu; } }

        #endregion

        #region Public methods

        public Menu()
        {
            tsmiOptions = new ToolStripMenuItem("Options");
            tsmiOptions.Click += new EventHandler((s, e) => OnShowOptions());

            tsmiETUDOptions = new ToolStripMenuItem("ETU-Driver");
            tsmiETUDOptions.Click += new EventHandler((s, e) => OnShowETUDOptions());

            tsmiETUDCalibrate = new ToolStripMenuItem("Calibrate");
            tsmiETUDCalibrate.Click += new EventHandler((s, e) => OnCalibrate());

            tsmiETUDToggleTracking = new ToolStripMenuItem("Start");
            tsmiETUDToggleTracking.Click += new EventHandler((s, e) => OnToggleTracking());

            tsmiExit = new ToolStripMenuItem("Exit");
            tsmiExit.Click += new EventHandler((s, e) => OnExit());

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
            tsmiOptions.Enabled = !aTrackerState.IsShowingOptions;// && !aTrackerState.IsTracking;
            tsmiETUDOptions.Enabled = !aTrackerState.IsShowingOptions && aTrackerState.HasDevices && !aTrackerState.IsTracking;
            tsmiETUDCalibrate.Enabled = !aTrackerState.IsShowingOptions && aTrackerState.IsConnected && !aTrackerState.IsTracking;
            tsmiETUDToggleTracking.Enabled = !aTrackerState.IsShowingOptions && aTrackerState.IsConnected && aTrackerState.IsCalibrated;
            tsmiETUDToggleTracking.Text = aTrackerState.IsTracking ? "Stop" : "Start";
            tsmiExit.Enabled = !aTrackerState.IsShowingOptions;
        }

        #endregion
    }
}
