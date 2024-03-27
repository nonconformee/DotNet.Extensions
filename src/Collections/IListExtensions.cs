using System;
using System.Collections.Generic;
using System.Linq;

using RI.Utilities.Comparison;
using RI.Utilities.Numbers;




namespace RI.Utilities.Collections
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="IList{T}" /> type and its implementations.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class IListExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Converts any instance implementing <see cref="IList{T}" /> to an explicit <see cref="IList{T}" />.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The instance implementing <see cref="IList{T}" />. </param>
        /// <returns>
        ///     The instance as explicit <see cref="IList{T}" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A conversion to an explicit <see cref="IList{T}" /> can be useful in cases where the utility/extension methods of <see cref="IList{T}" /> shall be used instead of the ones implemented by the instance itself.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        public static IList<T> AsList <T> (this IList<T> list)
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
        public static T GetIndexOrDefault <T> (this IList<T> list, int index)
        {
            return list.GetIndexOrDefault(index, default);
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
        public static T GetIndexOrDefault <T> (this IList<T> list, int index, T defaultValue)
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
        ///     Inserts multiple items into a list at the specified index.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="index"> The index at which the items are inserted. </param>
        /// <param name="items"> The sequence of items to insert into the list. </param>
        /// <returns>
        ///     The number of items inserted into the list.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="items" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> or <paramref name="items" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero or bigger than the size of the list. </exception>
        public static int InsertRange <T> (this IList<T> list, int index, IEnumerable<T> items)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if ((index < 0) || (index > list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int addedCount = 0;

            foreach (T value in items)
            {
                list.Insert(index + addedCount, value);
                addedCount++;
            }

            return addedCount;
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
        public static T Peek <T> (this IList<T> list, int index)
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
        public static T PeekClamp <T> (this IList<T> list, int index)
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
        public static T PeekRandom <T> (this IList<T> list, Random randomizer)
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
        ///     Gets and removes the item at the specified index.
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
        public static T Pop <T> (this IList<T> list, int index)
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

            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

        /// <summary>
        ///     Gets and removes the item at the specified index after clamping the index to the size of the list.
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
        public static T PopClamp <T> (this IList<T> list, int index)
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

            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

        /// <summary>
        ///     Gets and removes an item at a random position.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="randomizer"> The randomizer which is used for generating the random position. </param>
        /// <returns>
        ///     An item at a random index.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="InvalidOperationException"> <paramref name="list" /> contains no elements. </exception>
        public static T PopRandom <T> (this IList<T> list, Random randomizer)
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

            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

        /// <summary>
        ///     Removes a defined amount of items from a list at the specified index.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="index"> The index at which the remove of the items start. </param>
        /// <param name="count"> The number of items to remove beginning at the specified index. </param>
        /// <returns>
        ///     The number of items removed from the list.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If the range defined by <paramref name="index" /> and <paramref name="count" /> reaches outside the size of the list, all items from <paramref name="index" /> to the end of the list are removed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero or bigger or equal than the size of the list, or <paramref name="count" /> is less than zero. </exception>
        public static int RemoveAtRange <T> (this IList<T> list, int index, int count)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if ((index < 0) || (index >= list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            int removedCount;

            for (removedCount = 0; (removedCount < count) && (index < list.Count); removedCount++)
            {
                list.RemoveAt(index);
            }

            return removedCount;
        }

        /// <summary>
        ///     Removes multiple items at specified indices from a list.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="indices"> The sequence of indices to be removed from the list. </param>
        /// <returns>
        ///     The number of items removed from the list.
        ///     Zero if the sequence of indices contains no elements.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Before the indices are removed, they need to be de-duplicated and sorted, adding non-obvious overhead.
        ///         This is done by automatically.
        ///     </para>
        ///     <para>
        ///         <paramref name="indices" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> or <paramref name="indices" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> At least one index in <paramref name="indices" /> is invalid. </exception>
        public static int RemoveAtRange <T> (this IList<T> list, IEnumerable<int> indices)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (indices == null)
            {
                throw new ArgumentNullException(nameof(indices));
            }

            List<int> deduplicatedAndSorted = new List<int>(indices.Distinct());
            deduplicatedAndSorted.Sort();
            deduplicatedAndSorted.Reverse();

            for (int i1 = 0; i1 < deduplicatedAndSorted.Count; i1++)
            {
                if ((deduplicatedAndSorted[i1] < 0) || (deduplicatedAndSorted[i1] >= list.Count))
                {
                    throw new ArgumentOutOfRangeException(nameof(indices));
                }
            }

            for (int i1 = 0; i1 < deduplicatedAndSorted.Count; i1++)
            {
                list.RemoveAt(deduplicatedAndSorted[i1]);
            }

            return deduplicatedAndSorted.Count;
        }

        /// <summary>
        ///     Reverses the order of the items of a list.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <returns>
        ///     The number of reversed items.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Since the behaviour of the actual type of <paramref name="list" /> is not known, the reversing is not done in-place.
        ///         Instead, all items are copied and then reversed, the list cleared, and then all sorted items are added again.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        public static int Reverse <T> (this IList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            List<T> reversed = new List<T>(list);
            reversed.Reverse();
            list.Clear();
            list.AddRange(reversed);
            return list.Count;
        }

        /// <summary>
        ///     Puts all items in a list in random order.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="randomizer"> The randomizer which is used for randomizing the indices of the items. </param>
        /// <returns>
        ///     The number of items put into random order.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The list is completely shuffled.
        ///     </para>
        ///     <para>
        ///         Since the behaviour of the actual type of <paramref name="list" /> is not known, the shuffling is not done in-place.
        ///         Instead, all items are copied and then shuffled, the list cleared, and then all sorted items are added again.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> or <paramref name="randomizer" /> is null. </exception>
        public static int Shuffle <T> (this IList<T> list, Random randomizer)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            List<T> shuffled = new List<T>(list);

            for (int i1 = 0; i1 < (list.Count * 2); i1++)
            {
                int a = randomizer.Next(0, list.Count);
                int b = randomizer.Next(0, list.Count);
                shuffled.SwapInPlace(a, b);
            }

            list.Clear();
            list.AddRange(shuffled);
            return list.Count;
        }

        /// <summary>
        ///     Puts all items in a list in random order.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="randomizer"> The randomizer which is used for randomizing the indices of the items. </param>
        /// <param name="shakes"> The number of times the list is &quot;shaked&quot;. </param>
        /// <returns>
        ///     The number of items put into random order.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The list is partially shuffled.
        ///         That means that each item is moved randomly away from its original index.
        ///         The more <paramref name="shakes" />, the farther away an item might move from its original position.
        ///         A <paramref name="shakes" /> of zero means that the list remains the same and no items are shuffled.
        ///     </para>
        ///     <para>
        ///         Since the behaviour of the actual type of <paramref name="list" /> is not known, the shuffling is not done in-place.
        ///         Instead, all items are copied and then shuffled, the list cleared, and then all sorted items are added again.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> or <paramref name="randomizer" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="shakes" /> is below zero. </exception>
        public static int Shuffle <T> (this IList<T> list, Random randomizer, int shakes)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (randomizer == null)
            {
                throw new ArgumentNullException(nameof(randomizer));
            }

            if (shakes < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(shakes));
            }

            if (shakes == 0)
            {
                return list.Count;
            }

            List<T> shuffled = new List<T>(list);

            for (int i1 = 0; i1 < list.Count; i1++)
            {
                int newPosition = Math.Min(list.Count - 1, Math.Max(0, randomizer.Next(i1 - shakes, i1 + shakes + 1)));
                shuffled.SwapInPlace(i1, newPosition);
            }

            list.Clear();
            list.AddRange(shuffled);
            return list.Count;
        }

        /// <summary>
        ///     Sorts the items of a list.
        ///     Comparison is done using the default order comparison.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="ascending"> Specifies whether the sorting is done in ascending order (descending order otherwise). </param>
        /// <returns>
        ///     The number of sorted items.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Since the behaviour of the actual type of <paramref name="list" /> is not known, the sorting is not done in-place.
        ///         Instead, all items are copied and then sorted, the list cleared, and then all sorted items are added again.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        public static int Sort <T> (this IList<T> list, bool ascending)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            return list.Sort(ascending, Comparer<T>.Default.Compare);
        }

        /// <summary>
        ///     Sorts the items of a list.
        ///     Comparison is done using the specified order comparer.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="ascending"> Specifies whether the sorting is done in ascending order (descending order otherwise). </param>
        /// <param name="comparer"> The order comparer used to compare two items. </param>
        /// <returns>
        ///     The number of sorted items.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Since the behaviour of the actual type of <paramref name="list" /> is not known, the sorting is not done in-place.
        ///         Instead, all items are copied and then sorted, the list cleared, and then all sorted items are added again.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> or <paramref name="comparer" /> is null. </exception>
        public static int Sort <T> (this IList<T> list, bool ascending, IComparer<T> comparer)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return list.Sort(ascending, comparer.Compare);
        }

        /// <summary>
        ///     Sorts the items of a list.
        ///     Comparison is done using the specified function.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="ascending"> Specifies whether the sorting is done in ascending order (descending order otherwise). </param>
        /// <param name="comparer"> The function used to compare two items. </param>
        /// <returns>
        ///     The number of sorted items.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Since the behaviour of the actual type of <paramref name="list" /> is not known, the sorting is not done in-place.
        ///         Instead, all items are copied and then sorted, the list cleared, and then all sorted items are added again.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> or <paramref name="comparer" /> is null. </exception>
        public static int Sort <T> (this IList<T> list, bool ascending, Func<T, T, int> comparer)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            List<T> sorted = new List<T>(list);
            sorted.Sort(new OrderComparison<T>(!ascending, comparer));
            list.Clear();
            list.AddRange(sorted);
            return list.Count;
        }

        /// <summary>
        ///     Swaps two items in a list specified by their index.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="indexA"> The index of the first item to swap. </param>
        /// <param name="indexB"> The index of the second item to swap. </param>
        /// <remarks>
        ///     <para>
        ///         The swapping is done by copying the first and second item, then replacing the first and second item with default(<typeparamref name="T" />), and then replacing the first and second item with the swapped and copied items.
        ///         Therefore, briefly during the swapping, the list does not contain the first and second item but twice default(<typeparamref name="T" />) in their place.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="indexA" /> or <paramref name="indexB" /> is less than zero or bigger or equal to the size of the list. </exception>
        public static void SwapDefault <T> (this IList<T> list, int indexA, int indexB)
        {
            list.SwapDefault(indexA, indexB, default);
        }

        /// <summary>
        ///     Swaps two items in a list specified by their index.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="indexA"> The index of the first item to swap. </param>
        /// <param name="indexB"> The index of the second item to swap. </param>
        /// <param name="defaultValue"> The default value which is used as a placeholder during the swapping. </param>
        /// <remarks>
        ///     <para>
        ///         The swapping is done by copying the first and second item, then replacing the first and second item with the value of <paramref name="defaultValue" />, and then replacing the first and second item with the swapped and copied items.
        ///         Therefore, briefly during the swapping, the list does not contain the first and second item but twice the value of <paramref name="defaultValue" /> in their place.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="indexA" /> or <paramref name="indexB" /> is less than zero or bigger or equal to the size of the list. </exception>
        public static void SwapDefault <T> (this IList<T> list, int indexA, int indexB, T defaultValue)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if ((indexA < 0) || (indexA >= list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(indexA));
            }

            if ((indexB < 0) || (indexB >= list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(indexB));
            }

            if (indexA == indexB)
            {
                return;
            }

            T tempA = list[indexA];
            T tempB = list[indexB];
            list[indexA] = defaultValue;
            list[indexB] = defaultValue;
            list[indexA] = tempB;
            list[indexB] = tempA;
        }

        /// <summary>
        ///     Swaps two items in a list specified by their index.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="indexA"> The index of the first item to swap. </param>
        /// <param name="indexB"> The index of the second item to swap. </param>
        /// <remarks>
        ///     <para>
        ///         The swapping is done by copying the first item, then replacing the first item with the second item, and then replacing the second item with the copied first item.
        ///         Therefore, briefly during the swapping, the list does not contain the first item but twice the second item.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="indexA" /> or <paramref name="indexB" /> is less than zero or bigger or equal to the size of the list. </exception>
        public static void SwapInPlace <T> (this IList<T> list, int indexA, int indexB)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if ((indexA < 0) || (indexA >= list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(indexA));
            }

            if ((indexB < 0) || (indexB >= list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(indexB));
            }

            if (indexA == indexB)
            {
                return;
            }

            T temp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = temp;
        }

        /// <summary>
        ///     Swaps two items in a list specified by their index.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="indexA"> The index of the first item to swap. </param>
        /// <param name="indexB"> The index of the second item to swap. </param>
        /// <remarks>
        ///     <para>
        ///         The swapping is done by copying the first and second item, then removing the first and second item, and then inserting the first and second item at their swapped indices.
        ///         Therefore, briefly during the swapping, the list does not contain the first and second item and the list therefore changes size during the swapping.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="indexA" /> or <paramref name="indexB" /> is less than zero or bigger or equal to the size of the list. </exception>
        public static void SwapInsert <T> (this IList<T> list, int indexA, int indexB)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if ((indexA < 0) || (indexA >= list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(indexA));
            }

            if ((indexB < 0) || (indexB >= list.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(indexB));
            }

            if (indexA == indexB)
            {
                return;
            }

            int lower = indexA < indexB ? indexA : indexB;
            int higher = indexA > indexB ? indexA : indexB;
            T lowerItem = list[lower];
            T higherItem = list[higher];
            list.RemoveAt(higher);
            list.RemoveAt(lower);
            list.Insert(lower, higherItem);
            list.Insert(higher, lowerItem);
        }

        /// <summary>
        ///     Transforms each item in a list according to a specified transform function.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="transform"> The transform function which determines a new value for each item in the list, providing the item itself. </param>
        /// <returns>
        ///     The number of transformed items.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The transformation is done in-place where the new transformed value replaces the existing value at the same index.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> or <paramref name="transform" /> is null. </exception>
        public static int Transform <T> (this IList<T> list, Func<T, T> transform)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (transform == null)
            {
                throw new ArgumentNullException(nameof(transform));
            }

            for (int i1 = 0; i1 < list.Count; i1++)
            {
                list[i1] = transform(list[i1]);
            }

            return list.Count;
        }

        /// <summary>
        ///     Transforms each item in a list according to a specified transform function.
        /// </summary>
        /// <typeparam name="T"> The type of the items in <paramref name="list" />. </typeparam>
        /// <param name="list"> The list. </param>
        /// <param name="transform"> The transform function which determines a new value for each item in the list, providing the items index and the item itself. </param>
        /// <returns>
        ///     The number of transformed items.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The transformation is done in-place where the new transformed value replaces the existing value at the same index.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="list" /> or <paramref name="transform" /> is null. </exception>
        public static int Transform <T> (this IList<T> list, Func<int, T, T> transform)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (transform == null)
            {
                throw new ArgumentNullException(nameof(transform));
            }

            for (int i1 = 0; i1 < list.Count; i1++)
            {
                list[i1] = transform(i1, list[i1]);
            }

            return list.Count;
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
        public static bool TryGetIndex <T> (this IList<T> list, int index, out T item)
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
