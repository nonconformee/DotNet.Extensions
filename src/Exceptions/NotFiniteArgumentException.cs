using System;
using System.Runtime.Serialization;




namespace RI.Utilities.Exceptions
{
    /// <summary>
    ///     The <see cref="NotFiniteArgumentException" /> is thrown when a floating point argument is not a finite number (means: it is "NaN"/"Not-a-Number" or infinity (either positive or negative)).
    /// </summary>
    [Serializable,]
    public class NotFiniteArgumentException : ArgumentException
    {
        #region Constants

        private const string ExceptionMessage = "The floating-point argument is not a finite number.";

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="NotFiniteArgumentException" />.
        /// </summary>
        public NotFiniteArgumentException ()
            : base(NotFiniteArgumentException.ExceptionMessage) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotFiniteArgumentException" />.
        /// </summary>
        /// <param name="paramName"> The parameter which has a not-finite number. </param>
        public NotFiniteArgumentException (string paramName)
            : base(NotFiniteArgumentException.ExceptionMessage, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotFiniteArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter which has a not-finite number. </param>
        public NotFiniteArgumentException (string message, string paramName)
            : base(message, paramName) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotFiniteArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public NotFiniteArgumentException (string message, Exception innerException)
            : base(message, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotFiniteArgumentException" />.
        /// </summary>
        /// <param name="message"> The message which describes the exception. </param>
        /// <param name="paramName"> The parameter which has a not-finite number. </param>
        /// <param name="innerException"> The exception which triggered this exception. </param>
        public NotFiniteArgumentException (string message, string paramName, Exception innerException)
            : base(message, paramName, innerException) { }

        /// <summary>
        ///     Creates a new instance of <see cref="NotFiniteArgumentException" />.
        /// </summary>
        /// <param name="info"> The serialization data. </param>
        /// <param name="context"> The type of the source of the serialization data. </param>
        protected NotFiniteArgumentException (SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        #endregion
    }
}
