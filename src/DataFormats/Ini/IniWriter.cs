using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using RI.Utilities.DataFormats.Ini.Elements;
using RI.Utilities.Exceptions;
using RI.Utilities.Text;




namespace RI.Utilities.DataFormats.Ini
{
    /// <summary>
    ///     Implements a forward-only INI writer which iteratively writes INI data.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         See <see cref="IniDocument" /> for more general and detailed information about working with INI data.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class IniWriter : IDisposable
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="IniWriter" />.
        /// </summary>
        /// <param name="writer"> The used <see cref="TextWriter" />. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped writer is closed if this writer is closed.
        ///     </para>
        ///     <para>
        ///         INI writer settings with default values are used.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public IniWriter (TextWriter writer)
            : this(writer, false, null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="IniWriter" />.
        /// </summary>
        /// <param name="writer"> The used <see cref="TextWriter" />. </param>
        /// <param name="settings"> The used INI writer settings or null if default values should be used. </param>
        /// <remarks>
        ///     <para>
        ///         The wrapped writer is closed if this writer is closed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public IniWriter (TextWriter writer, IniWriterSettings settings)
            : this(writer, false, settings) { }

        /// <summary>
        ///     Creates a new instance of <see cref="IniWriter" />.
        /// </summary>
        /// <param name="writer"> The used <see cref="TextWriter" />. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped writer should be closed when this writer is closed (false) or kept open (true). </param>
        /// <remarks>
        ///     <para>
        ///         INI writer settings with default values are used.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public IniWriter (TextWriter writer, bool keepOpen)
            : this(writer, keepOpen, null) { }

        /// <summary>
        ///     Creates a new instance of <see cref="IniWriter" />.
        /// </summary>
        /// <param name="writer"> The used <see cref="TextWriter" />. </param>
        /// <param name="keepOpen"> Specifies whether the wrapped writer should be closed when this writer is closed (false) or kept open (true). </param>
        /// <param name="settings"> The used INI writer settings or null if default values should be used. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public IniWriter (TextWriter writer, bool keepOpen, IniWriterSettings settings)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.BaseWriter = writer;
            this.KeepOpen = keepOpen;
            this.Settings = settings ?? new IniWriterSettings();

            this.Written = false;
        }

