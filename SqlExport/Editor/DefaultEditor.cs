namespace SqlExport.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// Defines the DefaultEditor class.
    /// </summary>
    public class DefaultEditor : IEditorControl
    {
        /// <summary>
        /// The control
        /// </summary>
        private readonly RichTextBox control;

        /// <summary>
        /// The key handlers
        /// </summary>
        private readonly Dictionary<Keys, KeyEventHandler> keyHandlers;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultEditor"/> class.
        /// </summary>
        public DefaultEditor()
        {
            this.control = new RichTextBox();
            this.control.MouseUp += this.OnMouseUp;
            this.control.KeyDown += this.OnKeyDown;
            this.control.KeyUp += this.OnKeyUp;
            this.keyHandlers = new Dictionary<Keys, KeyEventHandler>();
        }

        /// <summary>
        /// Occurs when the text is changed by the user.
        /// </summary>
        public event EventHandler TextChanged
        {
            add { this.control.TextChanged += value; }
            remove { this.control.TextChanged += value; }
        }

        /// <summary>
        /// Occurs when the user moves the text caret.
        /// </summary>
        public event EventHandler CaretChanged;

        /// <summary>
        /// Gets or sets the location of the caret and selection.
        /// </summary>
        public CaretDetails Caret
        {
            get
            {
                return this.GetCaretFrom(this.control.SelectionStart, this.control.SelectionStart + this.control.SelectionLength);
            }

            set
            {
                this.control.SelectionStart = value.Start;
                this.control.SelectionLength = value.Length;
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
            get { return this.control.CanUndo; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the editor can redo the last action.
        /// </summary>
        public bool CanRedo
        {
            get { return this.control.CanRedo; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the editor can inset the current clipboard contents.
        /// </summary>
        public bool CanPaste
        {
            get { return Clipboard.ContainsText() && this.control.CanPaste(DataFormats.GetFormat(DataFormats.Text)); }
        }

        /// <summary>
        /// Sets the editors style configuration.
        /// </summary>
        /// <param name="editorStyle">The editor style.</param>
        public void SetEditorStyle(IEditorStyleConfiguration editorStyle)
        {
        }

        /// <summary>
        /// Adds key mapping to a handler.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        public void AddKeyDownHandler(Keys key, KeyEventHandler handler)
        {
            if (this.keyHandlers.ContainsKey(key))
            {
                this.keyHandlers[key] = (KeyEventHandler)Delegate.Combine(this.keyHandlers[key], handler);
            }
            else
            {
                this.keyHandlers[key] = handler;
            }
        }

        /// <summary>
        /// Undoes the last action.
        /// </summary>
        public void Undo()
        {
            this.control.Undo();
        }

        /// <summary>
        /// Redoes the last undo.
        /// </summary>
        public void Redo()
        {
            this.control.Redo();
        }

        /// <summary>
        /// Copies the currently selected text to the clipboard.
        /// </summary>
        public void Copy()
        {
            this.control.Copy();
        }

        /// <summary>
        /// Moves the currently selected text to the clipboard.
        /// </summary>
        public void Cut()
        {
            this.control.Cut();
        }

        /// <summary>
        /// Inserts the current clipboard content.
        /// </summary>
        public void Paste()
        {
            this.control.Paste();
        }

        /// <summary>
        /// Selects the entire document.
        /// </summary>
        public void SelectAll()
        {
            this.control.SelectAll();
        }

        /// <summary>
        /// Replaces the selected text.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetSelectedText(string text)
        {
            this.control.SelectedText = text;
        }

        /// <summary>
        /// Gets the selected text as either a styled or un-styled string.
        /// </summary>
        /// <param name="styled">if set to <c>true</c> [styled].</param>
        /// <returns>
        /// A string.
        /// </returns>
        public string GetSelectedText(bool styled)
        {
            return this.control.SelectedText;
        }

        /// <summary>
        /// Replaces the entire document.
        /// </summary>
        /// <param name="text">The text.</param>
        public void SetText(string text)
        {
            this.control.Text = text;
        }

        /// <summary>
        /// Gets the entire document as either a styled or un-styled string.
        /// </summary>
        /// <param name="styled">if set to <c>true</c> [styled].</param>
        /// <returns>
        /// A string.
        /// </returns>
        public string GetText(bool styled)
        {
            return this.control.Text;
        }

        /// <summary>
        /// Gets the line number from the character index.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>
        /// A line number.
        /// </returns>
        public int GetLineFromPosition(int position)
        {
            return this.control.GetLineFromCharIndex(position);
        }

        /// <summary>
        /// Selects an entire line.
        /// </summary>
        /// <param name="startLine">The start line.</param>
        /// <param name="endLine">The end line.</param>
        public void SelectLines(int startLine, int endLine)
        {
            startLine = Math.Min(Math.Max(startLine, 0), this.control.Lines.Length - 1);
            endLine = Math.Min(Math.Max(endLine, startLine), this.control.Lines.Length - 1);

            var selectionStartPosition = this.control.GetFirstCharIndexFromLine(startLine);
            var selectionEndPosition = this.control.GetFirstCharIndexFromLine(endLine) + this.control.Lines[endLine].Length;
            var newCaret = this.GetCaretFrom(selectionStartPosition, selectionEndPosition);

            this.Caret = newCaret;
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
                         select
                             new TextLine(
                             i,
                             this.control.GetFirstCharIndexFromLine(i),
                             this.control.GetFirstCharIndexFromLine(i) + line.Length)).ToArray();
            var range = new TextRange(start, end - start, lines);

            return new CaretDetails(start, range, 0, 0);
        }

        /// <summary>
        /// Called when [mouse up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            this.OnCaretChanged();
        }

        /// <summary>
        /// Called when [key down].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (this.keyHandlers.ContainsKey(e.KeyData))
            {
                this.keyHandlers[e.KeyData](this, e);
            }
        }

        /// <summary>
        /// Called when [key up].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            this.OnCaretChanged();
        }

        /// <summary>
        /// Called when [caret changed].
        /// </summary>
        private void OnCaretChanged()
        {
            if (this.CaretChanged != null)
            {
                this.CaretChanged(this, EventArgs.Empty);
            }
        }
    }
}
