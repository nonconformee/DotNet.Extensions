using System;
using System.Globalization;
using System.Text;




namespace RI.Utilities.Dates
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="DateTime" /> and <see cref="DateTimeOffset" /> types.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class DateTimeExtensions
    {
        #region Static Methods

        /// <summary>
        ///     Converts a date and time to an ISO8601 round-trip compatible string.
        /// </summary>
        /// <param name="dateTime"> The date and time. </param>
        /// <returns>
        ///     The date and time as an ISO8601 round-trip compatible string.
        ///     Example: <c> 2016-02-01T14:30:50.3330000 </c>
        /// </returns>
        public static string ToIso8601String (this DateTime dateTime)
        {
            return dateTime.ToString("o", CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Converts a date and time with UTC offset to an ISO8601 round-trip compatible string.
        /// </summary>
        /// <param name="dateTimeOffset"> The date and time with UTC offset. </param>
        /// <returns>
        ///     The date and time with UTC offset as an ISO8601 round-trip compatible string.
        ///     Example: <c> 2016-02-01T14:30:50.3330000+02:00 </c>
        /// </returns>
        public static string ToIso8601String (this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("o", CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Converts a date and time into a sortable string.
        /// </summary>
        /// <param name="dateTime"> The date and time. </param>
        /// <returns>
        ///     The date and time as a sortable string in the following format yyyyMMddHHmmssfff with no separator between the units.
        ///     Example: <c> 20160201143050333 </c>
        /// </returns>
        public static string ToSortableString (this DateTime dateTime)
        {
            return dateTime.ToSortableString(null);
        }

        /// <summary>
        ///     Converts a date and time into a sortable string.
        /// </summary>
        /// <param name="dateTime"> The date and time. </param>
        /// <param name="separator"> The separator between each unit of the date and time. </param>
        /// <returns>
        ///     The date and time as a sortable string in the following format yyyy-MM-dd-HH-mm-ss-fff where the dash is the used separator.
        ///     Example: <c> 2016_02_01_14_30_50_333 </c> when used with the underscore as a separator.
        /// </returns>
        public static string ToSortableString (this DateTime dateTime, char separator)
        {
            return dateTime.ToSortableString(new string(separator, 1));
        }

        /// <summary>
        ///     Converts a date and time into a sortable string.
        /// </summary>
        /// <param name="dateTime"> The date and time. </param>
        /// <param name="separator"> The separator between each unit of the date and time. Can be null to use no separator. </param>
        /// <returns>
        ///     The date and time as a sortable string in the following format yyyy-MM-dd-HH-mm-ss-fff where the dash is the used separator.
        ///     Example: <c> 2016_02_01_14_30_50_333 </c> when used with the underscore as a separator.
        /// </returns>
        public static string ToSortableString (this DateTime dateTime, string separator)
        {
            separator = separator ?? string.Empty;

            StringBuilder dateTimeString = new StringBuilder(23);

            dateTimeString.Append(dateTime.ToString("yyyy", CultureInfo.InvariantCulture));
            dateTimeString.Append(separator);
            dateTimeString.Append(dateTime.ToString("MM", CultureInfo.InvariantCulture));
            dateTimeString.Append(separator);
            dateTimeString.Append(dateTime.ToString("dd", CultureInfo.InvariantCulture));
            dateTimeString.Append(separator);
            dateTimeString.Append(dateTime.ToString("HH", CultureInfo.InvariantCulture));
            dateTimeString.Append(separator);
            dateTimeString.Append(dateTime.ToString("mm", CultureInfo.InvariantCulture));
            dateTimeString.Append(separator);
            dateTimeString.Append(dateTime.ToString("ss", CultureInfo.InvariantCulture));
            dateTimeString.Append(separator);
            dateTimeString.Append(dateTime.ToString("fff", CultureInfo.InvariantCulture));

            return dateTimeString.ToString();
        }

        #endregion
    }
}
