using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;

using RI.Utilities.Binary;
using RI.Utilities.Dates;
using RI.Utilities.Exceptions;
using RI.Utilities.Numbers;




namespace RI.Utilities.Text
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="string" /> type.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Note that some of the functionality provided by this class is rather specialized and intended to be used for certain string parsing/processing purposes.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public static class StringExtensions
    {
        #region Constants

        private static readonly string[] BooleanFalseValues =
        {
            "false",
            "no",
            "0",
            "off",
            "disabled",
        };

        private static readonly string[] BooleanTrueValues =
        {
            "true",
            "yes",
            "1",
            "on",
            "enabled",
        };

        #endregion




        #region Static Methods

        /// <summary>
        ///     Computes a GUID of a string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="encoding"> The encoding or null to use <see cref="Encoding.UTF8" />. Default value is null. </param>
        /// <returns>
        ///     The GUID.
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         <see cref="ComputeGuid" /> first reduces the string to a MD5 hash and uses this to construct a GUID.
        ///     </note>
        ///     <note type="security">
        ///         Do not use <see cref="ComputeGuid" /> for security relevant operations as it uses MD5 internally.
        ///     </note>
        /// </remarks>
        public static Guid ComputeGuid (this string str, Encoding encoding = null)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return Guid.Empty;
            }

            byte[] bytes = str.ComputeMd5(encoding);
            return new Guid(bytes);
        }

        /// <summary>
        ///     Computes the MD5 hash of a string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="encoding"> The encoding or null to use <see cref="Encoding.UTF8" />. Default value is null. </param>
        /// <returns>
        ///     The MD5 hash.
        /// </returns>
        /// <remarks>
        ///     <note type="security">
        ///         Do not use <see cref="ComputeMd5" /> for security relevant operations.
        ///     </note>
        /// </remarks>
        public static byte[] ComputeMd5 (this string str, Encoding encoding = null)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return new byte[16];
            }

            byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(str);
            return bytes.ComputeMd5();
        }

        /// <summary>
        ///     Computes the SHA256 hash of a string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="encoding"> The encoding or null to use <see cref="Encoding.UTF8" />. Default value is null. </param>
        /// <returns>
        ///     The SHA256 hash.
        /// </returns>
        public static byte[] ComputeSha256 (this string str, Encoding encoding = null)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return new byte[32];
            }

            byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(str);
            return bytes.ComputeSha256();
        }

        /// <summary>
        ///     Computes the SHA512 hash of a string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="encoding"> The encoding or null to use <see cref="Encoding.UTF8" />. Default value is null. </param>
        /// <returns>
        ///     The SHA512 hash.
        /// </returns>
        public static byte[] ComputeSha512 (this string str, Encoding encoding = null)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return new byte[32];
            }

            byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(str);
            return bytes.ComputeSha512();
        }

        /// <summary>
        ///     Determines whether a specified string occurs in a string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="value"> The string to find in the string. </param>
        /// <param name="comparisonType"> The string comparison used to find the string. </param>
        /// <returns>
        ///     true if <paramref name="value" /> is an empty string or it occurs at least once in the string, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> or <paramref name="value" /> is null. </exception>
        public static bool Contains (this string str, string value, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return str.IndexOf(value, comparisonType) != -1;
        }

        /// <summary>
        ///     Determines whether a specified string contains any whitespace characters.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     true if the string contains any whistespace characters, false otherwise.
        ///     If the string has a length of zero, false is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static bool ContainsWhitespace (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            for (int i1 = 0; i1 < str.Length; i1++)
            {
                if (char.IsWhiteSpace(str[i1]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Decodes a Base64 string to a byte array.
        /// </summary>
        /// <param name="str"> The Base64 string. </param>
        /// <returns> The decoded byte array. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static byte[] DecodeBase64Binary (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return new byte[0];
            }

            return Convert.FromBase64String(str);
        }

        /// <summary>
        ///     Decodes a Base64 string to a string.
        /// </summary>
        /// <param name="str"> The Base64 string. </param>
        /// <param name="encoding"> The encoding or null to use <see cref="Encoding.UTF8" />. Default value is null. </param>
        /// <returns> The decoded string. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string DecodeBase64Text (this string str, Encoding encoding = null)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            byte[] bytes = str.DecodeBase64Binary();
            return (encoding ?? Encoding.UTF8).GetString(bytes);
        }

        /// <summary>
        ///     Decodes a hexadecimal string to a byte array.
        /// </summary>
        /// <param name="str"> The hexadecimal string. </param>
        /// <returns> The decoded byte array. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static byte[] DecodeHexBinary (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return new byte[0];
            }

            byte[] data = new byte[str.Length / 2];

            for (int i1 = 0; i1 < (str.Length / 2); i1++)
            {
                string value = str.Substring(i1 * 2, 2);
                data[i1] = byte.Parse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }

        /// <summary>
        ///     Decodes a hexadecimal string to a string.
        /// </summary>
        /// <param name="str"> The hexadecimal string. </param>
        /// <param name="encoding"> The encoding or null to use <see cref="Encoding.UTF8" />. Default value is null. </param>
        /// <returns> The decoded string. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string DecodeHexText (this string str, Encoding encoding = null)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            byte[] bytes = str.DecodeHexBinary();
            return (encoding ?? Encoding.UTF8).GetString(bytes);
        }

        /// <summary>
        ///     Decodes a HTML compatible string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The decoded string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string DecodeHtml (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            return WebUtility.HtmlDecode(str);
        }

        /// <summary>
        ///     Decodes a URL compatible string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The decoded string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string DecodeUrl (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            return WebUtility.UrlDecode(str);
        }

        /// <summary>
        ///     Doubles each occurence of a specified character in a string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="chr"> The character of which each occurence is doubled. </param>
        /// <returns>
        ///     The resulting string with each specified character occurence doubled.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Doubling occurrences is done on a per-character basis and case-sensitive.
        ///     </para>
        ///     <para>
        ///         For example, when doubling the occurence for 'A', the string "" results in "", "A" in "AA", "AA" in "AAAA", "ABC" in "AABC", etc.
        ///     </para>
        ///     <para>
        ///         This is the same as using <see cref="StringExtensions.ModifyOccurrence" /> with a factor of 2.0 and offset of 0.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string DoubleOccurrence (this string str, char chr)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return str.ModifyOccurrence(chr, 2.0, 0);
        }

        /// <summary>
        ///     Encodes a string as Base64 string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="options"> The Base64 formatting options. Default value is <see cref="Base64FormattingOptions.None" />. </param>
        /// <param name="encoding"> The encoding or null to use <see cref="Encoding.UTF8" />. Default value is null. </param>
        /// <returns> The Base64 string or an empty string if the string is empty. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string EncodeBase64 (this string str, Base64FormattingOptions options = Base64FormattingOptions.None, Encoding encoding = null)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(str);
            return bytes.EncodeBase64(options);
        }

        /// <summary>
        ///     Encodes a string as a hexadecimal string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="encoding"> The encoding or null to use <see cref="Encoding.UTF8" />. Default value is null. </param>
        /// <returns>
        ///     The hexadecimal string or an empty string if the string is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string EncodeHex (this string str, Encoding encoding = null)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            byte[] bytes = (encoding ?? Encoding.UTF8).GetBytes(str);
            return bytes.EncodeHex();
        }

        /// <summary>
        ///     Encodes a string to be usable in HTML.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The HTML compatible string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string EncodeHtml (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            return WebUtility.HtmlEncode(str);
        }

        /// <summary>
        ///     Encodes a string into a byte array.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="encoding"> The encoding or null to use <see cref="Encoding.UTF8" />. Default value is null. </param>
        /// <returns> The byte array containing the encoded string. </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static byte[] EncodeString (this string str, Encoding encoding = null)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return new byte[0];
            }

            return (encoding ?? Encoding.UTF8).GetBytes(str);
        }

        /// <summary>
        ///     Encodes a string to be usable in a URL.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The URL compatible string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string EncodeUrl (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            return WebUtility.UrlEncode(str);
        }

        /// <summary>
        ///     Counts how many times a string ends with a specified character.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="value"> The character to count when occuring at the end of the string. </param>
        /// <param name="comparisonType"> The string comparison used to find the character. </param>
        /// <returns>
        ///     The number of times the specified character appears in succession at the end of the string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static int EndsWithCount (this string str, char value, StringComparison comparisonType)
        {
            return str.EndsWithCount(new string(value, 1), comparisonType);
        }

        /// <summary>
        ///     Counts how many times a string ends with a specified string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="value"> The string to count when occuring at the end of the string. </param>
        /// <param name="comparisonType"> The string comparison used to find the string. </param>
        /// <returns>
        ///     The number of times the specified string appears in succession at the end of the string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> or <paramref name="value" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="value" /> is a string with zero length. </exception>
        public static int EndsWithCount (this string str, string value, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length == 0)
            {
                throw new EmptyStringArgumentException(nameof(value));
            }

            int count = 0;
            int index = str.Length - value.Length;

            while (true)
            {
                if (index < 0)
                {
                    break;
                }

                string comparedPiece = str.Substring(index, value.Length);

                if (string.Equals(value, comparedPiece, comparisonType))
                {
                    count++;
                    index -= value.Length;
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        /// <summary>
        ///     Converts a string into another string where certain special characters are converted to escape sequences.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="options"> The conversion options. </param>
        /// <returns>
        ///     The resulting string with special characters converted to escape sequences.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         An escape sequence always starts with \ followed by a single character specifying the escape sequence, e.g. \n for new-line.
        ///     </para>
        ///     <para>
        ///         The following special characters are escaped: \a, \b, \f, \n, \r, \t, \v, \, ', ".
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string Escape (this string str, StringEscapeOptions options)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(str.Length * 2);

            for (int i1 = 0; i1 < str.Length; i1++)
            {
                char current = str[i1];
                string replacement = null;

                if ((current == '\a') && ((options & StringEscapeOptions.Alert) == StringEscapeOptions.Alert))
                {
                    replacement = "\\a";
                }
                else if ((current == '\b') && ((options & StringEscapeOptions.Backspace) == StringEscapeOptions.Backspace))
                {
                    replacement = "\\b";
                }
                else if ((current == '\f') && ((options & StringEscapeOptions.Formfeed) == StringEscapeOptions.Formfeed))
                {
                    replacement = "\\f";
                }
                else if ((current == '\n') && ((options & StringEscapeOptions.Newline) == StringEscapeOptions.Newline))
                {
                    replacement = "\\n";
                }
                else if ((current == '\r') && ((options & StringEscapeOptions.CarriageReturn) == StringEscapeOptions.CarriageReturn))
                {
                    replacement = "\\r";
                }
                else if ((current == '\t') && ((options & StringEscapeOptions.HorizontalTab) == StringEscapeOptions.HorizontalTab))
                {
                    replacement = "\\t";
                }
                else if ((current == '\v') && ((options & StringEscapeOptions.VerticalTap) == StringEscapeOptions.VerticalTap))
                {
                    replacement = "\\v";
                }
                else if ((current == '\\') && ((options & StringEscapeOptions.Backslash) == StringEscapeOptions.Backslash))
                {
                    replacement = "\\\\";
                }
                else if ((current == '\'') && ((options & StringEscapeOptions.SingleQuote) == StringEscapeOptions.SingleQuote))
                {
                    replacement = "\\\'";
                }
                else if ((current == '\"') && ((options & StringEscapeOptions.DoubleQuote) == StringEscapeOptions.DoubleQuote))
                {
                    replacement = "\\\"";
                }
                else if ((current == '?') && ((options & StringEscapeOptions.QuestionMark) == StringEscapeOptions.QuestionMark))
                {
                    replacement = "\\?";
                }

                if (replacement != null)
                {
                    sb.Append(replacement);
                }
                else
                {
                    sb.Append(current);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Halves each occurence of a specified character in a string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="chr"> The character of which each occurence is halved. </param>
        /// <returns>
        ///     The resulting string with each specified character occurrence halved.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Halving occurrences is done on a per-character basis and case-sensitive.
        ///     </para>
        ///     <para>
        ///         For example, when halving the occurence for 'A', the string "" results in "", "A" in "", "AA" results in "A", "AAA" in "A", "AAAA" in "AA", "ABC" in "BC", etc.
        ///     </para>
        ///     <para>
        ///         This is the same as using <see cref="StringExtensions.ModifyOccurrence" /> with a factor of 0.5 and offset of 0.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string HalveOccurrence (this string str, char chr)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return str.ModifyOccurrence(chr, 0.5, 0);
        }

        /// <summary>
        ///     Determines whether a string is empty.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     true if the string is empty, false otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A string is considered empty if has a length of zero.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static bool IsEmpty (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return str.Length == 0;
        }

        /// <summary>
        ///     Determines whether a string is empty or consists only of whitespace.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     true if the string is empty or consists only of whitespaces, false otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A string is considered empty if has a length of zero.
        ///     </para>
        ///     <para>
        ///         A string is considered consisting only of whitespaces if it is not empty and only has whitespace characters.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static bool IsEmptyOrWhitespace (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return true;
            }

            for (int i1 = 0; i1 < str.Length; i1++)
            {
                if (!char.IsWhiteSpace(str[i1]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Determines whether a string is null.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     true if the string is null, false otherwise.
        /// </returns>
        public static bool IsNull (this string str)
        {
            return str == null;
        }

        /// <summary>
        ///     Determines whether a string is null or empty.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     true if the string is null or empty, false otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A string is considered empty if has a length of zero.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static bool IsNullOrEmpty (this string str)
        {
            if (str == null)
            {
                return true;
            }

            return str.IsEmpty();
        }

        /// <summary>
        ///     Determines whether a string is null, empty, or consists only of whitespace.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     true if the string is null, empty, or consists only of whitespaces, false otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A string is considered empty if has a length of zero.
        ///     </para>
        ///     <para>
        ///         A string is considered consisting only of whitespaces if it is not empty and only has whitespace characters.
        ///     </para>
        /// </remarks>
        public static bool IsNullOrEmptyOrWhitespace (this string str)
        {
            if (str == null)
            {
                return true;
            }

            if (str.Length == 0)
            {
                return true;
            }

            for (int i1 = 0; i1 < str.Length; i1++)
            {
                if (!char.IsWhiteSpace(str[i1]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Determines whether a string is null or consists only of whitespaces.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     true if the string is null or consists only of whitespaces, false otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A string is considered consisting only of whitespaces if it is not empty and only has whitespace characters.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static bool IsNullOrWhitespace (this string str)
        {
            if (str == null)
            {
                return true;
            }

            return str.IsWhitespace();
        }

        /// <summary>
        ///     Determines whether a string consists only of whitespaces.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     true if the string consists only of whitespaces, false otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A string is considered consisting only of whitespaces if it is not empty and only has whitespace characters.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static bool IsWhitespace (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return false;
            }

            for (int i1 = 0; i1 < str.Length; i1++)
            {
                if (!char.IsWhiteSpace(str[i1]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Joins a sequence of strings together into one string without a separator between each string element.
        /// </summary>
        /// <param name="values"> The sequence of strings to join together </param>
        /// <returns>
        ///     The resulting string with each string element concatenated to the next.
        ///     The resulting string has a length of zero if the sequence contains no string elements or only string elements of zero length.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="values" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static string Join (this IEnumerable<string> values)
        {
            return values.Join(string.Empty);
        }

        /// <summary>
        ///     Joins a sequence of strings together into one string with a specified separator character between each string element.
        /// </summary>
        /// <param name="values"> The sequence of strings to join together </param>
        /// <param name="separator"> The used separator character. </param>
        /// <returns>
        ///     The resulting string with each string element concatenated to the next, separated by the specified separator character.
        ///     The resulting string has a length of zero if the sequence contains no string elements or only string elements of zero length.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="values" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static string Join (this IEnumerable<string> values, char separator)
        {
            return values.Join(new string(separator, 1));
        }

        /// <summary>
        ///     Joins a sequence of strings together into one string with a specified separator string between each string element.
        /// </summary>
        /// <param name="values"> The sequence of strings to join together </param>
        /// <param name="separator"> The used separator string. Can be null or <see cref="string" />.<see cref="string.Empty" /> if no separator should be used. </param>
        /// <returns>
        ///     The resulting string with each string element concatenated to the next, separated by the specified separator string.
        ///     The resulting string has a length of zero if the sequence contains no string elements or only string elements of zero length.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="values" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static string Join (this IEnumerable<string> values, string separator)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            return string.Join(separator ?? string.Empty, values.ToArray());
        }

        /// <summary>
        ///     Keeps only characters of a string based on a predicate.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="predicate"> The predicate used to test each character of the string. </param>
        /// <returns>
        ///     The string where each character is preserved based on the predicate.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> or <paramref name="predicate" /> is null. </exception>
        public static string Keep (this string str, Predicate<char> predicate)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            StringBuilder sb = new StringBuilder(str.Length);

            foreach (char c in str)
            {
                if (predicate(c))
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Gets the maximum length of multiple strings.
        /// </summary>
        /// <param name="values"> The sequence of strings to get the maximum length from. </param>
        /// <returns>
        ///     The length of the string with the maximum length or zero if <paramref name="values" /> is empty.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="values" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static int MaxLength (this IEnumerable<string> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            int max = int.MinValue;
            int count = 0;

            foreach (string str in values)
            {
                count++;

                if (str.Length > max)
                {
                    max = str.Length;
                }
            }

            return count == 0 ? 0 : max;
        }

        /// <summary>
        ///     Gets the minimum length of multiple strings.
        /// </summary>
        /// <param name="values"> The sequence of strings to get the minimum length from. </param>
        /// <param name="ignoreZeroLength"> Specifies whether string with zero length do not count. </param>
        /// <returns>
        ///     The length of the string with the minimum length or zero if <paramref name="values" /> is empty.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <paramref name="values" /> is enumerated exactly once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="values" /> is null. </exception>
        public static int MinLength (this IEnumerable<string> values, bool ignoreZeroLength)
        {
            if (values == null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            int min = int.MaxValue;
            int count = 0;

            foreach (string str in values)
            {
                if (!ignoreZeroLength || (str.Length > 0))
                {
                    count++;

                    if (str.Length < min)
                    {
                        min = str.Length;
                    }
                }
            }

            return count == 0 ? 0 : min;
        }

        /// <summary>
        ///     Modifies each occurence of a specified character in a string by a specified factor and/or offset.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="chr"> The character of which each occurence is modified. </param>
        /// <param name="factor"> The factor used by which each occurence is modified. </param>
        /// <param name="offset"> The offset used by which each occurence is modified. </param>
        /// <returns>
        ///     The resulting string with each specified character occurence modified.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Modifying occurrences is done on a per-character basis and case-sensitive.
        ///     </para>
        ///     <para>
        ///         Example with <paramref name="factor" /> of 3.0 and <paramref name="offset" /> of 0 for the character 'A': "" -> "", "A" -> "AAA", "AA" -> "AAAAAA", "AAA" -> "AAAAAAAAA", "ABC" -> "AAABC", etc.
        ///         Example with <paramref name="factor" /> of 0.0 and <paramref name="offset" /> of 1 for the character 'A': "" -> "", "A" -> "AA", "AA" -> "AAA", "AAAA" -> "AAAAA",  "ABC" -> "AABC", etc.
        ///     </para>
        ///     <para>
        ///         If <paramref name="factor" /> and <paramref name="offset" /> are both used (<paramref name="factor" /> not 1.0 and <paramref name="offset" /> not 0), the resulting character count is ((original count * <paramref name="factor" />) + <paramref name="offset" />).
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        /// <exception cref="NotFiniteArgumentException"> <paramref name="factor" /> is "NaN"/"Not-a-Number" or infinity (either positive or negative). </exception>
        public static string ModifyOccurrence (this string str, char chr, double factor, int offset)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (factor.IsNanOrInfinity())
            {
                throw new NotFiniteArgumentException(nameof(factor));
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            double count = 0.0;

            StringBuilder newStr = new StringBuilder();

            for (int i1 = 0; i1 < str.Length; i1++)
            {
                if (str[i1] == chr)
                {
                    char next = i1 >= (str.Length - 1) ? '\0' : str[i1 + 1];
                    count += 1.0;

                    if (next != chr)
                    {
                        newStr.Append(new string(chr, (int)Math.Max(0.0, (count * factor) + offset)));
                        count = 0.0;
                    }
                }
                else
                {
                    newStr.Append(str[i1]);
                    count = 0.0;
                }
            }

            return newStr.ToString();
        }

        /// <summary>
        ///     Normalizes all line breaks in a string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The string with all its line breaks normalized.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Normalizing line breaks means that all <c> CRLF </c> and <c> LF </c> are replaced with the value of <see cref="Environment.NewLine" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string NormalizeLineBreaks (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            str = str.Replace("\r\n", "\n");
            str = str.Replace("\n", Environment.NewLine);
            return str;
        }

        /// <summary>
        ///     Removes all line breaks in a string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The string with all its line breaks removed.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string RemoveLineBreaks (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            str = str.Replace("\r\n", "\n");
            str = str.Replace("\n", string.Empty);
            return str;
        }

        /// <summary>
        ///     Repeats a character a specified number of times without a separator between each character.
        /// </summary>
        /// <param name="chr"> The character. </param>
        /// <param name="count"> The number of times the character is repeated. </param>
        /// <returns>
        ///     The resulting string with the repeated character.
        ///     If <paramref name="count" /> is zero, the resulting string has a length of zero.
        ///     If <paramref name="count" /> is one, the resulting string consists only of the specified character.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count" /> is less than zero. </exception>
        public static string Repeat (this char chr, int count)
        {
            return chr.Repeat(count, null);
        }

        /// <summary>
        ///     Repeats a character a specified number of times with a specified separator between each character.
        /// </summary>
        /// <param name="chr"> The character. </param>
        /// <param name="count"> The number of times the character is repeated. </param>
        /// <param name="separator"> The used separator. </param>
        /// <returns>
        ///     The resulting string with the repeated character.
        ///     If <paramref name="count" /> is zero, the resulting string has a length of zero.
        ///     If <paramref name="count" /> is one, the resulting string consists only of the specified character.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count" /> is less than zero. </exception>
        public static string Repeat (this char chr, int count, char separator)
        {
            return chr.Repeat(count, new string(separator, 1));
        }

        /// <summary>
        ///     Repeats a character a specified number of times with a specified separator between each character.
        /// </summary>
        /// <param name="chr"> The character. </param>
        /// <param name="count"> The number of times the character is repeated. </param>
        /// <param name="separator"> The used separator. Can be null or <see cref="string" />.<see cref="string.Empty" /> if no separator should be used. </param>
        /// <returns>
        ///     The resulting string with the repeated character.
        ///     If <paramref name="count" /> is zero, the resulting string has a length of zero.
        ///     If <paramref name="count" /> is one, the resulting string consists only of the specified character.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count" /> is less than zero. </exception>
        public static string Repeat (this char chr, int count, string separator)
        {
            return new string(chr, 1).Repeat(count, separator);
        }

        /// <summary>
        ///     Repeats a string a specified number of times without a separator between each string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="count"> The number of times the string is repeated. </param>
        /// <returns>
        ///     The resulting string with the repeated string.
        ///     If <paramref name="count" /> is zero, the resulting string has a length of zero.
        ///     If <paramref name="count" /> is one, the resulting string consists only of the specified string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count" /> is less than zero. </exception>
        public static string Repeat (this string str, int count)
        {
            return str.Repeat(count, null);
        }

        /// <summary>
        ///     Repeats a string a specified number of times with a specified separator between each string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="count"> The number of times the string is repeated. </param>
        /// <param name="separator"> The used separator. </param>
        /// <returns>
        ///     The resulting string with the repeated string.
        ///     If <paramref name="count" /> is zero, the resulting string has a length of zero.
        ///     If <paramref name="count" /> is one, the resulting string consists only of the specified string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count" /> is less than zero. </exception>
        public static string Repeat (this string str, int count, char separator)
        {
            return str.Repeat(count, new string(separator, 1));
        }

        /// <summary>
        ///     Repeats a string a specified number of times with a specified separator between each string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="count"> The number of times the string is repeated. </param>
        /// <param name="separator"> The used separator. Can be null or <see cref="string" />.<see cref="string.Empty" /> if no separator should be used. </param>
        /// <returns>
        ///     The resulting string with the repeated string.
        ///     If <paramref name="count" /> is zero, the resulting string has a length of zero.
        ///     If <paramref name="count" /> is one, the resulting string consists only of the specified string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="count" /> is less than zero. </exception>
        public static string Repeat (this string str, int count, string separator)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count == 0)
            {
                return string.Empty;
            }

            if (count == 1)
            {
                return str;
            }

            separator ??= string.Empty;

            StringBuilder sb = new StringBuilder((str.Length * count) + (separator.Length * (count - 1)) + 1);

            for (int i1 = 0; i1 < count; i1++)
            {
                if (i1 != 0)
                {
                    sb.Append(separator);
                }

                sb.Append(str);
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Searchs a specified token in a string and replaces its occurence with a specified replacement, but only if the last character in the string, before a found token begins, is not the same as the first character of the specified token.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="token"> The token to find and replace. </param>
        /// <param name="replacement"> The replacement for each found token. </param>
        /// <param name="comparisonType"> The string comparison used to find the token. </param>
        /// <returns>
        ///     The resulting string where each token is replaced by the replacement.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The replacement only happens if the character before a found token is not the same as the first character of the token.
        ///         For example, when using "|x" as token and "X" as replacement, the string "ab|xcd" becomes "abXcd" but the string "ab||xcd" remains unchanged.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" />, <paramref name="token" />, or <paramref name="replacement" /> is null. </exception>
        public static string ReplaceSingleStart (this string str, string token, string replacement, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (replacement == null)
            {
                throw new ArgumentNullException(nameof(replacement));
            }

            if (token.Length == 0)
            {
                return str;
            }

            int index = -1;

            while (true)
            {
                index++;

                if (index >= str.Length)
                {
                    break;
                }

                index = str.IndexOf(token, index, comparisonType);

                if (index == -1)
                {
                    break;
                }

                bool replace = true;

                if (index >= 1)
                {
                    if (str[index - 1] == token[0])
                    {
                        replace = false;
                    }
                }

                if (replace)
                {
                    str = str.Substring(0, index) + replacement + str.Substring(index + token.Length);
                }
            }

            return str;
        }

        /// <summary>
        ///     Splits a string into pieces at each of the specified characters, excluding the characters from the resulting pieces.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="options"> The splitting options. </param>
        /// <param name="separator"> Zero, one, or more characters at which the string is split. </param>
        /// <returns>
        ///     The array of string pieces.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string[] Split (this string str, StringSplitOptions options, params char[] separator)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return str.Split(separator, options);
        }

        /// <summary>
        ///     Splits a string into pieces at each of the specified strings, excluding the strings from the resulting pieces.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="separator"> Zero, one, or more strings at which the string is split. </param>
        /// <returns>
        ///     The array of string pieces.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string[] Split (this string str, params string[] separator)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return str.Split(separator, StringSplitOptions.None);
        }

        /// <summary>
        ///     Splits a string into pieces at each of the specified strings, excluding the strings from the resulting pieces.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="options"> The splitting options. </param>
        /// <param name="separator"> Zero, one, or more strings at which the string is split. </param>
        /// <returns>
        ///     The array of string pieces.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string[] Split (this string str, StringSplitOptions options, params string[] separator)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return str.Split(separator, options);
        }

        /// <summary>
        ///     Splits a string into an array of strings at each line break.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The array of strings containing each line of the original string as a separate element.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Both CRLF and LF or \r\n and \n respectively are considered line breaks.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string[] SplitLines (this string str)
        {
            return str.SplitLines(StringSplitOptions.None);
        }

        /// <summary>
        ///     Splits a string into an array of strings at each line break.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="options"> The splitting options. </param>
        /// <returns>
        ///     The array of strings containing each line of the original string as a separate element.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         Both CRLF and LF or \r\n and \n respectively are considered line breaks.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string[] SplitLines (this string str, StringSplitOptions options)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return str.Replace("\r\n", "\n")
                      .Split(options, '\n');
        }

        /// <summary>
        ///     Splits a string into pieces at positions determined by a specified predicate function.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="predicate"> The function which determines at which positions a string is splitted. </param>
        /// <returns>
        ///     The array of string pieces.
        ///     If the specified predicate function never indicates a split position (never returns true), the array has only one element, equal to <paramref name="str" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         For more details about how to use the predicate function, see <see cref="StringSplitPredicate" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> or <paramref name="predicate" /> is null. </exception>
        public static string[] SplitWhere (this string str, StringSplitPredicate predicate)
        {
            return str.SplitWhere(StringSplitOptions.None, predicate);
        }

        /// <summary>
        ///     Splits a string into pieces at positions determined by a specified predicate function.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="options"> The splitting options. </param>
        /// <param name="predicate"> The function which determines at which positions a string is splitted. </param>
        /// <returns>
        ///     The array of string pieces.
        ///     If the specified predicate function never indicates a split position (never returns true), the array has only one element, equal to <paramref name="str" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         For more details about how to use the predicate function, see <see cref="StringSplitPredicate" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> or <paramref name="predicate" /> is null. </exception>
        public static string[] SplitWhere (this string str, StringSplitOptions options, StringSplitPredicate predicate)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            bool removeEmptyEntries = (options & StringSplitOptions.RemoveEmptyEntries) == StringSplitOptions.RemoveEmptyEntries;

            List<string> pieces = new List<string>();
            StringBuilder piece = new StringBuilder();

            for (int i1 = 0; i1 <= str.Length; i1++)
            {
                string currentPiece = piece.ToString();

                if (predicate(str, currentPiece, i1 - 1, i1))
                {
                    if (!removeEmptyEntries)
                    {
                        pieces.Add(currentPiece);
                    }

                    piece = new StringBuilder();
                }

                if (i1 <= (str.Length - 1))
                {
                    piece.Append(str[i1]);
                }
            }

            if (!removeEmptyEntries)
            {
                pieces.Add(piece.ToString());
            }

            return pieces.ToArray();
        }

        /// <summary>
        ///     Splits a string into pieces before each upper case character and on whitespaces.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The array of string pieces.
        ///     If the string does not contain any upper case characters except its first character, the array has only one element, equal to <paramref name="str" />.
        /// </returns>
        public static string[] SplitWords (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            return str.SplitWhere(StringSplitOptions.None, (s, currentToken, previous, next) =>
            {
                if ((previous < 0) || (next >= s.Length))
                {
                    return false;
                }

                char previousChar = s[previous];
                char nextChar = s[next];

                return (char.IsWhiteSpace(previousChar) && !char.IsWhiteSpace(nextChar)) || (char.IsWhiteSpace(nextChar) && !char.IsWhiteSpace(previousChar)) || (((char.IsLetter(previousChar) && char.IsLower(previousChar)) || !char.IsLetterOrDigit(previousChar)) && char.IsLetter(nextChar) && char.IsUpper(nextChar));
            });
        }

        /// <summary>
        ///     Counts how many times a string starts with a specified character.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="value"> The character to count when occuring at the start of the string. </param>
        /// <param name="comparisonType"> The string comparison used to find the character. </param>
        /// <returns>
        ///     The number of times the specified character appears in succession at the start of the string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static int StartsWithCount (this string str, char value, StringComparison comparisonType)
        {
            return str.StartsWithCount(new string(value, 1), comparisonType);
        }

        /// <summary>
        ///     Counts how many times a string starts with a specified string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="value"> The string to count when occuring at the start of the string. </param>
        /// <param name="comparisonType"> The string comparison used to find the string. </param>
        /// <returns>
        ///     The number of times the specified string appears in succession at the start of the string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> or <paramref name="value" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="value" /> is a string with zero length. </exception>
        public static int StartsWithCount (this string str, string value, StringComparison comparisonType)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (value.Length == 0)
            {
                throw new EmptyStringArgumentException(nameof(value));
            }

            int count = 0;
            int index = 0;

            while (true)
            {
                if ((index + value.Length) > str.Length)
                {
                    break;
                }

                string comparedPiece = str.Substring(index, value.Length);

                if (string.Equals(value, comparedPiece, comparisonType))
                {
                    count++;
                    index += value.Length;
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        /// <summary>
        ///     Attempts to convert a string into a boolean.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The boolean value represented by the string (true or false) if the string can be converted into a boolean, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The following strings will return true: &quot;true&quot;, &quot;yes&quot;, &quot;1&quot;, &quot;on&quot;.
        ///     </para>
        ///     <para>
        ///         The following strings will return false: &quot;false&quot;, &quot;no&quot;, &quot;0&quot;, &quot;off&quot;.
        ///     </para>
        ///     <para>
        ///         Any other string will return null.
        ///     </para>
        ///     <para>
        ///         The conversion is case-insensitive. Whitespace is not ignored and must be trimmed before if necessary (e.g. using <see cref="string.Trim()" />).
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static bool? ToBoolean (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            str = str.ToLowerInvariant();

            if (bool.TryParse(str, out bool value))
            {
                return value;
            }

            foreach (string booleanFalseValue in StringExtensions.BooleanFalseValues)
            {
                if (string.Equals(booleanFalseValue, str, StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }

            foreach (string booleanTrueValue in StringExtensions.BooleanTrueValues)
            {
                if (string.Equals(booleanTrueValue, str, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned byte value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The unsigned byte value represented by the string if the string can be converted into an unsigned byte, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.CurrentCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static byte? ToByte (this string str)
        {
            return str.ToByte(NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned byte value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="style"> The number styles which are to be expected in the string. </param>
        /// <param name="provider"> An object that supplies culture-specific formatting information for parsing the string. Can be null to use the current threads culture. </param>
        /// <returns>
        ///     The unsigned byte value represented by the string if the string can be converted into an unsigned byte, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static byte? ToByte (this string str, NumberStyles style, IFormatProvider provider)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (byte.TryParse(str, style, provider, out byte value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned byte value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The unsigned byte value represented by the string if the string can be converted into an unsigned byte, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.InvariantCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static byte? ToByteInvariant (this string str)
        {
            return str.ToByte(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into a date and time from an ISO8601 round-trip compatible string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The date and time represented by the string if the string is a date and time in the ISO8601 format, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static DateTime? ToDateTimeFromIso8601 (this string str)
        {
            if (DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime timestamp))
            {
                return timestamp;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into a date and time.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The date and time represented by the string if the string is a date and time as produced by <see cref="DateTimeExtensions.ToSortableString(System.DateTime)" />, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static DateTime? ToDateTimeFromSortable (this string str)
        {
            return str.ToDateTimeFromSortable(null);
        }

        /// <summary>
        ///     Attempts to convert a string into a date and time.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="separator"> The expected separator between each unit of the date and time. </param>
        /// <returns>
        ///     The date and time represented by the string if the string is a date and time as produced by <see cref="DateTimeExtensions.ToSortableString(DateTime,char)" />, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static DateTime? ToDateTimeFromSortable (this string str, char separator)
        {
            return str.ToDateTimeFromSortable(new string(separator, 1));
        }

        /// <summary>
        ///     Attempts to convert a string into a date and time.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="separator"> The expected separator between each unit of the date and time. </param>
        /// <returns>
        ///     The date and time represented by the string if the string is a date and time as produced by <see cref="DateTimeExtensions.ToSortableString(DateTime,string)" />, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static DateTime? ToDateTimeFromSortable (this string str, string separator)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            str = str.Trim();

            StringBuilder format = new StringBuilder(35);

            format.Append("yyyy");

            if (separator != null)
            {
                format.Append("'");
                format.Append(separator);
                format.Append("'");
            }

            format.Append("MM");

            if (separator != null)
            {
                format.Append("'");
                format.Append(separator);
                format.Append("'");
            }

            format.Append("dd");

            if (separator != null)
            {
                format.Append("'");
                format.Append(separator);
                format.Append("'");
            }

            format.Append("HH");

            if (separator != null)
            {
                format.Append("'");
                format.Append(separator);
                format.Append("'");
            }

            format.Append("mm");

            if (separator != null)
            {
                format.Append("'");
                format.Append(separator);
                format.Append("'");
            }

            format.Append("ss");

            if (separator != null)
            {
                format.Append("'");
                format.Append(separator);
                format.Append("'");
            }

            format.Append("fff");

            if (DateTime.TryParseExact(str, format.ToString(), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timestamp))
            {
                return timestamp;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into a date and time from an ISO8601 round-trip compatible string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The date and time represented by the string if the string is a date and time in the ISO8601 format, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static DateTimeOffset? ToDateTimeOffsetFromIso8601 (this string str)
        {
            if (DateTimeOffset.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTimeOffset timestamp))
            {
                return timestamp;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into a decimal floating point value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The decimal floating point value represented by the string if the string can be converted into a decimal floating point, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.CurrentCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static decimal? ToDecimal (this string str)
        {
            return str.ToDecimal(NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into a decimal floating point value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="style"> The number styles which are to be expected in the string. </param>
        /// <param name="provider"> An object that supplies culture-specific formatting information for parsing the string. Can be null to use the current threads culture. </param>
        /// <returns>
        ///     The decimal floating point value represented by the string if the string can be converted into a decimal floating point, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static decimal? ToDecimal (this string str, NumberStyles style, IFormatProvider provider)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (decimal.TryParse(str, style, provider, out decimal value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into a decimal floating point value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The decimal floating point value represented by the string if the string can be converted into a decimal floating point, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.InvariantCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static decimal? ToDecimalInvariant (this string str)
        {
            return str.ToDecimal(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into double precision floating point value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The double precision floating point value represented by the string if the string can be converted into a floating point, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.CurrentCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static double? ToDouble (this string str)
        {
            return str.ToDouble(NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into double precision floating point value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="style"> The number styles which are to be expected in the string. </param>
        /// <param name="provider"> An object that supplies culture-specific formatting information for parsing the string. Can be null to use the current threads culture. </param>
        /// <returns>
        ///     The double precision floating point value represented by the string if the string can be converted into a floating point, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static double? ToDouble (this string str, NumberStyles style, IFormatProvider provider)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (double.TryParse(str, style, provider, out double value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into double precision floating point value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The double precision floating point value represented by the string if the string can be converted into a floating point, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.InvariantCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static double? ToDoubleInvariant (this string str)
        {
            return str.ToDouble(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Returns an empty string if a string is null or empty.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     <see cref="string.Empty" /> if the string is null or empty, the original string otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A string is considered empty if has a length of zero.
        ///     </para>
        /// </remarks>
        public static string ToEmptyIfNullOrEmpty (this string str)
        {
            return str.IsNullOrEmpty() ? string.Empty : str;
        }

        /// <summary>
        ///     Returns an empty string if a string is null, empty, or consists only of whitespaces.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     <see cref="string.Empty" /> if the string is null, empty, or consists only of whitespaces, the original string otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A string is considered empty if has a length of zero.
        ///     </para>
        ///     <para>
        ///         A string is considered consisting only of whitespaces if it is not empty and only has whitespace characters.
        ///     </para>
        /// </remarks>
        public static string ToEmptyIfNullOrEmptyOrWhitespace (this string str)
        {
            return str.IsNullOrEmptyOrWhitespace() ? string.Empty : str;
        }

        /// <summary>
        ///     Attempts to convert a string into a specified enumeration type.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="enumType"> The enumeration type. </param>
        /// <returns>
        ///     The enumeration value represented by the string if the string can be converted into the specified enumeration type, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         This method is considered very slow.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> or <paramref name="enumType" /> is null. </exception>
        /// <exception cref="NotAnEnumerationArgumentException"> <paramref name="enumType" /> is not an enumeration type. </exception>
        public static object ToEnum (this string str, Type enumType)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (enumType == null)
            {
                throw new ArgumentNullException(nameof(enumType));
            }

            if (!enumType.IsEnum)
            {
                throw new NotAnEnumerationArgumentException(nameof(enumType));
            }

            try
            {
                return Enum.Parse(enumType, str, true);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Attempts to convert a string into a specified enumeration type.
        /// </summary>
        /// <typeparam name="T"> The enumeration type. </typeparam>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The enumeration value represented by the string if the string can be converted into the specified enumeration type, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         This method is considered very slow.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        /// <exception cref="NotAnEnumerationArgumentException"> <typeparamref name="T" /> is not an enumeration type. </exception>
        public static T? ToEnum <T> (this string str)
            where T : struct
        {
            return (T?)str.ToEnum(typeof(T));
        }

        /// <summary>
        ///     Attempts to convert a string into single precision floating point value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The single precision floating point value represented by the string if the string can be converted into a floating point, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.CurrentCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static float? ToFloat (this string str)
        {
            return str.ToFloat(NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into single precision floating point value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="style"> The number styles which are to be expected in the string. </param>
        /// <param name="provider"> An object that supplies culture-specific formatting information for parsing the string. Can be null to use the current threads culture. </param>
        /// <returns>
        ///     The single precision floating point value represented by the string if the string can be converted into a floating point, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static float? ToFloat (this string str, NumberStyles style, IFormatProvider provider)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (float.TryParse(str, style, provider, out float value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into single precision floating point value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The single precision floating point value represented by the string if the string can be converted into a floating point, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.InvariantCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static float? ToFloatInvariant (this string str)
        {
            return str.ToFloat(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into a <see cref="Guid" /> value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The <see cref="Guid" /> value represented by the string if the string can be converted into a <see cref="Guid" />, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         This method is considered very slow.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static Guid? ToGuid (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            try
            {
                return new Guid(str);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Attempts to convert a string into a signed short.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The signed short value represented by the string if the string can be converted into a signed short, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.CurrentCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static short? ToInt16 (this string str)
        {
            return str.ToInt16(NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into a signed short.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="style"> The number styles which are to be expected in the string. </param>
        /// <param name="provider"> An object that supplies culture-specific formatting information for parsing the string. Can be null to use the current threads culture. </param>
        /// <returns>
        ///     The signed short value represented by the string if the string can be converted into a signed short, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static short? ToInt16 (this string str, NumberStyles style, IFormatProvider provider)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (short.TryParse(str, style, provider, out short value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into a signed short.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The signed short value represented by the string if the string can be converted into a signed short, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.InvariantCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static short? ToInt16Invariant (this string str)
        {
            return str.ToInt16(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into a signed int.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The signed int value represented by the string if the string can be converted into a signed int, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.CurrentCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static int? ToInt32 (this string str)
        {
            return str.ToInt32(NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into a signed int.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="style"> The number styles which are to be expected in the string. </param>
        /// <param name="provider"> An object that supplies culture-specific formatting information for parsing the string. Can be null to use the current threads culture. </param>
        /// <returns>
        ///     The signed int value represented by the string if the string can be converted into a signed int, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static int? ToInt32 (this string str, NumberStyles style, IFormatProvider provider)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (int.TryParse(str, style, provider, out int value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into a signed int.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The signed int value represented by the string if the string can be converted into a signed int, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.InvariantCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static int? ToInt32Invariant (this string str)
        {
            return str.ToInt32(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into a signed long.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The signed long value represented by the string if the string can be converted into a signed long, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.CurrentCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static long? ToInt64 (this string str)
        {
            return str.ToInt64(NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into a signed long.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="style"> The number styles which are to be expected in the string. </param>
        /// <param name="provider"> An object that supplies culture-specific formatting information for parsing the string. Can be null to use the current threads culture. </param>
        /// <returns>
        ///     The signed long value represented by the string if the string can be converted into a signed long, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static long? ToInt64 (this string str, NumberStyles style, IFormatProvider provider)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (long.TryParse(str, style, provider, out long value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into a signed long.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The signed long value represented by the string if the string can be converted into a signed long, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.InvariantCulture" /> are used for parsing.
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static long? ToInt64Invariant (this string str)
        {
            return str.ToInt64(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Returns null if a string is null or empty.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     null if the string is null or empty, the original string otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A string is considered empty if has a length of zero.
        ///     </para>
        /// </remarks>
        public static string ToNullIfNullOrEmpty (this string str)
        {
            return str.IsNullOrEmpty() ? null : str;
        }

        /// <summary>
        ///     Returns null if a string is null, empty, or consists only of whitespaces.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     null if the string is null, empty, or consists only of whitespaces, the original string otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         A string is considered empty if has a length of zero.
        ///     </para>
        ///     <para>
        ///         A string is considered consisting only of whitespaces if it is not empty and only has whitespace characters.
        ///     </para>
        /// </remarks>
        public static string ToNullIfNullOrEmptyOrWhitespace (this string str)
        {
            return str.IsNullOrEmptyOrWhitespace() ? null : str;
        }

        /// <summary>
        ///     Attempts to convert a string into a roman number.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The roman number represented by the string if the string represents a valid roman number, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static RomanNumber? ToRomanNumber (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (RomanNumber.TryParse(str, out RomanNumber romanNumber))
            {
                return romanNumber;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into a signed byte value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The signed byte value represented by the string if the string can be converted into a signed byte, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.CurrentCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static sbyte? ToSByte (this string str)
        {
            return str.ToSByte(NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into a signed byte value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="style"> The number styles which are to be expected in the string. </param>
        /// <param name="provider"> An object that supplies culture-specific formatting information for parsing the string. Can be null to use the current threads culture. </param>
        /// <returns>
        ///     The signed byte value represented by the string if the string can be converted into a signed byte, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static sbyte? ToSByte (this string str, NumberStyles style, IFormatProvider provider)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (sbyte.TryParse(str, style, provider, out sbyte value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into a signed byte value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The signed byte value represented by the string if the string can be converted into a signed byte, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.InvariantCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static sbyte? ToSByteInvariant (this string str)
        {
            return str.ToSByte(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Converts a string into a secure string.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The secure string.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static SecureString ToSecureString (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            SecureString secureString = new SecureString();

            foreach (char chr in str)
            {
                secureString.AppendChar(chr);
            }

            return secureString;
        }

        /// <summary>
        ///     Transforms a string into a &quot;technical string&quot;.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The technical string.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         See <see cref="ToTechnical(string,TechnicalStringOptions,char?)" /> for more details.
        ///     </para>
        ///     <para>
        ///         <see cref="TechnicalStringOptions.None" /> is used for options.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string ToTechnical (this string str)
        {
            return str.ToTechnical(TechnicalStringOptions.None, null);
        }

        /// <summary>
        ///     Transforms a string into a &quot;technical string&quot;.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="options"> The technical string options. </param>
        /// <returns>
        ///     The technical string.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         See <see cref="ToTechnical(string,TechnicalStringOptions,char?)" /> for more details.
        ///     </para>
        ///     <para>
        ///         Whitespace is preserved and not replaced with a replacement character if <paramref name="options" /> specifies <see cref="TechnicalStringOptions.AllowWhitespaces" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string ToTechnical (this string str, TechnicalStringOptions options)
        {
            return str.ToTechnical(options, null);
        }

        /// <summary>
        ///     Transforms a string into a &quot;technical string&quot;.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="options"> The technical string options. </param>
        /// <param name="whitespaceReplacement"> The replacement character for whitespaces or null if the whitespace is to be preserved. </param>
        /// <returns>
        ///     The technical string.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The transformation of a string into a technical string is done by removing characters which can not be used for technical purposes.
        ///     </para>
        ///     <para>
        ///         A technical string is a string which can be used for technical purposes such as file names, IDs, etc. and therefore consists only of certain characters.
        ///         By default, if <paramref name="options" /> is <see cref="TechnicalStringOptions.None" />, only letters and digits will be preserved.
        ///         <paramref name="options" /> can specify additional characters which will be preserved.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string ToTechnical (this string str, TechnicalStringOptions options, char? whitespaceReplacement)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            bool allowWhitespaces = (options & TechnicalStringOptions.AllowWhitespaces) == TechnicalStringOptions.AllowWhitespaces;
            bool allowUnderscores = (options & TechnicalStringOptions.AllowUnderscores) == TechnicalStringOptions.AllowUnderscores;
            bool allowMinues = (options & TechnicalStringOptions.AllowMinus) == TechnicalStringOptions.AllowMinus;
            bool allowPeriodes = (options & TechnicalStringOptions.AllowPeriods) == TechnicalStringOptions.AllowPeriods;

            StringBuilder strBuilder = new StringBuilder(str.Length);

            for (int i1 = 0; i1 < str.Length; i1++)
            {
                char chr = str[i1];

                if (char.IsLetterOrDigit(chr))
                {
                    strBuilder.Append(chr);
                }
                else if (allowWhitespaces && char.IsWhiteSpace(chr))
                {
                    if (whitespaceReplacement.HasValue)
                    {
                        strBuilder.Append(whitespaceReplacement);
                    }
                    else
                    {
                        strBuilder.Append(chr);
                    }
                }
                else if (allowUnderscores && (chr == '_'))
                {
                    strBuilder.Append(chr);
                }
                else if (allowMinues && (chr == '-'))
                {
                    strBuilder.Append(chr);
                }
                else if (allowPeriodes && (chr == '.'))
                {
                    strBuilder.Append(chr);
                }
            }

            return strBuilder.ToString();
        }

        /// <summary>
        ///     Attempts to convert a string into a time span.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The time span represented by the string if the string is a time span as produced by <see cref="TimeSpanExtensions.ToSortableString(TimeSpan)" />, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static TimeSpan? ToTimeSpanFromSortable (this string str)
        {
            return str.ToTimeSpanFromSortable(null);
        }

        /// <summary>
        ///     Attempts to convert a string into a time span.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="separator"> The expected separator between each unit of the time span. </param>
        /// <returns>
        ///     The time span represented by the string if the string is a time span as produced by <see cref="TimeSpanExtensions.ToSortableString(TimeSpan,char)" />, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static TimeSpan? ToTimeSpanFromSortable (this string str, char separator)
        {
            return str.ToTimeSpanFromSortable(new string(separator, 1));
        }

        /// <summary>
        ///     Attempts to convert a string into a time span.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="separator"> The expected separator between each unit of the time span. </param>
        /// <returns>
        ///     The time span represented by the string if the string is a time span as produced by <see cref="TimeSpanExtensions.ToSortableString(TimeSpan,string)" />, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static TimeSpan? ToTimeSpanFromSortable (this string str, string separator)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            int separatorLength = separator?.Length ?? 0;

            str = str.Trim();

            if (str.Length < (10 + (separatorLength * 4)))
            {
                return null;
            }

            int daysLength = str.Length - (9 + (separatorLength * 4));
            int hoursIndex = daysLength + separatorLength;
            int minutesIndex = hoursIndex + 2 + separatorLength;
            int secondsIndex = minutesIndex + 2 + separatorLength;
            int millisecondsIndex = secondsIndex + 2 + separatorLength;

            int? days = str.Substring(0, daysLength)
                           .ToInt32(NumberStyles.Any, CultureInfo.InvariantCulture);

            int? hours = str.Substring(hoursIndex, 2)
                            .ToInt32(NumberStyles.Any, CultureInfo.InvariantCulture);

            int? minutes = str.Substring(minutesIndex, 2)
                              .ToInt32(NumberStyles.Any, CultureInfo.InvariantCulture);

            int? seconds = str.Substring(secondsIndex, 2)
                              .ToInt32(NumberStyles.Any, CultureInfo.InvariantCulture);

            int? milliseconds = str.Substring(millisecondsIndex, 3)
                                   .ToInt32(NumberStyles.Any, CultureInfo.InvariantCulture);

            if ((days == null) || (hours == null) || (minutes == null) || (seconds == null) || (milliseconds == null))
            {
                return null;
            }

            if ((hours < 0) || (hours > 23) || (minutes < 0) || (minutes > 59) || (seconds < 0) || (seconds > 59) || (milliseconds < 0) || (milliseconds > 999))
            {
                return null;
            }

            TimeSpan timeSpan = new TimeSpan(Math.Abs(days.Value), hours.Value, minutes.Value, seconds.Value, milliseconds.Value);

            if (days < 0)
            {
                timeSpan = new TimeSpan(timeSpan.Ticks * -1);
            }

            return timeSpan;
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned short.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The unsigned short value represented by the string if the string can be converted into an unsigned short, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.CurrentCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static ushort? ToUInt16 (this string str)
        {
            return str.ToUInt16(NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned short.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="style"> The number styles which are to be expected in the string. </param>
        /// <param name="provider"> An object that supplies culture-specific formatting information for parsing the string. Can be null to use the current threads culture. </param>
        /// <returns>
        ///     The unsigned short value represented by the string if the string can be converted into an unsigned short, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static ushort? ToUInt16 (this string str, NumberStyles style, IFormatProvider provider)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (ushort.TryParse(str, style, provider, out ushort value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned short.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The unsigned short value represented by the string if the string can be converted into an unsigned short, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.InvariantCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static ushort? ToUInt16Invariant (this string str)
        {
            return str.ToUInt16(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned int.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The unsigned int value represented by the string if the string can be converted into an unsigned int, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.CurrentCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static uint? ToUInt32 (this string str)
        {
            return str.ToUInt32(NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned int.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="style"> The number styles which are to be expected in the string. </param>
        /// <param name="provider"> An object that supplies culture-specific formatting information for parsing the string. Can be null to use the current threads culture. </param>
        /// <returns>
        ///     The unsigned int value represented by the string if the string can be converted into an unsigned int, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static uint? ToUInt32 (this string str, NumberStyles style, IFormatProvider provider)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (uint.TryParse(str, style, provider, out uint value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned int.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The unsigned int value represented by the string if the string can be converted into an unsigned int, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.InvariantCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static uint? ToUInt32Invariant (this string str)
        {
            return str.ToUInt32(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned long.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The unsigned long value represented by the string if the string can be converted into an unsigned long, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.CurrentCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static ulong? ToUInt64 (this string str)
        {
            return str.ToUInt64(NumberStyles.Any, CultureInfo.CurrentCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned long.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="style"> The number styles which are to be expected in the string. </param>
        /// <param name="provider"> An object that supplies culture-specific formatting information for parsing the string. Can be null to use the current threads culture. </param>
        /// <returns>
        ///     The unsigned long value represented by the string if the string can be converted into an unsigned long, null otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static ulong? ToUInt64 (this string str, NumberStyles style, IFormatProvider provider)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (ulong.TryParse(str, style, provider, out ulong value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        ///     Attempts to convert a string into an unsigned long.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The unsigned long value represented by the string if the string can be converted into an unsigned long, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="NumberStyles" />.<see cref="NumberStyles.Any" /> and <see cref="CultureInfo" />.<see cref="CultureInfo.InvariantCulture" /> are used for parsing.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        [CLSCompliant(false),]
        public static ulong? ToUInt64Invariant (this string str)
        {
            return str.ToUInt64(NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Attempts to convert a string into a <see cref="Version" /> value.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <returns>
        ///     The <see cref="Version" /> value represented by the string if the string can be converted into a <see cref="Version" />, null otherwise.
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         This method is considered very slow.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static Version ToVersion (this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            try
            {
                return new Version(str);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Converts a string into another string where escape sequences are converted back to certain special characters.
        /// </summary>
        /// <param name="str"> The string. </param>
        /// <param name="options"> The conversion options. </param>
        /// <returns>
        ///     The resulting string with escape sequences converted back to special characters.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         An escape sequence always starts with \ followed by a single character specifying the escape sequence, e.g. \n for new-line.
        ///     </para>
        ///     <para>
        ///         The following special characters are un-escaped: \a, \b, \f, \n, \r, \t, \v, \, ', ".
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="str" /> is null. </exception>
        public static string Unescape (this string str, StringEscapeOptions options)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (str.Length == 0)
            {
                return string.Empty;
            }

            StringBuilder sb = new StringBuilder(str.Length);

            for (int i1 = 0; i1 < str.Length; i1++)
            {
                char current = str[i1];

                if ((current == '\\') && (i1 < (str.Length - 1)))
                {
                    char next = str[i1 + 1];
                    char? replacement = null;

                    if ((next == 'a') && ((options & StringEscapeOptions.Alert) == StringEscapeOptions.Alert))
                    {
                        replacement = '\a';
                    }
                    else if ((next == 'b') && ((options & StringEscapeOptions.Backspace) == StringEscapeOptions.Backspace))
                    {
                        replacement = '\b';
                    }
                    else if ((next == 'f') && ((options & StringEscapeOptions.Formfeed) == StringEscapeOptions.Formfeed))
                    {
                        replacement = '\f';
                    }
                    else if ((next == 'n') && ((options & StringEscapeOptions.Newline) == StringEscapeOptions.Newline))
                    {
                        replacement = '\n';
                    }
                    else if ((next == 'r') && ((options & StringEscapeOptions.CarriageReturn) == StringEscapeOptions.CarriageReturn))
                    {
                        replacement = '\r';
                    }
                    else if ((next == 't') && ((options & StringEscapeOptions.HorizontalTab) == StringEscapeOptions.HorizontalTab))
                    {
                        replacement = '\t';
                    }
                    else if ((next == 'v') && ((options & StringEscapeOptions.VerticalTap) == StringEscapeOptions.VerticalTap))
                    {
                        replacement = '\v';
                    }
                    else if ((next == '\\') && ((options & StringEscapeOptions.Backslash) == StringEscapeOptions.Backslash))
                    {
                        replacement = '\\';
                    }
                    else if ((next == '\'') && ((options & StringEscapeOptions.SingleQuote) == StringEscapeOptions.SingleQuote))
                    {
                        replacement = '\'';
                    }
                    else if ((next == '\"') && ((options & StringEscapeOptions.DoubleQuote) == StringEscapeOptions.DoubleQuote))
                    {
                        replacement = '\"';
                    }
                    else if ((next == '?') && ((options & StringEscapeOptions.QuestionMark) == StringEscapeOptions.QuestionMark))
                    {
                        replacement = '?';
                    }

                    if (replacement.HasValue)
                    {
                        sb.Append(replacement.Value);
                        i1++;
                    }
                    else
                    {
                        sb.Append(current);
                    }
                }
                else
                {
                    sb.Append(current);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
