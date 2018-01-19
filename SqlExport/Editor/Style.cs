namespace SqlExport.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;

    using SqlExport.Common;
    using SqlExport.Common.Editor;
    using SqlExport.Common.Options;

    /// <summary>
    /// Defines the Style class.
    /// </summary>
    internal abstract class Style : IStyleConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Style"/> class.
        /// </summary>
        protected Style()
        {
            Configuration.SetOptionsOn(this);
        }

        #region IStyleConfiguration Members

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        public virtual Font Font { get; set; }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        public virtual Color Colour { get; set; }

        /// <summary>
        /// Gets or sets the back colour.
        /// </summary>
        public virtual Color BackColour { get; set; }

        /// <summary>
        /// Gets or sets the capitalisation.
        /// </summary>
        public virtual Capitalisation Capitalisation { get; set; }

        #endregion

        /// <summary>
        /// Defines the ConstantStyle class.
        /// </summary>
        internal class ConstantStyle : Style
        {
            [Option("WordStyle/Constant/@Color", DisplayName = "Foreground Color", DefaultValue = "Firebrick")]
            private string ForegroundColor
            {
                get { return ColorTranslator.ToHtml(this.Colour); }
                set { this.Colour = ColorTranslator.FromHtml(value); }
            }
        }

        /// <summary>
        /// Defines the VariableStyle class.
        /// </summary>
        internal class VariableStyle : Style
        {
            [Option("WordStyle/Variable/@Color", DisplayName = "Foreground Color", DefaultValue = "Purple")]
            private string ForegroundColor
            {
                get { return ColorTranslator.ToHtml(this.Colour); }
                set { this.Colour = ColorTranslator.FromHtml(value); }
            }
        }
    }
}
