
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
    /// <param name="newLine"> The string which is used for ending a line and going to the next line. The default value is null. If null is provided, the base writers value is used. If an empty string is provided, <see cref="Environment.NewLine"/> is used. </param>
    /// <exception cref="ArgumentException"><paramref name="newLine"/> can only be \n or \r\n. </exception>
    public IndentedTextWriter (TextWriter writer, bool keepOpen = false, bool indentEmptyLines = false, int indentLevel = 0, string identString = " ", string? newLine = null)
    {
        if(!string.IsNullOrWhiteSpace(newLine) && (newLine != "\n") && (newLine != "\r\n"))
        {
            throw new ArgumentException("Invalid line terminator.", nameof(newLine));
        }


        BaseWriter = writer;
        BaseWriter.NewLine = newLine is null ? BaseWriter.NewLine : (string.IsNullOrWhiteSpace(newLine) ? Environment.NewLine : newLine);

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

    private string CreateObjectString (object? value)
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
        // TODO Only one call to BaseWriter

        VerifyNotClosed();
        
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
        // TODO Only one call to BaseWriter
        
        VerifyNotClosed();
        
        if (_indentPending && IndentEmptyLines)
        {
            for (int i1 = 0; i1 < IndentLevel; i1++)
            {
                BaseWriter.Write(IndentString);
            }

            _indentPending = false;
        }
    }

    private async ValueTask WriteIndentAsync ()
    {
        // TODO Only one call to BaseWriter
        
        VerifyNotClosed();
        
        if (_indentPending)
        {
            for (int i1 = 0; i1 < IndentLevel; i1++)
            {
                await BaseWriter.WriteAsync(IndentString);
            }

            _indentPending = false;
        }
    }

    private async ValueTask WriteLineIndentAsync ()
    {
        // TODO Only one call to BaseWriter
        
        VerifyNotClosed();
        
        if (_indentPending && IndentEmptyLines)
        {
            for (int i1 = 0; i1 < IndentLevel; i1++)
            {
                await BaseWriter.WriteAsync(IndentString);
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
    public override void Write (bool value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write (char value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write (char[]? buffer)
    {
        WriteIndent();
        BaseWriter.Write(buffer);
    }

    /// <inheritdoc />
    public override void Write (char[] buffer, int index, int count)
    {
        WriteIndent();
        BaseWriter.Write(buffer, index, count);
    }

    /// <inheritdoc />
    public override void Write (decimal value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write (double value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write (float value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write (int value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write (long value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write (object? value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write (string format, object? arg0)
    {
        WriteIndent();
        BaseWriter.Write(format, arg0);
    }

    /// <inheritdoc />
    public override void Write (string format, object? arg0, object? arg1)
    {
        WriteIndent();
        BaseWriter.Write(format, arg0, arg1);
    }

    /// <inheritdoc />
    public override void Write (string format, object? arg0, object? arg1, object? arg2)
    {
        WriteIndent();
        BaseWriter.Write(format, arg0, arg1, arg2);
    }

    /// <inheritdoc />
    public override void Write (string format, params object?[] arg)
    {
        WriteIndent();
        BaseWriter.Write(format, arg);
    }

    /// <inheritdoc />
    public override void Write (string? value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write (uint value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write (ulong value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void Write(ReadOnlySpan<char> buffer)
    {
        WriteIndent();
        BaseWriter.Write(buffer);
    }

    /// <inheritdoc />
    public override void Write(StringBuilder? value)
    {
        WriteIndent();
        BaseWriter.Write(value);
    }

    /// <inheritdoc />
    public override void WriteLine ()
    {
        BaseWriter.WriteLine();
        _indentPending = true;
        WriteLineIndent();
    }

    /// <inheritdoc />
    public override void WriteLine (bool value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (char value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (char[]? buffer)
    {
        Write(buffer);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (char[] buffer, int index, int count)
    {
        Write(buffer, index, count);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (decimal value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (double value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (float value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (int value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (long value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (object? value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (string format, object? arg0)
    {
        Write(format, arg0);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (string format, object? arg0, object? arg1)
    {
        Write(format, arg0, arg1);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (string format, object? arg0, object? arg1, object? arg2)
    {
        Write(format, arg0, arg1, arg2);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (string format, params object?[] arg)
    {
        Write(format, arg);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (string? value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (uint value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine (ulong value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine(ReadOnlySpan<char> buffer)
    {
        Write(buffer);
        WriteLine();
    }

    /// <inheritdoc />
    public override void WriteLine(StringBuilder? value)
    {
        Write(value);
        WriteLine();
    }

    /// <inheritdoc />
    public override async Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
    {
        await WriteIndentAsync();
        await BaseWriter.WriteAsync(buffer, cancellationToken);
    }

    /// <inheritdoc />
    public override async Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default)
    {
        await WriteIndentAsync();
        await BaseWriter.WriteAsync(value, cancellationToken);
    }

    /// <inheritdoc />
    public override async Task WriteAsync(char value)
    {
        await WriteIndentAsync();
        await BaseWriter.WriteAsync(value);
    }

    /// <inheritdoc />
    public override async Task WriteAsync(char[] buffer, int index, int count)
    {
        await WriteIndentAsync();
        await BaseWriter.WriteAsync(buffer, index, count);
    }

    /// <inheritdoc />
    public override async Task WriteAsync(string? value)
    {
        await WriteIndentAsync();
        await BaseWriter.WriteAsync(value);
    }

    /// <inheritdoc />
    public override async Task WriteLineAsync()
    {
        await BaseWriter.WriteLineAsync();
        _indentPending = true;
        await WriteLineIndentAsync();
    }

    /// <inheritdoc />
    public override async Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
    {
        await WriteAsync(buffer, cancellationToken);
        await WriteLineAsync();
    }

    /// <inheritdoc />
    public override async Task WriteLineAsync(StringBuilder? value, CancellationToken cancellationToken = default)
    {
        await WriteAsync(value, cancellationToken);
        await WriteLineAsync();
    }

    /// <inheritdoc />
    public override async Task WriteLineAsync(char value)
    {
        await WriteAsync(value);
        await WriteLineAsync();
    }

    /// <inheritdoc />
    public override async Task WriteLineAsync(char[] buffer, int index, int count)
    {
        await WriteAsync(buffer, index, count);
        await WriteLineAsync();
    }

    /// <inheritdoc />
    public override async Task WriteLineAsync(string? value)
    {
        await WriteAsync(value);
        await WriteLineAsync();
    }

    #endregion
}
