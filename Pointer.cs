using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GazeLaser
{
    public class Pointer
    {
        #region Declarations

        public enum Style
        {
            Spot,
            Circle,
            Ring
        }

        [Serializable]
        public class Settings
        {
            public Style Appearance { get; set; }
            public double Opacity { get; set; }
            public int Size { get; set; }

            public Settings()
            {
                // All default values must be set here explicitely
                Appearance = Style.Spot;
                Opacity = 0.3;
                Size = 100;
            }

            public Settings(Settings aRef)
            {
                Appearance = aRef.Appearance;
                Opacity = aRef.Opacity;
                Size = aRef.Size;
            }
        }

        #endregion

        #region Internal members

        private Dictionary<Style, Bitmap> iStyleImages = new Dictionary<Style, Bitmap>();
        private Stack<Settings> iSettingsBuffer = new Stack<Settings>();

        private PointerWidget iWidget;
        private Settings iSettings;

        #endregion

        #region Properties

        public Style Appearance
        {
            get { return iSettings.Appearance; }
            set
            {
                iSettings.Appearance = value;
                if (iStyleImages.ContainsKey(value))
                {
                    iWidget.BackgroundImage = iStyleImages[value];
                }
                else
                {
                    throw new ArgumentException("Pointer.Appearance");
                }
            }
        }

        public double Opacity
        {
            get { return iWidget.Opacity; }
            set
            {
                iSettings.Opacity = value;
                iWidget.Opacity = value;
            }
        }

        public int Size
        {
            get { return iWidget.Width; }
            set
            {
                iSettings.Size = value;
                iWidget.Width = value;
                iWidget.Height = value;
            }
        }

        #endregion

        #region Public methods

        public Pointer()
        {
            iStyleImages.Add(Style.Spot, Properties.Resources.pointerSpot);
            iStyleImages.Add(Style.Circle, Properties.Resources.pointerCircle);
            iStyleImages.Add(Style.Ring, Properties.Resources.pointerRing);
            
            iWidget = new PointerWidget();

            ApplySettings(ObjectStorage<Settings>.load());
        }

        ~Pointer()
        {
            ObjectStorage<Settings>.save(iSettings);
        }

        public void show()
        {
            int size = Size;
            iWidget.Show();
            Size = size;
        }

        public void hide()
        {
            iWidget.Hide();
        }

        public void pushSettings()
        {
            iSettingsBuffer.Push(iSettings);
            iSettings = new Settings(iSettings);
        }

        public void popSettings(bool aRestore)
        {
            if (iSettingsBuffer.Count == 0)
                return;

            Settings settings = iSettingsBuffer.Pop();

            if (aRestore)
            {
                ApplySettings(settings);
            }
        }

        public void moveTo(Point aLocation)
        {
            iWidget.Location = new Point(aLocation.X - iWidget.Width / 2, aLocation.Y - iWidget.Height / 2);
        }

        #endregion

        #region Internal methods

        private void ApplySettings(Settings aSettings)
        {
            iSettings = aSettings;
            
            Appearance = iSettings.Appearance;
            Opacity = iSettings.Opacity;
            Size = iSettings.Size;
        }

        #endregion
    }
}
