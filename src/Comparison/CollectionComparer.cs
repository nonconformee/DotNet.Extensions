using System;
using System.Collections.Generic;
using System.Linq;




namespace RI.Utilities.Comparison
{
    /// <summary>
    ///     Implements an <see cref="IEqualityComparer{T}" /> for comparing collections and their elements.
    /// </summary>
    /// <typeparam name="T"> The type of the elements in the collections being compared for equality. </typeparam>
    /// <remarks>
    ///     <note type="note">
    ///         Two collections are considered equal if they contain the same number of elements and each element in one collection has an equal element in the other collection.
    ///         The collections themselves are not compared for equality, only their elements.
    ///     </note>
    ///     <note type="important">
    ///         This implementation of <see cref="IEqualityComparer{T}" /> is only intended for equality comparison of elements of collections.
    ///         It should not be used in scenarios where hash values of the collections themselves are used, e.g. used as a hash provider for the collections when storing the collections in another collection (e.g. storing collections in a <see cref="HashSet{T}" /> or <see cref="Dictionary{TKey,TValue}" />).
    ///     </note>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    /// <example>
    ///     <code language="cs">
    /// <![CDATA[
    /// // create some collections of strings we want to compare
    /// var upperCaseAscending  = new List<string>    { "ANDREW", "BOB", "CHARLES" };
    /// var lowerCaseDescending = new List<string>    { "charles", "bob", "andrew" };
    /// var mixedCase           = new HashSet<string> { "Andrew", "Charles", "Bob" };
    /// 
    /// // create collection comparer with case-insensitive string comparison
    /// var comparer = new CollectionComparer<string>(StringComparer.OrdinalIgnoreCase);
    /// 
    /// comparer.Equals(upperCaseAscending, lowerCaseDescending); // returns "false"
    /// comparer.Equals(upperCaseAscending, mixedCase);           // undefined, HashSet<T> does not guarantee the order its elements are enumerated
    /// comparer.Equals(lowerCaseDescending, mixedCase);          // undefined, HashSet<T> does not guarantee the order its elements are enumerated
    /// 
    /// // create collection comparer with case-insensitive string comparison and ignore order
    /// comparer = new CollectionComparer<string>(CollectionComparerFlags.IgnoreOrder, StringComparer.OrdinalIgnoreCase);
    /// 
    /// comparer.Equals(upperCaseAscending, lowerCaseDescending); // returns "true"
    /// comparer.Equals(upperCaseAscending, mixedCase);           // returns "true"
    /// comparer.Equals(lowerCaseDescending, mixedCase);          // returns "true"
    /// ]]>
    /// </code>
    /// </example>
    public sealed class CollectionComparer <T> : IEqualityComparer<IEnumerable<T>>
    {
        #region Static Constructor/Destructor

        static CollectionComparer ()
        {
            CollectionComparer<T>.Default = new CollectionComparer<T>();
            CollectionComparer<T>.DefaultIgnoreOrder = new CollectionComparer<T>(CollectionComparerFlags.IgnoreOrder);
            CollectionComparer<T>.ReferenceEquality = new CollectionComparer<T>(CollectionComparerFlags.ReferenceEquality);
            CollectionComparer<T>.ReferenceEqualityIgnoreOrder = new CollectionComparer<T>(CollectionComparerFlags.ReferenceEquality | CollectionComparerFlags.IgnoreOrder);
        }

        #endregion




        #region Static Properties/Indexer

        /// <summary>
        ///     Provides default collection comparison behaviour: Element equality comparison is based on the type of <typeparamref name="T" /> (using <see cref="EqualityComparer{T}" />.<see cref="EqualityComparer{T}.Default" />) -and- order of elements matters.
        /// </summary>
        /// <value>
        ///     The collection comparer.
        /// </value>
        public static CollectionComparer<T> Default { get; }

        /// <summary>
        ///     Provides default collection comparison behaviour: Element equality comparison is based on the type of <typeparamref name="T" /> (using <see cref="EqualityComparer{T}" />.<see cref="EqualityComparer{T}.Default" />) -and- order of elements is ignored.
        /// </summary>
        /// <value>
        ///     The collection comparer.
        /// </value>
        public static CollectionComparer<T> DefaultIgnoreOrder { get; }

        /// <summary>
        ///     Provides specialized collection comparison behaviour: Only elements of the same reference are equal (using <see cref="object" />.<see cref="object.ReferenceEquals" />) -and- order of elements matters.
        /// </summary>
        /// <value>
        ///     The collection comparer.
        /// </value>
        public static CollectionComparer<T> ReferenceEquality { get; }

        /// <summary>
        ///     Provides specialized collection comparison behaviour: Only elements of the same reference are equal (using <see cref="object" />.<see cref="object.ReferenceEquals" />) -and- order of elements is ignored.
        /// </summary>
        /// <value>
        ///     The collection comparer.
        /// </value>
        public static CollectionComparer<T> ReferenceEqualityIgnoreOrder { get; }

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="CollectionComparer{T}" />.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         <see cref="CollectionComparerFlags" />.<see cref="CollectionComparerFlags.None" /> is used as comparison options.
        ///     </para>
        ///     <para>
        ///         Equality of elements is compared using the default equality comparer for the type of <typeparamref name="T" />, using <see cref="EqualityComparer{T}" />.<see cref="EqualityComparer{T}.Default" />.
        ///     </para>
        /// </remarks>
        public CollectionComparer ()
        {
            this.Initialize(CollectionComparerFlags.None, null);
        }

