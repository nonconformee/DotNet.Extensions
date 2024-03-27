using System;
using System.Runtime.Serialization;




namespace RI.Utilities.Exceptions
{
    /// <summary>
    ///     The <see cref="InvalidGenericTypeArgumentException" /> is thrown when a generic argument is not of an expected or compatible type.
    /// </summary>
    [Serializable,]
    public class InvalidGenericTypeArgumentException : ArgumentException
    {
        #region Constants

        private const string ExceptionMessageWithError = "The generic argument type is invalid (expected: {0}; actual: {1}).";

        private const string ExceptionMessageWithoutError = "The generic argument type is invalid.";

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidGenericTypeArgumentException" />.
        /// </summary>
        public InvalidGenericTypeArgumentException ()
            : base(InvalidGenericTypeArgumentException.ExceptionMessageWithoutError) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidGenericTypeArgumentException" />.
        /// </summary>
        /// <param name="paramName"> The parameter whose type is invalid. </param>
        public InvalidGenericTypeArgumentException (string paramName)
            : base(InvalidGenericTypeArgumentException.ExceptionMessageWithoutError, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidGenericTypeArgumentException" />.
        /// </summary>
        /// <param name="paramName"> The parameter whose type is invalid. </param>
        /// <param name="expectedType"> The expected type of the argument. </param>
        /// <param name="actualType"> The actually provided type of the argument. </param>
        public InvalidGenericTypeArgumentException (string paramName, Type expectedType, Type actualType)
            : base(string.Format(InvalidGenericTypeArgumentException.ExceptionMessageWithError, expectedType.Name, actualType.Name), paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidGenericTypeArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter whose type is invalid. </param>
        public InvalidGenericTypeArgumentException (string message, string paramName)
            : base(message, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidGenericTypeArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public InvalidGenericTypeArgumentException (string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="EmptyStringArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter whose type is invalid. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public InvalidGenericTypeArgumentException (string message, string paramName, Exception innerException)
            : base(message, paramName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidGenericTypeArgumentException" />.
        /// </summary>
        /// <param name="info"> The serialization data. </param>
        /// <param name="context"> The type of the source of the serialization data. </param>
        protected InvalidGenericTypeArgumentException (SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