        /// <summary>
        ///     Garbage collects this instance of <see cref="IniWriter" />.
        /// </summary>
        ~IniWriter ()
        {
            this.Dispose(false);
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the <see cref="TextWriter" /> which is used by this INI writer to write the INI data.
        /// </summary>
        /// <value>
        ///     The <see cref="TextWriter" /> which is used by this INI writer to write the INI data or null if the the INI writer is closed/disposed.
        /// </value>
        public TextWriter BaseWriter { get; private set; }

        /// <summary>
        ///     Gets the used writer settings for this INI writer.
        /// </summary>
        /// <value>
        ///     The used writer settings for this INI writer.
        /// </value>
        public IniWriterSettings Settings { get; }

        private bool KeepOpen { get; }

        private bool Written { get; set; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Closes this INI writer and its underlying <see cref="TextWriter" /> (<see cref="BaseWriter" />).
        /// </summary>
        public void Close ()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Flushes all written data to the underlying <see cref="TextWriter" /> (<see cref="BaseWriter" />).
        /// </summary>
        /// <exception cref="ObjectDisposedException"> The INI writer has been closed/disposed. </exception>
        public void Flush ()
        {
            this.VerifyNotClosed();

            this.BaseWriter.Flush();
        }

        /// <summary>
        ///     Writes a comment.
        /// </summary>
        /// <param name="comment"> The comment. </param>
        /// <remarks>
        ///     <para>
        ///         The comment will not be encoded but if it contains multiple lines, multiple actual comments will be written (one per line).
        ///     </para>
        ///     <para>
        ///         If <paramref name="comment" /> is null or an empty string, an empty string is written as comment.
        ///     </para>
        /// </remarks>
        /// <exception cref="ObjectDisposedException"> The INI writer has been closed/disposed. </exception>
        public void WriteComment (string comment)
        {
            this.VerifyNotClosed();

            string[] lines = (comment ?? string.Empty).SplitLines();

            foreach (string line in lines)
            {
                this.WriteNewLineIfNecessary();
                this.WriteInternal(this.Settings.CommentStart);
                this.WriteInternal(line);
            }
        }

        /// <summary>
        ///     Writes an INI element.
        /// </summary>
        /// <param name="element"> The element to write. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="element" /> is null. </exception>
        /// <exception cref="ObjectDisposedException"> The INI writer has been closed/disposed. </exception>
        public void WriteElement (IniElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            this.VerifyNotClosed();

            if (element is SectionIniElement)
            {
                SectionIniElement sectionElement = (SectionIniElement)element;
                this.WriteSection(sectionElement.SectionName);
            }
            else if (element is ValueIniElement)
            {
                ValueIniElement valueElement = (ValueIniElement)element;
                this.WriteValue(valueElement.Name, valueElement.Value);
            }
            else if (element is TextIniElement)
            {
                TextIniElement textElement = (TextIniElement)element;
                this.WriteText(textElement.Text);
            }
            else if (element is CommentIniElement)
            {
                CommentIniElement commentElement = (CommentIniElement)element;
                this.WriteComment(commentElement.Comment);
            }
        }

        /// <summary>
        ///     Writes a section header.
        /// </summary>
        /// <param name="sectionName"> The name of the section. </param>
        /// <remarks>
        ///     <para>
        ///         The written section name will be encoded which means that certain special characters used in INI files will be replaced with escape sequences.
        ///         See <see cref="IniDocument" /> for more details.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="sectionName" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="sectionName" /> is an empty string. </exception>
        /// <exception cref="ObjectDisposedException"> The INI writer has been closed/disposed. </exception>
        public void WriteSection (string sectionName)
        {
            if (sectionName == null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (sectionName.IsEmptyOrWhitespace())
            {
                throw new EmptyStringArgumentException(nameof(sectionName));
            }

            this.VerifyNotClosed();

            sectionName = this.EncodeSectionName(sectionName);

            if (this.Settings.EmptyLineBeforeSectionHeader)
            {
                this.WriteNewLineIfNecessary();
            }

            this.WriteNewLineIfNecessary();
            this.WriteInternal(this.Settings.SectionStart);
            this.WriteInternal(sectionName);
            this.WriteInternal(this.Settings.SectionEnd);
        }

        /// <summary>
        ///     Writes arbitrary text.
        /// </summary>
        /// <param name="text"> The text. </param>
        /// <remarks>
        ///     <para>
        ///         The text will not be encoded but if it contains multiple lines, multiple actual text lines will be written.
        ///     </para>
        ///     <para>
        ///         If <paramref name="text" /> is null or an empty string, an empty string is written as text.
        ///     </para>
        /// </remarks>
        /// <exception cref="ObjectDisposedException"> The INI writer has been closed/disposed. </exception>
        public void WriteText (string text)
        {
            this.VerifyNotClosed();

            string[] lines = (text ?? string.Empty).SplitLines();

            foreach (string line in lines)
            {
                this.WriteNewLineIfNecessary();
                this.WriteInternal(line);
            }
        }

        /// <summary>
        ///     Writes a name-value-pair.
        /// </summary>
        /// <param name="name"> The name. </param>
        /// <param name="value"> The value. </param>
        /// <remarks>
        ///     <para>
        ///         The written name and value will be encoded which means that certain special characters used in INI files will be replaced with escape sequences.
        ///         See <see cref="IniDocument" /> for more details.
        ///     </para>
        ///     <para>
        ///         If <paramref name="value" /> is null or an empty string, an empty string is written as value.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="name" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="name" /> is an empty string. </exception>
        /// <exception cref="ObjectDisposedException"> The INI writer has been closed/disposed. </exception>
        public void WriteValue (string name, string value)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name.IsEmptyOrWhitespace())
            {
                throw new EmptyStringArgumentException(nameof(name));
            }

            this.VerifyNotClosed();

            name = this.EncodeName(name);
            value = this.EncodeValue(value ?? string.Empty);

            this.WriteNewLineIfNecessary();
            this.WriteInternal(name);
            this.WriteInternal(this.Settings.NameValueSeparator);
            this.WriteInternal(value);
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

        private string EncodeGeneral (string value)
        {
            value = value.DoubleOccurrence(this.Settings.EscapeCharacter);
            value = value.Replace("\r", this.Settings.EscapeCharacter + "r");
            value = value.Replace("\n", this.Settings.EscapeCharacter + "n");
            return value;
        }

        private string EncodeName (string name)
        {
            name = this.EncodeGeneral(name);
            name = name.Replace(this.Settings.SectionStart.ToString(), this.Settings.EscapeCharacter + this.Settings.SectionStart.ToString());
            name = name.Replace(this.Settings.SectionEnd.ToString(), this.Settings.EscapeCharacter + this.Settings.SectionEnd.ToString());
            name = name.Replace(this.Settings.CommentStart.ToString(), this.Settings.EscapeCharacter + this.Settings.CommentStart.ToString());
            name = name.Replace(this.Settings.NameValueSeparator.ToString(), this.Settings.EscapeCharacter + this.Settings.NameValueSeparator.ToString());
            return name;
        }

        private string EncodeSectionName (string sectionName)
        {
            sectionName = this.EncodeGeneral(sectionName);
            sectionName = sectionName.Replace(this.Settings.SectionStart.ToString(), this.Settings.EscapeCharacter + this.Settings.SectionStart.ToString());
            sectionName = sectionName.Replace(this.Settings.SectionEnd.ToString(), this.Settings.EscapeCharacter + this.Settings.SectionEnd.ToString());
            return sectionName;
        }

        private string EncodeValue (string value)
        {
            value = this.EncodeGeneral(value);
            return value;
        }

        private void VerifyNotClosed ()
        {
            if (this.BaseWriter == null)
            {
                throw new ObjectDisposedException(nameof(IniWriter));
            }
        }

        private void WriteInternal (char value)
        {
            this.BaseWriter.Write(value);
            this.Written = true;
        }

        private void WriteInternal (string value)
        {
            this.BaseWriter.Write(value);
            this.Written = true;
        }

        private void WriteNewLineIfNecessary ()
        {
            if (this.Written)
            {
                this.BaseWriter.WriteLine();
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
