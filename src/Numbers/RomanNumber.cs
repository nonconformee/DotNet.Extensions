using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using RI.Utilities.ObjectModel;
using RI.Utilities.Text;




namespace RI.Utilities.Numbers
{
    /// <summary>
    ///     Type to convert and/or store roman numbers.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         You can either use <see cref="RomanNumber" /> to store a roman number by creating instances of it or use its static methods to convert from/to roman numbers.
    ///     </para>
    ///     <para>
    ///         See <see href="https://en.wikipedia.org/wiki/Roman_numerals"> https://en.wikipedia.org/wiki/Roman_numerals </see> for details about roman numbers.
    ///     </para>
    ///     <para>
    ///         Only the uppercase letters I (1), V (5), X (10), L (50), C (100), D (500), M (1000) are supported, lowercase letters of those characters are considered invalid (as is any other character).
    ///     </para>
    ///     <para>
    ///         When converting from roman to decimal, additive and subtractive forms are supported.
    ///         When converting from decimal to roman, the subtractive form is used for certain special cases (see below).
    ///     </para>
    ///     <para>
    ///         The following special cases represent numbers in subtractive form (only these are supported): IV (4), IX (9), XL (40), XC (90), CD (400), CM (900).
    ///     </para>
    ///     <para>
    ///         Both positive and negative numbers are supported as well as zero.
    ///         For positive values, <see cref="DecimalToRoman" /> and <see cref="RomanValue" /> will never add the plus sign as a prefix but parsed values (<see cref="Parse" />, <see cref="TryParse" />, <see cref="DecimalToRoman" />) can have an optional plus sign prefix.
    ///         For negative values, both roman and decimal representations use a minus sign prefix.
    ///         An empty string is used to express zero.
    ///     </para>
    ///     <note type="important">
    ///         Apostrophus and Vinculum are not supported for larger numbers.
    ///         Therefore, the value 1'000'000'000 would result in one million <c> M </c>s.
    ///         It is advised that the numbers are clamped to a reasonable range which is suitable for you.
    ///     </note>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    /// <example>
    ///     <code language="cs">
    /// <![CDATA[
    /// // convert decimal to roman
    /// var roman = RomanNumber.DecimalToRoman(123); // Result: "CXXIII"
    /// 
    /// // convert roman to decimal
    /// var dec = RomanNumber.RomanToDecimal("CLIV"); // Result: 154
    /// 
    /// // create two roman numbers
    /// RomanNumber num1 = 123;
    /// RomanNumber num2 = "CLIV";
    /// 
    /// // do some math with roman numbers
    /// RomanNumber num3 = num1 + num2;
    /// 
    /// // get explicit values
    /// var romanValue = num3.RomanValue; // "CCLXXVII"
    /// var decimalValue = num3.DecimalValue; // 277
    /// ]]>
    /// </code>
    /// </example>
    [Serializable,]
    public struct RomanNumber : IEquatable<RomanNumber>, IComparable<RomanNumber>, IComparable, IFormattable, ICloneable<RomanNumber>, ICloneable, IConvertible
    {
        #region Static Constructor/Destructor

        static RomanNumber ()
        {
            RomanNumber.ValuesToDecimal = new Dictionary<char, int>();
            RomanNumber.ValuesToDecimal.Add('I', 1);
            RomanNumber.ValuesToDecimal.Add('V', 5);
            RomanNumber.ValuesToDecimal.Add('X', 10);
            RomanNumber.ValuesToDecimal.Add('L', 50);
            RomanNumber.ValuesToDecimal.Add('C', 100);
            RomanNumber.ValuesToDecimal.Add('D', 500);
            RomanNumber.ValuesToDecimal.Add('M', 1000);

            RomanNumber.ValuesToRoman = new Dictionary<int, char>();
            RomanNumber.ValuesToRoman.Add(1, 'I');
            RomanNumber.ValuesToRoman.Add(5, 'V');
            RomanNumber.ValuesToRoman.Add(10, 'X');
            RomanNumber.ValuesToRoman.Add(50, 'L');
            RomanNumber.ValuesToRoman.Add(100, 'C');
            RomanNumber.ValuesToRoman.Add(500, 'D');
            RomanNumber.ValuesToRoman.Add(1000, 'M');

            RomanNumber.SpecialCasesToDecimal = new Dictionary<string, int>(StringComparer.Ordinal);
            RomanNumber.SpecialCasesToDecimal.Add("IV", 4);
            RomanNumber.SpecialCasesToDecimal.Add("IX", 9);
            RomanNumber.SpecialCasesToDecimal.Add("XL", 40);
            RomanNumber.SpecialCasesToDecimal.Add("XC", 90);
            RomanNumber.SpecialCasesToDecimal.Add("CD", 400);
            RomanNumber.SpecialCasesToDecimal.Add("CM", 900);

            RomanNumber.SpecialCasesToRoman = new Dictionary<int, string>();
            RomanNumber.SpecialCasesToRoman.Add(4, "IV");
            RomanNumber.SpecialCasesToRoman.Add(9, "IX");
            RomanNumber.SpecialCasesToRoman.Add(40, "XL");
            RomanNumber.SpecialCasesToRoman.Add(90, "XC");
            RomanNumber.SpecialCasesToRoman.Add(400, "CD");
            RomanNumber.SpecialCasesToRoman.Add(900, "CM");
        }

