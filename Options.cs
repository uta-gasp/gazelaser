using System;
using System.Windows.Forms;

namespace GazeLaser
{
    public partial class Options : Form
    {
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
            cmbAppearance.SelectedItem = aPointer.Appearance;
            trbOpacity.Value = (int)Math.Round(aPointer.Opacity * 10);
            trbSize.Value = aPointer.Size / 10;
        }

        public void save(Pointer aPointer)
        {
            aPointer.Appearance = (Pointer.Style)cmbAppearance.SelectedItem;
            aPointer.Opacity = (double)trbOpacity.Value / 10;
            aPointer.Size = trbSize.Value * 10;
        }

        private void trbOpacity_ValueChanged(object sender, EventArgs e)
        {
            lblOpacity.Text = (10 * trbOpacity.Value).ToString() + "%";
        }

        private void trbSize_ValueChanged(object sender, EventArgs e)
        {
            lblSize.Text = (10 * trbSize.Value).ToString() + "px";
        }
    }
}
