using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows;

namespace SqlExport.Business
{
	public static class WindowExtensions
	{
		[DllImport( "user32.dll" )]
		static extern int GetWindowLong( IntPtr hwnd, int index );

		[DllImport( "user32.dll" )]
		static extern int SetWindowLong( IntPtr hwnd, int index, int newStyle );

		[DllImport( "user32.dll" )]
		static extern bool SetWindowPos( IntPtr hwnd, IntPtr hwndInsertAfter,
				   int x, int y, int width, int height, uint flags );

		[DllImport( "user32.dll" )]
		static extern IntPtr SendMessage( IntPtr hwnd, uint msg,
				   IntPtr wParam, IntPtr lParam );

		[DllImport( "user32.dll", EntryPoint = "GetSystemMenu" )]
		private static extern IntPtr GetSystemMenu( IntPtr hwnd, int revert );

		[DllImport( "user32.dll", EntryPoint = "GetMenuItemCount" )]
		private static extern int GetMenuItemCount( IntPtr hmenu );

		[DllImport( "user32.dll", EntryPoint = "RemoveMenu" )]
		private static extern int RemoveMenu( IntPtr hmenu, int npos, int wflags );

		[DllImport( "user32.dll", EntryPoint = "DrawMenuBar" )]
		private static extern int DrawMenuBar( IntPtr hwnd );

		[DllImport( "user32.dll" )]
		[return: MarshalAs( UnmanagedType.Bool )]
		public static extern bool GetCursorPos( out POINT lpPoint );

		[StructLayout( LayoutKind.Sequential )]
		public struct POINT
		{
			public int X;
			public int Y;

			public POINT( int x, int y )
			{
				this.X = x;
				this.Y = y;
			}
		}

		const int GWL_EXSTYLE = -20;
		const int WS_EX_DLGMODALFRAME = 0x0001;
		const int SWP_NOSIZE = 0x0001;
		const int SWP_NOMOVE = 0x0002;
		const int SWP_NOZORDER = 0x0004;
		const int SWP_FRAMECHANGED = 0x0020;
		const uint WM_SETICON = 0x0080;

		private const int MF_BYPOSITION = 0x0400;
		private const int MF_DISABLED = 0x0002;

		public static void RemoveIcon( this Window window )
		{
			// Get this window's handle
			IntPtr hwnd = new WindowInteropHelper( window ).Handle;

			// Change the extended window style to not show a window icon
			int extendedStyle = GetWindowLong( hwnd, GWL_EXSTYLE );
			SetWindowLong( hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME );

			// Update the window's non-client area to reflect the changes
			SetWindowPos( hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE |
				  SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED );
		}

		public static void RemoveControlBox( this Window window )
		{
			// Get this window's handle
			IntPtr hwnd = new WindowInteropHelper( window ).Handle;

			IntPtr hmenu = GetSystemMenu( hwnd, 0 );
			int cnt = GetMenuItemCount( hmenu );
			//remove the button
			RemoveMenu( hmenu, cnt - 1, MF_DISABLED | MF_BYPOSITION );
			//remove the extra menu line
			RemoveMenu( hmenu, cnt - 2, MF_DISABLED | MF_BYPOSITION );
			DrawMenuBar( hwnd ); // Redraw the menu bar
		}
	}
}
