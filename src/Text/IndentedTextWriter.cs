
using System.Text;

namespace nonconformee.DotNet.Extensions.Text;




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
/// <threadsafety static="true" instance="false" />
public sealed class IndentedTextWriter : TextWriter
{
    #region Instance Constructor/Finalizer

    /// <summary>
    ///     Creates a new instance of <see cref="IndentedTextWriter" />.
    /// </summary>
    /// <param name="writer"> The <see cref="TextWriter" /> to encapsulate. </param>
    /// <param name="keepOpen"> Specifies whether the encapsulated <see cref="TextWriter" /> is closed when this <see cref="IndentedTextWriter" /> is closed (false) or not (true). The default value is false. </param>
    /// <param name="indentEmptyLines"> Specifies whether empty lines are indented or not. The default value is false. </param>
    /// <param name="indentLevel"> Specifies the indentation level to start with The default value is 0. </param>
    /// <param name="identString"> The string which is used for indenting one level. The default value is a single space. If an empty string is provided, a single space will be used.</param>
    /// <param name="newLine"> The string which is used for ending a line and going to the next line. The default value is null. If null or an empty string is provided, the base writers value is used is used. </param>
    public IndentedTextWriter (TextWriter writer, bool keepOpen = false, bool indentEmptyLines = false, int indentLevel = 0, string identString = " ", string? newLine = null)
    {
        BaseWriter = writer;
        writer.NewLine = string.IsNullOrWhiteSpace(newLine) ? writer.NewLine : newLine;

        KeepOpen = keepOpen;
        IndentEmptyLines = indentEmptyLines;
        IndentLevel = indentLevel;      
        IndentString = string.IsNullOrEmpty(identString) ? " " : identString;

        _indentPending = true;
    }

    /// <summary>
    ///     Finalizes this instance of <see cref="IndentedTextWriter" />.
    /// </summary>
    ~IndentedTextWriter ()
    {
        Dispose();
    }

    #endregion




    #region Instance Fields/Properties/Indexer

    private int _indentLevel;
    private bool _indentPending;

    /// <summary>
    ///     Gets the <see cref="TextWriter" /> which is encapsulated by this <see cref="IndentedTextWriter" />.
    /// </summary>
    /// <value>
    ///     The <see cref="TextWriter" /> which is encapsulated by this <see cref="IndentedTextWriter" />.
    /// </value>
    public TextWriter BaseWriter { get; }

    /// <summary>
    ///  Gets whether the encapsulated <see cref="TextWriter" /> is closed when this <see cref="IndentedTextWriter" /> is closed.
    /// </summary>
    /// <value>false if the encapsulated text writer is disposed on close, true if the encapsulated text writer is kept open after this instance has beend closed/disposed.</value>
    public bool KeepOpen { get; }

    /// <summary>
    ///     Gets whether empty lines are indented or not.
    /// </summary>
    /// <value>
    ///     true if empty lines are to be indented, false if not.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         Empty lines are either strings of zero length or which only contain whitespaces.
    ///     </para>
    /// </remarks>
    public bool IndentEmptyLines { get; }

    /// <summary>
    ///     Gets the indentation string.
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
    /// </remarks>
    public string IndentString { get; }

    /// <summary>
    ///     Gets the current indentation level.
    /// </summary>
    /// <value>
    ///     The current indentation level.
    /// </value>
    /// <remarks>
    ///     <para>
    ///         If a value less than zero is set as indentation level, zero is used instead.
    ///     </para>
    /// </remarks>
    public int IndentLevel
    {
        get => this._indentLevel;
        private set
        {
            if (value < 0)
            {
                value = 0;
            }

            this._indentLevel = value;
        }
    }

    /// <summary>
    /// Indicates whether this instance has been closed/disposed.
    /// </summary>
    /// <value>true if this instance has been disposed, false otherwise.</value>
    public bool IsClosed { get; private set;}
    #endregion




    #region Instance Methods

    private void VerifyNotClosed ()
    {
        if(IsClosed)
        {
            throw new ObjectDisposedException(nameof(IndentedTextWriter));
        }
    }

    private string CreateObjectString (object value)
    {
        if (value is null)
        {
            return string.Empty;
        }

        if (value is IFormattable)
        {
            return ((IFormattable)value).ToString(null, FormatProvider);
        }

        return value.ToString()!;
    }

    private void WriteIndent ()
    {
        if (_indentPending)
        {
            for (int i1 = 0; i1 < IndentLevel; i1++)
            {
                BaseWriter.Write(IndentString);
            }

            _indentPending = false;
        }
    }

    private void WriteLineIndent ()
    {
        if (_indentPending && IndentEmptyLines)
        {
            for (int i1 = 0; i1 < IndentLevel; i1++)
            {
                BaseWriter.Write(IndentString);
            }

            _indentPending = false;
        }
    }

    #endregion




    #region Overrides

    /// <inheritdoc />
    public override Encoding Encoding => BaseWriter.Encoding;

    /// <inheritdoc />
    public override IFormatProvider FormatProvider => BaseWriter.FormatProvider;

    /// <inheritdoc />
    public override string NewLine
    {
        get => BaseWriter.NewLine;
        set => BaseWriter.NewLine = value;
    }

    /// <inheritdoc />
    protected override void Dispose (bool disposing)
    {
        if(!KeepOpen)
        {
            BaseWriter.Dispose();
        }

        base.Dispose(disposing);
    }

    /// <inheritdoc />
    public override async ValueTask DisposeAsync()
    {
        if(!KeepOpen)
        {
            await BaseWriter.DisposeAsync();
        }

        await base.DisposeAsync();
    }

    /// <inheritdoc />
    public override void Close() => Dispose();

    /// <inheritdoc />
    public override void Flush() => BaseWriter.Flush();

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

        this._indentPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine (bool value)
    {
        this.VerifyNotClosed();
        this.WriteIndent();
        this.BaseWriter.WriteLine(value);
        this._indentPending = true;
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
        this._indentPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine (double value)
    {
        this.VerifyNotClosed();
        this.WriteIndent();
        this.BaseWriter.WriteLine(value);
        this._indentPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine (float value)
    {
        this.VerifyNotClosed();
        this.WriteIndent();
        this.BaseWriter.WriteLine(value);
        this._indentPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine (int value)
    {
        this.VerifyNotClosed();
        this.WriteIndent();
        this.BaseWriter.WriteLine(value);
        this._indentPending = true;
    }

    /// <inheritdoc />
    public override void WriteLine (long value)
    {
        this.VerifyNotClosed();
        this.WriteIndent();
        this.BaseWriter.WriteLine(value);
        this._indentPending = true;
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
        this._indentPending = true;
    }

    /// <inheritdoc />
    [CLSCompliant(false),]
    public override void WriteLine (ulong value)
    {
        this.VerifyNotClosed();
        this.WriteIndent();
        this.BaseWriter.WriteLine(value);
        this._indentPending = true;
    }

    #endregion
}
