using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using RI.Utilities.Text;




namespace RI.Utilities.DataFormats.Csv
{
    /// <summary>
    ///     Implements a forward-only CSV writer which iteratively writes CSV data.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         See <see cref="CsvDocument" /> for more general and detailed information about working with CSV data.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class CsvWriter : IDisposable
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="CsvWriter" />.
        /// </summary>
        /// <param name="writer"> The used <see cref="TextWriter" />. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped writer is closed if this writer is closed.
        ///     </para>
        ///     <para>
        ///         CSV writer settings with default values are used.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public CsvWriter (TextWriter writer)
            : this(writer, false, null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="CsvWriter" />.
        /// </summary>
        /// <param name="writer"> The used <see cref="TextWriter" />. </param>
        /// <param name="settings"> The used CSV writer settings or null if default values should be used. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped writer is closed if this writer is closed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public CsvWriter (TextWriter writer, CsvWriterSettings settings)
            : this(writer, false, settings) { }

        /// <summary>
        ///     Creates a new instance of <see cref="CsvWriter" />.
        /// </summary>
        /// <param name="writer"> The used <see cref="TextWriter" />. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped writer should be closed when this writer is closed (false) or kept open (true). </param>
        /// <remarks>
        ///     <para>
        ///         CSV writer settings with default values are used.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public CsvWriter (TextWriter writer, bool keepOpen)
            : this(writer, keepOpen, null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="CsvWriter" />.
        /// </summary>
        /// <param name="writer"> The used <see cref="TextWriter" />. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped writer should be closed when this writer is closed (false) or kept open (true). </param>
        /// <param name="settings"> The used CSV writer settings or null if default values should be used. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public CsvWriter (TextWriter writer, bool keepOpen, CsvWriterSettings settings)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.BaseWriter = writer;
            this.KeepOpen = keepOpen;
            this.Settings = settings ?? new CsvWriterSettings();

            this.ValueWrittenOnCurrentLine = false;
        }

        /// <summary>
        ///     Garbage collects this instance of <see cref="CsvWriter" />.
        /// </summary>
        ~CsvWriter ()
        {
            this.Dispose(false);
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the <see cref="TextWriter" /> which is used by this CSV writer to write the CSV data.
        /// </summary>
        /// <value>
        ///     The <see cref="TextWriter" /> which is used by this CSV writer to write the CSV data or null if the the CSV writer is closed/disposed.
        /// </value>
        public TextWriter BaseWriter { get; private set; }

        /// <summary>
        ///     Gets the used writer settings for this CSV writer.
        /// </summary>
        /// <value>
        ///     The used writer settings for this CSV writer.
        /// </value>
        public CsvWriterSettings Settings { get; }

        private bool KeepOpen { get; }

        private bool ValueWrittenOnCurrentLine { get; set; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Closes this CSV writer and its underlying <see cref="TextWriter" /> (<see cref="BaseWriter" />).
        /// </summary>
        public void Close ()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Flushes all written data to the underlying <see cref="TextWriter" /> (<see cref="BaseWriter" />).
        /// </summary>
        /// <exception cref="ObjectDisposedException"> The CSV writer has been closed/disposed. </exception>
        public void Flush ()
        {
            this.VerifyNotClosed();

            this.BaseWriter.Flush();
        }

        /// <summary>
        ///     Completes the current row and starts a new row.
        /// </summary>
        /// <exception cref="ObjectDisposedException"> The CSV writer has been closed/disposed. </exception>
        public void NextRow ()
        {
            this.VerifyNotClosed();

            this.BaseWriter.WriteLine();

            this.ValueWrittenOnCurrentLine = false;
        }

        /// <summary>
        ///     Writes an entire row and moves to the next row.
        /// </summary>
        /// <param name="row"> </param>
        /// <exception cref="ArgumentNullException"> <paramref name="row" /> is null. </exception>
        /// <exception cref="ObjectDisposedException"> The CSV writer has been closed/disposed. </exception>
        public void WriteRow (IEnumerable<string> row)
        {
            if (row == null)
            {
                throw new ArgumentNullException(nameof(row));
            }

            this.VerifyNotClosed();

            foreach (string column in row)
            {
                this.WriteValue(column);
            }

            this.NextRow();
        }

        /// <summary>
        ///     Writes a value on the current row.
        /// </summary>
        /// <param name="value"> The value to write. </param>
        /// <exception cref="ObjectDisposedException"> The CSV writer has been closed/disposed. </exception>
        /// <remarks>
        ///     <para>
        ///         If <paramref name="value" /> is null, no value is written.
        ///     </para>
        /// </remarks>
        public void WriteValue (string value)
        {
            this.VerifyNotClosed();

            if (value == null)
            {
                return;
            }

            if (this.ValueWrittenOnCurrentLine)
            {
                this.BaseWriter.Write(this.Settings.Separator);
            }

            this.ValueWrittenOnCurrentLine = true;

            bool quote = this.Settings.AlwaysQuoteValues || (value.IndexOfAny(new[]
                                                                {
                                                                    this.Settings.Quote,
                                                                    this.Settings.Separator,
                                                                    '\n',
                                                                }) != -1);

            if (quote)
            {
                this.BaseWriter.Write(this.Settings.Quote);
            }

            string encoded = value.DoubleOccurrence(this.Settings.Quote);
            this.BaseWriter.Write(encoded);

            if (quote)
            {
                this.BaseWriter.Write(this.Settings.Quote);
            }
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local"),]
        private void Dispose (bool disposing)
        {
            if (this.BaseWriter != null)
            {
                this.BaseWriter.Flush();

                if (!this.KeepOpen)
                {
                    this.BaseWriter.Close();
                }

                this.BaseWriter = null;
            }
        }

        private void VerifyNotClosed ()
        {
            if (this.BaseWriter == null)
            {
                throw new ObjectDisposedException(nameof(CsvWriter));
            }
        }

        #endregion




        #region Interface: IDisposable

        /// <inheritdoc />
        void IDisposable.Dispose ()
        {
            this.Close();
        }

        #endregion
    }
}
