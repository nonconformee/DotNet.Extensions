using System;
using System.Collections.Generic;




namespace RI.Utilities.Collections.Generic
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="IPriorityQueue{T}" /> type and its implementations.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class IPriorityQueueExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Converts any instance implementing <see cref="IPriorityQueue{T}" /> to an explicit <see cref="IPriorityQueue{T}" />.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="priorityQueue" />. </typeparam>
        /// <param name="priorityQueue"> The instance implementing <see cref="IPriorityQueue{T}" />. </param>
        /// <returns>
        ///     The instance as explicit <see cref="IPriorityQueue{T}" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A conversion to an explicit <see cref="IPriorityQueue{T}" /> can be useful in cases where the utility/extension methods of <see cref="IPriorityQueue{T}" /> shall be used instead of the ones implemented by the instance itself.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="priorityQueue" /> is null. </exception>
        public static IPriorityQueue<T> AsPriorityQueue <T> (this IPriorityQueue<T> priorityQueue)
        {
            if (priorityQueue == null)
            {
                throw new ArgumentNullException(nameof(priorityQueue));
            }

            return priorityQueue;
        }

        /// <summary>
        ///     Removes all occurences of an item from the priority queue.
        ///     Comparison is done using the default equality comparison.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="priorityQueue" />. </typeparam>
        /// <param name="priorityQueue"> The priority queue. </param>
        /// <param name="item"> The item to remove. </param>
        /// <returns>
        ///     The number of times the item was removed from the priority queue.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         All matching items are removed, regardless of the priorities and the order in which they are in the queue.
        ///     </para>
        ///     <note type="important">
        ///         This method is considered very slow as it needs to rebuild the whole internal structure of the priority queue.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="priorityQueue" /> is null. </exception>
        public static int Remove <T> (this IPriorityQueue<T> priorityQueue, T item)
        {
            if (priorityQueue == null)
            {
                throw new ArgumentNullException(nameof(priorityQueue));
            }

            return priorityQueue.Remove(item, EqualityComparer<T>.Default.Equals);
        }

        /// <summary>
        ///     Removes all occurences of an item from the priority queue.
        ///     Comparison is done using the specified equality comparer.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="priorityQueue" />. </typeparam>
        /// <param name="priorityQueue"> The priority queue. </param>
        /// <param name="item"> The item to remove. </param>
        /// <param name="comparer"> The comparer to use to test which items to remove. </param>
        /// <returns>
        ///     The number of times the item was removed from the priority queue.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         All matching items are removed, regardless of the priorities and the order in which they are in the queue.
        ///     </para>
        ///     <note type="important">
        ///         This method is considered very slow as it needs to rebuild the whole internal structure of the priority queue.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="priorityQueue" /> or <paramref name="comparer" /> is null. </exception>
        public static int Remove <T> (this IPriorityQueue<T> priorityQueue, T item, IEqualityComparer<T> comparer)
        {
            if (priorityQueue == null)
            {
                throw new ArgumentNullException(nameof(priorityQueue));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return priorityQueue.Remove(item, comparer.Equals);
        }

        /// <summary>
        ///     Removes all occurences of an item from the priority queue.
        ///     Comparison is done using the specified function.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="priorityQueue" />. </typeparam>
        /// <param name="priorityQueue"> The priority queue. </param>
        /// <param name="item"> The item to remove. </param>
        /// <param name="comparer"> The function to use to test which items to remove. </param>
        /// <returns>
        ///     The number of times the item was removed from the priority queue.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         All matching items are removed, regardless of the priorities and the order in which they are in the queue.
        ///     </para>
        ///     <note type="important">
        ///         This method is considered very slow as it needs to rebuild the whole internal structure of the priority queue.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="priorityQueue" /> or <paramref name="comparer" /> is null. </exception>
        public static int Remove <T> (this IPriorityQueue<T> priorityQueue, T item, Func<T, T, bool> comparer)
        {
            if (priorityQueue == null)
            {
                throw new ArgumentNullException(nameof(priorityQueue));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            List<T> removedItems = priorityQueue.RemoveWhere((a, b) => comparer(item, a));
            return removedItems.Count;
        }

        /// <summary>
        ///     Removes items from the queue based on a predicate.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="priorityQueue" />. </typeparam>
        /// <param name="priorityQueue"> The priority queue. </param>
        /// <param name="predicate"> The predicate. </param>
        /// <returns>
        ///     The list of removed items.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         All items for which the predicate returns true are removed, regardless of the priorities and the order in which they are in the queue.
        ///     </para>
        ///     <note type="important">
        ///         This method is considered very slow as it needs to rebuild the whole internal structure of the priority queue.
        ///     </note>
        ///     <note type="important">
        ///         The priority queue is left in an undefined, most probably empty, state if an exception is thrown by <paramref name="predicate" />.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="priorityQueue" /> or <paramref name="predicate" /> is null. </exception>
        public static List<T> RemoveWhere <T> (this IPriorityQueue<T> priorityQueue, PriorityQueueRemovePredicate<T> predicate)
        {
            if (priorityQueue == null)
            {
                throw new ArgumentNullException(nameof(priorityQueue));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            List<RemovalTuple<T>> queueItems = new List<RemovalTuple<T>>();

            while (priorityQueue.Count > 0)
            {
                T queueItem = priorityQueue.Dequeue(out int priority);
                queueItems.Add(new RemovalTuple<T>(queueItem, priority));
            }

            priorityQueue.Clear();

            List<T> removed = new List<T>();
            List<RemovalTuple<T>> preserved = new List<RemovalTuple<T>>();

            foreach (RemovalTuple<T> queueItem in queueItems)
            {
                bool remove = predicate(queueItem.Item, queueItem.Priority);

                if (remove)
                {
                    removed.Add(queueItem.Item);
                }
                else
                {
                    preserved.Add(queueItem);
                }
            }

            foreach (RemovalTuple<T> preservedItem in preserved)
            {
                priorityQueue.Enqueue(preservedItem.Item, preservedItem.Priority);
            }

            return removed;
        }

        #endregion




        #region Type: RemovalTuple

        private sealed class RemovalTuple <T>
        {
            #region Instance Constructor/Destructor

            public RemovalTuple (T item, int priority)
            {
                this.Item = item;
                this.Priority = priority;
            }

            #endregion




            #region Instance Properties/Indexer

            public T Item { get; }

            public int Priority { get; }

            #endregion
        }

        #endregion
    }
}
