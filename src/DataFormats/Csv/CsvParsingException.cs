using System;
using System.Runtime.Serialization;




namespace RI.Utilities.DataFormats.Csv
{
    /// <summary>
    ///     The <see cref="CsvParsingException" /> is thrown when invalid CSV data is encountered while parsing CSV data.
    /// </summary>
    [Serializable,]
    public class CsvParsingException : Exception
    {
        #region Constants

        private const string ErrorExceptionMessage = "CSV parsing failed at line {0} with error {1}";

        private const string GenericExceptionMessage = "CSV parsing failed.";

        private const string SpecificExceptionMessage = "CSV parsing failed: {0}";

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="CsvParsingException" />.
        /// </summary>
        /// <param name="lineNumber"> The line number where the parsing error ocurred. </param>
        /// <param name="readerError"> The parsing error which ocurred. </param>
        public CsvParsingException (int lineNumber, CsvReaderError readerError)
            : base(string.Format(CsvParsingException.ErrorExceptionMessage, lineNumber, readerError)) { }

        /// <summary>
        ///     Creates a new instance of <see cref="CsvParsingException" />.
        /// </summary>
        public CsvParsingException ()
            : base(CsvParsingException.GenericExceptionMessage) { }

        /// <summary>
        ///     Creates a new instance of <see cref="CsvParsingException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        public CsvParsingException (string message)
            : base(string.Format(CsvParsingException.SpecificExceptionMessage, message)) { }

        /// <summary>
        ///     Creates a new instance of <see cref="CsvParsingException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public CsvParsingException (string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="CsvParsingException" />.
        /// </summary>
        /// <param name="info"> The serialization data. </param>
        /// <param name="context"> The type of the source of the serialization data. </param>
        private CsvParsingException (SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
