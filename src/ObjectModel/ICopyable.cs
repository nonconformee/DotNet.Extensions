using System;




namespace RI.Utilities.ObjectModel
{
    /// <summary>
    ///     Defines an interface to implement copying of an object to another of the same type.
    /// </summary>
    /// <typeparam name="T"> The type to copy. </typeparam>
    public interface ICopyable <in T>
    {
        /// <summary>
        ///     Copies the content of the object.
        /// </summary>
        /// <param name="other"> The other object which gets the values from the current object. </param>
        /// <remarks>
        ///     <note type="implement">
        ///         Whether it is a deep or shallow copy depends on the implementing type and its context.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="other" /> is null. </exception>
        void CopyTo (T other);
    }
}
