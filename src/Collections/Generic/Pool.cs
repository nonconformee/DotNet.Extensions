using System;




namespace RI.Utilities.Collections.Generic
{
    /// <summary>
    ///     Implements a simple pool which.
    /// </summary>
    /// <typeparam name="T"> The type of objects which can be stored and recycled by the pool. </typeparam>
    /// <remarks>
    ///     <para>
    ///         See <see cref="PoolBase{T}" /> for more details.
    ///     </para>
    ///     <para>
    ///         This pool implementation supports <see cref="IPoolAware" />.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    /// <example>
    ///     <code language="cs">
    /// <![CDATA[
    /// // create a pool
    /// var pool = new Pool<MyObject>();
    /// 
    /// // get an item from the pool (needs to be created as the pool is empty)
    /// var item1 = pool.Take();
    /// 
    /// // get another item from the pool (needs to be created as the pool is empty)
    /// var item2 = pool.Take();
    /// 
    /// // ... do something ...
    /// 
    /// // return one of the items
    /// pool.Return(item2);
    /// 
    /// // ... do something ...
    /// 
    /// // get another item (the former item2 is recycled)
    /// var item3 = pool.Take();
    /// ]]>
    /// </code>
    /// </example>
    public sealed class Pool <T> : PoolBase<T>
        where T : new()
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="Pool{T}" />.
        /// </summary>
        public Pool () { }

        /// <summary>
        ///     Creates a new instance of <see cref="Pool{T}" />.
        /// </summary>
        /// <param name="capacity"> The initial capacity of free items in the pool. </param>
        /// <remarks>
        ///     <para>
        ///         <paramref name="capacity" /> is only a hint of the expected number of free items.
        ///         No free items are created so the initial count of free items in the pool is zero, regardless of the value of <paramref name="capacity" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="capacity" /> is less than zero. </exception>
        public Pool (int capacity)
            : base(capacity) { }

        #endregion




        #region Overrides

        /// <inheritdoc />
        protected override T Create ()
        {
            return new T();
        }

        #endregion
    }
}
