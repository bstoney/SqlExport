namespace SqlExport.ViewModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Threading;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.Data;
    using SqlExport.Logic;
    using SqlExport.Properties;

    /// <summary>
    /// Defines the ObjectViewItem class.
    /// </summary>
    public partial class ObjectViewItemViewModel : ViewModelBase
    {
        /// <summary>
        /// The default sort order.
        /// </summary>
        public const string DefaultSortOrder = "Default";

        /// <summary>
        /// The alphanumeric sort order
        /// </summary>
        public const string AlphanumericSortOrder = "Alphanumeric";

        /// <summary>
        /// The parent
        /// </summary>
        private readonly ObjectViewItemViewModel parent;

        /// <summary>
        /// The database
        /// </summary>
        private readonly DatabaseDetails database;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewItemViewModel" /> class.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="name">The name.</param>
        /// <param name="index">The index.</param>
        /// <param name="parent">The parent.</param>
        private ObjectViewItemViewModel(DatabaseDetails database, string name, int index, ObjectViewItemViewModel parent = null)
        {
            this.database = database;
            this.parent = parent;
            this.Name = name;
            this.DefaultIndex = index;

            this.PropertyChanged += this.ObjectViewItemViewModelPropertyChanged;

            if (this.IsInDesignMode)
            {
                this.Items = new ObservableCollection<ObjectViewItemViewModel>();
            }
        }

        /// <summary>
        /// Objects the view item.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <returns>A object view item.</returns>
        public static implicit operator ObjectViewItemViewModel(DatabaseDetails database)
        {
            var itemViewModel = new ObjectViewItemViewModel(database, database.Name, 0)
            {
                SchemaItem = null,
                QueryText = string.Empty,
                IsExpanded = true
            };

            itemViewModel.StartLoadChildren();

            return itemViewModel;
        }

        /// <summary>
        /// Gets the items as text.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>A string.</returns>
        public static string GetItemsAsText(IEnumerable<ObjectViewItemViewModel> items)
        {
            // Get a list of the selected items and their appropriate delimiters
            var dragTextQuery = from d in items
                                select new
                                {
                                    d.QueryText,
                                    Delimiter = d.SchemaItem != null && d.SchemaItem.SchemaItemType == SchemaItemType.Column
                                        ? ", "
                                        : Environment.NewLine
                                };

            // Join all the items using the delimiter provided. 
            return dragTextQuery.Aggregate(
                (string)null, (a, i) => a == null ? i.QueryText : a + i.Delimiter + i.QueryText);
        }

        /// <summary>
        /// Gets the items from schema.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="schemaItems">The schema items.</param>
        /// <returns>
        /// A list of Items.
        /// </returns>
        private static IEnumerable<ObjectViewItemViewModel> GetItemsFromSchema(ObjectViewItemViewModel parent, IEnumerable<ISchemaItem> schemaItems)
        {
            foreach (var s in schemaItems.Select((s, i) => new { Item = s, Index = i }))
            {
                var item = new ObjectViewItemViewModel(parent.database, s.Item.DisplayName, s.Index, parent)
                               {
                                   SchemaItem = s.Item,
                                   QueryText = s.Item.QueryText
                               };

                if (s.Item.Children != null && s.Item.Children.Any())
                {
                    var children = GetItemsFromSchema(item, s.Item.Children);
                    parent.InvokeOnDispatcher(() => item.Items = new ObservableCollection<ObjectViewItemViewModel>(children));
                }

                yield return item;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Handles the PropertyChanged event of the ObjectViewItemViewModel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void ObjectViewItemViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case ObjectViewItemViewModel.IsExpandedPropertyName:
                    this.SortCommand.RaiseCanExecuteChanged();
                    break;
                case ObjectViewItemViewModel.IsLoadingPropertyName:
                    this.LoadCommand.RaiseCanExecuteChanged();
                    break;
                case ObjectViewItemViewModel.ItemsPropertyName:
                    this.GetItemListCommand.RaiseCanExecuteChanged();
                    this.SortCommand.RaiseCanExecuteChanged();
                    break;
            }
        }

        /// <summary>
        /// Determines whether this instance can load.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can load; otherwise, <c>false</c>.
        /// </returns>
        private bool CanLoad()
        {
            using (var schema = this.database.GetSchemaAdapter())
            {
                return schema != null && !this.IsLoading;
            }
        }

        /// <summary>
        /// Starts the loads of the child items.
        /// </summary>
        private void StartLoadChildren()
        {
            if (this.IsLoading)
            {
                return;
            }

            Task.Factory.StartNew(this.LoadChildren);
        }

        /// <summary>
        /// Loads the children.
        /// </summary>
        private void LoadChildren()
        {
            try
            {
                this.InvokeOnDispatcher(() => this.IsLoading = true);
                using (var schema = this.database.GetSchemaAdapter())
                {
                    if (schema != null)
                    {
                        var children = this.GetItemsFromSchema(schema);
                        this.InvokeOnDispatcher(
                            () => this.Items = new ObservableCollection<ObjectViewItemViewModel>(children));
                    }
                }
            }
            catch (Exception exp)
            {
                ErrorDialogLogic.AddError(exp);
                ErrorDialogLogic.ShowForm();
            }
            finally
            {
                this.InvokeOnDispatcher(() => this.IsLoading = false);
            }
        }

        /// <summary>
        /// Loads the children asynchronously.
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <returns>
        /// A task.
        /// </returns>
        private IEnumerable<ObjectViewItemViewModel> GetItemsFromSchema(ISchemaAdapter schema)
        {
            if (this.parent == null)
            {
                return from s in schema.GetSections().Select((s, i) => new { Section = s, Index = i })
                       select new ObjectViewItemViewModel(this.database, s.Section, s.Index, this)
                       {
                           SchemaItem = (ISchemaItem)new SchemaItem(s.Section, SchemaItemType.Folder),
                           QueryText = string.Empty,
                       };
            }

            var schemaItems = schema.PopulateFromPath(this.GetPathFromNode().ToArray());
            return GetItemsFromSchema(this, schemaItems);
        }

        /// <summary>
        /// Gets the path from node.
        /// </summary>
        /// <returns>A path list.</returns>
        private IEnumerable<string> GetPathFromNode()
        {
            if (this.parent != null)
            {
                return this.parent.GetPathFromNode().Concat(new[] { this.Name });
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Determines whether this instance can be removed from its parent.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can be removed from its parent; otherwise, <c>false</c>.
        /// </returns>
        private bool CanRemove()
        {
            return this.parent != null;
        }

        /// <summary>
        /// Removes this instance from its parent.
        /// </summary>
        private void Remove()
        {
            if (this.parent != null)
            {
                this.parent.Items.Remove(this);
            }
        }

        /// <summary>
        /// Gets the source.
        /// </summary>
        private void GetSource()
        {
            string script = null;
            try
            {
                using (var schema = this.database.GetSchemaAdapter())
                {
                    script = schema.GetSchemaItemScript(this.GetPathFromNode().ToArray());
                }
            }
            catch (Exception exp)
            {
                ErrorDialogLogic.AddError(exp);
                ErrorDialogLogic.ShowForm();
            }

            if (script != null)
            {
                Clipboard.SetDataObject(script);
            }
        }

        /// <summary>
        /// Sorts the specified sort order.
        /// </summary>
        /// <param name="sortOrder">The sort order.</param>
        private void Sort(string sortOrder)
        {
            IEnumerable<ObjectViewItemViewModel> sortedItems;
            switch (sortOrder)
            {
                case AlphanumericSortOrder:
                    sortedItems = from i in this.Items.ToArray()
                                  orderby i.Name
                                  select i;
                    break;
                default:
                    sortedItems = from i in this.Items.ToArray()
                                  orderby i.DefaultIndex
                                  select i;
                    break;
            }

            this.Items = new ObservableCollection<ObjectViewItemViewModel>(sortedItems);
        }

        /// <summary>
        /// Determines whether this instance can sort.
        /// </summary>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>
        ///   <c>true</c> if this instance can sort; otherwise, <c>false</c>.
        /// </returns>
        private bool CanSort(string sortOrder)
        {
            return this.Items != null && this.Items.Any() && this.IsExpanded
                   && (sortOrder == DefaultSortOrder || sortOrder == AlphanumericSortOrder);
        }

        /// <summary>
        /// Determines whether this instance [can get item list].
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance [can get item list]; otherwise, <c>false</c>.
        /// </returns>
        private bool CanGetItemList()
        {
            return this.Items == null || this.Items.Any();
        }

        /// <summary>
        /// Gets the item list.
        /// </summary>
        private void GetItemList()
        {
            if (this.Items == null)
            {
                this.LoadChildren();
            }

            var itemsText = GetItemsAsText(this.Items);
            if (!string.IsNullOrWhiteSpace(itemsText))
            {
                Clipboard.SetDataObject(itemsText);
            }
        }
    }
}