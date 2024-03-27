using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;




namespace RI.Utilities.DataFormats.Csv
{
    /// <summary>
    ///     Implements a forward-only CSV reader which iteratively reads CSV data.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         See <see cref="CsvDocument" /> for more general and detailed information about working with CSV data.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class CsvReader : IDisposable
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="CsvReader" />.
        /// </summary>
        /// <param name="reader"> The used <see cref="TextReader" />. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped reader is closed if this reader is closed.
        ///     </para>
        ///     <para>
        ///         CSV reader settings with default values are used.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public CsvReader (TextReader reader)
            : this(reader, false, null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="CsvReader" />.
        /// </summary>
        /// <param name="reader"> The used <see cref="TextReader" />. </param>
        /// <param name="settings"> The used CSV reader settings or null if default values should be used. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped reader is closed if this reader is closed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public CsvReader (TextReader reader, CsvReaderSettings settings)
            : this(reader, false, settings) { }

        /// <summary>
        ///     Creates a new instance of <see cref="CsvReader" />.
        /// </summary>
        /// <param name="reader"> The used <see cref="TextReader" />. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped reader should be closed when this reader is closed (false) or kept open (true). </param>
        /// <remarks>
        ///     <para>
        ///         CSV reader settings with default values are used.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public CsvReader (TextReader reader, bool keepOpen)
            : this(reader, keepOpen, null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="CsvReader" />.
        /// </summary>
        /// <param name="reader"> The used <see cref="TextReader" />. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped reader should be closed when this reader is closed (false) or kept open (true). </param>
        /// <param name="settings"> The used CSV reader settings or null if default values should be used. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public CsvReader (TextReader reader, bool keepOpen, CsvReaderSettings settings)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            this.BaseReader = reader;
            this.KeepOpen = keepOpen;
            this.Settings = settings ?? new CsvReaderSettings();

            this.CurrentLineNumberInternal = 0;

            this.CurrentLineNumber = 0;
            this.CurrentRow = null;
            this.CurrentError = CsvReaderError.None;
        }

        /// <summary>
        ///     Garbage collects this instance of <see cref="CsvReader" />.
        /// </summary>
        ~CsvReader ()
        {
            this.Dispose(false);
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the <see cref="TextReader" /> which is used by this CSV reader to read the CSV data.
        /// </summary>
        /// <value>
        ///     The <see cref="TextReader" /> which is used by this CSV reader to read the CSV data or null if the the CSV reader is closed/disposed.
        /// </value>
        public TextReader BaseReader { get; private set; }

        /// <summary>
        ///     Gets the current error which ocurred during the last call to <see cref="ReadNext" />.
        /// </summary>
        /// <value>
        ///     The current error.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         Before the first call to <see cref="ReadNext" />, this property is <see cref="CsvReaderError.None" />.
        ///     </para>
        ///     <para>
        ///         This property keeps its last value even if <see cref="ReadNext" /> returns false.
        ///     </para>
        /// </remarks>
        public CsvReaderError CurrentError { get; private set; }

        /// <summary>
        ///     Gets the current line number in the CSV data to which <see cref="CurrentRow" /> or <see cref="CurrentError" /> corresponds to.
        /// </summary>
        /// <value>
        ///     The current line number.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         Before the first call to <see cref="ReadNext" />, this property is zero.
        ///     </para>
        ///     <para>
        ///         This property keeps its last value even if <see cref="ReadNext" /> returns false.
        ///     </para>
        ///     <note type="note">
        ///         This value always corresponds to the last line which belongs to the current row (relevant for multiline-values).
        ///     </note>
        /// </remarks>
        public int CurrentLineNumber { get; private set; }

        /// <summary>
        ///     Gets the current CSV row values which was read during the last call to <see cref="ReadNext" />.
        /// </summary>
        /// <value>
        ///     The current CSV row values or null if last call to <see cref="ReadNext" /> created an error (<see cref="CurrentError" />).
        /// </value>
        /// <remarks>
        ///     <para>
        ///         Before the first call to <see cref="ReadNext" />, this property is null.
        ///     </para>
        ///     <para>
        ///         This property keeps its last value even if <see cref="ReadNext" /> returns false.
        ///     </para>
        ///     <para>
        ///         A new list instance is created for each row.
        ///     </para>
        /// </remarks>
        public List<string> CurrentRow { get; private set; }

        /// <summary>
        ///     Gets the used reader settings for this CSV reader.
        /// </summary>
        /// <value>
        ///     The used reader settings for this CSV reader.
        /// </value>
        public CsvReaderSettings Settings { get; }

        private int CurrentLineNumberInternal { get; set; }

        private bool KeepOpen { get; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Closes this CSV reader and its underlying <see cref="TextReader" /> (<see cref="BaseReader" />).
        /// </summary>
        public void Close ()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///     Reads the next CSV row from the CSV data.
        /// </summary>
        /// <returns>
        ///     true if a row was read and <see cref="CurrentRow" /> was updated, false if there are no more CSV rows (<see cref="CurrentRow" /> keeps its last value).
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         The CSV data is read line-by-line.
        ///         <see cref="ReadNext" /> reads logical CSV rows, which can result in reading multiple lines from the underlying <see cref="BaseReader" /> in case of multiline values.
        ///     </note>
        /// </remarks>
        /// <exception cref="ObjectDisposedException"> The INI reader has been closed/disposed. </exception>
        public bool ReadNext ()
        {
            this.VerifyNotClosed();

            if (this.CurrentError != CsvReaderError.None)
            {
                return false;
            }

            if (this.CurrentLineNumberInternal <= 0)
            {
                this.CurrentLineNumberInternal = 1;
            }

            ParseState state = ParseState.ExpectStart;
            StringBuilder value = null;

            CsvReaderError error = CsvReaderError.None;
            List<string> row = new List<string>();

            int readCharacters = 0;

            bool lastWasNewLine = false;


            while (true)
            {
                int current = this.BaseReader.Read();
                int next = this.BaseReader.Peek();

                if (current == -1)
                {
                    if (value != null)
                    {
                        row.Add(value.ToString());
                    }

                    break;
                }

                readCharacters++;

                //TODO: #29: Fix line number detection

                char currentChar = (char)current;
                char? nextChar = next == -1 ? (char?)null : (char)next;

                if (currentChar == '\r')
                {
                    continue;
                }

                bool isQuote = currentChar == this.Settings.Quote;
                bool isSeparator = currentChar == this.Settings.Separator;
                bool isNewLine = currentChar == '\n';
                bool isDoubleQuote = isQuote && (nextChar.HasValue ? nextChar.Value == this.Settings.Quote : false);

                lastWasNewLine = isNewLine;

                bool skipNext = false;
                bool finishValue = false;
                bool finishRow = false;
                string append = null;

                switch (state)
                {
                    case ParseState.ExpectStart:
                        if (isQuote)
                        {
                            value = new StringBuilder();
                            state = ParseState.ExpectContentQuoted;
                        }
                        else if (isSeparator)
                        {
                            row.Add(string.Empty);

                            if (!nextChar.HasValue)
                            {
                                row.Add(string.Empty);
                            }
                        }
                        else if (isNewLine)
                        {
                            finishRow = true;

                            if (row.Count > 0)
                            {
                                row.Add(string.Empty);
                            }
                        }
                        else
                        {
                            value = new StringBuilder();
                            state = ParseState.ExpectContentUnquoted;
                            append = currentChar.ToString();
                        }

                        break;

                    case ParseState.ExpectContentQuoted:
                        if (isDoubleQuote)
                        {
                            append = this.Settings.Quote.ToString();
                            skipNext = true;
                        }
                        else if (isQuote)
                        {
                            state = ParseState.ExpectSeparator;
                            finishValue = true;
                        }
                        else if (isNewLine)
                        {
                            if (this.Settings.AllowMultilineValues)
                            {
                                append = Environment.NewLine;
                            }
                            else
                            {
                                error = CsvReaderError.MultilineValueNotAllowed;
                            }
                        }
                        else
                        {
                            append = currentChar.ToString();
                        }

                        break;

                    case ParseState.ExpectContentUnquoted:
                        if (isQuote)
                        {
                            error = CsvReaderError.UnexpectedQuote;
                        }
                        else if (isSeparator)
                        {
                            state = ParseState.ExpectStart;
                            finishValue = true;
                        }
                        else if (isNewLine)
                        {
                            finishValue = true;
                            finishRow = true;
                        }
                        else
                        {
                            append = currentChar.ToString();
                        }

                        break;

                    case ParseState.ExpectSeparator:
                        if (isSeparator)
                        {
                            state = ParseState.ExpectStart;
                        }
                        else if (isNewLine)
                        {
                            finishRow = true;
                        }
                        else if (this.Settings.WhitespaceTolerant && char.IsWhiteSpace(currentChar)) { }
                        else
                        {
                            error = CsvReaderError.SeparatorExpected;
                        }

                        break;
                }

                if (isNewLine)
                {
                    this.CurrentLineNumberInternal++;
                }

                if (skipNext)
                {
                    this.BaseReader.Read();
                }

                if (append != null)
                {
                    value = value ?? new StringBuilder();
                    value.Append(append);
                }

                if ((finishRow || finishValue) && (value != null))
                {
                    row.Add(value.ToString());
                    value = null;
                }

                if (finishRow || (error != CsvReaderError.None))
                {
                    break;
                }
            }

            this.CurrentLineNumber = lastWasNewLine ? this.CurrentLineNumberInternal - 1 : this.CurrentLineNumberInternal;
            this.CurrentError = error;
            this.CurrentRow = error == CsvReaderError.None ? row : null;

            return error == CsvReaderError.None ? readCharacters > 0 : false;
        }

        [SuppressMessage("ReSharper", "UnusedParameter.Local"),]
        private void Dispose (bool disposing)
        {
            if (this.BaseReader != null)
            {
                if (!this.KeepOpen)
                {
                    this.BaseReader.Close();
                }

                this.BaseReader = null;
            }
        }

        private void VerifyNotClosed ()
        {
            if (this.BaseReader == null)
            {
                throw new ObjectDisposedException(nameof(CsvReader));
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




        #region Type: ParseState

        private enum ParseState
        {
            ExpectStart = 0,

            ExpectContentQuoted = 1,

            ExpectContentUnquoted = 2,

            ExpectSeparator = 3,
        }

        #endregion
    }
}
