using System;
using System.Windows.Forms;

namespace SqlExport.Editor
{
    /// <summary>
    /// Defines an interface for the editor control.
    /// </summary>
    public interface IEditorControl
    {
        /// <summary>
        /// Occurs when the text is changed by the user.
        /// </summary>
        event EventHandler TextChanged;

        /// <summary>
        /// Occurs when the user moves the text caret.
        /// </summary>
        event EventHandler CaretChanged;

        /// <summary>
        /// Gets or sets the location of the caret and selection.
        /// </summary>
        CaretDetails Caret { get; set; }

        /// <summary>
        /// Gets editor control.
        /// </summary>
        Control Control { get; }

        /// <summary>
        /// Gets a value indicating whether or not the editor can undo the last action.
        /// </summary>
        bool CanUndo { get; }

        /// <summary>
        /// Gets a value indicating whether or not the editor can redo the last action.
        /// </summary>
        bool CanRedo { get; }

        /// <summary>
        /// Gets a value indicating whether or not the editor can inset the current clipboard contents.
        /// </summary>
        bool CanPaste { get; }

        /// <summary>
        /// Adds key mapping to a handler.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="handler">The handler.</param>
        void AddKeyDownHandler(Keys key, KeyEventHandler handler);

        /// <summary>
        /// Sets the editors style configuration.
        /// </summary>
        /// <param name="editorStyle">The editor style.</param>
        void SetEditorStyle(IEditorStyleConfiguration editorStyle);

        /// <summary>
        /// Undoes the last action.
        /// </summary>
        void Undo();

        /// <summary>
        /// Redoes the last undo.
        /// </summary>
        void Redo();

        /// <summary>
        /// Copies the currently selected text to the clipboard.
        /// </summary>
        void Copy();

        /// <summary>
        /// Moves the currently selected text to the clipboard.
        /// </summary>
        void Cut();

        /// <summary>
        /// Inserts the current clipboard content.
        /// </summary>
        void Paste();

        /// <summary>
        /// Selects the entire document.
        /// </summary>
        void SelectAll();

        /// <summary>
        /// Replaces the selected text.
        /// </summary>
        /// <param name="text">The text.</param>
        void SetSelectedText(string text);

        /// <summary>
        /// Gets the selected text as either a styled or un-styled string.
        /// </summary>
        /// <param name="styled">if set to <c>true</c> [styled].</param>
        /// <returns>A string.</returns>
        string GetSelectedText(bool styled);

        /// <summary>
        /// Replaces the entire document.
        /// </summary>
        /// <param name="text">The text.</param>
        void SetText(string text);

        /// <summary>
        /// Gets the entire document as either a styled or un-styled string.
        /// </summary>
        /// <param name="styled">if set to <c>true</c> [styled].</param>
        /// <returns>A string.</returns>
        string GetText(bool styled);

        /// <summary>
        /// Gets the line number from the character index.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>A line number.</returns>
        int GetLineFromPosition(int position);

        /// <summary>
        /// Selects all the lines.
        /// </summary>
        /// <param name="startLine">The start line.</param>
        /// <param name="endLine">The end line.</param>
        void SelectLines(int startLine, int endLine);
    }
}
