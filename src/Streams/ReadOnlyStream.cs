using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;




namespace RI.Utilities.Streams
{
    /// <summary>
    ///     Implements a stream which wraps another stream and only allows read operations on that stream.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public sealed class ReadOnlyStream : Stream
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="ReadOnlyStream" />.
        /// </summary>
        /// <param name="stream"> The stream to wrap. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped stream is closed if this stream is closed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is null. </exception>
        public ReadOnlyStream (Stream stream)
            : this(stream, false) { }

        /// <summary>
        ///     Creates a new instance of <see cref="ReadOnlyStream" />.
        /// </summary>
        /// <param name="stream"> The stream to wrap. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped stream should be closed when this stream is closed (false) or kept open (true). </param>
        /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is null. </exception>
        public ReadOnlyStream (Stream stream, bool keepOpen)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            this.BaseStream = stream;
            this.KeepOpen = keepOpen;
        }

        /// <summary>
        ///     Garbage collects this instance of <see cref="ReadOnlyStream" />.
        /// </summary>
        ~ReadOnlyStream ()
        {
            this.Close();
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the wrapped stream.
        /// </summary>
        /// <value>
        ///     The wrapped stream.
        /// </value>
        public Stream BaseStream { get; private set; }

        private bool KeepOpen { get; }

        #endregion




        #region Instance Methods

        private bool CheckNotClosed ()
        {
            return this.BaseStream != null;
        }

        private void CloseInternal ()
        {
            if (this.BaseStream != null)
            {
                if (!this.KeepOpen)
                {
                    this.BaseStream.Close();
                }

                this.BaseStream = null;
            }
        }

        private void VerifyNotClosed ()
        {
            if (!this.CheckNotClosed())
            {
                throw new ObjectDisposedException(this.GetType()
                                                      .Name);
            }
        }

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override bool CanRead
        {
            get
            {
                this.VerifyNotClosed();
                return this.BaseStream.CanRead;
            }
        }

        /// <inheritdoc />
        public override bool CanSeek
        {
            get
            {
                this.VerifyNotClosed();
                return this.BaseStream.CanSeek;
            }
        }

        /// <inheritdoc />
        public override bool CanTimeout
        {
            get
            {
                this.VerifyNotClosed();
                return this.BaseStream.CanTimeout;
            }
        }

        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override long Length
        {
            get
            {
                this.VerifyNotClosed();
                return this.BaseStream.Length;
            }
        }

        /// <inheritdoc />
        public override long Position
        {
            get
            {
                this.VerifyNotClosed();
                return this.BaseStream.Position;
            }
            set
            {
                this.VerifyNotClosed();
                this.BaseStream.Position = value;
            }
        }

        /// <inheritdoc />
        public override int ReadTimeout
        {
            get
            {
                this.VerifyNotClosed();
                return this.BaseStream.ReadTimeout;
            }
            set
            {
                this.VerifyNotClosed();
                this.BaseStream.ReadTimeout = value;
            }
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ValueParameterNotUsed"),]
        public override int WriteTimeout
        {
            get
            {
                this.VerifyNotClosed();
                throw new InvalidOperationException(nameof(ReadOnlyStream) + " does not support writing.");
            }
            set
            {
                this.VerifyNotClosed();
                throw new InvalidOperationException(nameof(ReadOnlyStream) + " does not support writing.");
            }
        }

        /// <inheritdoc />
        public override IAsyncResult BeginRead (byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            this.VerifyNotClosed();
            return this.BaseStream.BeginRead(buffer, offset, count, callback, state);
        }

        /// <inheritdoc />
        public override IAsyncResult BeginWrite (byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            this.VerifyNotClosed();
            throw new NotSupportedException(nameof(ReadOnlyStream) + " does not support writing.");
        }

        /// <inheritdoc />
        public override void Close ()
        {
            this.CloseInternal();
            base.Close();
        }

        /// <inheritdoc />
        public override int EndRead (IAsyncResult asyncResult)
        {
            this.VerifyNotClosed();
            return this.BaseStream.EndRead(asyncResult);
        }

        /// <inheritdoc />
        public override void EndWrite (IAsyncResult asyncResult)
        {
            this.VerifyNotClosed();
            throw new NotSupportedException(nameof(ReadOnlyStream) + " does not support writing.");
        }

        /// <inheritdoc />
        public override void Flush ()
        {
            this.VerifyNotClosed();
            this.BaseStream.Flush();
        }

        /// <inheritdoc />
        public override int Read (byte[] buffer, int offset, int count)
        {
            this.VerifyNotClosed();
            return this.BaseStream.Read(buffer, offset, count);
        }

        /// <inheritdoc />
        public override int ReadByte ()
        {
            this.VerifyNotClosed();
            return this.BaseStream.ReadByte();
        }

        /// <inheritdoc />
        public override long Seek (long offset, SeekOrigin origin)
        {
            this.VerifyNotClosed();
            return this.BaseStream.Seek(offset, origin);
        }

        /// <inheritdoc />
        public override void SetLength (long value)
        {
            this.VerifyNotClosed();
            throw new NotSupportedException(nameof(ReadOnlyStream) + " does not support writing.");
        }

        /// <inheritdoc />
        public override void Write (byte[] buffer, int offset, int count)
        {
            this.VerifyNotClosed();
            throw new NotSupportedException(nameof(ReadOnlyStream) + " does not support writing.");
        }

        /// <inheritdoc />
        public override void WriteByte (byte value)
        {
            this.VerifyNotClosed();
            throw new NotSupportedException(nameof(ReadOnlyStream) + " does not support writing.");
        }

        /// <inheritdoc />
        protected override void Dispose (bool disposing)
        {
            this.CloseInternal();
            base.Dispose(disposing);
        }

        #endregion
    }
}
