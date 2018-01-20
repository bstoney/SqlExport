namespace SqlExport.Ui
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    using GalaSoft.MvvmLight;

    using SqlExport.Common;
    using SqlExport.Common.Editor;
    using SqlExport.Editor;

    /// <summary>
    /// Represents the method which will handle the caret changed event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="caretDetails">The caret details.</param>
    public delegate void CaretChangedHandler(object sender, CaretDetails caretDetails);

    /// <summary>
    /// Defines the SQLView class.
    /// </summary>
    internal partial class SqlView : UserControl, IContextMenuProvider
    {
        #region Fields

        /////// <summary>
        /////// The caret change count
        /////// </summary>
        ////private int caretChangeCount;

        /////// <summary>
        /////// Indicates whether the sequence is command.
        /////// </summary>
        ////private bool isCommandSequence;

        /////// <summary>
        /////// The editor control
        /////// </summary>
        ////private IEditorControl editorControl;

        /////// <summary>
        /////// The editor style
        /////// </summary>
        ////private IEditorStyleConfiguration editorStyle;

        /// <summary>
        /// The finder
        /// </summary>
        private TextFinder finder;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlView"/> class.
        /// </summary>
        public SqlView()
        {
            this.InitializeComponent();

            this.mnuContext.Opening += this.OnContextMenuOpening;
            ////this.mnuUndo.Click += this.OnUndo;
            ////this.mnuRedo.Click += this.OnRedo;
            ////this.mnuCut.Click += this.OnCut;
            ////this.mnuCopy.Click += this.OnCopy;
            ////this.mnuCopySource.Click += this.OnCopySource;
            ////this.mnuCopyVB.Click += this.OnCopyVB;
            ////this.mnuCopyCS.Click += this.OnCopyCS;
            ////this.mnuPaste.Click += this.OnPaste;
            ////this.mnuPasteCS.Click += this.OnPasteCS;
            ////this.mnuDelete.Click += this.OnDelete;
            ////this.mnuAutoFormat.Click += this.OnAutoFormat;
            ////this.mnuPasteVB.Click += this.OnPasteVB;
            ////this.mnuSelectAll.Click += this.OnSelectAll;
        }

        #endregion

        #region Public Events

        /////// <summary>
        /////// Occurs when the editor caret changes.
        /////// </summary>
        ////public event CaretChangedHandler CaretChanged;

        /////// <summary>
        /////// Occurs when the next short-cut is pressed.
        /////// </summary>
        ////public event EventHandler Next;

        /////// <summary>
        /////// Occurs when the previous short-cut is pressed.
        /////// </summary>
        ////public event EventHandler Previous;

        /////// <summary>
        /////// Occurs when the run short-cut is pressed.
        /////// </summary>
        ////public event EventHandler Run;

        /////// <summary>
        /////// Occurs when the save short-cut is pressed.
        /////// </summary>
        ////public event EventHandler Save;

        #endregion

        #region Public Properties

        /////// <summary>
        /////// Gets the caret.
        /////// </summary>
        /////// <value>
        /////// The caret.
        /////// </value>
        ////public CaretDetails Caret
        ////{
        ////    get
        ////    {
        ////        return this.editorControl.Caret;
        ////    }

        ////    set
        ////    {
        ////        this.editorControl.Caret = value;
        ////    }
        ////}

        /////// <summary>
        /////// Gets a value indicating whether has context menu.
        /////// </summary>
        ////public bool HasContextMenu
        ////{
        ////    get
        ////    {
        ////        return true;
        ////    }
        ////}

        /////// <summary>
        /////// Gets the menu title.
        /////// </summary>
        ////public string MenuTitle
        ////{
        ////    get
        ////    {
        ////        return "Edit";
        ////    }
        ////}

        /////// <summary>
        /////// Gets or sets the text.
        /////// </summary>
        /////// <returns>The text associated with this control.</returns>
        ////public new string Text
        ////{
        ////    get
        ////    {
        ////        return this.editorControl.GetText(false);
        ////    }

        ////    set
        ////    {
        ////        this.editorControl.SetText(value);
        ////    }
        ////}

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.ComponentModel.Component" /> is currently in design mode.
        /// </summary>
        /// <returns>true if the <see cref="T:System.ComponentModel.Component" /> is in design mode; otherwise, false.</returns>
        protected new bool DesignMode
        {
            get
            {
                return ViewModelBase.IsInDesignModeStatic;
            }
        }

        /////// <summary>
        /////// Gets or sets the query statement.
        /////// </summary>
        /////// <value>
        /////// The query statement.
        /////// </value>
        ////private string QueryStatement
        ////{
        ////    get
        ////    {
        ////        if (this.Caret.Length == 0)
        ////        {
        ////            return this.editorControl.GetText(true);
        ////        }

        ////        return this.editorControl.GetSelectedText(true);
        ////    }

        ////    set
        ////    {
        ////        if (this.Caret.Length == 0)
        ////        {
        ////            this.editorControl.SetText(value);
        ////        }
        ////        else
        ////        {
        ////            this.editorControl.SetSelectedText(value);
        ////        }
        ////    }
        ////}

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The find text.
        /// </summary>
        /// <param name="match">
        /// The match.
        /// </param>
        /// <param name="ignoreCase">
        /// The ignore case.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool FindText(string match, bool ignoreCase)
        {
            this.finder = new TextFinder(match, ignoreCase);
            return this.FindText();
        }

        /////// <summary>
        /////// The get context menu.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="ContextMenuStrip"/>.
        /////// </returns>
        ////public ContextMenuStrip GetContextMenu()
        ////{
        ////    return this.mnuContext;
        ////}

        /////// <summary>
        /////// Selects the line.
        /////// </summary>
        /////// <param name="startLine">The start line.</param>
        /////// <param name="endLine">The end line.</param>
        ////public void SelectLines(int startLine, int endLine)
        ////{
        ////    this.editorControl.SelectLines(startLine, endLine);
        ////}

        /////// <summary>
        /////// Sets the editor style.
        /////// </summary>
        /////// <param name="editorStyle">
        /////// The editor style.
        /////// </param>
        ////public void SetEditorStyle(IEditorStyleConfiguration editorStyle)
        ////{
        ////    if (editorStyle != null)
        ////    {
        ////        this.editorStyle = editorStyle;
        ////        this.editorControl.SetEditorStyle(this.editorStyle);
        ////    }
        ////}

        #endregion

        #region Methods

        /// <summary>
        /// The replace text.
        /// </summary>
        /// <param name="match">
        /// The match.
        /// </param>
        /// <param name="replace">
        /// The replace.
        /// </param>
        /// <param name="ignoreCase">
        /// The ignore case.
        /// </param>
        internal void ReplaceText(string match, string replace, bool ignoreCase)
        {
            if (string.Compare(this.editorControl.GetSelectedText(false), match, ignoreCase) == 0)
            {
                this.editorControl.SetSelectedText(replace);
            }

            this.FindText();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="T:System.EventArgs"/> that contains the event data.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ////string text = this.editorControl.GetText(false);
            ////try
            ////{
            ////    this.editorControl = DependencyResolver.Default.Resolve<IEditorControl>();
            ////}
            ////catch (Exception exp)
            ////{
            ////    if (!this.DesignMode)
            ////    {
            ////        ErrorDialogLogic.AddError(exp);
            ////    }

            ////    this.editorControl = new DefaultEditor();
            ////}

            ////this.editorControl.SetText(text);

            ////this.SetEditorStyle(this.editorStyle);

            ////this.editorControl.Control.Dock = DockStyle.Fill;
            ////this.editorControl.Control.ContextMenuStrip = this.mnuContext;
            ////this.editorControl.Control.MouseHover += this.OnMouseHover;

            ////this.editorControl.CaretChanged += this.OnCaretChanged;
            ////this.editorControl.TextChanged += this.OnEditorTextChanged;
            ////this.editorControl.AddKeyDownHandler(Keys.F5, this.OnRun);
            ////this.editorControl.AddKeyDownHandler(Keys.Control | Keys.T, this.OnRun);
            ////this.editorControl.AddKeyDownHandler(Keys.Control | Keys.S, this.OnSave);
            ////this.editorControl.AddKeyDownHandler(Keys.Control | Keys.Tab, this.OnNext);
            ////this.editorControl.AddKeyDownHandler(Keys.Control | Keys.Shift | Keys.Tab, this.OnPrevious);
            ////this.editorControl.AddKeyDownHandler(Keys.F3, this.OnFind);

            ////this.editorControl.AddKeyDownHandler(Keys.Control | Keys.K, this.OnCommandKey);
            ////this.editorControl.AddKeyDownHandler(Keys.Control | Keys.C, this.OnCommandKey);
            ////this.editorControl.AddKeyDownHandler(Keys.Control | Keys.U, this.OnCommandKey);

            ////this.Controls.Add(this.editorControl.Control);
        }

        /////// <summary>
        /////// The comment lines.
        /////// </summary>
        /////// <param name="lineFunction">The line function.</param>
        ////private void ForeachSelectedLine(Func<string, string> lineFunction)
        ////{
        ////    // Select each line to be commented.
        ////    var startLine = this.Caret.StartLine;
        ////    var endLine = this.Caret.EndLine;

        ////    // If more than one line is selected, but the selection ends at the start of the last line, don't include it.
        ////    if (endLine != startLine && this.Caret.Range.Lines.Last().Start == this.Caret.End)
        ////    {
        ////        endLine--;
        ////    }

        ////    this.editorControl.SelectLines(startLine, endLine);

        ////    // If there is still no selection there is nothing to do.
        ////    if (this.Caret.Length != 0)
        ////    {
        ////        // Apply the commenting rule.
        ////        var selectedText = this.editorControl.GetSelectedText(false);
        ////        var lines = from l in selectedText.Split('\n')
        ////                    select string.IsNullOrWhiteSpace(l) ? l : lineFunction(l);

        ////        // Replace the selection.
        ////        this.editorControl.SetSelectedText(string.Join("\n", lines));
        ////    }

        ////    this.editorControl.SelectLines(startLine, endLine);
        ////}

        /////// <summary>
        /////// The copy.
        /////// </summary>
        ////private void Copy()
        ////{
        ////    try
        ////    {
        ////        Clipboard.SetDataObject(this.editorControl.GetSelectedText(true), true);
        ////    }
        ////    catch (Exception)
        ////    {
        ////        // Ignore clipboard failures.
        ////    }
        ////}

        /////// <summary>
        /////// The copy source.
        /////// </summary>
        ////private void CopySource()
        ////{
        ////    this.editorControl.Copy();
        ////}

        /// <summary>
        /// The find text.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        private bool FindText()
        {
            bool found = false;
            if (this.finder != null)
            {
                int index = this.finder.Find(this.Text, this.Caret.End);
                if (index >= 0)
                {
                    this.Caret = new CaretDetails(
                        index,
                        new TextRange(index, this.finder.Match.Length, new TextLine[] { }),
                        this.Caret.HorizontalScroll,
                        this.Caret.VerticalScroll);
                    found = true;
                }
                else
                {
                    MessageBox.Show("Text not found.");
                }
            }

            return found;
        }

        /////// <summary>
        /////// The get comment text.
        /////// </summary>
        /////// <returns>
        /////// The <see cref="string"/>.
        /////// </returns>
        ////private string GetCommentText()
        ////{
        ////    if (this.editorStyle != null && this.editorStyle.CommentStyle.CommentSyntax == CommentSyntax.CSharp)
        ////    {
        ////        return "//";
        ////    }

        ////    return "--";
        ////}

////        /// <summary>
////        /// The on auto format.
////        /// </summary>
////        /// <param name="sender">
////        /// The sender.
////        /// </param>
////        /// <param name="e">
////        /// The e.
////        /// </param>
////        private void OnAutoFormat(object sender, EventArgs e)
////        {
////            string text = this.QueryStatement;

////            text = Regex.Replace(text, @"(?:(?<first>SELECT|FROM|IN)|
////(?:(?<first>CROSS|INNER|ORDER)\s+(?<second>JOIN|BY))|
////(?:(?<first>ORDER|GROUP)\s+(?<second>BY))|
////(?:(?<first>FULL|LEFT|RIGHT)\s+(?<second>OUTER)\s+(?<third>JOIN)))
////\s+", "${first} ${second} ${third} ", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
////            text = Regex.Replace(text, @"^[\t ]+", "\t", RegexOptions.Multiline);
////            text = Regex.Replace(text, @"(?<start>\S)[\t ]+(?<end>\S)", "${start} ${end}");
////            text = Regex.Replace(
////                text, @"(?:(?:[\t ]+)|\r\n)(?<keyword>(?:SELECT|INTO|FROM|
////INNER\s+JOIN|LEFT\s+OUTER\s+JOIN|RIGHT\s+OUTER\s+JOIN|FULL\s+OUTER\s+JOIN|CROSS\s+JOIN|
////WHERE|GROUP\s+BY|ORDER\s+BY|AND|OR)\b)\s+", "\r\n${keyword} ", RegexOptions.IgnoreCase
////                                                                                                                                                                                                                                    | RegexOptions
////                                                                                                                                                                                                                                          .IgnorePatternWhitespace);
////            text = Regex.Replace(text, @"\s+(?<keyword>AND|OR)\s+", "\r\n\t${keyword} ", RegexOptions.IgnoreCase);

////            this.QueryStatement = text;
////        }

        /////// <summary>
        /////// Called when caret changes.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The <see cref="EventArgs"/> instance containing the event data.
        /////// </param>
        ////private void OnCaretChanged(object sender, EventArgs e)
        ////{
        ////    // If the user has pressed the command sequence; count the caret changes.
        ////    if (this.isCommandSequence)
        ////    {
        ////        // If there has already been a caret change cancel the command sequence.
        ////        if (this.caretChangeCount > 1)
        ////        {
        ////            this.isCommandSequence = false;
        ////        }
        ////        else
        ////        {
        ////            this.caretChangeCount++;
        ////        }
        ////    }

        ////    if (this.CaretChanged != null && !this.DesignMode)
        ////    {
        ////        this.CaretChanged(this, this.editorControl.Caret);
        ////    }
        ////}

        /////// <summary>
        /////// The on command key.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="args">
        /////// The args.
        /////// </param>
        ////private void OnCommandKey(object sender, KeyEventArgs args)
        ////{
        ////    if (args.KeyData == (Keys.Control | Keys.K))
        ////    {
        ////        this.isCommandSequence = true;
        ////        this.caretChangeCount = 0;
        ////    }
        ////    else if (this.isCommandSequence)
        ////    {
        ////        string comment = this.GetCommentText();
        ////        switch (args.KeyData)
        ////        {
        ////            case Keys.Control | Keys.C:
        ////                this.ForeachSelectedLine(l => (!string.IsNullOrWhiteSpace(l) ? string.Concat(comment, l) : l));
        ////                args.Handled = true;
        ////                break;
        ////            case Keys.Control | Keys.U:
        ////                this.ForeachSelectedLine(l => (l.StartsWith(comment) ? l.Substring(comment.Length) : l));
        ////                args.Handled = true;
        ////                break;
        ////        }
        ////    }
        ////    else if (args.KeyData == (Keys.Control | Keys.C))
        ////    {
        ////        this.Copy();
        ////        args.Handled = true;
        ////    }
        ////}

        /// <summary>
        /// The on context menu opening.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnContextMenuOpening(object sender, CancelEventArgs e)
        {
            bool bolHasSelection = this.Caret.Length > 0;

            this.mnuUndo.Enabled = this.editorControl.CanUndo;
            this.mnuRedo.Enabled = this.editorControl.CanRedo;
            this.mnuCut.Enabled = bolHasSelection;
            this.mnuCopy.Enabled = bolHasSelection;
            this.mnuCopySpecial.Enabled = bolHasSelection;
            this.mnuPaste.Enabled = this.editorControl.CanPaste;
            this.mnuPasteSpecial.Enabled = this.editorControl.CanPaste;
            this.mnuDelete.Enabled = bolHasSelection;

            ContextMenuHelper.AddHierachicalContextMenu(this, this.GetContextMenu());
        }

        /////// <summary>
        /////// The on copy.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnCopy(object sender, EventArgs e)
        ////{
        ////    this.Copy();
        ////}

        /////// <summary>
        /////// The on copy cs.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnCopyCS(object sender, EventArgs e)
        ////{
        ////    string text = string.Concat("@\"", this.editorControl.GetSelectedText(true), "\"");
        ////    Clipboard.SetDataObject(text, true);
        ////}

        /////// <summary>
        /////// The on copy source.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnCopySource(object sender, EventArgs e)
        ////{
        ////    this.CopySource();
        ////}

        /////// <summary>
        /////// The on copy vb.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnCopyVB(object sender, EventArgs e)
        ////{
        ////    string text = this.editorControl.GetSelectedText(true);
        ////    text = string.Concat("\"", text.Replace("\"", "\"\"").Replace("\r\n", "\" & vbNewLine & _\r\n\""), "\"");
        ////    Clipboard.SetDataObject(text, true);
        ////}

        /////// <summary>
        /////// The on cut.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnCut(object sender, EventArgs e)
        ////{
        ////    this.editorControl.Cut();
        ////}

        /////// <summary>
        /////// The on delete.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnDelete(object sender, EventArgs e)
        ////{
        ////    this.editorControl.SetSelectedText(string.Empty);
        ////}

        /////// <summary>
        /////// The on editor context menu opening.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnEditorContextMenuOpening(object sender, CancelEventArgs e)
        ////{
        ////    e.Cancel = true;
        ////}

        /////// <summary>
        /////// Called when [editor text changed].
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The <see cref="EventArgs"/> instance containing the event data.
        /////// </param>
        ////private void OnEditorTextChanged(object sender, EventArgs e)
        ////{
        ////    this.OnTextChanged(EventArgs.Empty);
        ////}

        /// <summary>
        /// The on find.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        private void OnFind(object sender, KeyEventArgs args)
        {
            this.FindText();
        }

        /////// <summary>
        /////// Called when [mouse hover].
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The <see cref="EventArgs"/> instance containing the event data.
        /////// </param>
        ////private void OnMouseHover(object sender, EventArgs e)
        ////{
        ////    this.editorControl.Control.Focus();
        ////}

        /////// <summary>
        /////// The on next.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="args">
        /////// The args.
        /////// </param>
        ////private void OnNext(object sender, KeyEventArgs args)
        ////{
        ////    if (this.Next != null)
        ////    {
        ////        this.Next(this, EventArgs.Empty);
        ////    }
        ////}

        /////// <summary>
        /////// The on paste.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnPaste(object sender, EventArgs e)
        ////{
        ////    this.editorControl.Paste();
        ////}

        /////// <summary>
        /////// The on paste cs.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnPasteCS(object sender, EventArgs e)
        ////{
        ////    string text = Clipboard.GetDataObject().GetData("Text", true).ToString();

        ////    // TODO Regex re;
        ////    // re = new Regex( "(\"\\s*\\+\\s*\")", RegexOptions.IgnoreCase );
        ////    // strData = re.Replace( strData, "" );
        ////    // re = new Regex( "([^\\\\]|^)\"", RegexOptions.IgnoreCase );
        ////    // strData = re.Replace( strData, "${1}" );
        ////    // strData = strData.Replace( "\\\"", "\"" );
        ////    // strData = strData.Replace( "\\n", "\r\n" );
        ////    this.editorControl.SetSelectedText(text);
        ////}

        /////// <summary>
        /////// The on paste vb.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnPasteVB(object sender, EventArgs e)
        ////{
        ////    var text = Clipboard.GetDataObject().GetData("Text", true).ToString();
        ////    var unformattedText = new StringBuilder(text);

        ////    var isInString = false;
        ////    var i = 0;
        ////    while (i < unformattedText.Length)
        ////    {
        ////        var currentChar = unformattedText[i];
        ////        var nextChar = i + 1 < unformattedText.Length ? (char?)unformattedText[i + 1] : null;
        ////        if (isInString)
        ////        {
        ////            if (currentChar == '"' && nextChar == '"')
        ////            {
        ////                unformattedText.Remove(i, 1);
        ////            }
        ////            else if (currentChar == '"')
        ////            {
        ////                isInString = false;
        ////                unformattedText.Remove(i, 1);
        ////                continue;
        ////            }
        ////        }
        ////        else
        ////        {
        ////            switch (currentChar)
        ////            {
        ////                case '"':
        ////                    isInString = true;
        ////                    unformattedText.Remove(i, 1);
        ////                    continue;
        ////                case '&':
        ////                case '_':
        ////                    unformattedText.Remove(i, 1);
        ////                    continue;
        ////            }
        ////        }

        ////        i++;
        ////    }

        ////    ////text = Regex.Replace(text, "(\" & _\\s*\")", "", RegexOptions.IgnoreCase);
        ////    ////text = Regex.Replace(text, "(\" & vbnewline & _\\s*\")|(vbnewline)", "\r\n", RegexOptions.IgnoreCase);
        ////    ////text = Regex.Replace(text, "\"([^\"]|$)", "${1}", RegexOptions.IgnoreCase);
        ////    this.editorControl.SetSelectedText(unformattedText.ToString());
        ////}

        /////// <summary>
        /////// The on previous.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="args">
        /////// The args.
        /////// </param>
        ////private void OnPrevious(object sender, KeyEventArgs args)
        ////{
        ////    if (this.Previous != null)
        ////    {
        ////        this.Previous(this, EventArgs.Empty);
        ////    }
        ////}

        /////// <summary>
        /////// The on redo.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnRedo(object sender, EventArgs e)
        ////{
        ////    this.editorControl.Redo();
        ////}

        /////// <summary>
        /////// The on run.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="args">
        /////// The args.
        /////// </param>
        ////private void OnRun(object sender, KeyEventArgs args)
        ////{
        ////    if (this.Run != null)
        ////    {
        ////        this.Run(this, EventArgs.Empty);
        ////    }
        ////}

        /////// <summary>
        /////// The on save.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="args">
        /////// The args.
        /////// </param>
        ////private void OnSave(object sender, KeyEventArgs args)
        ////{
        ////    if (this.Save != null)
        ////    {
        ////        this.Save(this, EventArgs.Empty);
        ////    }
        ////}

        /////// <summary>
        /////// The on select all.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnSelectAll(object sender, EventArgs e)
        ////{
        ////    this.editorControl.SelectAll();
        ////}

        /////// <summary>
        /////// The on undo.
        /////// </summary>
        /////// <param name="sender">
        /////// The sender.
        /////// </param>
        /////// <param name="e">
        /////// The e.
        /////// </param>
        ////private void OnUndo(object sender, EventArgs e)
        ////{
        ////    this.editorControl.Undo();
        ////}

        #endregion

        // TODO private int sciSQL_Key( Control pSender, int ch, int modifiers, int modificationType )
        // {
        // switch( (Keys)ch )
        // {
        // case Keys.Space:

        // if( modifiers == this._ctlEditor.ScintillaKeyConsts.VB_SCMOD_CTRL + this._ctlEditor.ScintillaKeyConsts.VB_SCMOD_SHIFT )
        // {
        // string lst = Module1.frm.lstObjects.Sublist( "" );
        // if( lst != "" )
        // {
        // this._ctlEditor.AutoCShow( 0, lst );
        // }
        // }
        // break;
        // }
        // }

        // private void sciSQL_CharAdded( Control pSender, char CharacterAdded )
        // {
        // if( CharacterAdded == Constants.vbCr || CharacterAdded == Constants.vbLf )
        // {
        // int curLine = pSender.LineFromPosition( pSender.CurrentPos );
        // int lineLength = pSender.LineLength( curLine );
        // if( curLine > 0 && lineLength <= 2 )
        // {
        // int prevLineLength = pSender.LineLength( curLine - 1 );
        // string prevLine = pSender.GetLine( curLine - 1 );
        // string indent = "";
        // int i;
        // for( i = 0; i <= prevLineLength; i++ )
        // {
        // if( prevLine.Chars( i ) == " " || prevLine.Chars( i ) == "\t" )
        // {
        // indent += prevLine.Chars( i );
        // }
        // else
        // {
        // break;
        // }
        // }
        // if( indent != "" )
        // {
        // pSender.ReplaceSel( indent );
        // }
        // }
        // }
        // }

        // private void scisql_KeyUp( object sender, System.Windows.Forms.KeyEventArgs e )
        // {
        // this.sciSQL_MouseUp( this._ctlEditor, null );
        // }
    }
}