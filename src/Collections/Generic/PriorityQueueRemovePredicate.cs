namespace RI.Utilities.Collections.Generic
{
    /// <summary>
    ///     Defines a delegate which can be used for priority queue removals using <see cref="IPriorityQueueExtensions.RemoveWhere{T}(IPriorityQueue{T}, PriorityQueueRemovePredicate{T})" />.
    /// </summary>
    /// <typeparam name="T"> The type of the items in the priority queue. </typeparam>
    /// <param name="item"> The item. </param>
    /// <param name="priority"> The priority of the item. </param>
    /// <returns>
    ///     true if the item is to be removed, false otherwise.
    /// </returns>
    public delegate bool PriorityQueueRemovePredicate <in T> (T item, int priority);
}
