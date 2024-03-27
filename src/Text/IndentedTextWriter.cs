using System;
using System.IO;
using System.Text;




namespace RI.Utilities.Text
{
    /// <summary>
    ///     A <see cref="TextWriter" /> which encapsulates another <see cref="TextWriter" /> so that it can be used with auto-indentation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Auto-indentation means that each line written to the encapsulated <see cref="TextWriter" /> starts with a defined indentation.
    ///     </para>
    ///     <note type="important">
    ///         <see cref="IndentedTextWriter" /> only works reliable with new line strings of LF or CRLF (see <see cref="TextWriter.NewLine" />).
    ///     </note>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    /// <example>
    ///     <code language="cs">
    ///  <![CDATA[
    ///  // create a string builder to write into
    ///  var sb = new StringBuilder();
    ///  using(var sw = new StringWriter(sb))
    ///  {
    /// 		// create the indented text writer
    /// 		using(var itw = new IndentedTextWriter(sb))
    /// 		{
    /// 			// lets use tabs for indentation
    /// 			itw.IndentString = "\t";
    ///  
    /// 			// write some lines
    /// 			itw.WriteLine("Line 1");
    /// 			itw.IndentLevel++;
    /// 			itw.WriteLine("Line 2");
    /// 			itw.IndentLevel++;
    /// 			itw.WriteLine("Line 3");
    /// 			itw.IndentLevel--;
    /// 			itw.WriteLine("Line 4");
    /// 			itw.IndentLevel = 0;
    /// 			itw.WriteLine("Line 5");
    /// 		}
    ///  }
    ///  
    ///  // get final string
    ///  var text = sb.ToString();
    ///  
    ///  // result:
    ///  // Line 1
    ///  //     Line 2
    ///  //         Line 3
    ///  //     Line4
    ///  // Line 5
    ///  ]]>
    ///  </code>
    /// </example>
    public sealed class IndentedTextWriter : TextWriter
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="IndentedTextWriter" />.
        /// </summary>
        /// <param name="writer"> The <see cref="TextWriter" /> to encapsulate. </param>
        /// <param name="keepOpen"> Specifies whether the encapsulated <see cref="TextWriter" /> is closed when this <see cref="IndentedTextWriter" /> is closed (false) or not (true). </param>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public IndentedTextWriter (TextWriter writer, bool keepOpen)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.BaseWriter = writer;

            this.IndentEmptyLines = false;
            this.IndentLevel = 0;
            this.IndentString = " ";

            this.IndentPending = true;

