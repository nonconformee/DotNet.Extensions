using System;




namespace RI.Utilities.DataFormats.Csv
{
    /// <summary>
    ///     Describes an error which ocurred during reading CSV data using an <see cref="CsvReader" />.
    /// </summary>
    [Serializable,]
    public enum CsvReaderError
    {
        /// <summary>
        ///     No error (no line read or the last line which was read is valid).
        /// </summary>
        None = 0,

        /// <summary>
        ///     A quote was read unexpected, inside a value which was not started with a quote.
        /// </summary>
        UnexpectedQuote = 1,

        /// <summary>
        ///     A separator was expected but another unexpected character was read.
        /// </summary>
        SeparatorExpected = 2,

        /// <summary>
        ///     A multiline value was read but is not allowed.
        /// </summary>
        MultilineValueNotAllowed = 3,
    }
}
