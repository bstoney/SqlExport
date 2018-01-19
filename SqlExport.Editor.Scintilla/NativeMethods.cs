namespace SqlExport.Editor.Scintilla
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Defines the NativeMethods class.
    /// </summary>
    internal class NativeMethods
    {
        public const int WM_VSCROLL = 0x115;
        public const int WM_MOUSEWHEEL = 0x20A;
        public const int WM_USER = 0x400;
        public const int SB_VERT = 1;
        public const int EM_SETSCROLLPOS = WM_USER + 222;
        public const int EM_GETSCROLLPOS = WM_USER + 221;

        [DllImport("user32.dll")]
        public static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, Int32 wMsg, Int32 wParam, ref Point lParam);
    }
}
