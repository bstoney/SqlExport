namespace SqlExport.ViewModel
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Threading;
    using System.Windows.Threading;

    /// <summary>
    /// The safe observable.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class SafeObservable<T> : IList<T>, INotifyCollectionChanged
    {
        #region Fields

        /// <summary>
        /// The collection.
        /// </summary>
        private readonly IList<T> collection = new List<T>();

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
        /// Initializes a new instance of the <see cref="SafeObservable{T}"/> class.
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
        public T this[int index]
        {
            get
            {
                this.sync.AcquireReaderLock(Timeout.Infinite);
                T result = this.collection[index];
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
        public void Add(T item)
        {
            if (Thread.CurrentThread == this.dispatcher.Thread)
            {
                this.DoAdd(item);
            }
            else
            {
                this.dispatcher.BeginInvoke((Action)(() => { this.DoAdd(item); }));
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
                this.dispatcher.BeginInvoke((Action)(() => { this.DoClear(); }));
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
        public bool Contains(T item)
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
        public void CopyTo(T[] array, int arrayIndex)
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
        public IEnumerator<T> GetEnumerator()
        {
            return this.collection.GetEnumerator();
        }

        /// <summary>
        /// The index of.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The System.Int32.
        /// </returns>
        public int IndexOf(T item)
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
        public void Insert(int index, T item)
        {
            if (Thread.CurrentThread == this.dispatcher.Thread)
            {
                this.DoInsert(index, item);
            }
            else
            {
                this.dispatcher.BeginInvoke((Action)(() => { this.DoInsert(index, item); }));
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
        public bool Remove(T item)
        {
            if (Thread.CurrentThread == this.dispatcher.Thread)
            {
                return this.DoRemove(item);
            }
            else
            {
                DispatcherOperation op = this.dispatcher.BeginInvoke(new Func<T, bool>(this.DoRemove), item);
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
                this.dispatcher.BeginInvoke((Action)(() => { this.DoRemoveAt(index); }));
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
            return this.collection.GetEnumerator();
        }

        #endregion

        #region Methods

        /// <summary>
        /// The do add.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        private void DoAdd(T item)
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
        private void DoInsert(int index, T item)
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
        private bool DoRemove(T item)
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