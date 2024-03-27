namespace RI.Utilities.Binary
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="bool" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class BoolExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Converts a boolean value into a number.
        /// </summary>
        /// <param name="value"> The boolean value. </param>
        /// <returns>
        ///     0 if <paramref name="value" /> is false, 1 if <paramref name="value" /> is true.
        /// </returns>
        public static byte ToBinary (this bool value)
        {
            return value ? (byte)1 : (byte)0;
        }

        #endregion
    }
}
