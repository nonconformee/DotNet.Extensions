using System;




namespace RI.Utilities.Text
{
    /// <summary>
    ///     Describes the options for <see cref="StringExtensions.ToTechnical(string,TechnicalStringOptions)" /> and <see cref="StringExtensions.ToTechnical(string,TechnicalStringOptions,char?)" />
    /// </summary>
    [Serializable,]
    [Flags,]
    public enum TechnicalStringOptions
    {
        /// <summary>
        ///     No additional characters are allowed, only letters and digits.
        /// </summary>
        None = 0x00,

        /// <summary>
        ///     Whitespaces are allowed.
        /// </summary>
        AllowWhitespaces = 0x01,

        /// <summary>
        ///     Underscores (<c> _ </c>) are allowed.
        /// </summary>
        AllowUnderscores = 0x02,

        /// <summary>
        ///     Minus (<c> - </c>) are allowed.
        /// </summary>
        AllowMinus = 0x04,

        /// <summary>
        ///     Periods (<c> . </c>) are allowed.
        /// </summary>
        AllowPeriods = 0x08,

        /// <summary>
        ///     Allows everything which can be used for file and directory names and paths. Combination of <see cref="AllowWhitespaces" />, <see cref="AllowUnderscores" />, <see cref="AllowMinus" />, <see cref="AllowPeriods" />.
        /// </summary>
        FileCompatible = TechnicalStringOptions.AllowWhitespaces | TechnicalStringOptions.AllowUnderscores | TechnicalStringOptions.AllowMinus | TechnicalStringOptions.AllowPeriods,

        /// <summary>
        ///     Allows everything which can be used for file and directory names and paths, except whitespaces. Combination of <see cref="AllowUnderscores" />, <see cref="AllowMinus" />, <see cref="AllowPeriods" />.
        /// </summary>
        FileCompatibleNoWhitespaces = TechnicalStringOptions.AllowUnderscores | TechnicalStringOptions.AllowMinus | TechnicalStringOptions.AllowPeriods,
    }
}
