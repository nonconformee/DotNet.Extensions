using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;

using RI.Utilities.ObjectModel;




namespace RI.Utilities.Streams
{
    /// <summary>
    ///     Implements a stream where write operations are written to a queue and read operations read from that queue.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Bytes written using <see cref="Write" /> or <see cref="WriteByte" /> are written to a queue.
    ///         When bytes are read using <see cref="Read" /> or <see cref="ReadByte" />, the bytes are read from that queue.
    ///         The queue is using first-in first-out (FIFO), meaning that bytes written first are read first.
    ///     </para>
    ///     <para>
    ///         <see cref="LoopbackStream" /> is thread-safe. One thread can write to the strem while another thread can read from the stream at the same time.
    ///         However, only one read and only one write operation can be performed at the same time.
    ///         Calling a read or write operation while another is already in progress, the call blocks.
    ///     </para>
    ///     <para>
    ///         Timeouts are supported.
    ///         When a <see cref="ReadTimeout" /> greater than zero is used, read operations will wait for the specified amount of time if the queue is empty.
    ///         <see cref="WriteTimeout" /> can be used but is ignored by <see cref="LoopbackStream" />.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="true" instance="true" />
    public sealed class LoopbackStream : Stream, ISynchronizable
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="LoopbackStream" />.
        /// </summary>
        public LoopbackStream ()
        {
            this.SyncRoot = new object();
            this.ReadSyncRoot = new object();
            this.WriteSyncRoot = new object();

            this.IsDisposing = false;
            this.IsDisposed = false;

            this.Buffer = new List<byte[]>();
            this.DataWritten = new AutoResetEvent(false);

            this.ReadTimeout = 0;
            this.WriteTimeout = 0;
        }

        /// <summary>
        ///     Garbage collects this instance of <see cref="LoopbackStream" />.
        /// </summary>
        ~LoopbackStream ()
        {
            this.Close();
        }

        #endregion




        #region Instance Fields

        private bool _isDisposed;

        private bool _isDisposing;

        private int _readTimeout;

