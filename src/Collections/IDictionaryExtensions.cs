using System;
using System.Collections.Generic;
using System.Linq;




namespace RI.Utilities.Collections
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="IDictionary{TKey,TValue}" /> type and its implementations.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class IDictionaryExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Adds a new value to or replaces an existing value in a dictionary, based on the specified key.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="key"> The key corresponding to the value to add or replace. </param>
        /// <param name="value"> The value to add or replace. </param>
        /// <returns>
        ///     true if the key did not already exist in the dictionary and was added, false if the key already existed and the existing value was overwritten.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> is null. </exception>
        public static bool AddOrReplace <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return false;
            }

            dictionary.Add(key, value);
            return true;
        }

        /// <summary>
        ///     Adds multiple items to a dictionary.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="keys"> The sequence of keys to add to the dictionary. </param>
        /// <param name="values"> The sequence of values to add to the dictionary. </param>
        /// <returns>
        ///     The number of items added to the dictionary.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="keys" /> and <paramref name="values" /> are enumerated and the first item in <paramref name="keys" /> is used as the key for the first item in <paramref name="values" /> and so forth.
        ///     </para>
        ///     <para>
        ///         The number of items in <paramref name="keys" /> and <paramref name="values" /> can be different.
        ///         If so, adding of items stops when the first of the two is done enumerating.
        ///     </para>
        ///     <para>
        ///         <paramref name="keys" /> and <paramref name="values" /> are enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" />, <paramref name="keys" />, or <paramref name="values" /> is null. </exception>
        public static int AddRange <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys, IEnumerable<TValue> values)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            List<TKey> keyList = keys.ToList();
            List<TValue> valueList = values.ToList();
            int count = Math.Min(keyList.Count, valueList.Count);

            for (int i1 = 0; i1 < count; i1++)
            {
                dictionary.Add(keyList[i1], valueList[i1]);
            }

            return count;
        }

        /// <summary>
        ///     Adds multiple items to a dictionary.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="keys"> The sequence of keys to add to the dictionary. </param>
        /// <param name="values"> The sequence of values to add to the dictionary. </param>
        /// <returns>
        ///     The number of items added to the dictionary.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="keys" /> and <paramref name="values" /> are enumerated and the first item in <paramref name="keys" /> is used as the key for the first item in <paramref name="values" /> and so forth.
        ///     </para>
        ///     <para>
        ///         The number of items in <paramref name="keys" /> and <paramref name="values" /> must match, otherwise an <see cref="ArgumentException" /> is thrown.
        ///     </para>
        ///     <para>
        ///         <paramref name="keys" /> and <paramref name="values" /> are enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" />, <paramref name="keys" />, or <paramref name="values" /> is null. </exception>
        /// <exception cref="ArgumentException"> <paramref name="keys" /> and <paramref name="values" /> do not contain the same amount of items. </exception>
        public static int AddRangeExact <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys, IEnumerable<TValue> values)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            List<TKey> keyList = keys.ToList();
            List<TValue> valueList = values.ToList();

            if (keyList.Count != valueList.Count)
            {
                throw new ArgumentException("The number of keys and values do not match.", nameof(values));
            }

            for (int i1 = 0; i1 < keyList.Count; i1++)
            {
                dictionary.Add(keyList[i1], valueList[i1]);
            }

            return keyList.Count;
        }

        /// <summary>
        ///     Converts any instance implementing <see cref="IDictionary{TKey,TValue}" /> to an explicit <see cref="IDictionary{TKey,TValue}" />.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The instance implementing <see cref="IDictionary{TKey,TValue}" />. </param>
        /// <returns>
        ///     The instance as explicit <see cref="IDictionary{TKey,TValue}" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A conversion to an explicit <see cref="IDictionary{TKey,TValue}" /> can be useful in cases where the utility/extension methods of <see cref="IDictionary{TKey,TValue}" /> shall be used instead of the ones implemented by the instance itself.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> is null. </exception>
        public static IDictionary<TKey, TValue> AsDictionary <TKey, TValue> (this IDictionary<TKey, TValue> dictionary)
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
        public static bool ContainsKey <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, IEqualityComparer<TKey> comparer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return dictionary.ContainsKey(key, comparer.Equals);
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
        public static bool ContainsKey <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TKey, bool> comparer)
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
        public static bool ContainsValue <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TValue value)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return dictionary.ContainsValue(value, EqualityComparer<TValue>.Default.Equals);
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
        public static bool ContainsValue <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TValue value, IEqualityComparer<TValue> comparer)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return dictionary.ContainsValue(value, comparer.Equals);
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
        public static bool ContainsValue <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TValue value, Func<TValue, TValue, bool> comparer)
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
        public static List<TKey> GetKeys <TKey, TValue> (this IDictionary<TKey, TValue> dictionary)
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
        public static List<TKey> GetKeys <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> condition)
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
        public static TValue GetValueOrDefault <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.GetValueOrDefault(key, default);
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
        public static TValue GetValueOrDefault <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue)
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
        public static List<TValue> GetValues <TKey, TValue> (this IDictionary<TKey, TValue> dictionary)
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
        public static List<TValue> GetValues <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> condition)
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

        /// <summary>
        ///     Removes multiple key-value-pairs based on multiple keys from a dictionary.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="keys"> The sequence of keys to remove from the dictionary. </param>
        /// <returns>
        ///     The number of key-value-pairs removed from the dictionary.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="keys" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> or <paramref name="keys" /> is null. </exception>
        public static int RemoveRange <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (keys == null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            int removedCount = 0;

            foreach (TKey key in keys)
            {
                if (dictionary.Remove(key))
                {
                    removedCount++;
                }
            }

            return removedCount;
        }

        /// <summary>
        ///     Removes all key-value-pairs from a dictionary which satisfy a specified condition.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="condition"> The function which tests each key-value-pair for a condition. </param>
        /// <returns>
        ///     The list of key-value-pairs removed from the dictionary.
        ///     The list is empty if no key-value-pairs were removed.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> or <paramref name="condition" /> is null. </exception>
        public static List<KeyValuePair<TKey, TValue>> RemoveWhere <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> condition)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            List<TKey> itemsToRemove = new List<TKey>(dictionary.Count);
            List<KeyValuePair<TKey, TValue>> removedItems = new List<KeyValuePair<TKey, TValue>>(dictionary.Count);

            foreach (KeyValuePair<TKey, TValue> item in dictionary)
            {
                if (condition(item))
                {
                    itemsToRemove.Add(item.Key);
                    removedItems.Add(item);
                }
            }

            for (int i1 = 0; i1 < itemsToRemove.Count; i1++)
            {
                dictionary.Remove(itemsToRemove[i1]);
            }

            return removedItems;
        }

        /// <summary>
        ///     Transforms the value of each key-value-pair in a dictionary according to a specified transform function.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="transform"> The transform function which determines a new value for each key-value-pair in the dictionary. </param>
        /// <returns>
        ///     The number of transformed key-value-pairs.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> or <paramref name="transform" /> is null. </exception>
        public static int Transform <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, TValue> transform)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (transform == null)
            {
                throw new ArgumentNullException(nameof(transform));
            }

            List<TKey> keys = dictionary.GetKeys();

            for (int i1 = 0; i1 < keys.Count; i1++)
            {
                TKey key = keys[i1];
                dictionary[key] = transform(new KeyValuePair<TKey, TValue>(key, dictionary[key]));
            }

            return dictionary.Count;
        }

        /// <summary>
        ///     Adds a new value to a dictionary if the specified key does not already exist in the dictionary.
        /// </summary>
        /// <typeparam name="TKey"> The type of the keys in <paramref name="dictionary" />. </typeparam>
        /// <typeparam name="TValue"> The type of the values in <paramref name="dictionary" />. </typeparam>
        /// <param name="dictionary"> The dictionary. </param>
        /// <param name="key"> The key to add. </param>
        /// <param name="value"> The value to add. </param>
        /// <returns>
        ///     true if the key did not already exist in the dictionary and was added, false if the key already existed and the existing value was not overwritten.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="dictionary" /> is null. </exception>
        public static bool TryAdd <TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (dictionary.ContainsKey(key))
            {
                return false;
            }

            dictionary.Add(key, value);
            return true;
        }

        #endregion
    }
}