            this.KeepOpen = keepOpen;
        }

        /// <summary>
        ///     Creates a new instance of <see cref="IndentedTextWriter" />.
        /// </summary>
        /// <param name="writer"> The <see cref="TextWriter" /> to encapsulate. </param>
        /// <remarks>
        ///     <para>
        ///         The encapsulated <see cref="TextWriter" /> is closed when this <see cref="IndentedTextWriter" /> is closed.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public IndentedTextWriter (TextWriter writer)
            : this(writer, false) { }

        /// <summary>
        ///     Garbage collects this instance of <see cref="IndentedTextWriter" />.
        /// </summary>
        ~IndentedTextWriter ()
        {
            this.Close();
        }

        #endregion




        #region Instance Fields

        private int _indentLevel;

        private string _indentString;

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the <see cref="TextWriter" /> which is encapsulated by this <see cref="IndentedTextWriter" />.
        /// </summary>
        /// <value>
        ///     The <see cref="TextWriter" /> which is encapsulated by this <see cref="IndentedTextWriter" />.
        /// </value>
        public TextWriter BaseWriter { get; private set; }

        /// <summary>
        ///     Gets or sets whether empty lines are indented or not.
        /// </summary>
        /// <value>
        ///     true if empty lines are to be indented, false if not.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         Empty lines are either strings of zero length or which only contain whitespaces.
        ///     </para>
        ///     <para>
        ///         The default value is false.
        ///     </para>
        /// </remarks>
        public bool IndentEmptyLines { get; set; }

        /// <summary>
        ///     Gets or sets the current indentation level.
        /// </summary>
        /// <value>
        ///     The current indentation level.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         If a value less than zero is set as indentation level, zero is used instead.
        ///     </para>
        ///     <para>
        ///         The default value is zero.
        ///     </para>
        /// </remarks>
        public int IndentLevel
        {
            get => this._indentLevel;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                this._indentLevel = value;
            }
        }

        /// <summary>
        ///     Gets or sets the indentation string.
        /// </summary>
        /// <value>
        ///     The indentation string.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The indentation string is added to the start of each line x times, where x is <see cref="IndentLevel" />.
        ///     </para>
        ///     <para>
        ///         If null is set as indentation string, <see cref="string.Empty" /> is used instead.
        ///     </para>
        ///     <para>
        ///         The default value is a single space character.
        ///     </para>
        /// </remarks>
        public string IndentString
        {
            get => this._indentString;
            set
            {
                if (value == null)
                {
                    value = string.Empty;
                }

                this._indentString = value;
            }
        }

        private bool KeepOpen { get; }

        private bool IndentPending { get; set; }

        #endregion




        #region Instance Methods

        private bool CheckNotClosed ()
        {
            return this.BaseWriter != null;
        }

        private void CloseInternal ()
        {
            if (this.BaseWriter != null)
            {
                if (!this.KeepOpen)
                {
                    this.BaseWriter.Close();
                    this.BaseWriter.Dispose();
                }

                this.BaseWriter = null;
            }
        }

        private string CreateObjectString (object value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is IFormattable)
            {
                return ((IFormattable)value).ToString(null, this.FormatProvider);
            }

            return value.ToString();
        }

        private void VerifyNotClosed ()
        {
            if (!this.CheckNotClosed())
            {
                throw new ObjectDisposedException(nameof(IndentedTextWriter));
            }
        }

        private void WriteIndent ()
        {
            if (this.IndentPending)
            {
                for (int i1 = 0; i1 < this.IndentLevel; i1++)
                {
                    this.BaseWriter.Write(this.IndentString);
                }
            }

            this.IndentPending = false;
        }

        private void WriteLineIndent ()
        {
            if (this.IndentPending && this.IndentEmptyLines)
            {
                for (int i1 = 0; i1 < this.IndentLevel; i1++)
                {
                    this.BaseWriter.Write(this.IndentString);
                }
            }

            this.IndentPending = false;
        }

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override Encoding Encoding
        {
            get
            {
                this.VerifyNotClosed();

                return this.BaseWriter.Encoding;
            }
        }

        /// <inheritdoc />
        public override IFormatProvider FormatProvider
        {
            get
            {
                this.VerifyNotClosed();

                return this.BaseWriter.FormatProvider;
            }
        }

        /// <inheritdoc />
        public override string NewLine
        {
            get
            {
                this.VerifyNotClosed();

                return this.BaseWriter.NewLine;
            }
            set
            {
                this.VerifyNotClosed();

                this.BaseWriter.NewLine = value;
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

            this.BaseWriter.Flush();
        }

        /// <inheritdoc />
        public override void Write (bool value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.Write(value);
        }

        /// <inheritdoc />
        public override void Write (char value)
        {
            this.VerifyNotClosed();

            if (value == '\r')
            {
                return;
            }

            if ((value == '\n') || (value == this.NewLine[0]))
            {
                this.WriteLine();
            }
            else
            {
                this.WriteIndent();

                this.BaseWriter.Write(value);
            }
        }

        /// <inheritdoc />
        public override void Write (char[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            this.VerifyNotClosed();

            foreach (char chr in buffer)
            {
                this.Write(chr);
            }
        }

        /// <inheritdoc />
        public override void Write (char[] buffer, int index, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if ((index < 0) || ((index >= buffer.Length) && (count > 0)))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if ((count < 0) || ((index + count) >= buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            this.VerifyNotClosed();

            for (int i1 = 0; i1 < count; i1++)
            {
                this.Write(buffer[i1 + index]);
            }
        }

        /// <inheritdoc />
        public override void Write (decimal value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.Write(value);
        }

        /// <inheritdoc />
        public override void Write (double value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.Write(value);
        }

        /// <inheritdoc />
        public override void Write (float value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.Write(value);
        }

        /// <inheritdoc />
        public override void Write (int value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.Write(value);
        }

        /// <inheritdoc />
        public override void Write (long value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.Write(value);
        }

        /// <inheritdoc />
        public override void Write (object value)
        {
            this.VerifyNotClosed();

            string str = this.CreateObjectString(value);

            this.Write(str);
        }

        /// <inheritdoc />
        public override void Write (string format, object arg0)
        {
            this.VerifyNotClosed();

            string str = string.Format(this.FormatProvider, format, arg0);

            this.Write(str);
        }

        /// <inheritdoc />
        public override void Write (string format, object arg0, object arg1)
        {
            this.VerifyNotClosed();

            string str = string.Format(this.FormatProvider, format, arg0, arg1);

            this.Write(str);
        }

        /// <inheritdoc />
        public override void Write (string format, object arg0, object arg1, object arg2)
        {
            this.VerifyNotClosed();

            string str = string.Format(this.FormatProvider, format, arg0, arg1, arg2);

            this.Write(str);
        }

        /// <inheritdoc />
        public override void Write (string format, params object[] arg)
        {
            this.VerifyNotClosed();

            string str = string.Format(this.FormatProvider, format, arg);

            this.Write(str);
        }

        /// <inheritdoc />
        public override void Write (string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.VerifyNotClosed();

            for (int i1 = 0; i1 < value.Length; i1++)
            {
                this.Write(value[i1]);
            }
        }

        /// <inheritdoc />
        [CLSCompliant(false),]
        public override void Write (uint value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.Write(value);
        }

        /// <inheritdoc />
        [CLSCompliant(false),]
        public override void Write (ulong value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.Write(value);
        }

        /// <inheritdoc />
        public override void WriteLine ()
        {
            this.VerifyNotClosed();

            this.WriteLineIndent();

            this.BaseWriter.WriteLine();

            this.IndentPending = true;
        }

        /// <inheritdoc />
        public override void WriteLine (bool value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.WriteLine(value);
            this.IndentPending = true;
        }

        /// <inheritdoc />
        public override void WriteLine (char value)
        {
            this.VerifyNotClosed();

            this.Write(value);

            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine (char[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            this.VerifyNotClosed();

            for (int i1 = 0; i1 < buffer.Length; i1++)
            {
                this.Write(buffer[i1]);
            }

            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine (char[] buffer, int index, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if ((index < 0) || ((index >= buffer.Length) && (count > 0)))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if ((count < 0) || ((index + count) >= buffer.Length))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            this.VerifyNotClosed();

            for (int i1 = 0; i1 < count; i1++)
            {
                this.Write(buffer[i1 + index]);
            }

            this.WriteLine();
        }

        /// <inheritdoc />
        public override void WriteLine (decimal value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.WriteLine(value);
            this.IndentPending = true;
        }

        /// <inheritdoc />
        public override void WriteLine (double value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.WriteLine(value);
            this.IndentPending = true;
        }

        /// <inheritdoc />
        public override void WriteLine (float value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.WriteLine(value);
            this.IndentPending = true;
        }

        /// <inheritdoc />
        public override void WriteLine (int value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.WriteLine(value);
            this.IndentPending = true;
        }

        /// <inheritdoc />
        public override void WriteLine (long value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.WriteLine(value);
            this.IndentPending = true;
        }

        /// <inheritdoc />
        public override void WriteLine (object value)
        {
            this.VerifyNotClosed();

            string str = this.CreateObjectString(value);

            this.WriteLine(str);
        }

        /// <inheritdoc />
        public override void WriteLine (string format, object arg0)
        {
            this.VerifyNotClosed();

            string str = string.Format(this.FormatProvider, format, arg0);

            this.WriteLine(str);
        }

        /// <inheritdoc />
        public override void WriteLine (string format, object arg0, object arg1)
        {
            this.VerifyNotClosed();

            string str = string.Format(this.FormatProvider, format, arg0, arg1);

            this.WriteLine(str);
        }

        /// <inheritdoc />
        public override void WriteLine (string format, object arg0, object arg1, object arg2)
        {
            this.VerifyNotClosed();

            string str = string.Format(this.FormatProvider, format, arg0, arg1, arg2);

            this.WriteLine(str);
        }

        /// <inheritdoc />
        public override void WriteLine (string format, params object[] arg)
        {
            this.VerifyNotClosed();

            string str = string.Format(this.FormatProvider, format, arg);

            this.WriteLine(str);
        }

        /// <inheritdoc />
        public override void WriteLine (string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.VerifyNotClosed();

            for (int i1 = 0; i1 < value.Length; i1++)
            {
                this.Write(value[i1]);
            }

            this.WriteLine();
        }

        /// <inheritdoc />
        [CLSCompliant(false),]
        public override void WriteLine (uint value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.WriteLine(value);
            this.IndentPending = true;
        }

        /// <inheritdoc />
        [CLSCompliant(false),]
        public override void WriteLine (ulong value)
        {
            this.VerifyNotClosed();
            this.WriteIndent();
            this.BaseWriter.WriteLine(value);
            this.IndentPending = true;
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
