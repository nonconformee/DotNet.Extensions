namespace RI.Utilities.DataFormats.Ini
{
    /// <summary>
    ///     Provides basic INI data settings for both readers and writers.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         See <see cref="IniDocument" /> for more general and detailed information about working with INI data.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public abstract class IniSettings
    {
        #region Constants

        /// <summary>
        ///     The default character which starts a comment.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default value is <c> ; </c>.
        ///     </para>
        /// </remarks>
        public const char DefaultCommentStart = ';';

        /// <summary>
        ///     The default character which is used to escape certain characters.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default value is <c> | </c>.
        ///     </para>
        /// </remarks>
        public const char DefaultEscapeCharacter = '|';

        /// <summary>
        ///     The default separator between a name and its value.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default value is <c> = </c>.
        ///     </para>
        /// </remarks>
        public const char DefaultNameValueSeparator = '=';

        /// <summary>
        ///     The default character which ends a section name.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default value is <c> ] </c>.
        ///     </para>
        /// </remarks>
        public const char DefaultSectionEnd = ']';

        /// <summary>
        ///     The default character which starts a section name.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default value is <c> [ </c>.
        ///     </para>
        /// </remarks>
        public const char DefaultSectionStart = '[';

        #endregion




        #region Instance Constructor/Destructor

        internal IniSettings () { }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets or sets the character which starts a comment.
        /// </summary>
        /// <value>
        ///     The character which starts a comment.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The default value is <see cref="DefaultCommentStart" />.
        ///     </para>
        /// </remarks>
        public char CommentStart { get; set; } = IniSettings.DefaultCommentStart;

        /// <summary>
        ///     Gets or sets the character which is used to escape certain characters.
        /// </summary>
        /// <value>
        ///     The character which is used to escape certain characters.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The default value is <see cref="DefaultEscapeCharacter" />.
        ///     </para>
        /// </remarks>
        public char EscapeCharacter { get; set; } = IniSettings.DefaultEscapeCharacter;

        /// <summary>
        ///     Gets or sets the separator between a name and its value.
        /// </summary>
        /// <value>
        ///     The separator between a name and its value.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The default value is <see cref="DefaultNameValueSeparator" />.
        ///     </para>
        /// </remarks>
        public char NameValueSeparator { get; set; } = IniSettings.DefaultNameValueSeparator;

        /// <summary>
        ///     Gets or sets the character which ends a section name.
        /// </summary>
        /// <value>
        ///     The character which ends a section name.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The default value is <see cref="DefaultSectionEnd" />.
        ///     </para>
        /// </remarks>
        public char SectionEnd { get; set; } = IniSettings.DefaultSectionEnd;

        /// <summary>
        ///     Gets or sets the character which starts a section name.
        /// </summary>
        /// <value>
        ///     The character which starts a section name.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The default value is <see cref="DefaultSectionStart" />.
        ///     </para>
        /// </remarks>
        public char SectionStart { get; set; } = IniSettings.DefaultSectionStart;

        #endregion
    }
}
