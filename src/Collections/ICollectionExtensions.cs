using System;
using System.Collections.Generic;




namespace RI.Utilities.Collections
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="ICollection{T}" /> type and its implementations.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class ICollectionExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Adds multiple items to a collection.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="collection" />. </typeparam>
        /// <param name="collection"> The collection. </param>
        /// <param name="items"> The sequence of items to add to the collection. </param>
        /// <returns>
        ///     The number of items added to the collection.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="items" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="collection" /> or <paramref name="items" /> is null. </exception>
        public static int AddRange <T> (this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int addedCount = 0;

            foreach (T item in items)
            {
                collection.Add(item);
                addedCount++;
            }

            return addedCount;
        }

        /// <summary>
        ///     Converts any instance implementing <see cref="ICollection{T}" /> to an explicit <see cref="ICollection{T}" />.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="collection" />. </typeparam>
        /// <param name="collection"> The instance implementing <see cref="ICollection{T}" />. </param>
        /// <returns>
        ///     The instance as explicit <see cref="ICollection{T}" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A conversion to an explicit <see cref="ICollection{T}" /> can be useful in cases where the utility/extension methods of <see cref="ICollection{T}" /> shall be used instead of the ones implemented by the instance itself.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="collection" /> is null. </exception>
        public static ICollection<T> AsCollection <T> (this ICollection<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            return collection;
        }

        /// <summary>
        ///     Removes all occurences of an item from a collection.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="collection" />. </typeparam>
        /// <param name="collection"> The collection. </param>
        /// <param name="item"> The item to remove from the collection. </param>
        /// <returns>
        ///     The number of items removed from the collection.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="RemoveAll{T}" /> is useful in situations where a collection can contain the same item multiple times and <see cref="ICollection{T}.Remove" /> only removes the first occurence.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="collection" /> is null. </exception>
        public static int RemoveAll <T> (this ICollection<T> collection, T item)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            int removedCount = 0;

            while (collection.Remove(item))
            {
                removedCount++;
            }

            return removedCount;
        }

        /// <summary>
        ///     Removes all occurences of multiple items from a collection.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="collection" />. </typeparam>
        /// <param name="collection"> The collection. </param>
        /// <param name="items"> The sequence of items to remove from the collection. </param>
        /// <returns>
        ///     The number of items removed from the collection.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="items" /> is enumerated exactly once.
        ///     </para>
        ///     <para>
        ///         <see cref="RemoveAllRange{T}" /> is useful in situations where a collection can contain the same item multiple times and <see cref="ICollection{T}.Remove" /> only removes the first occurence.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="collection" /> or <paramref name="items" /> is null. </exception>
        public static int RemoveAllRange <T> (this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int removedCount = 0;

            foreach (T value in items)
            {
                removedCount += collection.RemoveAll(value);
            }

            return removedCount;
        }

        /// <summary>
        ///     Removes multiple items from a collection.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="collection" />. </typeparam>
        /// <param name="collection"> The collection. </param>
        /// <param name="items"> The sequence of items to remove from the collection. </param>
        /// <returns>
        ///     The number of items removed from the collection.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="items" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="collection" /> or <paramref name="items" /> is null. </exception>
        public static int RemoveRange <T> (this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int removedCount = 0;

            foreach (T value in items)
            {
                if (collection.Remove(value))
                {
                    removedCount++;
                }
            }

            return removedCount;
        }

        /// <summary>
        ///     Removes all items from a collection which satisfy a condition.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="collection" />. </typeparam>
        /// <param name="collection"> The collection. </param>
        /// <param name="condition"> The function which tests each item for a condition, providing the item itself. </param>
        /// <returns>
        ///     The list of removed items.
        ///     The list is empty if no elements were removed.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="collection" /> or <paramref name="condition" /> is null. </exception>
        public static List<T> RemoveWhere <T> (this ICollection<T> collection, Func<T, bool> condition)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            List<T> removedItems = new List<T>(collection.Count);

            foreach (T item in collection)
            {
                if (condition(item))
                {
                    removedItems.Add(item);
                }
            }

            for (int i1 = 0; i1 < removedItems.Count; i1++)
            {
                collection.Remove(removedItems[i1]);
            }

            return removedItems;
        }

        /// <summary>
        ///     Removes all items from a collection which satisfy a condition.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="collection" />. </typeparam>
        /// <param name="collection"> The collection. </param>
        /// <param name="condition"> The function which tests each item for a condition, providing the items index and the item itself. </param>
        /// <returns>
        ///     The list of removed items.
        ///     The list is empty if no elements were removed.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="collection" /> or <paramref name="condition" /> is null. </exception>
        public static List<T> RemoveWhere <T> (this ICollection<T> collection, Func<int, T, bool> condition)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            List<T> removedItems = new List<T>(collection.Count);
            int index = 0;

            foreach (T item in collection)
            {
                if (condition(index, item))
                {
                    removedItems.Add(item);
                }

                index++;
            }

            for (int i1 = 0; i1 < removedItems.Count; i1++)
            {
                collection.Remove(removedItems[i1]);
            }

            return removedItems;
        }

        #endregion
    }
}
