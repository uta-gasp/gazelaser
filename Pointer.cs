using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GazeLaser
{
    public class Pointer
    {
        public enum Style
        {
            Spot,
            Circle,
            Ring
        }

        private Dictionary<Style, Bitmap> iStyleImages = new Dictionary<Style, Bitmap>();

        private PointerWidget iWidget;
        private Style iStyle;

        public Style Appearance
        {
            get { return iStyle; }
            set
            {
                iStyle = value;
                if (iStyleImages.ContainsKey(iStyle))
                {
                    iWidget.BackgroundImage = iStyleImages[iStyle];
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

        public Pointer()
        {
            iStyleImages.Add(Style.Spot, Properties.Resources.pointerSpot);
            iStyleImages.Add(Style.Circle, Properties.Resources.pointerCircle);
            iStyleImages.Add(Style.Ring, Properties.Resources.pointerRing);
            
            iWidget = new PointerWidget();
            
            Appearance = Style.Spot;
            Opacity = 0.35;
            Size = 100;
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
    }
}
