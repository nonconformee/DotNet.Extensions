using System;
using System.Collections.Generic;

using RI.Utilities.Numbers;




namespace RI.Utilities.Collections
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="IReadOnlyList{T}" /> type and its implementations.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The <c> Ro </c> prefix is added to some extension methods to avoid ambiguity with extension methods from <see cref="IListExtensions" />.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public static class IReadonlyListExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Converts any instance implementing <see cref="IReadOnlyList{T}" /> to an explicit <see cref="IReadOnlyList{T}" />.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The instance implementing <see cref="IReadOnlyList{T}" />. </param>
        /// <returns>
        ///     The instance as explicit <see cref="IReadOnlyList{T}" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A conversion to an explicit <see cref="IReadOnlyList{T}" /> can be useful in cases where the utility/extension methods of <see cref="IReadOnlyList{T}" /> shall be used instead of the ones implemented by the instance itself.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        public static IReadOnlyList<T> AsReadOnlyList <T> (this IReadOnlyList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            return list;
        }

        /// <summary>
        ///     Gets the item at the specified index or the default value if the index is outside the range of the list.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="index"> The index of the item to retrieve. </param>
        /// <returns>
        ///     The value or default value of <typeparamref name="T" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero. </exception>
        public static T RoGetIndexOrDefault <T> (this IReadOnlyList<T> list, int index)
        {
            return list.RoGetIndexOrDefault(index, default);
        }

        /// <summary>
        ///     Gets the item at the specified index or a default value if the index is outside the range of the list.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="index"> The index of the item to retrieve. </param>
        /// <param name="defaultValue"> The default value to use if the index is outside the range of the list. </param>
        /// <returns>
        ///     The value or <paramref name="defaultValue" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero. </exception>
        public static T RoGetIndexOrDefault <T> (this IReadOnlyList<T> list, int index, T defaultValue)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index >= list.Count)
            {
                return defaultValue;
            }

            return list[index];
        }

        /// <summary>
        ///     Gets the item at the specified index without removing it.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="index"> The index of the item to retrieve. </param>
        /// <returns>
        ///     The item at the specified index.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="InvalidOperationException"> <paramref name="list" /> contains no elements. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero or bigger or equal to the size of the list. </exception>
        public static T RoPeek <T> (this IReadOnlyList<T> list, int index)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.Count == 0)
            {
                throw new InvalidOperationException("The list contains no elements.");
            }

            if ((index < 0) || (index >= list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return list[index];
        }

        /// <summary>
        ///     Gets the item at the specified index without removing it after clamping the index to the size of the list.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="index"> The index of the item to retrieve. </param>
        /// <returns>
        ///     The item at the specified index.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Before used to access the list, <paramref name="index" /> is clamped between zero and the size of the list minus one.
        ///         If the list does not contain any elements, <see cref="InvalidOperationException" /> is thrown.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="InvalidOperationException"> <paramref name="list" /> contains no elements. </exception>
        public static T RoPeekClamp <T> (this IReadOnlyList<T> list, int index)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.Count == 0)
            {
                throw new InvalidOperationException("The list contains no elements.");
            }

            index = index.Clamp(0, list.Count - 1);

            return list[index];
        }

        /// <summary>
        ///     Gets an item at a random position without removing it.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="randomizer"> The randomizer which is used for generating the random position. </param>
        /// <returns>
        ///     An item at a random index.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="InvalidOperationException"> <paramref name="list" /> contains no elements. </exception>
        public static T RoPeekRandom <T> (this IReadOnlyList<T> list, Random randomizer)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (list.Count == 0)
            {
                throw new InvalidOperationException("The list contains no elements.");
            }

            int index = randomizer.Next(0, list.Count);

            return list[index];
        }

        /// <summary>
        ///     Gets the item at the specified index if that index exists.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="index"> The index of the item to retrieve. </param>
        /// <param name="item"> Receives the item if the index is valid, default(<typeparamref name="T" />) otherwise. </param>
        /// <returns>
        ///     true if the index is valid, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero. </exception>
        public static bool RoTryGetIndex <T> (this IReadOnlyList<T> list, int index, out T item)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            item = default;

            if (index >= list.Count)
            {
                return false;
            }

            item = list[index];
            return true;
        }

        #endregion
    }
}
