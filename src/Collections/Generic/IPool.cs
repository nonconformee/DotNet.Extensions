using System;
using System.Collections.Generic;




namespace RI.Utilities.Collections.Generic
{
    /// <summary>
    ///     Defines a generic interface for pools which can be used to store and recycle objects.
    /// </summary>
    /// <typeparam name="T"> The type of objects which can be stored and recycled by the pool. </typeparam>
    /// <remarks>
    ///     <para>
    ///         Pools are used when it is desired to re-use instances instead of destroy/dispose/garbage-collect them, followed by creation of new instances, e.g. for performance reasons.
    ///     </para>
    ///     <note type="implement">
    ///         Implementations of <see cref="IPool{T}" /> should support <see cref="IPoolAware" /> if applicable.
    ///     </note>
    ///     <note type="implement">
    ///         Implementations of <see cref="IPool{T}" /> must not track taken items until they are returned again.
    ///     </note>
    /// </remarks>
    public interface IPool <T>
    {
        /// <summary>
        ///     Gets the number of free items in the pool.
        /// </summary>
        /// <value>
        ///     The number of free items in the pool.
        /// </value>
        int Count { get; }

        /// <summary>
        ///     Gets the sequence of free items in the pool.
        /// </summary>
        /// <value>
        ///     The sequence of free items in the pool.
        /// </value>
        /// <remarks>
        ///     <note type="implement">
        ///         <see cref="FreeItems" /> must not be null.
        ///     </note>
        /// </remarks>
        IEnumerable<T> FreeItems { get; }

        /// <summary>
        ///     Removes all free items from the pool.
        /// </summary>
        void Clear ();

        /// <summary>
        ///     Determines whether an item is in the pool as a free item.
        /// </summary>
        /// <param name="item"> The item which is checked to be in the pool as a free item. </param>
        /// <returns>
        ///     true if the specified item is in the pool as a free item, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="item" /> is null. </exception>
        bool Contains (T item);

        /// <summary>
        ///     Ensures that a minimum number of free items are in the pool by creating new items if necessary.
        /// </summary>
        /// <param name="minItems"> The number of minimum required free items in the pool. </param>
        /// <returns>
        ///     The number of newly created items.
        ///     Zero if no items were created or the number of free items is already bigger than the specified minimum.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="minItems" /> is less than zero. </exception>
        int Ensure (int minItems);

        /// <summary>
        ///     Ensures that a maximum number of free items are in a pool by removing excess free items if necessary.
        /// </summary>
        /// <param name="maxItems"> The number of maximum allowed free items in the pool. </param>
        /// <returns>
        ///     The number of removed items.
        ///     Zero if no items were removed or the number of free items is already less than the specified maximum.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="maxItems" /> is less than zero. </exception>
        int Reduce (int maxItems);

        /// <summary>
        ///     Returns an item to the pool as a free item so that it can be recycled. />.
        /// </summary>
        /// <param name="item"> The item to return to the pool. </param>
        /// <remarks>
        ///     <note type="note">
        ///         The behaviour when the same item is returned multiple times without being taken is defined by the <see cref="IPool{T}" /> implementation.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="item" /> is null. </exception>
        void Return (T item);

        /// <summary>
        ///     Takes the next free item from the pool or creates a new one if there is no free item.
        /// </summary>
        /// <returns>
        ///     The item taken from the pool or newly created.
        /// </returns>
        T Take ();

        /// <summary>
        ///     Takes the next free item from the pool or creates a new one if there is no free item.
        /// </summary>
        /// <param name="created"> Indicates whether the taken item was newly created (true) or was a free item (false). </param>
        /// <returns>
        ///     The item taken from the pool or newly created.
        /// </returns>
        T Take (out bool created);
    }
}