        /// <summary>
        ///     Creates a new instance of <see cref="CollectionComparer{T}" />.
        /// </summary>
        /// <param name="options"> The used comparison options. </param>
        /// <remarks>
        ///     <para>
        ///         Equality of elements is compared using the default equality comparer for the type of <typeparamref name="T" />, using <see cref="EqualityComparer{T}" />.<see cref="EqualityComparer{T}.Default" />.
        ///     </para>
        /// </remarks>
        public CollectionComparer (CollectionComparerFlags options)
        {
            this.Initialize(options, null);
        }

        /// <summary>
        ///     Creates a new instance of <see cref="CollectionComparer{T}" />.
        /// </summary>
        /// <param name="comparer"> The equality comparer used to compare elements for equality. Can be null to use the default equality comparer (<see cref="EqualityComparer{T}" />.<see cref="EqualityComparer{T}.Default" />). </param>
        /// <remarks>
        ///     <para>
        ///         <see cref="CollectionComparerFlags" />.<see cref="CollectionComparerFlags.None" /> is used as comparison options.
        ///     </para>
        ///     <para>
        ///         Equality of elements is compared using the specified equality comparer.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentException"> <paramref name="comparer" /> is not null and <see cref="CollectionComparerFlags" />.<see cref="CollectionComparerFlags.ReferenceEquality" /> is specifiead as comparison options. </exception>
        public CollectionComparer (IEqualityComparer<T> comparer)
        {
            this.Initialize(CollectionComparerFlags.None, comparer == null ? (Func<T, T, bool>)null : comparer.Equals);
        }

        /// <summary>
        ///     Creates a new instance of <see cref="CollectionComparer{T}" />.
        /// </summary>
        /// <param name="comparer"> The function used to compare elements for equality. Can be null to use the default equality comparer (<see cref="EqualityComparer{T}" />.<see cref="EqualityComparer{T}.Default" />). </param>
        /// <remarks>
        ///     <para>
        ///         <see cref="CollectionComparerFlags" />.<see cref="CollectionComparerFlags.None" /> is used as comparison options.
        ///     </para>
        ///     <para>
        ///         Equality of elements is compared using the specified function.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentException"> <paramref name="comparer" /> is not null and <see cref="CollectionComparerFlags" />.<see cref="CollectionComparerFlags.ReferenceEquality" /> is specifiead as comparison options. </exception>
        public CollectionComparer (Func<T, T, bool> comparer)
        {
            this.Initialize(CollectionComparerFlags.None, comparer);
        }

        /// <summary>
        ///     Creates a new instance of <see cref="CollectionComparer{T}" />.
        /// </summary>
        /// <param name="options"> The used comparison options. </param>
        /// <param name="comparer"> The equality comparer used to compare elements for equality. Can be null to use the default equality comparer (<see cref="EqualityComparer{T}" />.<see cref="EqualityComparer{T}.Default" />) or when reference equality is used (<see cref="CollectionComparerFlags" />.<see cref="CollectionComparerFlags.ReferenceEquality" /> with <paramref name="options" />). </param>
        /// <remarks>
        ///     <para>
        ///         Equality of elements is compared using the specified equality comparer.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentException"> <paramref name="comparer" /> is not null and <see cref="CollectionComparerFlags" />.<see cref="CollectionComparerFlags.ReferenceEquality" /> is specifiead as comparison options. </exception>
        public CollectionComparer (CollectionComparerFlags options, IEqualityComparer<T> comparer)
        {
            this.Initialize(options, comparer == null ? (Func<T, T, bool>)null : comparer.Equals);
        }

