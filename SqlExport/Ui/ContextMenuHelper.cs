using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;

namespace SqlExport.Ui
{
    public static class ContextMenuHelper
    {
        private static readonly object MenuTag = new object();

        public static void AddHierachicalContextMenu(Control control, ContextMenuStrip menu)
        {
            if (control != null)
            {
                BuildHierachicalContextMenu(control.Parent, menu);
            }
        }

        private static void BuildHierachicalContextMenu(Control control, ContextMenuStrip menu)
        {
            if (control != null)
            {
                BuildSubMenu(control, ref menu);
                ////if (control is QueryPane)
                ////{
                ////    BuildHierachicalContextMenu(control.Tag as FrameworkElement, menu);
                ////}
                ////else
                ////{
                    BuildHierachicalContextMenu(control.Parent, menu);
                ////}
            }
        }

        private static void BuildHierachicalContextMenu(FrameworkElement control, ContextMenuStrip menu)
        {
            if (control != null)
            {
                if (control is ScrollableTabControl)
                {
                    BuildHierachicalContextMenu(control.Tag as Control, menu);
                }
                else
                {
                    BuildHierachicalContextMenu(control.Parent as FrameworkElement, menu);
                }
            }
        }

        private static void BuildSubMenu(object control, ref ContextMenuStrip menu)
        {
            IContextMenuProvider cmp = control as IContextMenuProvider;
            if (cmp != null)
            {
                if (menu == null)
                {
                    throw new ArgumentNullException("menu");
                }

                ContextMenuStrip cms = cmp.GetContextMenu();

                ToolStripSeparator tss = new ToolStripSeparator();
                tss.Tag = MenuTag;
                menu.Items.Add(tss);

                ToolStripMenuItem tsmi = new ToolStripMenuItem(cmp.MenuTitle);
                tsmi.Tag = MenuTag;
                tsmi.DropDown = cms;
                menu.Items.Add(tsmi);

                // Attach handler to remove the sub menu when it is closed.
                menu.Closed += new ToolStripDropDownClosedEventHandler(OnContextMenuClosed);

                menu = cms;
            }
        }

        private static void OnContextMenuClosed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            ContextMenuStrip cms = sender as ContextMenuStrip;
            if (cms != null)
            {
                List<ToolStripItem> items = GetContextMenuItemsToRemove(cms);
                foreach (ToolStripItem item in items)
                {
                    item.Owner.Items.Remove(item);
                    item.Dispose();
                }
                cms.Closed -= new ToolStripDropDownClosedEventHandler(OnContextMenuClosed);
            }
        }


        private static List<ToolStripItem> GetContextMenuItemsToRemove(ContextMenuStrip cms)
        {
            List<ToolStripItem> items = new List<ToolStripItem>();
            foreach (ToolStripItem item in cms.Items)
            {
                if (item.Tag == MenuTag)
                {
                    items.Add(item);
                    ToolStripMenuItem tsmi = item as ToolStripMenuItem;
                    if (tsmi != null)
                    {
                        ContextMenuStrip childMenu = tsmi.DropDown as ContextMenuStrip;
                        if (childMenu != null)
                        {
                            items.AddRange(GetContextMenuItemsToRemove(childMenu));
                            childMenu.Hide();
                        }
                    }
                }
            }

            return items;
        }
    }
}
