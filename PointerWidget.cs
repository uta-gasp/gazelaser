using System;
using System.Drawing;
using System.Windows.Forms;

namespace GazeLaser
{
    public partial class PointerWidget : Form
    {
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return createParams;
            }
        }

        public PointerWidget()
        {
            InitializeComponent();

            this.BackColor = Color.White;
            this.TransparencyKey = Color.White; 
        }

        public void setImage(Bitmap aBitmap)
        {
            pcbImage.Image = aBitmap;
        }
    }
}
