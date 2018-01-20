#define Win32

namespace SqlExport
{
    using System;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Windows.Forms;

    using Microsoft.VisualBasic;

    /// <summary>
    /// Defines the SingleInstanceHelper class.
    /// </summary>
    internal static class SingleInstanceHelper
    {
        #region API

        private const int WM_COPYDATA = 0x4A;

        [StructLayout(LayoutKind.Sequential)]
        private struct CopyDataStructure
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        [DllImport("user32", EntryPoint = "GetPropA", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetProp(IntPtr hWnd, string lpString);

        [DllImport("user32", EntryPoint = "SetPropA", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SetProp(IntPtr hWnd, string lpString, int hData);

        private delegate int EnumWindowsProc(IntPtr hWnd, int lParam);

        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int EnumWindows(EnumWindowsProc lpEnumFunc, int lParam);

        [DllImport("user32", EntryPoint = "SendMessageA", ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        #endregion

        #region EnumWindows

        private static EnumWindowsProc ewp = new EnumWindowsProc(EWP);

        private static int EWP(IntPtr hWnd, int lParam)
        {
            // Customised windows enumeration procedure.  Stops
            // when it finds another application with the Window
            // property set, or when all windows are exhausted.
            try
            {
                if (IsThisApp(hWnd))
                {
                    hwnd = hWnd;
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            catch
            {
                return 0;
            }
        }

        private static bool IsThisApp(IntPtr hWnd)
        {
            // Check if the windows property is set for this window handle
            if (GetProp(hWnd, thisAppID + "_APPLICATION") == 1)
            {
                return true;
            }

            return false;
        }

        private static bool FindWindow()
        {
            if (hwnd == IntPtr.Zero)
            {
                EnumWindows(ewp, 0);
                if (hwnd == IntPtr.Zero)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        private static bool SendMessageToWindow(IntPtr destHandle, IntPtr srcHandle, object message)
        {
            ////Debugger.Break();

            string strCmd = null;
            try
            {
                strCmd = SerialHelper.SerializeToBase64String(message);
            }
            catch (Exception)
            {
                return false;
            }

            CopyDataStructure cds;

            cds.dwData = srcHandle;
            strCmd += '\0';

            cds.cbData = strCmd.Length + 1;
            cds.lpData = Marshal.AllocCoTaskMem(strCmd.Length);
            cds.lpData = Marshal.StringToCoTaskMemAnsi(strCmd);
            IntPtr iPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(cds));

            try
            {
                Marshal.StructureToPtr(cds, iPtr, true);

                // send to the MFC app
                SendMessage(destHandle, WM_COPYDATA, IntPtr.Zero, iPtr);

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                // Don't forget to free the allocated memory 
                Marshal.FreeCoTaskMem(cds.lpData);
                Marshal.FreeCoTaskMem(iPtr);
            }
        }

        #endregion

        private static IntPtr hwnd = IntPtr.Zero;
        private static string thisAppID;
        private static System.Threading.Mutex mutex;
        private static bool mutexOwned = false;
        private static ISingleInstanceForm mainForm;

        static SingleInstanceHelper()
        {
            ////Debugger.Break();

            thisAppID = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            mutex = new System.Threading.Mutex(true, String.Concat(thisAppID, "_APPLICATION_MUTEX"), out mutexOwned);
            if (!mutexOwned)
            {
                if (!FindWindow())
                {
                    mutexOwned = true;
                }
            }

            AppDomain.CurrentDomain.ProcessExit += new System.EventHandler(OnExit);
        }

        private static void OnExit(object sender, EventArgs e)
        {
            try
            {
                if (mutex != null)
                {
                    // Delete
                    ////oMutex.ReleaseMutex();

                    ((IDisposable)mutex).Dispose();
                    mutex = null;
                }
            }
            catch
            {
                // Do Nothing
            }
        }

        public static bool IsFirstInstance
        {
            get { return mutexOwned; }
        }

        public static bool NotifyPreviousWindow(IntPtr hWnd, object message)
        {
            return SendMessageToWindow(hwnd, hWnd, message);
        }

        public static void SetMainForm(ISingleInstanceForm frm)
        {
            mainForm = frm;

            try
            {
                SetProp(frm.Handle, thisAppID + "_APPLICATION", 1);
                mainForm.WndProc2 += new WndProc2EventHandler(MainForm_WndProc);
            }
            catch
            {
                mainForm = null;
            }
        }

        private static void MainForm_WndProc(Message m, ref bool Cancel)
        {
            switch (m.Msg)
            {
                case WM_COPYDATA:
                    try
                    {
                        string[] args = new string[] { };

                        try
                        {
                            CopyDataStructure cds = new CopyDataStructure();
                            cds = (CopyDataStructure)Marshal.PtrToStructure(m.LParam, typeof(CopyDataStructure));
                            if (cds.cbData > 0)
                            {
                                byte[] data = new byte[cds.cbData];
                                Marshal.Copy(cds.lpData, data, 0, cds.cbData);
                                Encoding unicodeStr = Encoding.ASCII;
                                char[] myString = unicodeStr.GetChars(data);
                                string returnText = new string(myString);
                                args = (string[])SerialHelper.DeserializeFromBase64String(returnText);
                            }
                        }
                        catch (Exception)
                        {
                        }

                        mainForm.HandleCommandArguments(args);

                        Cancel = true;

                    }
                    catch
                    {
                        Cancel = false;
                    }

                    break;
                default:
                    Cancel = false;
                    break;
            }
        }
    }
}
