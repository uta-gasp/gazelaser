using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace GazeLaser
{
    public class Pointer : IDisposable
    {
        #region Declarations

        // should match image names (without the preceiding 'pointer')
        public enum Style
        {
            Baloon,
            CircleDashed,
            CircleStripped,
            CrossX,
            CrossXT,
            CrossP,
            CrossPT,
            Dots,
            Frame,
            FrameRounded,
            FrameRoundedDashed,
            SpotLight,
            SpotDark,
            CircleDashedAnim,
            CircleStrippedAnim,
            CrossPAnim,
            CrossPTAnim,
            DotsAnim,
        }

        [Serializable]
        public class Settings
        {
            public Style Appearance { get; set; }
            public double Opacity { get; set; }
            public int Size { get; set; }
            public long FadingInterval { get; set; }
            public long NoDataVisibilityInterval { get; set; }

            public Settings()
            {
                // All default values must be set here explicitely
                Appearance = Style.FrameRoundedDashed;
                Opacity = 0.3;
                Size = 100;
                FadingInterval = 300;
                NoDataVisibilityInterval = 1000;
            }

            public Settings(Pointer aPointer)
            {
                loadFrom(aPointer);
            }

            public void loadFrom(Pointer aPointer)
            {
                Appearance = aPointer.Appearance;
                Opacity = aPointer.Opacity;
                Size = aPointer.Size;
                FadingInterval = aPointer.FadingInterval;
                NoDataVisibilityInterval = aPointer.NoDataVisibilityInterval;
            }

            public void saveTo(Pointer aPointer)
            {
                aPointer.Appearance = Appearance;
                aPointer.Opacity = Opacity;
                aPointer.Size = Size;
                aPointer.FadingInterval = FadingInterval;
                aPointer.NoDataVisibilityInterval = NoDataVisibilityInterval;
            }
        }

        #endregion

        #region Internal members

        private Dictionary<Style, Bitmap> iStyleImages = new Dictionary<Style, Bitmap>();
        private Stack<Settings> iSettingsBuffer = new Stack<Settings>();
        private bool iDisposed = false;

        private PointerWidget iWidget;
        private Style iAppearance;
        private double iOpacity;
        private double iDataAvailability;

        private long iLastDataTimestamp = 0;
        private Libs.HiResTimestamp iHRTimestamp;
        private Timer iDataAvailabilityTimer;

        #endregion

        #region Properties

        // Settings

        public Style Appearance
        {
            get { return iAppearance; }
            set
            {
                iAppearance = value;
                if (iStyleImages.ContainsKey(value))
                {
                    iWidget.setImage(iStyleImages[value]);
                }
                else
                {
                    throw new ArgumentException("Pointer.Appearance");
                }
            }
        }

        public double Opacity
        {
            get { return iOpacity; }
            set
            {
                iOpacity = value;
                UpdateWidgetOpacity();
            }
        }

        public int Size
        {
            get { return iWidget.Width; }
            set
            {
                iWidget.Width = value;
                iWidget.Height = value;
            }
        }

        public long FadingInterval { get; set; }

        public long NoDataVisibilityInterval { get; set; }

        // Other

        public bool Visible { get { return iWidget.Visible; } }
        public bool VisilityFollowsDataAvailability { get; set; }

        #endregion

        #region Events

        public event EventHandler OnHide = delegate { };
        public event EventHandler OnShow = delegate { };

        #endregion

        #region Public methods

        public Pointer()
        {
            LoadStyleImages();

            iWidget = new PointerWidget();

            iDataAvailabilityTimer = new Timer();
            iDataAvailabilityTimer.Interval = 30;
            iDataAvailabilityTimer.Tick += DataAvailabilityTimer_Tick;

            iHRTimestamp = new Libs.HiResTimestamp();

            VisilityFollowsDataAvailability = false;

            Settings settings = Utils.Storage<Settings>.load();
            settings.saveTo(this);
        }

        public void show()
        {
            int size = Size;
            iDataAvailability = 1.0;
            UpdateWidgetOpacity();
            iWidget.Show();
            Size = size;

            iDataAvailabilityTimer.Start();

            iLastDataTimestamp = 0;
        }

        public void hide()
        {
            iWidget.Hide();
            iDataAvailabilityTimer.Stop();
        }

        public void pushSettings()
        {
            Settings settings = new Settings(this);
            iSettingsBuffer.Push(settings);
        }

        public void popSettings(bool aRestore)
        {
            if (iSettingsBuffer.Count == 0)
                return;

            Settings settings = iSettingsBuffer.Pop();

            if (aRestore)
            {
                settings.saveTo(this);
            }
        }

        public void moveTo(PointF aLocation)
        {
            iLastDataTimestamp = iHRTimestamp.Milliseconds;

            if (iWidget.Visible)
            {
                iWidget.Location = new Point((int)(aLocation.X - iWidget.Width / 2), (int)(aLocation.Y - iWidget.Height / 2));
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Event handlers

        private void DataAvailabilityTimer_Tick(object sender, EventArgs e)
        {
            double dataAvailability = VisilityFollowsDataAvailability ? -1.0 : 1.0;
            double prevDataAvailability = iDataAvailability;

            if (VisilityFollowsDataAvailability)
            {
                long dataNotAvailableInterval = iHRTimestamp.Milliseconds - iLastDataTimestamp;
                if (dataNotAvailableInterval > NoDataVisibilityInterval)
                {
                    dataAvailability = 1.0 - Math.Min(1.0, (double)(dataNotAvailableInterval - NoDataVisibilityInterval) / FadingInterval);
                }
                else
                {
                    double opacityIncreaseStep = (double)(iDataAvailabilityTimer.Interval) / FadingInterval;
                    dataAvailability = Math.Min(1.0, iDataAvailability + opacityIncreaseStep);
                }
            }

            if (dataAvailability != iDataAvailability)
            {
                iDataAvailability = dataAvailability;
                UpdateWidgetOpacity();

                if (iDataAvailability == 0.0)
                {
                    if (prevDataAvailability != 1.0)
                    {
                        OnHide(this, new EventArgs());
                    }
                }
                else if (iDataAvailability == 1.0)
                {
                    OnShow(this, new EventArgs());
                }
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
                Settings settings = new Settings(this);
                Utils.Storage<Settings>.save(settings);
            }

            iDisposed = true;
        }

        private void LoadStyleImages()
        {
            Type resourceManagerType = typeof(Properties.Resources);
            foreach (Style style in Enum.GetValues(typeof(Style)))
            {
                string imageName = "pointer" + style.ToString();
                PropertyInfo property = resourceManagerType.GetProperty(imageName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                if (property == null)
                    throw new Exception("Internal error: missing some images for Pointer");

                object image = property.GetValue(null, null);
                if (image == null)
                    throw new Exception("Internal error: null image for Pointer");

                iStyleImages.Add(style, (Bitmap)image);
            }
        }

        private void UpdateWidgetOpacity()
        {
            iWidget.Opacity = iOpacity * iDataAvailability;
        }

        #endregion
    }
}
