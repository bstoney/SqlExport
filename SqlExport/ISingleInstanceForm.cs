using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SqlExport
{
    public delegate void WndProc2EventHandler(Message m, ref bool Cancel);

    internal interface ISingleInstanceForm
    {
        event WndProc2EventHandler WndProc2;

        IntPtr Handle { get; }

        void HandleCommandArguments(string[] args);
    }
}
