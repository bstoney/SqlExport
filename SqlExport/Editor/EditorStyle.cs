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
    /// Defines the EditorStyle class.
    /// </summary>
    internal class EditorStyle : IEditorStyleConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditorStyle"/> class.
        /// </summary>
        public EditorStyle()
        {
            this.Font = SystemFonts.DefaultFont;
            this.LineNumbers = true;
            this.TabSize = 4;
            this.KeywordStyle = new WordStyle.KeywordStyle();
            this.FunctionStyle = new WordStyle.FunctionStyle();
            this.DataTypeStyle = new WordStyle.DataTypeStyle();
            this.OperatorStyle = new WordStyle.OperatorStyle();
            this.ConstantStyle = new Style.ConstantStyle();
            this.VariableStyle = new Style.VariableStyle();
            this.CommentStyle = new CommentStyle();

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
        [EnumOption("Editor/Capitalisation", typeof(Capitalisation), DefaultValue = "Default")]
        public Capitalisation Capitalisation { get; set; }

        #endregion

        #region IEditorStyleConfiguration Members

        /// <summary>
        /// Gets or sets the size of the tab.
        /// </summary>
        /// <value>
        /// The size of the tab.
        /// </value>
        [Option("Editor/IndentSize", DisplayName = "Indent Size", DefaultValue = "4")]
        public int TabSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether enable word wrap.
        /// </summary>
        [Option("Editor/WordWrap", DisplayName = "Word Wrap", DefaultValue = "False")]
        public bool WordWrap { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show line numbers.
        /// </summary>
        [Option("Editor/LienNumbers", DisplayName = "Show Line Numbers", DefaultValue = "False")]
        public bool LineNumbers { get; set; }

        /// <summary>
        /// Gets the keyword style.
        /// </summary>
        public IWordStyleConfiguration KeywordStyle { get; private set; }

        /// <summary>
        /// Gets the function style.
        /// </summary>
        public IWordStyleConfiguration FunctionStyle { get; private set; }

        /// <summary>
        /// Gets the data type style.
        /// </summary>
        public IWordStyleConfiguration DataTypeStyle { get; private set; }

        /// <summary>
        /// Gets the operator style.
        /// </summary>
        public IWordStyleConfiguration OperatorStyle { get; private set; }

        /// <summary>
        /// Gets the constant style.
        /// </summary>
        public IStyleConfiguration ConstantStyle { get; private set; }

        /// <summary>
        /// Gets the variable style.
        /// </summary>
        public IStyleConfiguration VariableStyle { get; private set; }

        /// <summary>
        /// Gets the comment style.
        /// </summary>
        public ICommentStyleConfiguration CommentStyle { get; private set; }

        #endregion

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        /// <value>
        /// The name of the font.
        /// </value>
        [Option("Editor/EditorFont/@Name", DefaultValue = "Courier New")]
        private string FontName
        {
            get { return this.Font.Name; }
            set { this.Font = new Font(value, this.FontSize, this.FontStyle); }
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>
        /// The size of the font.
        /// </value>
        [Option("Editor/EditorFont/@Size", DefaultValue = "8")]
        private float FontSize
        {
            get { return this.Font.Size; }
            set { this.Font = new Font(this.FontName, value, this.FontStyle); }
        }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        [EnumOption("Editor/EditorFont/@Weight", typeof(FontStyle), DefaultValue = "Regular")]
        private FontStyle FontStyle
        {
            get { return this.Font.Style; }
            set { this.Font = new Font(this.FontName, this.FontSize, value); }
        }

        /// <summary>
        /// Gets or sets the color of the fore.
        /// </summary>
        /// <value>
        /// The color of the fore.
        /// </value>
        [Option("Editor/Color", DisplayName = "Foreground Color", DefaultValue = "Black")]
        private string ForegroundColor
        {
            get { return ColorTranslator.ToHtml(this.Colour); }
            set { this.Colour = ColorTranslator.FromHtml(value); }
        }

        /// <summary>
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>
        /// The color of the background.
        /// </value>
        [Option("Editor/BackColor", DisplayName = "Background Color")]
        private string BackgroundColor
        {
            get { return ColorTranslator.ToHtml(this.BackColour); }
            set { this.BackColour = ColorTranslator.FromHtml(value); }
        }
    }
}
