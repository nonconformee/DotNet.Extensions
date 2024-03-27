using System;
using System.IO;

using RI.Utilities.Exceptions;




namespace RI.Utilities.Streams
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="Stream" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class StreamExtensions
    {
        #region Constants

        /// <summary>
        ///     The default buffer size used for reading and writing.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The default buffer size is 4096 bytes.
        ///     </para>
        /// </remarks>
        public const int DefaultBufferSize = 4096;

        #endregion




        #region Static Methods

        /// <summary>
        ///     Reads from one stream into another.
        /// </summary>
        /// <param name="source"> The source stream. </param>
        /// <param name="target"> The target stream. </param>
        /// <returns>
        ///     The number of bytes read.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> or <paramref name="target" /> is null. </exception>
        /// <exception cref="NotReadableStreamArgumentException"> <paramref name="source" /> is a stream which cannot be read. </exception>
        /// <exception cref="NotWriteableStreamArgumentException"> <paramref name="target" /> is a stream which cannot be written. </exception>
        public static int Read (this Stream source, Stream target)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return StreamExtensions.Copy(target, source, -1, -1);
        }

        /// <summary>
        ///     Reads from one stream into another.
        /// </summary>
        /// <param name="source"> The source stream. </param>
        /// <param name="target"> The target stream. </param>
        /// <param name="length"> The number of bytes to read or -1 to read all bytes. </param>
        /// <returns>
        ///     The number of bytes read.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> or <paramref name="target" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero. </exception>
        /// <exception cref="NotReadableStreamArgumentException"> <paramref name="source" /> is a stream which cannot be read. </exception>
        /// <exception cref="NotWriteableStreamArgumentException"> <paramref name="target" /> is a stream which cannot be written. </exception>
        public static int Read (this Stream source, Stream target, int length)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return StreamExtensions.Copy(target, source, length, -1);
        }

        /// <summary>
        ///     Reads from one stream into another.
        /// </summary>
        /// <param name="source"> The source stream. </param>
        /// <param name="target"> The target stream. </param>
        /// <param name="length"> The number of bytes to read or -1 to read all bytes. </param>
        /// <param name="bufferSize"> The buffer size used for reading or -1 to use the default buffer size. </param>
        /// <returns>
        ///     The number of bytes read.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> or <paramref name="target" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero or <paramref name="bufferSize" /> is less than or equal to zero. </exception>
        /// <exception cref="NotReadableStreamArgumentException"> <paramref name="source" /> is a stream which cannot be read. </exception>
        /// <exception cref="NotWriteableStreamArgumentException"> <paramref name="target" /> is a stream which cannot be written. </exception>
        public static int Read (this Stream source, Stream target, int length, int bufferSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return StreamExtensions.Copy(target, source, length, bufferSize);
        }

        /// <summary>
        ///     Reads a byte array from a stream.
        /// </summary>
        /// <param name="source"> The stream. </param>
        /// <param name="data"> The byte array. </param>
        /// <returns>
        ///     The number of bytes read into the byte array, starting at index zero.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> or <paramref name="data" /> is null. </exception>
        /// <exception cref="NotSupportedException"> <paramref name="source" /> does not support reading. </exception>
        public static int Read (this Stream source, byte[] data)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return source.Read(data, 0, data.Length);
        }

        /// <summary>
        ///     Reads a stream into a byte array.
        /// </summary>
        /// <param name="source"> The stream. </param>
        /// <returns>
        ///     The byte array containing all bytes of the stream.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null. </exception>
        /// <exception cref="NotReadableStreamArgumentException"> <paramref name="source" /> is a stream which cannot be read. </exception>
        public static byte[] Read (this Stream source)
        {
            return source.Read(-1, -1);
        }

        /// <summary>
        ///     Reads a stream into a byte array.
        /// </summary>
        /// <param name="source"> The stream. </param>
        /// <param name="length"> The number of bytes to read or -1 to read all bytes. </param>
        /// <returns>
        ///     The byte array containing all bytes of the stream.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null. </exception>
        /// <exception cref="NotReadableStreamArgumentException"> <paramref name="source" /> is a stream which cannot be read. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero. </exception>
        public static byte[] Read (this Stream source, int length)
        {
            return source.Read(length, -1);
        }

        /// <summary>
        ///     Reads a stream into a byte array.
        /// </summary>
        /// <param name="source"> The stream. </param>
        /// <param name="length"> The number of bytes to read or -1 to read all bytes. </param>
        /// <param name="bufferSize"> The buffer size used for reading or -1 to use the default buffer size. </param>
        /// <returns>
        ///     The byte array containing all bytes of the stream.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> is null. </exception>
        /// <exception cref="NotReadableStreamArgumentException"> <paramref name="source" /> is a stream which cannot be read. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero or <paramref name="bufferSize" /> is less than or equal to zero. </exception>
        public static byte[] Read (this Stream source, int length, int bufferSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            using (MemoryStream ms = new MemoryStream())
            {
                StreamExtensions.Copy(ms, source, length, bufferSize);

                return ms.ToArray();
            }
        }

        /// <summary>
        ///     Removes a specified amount of bytes from the end of a stream.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="length"> The number of bytes to remove. </param>
        /// <remarks>
        ///     <para>
        ///         The streams position remains unchanged, except in cases the position is within the removed range where the position will be set to the end of the stream.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero or more than the length of <paramref name="stream" />. </exception>
        /// <exception cref="NotReadableStreamArgumentException"> <paramref name="stream" /> is a stream which cannot be read. </exception>
        /// <exception cref="NotWriteableStreamArgumentException"> <paramref name="stream" /> is a stream which cannot be written. </exception>
        /// <exception cref="NotSeekableStreamArgumentException"> <paramref name="stream" /> is a stream which cannot be seeked. </exception>
        public static void TruncateAtEnd (this Stream stream, int length)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (length == 0)
            {
                return;
            }

            if (!stream.CanRead)
            {
                throw new NotReadableStreamArgumentException(nameof(stream));
            }

            if (!stream.CanWrite)
            {
                throw new NotWriteableStreamArgumentException(nameof(stream));
            }

            if (!stream.CanSeek)
            {
                throw new NotSeekableStreamArgumentException(nameof(stream));
            }

            if (length > stream.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            int startPosition = (int)stream.Position;

            stream.SetLength(stream.Length - length);

            if (startPosition >= stream.Length)
            {
                startPosition = (int)stream.Length;
            }

            stream.Position = startPosition;
        }

        /// <summary>
        ///     Removes a specified amount of bytes from the start of a stream.
        /// </summary>
        /// <param name="stream"> The stream. </param>
        /// <param name="length"> The number of bytes to remove. </param>
        /// <remarks>
        ///     <para>
        ///         The streams position remains unchanged, except in cases the position is within the removed range where the position will be set to the start of the stream.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="stream" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero or more than the length of <paramref name="stream" />. </exception>
        /// <exception cref="NotReadableStreamArgumentException"> <paramref name="stream" /> is a stream which cannot be read. </exception>
        /// <exception cref="NotWriteableStreamArgumentException"> <paramref name="stream" /> is a stream which cannot be written. </exception>
        /// <exception cref="NotSeekableStreamArgumentException"> <paramref name="stream" /> is a stream which cannot be seeked. </exception>
        public static void TruncateAtStart (this Stream stream, int length)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (length == 0)
            {
                return;
            }

            if (!stream.CanRead)
            {
                throw new NotReadableStreamArgumentException(nameof(stream));
            }

            if (!stream.CanWrite)
            {
                throw new NotWriteableStreamArgumentException(nameof(stream));
            }

            if (!stream.CanSeek)
            {
                throw new NotSeekableStreamArgumentException(nameof(stream));
            }

            if (length > stream.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            int startPosition = (int)stream.Position - length;

            if (startPosition < 0)
            {
                startPosition = 0;
            }

            using (MemoryStream ms = new MemoryStream())
            {
                stream.Position = length;

                StreamExtensions.Copy(ms, stream, (int)stream.Length - length, -1);

                stream.SetLength(stream.Length - length);

                stream.Position = 0;
                ms.Position = 0;

                StreamExtensions.Copy(stream, ms, -1, -1);
            }

            if (startPosition >= stream.Length)
            {
                startPosition = (int)stream.Length;
            }

            stream.Position = startPosition;
        }

        /// <summary>
        ///     Writes a byte array to a stream.
        /// </summary>
        /// <param name="target"> The stream. </param>
        /// <param name="data"> The byte array. </param>
        /// <returns>
        ///     The number of bytes written.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="target" /> or <paramref name="data" /> is null. </exception>
        /// <exception cref="NotSupportedException"> <paramref name="target" /> does not support writing. </exception>
        public static int Write (this Stream target, byte[] data)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            target.Write(data, 0, data.Length);

            return data.Length;
        }

        /// <summary>
        ///     Writes from one stream into another.
        /// </summary>
        /// <param name="source"> The source stream. </param>
        /// <param name="target"> The target stream. </param>
        /// <returns>
        ///     The number of bytes written.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> or <paramref name="target" /> is null. </exception>
        /// <exception cref="NotReadableStreamArgumentException"> <paramref name="source" /> is a stream which cannot be read. </exception>
        /// <exception cref="NotWriteableStreamArgumentException"> <paramref name="target" /> is a stream which cannot be written. </exception>
        public static int Write (this Stream target, Stream source)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return StreamExtensions.Copy(target, source, -1, -1);
        }

        /// <summary>
        ///     Writes from one stream into another.
        /// </summary>
        /// <param name="source"> The source stream. </param>
        /// <param name="target"> The target stream. </param>
        /// <param name="length"> The number of bytes to read or -1 to read all bytes. </param>
        /// <returns>
        ///     The number of bytes written.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> or <paramref name="target" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero. </exception>
        /// <exception cref="NotReadableStreamArgumentException"> <paramref name="source" /> is a stream which cannot be read. </exception>
        /// <exception cref="NotWriteableStreamArgumentException"> <paramref name="target" /> is a stream which cannot be written. </exception>
        public static int Write (this Stream target, Stream source, int length)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return StreamExtensions.Copy(target, source, length, -1);
        }

        /// <summary>
        ///     Writes from one stream into another.
        /// </summary>
        /// <param name="source"> The source stream. </param>
        /// <param name="target"> The target stream. </param>
        /// <param name="length"> The number of bytes to read or -1 to read all bytes. </param>
        /// <param name="bufferSize"> The buffer size used for reading or -1 to use the default buffer size. </param>
        /// <returns>
        ///     The number of bytes written.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="source" /> or <paramref name="target" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero or <paramref name="bufferSize" /> is less than or equal to zero. </exception>
        /// <exception cref="NotReadableStreamArgumentException"> <paramref name="source" /> is a stream which cannot be read. </exception>
        /// <exception cref="NotWriteableStreamArgumentException"> <paramref name="target" /> is a stream which cannot be written. </exception>
        public static int Write (this Stream target, Stream source, int length, int bufferSize)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return StreamExtensions.Copy(target, source, length, bufferSize);
        }

        private static int Copy (Stream target, Stream source, int length, int bufferSize)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (!target.CanWrite)
            {
                throw new NotWriteableStreamArgumentException(nameof(target));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (!source.CanRead)
            {
                throw new NotReadableStreamArgumentException(nameof(source));
            }

            if ((length < 0) && (length != -1))
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if ((bufferSize <= 0) && (bufferSize != -1))
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            if (length == 0)
            {
                return 0;
            }

            if (bufferSize == -1)
            {
                bufferSize = StreamExtensions.DefaultBufferSize;
            }

            byte[] buffer = new byte[bufferSize];

            int total = 0;

            if (length == -1)
            {
                if (source.CanSeek)
                {
                    length = (int)(source.Length - source.Position);
                }
                else
                {
                    while (true)
                    {
                        int read = source.Read(buffer, 0, bufferSize);

                        total += read;

                        if (read == 0)
                        {
                            return total;
                        }

                        target.Write(buffer, 0, read);
                    }
                }
            }

            if (length <= 0)
            {
                return 0;
            }

            while (true)
            {
                int remaining = length - total;

                int read = source.Read(buffer, 0, remaining >= bufferSize ? bufferSize : remaining);

                total += read;

                target.Write(buffer, 0, read);

                if ((read == 0) || (total >= length))
                {
                    return total;
                }
            }
        }

        #endregion
    }
}
