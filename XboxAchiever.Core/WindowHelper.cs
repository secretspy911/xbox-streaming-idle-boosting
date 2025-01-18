using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Drawing;

namespace XboxAchiever.Core
{
    // https://stackoverflow.com/questions/604410/global-keyboard-capture-in-c-sharp-application
    public static class WindowHelper
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern IntPtr ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowA(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("gdi32.dll")]
        private static extern uint GetPixel(IntPtr dc, int x, int y);
        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr window, IntPtr dc);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        private const int HWND_TOPMOST = -1;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;

        enum ShowWindowCmd
        {
            Hide = 0,
            ShowNormal = 1,
            ShowMinimize = 2,
            ShowMaximize = 3, // Or ActivatedMaximized
            ShowNoActivate = 4,
            Show = 5,
            Minimize = 6,
            ShowMinNoActive = 7,
            ShowNA = 8,
            Restore = 9,
            ShowDefault = 10,
            ForceMinimize = 11
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static IntPtr hookID = IntPtr.Zero;

        private static LowLevelKeyboardProc proc = HookCallback;
        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate void HandleKeyPress(Keys key);
        private static HandleKeyPress handleKeyPress;

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                handleKeyPress((Keys)vkCode);
            }

            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        public static IntPtr SetHook(HandleKeyPress handleKeyPress)
        {
            WindowHelper.handleKeyPress = handleKeyPress;
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        public static void UnhookWindowsHookEx()
        {
            UnhookWindowsHookEx(hookID);
        }

        public static IntPtr FocusWindow(string windowTitle, ref string errorMessage)
        {
            IntPtr handle = FindWindowA(null, windowTitle);
            if (handle.ToInt32() == 0)
            {
                errorMessage = "Cannot find window with '" + windowTitle + "' as the title.";
                return IntPtr.Zero;
            }
            else
            {
                SetForegroundWindow(handle);
                return handle;
            }
        }

        public static Color GetColorAt(IntPtr hwnd, int x, int y)
        {
            IntPtr dc = GetDC(hwnd);
            uint pixel = GetPixel(dc, x, y);
            ReleaseDC(hwnd, dc);
            return Color.FromArgb((int)(pixel & 0x000000FF), (int)(pixel & 0x0000FF00) >> 8, (int)(pixel & 0x00FF0000) >> 16);
        }

        public static void SetTopMost()
        {
            IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;
            SetWindowPos(hWnd, new IntPtr(HWND_TOPMOST), 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }
    }
}
