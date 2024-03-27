using System;
using System.Runtime.Serialization;




namespace RI.Utilities.Exceptions
{
    /// <summary>
    ///     The <see cref="NotAnEnumerationArgumentException" /> is thrown when an argument is not of an enumeration type.
    /// </summary>
    [Serializable,]
    public class NotAnEnumerationArgumentException : ArgumentException
    {
        #region Constants

        private const string ExceptionMessage = "The argument is not an enumeration.";

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="NotAnEnumerationArgumentException" />.
        /// </summary>
        public NotAnEnumerationArgumentException ()
            : base(NotAnEnumerationArgumentException.ExceptionMessage) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotAnEnumerationArgumentException" />.
        /// </summary>
        /// <param name="paramName"> The parameter which is not an enumeration. </param>
        public NotAnEnumerationArgumentException (string paramName)
            : base(NotAnEnumerationArgumentException.ExceptionMessage, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotAnEnumerationArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter which is not an enumeration. </param>
        public NotAnEnumerationArgumentException (string message, string paramName)
            : base(message, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotAnEnumerationArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public NotAnEnumerationArgumentException (string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotAnEnumerationArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter which is not an enumeration. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public NotAnEnumerationArgumentException (string message, string paramName, Exception innerException)
            : base(message, paramName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotAnEnumerationArgumentException" />.
        /// </summary>
        /// <param name="info"> The serialization data. </param>
        /// <param name="context"> The type of the source of the serialization data. </param>
        protected NotAnEnumerationArgumentException (SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
