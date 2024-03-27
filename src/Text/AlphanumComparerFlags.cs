namespace nonconformee.DotNet.Extensions.Text;




/// <summary>
///     Specifies options for string comparison using <see cref="AlphanumComparer" />.
/// </summary>
[Serializable,]
[Flags,]
public enum AlphanumComparerFlags
{
    /// <summary>
    ///     The case of strings and their compared chunks is ignored.
    /// </summary>
    IgnoreCase = 0x01,

    /// <summary>
    ///     Strings and their compared chunks are trimmed of any leading or trailing whitespace before being compared.
    /// </summary>
    Trimmed = 0x02,

    /// <summary>
    ///     Recognizes the culture-specific decimal separator as part of a number.
    /// </summary>
    NumberDecimalSeparator = 0x04,

    /// <summary>
    ///     Recognizes the culture-specific positive sign as part of a number.
    /// </summary>
    PositiveSign = 0x08,

    /// <summary>
    ///     Recognizes the culture-specific negative sign as part of a number.
    /// </summary>
    NegativeSign = 0x10,

    /// <summary>
    ///     Recognizes the culture-specific number group separator as part of a number.
    /// </summary>
    NumberGroupSeparator = 0x20,

    /// <summary>
    ///     Only recognizes pure numbers (that is: digits without any of these: decimal separator, positive sign, negative sign, or number group separator).
    /// </summary>
    PureNumbers = 0x00,

    /// <summary>
    ///     Recognizes positive or negative integers (that is: digits with optional positive sign or negative sign but without any of these: decimal separator, number group separator).
    /// </summary>
    SignedNumbers = AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.PositiveSign | AlphanumComparerFlags.NegativeSign,

    /// <summary>
    ///     Recognizes positive or negative decimals (that is: digits with optional positive sign, negative and/or decimal separator sign but without number group separator).
    /// </summary>
    DecimalNumbers = AlphanumComparerFlags.PureNumbers | AlphanumComparerFlags.PositiveSign | AlphanumComparerFlags.NegativeSign | AlphanumComparerFlags.NumberDecimalSeparator,
}
