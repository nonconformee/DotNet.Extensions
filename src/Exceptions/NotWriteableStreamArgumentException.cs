using System;
using System.IO;
using System.Runtime.Serialization;




namespace RI.Utilities.Exceptions
{
    /// <summary>
    ///     The <see cref="NotWriteableStreamArgumentException" /> is thrown when a <see cref="Stream" /> argument cannot be written.
    /// </summary>
    [Serializable,]
    public class NotWriteableStreamArgumentException : ArgumentException
    {
        #region Constants

        private const string ExceptionMessage = "The Stream argument is not writeable.";

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="NotWriteableStreamArgumentException" />.
        /// </summary>
        public NotWriteableStreamArgumentException ()
            : base(NotWriteableStreamArgumentException.ExceptionMessage) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotWriteableStreamArgumentException" />.
        /// </summary>
        /// <param name="paramName"> The parameter which is a not writeable <see cref="Stream" />. </param>
        public NotWriteableStreamArgumentException (string paramName)
            : base(NotWriteableStreamArgumentException.ExceptionMessage, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotWriteableStreamArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter which is a not writeable <see cref="Stream" />. </param>
        public NotWriteableStreamArgumentException (string message, string paramName)
            : base(message, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotWriteableStreamArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public NotWriteableStreamArgumentException (string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotFiniteArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter which is a not writeable <see cref="Stream" />. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public NotWriteableStreamArgumentException (string message, string paramName, Exception innerException)
            : base(message, paramName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotWriteableStreamArgumentException" />.
        /// </summary>
        /// <param name="info"> The serialization data. </param>
        /// <param name="context"> The type of the source of the serialization data. </param>
        protected NotWriteableStreamArgumentException (SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
