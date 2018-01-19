using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace SqlExport
{
	internal static class ApiHelper
	{
		private static OwnerWindow _owner = new OwnerWindow();

		#region Get Special Folders

		// The Desktop - virtual folder
		private const short CSIDL_DESKTOP = 0x0;
		// Program Files
		private const short CSIDL_PROGRAMS = 2;
		// Control Panel - virtual folder
		private const short CSIDL_CONTROLS = 3;
		// Printers - virtual folder
		private const short CSIDL_PRINTERS = 4;
		// My Documents
		private const short CSIDL_DOCUMENTS = 5;
		// Favorites
		private const short CSIDL_FAVORITES = 6;
		// Startup Folder
		private const short CSIDL_STARTUP = 7;
		// Recent Documents
		private const short CSIDL_RECENT = 8;
		// Send To Folder
		private const short CSIDL_SENDTO = 9;
		// Recycle Bin - virtual folder
		private const short CSIDL_BITBUCKET = 10;
		// Start Menu
		private const short CSIDL_STARTMENU = 11;
		// Desktop folder
		private const short CSIDL_DESKTOPFOLDER = 16;
		// My Computer - virtual folder
		private const short CSIDL_DRIVES = 17;
		// Network Neighbourhood - virtual folder
		private const short CSIDL_NETWORK = 18;
		// NetHood Folder
		private const short CSIDL_NETHOOD = 19;
		// Fonts folder
		private const short CSIDL_FONTS = 20;
		// ShellNew folder
		private const short CSIDL_SHELLNEW = 21;

		private const short MAX_PATH = 260;

		private struct SH_ITEMID
		{
			public int cb;
			public byte abID;
		}

		private struct ItemIdList
		{
			public SH_ITEMID mkid;
		}

		[DllImport( "Shell32.dll", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true )]
		private static extern int SHGetSpecialFolderLocation( int hwndOwner, int nFolder, ref ItemIdList pidl );

		[DllImport( "Shell32.dll", EntryPoint = "SHGetPathFromIDListA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true )]
		private static extern int SHGetPathFromIDList( int pidl, string pszPath );

		public static string GetMyDocumentsFolder()
		{
			return GetSpecialFolder( CSIDL_DOCUMENTS );
		}

		/// <summary>
		/// Retrieve encodingInfo about system folders such as the "Desktop" folder. Info is stored in the IDL structure.
		/// </summary>
		private static string GetSpecialFolder( int specialFolderId )
		{
			string folderPath = String.Empty;
			ItemIdList idList = new ItemIdList();
			if( SHGetSpecialFolderLocation( _owner.Handle.ToInt32(), specialFolderId, ref idList ) == 0 )
			{
				// Get the path from the ID list, and return the folder.
				string path = new string( ' ', MAX_PATH );
				if( SHGetPathFromIDList( idList.mkid.cb, path ) == 0 )
				{
					folderPath = path.Substring( 0, path.IndexOf( '\0' ) - 1 ) + "\\";
				}
			}
			return folderPath;
		}

		#endregion

		#region Other

		// API Stuff
		[DllImport( "user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true )]
		public static extern int LockWindowUpdate( int hwndLock );
		[DllImport( "user32", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true )]
		public static extern int SendMessage( int hwnd, int wMsg, int wParam, ref IntPtr lParam );

		public const short EM_GETLINECOUNT = 0xBA;
		public const short EM_GETLINE = 0xC4;
		public const short EM_FMTLINES = 0xC8;
		public const short EM_LINELENGTH = 0xC1;
		public const short EC_LEFTMARGIN = 0x1;
		public const short EC_RIGHTMARGIN = 0x2;
		public const short EC_USEFONTINFO = 0x7FFF;
		public const short EM_SETMARGINS = 0xD3;
		public const short EM_GETMARGINS = 0xD4;
		public const short EM_CANUNDO = 0xC6;
		public const short EM_EMPTYUNDOBUFFER = 0xCD;
		public const short EM_GETFIRSTVISIBLELINE = 0xCE;
		public const short EM_GETHANDLE = 0xBD;
		public const short EM_GETMODIFY = 0xB8;
		public const short EM_GETPASSWORDCHAR = 0xD2;
		public const short EM_GETRECT = 0xB2;
		public const short EM_GETSEL = 0xB0;
		public const short EM_GETTHUMB = 0xBE;
		public const short EM_GETWORDBREAKPROC = 0xD1;
		public const short EM_LIMITTEXT = 0xC5;
		public const short EM_LINEFROMCHAR = 0xC9;
		public const short EM_LINEINDEX = 0xBB;

		public const short EM_LINESCROLL = 0xB6;
		public const short EM_REPLACESEL = 0xC2;
		public const short EM_SCROLL = 0xB5;
		public const short EM_SCROLLCARET = 0xB7;
		public const short EM_SETHANDLE = 0xBC;
		public const short EM_SETMODIFY = 0xB9;
		public const short EM_SETPASSWORDCHAR = 0xCC;
		public const short EM_SETREADONLY = 0xCF;
		public const short EM_SETRECT = 0xB3;
		public const short EM_SETRECTNP = 0xB4;
		public const short EM_SETSEL = 0xB1;
		public const short EM_SETTABSTOPS = 0xCB;
		public const short EM_SETWORDBREAKPROC = 0xD0;
		public const short EM_UNDO = 0xC7;

		#endregion

		private class OwnerWindow : NativeWindow
		{
		}
	}
}
