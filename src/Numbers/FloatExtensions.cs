namespace RI.Utilities.Numbers
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="float" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class FloatExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Gets the number or the default value (0.0f) if a single precision floating point number is "NaN"/"Not-a-Number" or infinity (positive or negative).
        /// </summary>
        /// <param name="value"> The single precision floating point number. </param>
        /// <returns>
        ///     Zero if the number is "NaN"/"Not-a-Number" or infinity (either positive or negative), <paramref name="value" /> otherwise.
        /// </returns>
        public static float GetValueOrDefault (this float value)
        {
            return value.IsNanOrInfinity() ? 0.0f : value;
        }

        /// <summary>
        ///     Gets the number or a specified default value if a single precision floating point number is "NaN"/"Not-a-Number" or infinity (positive or negative).
        /// </summary>
        /// <param name="value"> The single precision floating point number. </param>
        /// <param name="valueIfNanOrInfinity"> The value to return when <paramref name="value" /> is "NaN"/"Not-a-Number" or infinity. </param>
        /// <returns>
        ///     <paramref name="valueIfNanOrInfinity" /> if the number is "NaN"/"Not-a-Number" or infinity (either positive or negative), <paramref name="value" /> otherwise.
        /// </returns>
        public static float GetValueOrDefault (this float value, float valueIfNanOrInfinity)
        {
            return value.IsNanOrInfinity() ? valueIfNanOrInfinity : value;
        }

        /// <summary>
        ///     Determines whether a single precision floating point number is infinity (positive or negative).
        /// </summary>
        /// <param name="value"> The single precision floating point number. </param>
        /// <returns>
        ///     true if the number is infinity (either positive or negative), false otherwise.
        /// </returns>
        public static bool IsInfinity (this float value)
        {
            return float.IsInfinity(value);
        }

        /// <summary>
        ///     Determines whether a single precision floating point number is "NaN"/"Not-a-Number".
        /// </summary>
        /// <param name="value"> The single precision floating point number. </param>
        /// <returns>
        ///     true if the number is "NaN"/"Not-a-Number", false otherwise.
        /// </returns>
        public static bool IsNan (this float value)
        {
            return float.IsNaN(value);
        }

        /// <summary>
        ///     Determines whether a single precision floating point number is "NaN"/"Not-a-Number" or infinity (positive or negative).
        /// </summary>
        /// <param name="value"> The single precision floating point number. </param>
        /// <returns>
        ///     true if the number is "NaN"/"Not-a-Number" or infinity (either positive or negative), false otherwise.
        /// </returns>
        public static bool IsNanOrInfinity (this float value)
        {
            return float.IsNaN(value) || float.IsInfinity(value);
        }

        /// <summary>
        ///     Determines whether a single precision floating point number is neither "NaN"/"Not-a-Number" nor infinity (positive or negative).
        /// </summary>
        /// <param name="value"> The single precision floating point number. </param>
        /// <returns>
        ///     true if the number is neither "NaN"/"Not-a-Number" nor infinity (either positive or negative) but rather a real number, false otherwise.
        /// </returns>
        public static bool IsNumber (this float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value);
        }

        #endregion
    }
}
