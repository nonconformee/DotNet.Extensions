using System;
using System.Collections.Generic;




namespace RI.Utilities.Collections.Generic
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="IPool{T}" /> type and its implementations.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class IPoolExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Converts any instance implementing <see cref="IPool{T}" /> to an explicit <see cref="IPool{T}" />.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="pool" />. </typeparam>
        /// <param name="pool"> The instance implementing <see cref="IPool{T}" />. </param>
        /// <returns>
        ///     The instance as explicit <see cref="IPool{T}" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A conversion to an explicit <see cref="IPool{T}" /> can be useful in cases where the utility/extension methods of <see cref="IPool{T}" /> shall be used instead of the ones implemented by the instance itself.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="pool" /> is null. </exception>
        public static IPool<T> AsPriorityQueue <T> (this IPool<T> pool)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(nameof(pool));
            }

            return pool;
        }

        /// <summary>
        ///     Changes a pool so that it contains a specified exact number of free items.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="pool" />. </typeparam>
        /// <param name="pool"> The pool. </param>
        /// <param name="numItems"> The number of free items the pool must have. </param>
        /// <returns>
        ///     The change in free items which was necessary to get the specified number of free items.
        ///     The value is positive if new items were created, negative if free items were removed, or zero if the pool already contained the specified number of free items.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="pool" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="numItems" /> is less than zero. </exception>
        public static int ReduceEnsure <T> (this IPool<T> pool, int numItems)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(nameof(pool));
            }

            if (numItems < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numItems));
            }

            int difference = 0;
            difference -= pool.Reduce(numItems);
            difference += pool.Ensure(numItems);
            return difference;
        }

        /// <summary>
        ///     Returns multiple items to a pool for recycling.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="pool" />. </typeparam>
        /// <param name="pool"> The pool. </param>
        /// <param name="items"> The sequence of items to be returned to the pool. </param>
        /// <returns>
        ///     The number of items returned to the pool.
        ///     Zero if the sequence contained no elements.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="items" /> is enumerated exactly once.
        ///     </para>
        ///     <note type="note">
        ///         The behaviour when the same item is returned multiple times without being taken is defined by the <see cref="IPool{T}" /> implementation.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="pool" /> or <paramref name="items" /> is null. </exception>
        public static int ReturnRange <T> (this IPool<T> pool, IEnumerable<T> items)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(nameof(pool));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int count = 0;

            foreach (T item in items)
            {
                pool.Return(item);
                count++;
            }

            return count;
        }

        /// <summary>
        ///     Returns an item to a pool as a free item so that it can be recycled.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="pool" />. </typeparam>
        /// <param name="pool"> The pool. </param>
        /// <param name="item"> The item to return to the pool. </param>
        /// <returns>
        ///     true if the item was returned, false if it was already returned.
        /// </returns>
        /// <remarks>
        ///     <note type="important">
        ///         This return operation does check whether the item to be returned has already been returned to ensure consistency of the free and taken items.
        ///         If a more performant return operation is required, use <see cref="IPool{T}.Return" /> instead.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="pool" /> or <paramref name="item" /> is null. </exception>
        public static bool ReturnSafe <T> (this IPool<T> pool, T item)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(nameof(pool));
            }

            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            if (pool.Contains(item))
            {
                return false;
            }

            pool.Return(item);
            return true;
        }

        /// <summary>
        ///     Returns multiple items to a pool for recycling.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="pool" />. </typeparam>
        /// <param name="pool"> The pool. </param>
        /// <param name="items"> The sequence of items to be returned to the pool. </param>
        /// <returns>
        ///     The number of items returned to the pool.
        ///     Zero if the sequence contained no elements.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="items" /> is enumerated exactly once.
        ///     </para>
        ///     <note type="note">
        ///         This return operation does check whether the item to be returned has already been returned to ensure consistency of the free and taken items.
        ///         If a more performant return operation is required, use <see cref="IPoolExtensions.ReturnRange{T}(IPool{T},IEnumerable{T})" /> instead.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="pool" /> or <paramref name="items" /> is null. </exception>
        public static int ReturnSafeRange <T> (this IPool<T> pool, IEnumerable<T> items)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(nameof(pool));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int count = 0;

            foreach (T item in items)
            {
                if (!pool.Contains(item))
                {
                    pool.Return(item);
                }

                count++;
            }

            return count;
        }

        /// <summary>
        ///     Takes multiple items from a pool and creates as much new items as necessary.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="pool" />. </typeparam>
        /// <param name="pool"> The pool. </param>
        /// <param name="numItems"> The number of items to take. </param>
        /// <returns>
        ///     The array of taken items.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="pool" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="numItems" /> is less than zero. </exception>
        public static T[] TakeRange <T> (this IPool<T> pool, int numItems)
        {
            if (pool == null)
            {
                throw new ArgumentNullException(nameof(pool));
            }

            if (numItems < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numItems));
            }

            T[] items = new T[numItems];

            for (int i1 = 0; i1 < numItems; i1++)
            {
                items[i1] = pool.Take();
            }

            return items;
        }

        #endregion
    }
}
