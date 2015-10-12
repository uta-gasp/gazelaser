using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GazeLaser
{
    public class Pointer
    {
        public enum Style
        {
            Spot,
            Circle
        }

        private PointerWidget iWidget;
        private Style iStyle;

        public Style Appearance
        {
            get { return iStyle; }
            set
            {
                iStyle = value;
                switch (iStyle)
                {
                    case Style.Spot:
                        iWidget.BackgroundImage = Properties.Resources.pointerSpot;
                        break;
                    case Style.Circle:
                        iWidget.BackgroundImage = Properties.Resources.pointerCircle;
                        break;
                    default:
                        throw new NotSupportedException("Pointer.Appearance");
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
