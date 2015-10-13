using System;
using System.Windows.Forms;

namespace GazeLaser
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GazeLaser gazeLaser = new GazeLaser();

            if (gazeLaser.AutoStarter.Enabled)
            {
                gazeLaser.AutoStarter.run(gazeLaser);
            }

            Application.Run();

            gazeLaser.Dispose();
        }
    }
}
