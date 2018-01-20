namespace SqlExport.Editor.Scintilla
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Windows.Forms;

    using ScintillaNet;
    using ScintillaNet.Configuration;

    using SqlExport.Common.Editor;
    using SqlExport.Common.Extensions;
    using SqlExport.Common.Options;

    /// <summary>
    /// Defines the ScintillaTextBox class.
    /// </summary>
    public class ScintillaTextBox : IEditorControl, IExtension
    {
        /// <summary>
        /// The brace characters
        /// </summary>
        private static readonly char[] BraceCharacters = new char[] { '(', ')', '[', ']' };

        /// <summary>
        /// The control
        /// </summary>
        private readonly Scintilla control;

        /// <summary>
        /// The key handlers
        /// </summary>
        private readonly Dictionary<Keys, KeyEventHandler> keyHandlers;

        /// <summary>
        /// The style case
        /// </summary>
        private readonly Dictionary<int, Capitalisation> styleCase;

        /// <summary>
        /// The syntax config.
        /// </summary>
        private Configuration syntaxConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScintillaTextBox"/> class.
        /// </summary>
        public ScintillaTextBox()
        {
            this.control = new Scintilla();

            this.control.Styles.ClearAll();

            this.control.TextDeleted += this.OnTextChanged;
            this.control.TextInserted += this.OnTextChanged;
            this.control.MouseUp += this.OnMouseUp;
            this.control.KeyDown += this.OnKeyDown;
            this.control.KeyUp += this.OnKeyUp;
            this.control.DoubleClick += this.OnDoubleClick;
            this.control.MouseWheel += (sender, args) => this.OnCaretChanged();
            this.control.Scroll += (sender, args) => this.OnCaretChanged();
            this.keyHandlers = new Dictionary<Keys, KeyEventHandler>();
            this.styleCase = new Dictionary<int, Capitalisation>();

            Common.Configuration.SetOptionsOn(this);
        }

        /// <summary>
        /// Occurs when the text is changed by the user.
        /// </summary>
        public event EventHandler TextChanged;

        /// <summary>
        /// Occurs when the user moves the text caret.
        /// </summary>
        public event EventHandler CaretChanged;

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name
        {
            get { return "ScintillaTextBox"; }
        }

        /// <summary>
        /// Gets or sets the index of the selection start.
        /// </summary>
        public CaretDetails Caret
        {
            get
            {
                return this.GetCaretFrom(this.control.Selection.Start, this.control.Selection.End);
            }

            set
            {
                this.control.Caret.Position = value.Location;
                if (value.Location == value.Start)
                {
                    this.control.Caret.Anchor = value.Location + value.Length;
                }
                else
                {
                    this.control.Caret.Anchor = value.Location - value.Length;
                }

                this.control.Scrolling.ScrollBy(value.HorizontalScroll, value.VerticalScroll);
                this.UpdateBraceHighlighting();
            }
        }

        /// <summary>
        /// Gets editor control.
        /// </summary>
        public Control Control
        {
            get { return this.control; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the editor can undo the last action.
        /// </summary>
        public bool CanUndo
        {
            get { return this.control.UndoRedo.CanUndo; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the editor can redo the last action.
        /// </summary>
        public bool CanRedo
        {
            get { return this.control.UndoRedo.CanRedo; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the editor can inset the current clipboard contents.
        /// </summary>
        public bool CanPaste
        {
            get { return this.control.Clipboard.CanPaste; }
        }

        /// <summary>
        /// Gets or sets the whitespace mode.
        /// </summary>
        [EnumOption("WhitespaceMode/@Mode", typeof(WhitespaceMode), DisplayName = "Mode", DefaultValue = "Invisible")]
        private WhitespaceMode WhitespaceMode
        {
            get { return this.control.Whitespace.Mode; }
            set { this.control.Whitespace.Mode = value; }
        }

        /// <summary>
        /// Gets or sets the whitespace fore color.
        /// </summary>
        /// <value>
        /// The the whitespace fore color.
        /// </value>
        [Option("WhitespaceMode/@Color", DisplayName = "Color", DefaultValue = "LightGray")]
        private string WhitespaceForeColor
        {
            get
            {
                return ColorTranslator.ToHtml(this.control.Whitespace.ForeColor);
            }

            set
            {
                var color = ColorTranslator.FromHtml(value);
                this.control.Whitespace.ForeColor = color;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether indentation uses tabs.
        /// </summary>
        [Option("Indentation/@UseTabs", DisplayName = "Use Tabs", DefaultValue = "False")]
        private bool IndentationUseTabs
        {
            get { return this.control.Indentation.UseTabs; }
            set { this.control.Indentation.UseTabs = value; }
        }

        /// <summary>
        /// Adds key mapping to a handler.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        public void AddKeyDownHandler(System.Windows.Forms.Keys key, KeyEventHandler handler)
        {
            if (this.keyHandlers.ContainsKey(key))
            {
                this.keyHandlers[key] = (KeyEventHandler)Delegate.Combine(this.keyHandlers[key], handler);
            }
            else
            {
                this.keyHandlers[key] = handler;
            }

            this.control.Commands.RemoveBinding(key);
        }

        /// <summary>
        /// Sets the editors style configuration.
        /// </summary>
        /// <param name="editorStyle">The editor style.</param>
        public void SetEditorStyle(IEditorStyleConfiguration editorStyle)
        {
            this.control.Styles.ClearAll();

            this.styleCase.Clear();

            switch (editorStyle.CommentStyle.CommentSyntax)
            {
                case CommentSyntax.CSharp:
                    this.SetCppStyle(editorStyle);
                    break;
                case CommentSyntax.TSql:
                    this.SetMsSqlStyle(editorStyle);
                    break;
                case CommentSyntax.None:
                    this.SetPlainStyle(editorStyle);
                    break;
            }

            this.syntaxConfig.Markers_List.Add(new MarkersConfig
            {
                Alpha = 200,
                ForeColor = Color.Indigo,
                BackColor = Color.Purple,
                Number = this.control.FindReplace.Marker.Number,
                Name = "Selection",
                Symbol = MarkerSymbol.ShortArrow
            });

            // Run configure twice as sometimes the colours don't stick.
            this.control.ConfigurationManager.Configure(this.syntaxConfig);
            this.control.ConfigurationManager.Configure(this.syntaxConfig);

            this.control.Styles.LineNumber.Font = editorStyle.Font;

            this.control.IsBraceMatching = true;
            this.control.Styles.BraceLight.Bold = true;
            this.control.Styles.BraceLight.BackColor = Color.FromArgb(200, 200, 200);
            this.control.Styles.BraceLight.ForeColor = Color.FromArgb(0, 0, 255);

            this.control.Indentation.IndentWidth = editorStyle.TabSize;
            this.control.Indentation.TabWidth = editorStyle.TabSize;
            this.control.Indentation.BackspaceUnindents = true;

            this.control.LineWrap.Mode = editorStyle.WordWrap ? WrapMode.Word : WrapMode.None;
            if (editorStyle.LineNumbers)
            {
                this.control.Margins.Margin0.Width = 27;
            }
            else
            {
                this.control.Margins.Margin0.Width = 0;
            }

            this.control.NativeInterface.StartStyling(0, this.control.Text.Length);
        }

        public void Undo()
        {
            this.control.UndoRedo.Undo();
        }

        public void Redo()
        {
            this.control.UndoRedo.Redo();
        }

        public void Copy()
        {
            this.control.Clipboard.Copy();
        }

        public void Cut()
        {
            this.control.Clipboard.Cut();
        }

        public void Paste()
        {
            this.control.Clipboard.Paste();
        }

        public void SelectAll()
        {
            this.control.Selection.SelectAll();
        }

        public void SetSelectedText(string text)
        {
            this.control.Selection.Text = text;
        }

        public string GetSelectedText(bool styled)
        {
            string text = this.control.Selection.Text;
            if (styled)
            {
                text = this.StyleText(text, this.control.Selection.Start);
            }

            return text;
        }

        public void SetText(string text)
        {
            this.control.Text = text;
        }

        public string GetText(bool styled)
        {
            string text = this.control.Text;
            if (styled)
            {
                text = this.StyleText(text, 0);
            }

            return text;
        }

        public int GetLineFromPosition(int position)
        {
            return this.control.Lines.FromPosition(position).Number;
        }

        /// <summary>
        /// Selects the lines.
        /// </summary>
        /// <param name="startLine">The start line.</param>
        /// <param name="endLine">The end line.</param>
        public void SelectLines(int startLine, int endLine)
        {
            startLine = Math.Min(Math.Max(startLine, 0), this.control.Lines.Count - 1);
            endLine = Math.Min(Math.Max(endLine, startLine), this.control.Lines.Count - 1);

            var startPosition = this.control.Lines[startLine].StartPosition;
            var endPosition = this.control.Lines[endLine].EndPosition;
            var newCaret = this.GetCaretFrom(startPosition, endPosition);

            if (this.Caret != newCaret)
            {
                this.Caret = newCaret;
                this.control.Scrolling.ScrollToCaret();

                this.OnCaretChanged();
            }
        }

        /// <summary>
        /// Gets the caret from.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <returns>The caret details.</returns>
        private CaretDetails GetCaretFrom(int start, int end)
        {
            var startLine = this.GetLineFromPosition(start);
            var endLine = this.GetLineFromPosition(end);
            var lines = (from i in Enumerable.Range(startLine, endLine - startLine + 1)
                         let line = this.control.Lines[i]
                         select new TextLine(i, line.StartPosition, line.EndPosition)).ToArray();
            var range = new Editor.TextRange(start, end - start, lines);

            return new CaretDetails(
                start,
                range,
                this.control.NativeInterface.GetXOffset(),
                this.control.NativeInterface.GetFirstVisibleLine());
        }

        private void OnDoubleClick(object sender, EventArgs e)
        {
            string selection = this.GetSelectedText(false);
            this.control.Markers.DeleteAll(this.control.FindReplace.Marker.Number);
            this.control.FindReplace.ClearAllHighlights();
            if (!string.IsNullOrEmpty(selection) && System.Text.RegularExpressions.Regex.IsMatch(selection, @"^\w+$"))
            {
                IList<Range> ranges = this.control.FindReplace.FindAll(selection);
                this.control.FindReplace.HighlightAll(ranges);
                this.control.FindReplace.MarkAll(ranges);
            }

            this.UpdateBraceHighlighting();
        }

        private void OnTextChanged(object sender, TextModifiedEventArgs e)
        {
            if (!this.control.Focused && this.control.RectangleToScreen(this.control.Bounds).Contains(Cursor.Position))
            {
                if (Form.ActiveForm == this.control.FindForm())
                {
                    this.control.Focus();
                }
            }

            if (this.TextChanged != null)
            {
                this.TextChanged(this, EventArgs.Empty);
            }

            this.UpdateBraceHighlighting();
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            this.OnCaretChanged();
            this.UpdateBraceHighlighting();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (this.keyHandlers.ContainsKey(e.KeyData))
            {
                this.keyHandlers[e.KeyData](this, e);
            }

            this.UpdateBraceHighlighting();
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            this.OnCaretChanged();
            this.UpdateBraceHighlighting();
        }

        /// <summary>
        /// Raises the CaretChanged event.
        /// </summary>
        private void OnCaretChanged()
        {
            if (this.CaretChanged != null)
            {
                this.CaretChanged(this, EventArgs.Empty);
            }
        }

        private string StyleText(string text, int start)
        {
            char[] chrAry = text.ToCharArray();
            for (int i = 0; i < chrAry.Length; i++)
            {
                chrAry[i] = this.StyleChar(this.control.Styles.GetStyleAt(i + start), chrAry[i]);
            }

            return new string(chrAry);
        }

        private char StyleChar(int style, char c)
        {
            if (this.syntaxConfig != null)
            {
                switch (this.syntaxConfig.Styles[style].Case)
                {
                    case StyleCase.Mixed:
                        return c;
                    case StyleCase.Upper:
                        return Char.ToUpper(c);
                    case StyleCase.Lower:
                        return Char.ToLower(c);
                    default:
                        throw new InvalidOperationException("Invalid capitalisation type.");
                }
            }

            return c;
        }

        /// <summary>
        /// Sets the plain style.
        /// </summary>
        /// <param name="editorStyle">The editor style.</param>
        private void SetPlainStyle(IEditorStyleConfiguration editorStyle)
        {
            this.syntaxConfig = new Configuration("null");

            this.syntaxConfig.Markers_List = new MarkersConfigList(); // not created by constructor.

            this.syntaxConfig.Lexing_Language = "null";
            this.syntaxConfig.Indentation_SmartIndentType = SmartIndent.None;
        }

        /// <summary>
        /// Sets the MS SQL style.
        /// </summary>
        /// <param name="editorStyle">The editor style.</param>
        private void SetMsSqlStyle(IEditorStyleConfiguration editorStyle)
        {
            this.syntaxConfig = new Configuration("mssql");

            this.syntaxConfig.Markers_List = new MarkersConfigList(); // not created by constructor.

            this.syntaxConfig.Lexing_Language = "mssql";
            this.syntaxConfig.Lexing_LineCommentPrefix = "--";
            this.syntaxConfig.Lexing_StreamCommentPrefix = "/*";
            this.syntaxConfig.Lexing_StreamCommentSuffix = "*/";
            this.syntaxConfig.Indentation_SmartIndentType = SmartIndent.Simple;

            /*
             * Word Lists
Statements
Data Types
System tables
Global variables
Functions
System Stored Procedures
Operators
             * Style Names
DOCUMENT_DEFAULT		= 0
COMMENT					= 1
LINE_COMMENT			= 2
NUMBER					= 3
STRING					= 4
OPERATOR				= 5
IDENTIFIER				= 6
VARIABLE				= 7
COLUMN_NAME				= 8
STATEMENT				= 9
DATATYPE				= 10
SYSTABLE				= 11
GLOBAL_VARIABLE			= 12
FUNCTION				= 13
STORED_PROCEDURE		= 14
DEFAULT_PREF_DATATYPE	= 15
COLUMN_NAME_2			= 16
             */

            this.syntaxConfig.Styles.Add(this.SetWordStyle("Default", 0, editorStyle, editorStyle));

            this.syntaxConfig.Styles.Add(this.SetWordStyle("Comment", 1, editorStyle.CommentStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Line Comment", 2, editorStyle.CommentStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Number", 3, editorStyle.ConstantStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("String", 4, editorStyle.ConstantStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Operator", 5, editorStyle.OperatorStyle, editorStyle));
            this.syntaxConfig.Lexing_Keywords.Add(new KeyWordConfig(6, BuildWordList(editorStyle.OperatorStyle.Words), false));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Identifier", 6, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Variable", 7, editorStyle.VariableStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Column Name", 8, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Statment", 9, editorStyle.KeywordStyle, editorStyle));
            this.syntaxConfig.Lexing_Keywords.Add(new KeyWordConfig(0, BuildWordList(editorStyle.KeywordStyle.Words), false));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Datatype", 10, editorStyle.DataTypeStyle, editorStyle));
            this.syntaxConfig.Lexing_Keywords.Add(new KeyWordConfig(1, BuildWordList(editorStyle.DataTypeStyle.Words), false));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Systable", 11, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Global Variable", 12, editorStyle.VariableStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Function", 13, editorStyle.FunctionStyle, editorStyle));
            this.syntaxConfig.Lexing_Keywords.Add(new KeyWordConfig(4, BuildWordList(editorStyle.FunctionStyle.Words), false));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Stored Procedure", 14, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Default Pref Datatype", 15, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Column Name 2", 16, editorStyle, editorStyle));
        }

        /// <summary>
        /// Sets the C++ style.
        /// </summary>
        /// <param name="editorStyle">The editor style.</param>
        private void SetCppStyle(IEditorStyleConfiguration editorStyle)
        {
            this.syntaxConfig = new Configuration("linq");

            this.syntaxConfig.Markers_List = new MarkersConfigList(); // not created by constructor.

            this.syntaxConfig.Lexing_Language = "cpp";
            this.syntaxConfig.Lexing_LineCommentPrefix = "//";
            this.syntaxConfig.Lexing_StreamCommentPrefix = "/*";
            this.syntaxConfig.Lexing_StreamCommentSuffix = "*/";
            this.syntaxConfig.Indentation_SmartIndentType = SmartIndent.CPP;

            /*
             * Word Lists
Primary keywords and identifiers
Secondary keywords and identifiers
Documentation comment keywords
Unused
Global classes and typedefs
             * Style Names
DOCUMENT_DEFAULT			= 0
COMMENT						= 1
COMMENTLINE					= 2
COMMENTDOC					= 3
NUMBER						= 4
WORD						= 5
STRING						= 6
CHARACTER					= 7
UUID						= 8
PREPROCESSOR				= 9
OPERATOR					= 10
IDENTIFIER					= 11
STRINGEOL					= 12
VERBATIM					= 13
REGEX						= 14
COMMENTLINEDOC				= 15
WORD2						= 16
COMMENTDOCKEYWORD			= 17
COMMENTDOCKEYWORDERROR		= 18
GLOBALCLASS					= 19
             */

            this.syntaxConfig.Styles.Add(this.SetWordStyle("Default", 0, editorStyle, editorStyle));

            this.syntaxConfig.Styles.Add(this.SetWordStyle("Comment", 1, editorStyle.CommentStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Line Comment", 2, editorStyle.CommentStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Doc Comment", 3, editorStyle.CommentStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Number", 4, editorStyle.ConstantStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Word", 5, editorStyle.KeywordStyle, editorStyle));
            this.syntaxConfig.Lexing_Keywords.Add(new KeyWordConfig(0, BuildWordList(editorStyle.KeywordStyle.Words), true));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("String", 6, editorStyle.ConstantStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Character", 7, editorStyle.ConstantStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("UUID", 8, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Preprocessor", 9, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Operator", 10, editorStyle.OperatorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Identifier", 11, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("String EOL", 12, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Verbatim", 13, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Regex", 14, editorStyle.ConstantStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Doc Line Comment", 15, editorStyle.CommentStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Word2", 16, editorStyle.FunctionStyle, editorStyle));
            this.syntaxConfig.Lexing_Keywords.Add(new KeyWordConfig(2, BuildWordList(editorStyle.FunctionStyle.Words), true));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Doc Comment Keyword", 17, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Doc Comment Keyword Error", 18, editorStyle, editorStyle));
            this.syntaxConfig.Styles.Add(this.SetWordStyle("Global Class", 19, editorStyle, editorStyle));
            this.syntaxConfig.Lexing_Keywords.Add(new KeyWordConfig(4, BuildWordList(editorStyle.DataTypeStyle.Words), true));
        }

        /// <summary>
        /// Builds the word list.
        /// </summary>
        /// <param name="words">The words.</param>
        /// <returns></returns>
        private static string BuildWordList(params string[] words)
        {
            return (words != null ? string.Join(" ", words).ToLower() : string.Empty);
        }

        /// <summary>
        /// Sets the word style.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="number">The number.</param>
        /// <param name="styleConfig">The style config.</param>
        /// <param name="defaultStyleConfig">The default style config.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException">Invalid capitalisation type.</exception>
        private StyleConfig SetWordStyle(string name, int number, IStyleConfiguration styleConfig, IStyleConfiguration defaultStyleConfig)
        {
            StyleConfig style = new StyleConfig();
            style.Name = name;
            style.Number = number;
            style.ForeColor = (!styleConfig.Colour.IsEmpty ? styleConfig.Colour : defaultStyleConfig.Colour);
            style.BackColor = (!styleConfig.BackColour.IsEmpty ? styleConfig.BackColour : defaultStyleConfig.BackColour);
            Font font = (styleConfig.Font != null ? styleConfig.Font : defaultStyleConfig.Font);
            if (font != null)
            {
                style.FontName = font.Name;
                style.Size = (int)font.Size;
                style.Bold = font.Bold;
                style.Italic = font.Italic;
            }

            this.styleCase[number] = styleConfig.Capitalisation;
            switch (styleConfig.Capitalisation)
            {
                case Capitalisation.LowerCase:
                    style.Case = StyleCase.Lower;
                    break;
                case Capitalisation.UpperCase:
                    style.Case = StyleCase.Upper;
                    break;
                case Capitalisation.Default:
                case Capitalisation.TitleCase:
                    style.Case = StyleCase.Mixed;
                    break;
                default:
                    throw new InvalidOperationException("Invalid capitalisation type.");
            }

            return style;
        }

        /// <summary>
        /// Updates the brace highlighting.
        /// </summary>
        private void UpdateBraceHighlighting()
        {
            // When the caret changes update the brace highlighting.
            var selection = this.control.Selection;
            if (selection.End == selection.Start)
            {
                if (BraceCharacters.Any(c => c == this.control.CharAt(selection.Start)))
                {
                    int pos = this.control.NativeInterface.BraceMatch(selection.Start, 1000);
                    if (pos >= 0)
                    {
                        this.control.NativeInterface.BraceHighlight(selection.Start, pos);
                    }
                }
                else if (selection.Start > 0)
                {
                    if (BraceCharacters.Any(c => c == this.control.CharAt(selection.Start - 1)))
                    {
                        int pos = this.control.NativeInterface.BraceMatch(selection.Start - 1, 1000);
                        if (pos >= 0)
                        {
                            this.control.NativeInterface.BraceHighlight(selection.Start - 1, pos);
                        }
                    }
                }
                else
                {
                    this.control.NativeInterface.BraceHighlight(-1, -1);
                }
            }
            else
            {
                this.control.NativeInterface.BraceHighlight(-1, -1);
            }
        }
    }
}
