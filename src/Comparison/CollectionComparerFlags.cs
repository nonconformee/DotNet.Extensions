using System;




namespace RI.Utilities.Comparison
{
    /// <summary>
    ///     Specifies comparison options when using <see cref="CollectionComparer{T}" /> to compare two collections.
    /// </summary>
    [Serializable,]
    [Flags,]
    public enum CollectionComparerFlags
    {
        /// <summary>
        ///     No options.
        /// </summary>
        None = 0x00,

        /// <summary>
        ///     Two collections are also considered equal if their elements are equal but in different orders.
        /// </summary>
        IgnoreOrder = 0x01,

        /// <summary>
        ///     Two collections are only considered equal if their elements are of the same reference, regardless of the elements own behaviour regarding equality.
        /// </summary>
        ReferenceEquality = 0x02,

        /// <summary>
        ///     The hash code of a collection is not based on the elements but rather on the collections own <see cref="object.GetHashCode" /> method.
        /// </summary>
        DoNotGetHashFromElements = 0x04,
    }
}
