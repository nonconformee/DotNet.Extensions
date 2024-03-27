using System;
using System.Collections.Generic;
using System.Linq;




namespace RI.Utilities.Collections
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="ISet{T}" /> type and its implementations.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class ISetExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Adds multiple items to a set.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="set" />. </typeparam>
        /// <param name="set"> The set. </param>
        /// <param name="items"> The sequence of items to add to the set. </param>
        /// <returns>
        ///     The number of items added to the set.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="items" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="set" /> or <paramref name="items" /> is null. </exception>
        public static int AddRange <T> (this ISet<T> set, IEnumerable<T> items)
        {
            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int count = set.Count;
            set.UnionWith(items.ToList());
            return set.Count - count;
        }

        /// <summary>
        ///     Converts any instance implementing <see cref="ISet{T}" /> to an explicit <see cref="ISet{T}" />.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="set" />. </typeparam>
        /// <param name="set"> The instance implementing <see cref="ISet{T}" />. </param>
        /// <returns>
        ///     The instance as explicit <see cref="ISet{T}" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A conversion to an explicit <see cref="ISet{T}" /> can be useful in cases where the utility/extension methods of <see cref="ISet{T}" /> shall be used instead of the ones implemented by the instance itself.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="set" /> is null. </exception>
        public static ISet<T> AsSet <T> (this ISet<T> set)
        {
            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            return set;
        }

        /// <summary>
        ///     Removes multiple items from a set.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="set" />. </typeparam>
        /// <param name="set"> The set. </param>
        /// <param name="items"> The sequence of items to remove from the set. </param>
        /// <returns>
        ///     The number of items removed from hash set.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="items" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="set" /> or <paramref name="items" /> is null. </exception>
        public static int RemoveRange <T> (this ISet<T> set, IEnumerable<T> items)
        {
            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int count = set.Count;

            set.ExceptWith(items.AsEnumerable()
                                .ToList());

            return count - set.Count;
        }

        #endregion
    }
}