        #endregion




        #region Static Properties/Indexer

        private static Dictionary<string, int> SpecialCasesToDecimal { get; }

        private static Dictionary<int, string> SpecialCasesToRoman { get; }

        private static Dictionary<char, int> ValuesToDecimal { get; }

        private static Dictionary<int, char> ValuesToRoman { get; }

        #endregion




        #region Static Methods

        /// <summary>
        ///     Converts a decimal number into a roman number.
        /// </summary>
        /// <param name="value"> The decimal number. </param>
        /// <returns>
        ///     The roman number as a string.
        /// </returns>
        public static string DecimalToRoman (int value)
        {
            if (value == 0)
            {
                return string.Empty;
            }

            int absValue = Math.Abs(value);
            string stringValue = absValue.ToString("D", CultureInfo.InvariantCulture);

            StringBuilder sb = new StringBuilder();
            int remaining = absValue;

            if (value < 0)
            {
                sb.Append('-');
            }

            if (stringValue.Length > 3)
            {
                string thousandString = stringValue.Substring(0, stringValue.Length - 3);

                int thousandValue = thousandString.ToInt32Invariant()
                                                  .GetValueOrDefault(0);

                sb.Append(new string(RomanNumber.ValuesToRoman[1000], thousandValue));
                remaining -= thousandValue * 1000;
            }

            List<int> specialCases = RomanNumber.SpecialCasesToRoman.Keys.ToList();
            specialCases.Sort();
            specialCases.Reverse();

            List<int> specialCaseMagnitudes = specialCases.Select(x => x.Magnitude())
                                                          .ToList();

            List<int> values = RomanNumber.ValuesToRoman.Keys.ToList();
            values.Remove(1000);
            values.Sort();
            values.Reverse();

            List<int> valueMagnitudes = values.Select(x => x.Magnitude())
                                              .ToList();

            while (true)
            {
                int magnitude = remaining.Magnitude();

                if (magnitude <= 0)
                {
                    break;
                }

                if (RomanNumber.SpecialCasesToRoman.ContainsKey(remaining))
                {
                    sb.Append(RomanNumber.SpecialCasesToRoman[remaining]);
                    break;
                }

                if (RomanNumber.ValuesToRoman.ContainsKey(remaining))
                {
                    sb.Append(RomanNumber.ValuesToRoman[remaining]);
                    break;
                }

                bool continueAfterSpecialCase = false;

                for (int i1 = 0; i1 < specialCases.Count; i1++)
                {
                    int specialCase = specialCases[i1];
                    int specialCaseMagnitude = specialCaseMagnitudes[i1];

                    if (specialCaseMagnitude != magnitude)
                    {
                        continue;
                    }

                    int remainingCandidate = remaining - specialCase;
                    int remainingCandidateMagnitude = remainingCandidate.Magnitude();

                    if ((remainingCandidateMagnitude < magnitude) && (remainingCandidateMagnitude >= 0))
                    {
                        sb.Append(RomanNumber.SpecialCasesToRoman[specialCase]);
                        remaining = remainingCandidate;
                        continueAfterSpecialCase = true;
                        break;
                    }
                }

                if (continueAfterSpecialCase)
                {
                    continue;
                }

                for (int i2 = 0; i2 < values.Count; i2++)
                {
                    int currentValue = values[i2];
                    int currentValueMagnitude = valueMagnitudes[i2];

                    if (currentValueMagnitude != magnitude)
                    {
                        continue;
                    }

                    int valueCount = 0;

                    while (remaining >= currentValue)
                    {
                        remaining -= currentValue;
                        valueCount++;
                    }

                    if (valueCount > 0)
                    {
                        sb.Append(new string(RomanNumber.ValuesToRoman[currentValue], valueCount));
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Adds two <see cref="RomanNumber" />s.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The result.
        /// </returns>
        public static RomanNumber operator + (RomanNumber x, RomanNumber y)
        {
            return new RomanNumber(x.DecimalValue + y.DecimalValue);
        }

        /// <summary>
        ///     Decrements a <see cref="RomanNumber" /> by 1.
        /// </summary>
        /// <param name="x"> The value. </param>
        /// <returns>
        ///     The result.
        /// </returns>
        public static RomanNumber operator -- (RomanNumber x)
        {
            return new RomanNumber(x.DecimalValue - 1);
        }

        /// <summary>
        ///     Divides a <see cref="RomanNumber" /> by another.
        /// </summary>
        /// <param name="x"> The value to divide. </param>
        /// <param name="y"> The value to divide by. </param>
        /// <returns>
        ///     The result.
        /// </returns>
        public static RomanNumber operator / (RomanNumber x, RomanNumber y)
        {
            return new RomanNumber(x.DecimalValue / y.DecimalValue);
        }

        /// <summary>
        ///     Compares two <see cref="RomanNumber" />s for equality.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     true if both values are equal, false otherwise.
        /// </returns>
        public static bool operator == (RomanNumber x, RomanNumber y)
        {
            return x.DecimalValue == y.DecimalValue;
        }

        /// <summary>
        ///     Checks if a <see cref="RomanNumber" /> is greater than another.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     true if the first value is greater than the second value, false otherwise.
        /// </returns>
        public static bool operator > (RomanNumber x, RomanNumber y)
        {
            return x.DecimalValue > y.DecimalValue;
        }

        /// <summary>
        ///     Checks if a <see cref="RomanNumber" /> is greater or equal than another.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     true if the first value is greater or equal than the second value, false otherwise.
        /// </returns>
        public static bool operator >= (RomanNumber x, RomanNumber y)
        {
            return x.DecimalValue >= y.DecimalValue;
        }

        /// <summary>
        ///     Implicitly converts a string to a <see cref="RomanNumber" />.
        /// </summary>
        /// <param name="value"> The string to convert. </param>
        /// <returns>
        ///     The <see cref="RomanNumber" />.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value" /> is null. </exception>
        /// <exception cref="FormatException"> <paramref name="value" /> is not a valid roman number. </exception>
        public static implicit operator RomanNumber (string value)
        {
            return RomanNumber.Parse(value);
        }

        /// <summary>
        ///     Implicitly converts an integer to a <see cref="RomanNumber" />.
        /// </summary>
        /// <param name="value"> The integer to convert. </param>
        /// <returns>
        ///     The <see cref="RomanNumber" />.
        /// </returns>
        public static implicit operator RomanNumber (int value)
        {
            return new RomanNumber(value);
        }

        /// <summary>
        ///     Implicitly converts a <see cref="RomanNumber" /> to a string.
        /// </summary>
        /// <param name="value"> The value to convert. </param>
        /// <returns>
        ///     The string.
        /// </returns>
        public static implicit operator string (RomanNumber value)
        {
            return value.RomanValue;
        }

        /// <summary>
        ///     Implicitly converts a <see cref="RomanNumber" /> to an integer.
        /// </summary>
        /// <param name="value"> The value to convert. </param>
        /// <returns>
        ///     The integer.
        /// </returns>
        public static implicit operator int (RomanNumber value)
        {
            return value.DecimalValue;
        }

        /// <summary>
        ///     Increments a <see cref="RomanNumber" /> by 1.
        /// </summary>
        /// <param name="x"> The value. </param>
        /// <returns>
        ///     The result.
        /// </returns>
        public static RomanNumber operator ++ (RomanNumber x)
        {
            return new RomanNumber(x.DecimalValue + 1);
        }

        /// <summary>
        ///     Compares two <see cref="RomanNumber" />s for inequality.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     false if both values are equal, true otherwise.
        /// </returns>
        public static bool operator != (RomanNumber x, RomanNumber y)
        {
            return x.DecimalValue != y.DecimalValue;
        }

        /// <summary>
        ///     Checks if a <see cref="RomanNumber" /> is less than another.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     true if the first value is less than the second value, false otherwise.
        /// </returns>
        public static bool operator < (RomanNumber x, RomanNumber y)
        {
            return x.DecimalValue < y.DecimalValue;
        }

        /// <summary>
        ///     Checks if a <see cref="RomanNumber" /> is less or equal than another.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     true if the first value is less or equal than the second value, false otherwise.
        /// </returns>
        public static bool operator <= (RomanNumber x, RomanNumber y)
        {
            return x.DecimalValue <= y.DecimalValue;
        }

        /// <summary>
        ///     Gets the remainder from a division of a <see cref="RomanNumber" /> by another.
        /// </summary>
        /// <param name="x"> The value to divide. </param>
        /// <param name="y"> The value to divide by. </param>
        /// <returns>
        ///     The result.
        /// </returns>
        public static RomanNumber operator % (RomanNumber x, RomanNumber y)
        {
            return new RomanNumber(x.DecimalValue % y.DecimalValue);
        }

        /// <summary>
        ///     Multiplies two <see cref="RomanNumber" />s.
        /// </summary>
        /// <param name="x"> The first value. </param>
        /// <param name="y"> The second value. </param>
        /// <returns>
        ///     The result.
        /// </returns>
        public static RomanNumber operator * (RomanNumber x, RomanNumber y)
        {
            return new RomanNumber(x.DecimalValue * y.DecimalValue);
        }

        /// <summary>
        ///     Subtracts a <see cref="RomanNumber" /> from another.
        /// </summary>
        /// <param name="x"> The value subtracted from. </param>
        /// <param name="y"> The value to subtract. </param>
        /// <returns>
        ///     The result.
        /// </returns>
        public static RomanNumber operator - (RomanNumber x, RomanNumber y)
        {
            return new RomanNumber(x.DecimalValue + y.DecimalValue);
        }

        /// <summary>
        ///     Multiplies a <see cref="RomanNumber" /> with -1.
        /// </summary>
        /// <param name="x"> The value. </param>
        /// <returns>
        ///     The result.
        /// </returns>
        public static RomanNumber operator - (RomanNumber x)
        {
            return new RomanNumber(x.DecimalValue * -1);
        }

        /// <summary>
        ///     Multiplies a <see cref="RomanNumber" /> with 1.
        /// </summary>
        /// <param name="x"> The value. </param>
        /// <returns>
        ///     The result.
        /// </returns>
        public static RomanNumber operator + (RomanNumber x)
        {
            return new RomanNumber(x.DecimalValue);
        }

        /// <summary>
        ///     Parses a string as a roman number.
        /// </summary>
        /// <param name="str"> The string to parse. </param>
        /// <returns>
        ///     The roman number.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        /// <exception cref="FormatException"> <paramref name="str" /> is not a valid roman number. </exception>
        public static RomanNumber Parse (string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            RomanNumber value;

            if (!RomanNumber.TryParse(str, out value))
            {
                throw new FormatException("\"" + str + "\" is not a valid roman number.");
            }

            return value;
        }

        /// <summary>
        ///     Tries to convert a roman number into a decimal number.
        /// </summary>
        /// <param name="value"> The roman number as a string. </param>
        /// <returns>
        ///     The decimal number or null if <paramref name="value" /> is not a vlid roman number.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="value" /> is null. </exception>
        public static int? RomanToDecimal (string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.IsEmpty())
            {
                return 0;
            }

            int start = 0;
            int sum = 0;
            int factor = 1;

            if (value[0] == '-')
            {
                start = 1;
                factor = -1;
            }
            else if (value[0] == '+')
            {
                start = 1;
                factor = 1;
            }

            for (int i1 = start; i1 < value.Length; i1++)
            {
                if (i1 < (value.Length - 1))
                {
                    string specialCaseCandidate = value.Substring(i1, 2);

                    if (RomanNumber.SpecialCasesToDecimal.ContainsKey(specialCaseCandidate))
                    {
                        sum += RomanNumber.SpecialCasesToDecimal[specialCaseCandidate];
                        i1++;
                        continue;
                    }
                }

                char currentCharacter = value[i1];

                if (RomanNumber.ValuesToDecimal.ContainsKey(currentCharacter))
                {
                    sum += RomanNumber.ValuesToDecimal[currentCharacter];
                    continue;
                }

                return null;
            }

            return sum * factor;
        }

        /// <summary>
        ///     Tries to parse a string as a roman number.
        /// </summary>
        /// <param name="str"> The string to parse. </param>
        /// <param name="value"> The parsed roman number. </param>
        /// <returns>
        ///     true if <paramref name="str" /> was a valid roman number, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static bool TryParse (string str, out RomanNumber value)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            int? decimalValue = RomanNumber.RomanToDecimal(str);

            if (decimalValue.HasValue)
            {
                value = new RomanNumber(decimalValue.Value);
                return true;
            }

            value = new RomanNumber();
            return false;
        }

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="RomanNumber" />.
        /// </summary>
        /// <param name="decimalValue"> The number as a decimal number. </param>
        public RomanNumber (int decimalValue)
        {
            this.DecimalValue = decimalValue;
            this._romanValue = null;
        }

        #endregion




        #region Instance Fields

        private string _romanValue;

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the value as a decimal value.
        /// </summary>
        /// <value>
        ///     The value as a decimal value.
        /// </value>
        public int DecimalValue { get; }

        /// <summary>
        ///     Gets the value as a roman value.
        /// </summary>
        /// <value>
        ///     The value as a roman value.
        /// </value>
        public string RomanValue
        {
            get
            {
                if (this._romanValue == null)
                {
                    this._romanValue = RomanNumber.DecimalToRoman(this.DecimalValue);
                }

                return this._romanValue;
            }
        }

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override bool Equals (object obj)
        {
            if (obj is RomanNumber)
            {
                return this.Equals((RomanNumber)obj);
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode ()
        {
            return this.DecimalValue;
        }

        /// <inheritdoc />
        public override string ToString ()
        {
            return this.RomanValue;
        }

        #endregion




        #region Interface: ICloneable<RomanNumber>

        /// <inheritdoc />
        object ICloneable.Clone ()
        {
            return this.Clone();
        }

        /// <inheritdoc />
        public RomanNumber Clone ()
        {
            return new RomanNumber(this.DecimalValue);
        }

        #endregion




        #region Interface: IComparable

        /// <inheritdoc />
        int IComparable.CompareTo (object obj)
        {
            if (obj is RomanNumber)
            {
                return this.CompareTo((RomanNumber)obj);
            }

            return 1;
        }

        #endregion




        #region Interface: IComparable<RomanNumber>

        /// <inheritdoc />
        public int CompareTo (RomanNumber other)
        {
            return this.DecimalValue.CompareTo(other.DecimalValue);
        }

        #endregion




        #region Interface: IConvertible

        TypeCode IConvertible.GetTypeCode ()
        {
            return TypeCode.Object;
        }

        bool IConvertible.ToBoolean (IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert from " + nameof(RomanNumber) + " to " + nameof(Boolean) + ".");
        }

        byte IConvertible.ToByte (IFormatProvider provider)
        {
            return (byte)this.DecimalValue;
        }

        char IConvertible.ToChar (IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert from " + nameof(RomanNumber) + " to " + nameof(Char) + ".");
        }

        DateTime IConvertible.ToDateTime (IFormatProvider provider)
        {
            throw new InvalidCastException("Cannot convert from " + nameof(RomanNumber) + " to " + nameof(DateTime) + ".");
        }

        decimal IConvertible.ToDecimal (IFormatProvider provider)
        {
            return this.DecimalValue;
        }

        double IConvertible.ToDouble (IFormatProvider provider)
        {
            return this.DecimalValue;
        }

        short IConvertible.ToInt16 (IFormatProvider provider)
        {
            return (short)this.DecimalValue;
        }

        int IConvertible.ToInt32 (IFormatProvider provider)
        {
            return this.DecimalValue;
        }

        long IConvertible.ToInt64 (IFormatProvider provider)
        {
            return this.DecimalValue;
        }

        sbyte IConvertible.ToSByte (IFormatProvider provider)
        {
            return (sbyte)this.DecimalValue;
        }

        float IConvertible.ToSingle (IFormatProvider provider)
        {
            return this.DecimalValue;
        }

        string IConvertible.ToString (IFormatProvider provider)
        {
            return this.ToString();
        }

        object IConvertible.ToType (Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(this.DecimalValue, conversionType, provider);
        }

        ushort IConvertible.ToUInt16 (IFormatProvider provider)
        {
            return (ushort)this.DecimalValue;
        }

        uint IConvertible.ToUInt32 (IFormatProvider provider)
        {
            return (uint)this.DecimalValue;
        }

        ulong IConvertible.ToUInt64 (IFormatProvider provider)
        {
            return (ulong)this.DecimalValue;
        }

        #endregion




        #region Interface: IEquatable<RomanNumber>

        /// <inheritdoc />
        public bool Equals (RomanNumber other)
        {
            return this.DecimalValue == other.DecimalValue;
        }

        #endregion




        #region Interface: IFormattable

        /// <inheritdoc />
        public string ToString (string format, IFormatProvider formatProvider)
        {
            if (format == null)
            {
                return this.ToString();
            }

            if (string.Equals(format, "g", StringComparison.InvariantCultureIgnoreCase))
            {
                return this.ToString();
            }

            return this.DecimalValue.ToString(format, formatProvider);
        }

        #endregion
    }
}
