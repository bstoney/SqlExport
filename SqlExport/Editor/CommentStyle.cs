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
    /// Defines the CommentStyle class.
    /// </summary>
    internal class CommentStyle : ICommentStyleConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentStyle"/> class.
        /// </summary>
        public CommentStyle()
        {
            this.CommentSyntax = CommentSyntax.TSql;

            Configuration.SetOptionsOn(this);
        }

        #region IStyleConfiguration Members

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the colour.
        /// </summary>
        public Color Colour { get; set; }

        /// <summary>
        /// Gets or sets the back colour.
        /// </summary>
        public Color BackColour { get; set; }

        /// <summary>
        /// Gets or sets the capitalisation.
        /// </summary>
        public Capitalisation Capitalisation { get; set; }

        #endregion

        #region ICommentStyleConfiguration Members

        /// <summary>
        /// Gets or sets the comment syntax.
        /// </summary>
        public CommentSyntax CommentSyntax { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the color of the foreground.
        /// </summary>
        /// <value>
        /// The color of the foreground.
        /// </value>
        [Option("WordStyle/Comment/@Color", DisplayName = "Foreground Color", DefaultValue = "#008000")]
        private string ForegroundColor
        {
            get { return ColorTranslator.ToHtml(this.Colour); }
            set { this.Colour = ColorTranslator.FromHtml(value); }
        }
    }
}
