using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using RI.Utilities.DataFormats.Ini.Elements;
using RI.Utilities.Text;




namespace RI.Utilities.DataFormats.Ini
{
    /// <summary>
    ///     Implements a forward-only INI reader which iteratively reads INI data.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         See <see cref="IniDocument" /> for more general and detailed information about working with INI data.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class IniReader : IDisposable
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="IniReader" />.
        /// </summary>
        /// <param name="reader"> The used <see cref="TextReader" />. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped reader is closed if this reader is closed.
        ///     </para>
        ///     <para>
        ///         INI reader settings with default values are used.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public IniReader (TextReader reader)
            : this(reader, false, null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="IniReader" />.
        /// </summary>
        /// <param name="reader"> The used <see cref="TextReader" />. </param>
        /// <param name="settings"> The used INI reader settings or null if default values should be used. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped reader is closed if this reader is closed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public IniReader (TextReader reader, IniReaderSettings settings)
            : this(reader, false, settings) { }

        /// <summary>
        ///     Creates a new instance of <see cref="IniReader" />.
        /// </summary>
        /// <param name="reader"> The used <see cref="TextReader" />. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped reader should be closed when this reader is closed (false) or kept open (true). </param>
        /// <remarks>
        ///     <para>
        ///         INI reader settings with default values are used.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public IniReader (TextReader reader, bool keepOpen)
            : this(reader, keepOpen, null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="IniReader" />.
        /// </summary>
        /// <param name="reader"> The used <see cref="TextReader" />. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped reader should be closed when this reader is closed (false) or kept open (true). </param>
        /// <param name="settings"> The used INI reader settings or null if default values should be used. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        public IniReader (TextReader reader, bool keepOpen, IniReaderSettings settings)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            this.BaseReader = reader;
            this.KeepOpen = keepOpen;
            this.Settings = settings ?? new IniReaderSettings();

            this.CurrentLineNumber = 0;
            this.CurrentElement = null;
            this.CurrentError = IniReaderError.None;
            this.Buffer = null;
        }

        /// <summary>
        ///     Garbage collects this instance of <see cref="IniReader" />.
        /// </summary>
        ~IniReader ()
        {
            this.Dispose(false);
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the <see cref="TextReader" /> which is used by this INI reader to read the INI data.
        /// </summary>
        /// <value>
        ///     The <see cref="TextReader" /> which is used by this INI reader to read the INI data or null if the the INI reader is closed/disposed.
        /// </value>
        public TextReader BaseReader { get; private set; }

        /// <summary>
        ///     Gets the current INI element which was read during the last call to <see cref="ReadNext" />.
        /// </summary>
        /// <value>
        ///     The current INI element or null if last call to <see cref="ReadNext" /> created an error (<see cref="CurrentError" />).
        /// </value>
        /// <remarks>
        ///     <para>
        ///         Before the first call to <see cref="ReadNext" />, this property is null.
        ///     </para>
        ///     <para>
        ///         This property keeps its last value even if <see cref="ReadNext" /> returns false.
        ///     </para>
        /// </remarks>
        public IniElement CurrentElement { get; private set; }

        /// <summary>
        ///     Gets the current error which ocurred during the last call to <see cref="ReadNext" />.
        /// </summary>
        /// <value>
        ///     The current error.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         Before the first call to <see cref="ReadNext" />, this property is <see cref="IniReaderError.None" />.
        ///     </para>
        ///     <para>
        ///         This property keeps its last value even if <see cref="ReadNext" /> returns false.
        ///     </para>
        /// </remarks>
        public IniReaderError CurrentError { get; private set; }

        /// <summary>
        ///     Gets the current line number in the INI data to which <see cref="CurrentElement" /> or <see cref="CurrentError" /> corresponds to.
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
        /// </remarks>
        public int CurrentLineNumber { get; private set; }

        /// <summary>
        ///     Gets the used reader settings for this INI reader.
        /// </summary>
        /// <value>
        ///     The used reader settings for this INI reader.
        /// </value>
        public IniReaderSettings Settings { get; }

        private string Buffer { get; set; }

        private bool KeepOpen { get; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Closes this INI reader and its underlying <see cref="TextReader" /> (<see cref="BaseReader" />).
        /// </summary>
        public void Close ()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Reads the next INI element from the INI data.
        /// </summary>
        /// <returns>
        ///     true if an element was read and <see cref="CurrentElement" /> was updated, false if there are no more INI elements (<see cref="CurrentElement" /> keeps its last value).
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         The INI data is read line-by-line.
        ///         Multiple consecutive comment or text lines are combined into a single comment or text line.
        ///     </note>
        /// </remarks>
        /// <exception cref="ObjectDisposedException"> The INI reader has been closed/disposed. </exception>
        public bool ReadNext ()
        {
            this.VerifyNotClosed();

            string line = this.ReadLine();
            IniElement element = this.ProcessLine(line);

            if (element == null)
            {
                return false;
            }

            if (element is ErrorElement)
            {
                this.CurrentElement = null;
                this.CurrentError = ((ErrorElement)element).Error;
            }
            else
            {
                this.CurrentElement = element;
                this.CurrentError = IniReaderError.None;
            }

            if (element is TextIniElement)
            {
                TextIniElement textElement = (TextIniElement)element;

                while (true)
                {
                    string nextLine = this.PeekLine();
                    IniElement nextElement = this.ProcessLine(nextLine);

                    if (!(nextElement is TextIniElement))
                    {
                        break;
                    }

                    this.ReadLine();
                    TextIniElement nextTextElement = (TextIniElement)nextElement;
                    textElement.Text += Environment.NewLine + nextTextElement.Text;
                }
            }
            else if (element is CommentIniElement)
            {
                CommentIniElement commentElement = (CommentIniElement)element;

                while (true)
                {
                    string nextLine = this.PeekLine();
                    IniElement nextElement = this.ProcessLine(nextLine);

                    if (!(nextElement is CommentIniElement))
                    {
                        break;
                    }

                    this.ReadLine();
                    CommentIniElement nextCommentElement = (CommentIniElement)nextElement;
                    commentElement.Comment += Environment.NewLine + nextCommentElement.Comment;
                }
            }

            return true;
        }

        private string Decode (string value)
        {
            value = value.ReplaceSingleStart(this.Settings.EscapeCharacter + this.Settings.NameValueSeparator.ToString(), this.Settings.NameValueSeparator.ToString(), StringComparison.Ordinal);
            value = value.ReplaceSingleStart(this.Settings.EscapeCharacter + this.Settings.CommentStart.ToString(), this.Settings.CommentStart.ToString(), StringComparison.Ordinal);
            value = value.ReplaceSingleStart(this.Settings.EscapeCharacter + this.Settings.SectionEnd.ToString(), this.Settings.SectionEnd.ToString(), StringComparison.Ordinal);
            value = value.ReplaceSingleStart(this.Settings.EscapeCharacter + this.Settings.SectionStart.ToString(), this.Settings.SectionStart.ToString(), StringComparison.Ordinal);
            value = value.ReplaceSingleStart(this.Settings.EscapeCharacter + "n", "\n", StringComparison.Ordinal);
            value = value.ReplaceSingleStart(this.Settings.EscapeCharacter + "r", "\r", StringComparison.Ordinal);
            value = value.HalveOccurrence(this.Settings.EscapeCharacter);
            return value;
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

        private string PeekLine ()
        {
            if (this.Buffer != null)
            {
                return this.Buffer;
            }

            string line = this.BaseReader.ReadLine();
            this.Buffer = line;
            return line;
        }

        private IniElement ProcessLine (string line)
        {
            if (line == null)
            {
                return null;
            }

            string trimmedStart = line.TrimStart();

            if (trimmedStart.StartsWith(this.Settings.SectionStart.ToString(), StringComparison.Ordinal) && line.TrimEnd()
                                                                                                                .EndsWith(this.Settings.SectionEnd.ToString(), StringComparison.Ordinal))
            {
                string sectionName = line.Trim()
                                         .TrimStart(this.Settings.SectionStart)
                                         .TrimEnd(this.Settings.SectionEnd);

                sectionName = this.Decode(sectionName);

                try
                {
                    return new SectionIniElement(sectionName);
                }
                catch
                {
                    return new ErrorElement(IniReaderError.InvalidSectionName);
                }
            }

            if (trimmedStart.StartsWith(this.Settings.CommentStart.ToString(), StringComparison.Ordinal))
            {
                string comment = trimmedStart.Substring(1);
                return new CommentIniElement(comment);
            }

            int separatorIndex = line.IndexOf(this.Settings.NameValueSeparator);

            if (separatorIndex == -1)
            {
                return new TextIniElement(line);
            }

            string name = line.Substring(0, separatorIndex);
            name = this.Decode(name);
            string value = line.Substring(separatorIndex + 1);
            value = this.Decode(value);

            try
            {
                return new ValueIniElement(name, value);
            }
            catch
            {
                return new ErrorElement(IniReaderError.InvalidValueName);
            }
        }

        private string ReadLine ()
        {
            this.CurrentLineNumber++;

            if (this.Buffer != null)
            {
                string line = this.Buffer;
                this.Buffer = null;
                return line;
            }

            string read = this.BaseReader.ReadLine();
            return read;
        }

        private void VerifyNotClosed ()
        {
            if (this.BaseReader == null)
            {
                throw new ObjectDisposedException(nameof(IniReader));
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




        #region Type: ErrorElement

        private sealed class ErrorElement : IniElement
        {
            #region Instance Constructor/Destructor

            public ErrorElement (IniReaderError error)
            {
                this.Error = error;
            }

            #endregion




            #region Instance Properties/Indexer

            public IniReaderError Error { get; }

            #endregion
        }

        #endregion
    }
}
