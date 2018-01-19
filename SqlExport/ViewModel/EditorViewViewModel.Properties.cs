namespace SqlExport.ViewModel
{
    using GalaSoft.MvvmLight.Command;

    using SqlExport.Editor;

    /// <summary>
    /// Defines the EditorViewViewModel class.
    /// </summary>
    public partial class EditorViewViewModel
    {
        /// <summary>
        /// The <see cref="IsFinding" /> property's name.
        /// </summary>
        public const string IsFindingPropertyName = "IsFinding";

        /// <summary>
        /// The <see cref="HasChanged" /> property's name.
        /// </summary>
        public const string HasChangedPropertyName = "HasChanged";

        /// <summary>
        /// The <see cref="AllText" /> property's name.
        /// </summary>
        public const string AllTextPropertyName = "AllText";

        /// <summary>
        /// The <see cref="Caret" /> property's name.
        /// </summary>
        public const string CaretPropertyName = "Caret";

        /// <summary>
        /// The <see cref="FindText" /> property's name.
        /// </summary>
        public const string FindTextPropertyName = "FindText";

        /// <summary>
        /// The is finding.
        /// </summary>
        private bool isFinding = false;

        /// <summary>
        /// The caret
        /// </summary>
        private CaretDetails caret;

        /// <summary>
        /// All text.
        /// </summary>
        private string allText = null;

        /// <summary>
        /// The has changed.
        /// </summary>
        private bool hasChanged = false;

        /// <summary>
        /// The find text
        /// </summary>
        private string findText = null;

        /// <summary>
        /// The undo command
        /// </summary>
        private RelayCommand undoCommand;

        /// <summary>
        /// The redo command
        /// </summary>
        private RelayCommand redoCommand;

        /// <summary>
        /// The cut command
        /// </summary>
        private RelayCommand cutCommand;

        /// <summary>
        /// The copy command
        /// </summary>
        private RelayCommand<string> copyCommand;

        /// <summary>
        /// The paste command
        /// </summary>
        private RelayCommand<string> pasteCommand;

        /// <summary>
        /// The select all command
        /// </summary>
        private RelayCommand selectAllCommand;

        /// <summary>
        /// The auto format command
        /// </summary>
        private RelayCommand autoFormatCommand;

        /// <summary>
        /// The delete command
        /// </summary>
        private RelayCommand deleteCommand;

        /// <summary>
        /// The find command
        /// </summary>
        private RelayCommand findCommand;

        /// <summary>
        /// The cancel find command
        /// </summary>
        private RelayCommand cancelFindCommand;

        /// <summary>
        /// Gets the UndoCommand.
        /// </summary>
        public RelayCommand UndoCommand
        {
            get
            {
                return this.undoCommand
                       ?? (this.undoCommand =
                           new RelayCommand(() => this.EditorControl.Undo(), () => this.EditorControl.CanUndo));
            }
        }

        /// <summary>
        /// Gets the RedoCommand.
        /// </summary>
        public RelayCommand RedoCommand
        {
            get
            {
                return this.redoCommand
                       ?? (this.redoCommand =
                           new RelayCommand(() => this.EditorControl.Redo(), () => this.EditorControl.CanRedo));
            }
        }

        /// <summary>
        /// Gets the CutCommand.
        /// </summary>
        public RelayCommand CutCommand
        {
            get
            {
                return this.cutCommand
                       ?? (this.cutCommand =
                           new RelayCommand(() => this.editorControl.Cut(), () => this.Caret.Length > 0));
            }
        }

        /// <summary>
        /// Gets the CopyCommand.
        /// </summary>
        public RelayCommand<string> CopyCommand
        {
            get
            {
                return this.copyCommand
                       ?? (this.copyCommand = new RelayCommand<string>(this.Copy, p => this.caret.Length > 0));
            }
        }

        /// <summary>
        /// Gets the PasteCommand.
        /// </summary>
        public RelayCommand<string> PasteCommand
        {
            get
            {
                return this.pasteCommand
                       ?? (this.pasteCommand = new RelayCommand<string>(this.Paste, p => this.EditorControl.CanPaste));
            }
        }

        /// <summary>
        /// Gets the DeleteCommand.
        /// </summary>
        public RelayCommand DeleteCommand
        {
            get
            {
                return this.deleteCommand
                       ?? (this.deleteCommand =
                           new RelayCommand(
                               () => this.EditorControl.SetSelectedText(string.Empty),
                               () => this.EditorControl.GetSelectedText(false) != string.Empty));
            }
        }

        /// <summary>
        /// Gets the SelectAllCommand.
        /// </summary>
        public RelayCommand SelectAllCommand
        {
            get
            {
                return this.selectAllCommand
                       ?? (this.selectAllCommand = new RelayCommand(() => this.editorControl.SelectAll()));
            }
        }

        /// <summary>
        /// Gets the AutoFormatCommand.
        /// </summary>
        public RelayCommand AutoFormatCommand
        {
            get
            {
                return this.autoFormatCommand ?? (this.autoFormatCommand = new RelayCommand(this.AutoFormat));
            }
        }

        /// <summary>
        /// Gets the FindCommand.
        /// </summary>
        public RelayCommand FindCommand
        {
            get
            {
                return this.findCommand ?? (this.findCommand = new RelayCommand(this.Find));
            }
        }

        /// <summary>
        /// Gets the CancelFindCommand.
        /// </summary>
        public RelayCommand CancelFindCommand
        {
            get
            {
                return this.cancelFindCommand
                       ?? (this.cancelFindCommand = new RelayCommand(() => this.IsFinding = false));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has changed.
        /// </summary>
        public bool HasChanged
        {
            get
            {
                return this.hasChanged;
            }

            set
            {
                if (this.hasChanged == value)
                {
                    return;
                }

                this.hasChanged = value;
                this.RaisePropertyChanged(HasChangedPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the AllText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string AllText
        {
            get
            {
                return this.allText;
            }

            set
            {
                if (this.allText == value)
                {
                    return;
                }

                this.allText = value;
                if (!this.isUpdating)
                {
                    this.editorControl.SetText(value);
                }

                this.RaisePropertyChanged(AllTextPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the caret.
        /// </summary>
        /// <value>
        /// The caret.
        /// </value>
        public CaretDetails Caret
        {
            get
            {
                return this.caret;
            }

            set
            {
                if (this.caret == value)
                {
                    return;
                }

                this.caret = value;
                if (!this.isUpdating)
                {
                    this.editorControl.Caret = value;
                }

                this.RaisePropertyChanged(CaretPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is finding.
        /// </summary>
        public bool IsFinding
        {
            get
            {
                return this.isFinding;
            }

            set
            {
                if (this.isFinding == value)
                {
                    return;
                }

                this.isFinding = value;
                this.RaisePropertyChanged(IsFindingPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets the FindText property.
        /// </summary>
        public string FindText
        {
            get
            {
                return this.findText;
            }

            set
            {
                if (this.findText == value)
                {
                    return;
                }

                this.findText = value;
                this.RaisePropertyChanged(FindTextPropertyName);
            }
        }
    }
}