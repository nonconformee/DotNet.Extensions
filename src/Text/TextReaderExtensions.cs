using System;
using System.Collections.Generic;
using System.IO;




namespace RI.Utilities.Text
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="TextReader" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class TextReaderExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Reads characters into an array.
        /// </summary>
        /// <param name="reader"> The reader. </param>
        /// <param name="data"> The array. </param>
        /// <returns>
        ///     The number of characters read.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> or <paramref name="data" /> is null. </exception>
        public static int Read (this TextReader reader, char[] data)
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
        ///     Reads characters into an array.
        /// </summary>
        /// <param name="reader"> The reader. </param>
        /// <param name="data"> The array. </param>
        /// <returns>
        ///     The number of characters read.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> or <paramref name="data" /> is null. </exception>
        public static int ReadBlock (this TextReader reader, char[] data)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            return reader.ReadBlock(data, 0, data.Length);
        }

        /// <summary>
        ///     Reads all available lines into an array.
        /// </summary>
        /// <param name="reader"> The reader. </param>
        /// <returns>
        ///     The array of lines read to the end of the reader or null if the end of the reader was reached without reading any line.
        /// </returns>
        public static string[] ReadLines (this TextReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            List<string> lines = new List<string>();

            while (true)
            {
                string line = reader.ReadLine();

                if (line == null)
                {
                    break;
                }

                lines.Add(line);
            }

            return lines.Count == 0 ? null : lines.ToArray();
        }

        #endregion
    }
}
