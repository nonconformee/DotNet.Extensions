using System;
using System.Collections.Generic;
using System.Linq;




namespace RI.Utilities.Collections
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="Queue{T}" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class QueueExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Gets all the items from a queue in the order they are dequeue'ed and removes them.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="queue" />. </typeparam>
        /// <param name="queue"> The queue. </param>
        /// <returns>
        ///     The list which contains all the items of the queue in the order they are dequeue'ed.
        ///     The list is empty if the queue contains no items.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="queue" /> is null. </exception>
        public static List<T> DequeueAll <T> (this Queue<T> queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            List<T> items = new List<T>(queue.Count);

            while (queue.Count > 0)
            {
                items.Add(queue.Dequeue());
            }

            return items;
        }

        /// <summary>
        ///     Gets all the items from a queue in the order they are dequeue'ed and removes them.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="queue" />. </typeparam>
        /// <param name="queue"> The queue. </param>
        /// <param name="collection"> The collection the dequeued items are put into. </param>
        /// <returns>
        ///     The number of dequeued items.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="queue" /> or <paramref name="collection" /> is null. </exception>
        public static int DequeueInto <T> (this Queue<T> queue, ICollection<T> collection)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            int count = queue.Count;

            while (queue.Count > 0)
            {
                collection.Add(queue.Dequeue());
            }

            return count;
        }

        /// <summary>
        ///     Enqueues multiple items to a queue.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="queue" />. </typeparam>
        /// <param name="queue"> The queue. </param>
        /// <param name="items"> The sequence of items to enqueue to the queue. </param>
        /// <returns>
        ///     The number of items enqueued to the queue.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The items in <paramref name="items" /> are enqueued in the order they are enumerated.
        ///     </para>
        ///     <para>
        ///         <paramref name="items" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="queue" /> or <paramref name="items" /> is null. </exception>
        public static int EnqueueRange <T> (this Queue<T> queue, IEnumerable<T> items)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int count = 0;

            foreach (T item in items)
            {
                queue.Enqueue(item);
                count++;
            }

            return count;
        }

        /// <summary>
        ///     Gets all the items from a queue in the order they would be dequeue'ed without removing them.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="queue" />. </typeparam>
        /// <param name="queue"> The queue. </param>
        /// <returns>
        ///     The list which contains all the items of the queue in the order they would be dequeue'ed.
        ///     The list is empty if the queue contains no items.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="queue" /> is null. </exception>
        public static List<T> PeekAll <T> (this Queue<T> queue)
        {
            if (queue == null)
            {
                throw new ArgumentNullException(nameof(queue));
            }

            return queue.AsEnumerable()
                        .ToList();
        }

        #endregion
    }
}
