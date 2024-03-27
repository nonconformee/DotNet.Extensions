using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;




namespace RI.Utilities.Collections
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="KeyedCollection{TKey,TItem}" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class KeyedCollectionExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Determines whether a keyed collection contains a specified key.
        ///     Comparison is done using the default equality comparison.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="keyedCollection" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="keyedCollection" />. </typeparam>
        /// <param name="keyedCollection"> The keyed collection. </param>
        /// <param name="key"> The key to find in the keyed collection. </param>
        /// <returns>
        ///     true if the keyed collection contains the key, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="keyedCollection" /> is null. </exception>
        public static bool ContainsKey <TKey, TValue> (this KeyedCollection<TKey, TValue> keyedCollection, TKey key)
        {
            if (keyedCollection == null)
            {
                throw new ArgumentNullException(nameof(keyedCollection));
            }

            return keyedCollection.Contains(key);
        }

        /// <summary>
        ///     Determines whether a keyed collection contains a specified value.
        ///     Comparison is done using the default equality comparison.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="keyedCollection" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="keyedCollection" />. </typeparam>
        /// <param name="keyedCollection"> The keyed collection. </param>
        /// <param name="value"> The value to find in the keyed collection. </param>
        /// <returns>
        ///     true if the keyed collection contains the value at least once, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="keyedCollection" /> is null. </exception>
        public static bool ContainsValue <TKey, TValue> (this KeyedCollection<TKey, TValue> keyedCollection, TValue value)
        {
            if (keyedCollection == null)
            {
                throw new ArgumentNullException(nameof(keyedCollection));
            }

            return keyedCollection.ContainsValue(value, EqualityComparer<TValue>.Default.Equals);
        }

        /// <summary>
        ///     Determines whether a keyed collection contains a specified value.
        ///     Comparison is done using the specified equality comparer.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="keyedCollection" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="keyedCollection" />. </typeparam>
        /// <param name="keyedCollection"> The keyed collection. </param>
        /// <param name="value"> The value to find in the keyed collection. </param>
        /// <param name="comparer"> The equality comparer used to compare the specified key and the keys in the keyed collection to look for a match. </param>
        /// <returns>
        ///     true if the keyed collection contains the value at least once, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="keyedCollection" /> or <paramref name="comparer" /> is null. </exception>
        public static bool ContainsValue <TKey, TValue> (this KeyedCollection<TKey, TValue> keyedCollection, TValue value, IEqualityComparer<TValue> comparer)
        {
            if (keyedCollection == null)
            {
                throw new ArgumentNullException(nameof(keyedCollection));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return keyedCollection.ContainsValue(value, comparer.Equals);
        }

        /// <summary>
        ///     Determines whether a keyed collection contains a specified value.
        ///     Comparison is done using the specified function.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="keyedCollection" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="keyedCollection" />. </typeparam>
        /// <param name="keyedCollection"> The keyed collection. </param>
        /// <param name="value"> The value to find in the keyed collection. </param>
        /// <param name="comparer"> The function used to compare the specified key and the keys in the keyed collection to look for a match. </param>
        /// <returns>
        ///     true if the keyed collection contains the value at least once, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="keyedCollection" /> or <paramref name="comparer" /> is null. </exception>
        public static bool ContainsValue <TKey, TValue> (this KeyedCollection<TKey, TValue> keyedCollection, TValue value, Func<TValue, TValue, bool> comparer)
        {
            if (keyedCollection == null)
            {
                throw new ArgumentNullException(nameof(keyedCollection));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            foreach (TValue currentValue in keyedCollection)
            {
                if (comparer(currentValue, value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Gets a value from a keyed collection or the default value if the key does not exist in the keyed collection.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="keyedCollection" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="keyedCollection" />. </typeparam>
        /// <param name="keyedCollection"> The keyed collection. </param>
        /// <param name="key"> The key to get its value of. </param>
        /// <returns>
        ///     The value or default value of <typeparamref name="TValue" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="keyedCollection" /> is null. </exception>
        public static TValue GetValueOrDefault <TKey, TValue> (this KeyedCollection<TKey, TValue> keyedCollection, TKey key)
        {
            return keyedCollection.GetValueOrDefault(key, default);
        }

        /// <summary>
        ///     Gets a value from a keyed collection or a default value if the key does not exist in the keyed collection.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="keyedCollection" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="keyedCollection" />. </typeparam>
        /// <param name="keyedCollection"> The keyed collection. </param>
        /// <param name="key"> The key to get its value of. </param>
        /// <param name="defaultValue"> The default value to use if the key does not exist in the keyed collection. </param>
        /// <returns>
        ///     The value or <paramref name="defaultValue" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="keyedCollection" /> is null. </exception>
        public static TValue GetValueOrDefault <TKey, TValue> (this KeyedCollection<TKey, TValue> keyedCollection, TKey key, TValue defaultValue)
        {
            if (keyedCollection == null)
            {
                throw new ArgumentNullException(nameof(keyedCollection));
            }

            if (keyedCollection.ContainsKey(key))
            {
                return keyedCollection[key];
            }

            return defaultValue;
        }

        /// <summary>
        ///     Removes multiple key-value-pairs based on multiple keys from a keyed collection.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="keyedCollection" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="keyedCollection" />. </typeparam>
        /// <param name="keyedCollection"> The keyed collection. </param>
        /// <param name="keys"> The sequence of keys to remove from the keyed collection. </param>
        /// <returns>
        ///     The number of key-value-pairs removed from the keyed collection.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="keys" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="keyedCollection" /> or <paramref name="keys" /> is null. </exception>
        public static int RemoveRange <TKey, TValue> (this KeyedCollection<TKey, TValue> keyedCollection, IEnumerable<TKey> keys)
        {
            if (keyedCollection == null)
            {
                throw new ArgumentNullException(nameof(keyedCollection));
            }

            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            int removedCount = 0;

            foreach (TKey key in keys)
            {
                if (keyedCollection.Remove(key))
                {
                    removedCount++;
                }
            }

            return removedCount;
        }

        #endregion
    }
}
