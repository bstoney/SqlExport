namespace SqlExport
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;

    using SqlExport.Common;
    using SqlExport.Common.Editor;
    using SqlExport.Editor;

    /// <summary>
    /// Defines the DefaultOptions class.
    /// </summary>
    public static class DefaultOptions
    {
        /// <summary>
        /// Gets the editor style.
        /// </summary>
        /// <returns>A editor style configuration.</returns>
        public static IEditorStyleConfiguration GetEditorStyle()
        {
            var style = new EditorStyle();

            return style;
        }
    }
}
