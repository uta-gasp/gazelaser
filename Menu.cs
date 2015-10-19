using System;
using System.Windows.Forms;

namespace GazeLaser
{
    public class Menu
    {
        #region Declarations

        public struct State
        {
            public bool IsShowingOptions;
            public bool IsVisible;
            public bool HasDevices;
            public bool IsConnected;
            public bool IsCalibrated;
            public bool IsTracking;
        }

        #endregion

        #region Internal members

        private ToolStripMenuItem tsmiOptions;
        private ToolStripMenuItem tsmiToggleVisibility;
        private ToolStripMenuItem tsmiETUDOptions;
        private ToolStripMenuItem tsmiETUDCalibrate;
        private ToolStripMenuItem tsmiETUDToggleTracking;
        private ToolStripMenuItem tsmiExit;
        private ContextMenuStrip cmsMenu;

        #endregion

        #region Events

        public event Action OnShowOptions = delegate { };
        public event Action OnToggleVisibility = delegate { };
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

            tsmiToggleVisibility = new ToolStripMenuItem("Show");
            tsmiToggleVisibility.Click += new EventHandler((s, e) => OnToggleVisibility());

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
            cmsMenu.Items.Add(tsmiToggleVisibility);
            cmsMenu.Items.Add("-");
            cmsMenu.Items.Add(tsmiETUDOptions);
            cmsMenu.Items.Add(tsmiETUDCalibrate);
            cmsMenu.Items.Add(tsmiETUDToggleTracking);
            cmsMenu.Items.Add("-");
            cmsMenu.Items.Add(tsmiExit);
        }

        public void update(State aState)
        {
            tsmiOptions.Enabled = !aState.IsShowingOptions;// && !aState.IsTracking;
            tsmiToggleVisibility.Text = aState.IsVisible ? "Hide" : "Show";
            tsmiETUDOptions.Enabled = !aState.IsShowingOptions && aState.HasDevices && !aState.IsTracking;
            tsmiETUDCalibrate.Enabled = !aState.IsShowingOptions && aState.IsConnected && !aState.IsTracking;
            tsmiETUDToggleTracking.Enabled = !aState.IsShowingOptions && aState.IsConnected && aState.IsCalibrated;
            tsmiETUDToggleTracking.Text = aState.IsTracking ? "Stop" : "Start";
            tsmiExit.Enabled = !aState.IsShowingOptions;
        }

        #endregion
    }
}
