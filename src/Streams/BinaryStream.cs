using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;




namespace RI.Utilities.Streams
{
    /// <summary>
    ///     Implements a stream which wraps either a <see cref="BinaryReader" /> or a <see cref="BinaryWriter" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="BinaryStream" /> is used when a <see cref="BinaryReader" /> or <see cref="BinaryWriter" /> needs to be used as a stream.
    ///     </para>
    ///     <para>
    ///         A <see cref="BinaryStream" /> can either support reading (using a <see cref="BinaryReader" />) or writing (using a <see cref="BinaryWriter" />) but not both at the same time.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class BinaryStream : Stream
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="BinaryStream" />.
        /// </summary>
        /// <param name="reader"> The <see cref="BinaryReader" /> to use. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped reader should be closed when this stream is closed (false) or kept open (true). </param>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public BinaryStream (BinaryReader reader, bool keepOpen)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            this.Reader = reader;
            this.Writer = null;
            this.KeepOpen = keepOpen;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="BinaryStream" />.
        /// </summary>
        /// <param name="reader"> The <see cref="BinaryReader" /> to use. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped reader is closed if this stream is closed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public BinaryStream (BinaryReader reader)
            : this(reader, false) { }

        /// <summary>
        ///     Creates a new instance of <see cref="BinaryStream" />.
        /// </summary>
        /// <param name="writer"> The <see cref="BinaryWriter" /> to use. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped writer should be closed when this stream is closed (false) or kept open (true). </param>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public BinaryStream (BinaryWriter writer, bool keepOpen)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.Reader = null;
            this.Writer = writer;
            this.KeepOpen = keepOpen;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="BinaryStream" />.
        /// </summary>
        /// <param name="writer"> The <see cref="BinaryWriter" /> to use. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped writer is closed if this stream is closed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public BinaryStream (BinaryWriter writer)
            : this(writer, false) { }

        /// <summary>
        ///     Creates a new instance of <see cref="BinaryStream" />.
        /// </summary>
        /// <param name="reader"> The <see cref="BinaryReader" /> to use. </param>
        /// <param name="writer"> The <see cref="BinaryWriter" /> to use. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped reader and writer should be closed when this stream is closed (false) or kept open (true). </param>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public BinaryStream (BinaryReader reader, BinaryWriter writer, bool keepOpen)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.Reader = reader;
            this.Writer = writer;
            this.KeepOpen = keepOpen;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="BinaryStream" />.
        /// </summary>
        /// <param name="reader"> The <see cref="BinaryReader" /> to use. </param>
        /// <param name="writer"> The <see cref="BinaryWriter" /> to use. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped reader and writer is closed if this stream is closed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public BinaryStream (BinaryReader reader, BinaryWriter writer)
            : this(reader, writer, false) { }

        /// <summary>
        ///     Garbage collects this instance of <see cref="BinaryStream" />.
        /// </summary>
        ~BinaryStream ()
        {
            this.Close();
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the used <see cref="BinaryReader" />, if any.
        /// </summary>
        /// <value>
        ///     The used <see cref="BinaryReader" /> or null if no <see cref="BinaryReader" /> is used.
        /// </value>
        public BinaryReader Reader { get; private set; }

        /// <summary>
        ///     Gets the used <see cref="BinaryWriter" />, if any.
        /// </summary>
        /// <value>
        ///     The used <see cref="BinaryWriter" /> or null if no <see cref="BinaryWriter" /> is used.
        /// </value>
        public BinaryWriter Writer { get; private set; }

        private bool KeepOpen { get; }

        #endregion




        #region Instance Methods

        private bool CheckNotClosed ()
        {
            return (this.Writer != null) || (this.Reader != null);
        }

        private void CloseInternal ()
        {
            if (this.Reader != null)
            {
                if (!this.KeepOpen)
                {
                    this.Reader.Close();
                }

                this.Reader = null;
            }

            if (this.Writer != null)
            {
                if (!this.KeepOpen)
                {
                    this.Writer.Close();
                }

                this.Writer = null;
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
                return this.Reader != null;
            }
        }

        /// <inheritdoc />
        public override bool CanSeek => false;

        /// <inheritdoc />
        public override bool CanTimeout => false;

        /// <inheritdoc />
        public override bool CanWrite
        {
            get
            {
                this.VerifyNotClosed();
                return this.Writer != null;
            }
        }

        /// <inheritdoc />
        public override long Length
        {
            get
            {
                this.VerifyNotClosed();
                throw new NotSupportedException(nameof(BinaryStream) + " does not support seeking.");
            }
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ValueParameterNotUsed"),]
        public override long Position
        {
            get
            {
                this.VerifyNotClosed();
                throw new NotSupportedException(nameof(BinaryStream) + " does not support seeking.");
            }
            set
            {
                this.VerifyNotClosed();
                throw new NotSupportedException(nameof(BinaryStream) + " does not support seeking.");
            }
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ValueParameterNotUsed"),]
        public override int ReadTimeout
        {
            get
            {
                this.VerifyNotClosed();
                throw new InvalidOperationException(nameof(BinaryStream) + " does not support timeouts.");
            }
            set
            {
                this.VerifyNotClosed();
                throw new InvalidOperationException(nameof(BinaryStream) + " does not support timeouts.");
            }
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ValueParameterNotUsed"),]
        public override int WriteTimeout
        {
            get
            {
                this.VerifyNotClosed();
                throw new InvalidOperationException(nameof(BinaryStream) + " does not support timeouts.");
            }
            set
            {
                this.VerifyNotClosed();
                throw new InvalidOperationException(nameof(BinaryStream) + " does not support timeouts.");
            }
        }

        /// <inheritdoc />
        public override void Close ()
        {
            this.CloseInternal();
            base.Close();
        }

        /// <inheritdoc />
        public override void Flush ()
        {
            this.VerifyNotClosed();
            this.Writer?.Flush();
        }

        /// <inheritdoc />
        public override int Read (byte[] buffer, int offset, int count)
        {
            this.VerifyNotClosed();

            if (this.Reader == null)
            {
                throw new NotSupportedException(nameof(BinaryStream) + " does not support reading.");
            }

            return this.Reader.Read(buffer, offset, count);
        }

        /// <inheritdoc />
        public override int ReadByte ()
        {
            byte[] buffer = new byte[1];

            int read = this.Read(buffer, 0, 1);

            if (read != 1)
            {
                return -1;
            }

            return buffer[0];
        }

        /// <inheritdoc />
        public override long Seek (long offset, SeekOrigin origin)
        {
            this.VerifyNotClosed();
            throw new NotSupportedException(nameof(BinaryStream) + " does not support seeking.");
        }

        /// <inheritdoc />
        public override void SetLength (long value)
        {
            this.VerifyNotClosed();
            throw new NotSupportedException(nameof(BinaryStream) + " does not support seeking.");
        }

        /// <inheritdoc />
        public override void Write (byte[] buffer, int offset, int count)
        {
            this.VerifyNotClosed();

            if (this.Writer == null)
            {
                throw new NotSupportedException(nameof(BinaryStream) + " does not support writing.");
            }

            this.Writer.Write(buffer, offset, count);
        }

        /// <inheritdoc />
        public override void WriteByte (byte value)
        {
            this.Write(new[]
            {
                value,
            });
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
