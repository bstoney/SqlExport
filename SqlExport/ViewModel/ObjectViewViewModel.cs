namespace SqlExport.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Reactive.Linq;
    using System.Text;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using SqlExport.Common;
    using SqlExport.Common.Data;
    using SqlExport.SampleData;

    /// <summary>
    /// Defines the ObjectViewViewModel class.
    /// </summary>
    public class ObjectViewViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="SearchText" /> property's name.
        /// </summary>
        public const string SearchTextPropertyName = "SearchText";

        /// <summary>
        /// The search text
        /// </summary>
        private string searchText = null;

        /// <summary>
        /// The search command
        /// </summary>
        private RelayCommand searchCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectViewViewModel"/> class.
        /// </summary>
        public ObjectViewViewModel()
        {
            this.Connections = new ObservableCollection<ObjectViewItemViewModel>();

            if (this.IsInDesignMode)
            {
                SampleConnectionAdapter.Initialise();

                this.Connections.Add(
                    new DatabaseDetails
                        {
                            Name = "Demo Data",
                            Type = SampleConnectionAdapter.SampleConnectionAdapterName
                        });

                this.Connections[0].IsSelected = true;
            }

            // Listen to all property change events on SearchText
            var searchTextChanged =
                Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    ev => PropertyChanged += ev, ev => PropertyChanged -= ev)
                          .Where(ev => ev.EventArgs.PropertyName == SearchTextPropertyName);

            // Transform the event stream into a stream of strings (the input values)
            var input = searchTextChanged
                .Throttle(TimeSpan.FromMilliseconds(400))
                .Select(args => this.SearchText);

            // Log all events in the event stream to the Log viewer
            input.ObserveOn(DispatcherScheduler.Current).Subscribe(e => this.Search());
        }

        /// <summary>
        /// Gets the connections.
        /// </summary>
        public ObservableCollection<ObjectViewItemViewModel> Connections { get; private set; }

        /// <summary>
        /// Gets the search command.
        /// </summary>
        public RelayCommand SearchCommand
        {
            get
            {
                return this.searchCommand ?? (this.searchCommand = new RelayCommand(this.Search));
            }
        }

        /// <summary>
        /// Gets or sets the SearchText property.
        /// </summary>
        public string SearchText
        {
            get
            {
                return this.searchText;
            }

            set
            {
                if (this.searchText == value)
                {
                    return;
                }

                this.searchText = value;
                this.RaisePropertyChanged(SearchTextPropertyName);
            }
        }

        /// <summary>
        /// Updates the visibility.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>
        /// A list ObjectViewItemViewModel.
        /// </returns>
        private static IEnumerable<ObjectViewItemViewModel> Flatten(IEnumerable<ObjectViewItemViewModel> items)
        {
            if (items == null)
            {
                return Enumerable.Empty<ObjectViewItemViewModel>();
            }

            var query = from i in items
                        from c in new[] { i }.Concat(Flatten(i.Items))
                        select c;

            return query;
        }

        /// <summary>
        /// Search the connections for the search text.
        /// </summary>
        private void Search()
        {
            Func<ObjectViewItemViewModel, bool> isMatch = i =>
                string.IsNullOrEmpty(this.SearchText) ||
                i.Name.IndexOf(this.SearchText, StringComparison.InvariantCultureIgnoreCase) >= 0;

            var items = Flatten(this.Connections).ToList();
            items.ForEach(i => i.IsVisible = false);
            items.Where(isMatch).ToList().ForEach(i => i.IsVisible = true);
        }
    }
}
