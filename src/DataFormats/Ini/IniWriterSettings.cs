namespace RI.Utilities.DataFormats.Ini
{
    /// <summary>
    ///     Provides INI writer settings.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         INI writer settings can be used to customize how INI data is generated and formated.
    ///     </para>
    ///     <para>
    ///         See <see cref="IniDocument" /> for more general and detailed information about working with INI data.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class IniWriterSettings : IniSettings
    {
        #region Constants

        /// <summary>
        ///     The default whether an additional new line is written before a section header.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default value is false.
        ///     </para>
        /// </remarks>
        public const bool DefaultEmptyLineBeforeSectionHeader = false;

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets or sets whether an additional new line is written before a section header.
        /// </summary>
        /// <value>
        ///     true if an additional new line is written before a section header, false otherwise.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The default value is <see cref="DefaultEmptyLineBeforeSectionHeader" />.
        ///     </para>
        /// </remarks>
        public bool EmptyLineBeforeSectionHeader { get; set; } = IniWriterSettings.DefaultEmptyLineBeforeSectionHeader;

        #endregion
    }
}
