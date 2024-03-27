using System;




namespace RI.Utilities.ObjectModel
{
    /// <summary>
    ///     Defines an interface to implement type-safe cloning.
    /// </summary>
    /// <typeparam name="T"> The type to clone. </typeparam>
    public interface ICloneable <out T> : ICloneable
    {
        /// <summary>
        ///     Clones the object.
        /// </summary>
        /// <returns> The cloned object. </returns>
        /// <remarks>
        ///     <note type="implement">
        ///         Whether it is a deep or shallow clone depends on the implementing type and its context.
        ///     </note>
        /// </remarks>
        new T Clone ();
    }
}
