using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SqlExport.Ui
{
    internal interface IContextMenuProvider
    {
        string MenuTitle { get; }
        bool HasContextMenu { get; }
        ContextMenuStrip GetContextMenu();
    }
}
