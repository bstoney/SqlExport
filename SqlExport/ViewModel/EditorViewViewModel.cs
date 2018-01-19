using SqlExport.Messages;
namespace SqlExport.ViewModel
{
    using System;
    using System.Linq;
    using System.Monads;
    using System.Text;
    using System.Windows;
    using System.Windows.Forms.VisualStyles;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;

    using Microsoft.Practices.Unity;

    using SqlExport.Common;
    using SqlExport.Common.Editor;
    using SqlExport.Data;
    using SqlExport.Editor;
    using SqlExport.Logic;
    using SqlExport.Messages;
    using SqlExport.Messages.StatusPanel;

    /// <summary>
    /// Defines the EditorViewViewModel class.
    /// </summary>
    public partial class EditorViewViewModel : ViewModelBase
    {
        /// <summary>
        /// The special VB
        /// </summary>
        public const string SpecialVb = "VB";

        /// <summary>
        /// The special C#
        /// </summary>
        public const string SpecialCsharp = "C#";

        /// <summary>
        /// The special unformatted
        /// </summary>
        public const string SpecialUnformatted = "Unformatted";

        /// <summary>
        /// The editor control
        /// </summary>
        private readonly IEditorControl editorControl;

        /// <summary>
        /// The current style.
        /// </summary>
        private readonly IEditorStyleConfiguration currentStyle;

        /// <summary>
        /// The is updating.
        /// </summary>
        private bool isUpdating;

