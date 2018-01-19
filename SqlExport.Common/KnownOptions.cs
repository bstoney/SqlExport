namespace SqlExport.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Defines the KnownOptions type.
    /// </summary>
    internal static class KnownOptions
    {
        public const string EditorFontName = "EditorFont/@Name";
        public const string EditorFontSize = "EditorFont/@Size";
        public const string EditorFontStyle = "EditorFont/@Weight";

        public const string WindowStateWindowSizeX = "WindowState/WindowSize/@X";
        public const string WindowStateWindowSizeY = "WindowState/WindowSize/@Y";
        public const string WindowStateWindowSizeWidth = "WindowState/WindowSize/@Width";
        public const string WindowStateWindowSizeHeight = "WindowState/WindowSize/@Height";
        public const string WindowStateFormState = "WindowState/FormState";
    }
}
