using System.Globalization;




namespace RI.Utilities.DataFormats.Csv
{
    /// <summary>
    ///     Provides basic CSV data settings for both readers and writers.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         See <see cref="CsvDocument" /> for more general and detailed information about working with CSV data.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public abstract class CsvSettings
    {
        #region Constants

        /// <summary>
        ///     The default character which is used to quote values.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default value is <c> &quot; </c>.
        ///     </para>
        /// </remarks>
        public const char DefaultQuote = '\"';

        /// <summary>
        ///     The fallback character which is used to separate values if it cannot be determined from the system.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default value is <c> , </c>.
        ///     </para>
        /// </remarks>
        public const char FallbackSeparator = ',';

        #endregion




        #region Instance Constructor/Destructor

        internal CsvSettings () { }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets or sets the character which is used to quote values.
        /// </summary>
        /// <value>
        ///     The character which is used to quote values.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The default value is <see cref="DefaultQuote" />.
        ///     </para>
        /// </remarks>
        public char Quote { get; set; } = CsvSettings.DefaultQuote;

        /// <summary>
        ///     Gets or sets the character which is used to separate values.
        /// </summary>
        /// <value>
        ///     The character which is used to separate values.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The default value is determined by <see cref="TextInfo.ListSeparator" />.
        ///         However, if that separator is not exactly one character in length, <see cref="FallbackSeparator" /> is used.
        ///     </para>
        /// </remarks>
        public char Separator { get; set; } = CultureInfo.CurrentCulture.TextInfo.ListSeparator.Length == 1 ? CultureInfo.CurrentCulture.TextInfo.ListSeparator[0] : CsvSettings.FallbackSeparator;

        #endregion
    }
}