        /// <summary>
        ///     Creates a new instance of <see cref="CollectionComparer{T}" />.
        /// </summary>
        /// <param name="options"> The used comparison options. </param>
        /// <param name="comparer"> The function used to compare elements for equality. Can be null to use the default equality comparer (<see cref="EqualityComparer{T}" />.<see cref="EqualityComparer{T}.Default" />) or when reference equality is used (<see cref="CollectionComparerFlags" />.<see cref="CollectionComparerFlags.ReferenceEquality" /> with <paramref name="options" />). </param>
        /// <remarks>
        ///     <para>
        ///         Equality of elements is compared using the specified function.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentException"> <paramref name="comparer" /> is not null and <see cref="CollectionComparerFlags" />.<see cref="CollectionComparerFlags.ReferenceEquality" /> is specifiead as comparison options. </exception>
        public CollectionComparer (CollectionComparerFlags options, Func<T, T, bool> comparer)
        {
            this.Initialize(options, comparer);
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the used function which compares elements for equality.
        /// </summary>
        /// <value>
        ///     The used function which compares elements for equality.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         This delegate always points to the function which eventually does the comparison between two elements of the collections being compared.
        ///         If <see cref="CollectionComparer{T}" /> is constructed using an <see cref="IEqualityComparer{T}" /> or is using default equality comparison (using <see cref="EqualityComparer{T}" />.<see cref="EqualityComparer{T}.Default" />), the delegate points to the <see cref="IEqualityComparer{T}.Equals(T,T)" /> method of that equality comparer.
        ///         If <see cref="CollectionComparer{T}" /> is constructed using a comparison function, the delegate points to that function.
        ///         If the <see cref="CollectionComparer{T}" /> uses reference equality, the delegate points to an anonymous function which encapsulates <see cref="object" />.<see cref="object.ReferenceEquals" />.
        ///     </para>
        /// </remarks>
        public Func<T, T, bool> Comparer { get; private set; }

        /// <summary>
        ///     Gets the used comparison options.
        /// </summary>
        /// <value>
        ///     The used comparison options.
        /// </value>
        public CollectionComparerFlags Options { get; private set; }

        #endregion




        #region Instance Methods

        private void Initialize (CollectionComparerFlags options, Func<T, T, bool> comparer)
        {
            bool referenceEquality = (options & CollectionComparerFlags.ReferenceEquality) == CollectionComparerFlags.ReferenceEquality;

            if ((comparer != null) && referenceEquality)
            {
                throw new ArgumentException("Cannot use both reference quality and a comparer.");
            }

            this.Options = options;

            if (referenceEquality)
            {
                this.Comparer = (x, y) => ReferenceEquals(x, y);
            }
            else
            {
                this.Comparer = comparer ?? EqualityComparer<T>.Default.Equals;
            }
        }

        #endregion




        #region Interface: IEqualityComparer<IEnumerable<T>>

        /// <summary>
        ///     Determines whether two specified collections, or their elements respectively, are considered equal.
        /// </summary>
        /// <param name="x"> The first collection to compare with <paramref name="y" />. </param>
        /// <param name="y"> The second collection to compare with <paramref name="x" />. </param>
        /// <returns>
        ///     true if the collections are considered equal, false otherwise.
        ///     true is returned if both <paramref name="x" /> and <paramref name="y" /> are null.
        ///     false is returned if only one of <paramref name="x" /> and <paramref name="y" /> is null.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="x" /> and <paramref name="y" /> are each enumerated exactly once.
        ///         <paramref name="x" /> and <paramref name="y" /> are not enumerated if both are the same reference or either one is null.
        ///     </para>
        ///     <para>
        ///         This is a O(n*m) operation if order is ignored or a O(n+m) operation if order is not ignored, where n is the number of elements in <paramref name="x" /> and m is the number of elements in <paramref name="y" />.
        ///     </para>
        /// </remarks>
        public bool Equals (IEnumerable<T> x, IEnumerable<T> y)
        {
            if ((x == null) && (y == null))
            {
                return true;
            }

            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if ((x == null) || (y == null))
            {
                return false;
            }

            IList<T> xList = x as IList<T> ?? x.ToList();
            IList<T> yList = y as IList<T> ?? y.ToList();

            if (xList.Count != yList.Count)
            {
                return false;
            }

            bool ignoreOrder = (this.Options & CollectionComparerFlags.IgnoreOrder) == CollectionComparerFlags.IgnoreOrder;

            if (ignoreOrder)
            {
                for (int i1 = 0; i1 < xList.Count; i1++)
                {
                    T xValue = xList[i1];
                    bool found = false;

                    for (int i2 = 0; i2 < yList.Count; i2++)
                    {
                        T yValue = yList[i2];

                        if (this.Comparer(xValue, yValue))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int i1 = 0; i1 < xList.Count; i1++)
                {
                    T xValue = xList[i1];
                    T yValue = yList[i1];

                    if (!this.Comparer(xValue, yValue))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     Determines the hash value of a specified collection.
        /// </summary>
        /// <param name="obj"> The collection. </param>
        /// <returns>
        ///     The hash value of the collection.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The behaviour of this method is controlled by the presence or absence of the <see cref="CollectionComparerFlags.DoNotGetHashFromElements" /> option.
        ///     </para>
        ///     <para>
        ///         <paramref name="obj" />is enumerated exactly once.
        ///         <paramref name="obj" /> is not enumerated if the hash code is taken from the collection itself.
        ///     </para>
        ///     <para>
        ///         This is a O(1) operation if the hash code is taken from the collection itself, O(n) otherwise where n is the number of items in <paramref name="obj" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="obj" /> is null. </exception>
        int IEqualityComparer<IEnumerable<T>>.GetHashCode (IEnumerable<T> obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            bool useCollectionHash = (this.Options & CollectionComparerFlags.DoNotGetHashFromElements) == CollectionComparerFlags.DoNotGetHashFromElements;

            if (useCollectionHash)
            {
                return obj.GetHashCode();
            }

            long hash = 0;
            long count = 0;

            foreach (T item in obj)
            {
                hash += item?.GetHashCode() ?? 0;
                count++;
            }

            return (int)(count == 0 ? 0 : hash / count);
        }

        #endregion
    }
}
