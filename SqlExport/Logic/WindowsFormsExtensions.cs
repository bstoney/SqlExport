namespace SqlExport.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Monads;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Forms;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using PixelFormat = System.Drawing.Imaging.PixelFormat;

    /// <summary>
    /// Defines the WindowsFormsExtensions class.
    /// </summary>
    public static class WindowsFormsExtensions
    {
        /// <summary>
        /// Converts the WPF context menu to a <see cref="ContextMenuStrip"/>.
        /// </summary>
        /// <param name="wpfContextMenu">The WPF context menu.</param>
        /// <returns>
        /// A new <see cref="ContextMenuStrip"/>.
        /// </returns>
        public static ContextMenuStrip ToForms(this System.Windows.Controls.ContextMenu wpfContextMenu)
        {
            var contextMenuStrip = new ContextMenuStrip();

            foreach (var item in wpfContextMenu.Items)
            {
                (item as System.Windows.Controls.MenuItem).With(i => i.ToForms()).Do(i => contextMenuStrip.Items.Add(i));
            }

            return contextMenuStrip;
        }

        /// <summary>
        /// Converts the menu item to a <see cref="ToolStripItem"/>.
        /// </summary>
        /// <param name="menuItem">The menu item.</param>
        /// <returns>
        /// A new <see cref="ToolStripItem"/>.
        /// </returns>
        public static ToolStripItem ToForms(this System.Windows.Controls.MenuItem menuItem)
        {
            var text = menuItem.Header.With(h => h.ToString());
            var icon = (menuItem.Icon as System.Windows.Controls.Image).ToForms();
            var command = menuItem.Command;
            var parameter = menuItem.CommandParameter;
            var toolStripItem = new ToolStripMenuItem(text, icon);
            command.Do(i => toolStripItem.Click += (s, e) => command.Execute(parameter));

            foreach (var item in menuItem.Items)
            {
                (item as System.Windows.Controls.MenuItem).With(i => i.ToForms())
                                                          .Do(i => toolStripItem.DropDownItems.Add(i));
            }

            return toolStripItem;
        }

        /// <summary>
        /// Converts the image to a <see cref="Image"/>.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>
        /// A new <see cref="Image"/>.
        /// </returns>
        public static Image ToForms(this System.Windows.Controls.Image image)
        {
            return image.With(i => i.Source as BitmapSource).ToStream().ToImage();
        }
    }
}
