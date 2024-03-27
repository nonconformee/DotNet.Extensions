using System;
using System.Runtime.Serialization;




namespace RI.Utilities.DataFormats.Ini
{
    /// <summary>
    ///     The <see cref="IniParsingException" /> is thrown when invalid INI elements are encountered while parsing INI data.
    /// </summary>
    [Serializable,]
    public class IniParsingException : Exception
    {
        #region Constants

        private const string ErrorExceptionMessage = "INI parsing failed at line {0} with error {1}";

        private const string GenericExceptionMessage = "INI parsing failed.";

        private const string SpecificExceptionMessage = "INI parsing failed: {0}";

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="IniParsingException" />.
        /// </summary>
        /// <param name="lineNumber"> The line number where the parsing error ocurred. </param>
        /// <param name="readerError"> The parsing error which ocurred. </param>
        public IniParsingException (int lineNumber, IniReaderError readerError)
            : base(string.Format(IniParsingException.ErrorExceptionMessage, lineNumber, readerError)) { }

        /// <summary>
        ///     Creates a new instance of <see cref="IniParsingException" />.
        /// </summary>
        public IniParsingException ()
            : base(IniParsingException.GenericExceptionMessage) { }

        /// <summary>
        ///     Creates a new instance of <see cref="IniParsingException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        public IniParsingException (string message)
            : base(string.Format(IniParsingException.SpecificExceptionMessage, message)) { }

        /// <summary>
        ///     Creates a new instance of <see cref="IniParsingException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public IniParsingException (string message, Exception innerException)
            : base(string.Format(IniParsingException.SpecificExceptionMessage, message), innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="IniParsingException" />.
        /// </summary>
        /// <param name="info"> The serialization data. </param>
        /// <param name="context"> The type of the source of the serialization data. </param>
        private IniParsingException (SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
