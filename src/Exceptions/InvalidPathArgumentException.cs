using System;
using System.Runtime.Serialization;

using RI.Utilities.Paths;




namespace RI.Utilities.Exceptions
{
    /// <summary>
    ///     The <see cref="InvalidPathArgumentException" /> is thrown when an argument is not a valid path or a path of an unexpected type.
    /// </summary>
    [Serializable,]
    public class InvalidPathArgumentException : ArgumentException
    {
        #region Constants

        private const string ExceptionMessageWithError = "The path argument is invalid (path error: {0}).";

        private const string ExceptionMessageWithoutError = "The path argument is invalid.";

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidPathArgumentException" />.
        /// </summary>
        public InvalidPathArgumentException ()
            : base(InvalidPathArgumentException.ExceptionMessageWithoutError) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidPathArgumentException" />.
        /// </summary>
        /// <param name="paramName"> The parameter whose path is invalid. </param>
        public InvalidPathArgumentException (string paramName)
            : base(InvalidPathArgumentException.ExceptionMessageWithoutError, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidPathArgumentException" />.
        /// </summary>
        /// <param name="paramName"> The parameter whose path is invalid. </param>
        /// <param name="error"> The path error as analyzed by <see cref="PathProperties" />. </param>
        public InvalidPathArgumentException (string paramName, PathError error)
            : base(string.Format(InvalidPathArgumentException.ExceptionMessageWithError, error), paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidPathArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter whose path is invalid. </param>
        public InvalidPathArgumentException (string message, string paramName)
            : base(message, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidPathArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public InvalidPathArgumentException (string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="EmptyStringArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter whose path is invalid. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public InvalidPathArgumentException (string message, string paramName, Exception innerException)
            : base(message, paramName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="InvalidPathArgumentException" />.
        /// </summary>
        /// <param name="info"> The serialization data. </param>
        /// <param name="context"> The type of the source of the serialization data. </param>
        protected InvalidPathArgumentException (SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
