using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace GazeLaser.Utils
{
    public class GlobalShortcut
    {
        #region Declarations

        private delegate IntPtr LowLevelKeyboardProc(int aCode, IntPtr aWParam, IntPtr aLParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int aHookID,
            LowLevelKeyboardProc aProc, IntPtr aMod, uint aThreadID);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr aHookID);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr aHookID, int aCode,
            IntPtr aWParam, IntPtr aLParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string aModuleName);

        #endregion

        #region Constants

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        #endregion

        #region Internal members

        private static LowLevelKeyboardProc iProc = HookCallback;
        private static IntPtr iHookID = IntPtr.Zero;
        private static List<Shortcut> iShortcuts = new List<Shortcut>();

        #endregion

        #region Public methods

        public static void init()
        {
            iHookID = SetHook(iProc);
        }

        public static void close()
        {
            UnhookWindowsHookEx(iHookID);
        }

        public static void add(Shortcut aShortcut)
        {
            lock (iShortcuts)
            {
                iShortcuts.Add(aShortcut);
            }
        }

        public static void clear(string aGroup = "")
        {
            lock (iShortcuts)
            {
                if (String.IsNullOrEmpty(aGroup))
                {
                    iShortcuts.Clear();
                }
                else
                {
                    List<Shortcut> shortcuts = new List<Shortcut>();
                    foreach (Shortcut shortcut in iShortcuts)
                    {
                        if (shortcut.Group != aGroup)
                        {
                            shortcuts.Add(shortcut);
                        }
                    }
                    iShortcuts = shortcuts;
                }
            }
        }

        #endregion

        #region Internal methods

        private GlobalShortcut() { }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int aCode, IntPtr aWParam, IntPtr aLParam)
        {
            if (aCode >= 0 && aWParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(aLParam);
                lock (iShortcuts)
                {
                    foreach (Shortcut shortcut in iShortcuts)
                    {
                        if (shortcut.isPressed(vkCode))
                        {
                            shortcut.Callback();
                        }
                    }
                }
            }
            return CallNextHookEx(iHookID, aCode, aWParam, aLParam);
        }

        #endregion
    }
}