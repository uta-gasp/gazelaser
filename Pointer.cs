using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GazeLaser
{
    public partial class Pointer : Form
    {
        private int SIZE = 100;

        public enum Style
        {
            Spot
        }

        private Style iStyle;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return createParams;
            }
        }

        public Style Appearance
        {
            get { return iStyle; }
            set
            {
                iStyle = value;
                switch (iStyle)
                {
                    case Pointer.Style.Spot:
                        this.BackgroundImage = Properties.Resources.pointerSpot;
                        break;
                }
            }
        }

        public Pointer()
        {
            InitializeComponent();

            this.BackColor = Color.White;
            this.TransparencyKey = Color.White; 
            
            Appearance = Style.Spot;
            Opacity = 0.35;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            Width = SIZE;
            Height = SIZE;
        }
    }
}
