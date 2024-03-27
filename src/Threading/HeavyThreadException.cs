using System;
using System.Runtime.Serialization;




namespace RI.Utilities.Threading
{
    /// <summary>
    ///     The <see cref="HeavyThreadException" /> is thrown when the thread of a <see cref="HeavyThread" /> had an exception.
    /// </summary>
    [Serializable,]
    public class HeavyThreadException : Exception
    {
        #region Constants

        private const string ExceptionMessageWithException = "Exception in thread ({0}): {1}";

        private const string ExceptionMessageWithoutException = "Exception in thread.";

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="HeavyThreadException" />.
        /// </summary>
        public HeavyThreadException ()
            : base(HeavyThreadException.ExceptionMessageWithoutException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="HeavyThreadException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        public HeavyThreadException (string message)
            : base(message) { }

        /// <summary>
        ///     Creates a new instance of <see cref="HeavyThreadException" />.
        /// </summary>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public HeavyThreadException (Exception innerException)
            : base(string.Format(HeavyThreadException.ExceptionMessageWithException, innerException.GetType()
                                                                                                   .Name, innerException.Message), innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="HeavyThreadException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public HeavyThreadException (string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="HeavyThreadException" />.
        /// </summary>
        /// <param name="info"> The serialization data. </param>
        /// <param name="context"> The type of the source of the serialization data. </param>
        protected HeavyThreadException (SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
