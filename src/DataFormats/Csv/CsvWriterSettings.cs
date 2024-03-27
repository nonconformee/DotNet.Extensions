namespace RI.Utilities.DataFormats.Csv
{
    /// <summary>
    ///     Provides CSV writer settings.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         CSV writer settings can be used to customize how CSV data is generated and formated.
    ///     </para>
    ///     <para>
    ///         See <see cref="CsvDocument" /> for more general and detailed information about working with CSV data.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class CsvWriterSettings : CsvSettings
    {
        #region Constants

        /// <summary>
        ///     The default whether values are always quoted.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default value is false.
        ///     </para>
        /// </remarks>
        public const bool DefaultAlwaysQuoteValues = false;

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets or sets whether values are always quoted.
        /// </summary>
        /// <value>
        ///     true if values are always quoted, false if they are only quoted when necessary.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The default value is <see cref="DefaultAlwaysQuoteValues" />.
        ///     </para>
        /// </remarks>
        public bool AlwaysQuoteValues { get; set; } = CsvWriterSettings.DefaultAlwaysQuoteValues;

        #endregion
    }
}
