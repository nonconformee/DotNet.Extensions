using System;
using System.Collections.Generic;




namespace RI.Utilities.Collections
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="IReadOnlyDictionary{TKey,TValue}" /> type and its implementations.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The <c> Ro </c> prefix is added to some extension methods to avoid ambiguity with extension methods from <see cref="IDictionaryExtensions" />.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public static class IReadOnlyDictionaryExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Converts any instance implementing <see cref="IReadOnlyDictionary{TKey,TValue}" /> to an explicit <see cref="IReadOnlyDictionary{TKey,TValue}" />.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The instance implementing <see cref="IReadOnlyDictionary{TKey,TValue}" />. </param>
        /// <returns>
        ///     The instance as explicit <see cref="IReadOnlyDictionary{TKey,TValue}" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A conversion to an explicit <see cref="IReadOnlyDictionary{TKey,TValue}" /> can be useful in cases where the utility/extension methods of <see cref="IReadOnlyDictionary{TKey,TValue}" /> shall be used instead of the ones implemented by the instance itself.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> is null. </exception>
        public static IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return dictionary;
        }

        /// <summary>
        ///     Determines whether a dictionary contains a specified key.
        ///     Comparison is done using the specified equality comparer.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="key"> The key to find in the dictionary. </param>
        /// <param name="comparer"> The equality comparer used to compare the specified key and the keys in the dictionary to look for a match. </param>
        /// <returns>
        ///     true if the dictionary contains the key, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> or <paramref name="comparer" /> is null. </exception>
        public static bool RoContainsKey <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, IEqualityComparer<TKey> comparer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return dictionary.RoContainsKey(key, comparer.Equals);
        }

        /// <summary>
        ///     Determines whether a dictionary contains a specified key.
        ///     Comparison is done using the specified function.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="key"> The key to find in the dictionary. </param>
        /// <param name="comparer"> The function used to compare the specified key and the keys in the dictionary to look for a match. </param>
        /// <returns>
        ///     true if the dictionary contains the key, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> or <paramref name="comparer" /> is null. </exception>
        public static bool RoContainsKey <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TKey, bool> comparer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (comparer(pair.Key, key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Determines whether a dictionary contains a specified value.
        ///     Comparison is done using the default equality comparison.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="value"> The value to find in the dictionary. </param>
        /// <returns>
        ///     true if the dictionary contains the value at least once, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> is null. </exception>
        public static bool RoContainsValue <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary, TValue value)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return dictionary.RoContainsValue(value, EqualityComparer<TValue>.Default.Equals);
        }

        /// <summary>
        ///     Determines whether a dictionary contains a specified value.
        ///     Comparison is done using the specified equality comparer.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="value"> The value to find in the dictionary. </param>
        /// <param name="comparer"> The equality comparer used to compare the specified key and the keys in the dictionary to look for a match. </param>
        /// <returns>
        ///     true if the dictionary contains the value at least once, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> or <paramref name="comparer" /> is null. </exception>
        public static bool RoContainsValue <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary, TValue value, IEqualityComparer<TValue> comparer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return dictionary.RoContainsValue(value, comparer.Equals);
        }

        /// <summary>
        ///     Determines whether a dictionary contains a specified value.
        ///     Comparison is done using the specified function.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="value"> The value to find in the dictionary. </param>
        /// <param name="comparer"> The function used to compare the specified key and the keys in the dictionary to look for a match. </param>
        /// <returns>
        ///     true if the dictionary contains the value at least once, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> or <paramref name="comparer" /> is null. </exception>
        public static bool RoContainsValue <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary, TValue value, Func<TValue, TValue, bool> comparer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (comparer(pair.Value, value))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Gets all keys of a dictionary.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <returns>
        ///     The list which contains all the keys of the dictionary.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> is null. </exception>
        public static List<TKey> RoGetKeys <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return new List<TKey>(dictionary.Keys);
        }

        /// <summary>
        ///     Gets all keys of a dictionary where the key-value-pair satisfy a specified condition.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="condition"> The function which tests each key-value-pair for a condition. </param>
        /// <returns>
        ///     The list which contains all the keys of the dictionary where the corresponding key-value-pair satisfied the specified condition.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> or <paramref name="condition" /> is null. </exception>
        public static List<TKey> RoGetKeys <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> condition)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            List<TKey> keys = new List<TKey>(dictionary.Count);

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (condition(pair))
                {
                    keys.Add(pair.Key);
                }
            }

            return keys;
        }

        /// <summary>
        ///     Gets a value from a dictionary or the default value if the key does not exist in the dictionary.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="key"> The key to get its value of. </param>
        /// <returns>
        ///     The value or default value of <typeparamref name="TValue" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> is null. </exception>
        public static TValue RoGetValueOrDefault <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.RoGetValueOrDefault(key, default);
        }

        /// <summary>
        ///     Gets a value from a dictionary or a default value if the key does not exist in the dictionary.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="key"> The key to get its value of. </param>
        /// <param name="defaultValue"> The default value to use if the key does not exist in the dictionary. </param>
        /// <returns>
        ///     The value or <paramref name="defaultValue" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> is null. </exception>
        public static TValue RoGetValueOrDefault <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return defaultValue;
        }

        /// <summary>
        ///     Gets all values of a dictionary.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <returns>
        ///     The list which contains all the values of the dictionary.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> is null. </exception>
        public static List<TValue> RoGetValues <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return new List<TValue>(dictionary.Values);
        }

        /// <summary>
        ///     Gets all values of a dictionary where the key-value-pair satisfy a specified condition.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="condition"> The function which tests each key-value-pair for a condition. </param>
        /// <returns>
        ///     The list which contains all the values of the dictionary where the corresponding key-value-pair satisfied the specified condition.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> or <paramref name="condition" /> is null. </exception>
        public static List<TValue> RoGetValues <TKey, TValue> (this IReadOnlyDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> condition)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            List<TValue> values = new List<TValue>(dictionary.Count);

            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (condition(pair))
                {
                    values.Add(pair.Value);
                }
            }

            return values;
        }

        #endregion
    }
}