        private int _writeTimeout;

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets whether the stream is closed/disposed.
        /// </summary>
        /// <value>
        ///     true if the stream is closed/disposed, false otherwise.
        /// </value>
        public bool IsDisposed
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._isDisposed;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._isDisposed = value;
                }
            }
        }

        /// <summary>
        ///     Gets whether the stream is currently being closed/disposed.
        /// </summary>
        /// <value>
        ///     true if the stream is currently being closed/disposed or is already closed/disposed, false otherwise.
        /// </value>
        public bool IsDisposing
        {
            get
            {
                lock (this.SyncRoot)
                {
                    return this._isDisposing;
                }
            }
            private set
            {
                lock (this.SyncRoot)
                {
                    this._isDisposing = value;
                }
            }
        }

        private List<byte[]> Buffer { get; set; }

        private AutoResetEvent DataWritten { get; set; }

        private object ReadSyncRoot { get; }

        private object WriteSyncRoot { get; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Clears the queue.
        /// </summary>
        /// <exception cref="ObjectDisposedException"> This instance is closed/disposed. </exception>
        public void Clear ()
        {
            lock (this.SyncRoot)
            {
                this.VerifyNotClosed();

                this.Buffer.Clear();
            }
        }

        /// <summary>
        ///     Copies the current content of the queue to an array.
        /// </summary>
        /// <returns>
        ///     The array with the current content of the queue.
        ///     If the queue is empty, an empty array is returned.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The content of the queue is copied, the queue itself is not changed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ObjectDisposedException"> This instance is closed/disposed. </exception>
        public byte[] ToArray ()
        {
            lock (this.SyncRoot)
            {
                this.VerifyNotClosed();

                byte[] array = new byte[this.Length];

                int position = 0;

                foreach (byte[] chunk in this.Buffer)
                {
                    chunk.CopyTo(array, position);
                    position += chunk.Length;
                }

                return array;
            }
        }

        private void CloseInternal ()
        {
            this.IsDisposing = true;

            if (this.DataWritten != null)
            {
                this.DataWritten.Close();
                this.DataWritten = null;
            }

            if (this.Buffer != null)
            {
                this.Buffer.Clear();
                this.Buffer = null;
            }

            this.IsDisposed = true;
        }

        private int ReadInternal (byte[] buffer, int offset, int count, TimeSpan? timeout)
        {
            List<byte[]> read = new List<byte[]>();
            int readBytes = 0;
            bool tryTimeout = timeout.HasValue;

            while (true)
            {
                if (readBytes >= count)
                {
                    break;
                }

                int available;

                lock (this.SyncRoot)
                {
                    available = this.Buffer.Count;
                }

                if (available == 0)
                {
                    if (tryTimeout)
                    {
                        tryTimeout = false;

                        if (this.DataWritten.WaitOne(timeout.Value))
                        {
                            continue;
                        }
                    }

                    break;
                }

                lock (this.SyncRoot)
                {
                    if (this.Buffer.Count > 0)
                    {
                        byte[] nextChunk = this.Buffer[0];
                        int nextReadBytes = readBytes + nextChunk.Length;

                        if (nextReadBytes <= count)
                        {
                            read.Add(nextChunk);
                            readBytes += nextChunk.Length;

                            this.Buffer.RemoveAt(0);
                        }
                        else
                        {
                            int toRead = nextChunk.Length - (nextReadBytes - count);
                            int remaining = nextChunk.Length - toRead;

                            byte[] nextChunkPart = new byte[toRead];
                            byte[] nextChunkRemaining = new byte[remaining];

                            Array.Copy(nextChunk, 0, nextChunkPart, 0, toRead);
                            Array.Copy(nextChunk, toRead, nextChunkRemaining, 0, remaining);

                            read.Add(nextChunkPart);
                            readBytes += nextChunkPart.Length;

                            this.Buffer[0] = nextChunkRemaining;
                        }
                    }
                }
            }

            int position = 0;

            foreach (byte[] readChunk in read)
            {
                Array.Copy(readChunk, 0, buffer, offset + position, readChunk.Length);
                position += readChunk.Length;
            }

            return position;
        }

        private void VerifyNotClosed ()
        {
            if (this.IsDisposed)
            {
                throw new ObjectDisposedException(this.GetType()
                                                      .Name);
            }
        }

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override bool CanRead => true;

        /// <inheritdoc />
        public override bool CanSeek => false;

        /// <inheritdoc />
        public override bool CanTimeout => true;

        /// <inheritdoc />
        public override bool CanWrite => true;

        /// <inheritdoc />
        public override long Length
        {
            get
            {
                lock (this.SyncRoot)
                {
                    this.VerifyNotClosed();

                    long count = 0;

                    foreach (byte[] chunk in this.Buffer)
                    {
                        count += chunk.Length;
                    }

                    return count;
                }
            }
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ValueParameterNotUsed"),]
        public override long Position
        {
            get
            {
                lock (this.SyncRoot)
                {
                    this.VerifyNotClosed();
                    throw new NotSupportedException(nameof(LoopbackStream) + " does not support seeking.");
                }
            }
            set
            {
                lock (this.SyncRoot)
                {
                    this.VerifyNotClosed();
                    throw new NotSupportedException(nameof(LoopbackStream) + " does not support seeking.");
                }
            }
        }

        /// <inheritdoc />
        public override int ReadTimeout
        {
            get
            {
                lock (this.SyncRoot)
                {
                    this.VerifyNotClosed();
                    return this._readTimeout;
                }
            }
            set
            {
                lock (this.SyncRoot)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(value));
                    }

                    this.VerifyNotClosed();
                    this._readTimeout = value;
                }
            }
        }

        /// <inheritdoc />
        public override int WriteTimeout
        {
            get
            {
                lock (this.SyncRoot)
                {
                    this.VerifyNotClosed();
                    return this._writeTimeout;
                }
            }
            set
            {
                lock (this.SyncRoot)
                {
                    if (value < 0)
                    {
                        throw new ArgumentOutOfRangeException(nameof(value));
                    }

                    this.VerifyNotClosed();
                    this._writeTimeout = value;
                }
            }
        }

        /// <inheritdoc />
        public override void Close ()
        {
            lock (this.SyncRoot)
            {
                this.CloseInternal();
                base.Close();
            }
        }

        /// <inheritdoc />
        public override void Flush ()
        {
            lock (this.SyncRoot)
            {
                this.VerifyNotClosed();
            }
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse"),]
        public override int Read (byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if ((offset < 0) && (offset >= buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if ((count < 0) && ((count + offset) >= buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            lock (this.ReadSyncRoot)
            {
                int timeout;

                lock (this.SyncRoot)
                {
                    this.VerifyNotClosed();

                    if (count == 0)
                    {
                        return 0;
                    }

                    timeout = this.ReadTimeout;

                    if (timeout <= 0)
                    {
                        return this.ReadInternal(buffer, offset, count, null);
                    }
                }

                return this.ReadInternal(buffer, offset, count, TimeSpan.FromMilliseconds(timeout));
            }
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
            lock (this.SyncRoot)
            {
                this.VerifyNotClosed();
                throw new NotSupportedException(nameof(LoopbackStream) + " does not support seeking.");
            }
        }

        /// <inheritdoc />
        public override void SetLength (long value)
        {
            lock (this.SyncRoot)
            {
                this.VerifyNotClosed();
                throw new NotSupportedException(nameof(LoopbackStream) + " does not support seeking.");
            }
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse"),]
        public override void Write (byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if ((offset < 0) && (offset >= buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            if ((count < 0) && ((count + offset) >= buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            lock (this.WriteSyncRoot)
            {
                lock (this.SyncRoot)
                {
                    this.VerifyNotClosed();

                    if (count == 0)
                    {
                        return;
                    }

                    byte[] data = new byte[count];
                    Array.Copy(buffer, offset, data, 0, count);

                    this.Buffer.Add(data);

                    this.DataWritten.Set();
                }
            }
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
            lock (this.SyncRoot)
            {
                this.CloseInternal();
                base.Dispose(disposing);
            }
        }

        #endregion




        #region Interface: ISynchronizable

        /// <inheritdoc />
        public bool IsSynchronized => true;

        /// <inheritdoc />
        public object SyncRoot { get; }

        #endregion
    }
}
