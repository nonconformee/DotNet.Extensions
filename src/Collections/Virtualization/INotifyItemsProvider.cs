using System;




namespace RI.Utilities.Collections.Virtualization
{
    /// <summary>
    ///     Defines the interface of an <see cref="IItemsProvider{T}" /> which also supports notification when the items in the data source have changed.
    /// </summary>
    /// <typeparam name="T"> The type of items loaded by this data source. </typeparam>
    public interface INotifyItemsProvider <T> : IItemsProvider<T>
    {
        /// <summary>
        ///     Raised when the data source detects that the items have changed.
        /// </summary>
        event EventHandler ItemsChanged;
    }
}
