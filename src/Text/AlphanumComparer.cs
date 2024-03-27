using System.Collections;
using System.Globalization;
using System.Text;

namespace nonconformee.DotNet.Extensions.Text;




/// <summary>
///     Implements a comparer which uses natural alphanumeric string sorting.
/// </summary>
/// <remarks>
///     <para>
///         Natural alphanumeric sorting is a more sophisticated string sorting algorithm which respects the natural way humans look at strings, especially if they contain numbers.
///     </para>
///     <para>
///         The following list of strings is used as an example: Test100xyz, Test200abc, Test99abc.
///         Traditional sorting would sort this list as shown, putting Test99abc at the end (because the first four characters are the same and then 9 is greater than 2).
///         With natural alphanumeric sorting, the list is sorted as: Test99abc, Test100xyz, Test200abc (because the algorithm builds chunks and compares those instead of character-by-character).
///     </para>
///     <para>
///         Because this kind of sorting depends on the used culture, the used culture must be specified.
///     </para>
/// </remarks>
/// <threadsafety static="true" instance="true" />
public sealed class AlphanumComparer : IComparer<string>, IComparer, IEqualityComparer<string>, IEqualityComparer
{
    #region Static Properties

    /// <summary>
    ///     Creates a natural alphanumeric comparer for the current thread culture (case matters; strings are not trimmed before comparison; only pure numbers).
    /// </summary>
    /// <returns>
    ///     The comparer.
    /// </returns>
    public static AlphanumComparer CurrentCulture => new (CultureInfo.CurrentCulture, AlphanumComparerFlags.PureNumbers);

    /// <summary>
    ///     Creates a natural alphanumeric comparer for the current thread culture (case is ignored; strings are not trimmed before comparison; only pure numbers).
    /// </summary>
    /// <returns>
    ///     The comparer.
    /// </returns>
    public static AlphanumComparer CurrentCultureIgnoreCase => new (CultureInfo.CurrentCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.IgnoreCase);

    /// <summary>
    ///     Creates a natural alphanumeric comparer for the invariant culture (case matters; strings are not trimmed before comparison; only pure numbers).
    /// </summary>
    /// <returns>
    ///     The comparer.
    /// </returns>
    public static AlphanumComparer InvariantCulture => new (CultureInfo.InvariantCulture, AlphanumComparerFlags.PureNumbers);

    /// <summary>
    ///     Creates a natural alphanumeric comparer for the invariant culture (case is ignored; strings are not trimmed before comparison; only pure numbers).
    /// </summary>
    /// <returns>
    ///     The comparer.
    /// </returns>
    public static AlphanumComparer InvariantCultureIgnoreCase => new (CultureInfo.InvariantCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.IgnoreCase);

    /// <summary>
    ///     Creates a natural alphanumeric comparer for the current thread culture (case matters; strings are trimmed before comparison; only pure numbers).
    /// </summary>
    /// <returns>
    ///     The comparer.
    /// </returns>
    public static AlphanumComparer TrimmedCurrentCulture => new (CultureInfo.CurrentCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.Trimmed);

    /// <summary>
    ///     Creates a natural alphanumeric comparer for the current thread culture (case is ignored; strings are trimmed before comparison; only pure numbers).
    /// </summary>
    /// <returns>
    ///     The comparer.
    /// </returns>
    public static AlphanumComparer TrimmedCurrentCultureIgnoreCase => new (CultureInfo.CurrentCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.Trimmed | AlphanumComparerFlags.IgnoreCase);

    /// <summary>
    ///     Creates a natural alphanumeric comparer for the invariant culture (case matters; strings are trimmed before comparison; only pure numbers).
    /// </summary>
    /// <returns>
    ///     The comparer.
    /// </returns>
    public static AlphanumComparer TrimmedInvariantCulture => new (CultureInfo.InvariantCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.Trimmed);

    /// <summary>
    ///     Creates a natural alphanumeric comparer for the invariant culture (case is ignored; strings are trimmed before comparison; only pure numbers).
    /// </summary>
    /// <returns>
    ///     The comparer.
    /// </returns>
    public static AlphanumComparer TrimmedInvariantCultureIgnoreCase => new (CultureInfo.InvariantCulture, AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.Trimmed | AlphanumComparerFlags.IgnoreCase);

    #endregion




    #region Instance Constructor/Destructor

    /// <summary>
    ///     Creates a new instance of <see cref="AlphanumComparer" />.
    /// </summary>
    /// <param name="culture"> The used culture. </param>
    /// <param name="options"> The used comparison options. </param>
    /// <exception cref="ArgumentNullException"> <paramref name="culture" /> is null. </exception>
    public AlphanumComparer (CultureInfo culture, AlphanumComparerFlags options)
    {
        Culture = culture;
        Options = options;

        NumberCharacters = new HashSet<string>(StringComparer.Ordinal);

        if ((options & AlphanumComparerFlags.NumberDecimalSeparator) == AlphanumComparerFlags.NumberDecimalSeparator)
        {
            NumberCharacters.Add(Culture.NumberFormat.NumberDecimalSeparator);
        }

        if ((options & AlphanumComparerFlags.PositiveSign) == AlphanumComparerFlags.PositiveSign)
        {
            NumberCharacters.Add(Culture.NumberFormat.PositiveSign);
        }

        if ((options & AlphanumComparerFlags.NegativeSign) == AlphanumComparerFlags.NegativeSign)
        {
            NumberCharacters.Add(Culture.NumberFormat.NegativeSign);
        }

        if ((options & AlphanumComparerFlags.NumberGroupSeparator) == AlphanumComparerFlags.NumberGroupSeparator)
        {
            NumberCharacters.Add(Culture.NumberFormat.NumberGroupSeparator);
        }
    }

