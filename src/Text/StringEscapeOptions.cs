using System;




namespace RI.Utilities.Text
{
    /// <summary>
    ///     Describes options used for escaping/unescaping strings.
    /// </summary>
    [Flags,]
    [Serializable,]
    public enum StringEscapeOptions
    {
        /// <summary>
        ///     <c> \a </c>
        /// </summary>
        Alert = 0x0001,

        /// <summary>
        ///     <c> \b </c>
        /// </summary>
        Backspace = 0x0002,

        /// <summary>
        ///     <c> \f </c>
        /// </summary>
        Formfeed = 0x0004,

        /// <summary>
        ///     <c> \n </c>
        /// </summary>
        Newline = 0x0008,

        /// <summary>
        ///     <c> \r </c>
        /// </summary>
        CarriageReturn = 0x0010,

        /// <summary>
        ///     <c> \t </c>
        /// </summary>
        HorizontalTab = 0x0020,

        /// <summary>
        ///     <c> \v </c>
        /// </summary>
        VerticalTap = 0x0040,

        /// <summary>
        ///     <c> \\ </c>
        /// </summary>
        Backslash = 0x0080,

        /// <summary>
        ///     <c> \' </c>
        /// </summary>
        SingleQuote = 0x0100,

        /// <summary>
        ///     <c> \&quot; </c>
        /// </summary>
        DoubleQuote = 0x0200,

        /// <summary>
        ///     <c> \? </c>
        /// </summary>
        QuestionMark = 0x400,

        /// <summary>
        ///     None.
        /// </summary>
        None = 0x0000,

        /// <summary>
        ///     All except <c> \? </c>.
        /// </summary>
        Default = StringEscapeOptions.Alert | StringEscapeOptions.Backspace | StringEscapeOptions.Formfeed | StringEscapeOptions.Newline | StringEscapeOptions.CarriageReturn | StringEscapeOptions.HorizontalTab | StringEscapeOptions.VerticalTap | StringEscapeOptions.Backslash | StringEscapeOptions.SingleQuote | StringEscapeOptions.DoubleQuote,

        /// <summary>
        ///     All except <c> \? </c> and <c> ' </c> and <c> " </c>.
        /// </summary>
        NonPrintable = StringEscapeOptions.Alert | StringEscapeOptions.Backspace | StringEscapeOptions.Formfeed | StringEscapeOptions.Newline | StringEscapeOptions.CarriageReturn | StringEscapeOptions.HorizontalTab | StringEscapeOptions.VerticalTap | StringEscapeOptions.Backslash,

        /// <summary>
        ///     All.
        /// </summary>
        All = StringEscapeOptions.Default | StringEscapeOptions.QuestionMark,

        /// <summary>
        ///     <c> \' </c> and <c> \&quot; </c>
        /// </summary>
        Quote = StringEscapeOptions.SingleQuote | StringEscapeOptions.DoubleQuote,

        /// <summary>
        ///     <c> \r </c> and <c> \n </c>
        /// </summary>
        LineFeed = StringEscapeOptions.Newline | StringEscapeOptions.CarriageReturn,
    }
}
