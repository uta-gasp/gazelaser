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

            Application.Run();

            gazeLaser.Dispose();
        }
    }
}
