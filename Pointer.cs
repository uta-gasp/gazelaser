using System;
using System.Collections.Generic;
using System.Drawing;

namespace GazeLaser
{
    public class Pointer : IDisposable
    {
        #region Declarations

        public enum Style
        {
            Spot,
            Circle,
            Ring,
            Cross,
            Dots,
            Rect,
            CorssAni
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

            public Settings(Pointer aPointer)
            {
                loadFrom(aPointer);
            }

            public void loadFrom(Pointer aPointer)
            {
                Appearance = aPointer.Appearance;
                Opacity = aPointer.Opacity;
                Size = aPointer.Size;
            }

            public void saveTo(Pointer aPointer)
            {
                aPointer.Appearance = Appearance;
                aPointer.Opacity = Opacity;
                aPointer.Size = Size;
            }
        }

        #endregion

        #region Internal members

        private Dictionary<Style, Bitmap> iStyleImages = new Dictionary<Style, Bitmap>();
        private Stack<Settings> iSettingsBuffer = new Stack<Settings>();
        private bool iDisposed = false;

        private PointerWidget iWidget;
        private Style iAppearance;

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
            get { return iWidget.Opacity; }
            set { iWidget.Opacity = value; }
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

        // Other

        public bool Visible
        {
            get { return iWidget.Visible; }
        }

        #endregion

        #region Public methods

        public Pointer()
        {
            iStyleImages.Add(Style.Spot, Properties.Resources.pointerSpot);
            iStyleImages.Add(Style.Circle, Properties.Resources.pointerCircle);
            iStyleImages.Add(Style.Ring, Properties.Resources.pointerRing);
            iStyleImages.Add(Style.Cross, Properties.Resources.pointerCross);
            iStyleImages.Add(Style.Dots, Properties.Resources.pointerDots);
            iStyleImages.Add(Style.Rect, Properties.Resources.pointerRect);
            iStyleImages.Add(Style.CorssAni, Properties.Resources.pointerCrossAnim);
            
            iWidget = new PointerWidget();

            Settings settings = Utils.ObjectStorage<Settings>.load();
            settings.saveTo(this);
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

        public void moveTo(Point aLocation)
        {
            if (iWidget.Visible)
            {
                iWidget.Location = new Point(aLocation.X - iWidget.Width / 2, aLocation.Y - iWidget.Height / 2);
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                Utils.ObjectStorage<Settings>.save(settings);
            }

            iDisposed = true;
        }

        #endregion
    }
}
