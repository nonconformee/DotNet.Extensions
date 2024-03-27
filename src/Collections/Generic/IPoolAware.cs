namespace RI.Utilities.Collections.Generic
{
    /// <summary>
    ///     Supports pool awareness of items managed by an <see cref="IPool{T}" /> implementation.
    /// </summary>
    public interface IPoolAware
    {
        /// <summary>
        ///     Called after the object has been created by the pool.
        /// </summary>
        void Created ();

        /// <summary>
        ///     Called after the object has been removed from the pool.
        /// </summary>
        void Removed ();

        /// <summary>
        ///     Called after the object has been returned to the pool.
        /// </summary>
        void Returned ();

        /// <summary>
        ///     Called before the object is taken from the pool.
        /// </summary>
        void Taking ();
    }
}