    #endregion




    #region Instance Properties/Indexer

    /// <summary>
    ///     Gets the used culture.
    /// </summary>
    /// <value>
    ///     The used culture.
    /// </value>
    public CultureInfo Culture { get; }

    /// <summary>
    ///     Gets whether comparison is performed case-insensitive.
    /// </summary>
    /// <value>
    ///     true if the case is ignored, false otherwise.
    /// </value>
    public bool IgnoreCase => (Options & AlphanumComparerFlags.IgnoreCase) == AlphanumComparerFlags.IgnoreCase;

    /// <summary>
    ///     Gets whether comparison is performed trimmed.
    /// </summary>
    /// <value>
    ///     true if the values are trimmed of whitespaces before being compared.
    /// </value>
    public bool Trimmed => (Options & AlphanumComparerFlags.Trimmed) == AlphanumComparerFlags.Trimmed;

    /// <summary>
    ///     Gets the used comparison options.
    /// </summary>
    /// <value>
    ///     The used comparison options.
    /// </value>
    public AlphanumComparerFlags Options { get; }

    private HashSet<string> NumberCharacters { get; }

    #endregion




    #region Instance Methods

    private int CompareChunks (string x, string y)
    {
        NumberStyles numberCompareStyles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign | NumberStyles.AllowLeadingWhite | NumberStyles.AllowThousands | NumberStyles.AllowTrailingWhite;

        double xValue;
        double yValue;

        if (double.TryParse(x, numberCompareStyles, Culture, out xValue) && double.TryParse(y, numberCompareStyles, Culture, out yValue))
        {
            return xValue.CompareTo(yValue);
        }

        CompareInfo comparer = Culture.CompareInfo;
        int result = comparer.Compare(x, y, IgnoreCase ? CompareOptions.IgnoreCase : CompareOptions.None);
        return result;
    }

    private bool IsDigit (char chr)
    {
        return char.IsDigit(chr) || NumberCharacters.Contains(new string(chr, 1));
    }

    private int ReadWhileSameType (string str, int startIndex, StringBuilder sb)
    {
        bool isDigit = false;
        int readCount;

        for (readCount = startIndex; readCount < str.Length; readCount++)
        {
            char chr = str[readCount];

            if (readCount == 0)
            {
                isDigit = IsDigit(chr);
                sb.Append(chr);
            }
            else
            {
                if (isDigit != this.IsDigit(chr))
                {
                    break;
                }

                sb.Append(chr);
            }
        }

        return readCount;
    }

    #endregion




    #region Interface: IComparer

    /// <inheritdoc />
    public int Compare(object? x, object? y) => Compare((string?)x, (string?)y);

    #endregion




    #region Interface: IComparer<string>

    /// <inheritdoc />
    public int Compare (string? x, string? y)
    {
        if ((x is null) && (y is null))
        {
            return 0;
        }

        if (x is null)
        {
            return -1;
        }

        if (y is null)
        {
            return 1;
        }

        if (ReferenceEquals(x, y))
        {
            return 0;
        }

        if (Trimmed)
        {
            x = x.Trim();
            y = y.Trim();
        }

        if ((x.Length == 0) && (y.Length == 0))
        {
            return 0;
        }

        if (x.Length == 0)
        {
            return -1;
        }

        if (y.Length == 0)
        {
            return 1;
        }

        int xIndex = 0;
        int yIndex = 0;

        StringBuilder xChunk = new StringBuilder();
        StringBuilder yChunk = new StringBuilder();

        while ((xIndex < x.Length) && (yIndex < y.Length))
        {
            xChunk.Remove(0, xChunk.Length);
            yChunk.Remove(0, xChunk.Length);

            int xRead = ReadWhileSameType(x, xIndex, xChunk);
            int yRead = ReadWhileSameType(y, yIndex, yChunk);

            int result = CompareChunks(xChunk.ToString(), yChunk.ToString());

            if (result != 0)
            {
                return result;
            }

            xIndex += xRead;
            yIndex += yRead;
        }

        while (xIndex < x.Length)
        {
            xChunk.Append(x[xIndex]);
            xIndex++;
        }

        while (yIndex < y.Length)
        {
            yChunk.Append(y[yIndex]);
            yIndex++;
        }

        return CompareChunks(xChunk.ToString(), yChunk.ToString());
    }

    #endregion




    #region Interface: IEqualityComparer

    /// <inheritdoc />
    bool IEqualityComparer.Equals(object? x, object? y) => Equals((string?)x, (string?)y);

    /// <inheritdoc />
    int IEqualityComparer.GetHashCode(object? obj) => GetHashCode((string?)obj);

    #endregion




    #region Interface: IEqualityComparer<string>

    /// <inheritdoc />
    public bool Equals(string? x, string? y) => Compare(x, y) == 0;

    /// <inheritdoc />
    public int GetHashCode (string? obj) => obj!.GetHashCode();

    #endregion
}
