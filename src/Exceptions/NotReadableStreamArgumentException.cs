using System;
using System.IO;
using System.Runtime.Serialization;




namespace RI.Utilities.Exceptions
{
    /// <summary>
    ///     The <see cref="NotReadableStreamArgumentException" /> is thrown when a <see cref="Stream" /> argument cannot be read.
    /// </summary>
    [Serializable,]
    public class NotReadableStreamArgumentException : ArgumentException
    {
        #region Constants

        private const string ExceptionMessage = "The Stream argument is not readable.";

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="NotReadableStreamArgumentException" />.
        /// </summary>
        public NotReadableStreamArgumentException ()
            : base(NotReadableStreamArgumentException.ExceptionMessage) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotReadableStreamArgumentException" />.
        /// </summary>
        /// <param name="paramName"> The parameter which is a not readable <see cref="Stream" />. </param>
        public NotReadableStreamArgumentException (string paramName)
            : base(NotReadableStreamArgumentException.ExceptionMessage, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotReadableStreamArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter which is a not readable <see cref="Stream" />. </param>
        public NotReadableStreamArgumentException (string message, string paramName)
            : base(message, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotReadableStreamArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public NotReadableStreamArgumentException (string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotFiniteArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter which is a not readable <see cref="Stream" />. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public NotReadableStreamArgumentException (string message, string paramName, Exception innerException)
            : base(message, paramName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotReadableStreamArgumentException" />.
        /// </summary>
        /// <param name="info"> The serialization data. </param>
        /// <param name="context"> The type of the source of the serialization data. </param>
        protected NotReadableStreamArgumentException (SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
