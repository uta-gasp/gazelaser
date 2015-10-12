using System;
using System.Windows.Forms;

namespace GazeLaser
{
    public partial class Options : Form
    {
        private Pointer iPointer;

        public Options()
        {
            InitializeComponent();

            foreach (Pointer.Style pointerStyle in Enum.GetValues(typeof(Pointer.Style)))
            {
                cmbAppearance.Items.Add(pointerStyle);
            }
        }

        public void load(Pointer aPointer)
        {
            iPointer = aPointer;
            iPointer.pushSettings();

            cmbAppearance.SelectedItem = aPointer.Appearance;
            trbOpacity.Value = (int)Math.Round(aPointer.Opacity * 10);
            trbSize.Value = aPointer.Size / 10;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            iPointer.popSettings(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            iPointer.popSettings(true);
        }

        private void cmbAppearance_SelectedIndexChanged(object sender, EventArgs e)
        {
            iPointer.Appearance = (Pointer.Style)cmbAppearance.SelectedItem;
        }

        private void trbOpacity_ValueChanged(object sender, EventArgs e)
        {
            lblOpacity.Text = (10 * trbOpacity.Value).ToString() + "%";
            iPointer.Opacity = (double)trbOpacity.Value / 10;
        }

        private void trbSize_ValueChanged(object sender, EventArgs e)
        {
            lblSize.Text = (10 * trbSize.Value).ToString() + " px";
            iPointer.Size = trbSize.Value * 10;
        }
    }
}
