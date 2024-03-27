using System;
using System.Runtime.Serialization;




namespace RI.Utilities.Exceptions
{
    /// <summary>
    ///     The <see cref="InvalidTypeArgumentException" /> is thrown when an argument is not of an expected or compatible type.
    /// </summary>
    [Serializable,]
    public class InvalidTypeArgumentException : ArgumentException
    {
        #region Constants

        private const string ExceptionMessageWithError = "The argument type is invalid (expected: {0}; actual: {1}).";

        private const string ExceptionMessageWithoutError = "The argument type is invalid.";

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidTypeArgumentException" />.
        /// </summary>
        public InvalidTypeArgumentException ()
            : base(InvalidTypeArgumentException.ExceptionMessageWithoutError) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidTypeArgumentException" />.
        /// </summary>
        /// <param name="paramName"> The parameter whose type is invalid. </param>
        public InvalidTypeArgumentException (string paramName)
            : base(InvalidTypeArgumentException.ExceptionMessageWithoutError, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidTypeArgumentException" />.
        /// </summary>
        /// <param name="paramName"> The parameter whose type is invalid. </param>
        /// <param name="expectedType"> The expected type of the argument. </param>
        /// <param name="actualType"> The actually provided type of the argument. </param>
        public InvalidTypeArgumentException (string paramName, Type expectedType, Type actualType)
            : base(string.Format(InvalidTypeArgumentException.ExceptionMessageWithError, expectedType.Name, actualType.Name), paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidTypeArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter whose type is invalid. </param>
        public InvalidTypeArgumentException (string message, string paramName)
            : base(message, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidTypeArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public InvalidTypeArgumentException (string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="EmptyStringArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter whose type is invalid. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public InvalidTypeArgumentException (string message, string paramName, Exception innerException)
            : base(message, paramName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidTypeArgumentException" />.
        /// </summary>
        /// <param name="info"> The serialization data. </param>
        /// <param name="context"> The type of the source of the serialization data. </param>
        protected InvalidTypeArgumentException (SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