        /// <summary>
        /// The finder
        /// </summary>
        private TextFinder finder;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorViewViewModel"/> class.
        /// </summary>
        public EditorViewViewModel()
            : this(new DefaultEditor())
        {
            // Load the default editor for design mode.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorViewViewModel"/> class.
        /// </summary>
        /// <param name="editorControl">The editor control.</param>
        [InjectionConstructor]
        public EditorViewViewModel(IEditorControl editorControl)
        {
            this.currentStyle = DefaultOptions.GetEditorStyle();

            // Load the default editor for design mode.
            this.editorControl = editorControl;

            this.EditorControl.Initialise();

            this.editorControl.CaretChanged += (s, caret) => this.EditorCaretChanged();
            this.editorControl.TextChanged += (s, e) => this.EditorTextChanged();

            this.editorControl.AddKeyHandler(EditorKey.RunKey, () => Messenger.Default.Send(new RunQueryMessage(), this));
            this.editorControl.AddKeyHandler(EditorKey.AlternateRunKey, () => Messenger.Default.Send(new RunQueryMessage(), this));
            this.editorControl.AddKeyHandler(EditorKey.SaveKey, () => Messenger.Default.Send(new SaveQueryMessage(), this));
            this.editorControl.AddKeyHandler(EditorKey.NextQueryKey, () => Messenger.Default.Send(new NextQueryMessage()));
            this.editorControl.AddKeyHandler(EditorKey.PreviousQueryKey, () => Messenger.Default.Send(new PreviousQueryMessage()));

            this.editorControl.AddKeyHandler(
                EditorKey.FindKey,
                () =>
                    {
                        this.IsFinding = true;
                        Messenger.Default.Send(new FocusFindMessage(), this);
                    });

            this.editorControl.AddKeyHandler(
                EditorKey.FindKeyNext,
                () =>
                    {
                        if (this.IsFinding)
                        {
                            this.Find();
                        }
                    });

            this.editorControl.AddKeyHandler(EditorKey.CommentKey, () => this.CommentSelection());
            this.editorControl.AddKeyHandler(EditorKey.UncommentKey, () => this.UncommentSelection());

            this.editorControl.AddKeyHandler(EditorKey.CancelKey, () => this.IsFinding = false);

            Messenger.Default.Register<SetDatabaseMessage>(this, this, m => this.SetDatabaseStyle(m.Database));
            Messenger.Default.Register<ErrorSelectedMessage>(
                this, this, m => this.editorControl.SelectLines(m.LineNumber - 1, m.LineNumber - 1));

            if (ViewModelBase.IsInDesignModeStatic)
            {
                this.EditorControl.SetText("***** Design Mode *****");
            }
        }

        /// <summary>
        /// Gets the editor control.
        /// </summary>
        public IEditorControl EditorControl
        {
            get { return this.editorControl; }
        }

        /// <summary>
        /// Gets the QueryText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string QueryText
        {
            get { return this.Caret.Length > 0 ? this.AllText.Substring(this.Caret.Start, this.Caret.Length) : this.AllText; }
        }

        /// <summary>
        /// Editor's text changed.
        /// </summary>
        private void EditorTextChanged()
        {
            if (!this.HasChanged)
            {
                this.HasChanged = true;
            }

            this.isUpdating = true;
            this.AllText = this.editorControl.GetText(false);
            this.isUpdating = false;
        }

        /// <summary>
        /// Editor's caret changed.
        /// </summary>
        private void EditorCaretChanged()
        {
            if (!this.IsInDesignMode)
            {
                this.isUpdating = true;
                this.Caret = this.editorControl.Caret;
                this.isUpdating = false;

                Messenger.Default.Send(new CaretChangedMessage(this.Caret), this);
            }
        }

        /// <summary>
        /// Copies the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void Copy(string parameter)
        {
            string text;
            switch (parameter)
            {
                case EditorViewViewModel.SpecialVb:
                    text = this.editorControl.GetSelectedText(true);
                    text = string.Concat("\"", text.Replace("\"", "\"\"").Replace("\r\n", "\" & vbNewLine & _\r\n\""), "\"");
                    Clipboard.SetDataObject(text, true);
                    break;
                case EditorViewViewModel.SpecialCsharp:
                    text = string.Concat("@\"", this.editorControl.GetSelectedText(true), "\"");
                    break;
                case EditorViewViewModel.SpecialUnformatted:
                    text = this.editorControl.GetSelectedText(false);
                    break;
                default:
                    text = this.editorControl.GetSelectedText(true);
                    break;
            }

            Clipboard.SetDataObject(text, true);
        }

        /// <summary>
        /// Pastes the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        private void Paste(string parameter)
        {
            var text = Clipboard.GetDataObject().With(d => d.GetData("Text", true)).ToString();
            switch (parameter)
            {
                case EditorViewViewModel.SpecialVb:
                    text = TextHelper.VbToPlainText(text);
                    break;
                case EditorViewViewModel.SpecialCsharp:
                    text = TextHelper.CsharpToPlainText(text);
                    break;
                default:
                    this.editorControl.Copy();
                    break;
            }

            this.editorControl.SetSelectedText(text);
        }

        /// <summary>
        /// Auto formats the current selection.
        /// </summary>
        private void AutoFormat()
        {
            var text = this.editorControl.GetSelectedText(false);

            // TODO
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

            this.editorControl.SetText(text);
        }

        /// <summary>
        /// Uncomments the selection.
        /// </summary>
        private void UncommentSelection()
        {
            string comment = this.GetCommentText();
            this.ForeachSelectedLine(l => (l.StartsWith(comment) ? l.Substring(comment.Length) : l));
        }

        /// <summary>
        /// Comments the selection.
        /// </summary>
        private void CommentSelection()
        {
            string comment = this.GetCommentText();
            this.ForeachSelectedLine(l => (!string.IsNullOrWhiteSpace(l) ? string.Concat(comment, l) : l));
        }

        /// <summary>
        /// Finds this instance.
        /// </summary>
        private void Find()
        {
            this.finder = new TextFinder(this.FindText, true);

            if (this.finder != null)
            {
                int index = this.finder.Find(this.AllText, this.Caret.End);
                if (index >= 0)
                {
                    this.Caret = new CaretDetails(
                        index,
                        new TextRange(index, this.finder.Match.Length, new TextLine[] { }),
                        this.Caret.HorizontalScroll,
                        this.Caret.VerticalScroll);
                }
                else
                {
                    MessageBox.Show("Text not found.");
                }
            }
        }

        /// <summary>
        /// The get comment text.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetCommentText()
        {
            if (this.currentStyle.With(s => s.CommentStyle.CommentSyntax == CommentSyntax.CSharp))
            {
                return "//";
            }

            return "--";
        }

        /// <summary>
        /// The comment lines.
        /// </summary>
        /// <param name="lineFunction">The line function.</param>
        private void ForeachSelectedLine(Func<string, string> lineFunction)
        {
            // Select each line to be commented.
            var startLine = this.Caret.StartLine;
            var endLine = this.Caret.EndLine;

            // If more than one line is selected, but the selection ends at the start of the last line, don't include it.
            if (endLine != startLine && this.Caret.Range.Lines.Last().Start == this.Caret.End)
            {
                endLine--;
            }

            this.editorControl.SelectLines(startLine, endLine);

            // If there is still no selection there is nothing to do.
            if (this.Caret.Length != 0)
            {
                // Apply the commenting rule.
                var selectedText = this.editorControl.GetSelectedText(false);
                var lines = from l in selectedText.Split('\n')
                            select string.IsNullOrWhiteSpace(l) ? l : lineFunction(l);

                // Replace the selection.
                this.editorControl.SetSelectedText(string.Join("\n", lines));
            }

            this.editorControl.SelectLines(startLine, endLine);
        }

        /// <summary>
        /// Sets the style from the database.
        /// </summary>
        /// <param name="database">The database.</param>
        private void SetDatabaseStyle(DatabaseDetails database)
        {
            if (database != null)
            {
                try
                {
                    var sd = database.GetSyntaxDefinition();
                    this.currentStyle.KeywordStyle.Words = sd.Keywords;
                    this.currentStyle.KeywordStyle.Capitalisation = sd.KeywordCapitalisation;
                    this.currentStyle.FunctionStyle.Words = sd.Functions;
                    this.currentStyle.FunctionStyle.Capitalisation = sd.KeywordCapitalisation;
                    this.currentStyle.DataTypeStyle.Words = sd.DataTypes;
                    this.currentStyle.DataTypeStyle.Capitalisation = sd.KeywordCapitalisation;
                    this.currentStyle.OperatorStyle.Words = sd.Operators;
                    this.currentStyle.OperatorStyle.Capitalisation = sd.KeywordCapitalisation;
                    this.currentStyle.CommentStyle.CommentSyntax = sd.CommentSyntax;
                }
                catch (Exception exp)
                {
                    Messenger.Default.Send((DisplayMessage)exp, this);
                }
            }
            else
            {
                this.currentStyle.KeywordStyle.Words = new string[] { };
                this.currentStyle.KeywordStyle.Capitalisation = Capitalisation.Default;
                this.currentStyle.FunctionStyle.Words = new string[] { };
                this.currentStyle.FunctionStyle.Capitalisation = Capitalisation.Default;
                this.currentStyle.DataTypeStyle.Words = new string[] { };
                this.currentStyle.DataTypeStyle.Capitalisation = Capitalisation.Default;
                this.currentStyle.OperatorStyle.Words = new string[] { };
                this.currentStyle.OperatorStyle.Capitalisation = Capitalisation.Default;
                this.currentStyle.CommentStyle.CommentSyntax = CommentSyntax.None;
            }

            this.editorControl.SetEditorStyle(this.currentStyle);
        }
    }
}