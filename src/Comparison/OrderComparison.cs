using System;
using System.Collections;
using System.Collections.Generic;




namespace RI.Utilities.Comparison
{
    /// <summary>
    ///     Implements an order comparer based on a order comparison function.
    /// </summary>
    /// <typeparam name="T"> The type of objects to compare for order. </typeparam>
    /// <remarks>
    ///     <para>
    ///         This order comparer can be used if an <see cref="IComparer{T}" /> or <see cref="IComparer" /> is required but order comparison should be handled by a custom function or lambda expression.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    /// <example>
    ///     <code language="cs">
    ///   <![CDATA[
    ///   // create a list of strings we want to sort
    ///   var names = new List<string> { "Andrew", "Charles", "Bob" };
    ///  
    ///   // create the comparer
    ///   // what we do: longer strings are always "greater than" shorter strings, strings of equal length use default case-insensitive comparison
    ///   var comparer = new OrderComparison<string>((x,y) => x.Length != y.Length ? x.Length.CompareTo(y.Length) : StringComparer.OrdinalIgnoreCase.Compare(x, y));
    ///   
    ///   // sort the strings based on our customized comparison
    ///   names.Sort(comparer);
    ///   ]]>
    ///   </code>
    /// </example>
    public sealed class OrderComparison <T> : IComparer<T>, IComparer
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="OrderComparison{T}" />.
        /// </summary>
        /// <param name="comparer"> The function which is used to compare two objects of type <typeparamref name="T" />. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="comparer" /> is null. </exception>
        public OrderComparison (Func<T, T, int> comparer)
            : this(false, comparer) { }

        /// <summary>
        ///     Creates a new instance of <see cref="OrderComparison{T}" />.
        /// </summary>
        /// <param name="reverseOrder"> Specifies whether the comparison uses reversed order (multiplying the comparison result by -1) or not. </param>
        /// <param name="comparer"> The function which is used to compare two objects of type <typeparamref name="T" />. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="comparer" /> is null. </exception>
        public OrderComparison (bool reverseOrder, Func<T, T, int> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            this.ReverseOrder = reverseOrder;
            this.Comparer = comparer;
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the function used to compare two objects.
        /// </summary>
        /// <value>
        ///     The function used to compare two objects.
        /// </value>
        public Func<T, T, int> Comparer { get; }

        /// <summary>
        ///     Gets whether the comparison is done in reverse order (multiplying the comparison result by -1) or not.
        /// </summary>
        /// <value>
        ///     true if the comparison is done in reverse order (multiplying the comparison result by -1), false otherwise.
        /// </value>
        public bool ReverseOrder { get; }

        #endregion




        #region Interface: IComparer

        /// <inheritdoc cref="OrderComparison{T}.Compare(T, T)" />
        /// <remarks>
        ///     <note type="note">
        ///         The return value is always -1 if one or both of <paramref name="x" /> and <paramref name="y" /> is null or not of type <typeparamref name="T" />.
        ///     </note>
        /// </remarks>
        int IComparer.Compare (object x, object y)
        {
            if (!(x is T) || !(y is T))
            {
                return -1;
            }

            return this.Compare((T)x, (T)y);
        }

        #endregion




        #region Interface: IComparer<T>

        /// <summary>
        ///     Compares two objects.
        /// </summary>
        /// <param name="x"> The first object to compare. </param>
        /// <param name="y"> The second object to compare </param>
        /// <returns>
        ///     The value which indicates wheter x is less than y (return value &lt; 0), equals y (return value = 0), or greater than y (return value &gt; 0).
        /// </returns>
        public int Compare (T x, T y)
        {
            return this.ReverseOrder ? this.Comparer(x, y) * -1 : this.Comparer(x, y);
        }

        #endregion
    }
}
