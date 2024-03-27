using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;




namespace RI.Utilities.Randomizing
{
    /// <summary>
    ///     Implements a stream which reads random bytes.
    /// </summary>
    /// <value>
    ///     <see cref="RandomStream" /> is only readable and cannot be written or seeked.
    /// </value>
    /// <threadsafety static="false" instance="false" />
    public sealed class RandomStream : Stream
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="RandomStream" />.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         A new default randomizer is used.
        ///     </para>
        /// </remarks>
        public RandomStream ()
            : this(null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="RandomStream" />.
        /// </summary>
        /// <param name="randomizer"> The used randomizer. Can be null to use a new default randomizer. </param>
        public RandomStream (Random randomizer)
        {
            this.Randomizer = randomizer ?? new Random();
        }

        /// <summary>
        ///     Garbage collects this instance of <see cref="RandomStream" />.
        /// </summary>
        ~RandomStream ()
        {
            this.Close();
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the used randomizer.
        /// </summary>
        /// <value>
        ///     The used randomizer.
        /// </value>
        public Random Randomizer { get; }

        #endregion




        #region Instance Methods

        /// <inheritdoc />
        private void ThrowNotSeekable ()
        {
            throw new NotSupportedException(this.GetType()
                                                .Name + " does not support seeking.");
        }

        /// <inheritdoc />
        private void ThrowNotTimeoutable ()
        {
            throw new InvalidOperationException(this.GetType()
                                                    .Name + " does not support timing out.");
        }

        /// <inheritdoc />
        private void ThrowNotWriteable ()
        {
            throw new NotSupportedException(this.GetType()
                                                .Name + " does not support writing.");
        }

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override bool CanRead => true;

        /// <inheritdoc />
        public override bool CanSeek => false;

        /// <inheritdoc />
        public override bool CanTimeout => false;

        /// <inheritdoc />
        public override bool CanWrite => false;

        /// <inheritdoc />
        public override long Length
        {
            get
            {
                this.ThrowNotSeekable();
                return 0;
            }
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ValueParameterNotUsed"),]
        public override long Position
        {
            get
            {
                this.ThrowNotSeekable();
                return 0;
            }
            set => this.ThrowNotSeekable();
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ValueParameterNotUsed"),]
        public override int ReadTimeout
        {
            get
            {
                this.ThrowNotTimeoutable();
                return 0;
            }
            set => this.ThrowNotTimeoutable();
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "ValueParameterNotUsed"),]
        public override int WriteTimeout
        {
            get
            {
                this.ThrowNotTimeoutable();
                return 0;
            }
            set => this.ThrowNotTimeoutable();
        }

        /// <inheritdoc />
        public override void Flush () { }

        /// <inheritdoc />
        public override int Read (byte[] buffer, int offset, int count)
        {
            this.Randomizer.NextBytes(buffer, offset, count);
            return count;
        }

        /// <inheritdoc />
        public override long Seek (long offset, SeekOrigin origin)
        {
            this.ThrowNotSeekable();
            return 0;
        }

        /// <inheritdoc />
        public override void SetLength (long value)
        {
            this.ThrowNotSeekable();
        }

        /// <inheritdoc />
        public override void Write (byte[] buffer, int offset, int count)
        {
            this.ThrowNotWriteable();
        }

        #endregion
    }
}
