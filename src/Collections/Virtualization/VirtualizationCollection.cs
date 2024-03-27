using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

using RI.Utilities.ObjectModel;




namespace RI.Utilities.Collections.Virtualization
{
    /// <summary>
    ///     Implements a list which uses data virtualization to load data (the items in the collection) on demand.
    /// </summary>
    /// <typeparam name="T"> The type of items virtualized. </typeparam>
    /// <remarks>
    ///     <para>
    ///         <see cref="VirtualizationCollection{T}" /> uses an <see cref="IItemsProvider{T}" /> or <see cref="INotifyItemsProvider{T}" /> to load items on-demand.
    ///         That means that items are only loaded when they are actually requested through <see cref="VirtualizationCollection{T}" /> (e.g. by using the collections indexer property).
    ///     </para>
    ///     <para>
    ///         Items are loaded in pages which size can be specified when constructing <see cref="VirtualizationCollection{T}" />.
    ///         The loaded pages will then stay in the cache for a specified amount of time.
    ///     </para>
    ///     <note type="note">
    ///         If <see cref="INotifyItemsProvider{T}" /> is used, <see cref="INotifyItemsProvider{T}.ItemsChanged" /> will clear the entire cache.
    ///     </note>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class VirtualizationCollection <T> : IList<T>, IReadOnlyList<T>, ICollection<T>, IReadOnlyCollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable, IDisposable, ISynchronizable
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="VirtualizationCollection{T}" />
        /// </summary>
        /// <param name="pageSize"> The page size. </param>
        /// <param name="cacheTime"> The time pages stay in the cache or null if the pages stay in the cache indefinitely. </param>
        /// <param name="itemsProvider"> The provider which is used to load the items as needed. </param>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="pageSize" /> is less than 1 or <paramref name="cacheTime" /> is negative. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="itemsProvider" /> is null. </exception>
        public VirtualizationCollection (int pageSize, TimeSpan? cacheTime, IItemsProvider<T> itemsProvider)
            : this(pageSize, cacheTime == null ? 0 : (int)cacheTime.Value.TotalMilliseconds, itemsProvider) { }

