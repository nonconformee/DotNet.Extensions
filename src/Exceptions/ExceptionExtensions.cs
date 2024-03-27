using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using RI.Utilities.Text;




namespace RI.Utilities.Exceptions
{
    /// <summary>
    ///     Provides utility/extension methods for the <see cref="Exception" /> type.
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    public static class ExceptionExtensions
    {
        #region Constants

        private const string DefaultIndent = " ";

        private const string FieldPrefix = "* ";

        private const string FieldSeparator = " : ";

        private const string NullString = "[null]";

        private const string PropertyPrefix = "# ";

        private const string PropertySeparator = " : ";

        private const string StackTracePrefix = "-> ";

        private const string TargetSiteSeparator = ".";

        private static readonly string[] IgnoredPropertiesAndFields =
        {
            "Message",
            "Source",
            "TargetSite",
            "HelpLink",
            "StackTrace",
            "InnerException",
            "InnerExceptions",
            "WatsonBuckets",
        };

        #endregion




        #region Static Methods

        /// <summary>
        ///     Creates a detailed string representation of an exception.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <returns>
        ///     The detailed string representation of the exception.
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         The created string representation is not intented for serializing or storing an exception, it is only used for logging and debugging purposes.
        ///     </note>
        ///     <para>
        ///         A single space character is used as an indentation string for inner exceptions.
        ///     </para>
        /// </remarks>
        public static string ToDetailedString (this Exception exception)
        {
            return exception.ToDetailedString(null);
        }

        /// <summary>
        ///     Creates a detailed string representation of an exception.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <param name="indentString"> An indentation string which is used to indent inner exceptions in the string. </param>
        /// <returns>
        ///     The detailed string representation of the exception.
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         The created string representation is not intented for serializing or storing an exception, it is only used for logging and debugging purposes.
        ///     </note>
        /// </remarks>
        [SuppressMessage("ReSharper", "ConstantConditionalAccessQualifier"),]
        [SuppressMessage("ReSharper", "ConstantNullCoalescingCondition"),]
        public static string ToDetailedString (this Exception exception, string indentString)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            indentString = indentString ?? ExceptionExtensions.DefaultIndent;

            StringBuilder sb = new StringBuilder();

            using (StringWriter stringWriter = new StringWriter(sb, CultureInfo.InvariantCulture))
            {
                using (IndentedTextWriter writer = new IndentedTextWriter(stringWriter))
                {
                    writer.IndentEmptyLines = false;
                    writer.IndentLevel = 0;
                    writer.IndentString = indentString;

                    writer.Write("Message:     ");
                    writer.WriteLine(exception.Message.Trim());

                    writer.Write("Type:        ");

                    writer.WriteLine(exception.GetType()
                                              .AssemblyQualifiedName);

                    writer.Write("Source:      ");
                    writer.WriteLine(exception.Source == null ? ExceptionExtensions.NullString : exception.Source.Trim());

                    writer.Write("Target site: ");

                    if (exception.TargetSite == null)
                    {
                        writer.WriteLine(ExceptionExtensions.NullString);
                    }
                    else
                    {
                        writer.Write(exception.TargetSite?.DeclaringType?.AssemblyQualifiedName?.Trim() ?? ExceptionExtensions.NullString);
                        writer.Write(ExceptionExtensions.TargetSiteSeparator);
                        writer.WriteLine(exception.TargetSite?.Name?.Trim() ?? ExceptionExtensions.NullString);
                    }

                    writer.Write("Help link:   ");
                    writer.WriteLine(exception.HelpLink == null ? ExceptionExtensions.NullString : exception.HelpLink.Trim());

                    List<PropertyInfo> writtenProperties = new List<PropertyInfo>();

                    try
                    {
                        PropertyInfo[] properties = exception.GetType()
                                                             .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                        foreach (PropertyInfo property in properties)
                        {
                            try
                            {
                                string name = property.Name;

                                if (ExceptionExtensions.IgnoredPropertiesAndFields.Contains(name, StringComparer.InvariantCultureIgnoreCase))
                                {
                                    continue;
                                }

                                MethodInfo getter = property.GetGetMethod(true);
                                object propertyValue = getter.Invoke(exception, null);

                                string propertyType = property.PropertyType.Name;

                                string valueType = propertyValue?.GetType()
                                                                ?.Name;

                                string type = propertyType + (valueType == null ? string.Empty : "[" + valueType + "]");

                                string stringValue = ExceptionExtensions.GetStringFromValue(propertyValue, indentString);
                                string escapedStringValue = stringValue.Escape(StringEscapeOptions.NonPrintable);

                                writer.Write(ExceptionExtensions.PropertyPrefix);
                                writer.Write(name);
                                writer.Write(ExceptionExtensions.PropertySeparator);
                                writer.Write(type);
                                writer.Write(ExceptionExtensions.PropertySeparator);
                                writer.WriteLine(escapedStringValue);

                                writtenProperties.Add(property);
                            }
                            catch
                            {
                                writer.Write("(failure while printing exception properties; property: )");
                                writer.WriteLine(property.Name);
                            }
                        }
                    }
                    catch
                    {
                        writer.WriteLine("(failure while printing exception properties; general)");
                    }

                    try
                    {
                        FieldInfo[] fields = exception.GetType()
                                                      .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                        foreach (FieldInfo field in fields)
                        {
                            try
                            {
                                string name = field.Name;

                                if (ExceptionExtensions.IgnoredPropertiesAndFields.Contains(name, StringComparer.InvariantCultureIgnoreCase))
                                {
                                    continue;
                                }

                                if (writtenProperties.Any(x => name.Contains(x.Name, StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    continue;
                                }

                                object fieldValue = field.GetValue(exception);

                                string fieldType = field.FieldType.Name;

                                string valueType = fieldValue?.GetType()
                                                             ?.Name;

                                string type = fieldType + (valueType == null ? string.Empty : "[" + valueType + "]");

                                string stringValue = ExceptionExtensions.GetStringFromValue(fieldValue, indentString);
                                string escapedStringValue = stringValue.Escape(StringEscapeOptions.NonPrintable);

                                writer.Write(ExceptionExtensions.FieldPrefix);
                                writer.Write(name);
                                writer.Write(ExceptionExtensions.FieldSeparator);
                                writer.Write(type);
                                writer.Write(ExceptionExtensions.FieldSeparator);
                                writer.WriteLine(escapedStringValue);
                            }
                            catch
                            {
                                writer.Write("(failure while printing exception fields; field: )");
                                writer.WriteLine(field.Name);
                            }
                        }
                    }
                    catch
                    {
                        writer.WriteLine("(failure while printing exception fields; general)");
                    }

                    writer.Write("Stack trace:");

                    if (exception.StackTrace == null)
                    {
                        writer.Write(" ");
                        writer.WriteLine(ExceptionExtensions.NullString);
                    }
                    else
                    {
                        string[] lines = exception.StackTrace.SplitLines(StringSplitOptions.RemoveEmptyEntries);

                        if (lines.Length == 0)
                        {
                            writer.Write(" ");
                            writer.WriteLine(ExceptionExtensions.NullString);
                        }
                        else
                        {
                            writer.WriteLine();

                            for (int i1 = 0; i1 < lines.Length; i1++)
                            {
                                string line = lines[i1];
                                writer.Write(ExceptionExtensions.StackTracePrefix);
                                writer.WriteLine(line.Trim());
                            }
                        }
                    }

                    if (exception.InnerException != null)
                    {
                        writer.WriteLine("Inner exception:");
                        writer.IndentLevel++;
                        writer.WriteLine(exception.InnerException.ToDetailedString(indentString));
                        writer.IndentLevel--;
                    }

                    if (exception is AggregateException aggregateException)
                    {
                        foreach (Exception innerException in aggregateException.InnerExceptions)
                        {
                            writer.WriteLine("Inner exception (aggregated):");
                            writer.IndentLevel++;
                            writer.WriteLine(innerException.ToDetailedString(indentString));
                            writer.IndentLevel--;
                        }
                    }
                }
            }

            return sb.ToString()
                     .Trim();
        }

        /// <summary>
        ///     Creates a detailed string representation of an exception.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        /// <param name="indentChar"> An indentation character which is used to indent inner exceptions in the string. </param>
        /// <returns>
        ///     The detailed string representation of the exception.
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         The created string representation is not intented for serializing or storing an exception, it is only used for logging and debugging purposes.
        ///     </note>
        /// </remarks>
        public static string ToDetailedString (this Exception exception, char indentChar)
        {
            return exception.ToDetailedString(new string(indentChar, 1));
        }

        private static string GetStringFromValue (object value, string indentString)
        {
            if (value == null)
            {
                return ExceptionExtensions.NullString;
            }

            if (value is Exception)
            {
                return ((Exception)value).ToDetailedString(indentString);
            }

            if (value is string)
            {
                return (string)value;
            }

            if (value is byte[])
            {
                return "0x" + ((byte[])value).Select(x => x.ToString("x2", CultureInfo.InvariantCulture))
                                             .Join();
            }

            if (value is IDictionary)
            {
                return (from DictionaryEntry x in (IDictionary)value
                        select "[" + ExceptionExtensions.GetStringFromValue(x.Key, indentString) + "]=[" + ExceptionExtensions.GetStringFromValue(x.Value, indentString) + "]").Join(";");
            }

            if (value is IEnumerable)
            {
                return (from x in ((IEnumerable)value).Cast<object>()
                        select "[" + ExceptionExtensions.GetStringFromValue(x, indentString) + "]").Join(";");
            }

            return value.ToString();
        }

        #endregion
    }
}
