using System;
using System.Globalization;




namespace RI.Utilities.Globalization
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="CultureInfo" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class CultureInfoExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Determines whether two cultures have the same language.
        /// </summary>
        /// <param name="culture"> The first culture. </param>
        /// <param name="other"> The second culture. </param>
        /// <returns>
        ///     true if the two cultures have the same language, false otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Two cultures are considered language-equal if the have the same language part.
        ///         For example, <c> en-US </c> is language-equal to <c> en </c> or <c> en-GB </c>, but not to <c> de-DE </c> or <c> fr </c>.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="culture" /> or <paramref name="other" /> is null. </exception>
        public static bool EqualsLanguage (this CultureInfo culture, CultureInfo other)
        {
            if (culture == null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            CultureInfo x = culture.IsNeutralCulture ? culture : culture.Parent;
            CultureInfo y = other.IsNeutralCulture ? other : other.Parent;

            return x.Equals(y);
        }

        #endregion
    }
}
