using System;
using System.Runtime.Serialization;




namespace RI.Utilities.Exceptions
{
    /// <summary>
    ///     The <see cref="EmptyStringArgumentException" /> is thrown when a string argument is empty (has a length of zero or only whitespaces).
    /// </summary>
    [Serializable,]
    public class EmptyStringArgumentException : ArgumentException
    {
        #region Constants

        private const string ExceptionMessage = "The string argument is empty.";

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="EmptyStringArgumentException" />.
        /// </summary>
        public EmptyStringArgumentException ()
            : base(EmptyStringArgumentException.ExceptionMessage) { }

        /// <summary>
        ///     Creates a new instance of <see cref="EmptyStringArgumentException" />.
        /// </summary>
        /// <param name="paramName"> The parameter which has an empty string. </param>
        public EmptyStringArgumentException (string paramName)
            : base(EmptyStringArgumentException.ExceptionMessage, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="EmptyStringArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter which has an empty string. </param>
        public EmptyStringArgumentException (string message, string paramName)
            : base(message, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="EmptyStringArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public EmptyStringArgumentException (string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="EmptyStringArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter which has an empty string. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public EmptyStringArgumentException (string message, string paramName, Exception innerException)
            : base(message, paramName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="EmptyStringArgumentException" />.
        /// </summary>
        /// <param name="info"> The serialization data. </param>
        /// <param name="context"> The type of the source of the serialization data. </param>
        protected EmptyStringArgumentException (SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
