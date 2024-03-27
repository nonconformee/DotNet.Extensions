using System;
using System.Collections;
using System.Collections.Generic;

using RI.Utilities.ObjectModel;




namespace RI.Utilities.Collections.Generic
{
    /// <summary>
    ///     Defines a generic interface for priority queues.
    /// </summary>
    /// <typeparam name="T"> The type of items stored in the priority queue. </typeparam>
    /// <remarks>
    ///     <para>
    ///         A priority queue stores items sorted by their assigned priority.
    ///         The priority is assigned to an item when the item is added to the priority queue using <see cref="Enqueue(T,int)" />.
    ///         The higher the priority, the earlier the item is dequeued (highest priority, first out).
    ///         For items of the same priority, the order in which they are added is maintained (first in, first out).
    ///     </para>
    ///     <para>
    ///         null are valid item values if <typeparamref name="T" /> is a reference type.
    ///     </para>
    /// </remarks>
    public interface IPriorityQueue <T> : ICollection, IEnumerable<T>, IEnumerable, ICopyable<IPriorityQueue<T>>
    {
        /// <summary>
        ///     Gets the highest priority currently in the queue.
        /// </summary>
        /// <value>
        ///     The highest priority currently in the queue or -1 if the queue is empty.
        /// </value>
        int HighestPriority { get; }

        /// <summary>
        ///     Gets the lowest priority currently in the queue.
        /// </summary>
        /// <value>
        ///     The lowest priority currently in the queue or -1 if the queue is empty.
        /// </value>
        int LowestPriority { get; }

        /// <summary>
        ///     Removes all items from the priority queue.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         This is a O(1) operation.
        ///     </para>
        /// </remarks>
        void Clear ();

        /// <summary>
        ///     Gets the next item in the queue and removes it.
        /// </summary>
        /// <returns>
        ///     The item.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(1) operation.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The priority queue is empty. </exception>
        T Dequeue ();

        /// <summary>
        ///     Gets the next item in the queue and removes it.
        /// </summary>
        /// <param name="priority"> The priority of the item. </param>
        /// <returns>
        ///     The item.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(1) operation.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The priority queue is empty. </exception>
        T Dequeue (out int priority);

        /// <summary>
        ///     Adds an item to the queue.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <remarks>
        ///     <para>
        ///         The item is added to the queue using the lowest current priority (similar to <see cref="LowestPriority" />) or with a priority of 0 if the queue is empty.
        ///         This ensures the item is inserted truly at the end of the queue.
        ///     </para>
        ///     <para>
        ///         This is a O(1) operation.
        ///     </para>
        /// </remarks>
        void Enqueue (T item);

        /// <summary>
        ///     Adds an item to the queue.
        /// </summary>
        /// <param name="item"> The item. </param>
        /// <param name="priority"> The priority of the item. </param>
        /// <remarks>
        ///     <para>
        ///         This is a O(x) operation, where x is the number of priorities currently in use, if <paramref name="priority" /> is currently not yet in use, or a O(1) operation if there are already other items of the same priority.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="priority" /> is less than zero. </exception>
        void Enqueue (T item, int priority);

        /// <summary>
        ///     Moves all items of this queue to another queue while keeping the assigned priorities.
        /// </summary>
        /// <param name="queue"> The other queue the items are moved to. </param>
        /// <returns>
        ///     The number of moved items.
        /// </returns>
        /// <remarks>
        ///     <note type="important">
        ///         As the name of this method implies, the items are moved to the other queue, dequeueing all items from this queue.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="queue" /> is null. </exception>
        int MoveTo (PriorityQueue<T> queue);

        /// <summary>
        ///     Gets the next item in the queue without removing it.
        /// </summary>
        /// <returns>
        ///     The item.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(1) operation.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The priority queue is empty. </exception>
        T Peek ();

        /// <summary>
        ///     Gets the next item in the queue without removing it.
        /// </summary>
        /// <param name="priority"> The priority of the item. </param>
        /// <returns>
        ///     The item.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(1) operation.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The priority queue is empty. </exception>
        T Peek (out int priority);
    }
}
