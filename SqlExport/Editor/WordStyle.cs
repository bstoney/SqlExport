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
    /// Defines the WordStyle class.
    /// </summary>
    internal abstract class WordStyle : IWordStyleConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WordStyle"/> class.
        /// </summary>
        protected WordStyle()
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

        #region IWordStyleConfiguration Members

        /// <summary>
        /// Gets or sets the words.
        /// </summary>
        public virtual string[] Words { get; set; }

        #endregion

        /// <summary>
        /// Defines the KeywordStyle class.
        /// </summary>
        internal class KeywordStyle : WordStyle
        {
            [Option("WordStyle/Keyword/@Color", DisplayName = "Foreground Color", DefaultValue = "Blue")]
            private string ForegroundColor
            {
                get { return ColorTranslator.ToHtml(this.Colour); }
                set { this.Colour = ColorTranslator.FromHtml(value); }
            }
        }

        /// <summary>
        /// Defines the FunctionStyle class.
        /// </summary>
        internal class FunctionStyle : WordStyle
        {
            [Option("WordStyle/Function/@Color", DisplayName = "Foreground Color", DefaultValue = "DarkRed")]
            private string ForegroundColor
            {
                get { return ColorTranslator.ToHtml(this.Colour); }
                set { this.Colour = ColorTranslator.FromHtml(value); }
            }
        }

        /// <summary>
        /// Defines the DataTypeStyle class.
        /// </summary>
        internal class DataTypeStyle : WordStyle
        {
            [Option("WordStyle/DataType/@Color", DisplayName = "Foreground Color", DefaultValue = "DarkGreen")]
            private string ForegroundColor
            {
                get { return ColorTranslator.ToHtml(this.Colour); }
                set { this.Colour = ColorTranslator.FromHtml(value); }
            }
        }

        /// <summary>
        /// Defines the OperatorStyle class.
        /// </summary>
        internal class OperatorStyle : WordStyle
        {
            [Option("WordStyle/Operator/@Color", DisplayName = "Foreground Color", DefaultValue = "Gray")]
            private string ForegroundColor
            {
                get { return ColorTranslator.ToHtml(this.Colour); }
                set { this.Colour = ColorTranslator.FromHtml(value); }
            }
        }
    }
}
