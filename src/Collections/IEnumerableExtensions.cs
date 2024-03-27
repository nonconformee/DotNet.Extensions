using System;
using System.Collections;
using System.Collections.Generic;

using RI.Utilities.Comparison;




namespace RI.Utilities.Collections
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="IEnumerable{T}" /> type and its implementations.
    /// </summary>
    /// <remarks>
    ///     <note type="important">
    ///         The complexity stated for the operations provided by this class are under the assumption that enumerating an <see cref="IEnumerable{T}" /> has a complexity of O(n) where n is the number of elements in the sequence.
    ///     </note>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public static class IEnumerableExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Converts any instance implementing <see cref="IEnumerable{T}" /> to an explicit <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="enumerable" />. </typeparam>
        /// <param name="enumerable"> The instance implementing <see cref="IEnumerable{T}" />. </param>
        /// <returns>
        ///     The instance as explicit <see cref="IEnumerable{T}" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A conversion to an explicit <see cref="IEnumerable{T}" /> can be useful in cases where the utility/extension methods of <see cref="IEnumerable{T}" /> shall be used instead of the ones implemented by the instance itself.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" /> is null. </exception>
        public static IEnumerable<T> AsEnumerable <T> (this IEnumerable<T> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            return enumerable;
        }

        /// <summary>
        ///     Executes a specified action on each element of a sequence.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="enumerable" />. </typeparam>
        /// <param name="enumerable"> The sequence which contains the elements. </param>
        /// <param name="action"> The action to execute for each element, providing the element itself. </param>
        /// <returns>
        ///     The number of processed elements.
        ///     Zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n) operation where n is the number of elements in the sequence.
        ///     </para>
        ///     <para>
        ///         <paramref name="enumerable" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" /> or <paramref name="action" /> is null. </exception>
        public static int ForEach <T> (this IEnumerable<T> enumerable, Action<T> action)
        {
            return enumerable.ForEach((i, e) => action(e));
        }

        /// <summary>
        ///     Executes a specified action on each element of a sequence.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="enumerable" />. </typeparam>
        /// <param name="enumerable"> The sequence which contains the elements. </param>
        /// <param name="action"> The action to execute for each element, providing the elements index and the element itself. </param>
        /// <returns>
        ///     The number of processed elements.
        ///     Zero if the sequence is empty.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n) operation where n is the number of elements in the sequence.
        ///     </para>
        ///     <para>
        ///         <paramref name="enumerable" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" /> or <paramref name="action" /> is null. </exception>
        public static int ForEach <T> (this IEnumerable<T> enumerable, Action<int, T> action)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            int count = 0;

            foreach (T item in enumerable)
            {
                action(count, item);
                count++;
            }

            return count;
        }

        /// <summary>
        ///     Compares two sequences and determines whether they contain the same elements.
        ///     Comparison is done using the default equality comparison.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="x" /> and <paramref name="y" />. </typeparam>
        /// <param name="x"> The first of the two sequences whose elements are compared against the elements of <paramref name="y" />. </param>
        /// <param name="y"> The second of the two sequences whose elements are compared against the elements of <paramref name="x" />. </param>
        /// <returns>
        ///     true if both sequences are equal or contain the same elements respectively.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n+m) operation, where n is the number of elements in <paramref name="x" /> and m is the number of elements in <paramref name="y" />.
        ///     </para>
        ///     <para>
        ///         <paramref name="x" /> and <paramref name="y" /> are each enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="x" /> or <paramref name="y" /> is null. </exception>
        public static bool SequenceEqual <T> (this IEnumerable<T> x, IEnumerable<T> y)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            return CollectionComparer<T>.Default.Equals(x, y);
        }

        /// <summary>
        ///     Compares two sequences and determines whether they contain the same elements.
        ///     Comparison is done using the default equality comparison.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="x" /> and <paramref name="y" />. </typeparam>
        /// <param name="x"> The first of the two sequences whose elements are compared against the elements of <paramref name="y" />. </param>
        /// <param name="y"> The second of the two sequences whose elements are compared against the elements of <paramref name="x" />. </param>
        /// <param name="options"> The options which specify comparison options. </param>
        /// <returns>
        ///     true if both sequences are equal or contain the same elements respectively.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n*m) operation if order is ignored or a O(n+m) operation if order is not ignored, where n is the number of elements in <paramref name="x" /> and m is the number of elements in <paramref name="y" />.
        ///     </para>
        ///     <para>
        ///         <paramref name="x" /> and <paramref name="y" /> are each enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="x" /> or <paramref name="y" /> is null. </exception>
        public static bool SequenceEqual <T> (this IEnumerable<T> x, IEnumerable<T> y, CollectionComparerFlags options)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            CollectionComparer<T> collectionComparer = new CollectionComparer<T>(options);
            return collectionComparer.Equals(x, y);
        }

        /// <summary>
        ///     Compares two sequences and determines whether they contain the same elements.
        ///     Comparison is done using the specified equality comparer.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="x" /> and <paramref name="y" />. </typeparam>
        /// <param name="x"> The first of the two sequences whose elements are compared against the elements of <paramref name="y" />. </param>
        /// <param name="y"> The second of the two sequences whose elements are compared against the elements of <paramref name="x" />. </param>
        /// <param name="comparer"> The equality comparer used to compare the elements in the sequences to look for matches. </param>
        /// <returns>
        ///     true if both sequences are equal or contain the same elements respectively.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n*m) operation if order is ignored or a O(n+m) operation if order is not ignored, where n is the number of elements in <paramref name="x" /> and m is the number of elements in <paramref name="y" />.
        ///     </para>
        ///     <para>
        ///         <paramref name="x" /> and <paramref name="y" /> are each enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="x" />, <paramref name="y" />, or <paramref name="comparer" /> is null. </exception>
        public static bool SequenceEqual <T> (this IEnumerable<T> x, IEnumerable<T> y, IEqualityComparer<T> comparer)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            CollectionComparer<T> collectionComparer = new CollectionComparer<T>(comparer);
            return collectionComparer.Equals(x, y);
        }

        /// <summary>
        ///     Compares two sequences and determines whether they contain the same elements.
        ///     Comparison is done using the specified function.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="x" /> and <paramref name="y" />. </typeparam>
        /// <param name="x"> The first of the two sequences whose elements are compared against the elements of <paramref name="y" />. </param>
        /// <param name="y"> The second of the two sequences whose elements are compared against the elements of <paramref name="x" />. </param>
        /// <param name="comparer"> The function used to compare the elements in the sequences to look for matches. </param>
        /// <returns>
        ///     true if both sequences are equal or contain the same elements respectively.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n*m) operation if order is ignored or a O(n+m) operation if order is not ignored, where n is the number of elements in <paramref name="x" /> and m is the number of elements in <paramref name="y" />.
        ///     </para>
        ///     <para>
        ///         <paramref name="x" /> and <paramref name="y" /> are each enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="x" />, <paramref name="y" />, or <paramref name="comparer" /> is null. </exception>
        public static bool SequenceEqual <T> (this IEnumerable<T> x, IEnumerable<T> y, Func<T, T, bool> comparer)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            CollectionComparer<T> collectionComparer = new CollectionComparer<T>(comparer);
            return collectionComparer.Equals(x, y);
        }

        /// <summary>
        ///     Compares two sequences and determines whether they contain the same elements.
        ///     Comparison is done using the specified equality comparer.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="x" /> and <paramref name="y" />. </typeparam>
        /// <param name="x"> The first of the two sequences whose elements are compared against the elements of <paramref name="y" />. </param>
        /// <param name="y"> The second of the two sequences whose elements are compared against the elements of <paramref name="x" />. </param>
        /// <param name="options"> The options which specify comparison options. </param>
        /// <param name="comparer"> The equality comparer used to compare the elements in the sequences to look for matches. </param>
        /// <returns>
        ///     true if both sequences are equal or contain the same elements respectively.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n*m) operation if order is ignored or a O(n+m) operation if order is not ignored, where n is the number of elements in <paramref name="x" /> and m is the number of elements in <paramref name="y" />.
        ///     </para>
        ///     <para>
        ///         <paramref name="x" /> and <paramref name="y" /> are each enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="x" />, <paramref name="y" />, or <paramref name="comparer" /> is null. </exception>
        public static bool SequenceEqual <T> (this IEnumerable<T> x, IEnumerable<T> y, CollectionComparerFlags options, IEqualityComparer<T> comparer)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            CollectionComparer<T> collectionComparer = new CollectionComparer<T>(options, comparer);
            return collectionComparer.Equals(x, y);
        }

        /// <summary>
        ///     Compares two sequences and determines whether they contain the same elements.
        ///     Comparison is done using the specified function.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="x" /> and <paramref name="y" />. </typeparam>
        /// <param name="x"> The first of the two sequences whose elements are compared against the elements of <paramref name="y" />. </param>
        /// <param name="y"> The second of the two sequences whose elements are compared against the elements of <paramref name="x" />. </param>
        /// <param name="options"> The options which specify comparison options. </param>
        /// <param name="comparer"> The function used to compare the elements in the sequences to look for matches. </param>
        /// <returns>
        ///     true if both sequences are equal or contain the same elements respectively.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n*m) operation if order is ignored or a O(n+m) operation if order is not ignored, where n is the number of elements in <paramref name="x" /> and m is the number of elements in <paramref name="y" />.
        ///     </para>
        ///     <para>
        ///         <paramref name="x" /> and <paramref name="y" /> are each enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="x" />, <paramref name="y" />, or <paramref name="comparer" /> is null. </exception>
        public static bool SequenceEqual <T> (this IEnumerable<T> x, IEnumerable<T> y, CollectionComparerFlags options, Func<T, T, bool> comparer)
        {
            if (x == null)
            {
                throw new ArgumentNullException(nameof(x));
            }

            if (y == null)
            {
                throw new ArgumentNullException(nameof(y));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            CollectionComparer<T> collectionComparer = new CollectionComparer<T>(options, comparer);
            return collectionComparer.Equals(x, y);
        }

        /// <summary>
        ///     Converts a sequence to a new array, starting at a specified index.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="enumerable" />. </typeparam>
        /// <param name="enumerable"> The sequence which contains the elements. </param>
        /// <param name="index"> The index from which the elements are copied to the array. </param>
        /// <returns>
        ///     An array which contains all elements of the sequence, starting at the specified index, in the order they were enumerated.
        ///     The array has a length of zero if the sequence contains no elements or the specified index is outside the sequence.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n) operation where n is the number of elements in the sequence.
        ///     </para>
        ///     <para>
        ///         <paramref name="enumerable" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero. </exception>
        public static T[] ToArray <T> (this IEnumerable<T> enumerable, int index)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return IEnumerableExtensions.ToListInternal(enumerable, index, -1)
                                        .ToArray();
        }

        /// <summary>
        ///     Converts a sequence to a new array, starting at a specified index for a specified number of elements.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="enumerable" />. </typeparam>
        /// <param name="enumerable"> The sequence which contains the elements. </param>
        /// <param name="index"> The index from which the elements are copied to the array. </param>
        /// <param name="count"> The number of elements to copy to the array, starting at the specified index. </param>
        /// <returns>
        ///     An array which contains the specified number of elements of the sequence, starting at the specified index, in the order they were enumerated.
        ///     The array has a length of zero if the sequence contains no elements, the specified index is outside the sequence, or <paramref name="count" /> is zero.
        ///     If the range specified by <paramref name="index" /> and <paramref name="count" /> reaches outside the sequence, the array stops at the last element of the sequence.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n) operation where n is the number of elements in the sequence.
        ///     </para>
        ///     <para>
        ///         <paramref name="enumerable" /> is enumerated only once and only up to the last element in the range specified by <paramref name="index" /> and <paramref name="count" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> or <paramref name="count" /> is less than zero. </exception>
        public static T[] ToArray <T> (this IEnumerable<T> enumerable, int index, int count)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return IEnumerableExtensions.ToListInternal(enumerable, index, count)
                                        .ToArray();
        }

        /// <summary>
        ///     Converts a sequence to a dictionary by deriving a key from each element.
        ///     Each key can be assigned to one or more elements.
        ///     Key equality is checked using the default equality comparer for the key type.
        /// </summary>
        /// <typeparam name="TIn"> The type of the elements of <paramref name="enumerable" />. </typeparam>
        /// <typeparam name="TKey"> The type of the derived keys in the dictionary. </typeparam>
        /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
        /// <param name="enumerable"> The sequence which contains the elements. </param>
        /// <param name="mapper"> The function which derives a key and a value for each element in the sequence. </param>
        /// <returns>
        ///     The dictionary which contains the key-value-pairs where each value is a list of values derived from the elements which have the same key derived/assigned.
        ///     The dictionary is empty if the sequence contains no elements.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n^2) operation where n is the number of elements in the sequence.
        ///     </para>
        ///     <para>
        ///         <paramref name="enumerable" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" /> or <paramref name="mapper" /> is null. </exception>
        public static Dictionary<TKey, List<TValue>> ToDictionaryList <TIn, TKey, TValue> (this IEnumerable<TIn> enumerable, Func<TIn, KeyValuePair<TKey, TValue>> mapper)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return IEnumerableExtensions.ToDictionaryListInternal(enumerable, null, mapper);
        }

        /// <summary>
        ///     Converts a sequence to a dictionary by deriving a key from each element.
        ///     Each key can be assigned to one or more elements.
        ///     Key equality is checked using the specified equality comparer for the key type.
        /// </summary>
        /// <typeparam name="TIn"> The type of the elements of <paramref name="enumerable" />. </typeparam>
        /// <typeparam name="TKey"> The type of the derived keys in the dictionary. </typeparam>
        /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
        /// <param name="enumerable"> The sequence which contains the elements. </param>
        /// <param name="keyComparer"> The equality comparer for the keys, used by the returned dictionary. </param>
        /// <param name="mapper"> The function which derives a key and a value for each element in the sequence. </param>
        /// <returns>
        ///     The dictionary which contains the key-value-pairs where each value is a list of values derived from the elements which have the same key derived/assigned.
        ///     The dictionary is empty if the sequence contains no elements.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n^2) operation where n is the number of elements in the sequence.
        ///     </para>
        ///     <para>
        ///         <paramref name="enumerable" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" />, <paramref name="keyComparer" />, or <paramref name="mapper" /> is null. </exception>
        public static Dictionary<TKey, List<TValue>> ToDictionaryList <TIn, TKey, TValue> (this IEnumerable<TIn> enumerable, IEqualityComparer<TKey> keyComparer, Func<TIn, KeyValuePair<TKey, TValue>> mapper)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (keyComparer == null)
            {
                throw new ArgumentNullException(nameof(keyComparer));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return IEnumerableExtensions.ToDictionaryListInternal(enumerable, keyComparer, mapper);
        }

        /// <summary>
        ///     Converts a sequence to a dictionary by deriving a key from each element.
        ///     Each key can be assigned to one or more elements.
        ///     Key equality is checked using the default equality comparer for the key type.
        /// </summary>
        /// <typeparam name="TIn"> The type of the elements of <paramref name="enumerable" />. </typeparam>
        /// <typeparam name="TKey"> The type of the derived keys in the dictionary. </typeparam>
        /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
        /// <param name="enumerable"> The sequence which contains the elements. </param>
        /// <param name="mapper"> The function which derives a key and a value for each element in the sequence. </param>
        /// <returns>
        ///     The dictionary which contains the key-value-pairs where each value is a set of values derived from the elements which have the same key derived/assigned.
        ///     The dictionary is empty if the sequence contains no elements.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n^2) operation where n is the number of elements in the sequence.
        ///     </para>
        ///     <para>
        ///         <paramref name="enumerable" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" /> or <paramref name="mapper" /> is null. </exception>
        public static Dictionary<TKey, HashSet<TValue>> ToDictionarySet <TIn, TKey, TValue> (this IEnumerable<TIn> enumerable, Func<TIn, KeyValuePair<TKey, TValue>> mapper)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return IEnumerableExtensions.ToDictionarySetInternal(enumerable, null, null, mapper);
        }

        /// <summary>
        ///     Converts a sequence to a dictionary by deriving a key from each element.
        ///     Each key can be assigned to one or more elements.
        ///     Key equality is checked using the specified equality comparer for the key type.
        /// </summary>
        /// <typeparam name="TIn"> The type of the elements of <paramref name="enumerable" />. </typeparam>
        /// <typeparam name="TKey"> The type of the derived keys in the dictionary. </typeparam>
        /// <typeparam name="TValue"> The type of the values in the dictionary. </typeparam>
        /// <param name="enumerable"> The sequence which contains the elements. </param>
        /// <param name="keyComparer"> The equality comparer for the keys, used by the returned dictionary. </param>
        /// <param name="setComparer"> The equality comparer for the values, used by the sets in the returned dictionary. </param>
        /// <param name="mapper"> The function which derives a key and a value for each element in the sequence. </param>
        /// <returns>
        ///     The dictionary which contains the key-value-pairs where each value is a set of values derived from the elements which have the same key derived/assigned.
        ///     The dictionary is empty if the sequence contains no elements.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n^2) operation where n is the number of elements in the sequence.
        ///     </para>
        ///     <para>
        ///         <paramref name="enumerable" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" />, <paramref name="keyComparer" />, <paramref name="setComparer" />, or <paramref name="mapper" /> is null. </exception>
        public static Dictionary<TKey, HashSet<TValue>> ToDictionarySet <TIn, TKey, TValue> (this IEnumerable<TIn> enumerable, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> setComparer, Func<TIn, KeyValuePair<TKey, TValue>> mapper)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (keyComparer == null)
            {
                throw new ArgumentNullException(nameof(keyComparer));
            }

            if (setComparer == null)
            {
                throw new ArgumentNullException(nameof(setComparer));
            }

            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            return IEnumerableExtensions.ToDictionarySetInternal(enumerable, keyComparer, setComparer, mapper);
        }

        /// <summary>
        ///     Converts a non-generic sequence to a new list.
        /// </summary>
        /// <param name="enumerable"> The sequence which contains the elements. </param>
        /// <returns>
        ///     A list which contains all elements of the sequence in the order they were enumerated.
        ///     The list has a length of zero if the sequence contains no elements.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n) operation where n is the number of elements in the sequence.
        ///     </para>
        ///     <para>
        ///         <paramref name="enumerable" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" /> is null. </exception>
        public static List<object> ToGenericList (this IEnumerable enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            List<object> result = new List<object>();

            foreach (object item in enumerable)
            {
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        ///     Converts a sequence to a new list, starting at a specified index.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="enumerable" />. </typeparam>
        /// <param name="enumerable"> The sequence which contains the elements. </param>
        /// <param name="index"> The index from which the elements are copied to the list. </param>
        /// <returns>
        ///     A list which contains all elements of the sequence, starting at the specified index, in the order they were enumerated.
        ///     The list has a length of zero if the sequence contains no elements or the specified index is outside the sequence.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n) operation where n is the number of elements in the sequence.
        ///     </para>
        ///     <para>
        ///         <paramref name="enumerable" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> is less than zero. </exception>
        public static List<T> ToList <T> (this IEnumerable<T> enumerable, int index)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return IEnumerableExtensions.ToListInternal(enumerable, index, -1);
        }

        /// <summary>
        ///     Converts a sequence to a new list, starting at a specified index for a specified number of elements.
        /// </summary>
        /// <typeparam name="T"> The type of the elements of <paramref name="enumerable" />. </typeparam>
        /// <param name="enumerable"> The sequence which contains the elements. </param>
        /// <param name="index"> The index from which the elements are copied to the list. </param>
        /// <param name="count"> The number of elements to copy to the list, starting at the specified index. </param>
        /// <returns>
        ///     A list which contains the specified number of elements of the sequence in the order they were enumerated.
        ///     The list has a length of zero if the sequence contains no elements, the specified index is outside the sequence, or <paramref name="count" /> is zero.
        ///     If the range specified by <paramref name="index" /> and <paramref name="count" /> reaches outside the sequence, the list stops at the last element of the sequence.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         This is a O(n) operation where n is the number of elements in the sequence.
        ///     </para>
        ///     <para>
        ///         <paramref name="enumerable" /> is enumerated only once and only up to the last element in the range specified by <paramref name="index" /> and <paramref name="count" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="enumerable" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="index" /> or <paramref name="count" /> is less than zero. </exception>
        public static List<T> ToList <T> (this IEnumerable<T> enumerable, int index, int count)
        {
            if (enumerable == null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            return IEnumerableExtensions.ToListInternal(enumerable, index, count);
        }

        private static Dictionary<TKey, List<TValue>> ToDictionaryListInternal <TIn, TKey, TValue> (IEnumerable<TIn> enumerable, IEqualityComparer<TKey> keyComparer, Func<TIn, KeyValuePair<TKey, TValue>> mapper)
        {
            Dictionary<TKey, List<TValue>> dictionary = new Dictionary<TKey, List<TValue>>(keyComparer ?? EqualityComparer<TKey>.Default);

            foreach (TIn item in enumerable)
            {
                KeyValuePair<TKey, TValue> pair = mapper(item);

                if (!dictionary.ContainsKey(pair.Key))
                {
                    dictionary.Add(pair.Key, new List<TValue>());
                }

                dictionary[pair.Key]
                    .Add(pair.Value);
            }

            return dictionary;
        }

        private static Dictionary<TKey, HashSet<TValue>> ToDictionarySetInternal <TIn, TKey, TValue> (IEnumerable<TIn> enumerable, IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> setComparer, Func<TIn, KeyValuePair<TKey, TValue>> mapper)
        {
            setComparer = setComparer ?? EqualityComparer<TValue>.Default;
            Dictionary<TKey, HashSet<TValue>> dictionary = new Dictionary<TKey, HashSet<TValue>>(keyComparer ?? EqualityComparer<TKey>.Default);

            foreach (TIn item in enumerable)
            {
                KeyValuePair<TKey, TValue> pair = mapper(item);

                if (!dictionary.ContainsKey(pair.Key))
                {
                    dictionary.Add(pair.Key, new HashSet<TValue>(setComparer));
                }

                dictionary[pair.Key]
                    .Add(pair.Value);
            }

            return dictionary;
        }

        private static List<T> ToListInternal <T> (IEnumerable<T> enumerable, int index, int count)
        {
            List<T> items = new List<T>();
            int currentIndex = 0;

            foreach (T item in enumerable)
            {
                if (currentIndex >= index)
                {
                    if ((currentIndex >= (index + count)) && (count != -1))
                    {
                        break;
                    }

                    items.Add(item);
                }

                currentIndex++;
            }

            return items;
        }

        #endregion
    }
}