        /// <summary>
        ///     Creates a new instance of <see cref="VirtualizationCollection{T}" />
        /// </summary>
        /// <param name="pageSize"> The page size. </param>
        /// <param name="cacheTimeMilliseconds"> The time in milliseconds pages stay in the cache or zero if the pages stay in the cache indefinitely. </param>
        /// <param name="itemsProvider"> The provider which is used to load the items as needed. </param>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="pageSize" /> is less than 1 or <paramref name="cacheTimeMilliseconds" /> is negative. </exception>
        /// <exception cref="ArgumentNullException"> <paramref name="itemsProvider" /> is null. </exception>
        public VirtualizationCollection (int pageSize, int cacheTimeMilliseconds, IItemsProvider<T> itemsProvider)
        {
            if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize));
            }

            if (cacheTimeMilliseconds < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(cacheTimeMilliseconds));
            }

            if (itemsProvider == null)
            {
                throw new ArgumentNullException(nameof(itemsProvider));
            }

            this.SyncRoot = new object();

            this.PageSize = pageSize;
            this.CacheTime = cacheTimeMilliseconds == 0 ? (TimeSpan?)null : TimeSpan.FromMilliseconds(cacheTimeMilliseconds);
            this.ItemsProvider = itemsProvider;

            this.Cache = new PageCollection();
            this.ItemsChangedHandler = this.ItemsChangedMethod;

            if (this.ItemsProvider is INotifyItemsProvider<T>)
            {
                ((INotifyItemsProvider<T>)this.ItemsProvider).ItemsChanged += this.ItemsChangedHandler;
            }
        }

        /// <summary>
        ///     Garbage collects this instance of <see cref="VirtualizationCollection{T}" />.
        /// </summary>
        ~VirtualizationCollection ()
        {
            this.Dispose();
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the time in milliseconds pages stay in the cache.
        /// </summary>
        /// <value>
        ///     The time in milliseconds pages stay in the cache or null if the pages stay in the cache indefinitely.
        /// </value>
        public TimeSpan? CacheTime { get; }

        /// <summary>
        ///     Gets the page size.
        /// </summary>
        /// <value>
        ///     The page size.
        /// </value>
        public int PageSize { get; }

        private PageCollection Cache { get; }

        private EventHandler ItemsChangedHandler { get; }

        private IItemsProvider<T> ItemsProvider { get; set; }

        private object SyncRoot { get; }

        #endregion




        #region Instance Events

        /// <summary>
        ///     Raised when the used <see cref="IItemsProvider{T}" /> signalled that items have changed.
        /// </summary>
        public event EventHandler ItemsChanged;

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Removes all outdated cached pages.
        /// </summary>
        public void CleanupCache ()
        {
            if (!this.CacheTime.HasValue)
            {
                return;
            }

            DateTime now = DateTime.UtcNow;
            this.Cache.RemoveWhere(x => now.Subtract(x.Timestamp) > this.CacheTime.Value);
        }

        /// <summary>
        ///     Clears the cache by removing all cached pages.
        /// </summary>
        public void ClearCache ()
        {
            this.VerifyNotDisposed();
            this.Cache.Clear();
        }

        private void ItemsChangedMethod (object sender, EventArgs args)
        {
            this.ClearCache();

            this.ItemsChanged?.Invoke(this, EventArgs.Empty);
        }

        private Page LoadPage (int pageIndex, bool temporary)
        {
            Page page;
            bool load;

            if (!temporary && this.Cache.Contains(pageIndex))
            {
                page = this.Cache[pageIndex];
                load = false;
            }
            else
            {
                page = new Page(pageIndex);
                load = true;
            }

            if (load)
            {
                int start = pageIndex * this.PageSize;
                int count = this.PageSize;

                IEnumerable<T> enumerator = this.ItemsProvider.GetItems(start, count);

                if (enumerator == null)
                {
                    page = null;
                }
                else
                {
                    page.Items.Clear();
                    page.Items.AddRange(enumerator);

                    if (page.Items.Count == 0)
                    {
                        page = null;
                    }
                }
            }

            if (!temporary)
            {
                this.Cache.Remove(pageIndex);

                if (page != null)
                {
                    this.Cache.Add(page);
                }
            }

            return page;
        }

        private void ThrowReadOnlyException ()
        {
            throw new NotSupportedException("A virtualization collection is read-only.");
        }

        private void VerifyNotDisposed ()
        {
            if (this.ItemsProvider == null)
            {
                throw new ObjectDisposedException(this.GetType()
                                                      .Name);
            }
        }

        #endregion




        #region Interface: IDisposable

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global"),]
        public void Dispose ()
        {
            if (this.ItemsProvider is INotifyItemsProvider<T>)
            {
                ((INotifyItemsProvider<T>)this.ItemsProvider).ItemsChanged -= this.ItemsChangedHandler;
            }

            (this.ItemsProvider as IDisposable)?.Dispose();
            this.ItemsProvider = null;

            this.Cache?.Clear();
        }

        #endregion




        #region Interface: IList

        /// <inheritdoc />
        bool IList.IsFixedSize => false;

        /// <inheritdoc />
        bool ICollection.IsSynchronized => ((ISynchronizable)this).IsSynchronized;

        /// <inheritdoc />
        object ICollection.SyncRoot => ((ISynchronizable)this).SyncRoot;

        /// <inheritdoc />
        object IList.this [int index]
        {
            get => this[index];
            set => this[index] = (T)value;
        }

        /// <inheritdoc />
        int IList.Add (object value)
        {
            ((IList<T>)this).Add((T)value);
            return ((IList)this).IndexOf(value);
        }

        /// <inheritdoc />
        void IList.Clear ()
        {
            ((IList<T>)this).Clear();
        }

        /// <inheritdoc />
        bool IList.Contains (object value)
        {
            return this.Contains((T)value);
        }

        /// <inheritdoc />
        void ICollection.CopyTo (Array array, int index)
        {
            int i1 = 0;

            foreach (T item in this)
            {
                array.SetValue(item, index + i1);
                i1++;
            }
        }

        /// <inheritdoc />
        int IList.IndexOf (object value)
        {
            return this.IndexOf((T)value);
        }

        /// <inheritdoc />
        void IList.Insert (int index, object value)
        {
            ((IList<T>)this).Insert(index, (T)value);
        }

        /// <inheritdoc />
        void IList.Remove (object value)
        {
            ((IList<T>)this).Remove((T)value);
        }

        /// <inheritdoc />
        void IList.RemoveAt (int index)
        {
            ((IList<T>)this).RemoveAt(index);
        }

        #endregion




        #region Interface: IList<T>

        /// <inheritdoc />
        public int Count
        {
            get
            {
                this.VerifyNotDisposed();

                this.CleanupCache();

                return this.ItemsProvider.GetCount();
            }
        }

        /// <inheritdoc />
        public bool IsReadOnly => true;

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ValueParameterNotUsed"),]
        public T this [int index]
        {
            get
            {
                this.VerifyNotDisposed();

                this.CleanupCache();

                if (index < 0)
                {
                    throw new IndexOutOfRangeException("The index cannot be less than zero.");
                }

                int pageIndex = index / this.PageSize;
                int pageOffset = index % this.PageSize;

                this.LoadPage(pageIndex + 1, false);

                if (pageIndex > 0)
                {
                    this.LoadPage(pageIndex - 1, false);
                }

                Page page = this.LoadPage(pageIndex, false);

                if (page == null)
                {
                    throw new IndexOutOfRangeException("The index points to a nonexistent page.");
                }

                if (pageOffset >= page.Items.Count)
                {
                    throw new IndexOutOfRangeException("The index results in a too large offset.");
                }

                return page.Items[pageOffset];
            }
            set => this.ThrowReadOnlyException();
        }

        /// <inheritdoc />
        void ICollection<T>.Add (T item)
        {
            this.ThrowReadOnlyException();
        }

        /// <inheritdoc />
        void ICollection<T>.Clear ()
        {
            this.ThrowReadOnlyException();
        }

        /// <inheritdoc />
        public bool Contains (T item)
        {
            this.VerifyNotDisposed();

            this.CleanupCache();

            return this.ItemsProvider.Search(item) != -1;
        }

        /// <inheritdoc />
        public void CopyTo (T[] array, int arrayIndex)
        {
            this.VerifyNotDisposed();

            this.CleanupCache();

            foreach (T item in this)
            {
                array[arrayIndex] = item;
                arrayIndex++;
            }
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator ()
        {
            this.VerifyNotDisposed();

            this.CleanupCache();

            int pageIndex = 0;

            while (true)
            {
                Page page = this.LoadPage(pageIndex, true);

                if (page == null)
                {
                    yield break;
                }

                foreach (T item in page.Items)
                {
                    yield return item;
                }

                pageIndex++;
            }
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator ()
        {
            return this.GetEnumerator();
        }

        /// <inheritdoc />
        public int IndexOf (T item)
        {
            this.VerifyNotDisposed();

            this.CleanupCache();

            return this.ItemsProvider.Search(item);
        }

        /// <inheritdoc />
        void IList<T>.Insert (int index, T item)
        {
            this.ThrowReadOnlyException();
        }

        /// <inheritdoc />
        bool ICollection<T>.Remove (T item)
        {
            this.ThrowReadOnlyException();
            return false;
        }

        /// <inheritdoc />
        void IList<T>.RemoveAt (int index)
        {
            this.ThrowReadOnlyException();
        }

        #endregion




        #region Interface: ISynchronizable

        /// <inheritdoc />
        bool ISynchronizable.IsSynchronized => false;

        /// <inheritdoc />
        object ISynchronizable.SyncRoot => this.SyncRoot;

        #endregion




        #region Type: Page

        private sealed class Page
        {
            #region Instance Constructor/Destructor

            public Page (int pageIndex)
            {
                if (pageIndex < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(pageIndex));
                }

                this.PageIndex = pageIndex;

                this.Items = new List<T>();
                this.Timestamp = DateTime.UtcNow;
            }

            #endregion




            #region Instance Properties/Indexer

            public List<T> Items { get; }

            public int PageIndex { get; }

            public DateTime Timestamp { get; private set; }

            #endregion




            #region Instance Methods

            public void ResetTtl ()
            {
                this.Timestamp = DateTime.UtcNow;
            }

            #endregion
        }

        #endregion




        #region Type: PageCollection

        private sealed class PageCollection : KeyedCollection<int, Page>
        {
            #region Overrides

            protected override int GetKeyForItem (Page item)
            {
                return item.PageIndex;
            }

            #endregion
        }

        #endregion
    }
}
