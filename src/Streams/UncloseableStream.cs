using System;
using System.IO;




namespace RI.Utilities.Streams
{
    /// <summary>
    ///     Implements a stream which wraps another stream and prevents the wrapped stream from being closed.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="UncloseableStream" /> can be helpful in situations where you pass a stream to an object or method which, after it has done its job, closes the stream and you need the stream still open afterwards.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class UncloseableStream : Stream
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="UncloseableStream" />.
        /// </summary>
        /// <param name="stream"> The stream to wrap. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is null. </exception>
        public UncloseableStream (Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            this.BaseStream = stream;
            this.CloseIntercepted = false;
        }

        /// <summary>
        ///     Garbage collects this instance of <see cref="UncloseableStream" />.
        /// </summary>
        ~UncloseableStream ()
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
        public Stream BaseStream { get; }

        /// <summary>
        ///     Gets whether a close or dispose attempt has been intercepted by this <see cref="UncloseableStream" /> instance.
        /// </summary>
        /// <value>
        ///     true if a close or dispose attempt has been intercepted by this <see cref="UncloseableStream" /> instance, false otherwise.
        /// </value>
        public bool CloseIntercepted { get; private set; }

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override bool CanRead => this.BaseStream.CanRead;

        /// <inheritdoc />
        public override bool CanSeek => this.BaseStream.CanSeek;

        /// <inheritdoc />
        public override bool CanTimeout => this.BaseStream.CanTimeout;

        /// <inheritdoc />
        public override bool CanWrite => this.BaseStream.CanWrite;

        /// <inheritdoc />
        public override long Length => this.BaseStream.Length;

        /// <inheritdoc />
        public override long Position
        {
            get => this.BaseStream.Position;
            set => this.BaseStream.Position = value;
        }

        /// <inheritdoc />
        public override int ReadTimeout
        {
            get => this.BaseStream.ReadTimeout;
            set => this.BaseStream.ReadTimeout = value;
        }

        /// <inheritdoc />
        public override int WriteTimeout
        {
            get => this.BaseStream.WriteTimeout;
            set => this.BaseStream.WriteTimeout = value;
        }

        /// <inheritdoc />
        public override IAsyncResult BeginRead (byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this.BaseStream.BeginRead(buffer, offset, count, callback, state);
        }

        /// <inheritdoc />
        public override IAsyncResult BeginWrite (byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return this.BaseStream.BeginWrite(buffer, offset, count, callback, state);
        }

        /// <summary>
        ///     Intercepts any closing attempts to the wrapped stream.
        /// </summary>
        public override void Close ()
        {
            this.CloseIntercepted = true;
            base.Close();
        }

        /// <inheritdoc />
        public override int EndRead (IAsyncResult asyncResult)
        {
            return this.BaseStream.EndRead(asyncResult);
        }

        /// <inheritdoc />
        public override void EndWrite (IAsyncResult asyncResult)
        {
            this.BaseStream.EndWrite(asyncResult);
        }

        /// <inheritdoc />
        public override void Flush ()
        {
            this.BaseStream.Flush();
        }

        /// <inheritdoc />
        public override int Read (byte[] buffer, int offset, int count)
        {
            return this.BaseStream.Read(buffer, offset, count);
        }

        /// <inheritdoc />
        public override int ReadByte ()
        {
            return this.BaseStream.ReadByte();
        }

        /// <inheritdoc />
        public override long Seek (long offset, SeekOrigin origin)
        {
            return this.BaseStream.Seek(offset, origin);
        }

        /// <inheritdoc />
        public override void SetLength (long value)
        {
            this.BaseStream.SetLength(value);
        }

        /// <inheritdoc />
        public override void Write (byte[] buffer, int offset, int count)
        {
            this.BaseStream.Write(buffer, offset, count);
        }

        /// <inheritdoc />
        public override void WriteByte (byte value)
        {
            this.BaseStream.WriteByte(value);
        }

        /// <summary>
        ///     Intercepts any disposing attempts to the wrapped stream.
        /// </summary>
        /// <param name="disposing"> true to release both managed and unmanaged resources, false to release only unmanaged resources. </param>
        protected override void Dispose (bool disposing)
        {
            this.CloseIntercepted = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}
