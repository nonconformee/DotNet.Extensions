using System;
using System.IO;




namespace RI.Utilities.Binary
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="BinaryReader" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class BinaryReaderExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Reads the next byte from the reader without advancing the read position.
        /// </summary>
        /// <param name="reader"> The reader. </param>
        /// <returns>
        ///     The read byte or -1 if the end of the reader was reached.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        /// <exception cref="NotSupportedException"> <paramref name="reader" /> does not support seeking. </exception>
        public static int PeekByte (this BinaryReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (!reader.BaseStream.CanSeek)
            {
                throw new NotSupportedException();
            }

            long position = reader.BaseStream.Position;

            int result = reader.BaseStream.ReadByte();

            reader.BaseStream.Position = position;

            return result;
        }

        /// <summary>
        ///     Reads characters into an array.
        /// </summary>
        /// <param name="reader"> The reader. </param>
        /// <param name="data"> The array. </param>
        /// <returns>
        ///     The number of characters read into the array, beginning at index zero.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> or <paramref name="data" /> is null. </exception>
        public static int Read (this BinaryReader reader, char[] data)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return reader.Read(data, 0, data.Length);
        }

        /// <summary>
        ///     Reads bytes into an array.
        /// </summary>
        /// <param name="reader"> The reader. </param>
        /// <param name="data"> The array. </param>
        /// <returns>
        ///     The number of bytes read into the array, beginning at index zero.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> or <paramref name="data" /> is null. </exception>
        public static int Read (this BinaryReader reader, byte[] data)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return reader.Read(data, 0, data.Length);
        }

        #endregion
    }
}
