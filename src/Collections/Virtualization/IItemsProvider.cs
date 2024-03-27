using System.Collections.Generic;




namespace RI.Utilities.Collections.Virtualization
{
    /// <summary>
    ///     Defines the interface for a data source which provides data or items respectively to a <see cref="VirtualizationCollection{T}" />.
    /// </summary>
    /// <typeparam name="T"> The type of items loaded by this data source. </typeparam>
    public interface IItemsProvider <T>
    {
        /// <summary>
        ///     Gets the total number of items.
        /// </summary>
        /// <returns>
        ///     The total number of items.
        /// </returns>
        int GetCount ();

        /// <summary>
        ///     Gets the items in the specified range.
        /// </summary>
        /// <param name="start"> The start index of the first item. </param>
        /// <param name="count"> The number of items to get. </param>
        /// <returns>
        ///     A sequence which can be enumerated to actually load the items.
        /// </returns>
        /// <remarks>
        ///     <note type="important">
        ///         The number of elements in the sequence is not necessarily <paramref name="count" /> but guaranteed to be less or equal.
        ///     </note>
        /// </remarks>
        IEnumerable<T> GetItems (int start, int count);

        /// <summary>
        ///     Searches the data source for a specified item.
        /// </summary>
        /// <param name="item"> The item to search for. </param>
        /// <returns>
        ///     The index of the item or -1 if the item could not be found.
        /// </returns>
        int Search (T item);
    }
}
