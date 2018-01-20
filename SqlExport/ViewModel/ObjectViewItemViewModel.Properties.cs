namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Data;
    using SqlExport.Properties;

    /// <summary>
    /// Defines the ObjectViewItem class.
    /// </summary>
    public partial class ObjectViewItemViewModel
    {
        /// <summary>
        /// The <see cref="Items" /> property's name.
        /// </summary>
        public const string ItemsPropertyName = "Items";

        /// <summary>
        /// The <see cref="IsExpanded" /> property's name.
        /// </summary>
        public const string IsExpandedPropertyName = "IsExpanded";

        /// <summary>
        /// The <see cref="IsSelected" /> property's name.
        /// </summary>
        public const string IsSelectedPropertyName = "IsSelected";

        /// <summary>
        /// The <see cref="IsVisible" /> property's name.
        /// </summary>
        public const string IsVisiblePropertyName = "IsVisible";

        /// <summary>
        /// The <see cref="IsLoading" /> property's name.
        /// </summary>
        public const string IsLoadingPropertyName = "IsLoading";

        /// <summary>
        /// Indicates that info is loading.
        /// </summary>
        private bool isLoading = false;

        /// <summary>
        /// Indicates whether the item is visible.
        /// </summary>
        private bool isVisible = true;

        /// <summary>
        /// Indicates whether the item is selected.
        /// </summary>
        private bool isSelected = false;

        /// <summary>
        /// Indicates whether the item is expanded.
        /// </summary>
        private bool isExpanded = false;

        /// <summary>
        /// The items
        /// </summary>
        private ObservableCollection<ObjectViewItemViewModel> items = null;

        /// <summary>
        /// The load command
        /// </summary>
        private RelayCommand loadCommand;

        /// <summary>
        /// The remove command
        /// </summary>
        private RelayCommand removeCommand;

        /// <summary>
        /// The get item list command
        /// </summary>
        private RelayCommand getItemListCommand;

        /// <summary>
        /// The get source command
        /// </summary>
        private RelayCommand getSourceCommand;

        /// <summary>
        /// The sort command
        /// </summary>
        private RelayCommand<string> sortCommand;

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the default index.
        /// </summary>
        public int DefaultIndex { get; private set; }

        /// <summary>
        /// Gets the image style key.
        /// </summary>
        public string ImageStyleKey
        {
            get
            {
                if (this.SchemaItem == null)
                {
                    return "SchemaServerStyle";
                }

                return "Schema" + this.SchemaItem.SchemaItemType + "Style";
            }
        }

        /// <summary>
        /// Gets the schema item.
        /// </summary>
        public ISchemaItem SchemaItem { get; private set; }

        /// <summary>
        /// Gets the query text.
        /// </summary>
        public string QueryText { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is expanded.
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return this.isExpanded;
            }

            set
            {
                if (this.isExpanded == value)
                {
                    return;
                }

                this.isExpanded = value;
                this.RaisePropertyChanged(IsExpandedPropertyName);

                // Expand all the way up to the root.
                if (this.isExpanded && this.parent != null)
                {
                    this.parent.IsExpanded = true;
                }

                // Lazy load the child items, if necessary.
                if (this.Items == null)
                {
                    this.StartLoadChildren();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this item is selected.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }

            set
            {
                if (this.isSelected == value)
                {
                    return;
                }

                this.isSelected = value;
                this.RaisePropertyChanged(IsSelectedPropertyName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is visible.
        /// </summary>
        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }

            set
            {
                if (this.isVisible == value)
                {
                    return;
                }

                this.isVisible = value;
                this.RaisePropertyChanged(IsVisiblePropertyName);

                if (this.isVisible && this.parent != null)
                {
                    this.parent.IsVisible = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is loading.
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                if (this.IsLoading == value)
                {
                    return;
                }

                this.isLoading = value;
                this.RaisePropertyChanged(IsLoadingPropertyName);

                if (this.parent != null)
                {
                    this.parent.IsLoading = this.isLoading;
                }
            }
        }

        /// <summary>
        /// Gets or sets the Items property.
        /// </summary>
        public ObservableCollection<ObjectViewItemViewModel> Items
        {
            get
            {
                return this.items;
            }

            set
            {
                if (this.items == value)
                {
                    return;
                }

                this.items = value;
                this.RaisePropertyChanged(ItemsPropertyName);
            }
        }

        /// <summary>
        /// Gets the LoadCommand.
        /// </summary>
        public RelayCommand LoadCommand
        {
            get
            {
                return this.loadCommand
                       ?? (this.loadCommand = new RelayCommand(this.StartLoadChildren, this.CanLoad));
            }
        }

        /// <summary>
        /// Gets the RemoveCommand.
        /// </summary>
        public RelayCommand RemoveCommand
        {
            get
            {
                return this.removeCommand
                       ?? (this.removeCommand = new RelayCommand(this.Remove, this.CanRemove));
            }
        }

        /// <summary>
        /// Gets the GetItemListCommand.
        /// </summary>
        public RelayCommand GetItemListCommand
        {
            get
            {
                return this.getItemListCommand
                       ?? (this.getItemListCommand = new RelayCommand(this.GetItemList, this.CanGetItemList));
            }
        }

        /// <summary>
        /// Gets the GetSourceCommand.
        /// </summary>
        public RelayCommand GetSourceCommand
        {
            get
            {
                return this.getSourceCommand ?? (this.getSourceCommand = new RelayCommand(this.GetSource, () => false));
            }
        }

        /// <summary>
        /// Gets the SortCommand.
        /// </summary>
        public RelayCommand<string> SortCommand
        {
            get
            {
                return this.sortCommand ?? (this.sortCommand = new RelayCommand<string>(this.Sort, this.CanSort));
            }
        }
    }
}