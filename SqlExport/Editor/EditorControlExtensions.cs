namespace SqlExport.Editor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using SqlExport.Logic;

    /// <summary>
    /// Defines the EditorControlExtensions class.
    /// </summary>
    public static class EditorControlExtensions
    {
        /// <summary>
        /// The command handlers
        /// </summary>
        private static readonly List<EditorControlCommandHandler> CommandHandlers =
            new List<EditorControlCommandHandler>();

        /// <summary>
        /// Initialises the specified editor control.
        /// </summary>
        /// <param name="editorControl">The editor control.</param>
        public static void Initialise(this IEditorControl editorControl)
        {
            editorControl.Control.Dock = DockStyle.Fill;
            editorControl.Control.MouseHover += (s, e) => editorControl.Control.Focus();
        }

        /// <summary>
        /// Sets the context menu.
        /// </summary>
        /// <param name="editorControl">The editor control.</param>
        /// <param name="contextMenu">The context menu.</param>
        public static void SetContextMenu(this IEditorControl editorControl, ContextMenuStrip contextMenu)
        {
            editorControl.Control.ContextMenuStrip = contextMenu;
        }

        /// <summary>
        /// Adds the key handler.
        /// </summary>
        /// <param name="editorControl">The editor control.</param>
        /// <param name="key">The key.</param>
        /// <param name="action">The action.</param>
        public static void AddKeyHandler(this IEditorControl editorControl, EditorKey key, Action action)
        {
            editorControl.AddKeyHandler(
                key,
                () =>
                {
                    action();
                    return true;
                });
        }

        /// <summary>
        /// Keys the handler.
        /// </summary>
        /// <param name="editorControl">The editor control.</param>
        /// <param name="key">The key.</param>
        /// <param name="action">A action to perform on the key press. Returns true if handled; otherwise, false..</param>
        public static void AddKeyHandler(this IEditorControl editorControl, EditorKey key, Func<bool> action)
        {
            if (key.CommandKey == Keys.None)
            {
                editorControl.AddKeyDownHandler(key.Key, (o, e) => e.Handled = action());
            }
            else
            {
                CommandHandlers.RemoveAll(c => !c.EditorControl.IsAlive);

                var commandHandler =
                    CommandHandlers.FirstOrDefault(
                        h => h.EditorControl.Target == editorControl && h.CommandKey.Equals(key.CommandKey));
                if (commandHandler == null)
                {
                    commandHandler = new EditorControlCommandHandler(editorControl, key.CommandKey);

                    CommandHandlers.Add(commandHandler);
                }

                commandHandler.AddKey(key.Key, action);
            }
        }

        /// <summary>
        /// Defines the EditorControlCommandHandler class.
        /// </summary>
        private class EditorControlCommandHandler
        {
            /// <summary>
            /// Indicates whether a command sequence has started.
            /// </summary>
            private bool isCommandSequence;

            /// <summary>
            /// The caret change count
            /// </summary>
            private int caretChangeCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="EditorControlCommandHandler"/> class.
            /// </summary>
            /// <param name="editorControl">The editor control.</param>
            /// <param name="commandKey">The command key.</param>
            public EditorControlCommandHandler(IEditorControl editorControl, EditorKey commandKey)
            {
                this.EditorControl = new WeakReference(editorControl);
                this.CommandKey = commandKey;

                editorControl.AddKeyDownHandler(
                    commandKey.Key,
                    (s, e) =>
                    {
                        this.isCommandSequence = true;
                        this.caretChangeCount = 0;
                    });
                editorControl.CaretChanged += (s, e) =>
                    {
                        // If the user has pressed the command sequence; count the caret changes.
                        if (this.isCommandSequence)
                        {
                            // If there has already been a caret change cancel the command sequence.
                            if (this.caretChangeCount > 1)
                            {
                                this.isCommandSequence = false;
                            }
                            else
                            {
                                this.caretChangeCount++;
                            }
                        }
                    };
            }

            /// <summary>
            /// Gets the editor control.
            /// </summary>
            public WeakReference EditorControl { get; private set; }

            /// <summary>
            /// Gets the command key.
            /// </summary>
            public EditorKey CommandKey { get; private set; }

            /// <summary>
            /// Adds the key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="action">The action.</param>
            public void AddKey(EditorKey key, Func<bool> action)
            {
                if (this.EditorControl.IsAlive)
                {
                    ((IEditorControl)this.EditorControl.Target).AddKeyDownHandler(
                        key.Key,
                        (o, e) =>
                        {
                            if (this.isCommandSequence)
                            {
                                e.Handled = action();
                            }
                        });
                }
            }
        }
    }
}
