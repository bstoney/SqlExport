namespace SqlExport.ViewModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading;
    using System.Windows.Threading;

    /////// <summary>
    /////// Defines the AsyncObservableCollection class.
    /////// </summary>
    /////// <typeparam name="TItem">The type of the item.</typeparam>
    ////public class AsyncObservableCollection<TItem> : ObservableCollection<TItem>
    ////{
    ////    /// <summary>
    ////    /// The synchronization context
    ////    /// </summary>
    ////    private readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current;

    ////    /// <summary>
    ////    /// Initializes a new instance of the <see cref="AsyncObservableCollection{TItem}"/> class.
    ////    /// </summary>
    ////    public AsyncObservableCollection()
    ////    {
    ////    }

    ////    /// <summary>
    ////    /// Initializes a new instance of the <see cref="AsyncObservableCollection{TItem}"/> class.
    ////    /// </summary>
    ////    /// <param name="list">The list.</param>
    ////    public AsyncObservableCollection(IEnumerable<TItem> list)
    ////        : base(list)
    ////    {
    ////    }

    ////    /// <summary>
    ////    /// Raises the <see cref="E:CollectionChanged" /> event.
    ////    /// </summary>
    ////    /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
    ////    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    ////    {
    ////        if (SynchronizationContext.Current == this.synchronizationContext)
    ////        {
    ////            // Execute the CollectionChanged event on the current thread
    ////            this.RaiseCollectionChanged(e);
    ////        }
    ////        else
    ////        {
    ////            // Post the CollectionChanged event on the creator thread
    ////            this.synchronizationContext.Post(this.RaiseCollectionChanged, e);
    ////        }
    ////    }

    ////    /// <summary>
    ////    /// Raises the collection changed.
    ////    /// </summary>
    ////    /// <param name="param">The parameter.</param>
    ////    private void RaiseCollectionChanged(object param)
    ////    {
    ////        // We are in the creator thread, call the base implementation directly
    ////        base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
    ////    }

    ////    /// <summary>
    ////    /// Raises the <see cref="E:PropertyChanged" /> event.
    ////    /// </summary>
    ////    /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
    ////    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    ////    {
    ////        if (SynchronizationContext.Current == this.synchronizationContext)
    ////        {
    ////            // Execute the PropertyChanged event on the current thread
    ////            this.RaisePropertyChanged(e);
    ////        }
    ////        else
    ////        {
    ////            // Post the PropertyChanged event on the creator thread
    ////            this.synchronizationContext.Post(this.RaisePropertyChanged, e);
    ////        }
    ////    }

    ////    /// <summary>
    ////    /// Raises the property changed.
    ////    /// </summary>
    ////    /// <param name="param">The parameter.</param>
    ////    private void RaisePropertyChanged(object param)
    ////    {
    ////        // We are in the creator thread, call the base implementation directly
    ////        base.OnPropertyChanged((PropertyChangedEventArgs)param);
    ////    }
    ////}

    /// <summary>
    /// The safe observable.
    /// </summary>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class SafeObservable<TItem> : IList<TItem>, INotifyCollectionChanged
    {
        #region Fields

        /// <summary>
        /// The collection.
        /// </summary>
        private readonly IList<TItem> collection = new List<TItem>();

        /// <summary>
        /// The dispatcher.
        /// </summary>
        private readonly Dispatcher dispatcher;

        /// <summary>
        /// The sync.
        /// </summary>
        private readonly ReaderWriterLock sync = new ReaderWriterLock();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeObservable{TItem}"/> class.
        /// </summary>
        public SafeObservable()
        {
            this.dispatcher = Dispatcher.CurrentDispatcher;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// The collection changed.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the count.
        /// </summary>
        public int Count
        {
            get
            {
                this.sync.AcquireReaderLock(Timeout.Infinite);
                int result = this.collection.Count;
                this.sync.ReleaseReaderLock();
                return result;
            }
        }

        /// <summary>
        /// Gets a value indicating whether is read only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this.collection.IsReadOnly;
            }
        }

        #endregion

        #region Public Indexers

        /// <summary>
        /// The this.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <returns>
        /// The T.
        /// </returns>
        public TItem this[int index]
        {
            get
            {
                this.sync.AcquireReaderLock(Timeout.Infinite);
                TItem result = this.collection[index];
                this.sync.ReleaseReaderLock();
                return result;
            }

            set
            {
                this.sync.AcquireWriterLock(Timeout.Infinite);
                if (this.collection.Count == 0 || this.collection.Count <= index)
                {
                    this.sync.ReleaseWriterLock();
                    return;
                }

                this.collection[index] = value;
                this.sync.ReleaseWriterLock();
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The add.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Add(TItem item)
        {
            if (Thread.CurrentThread == this.dispatcher.Thread)
            {
                this.DoAdd(item);
            }
            else
            {
                this.dispatcher.BeginInvoke((Action)(() => this.DoAdd(item)));
            }
        }

        /// <summary>
        /// The clear.
        /// </summary>
        public void Clear()
        {
            if (Thread.CurrentThread == this.dispatcher.Thread)
            {
                this.DoClear();
            }
            else
            {
                this.dispatcher.BeginInvoke((Action)(this.DoClear));
            }
        }

        /// <summary>
        /// The contains.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public bool Contains(TItem item)
        {
            this.sync.AcquireReaderLock(Timeout.Infinite);
            bool result = this.collection.Contains(item);
            this.sync.ReleaseReaderLock();
            return result;
        }

        /// <summary>
        /// The copy to.
        /// </summary>
        /// <param name="array">
        /// The array.
        /// </param>
        /// <param name="arrayIndex">
        /// The array index.
        /// </param>
        public void CopyTo(TItem[] array, int arrayIndex)
        {
            this.sync.AcquireWriterLock(Timeout.Infinite);
            this.collection.CopyTo(array, arrayIndex);
            this.sync.ReleaseWriterLock();
        }

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The System.Collections.Generic.IEnumerator`1[T -&gt; T].
        /// </returns>
        public IEnumerator<TItem> GetEnumerator()
        {
            this.sync.AcquireReaderLock(Timeout.Infinite);
            var items = this.collection.ToArray();
            this.sync.ReleaseReaderLock();
            return items.OfType<TItem>().GetEnumerator();
        }

        /// <summary>
        /// The index of.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The index.
        /// </returns>
        public int IndexOf(TItem item)
        {
            this.sync.AcquireReaderLock(Timeout.Infinite);
            int result = this.collection.IndexOf(item);
            this.sync.ReleaseReaderLock();
            return result;
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        public void Insert(int index, TItem item)
        {
            if (Thread.CurrentThread == this.dispatcher.Thread)
            {
                this.DoInsert(index, item);
            }
            else
            {
                this.dispatcher.BeginInvoke((Action)(() => this.DoInsert(index, item)));
            }
        }

        /// <summary>
        /// The remove.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        public bool Remove(TItem item)
        {
            if (Thread.CurrentThread == this.dispatcher.Thread)
            {
                return this.DoRemove(item);
            }
            else
            {
                DispatcherOperation op = this.dispatcher.BeginInvoke(new Func<TItem, bool>(this.DoRemove), item);
                if (op == null || op.Result == null)
                {
                    return false;
                }

                return (bool)op.Result;
            }
        }

        /// <summary>
        /// The remove at.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        public void RemoveAt(int index)
        {
            if (Thread.CurrentThread == this.dispatcher.Thread)
            {
                this.DoRemoveAt(index);
            }
            else
            {
                this.dispatcher.BeginInvoke((Action)(() => this.DoRemoveAt(index)));
            }
        }

        #endregion

        #region Explicit Interface Methods

        /// <summary>
        /// The get enumerator.
        /// </summary>
        /// <returns>
        /// The System.Collections.IEnumerator.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            this.sync.AcquireReaderLock(Timeout.Infinite);
            var items = this.collection.ToArray();
            this.sync.ReleaseReaderLock();
            return items.GetEnumerator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The do add.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        private void DoAdd(TItem item)
        {
            this.sync.AcquireWriterLock(Timeout.Infinite);
            this.collection.Add(item);
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(
                    this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            }

            this.sync.ReleaseWriterLock();
        }

        /// <summary>
        /// The do clear.
        /// </summary>
        private void DoClear()
        {
            this.sync.AcquireWriterLock(Timeout.Infinite);
            this.collection.Clear();
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            this.sync.ReleaseWriterLock();
        }

        /// <summary>
        /// The do insert.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        private void DoInsert(int index, TItem item)
        {
            this.sync.AcquireWriterLock(Timeout.Infinite);
            this.collection.Insert(index, item);
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(
                    this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            }

            this.sync.ReleaseWriterLock();
        }

        /// <summary>
        /// The do remove.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The System.Boolean.
        /// </returns>
        private bool DoRemove(TItem item)
        {
            this.sync.AcquireWriterLock(Timeout.Infinite);
            int index = this.collection.IndexOf(item);
            if (index == -1)
            {
                this.sync.ReleaseWriterLock();
                return false;
            }

            bool result = this.collection.Remove(item);
            if (result && this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            this.sync.ReleaseWriterLock();
            return result;
        }

        /// <summary>
        /// The do remove at.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        private void DoRemoveAt(int index)
        {
            this.sync.AcquireWriterLock(Timeout.Infinite);
            if (this.collection.Count == 0 || this.collection.Count <= index)
            {
                this.sync.ReleaseWriterLock();
                return;
            }

            this.collection.RemoveAt(index);
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }

            this.sync.ReleaseWriterLock();
        }

        #endregion
    }
}