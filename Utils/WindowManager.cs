using System;
using System.Runtime.InteropServices;

namespace GazeLaser.Utils
{
    public static class WindowManager
    {
        public enum Placement : int
        {
            NOTOPMOST = -2,
            TOPMOST = -1,
            TOP = 0,
            BOTTOM = 1
        }

        [Flags]
        public enum Options : int
        {
            NOSIZE = 0x0001,
            NOMOVE = 0x0002,
            NOZORDER = 0x0004,
            NOREDRAW = 0x0008,
            NOACTIVATE = 0x0010,
            FRAMECHANGED = 0x0020,
            DRAWFRAME = 0x0020,
            SHOWWINDOW = 0x0040,
            HIDEWINDOW = 0x0080,
            NOCOPYBITS = 0x0100,
            NOOWNERZORDER = 0x0200,
            NOREPOSITION = 0x0200,
            NOSENDCHANGING = 0x0400,
            DEFERERASE = 0x2000,
            ASYNCWINDOWPOS = 0x4000
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowPos(IntPtr aWnd, Placement aWndInsertAfter, int aX, int aY, int aWidth, int aHeight, Options aFlags);
    }
}
