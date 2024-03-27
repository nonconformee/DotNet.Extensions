using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using RI.Utilities.Collections;
using RI.Utilities.ObjectModel;




namespace RI.Utilities.DataFormats.Csv
{
    /// <summary>
    ///     Contains and manages structured CSV data.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         CSV data in a <see cref="CsvDocument" /> is stored in the <see cref="Data" /> property.
    ///         <see cref="Data" /> is always kept up to date and all actions performed on an <see cref="CsvDocument" /> directly read from or modify <see cref="Data" />.
    ///     </para>
    ///     <para>
    ///         <see cref="Data" /> is organized as a list containing lists of strings in a sequential order (e.g. as they would appear in a CSV file).
    ///         The outer list represent rows while the inner lists represent columns (all columns of one row per inner list).
    ///     </para>
    ///     <para>
    ///         <see cref="Data" /> can be modified arbitrarily by either using methods of <see cref="CsvDocument" /> or by modifying the list itself.
    ///         The list can contain or be modified to contain any sequence of strings.
    ///     </para>
    ///     <para>
    ///         <b> ANATOMY OF CSV DATA </b>
    ///     </para>
    ///     <para>
    ///         CSV data is organized in rows and columns where the data is usually used and organized row-by-row.
    ///         Each row can have an arbitrary length of columns, therefore the number of columns can vary from row to row.
    ///     </para>
    ///     <para>
    ///         There are zero, one, or more rows.
    ///         Empty data is interpreted as having no rows (and therefore no columns) while data consisting only of whitespaces is considered having one row with one column.
    ///     </para>
    ///     <para>
    ///         A row has zero, one, or more columns.
    ///         An empty line is interpreted as having no columns while a line consisting only of whitespaces is considered having one column.
    ///     </para>
    ///     <para>
    ///         A value is a specific column of a specific row (sometimes also called cells).
    ///     </para>
    ///     <para>
    ///         Values are sperated by a separator character (see <see cref="CsvSettings.Separator" />).
    ///     </para>
    ///     <para>
    ///         Rows are separated by carriage-returns (CR) and line-feeds (LF), depending on the environment.
    ///     </para>
    ///     <para>
    ///         <b> QUOTING </b>
    ///     </para>
    ///     <para>
    ///         A value are enclosed in quotes (<c> &quot; </c>) if the value itself consists of multiple lines or the value itslef contains quotes.
    ///     </para>
    ///     <para>
    ///         For multiline values, the carriage-return (CR) / line-feed (LF) does not represent a row separator.
    ///         It is merely the same logical CSV row which spans multiple textual rows where the quote is used to detect that the carriage-return/line-feed belongs to the value and is not a row separator (because it occurs inside a quoted block).
    ///     </para>
    ///     <para>
    ///         Each quote contained in a value is written using twice the quote character.
    ///         For example, the value <c> Test&quot;123 </c> is written as <c> &quot;Test&quot;&quot;123&quot; </c> and <c> &quot;&quot;Hello </c> is written as <c> &quot;&quot;&quot;&quot;&quot;Hello&quot; </c>.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    /// <example>
    ///     <code language="cs">
    /// <![CDATA[
    /// // create an empty CSV document
    /// var doc = new CsvDocument();
    /// 
    /// // load a CSV file
    /// doc.Load("data.csv");
    /// 
    /// // get column headers
    /// var headers = doc.Data[0];
    /// 
    /// // iterate through all rows, except the column headers
    /// for (int index = 1; index < doc.Data.Count; index++)
    /// {
    ///    ...
    /// }
    /// 
    /// // save back into the CSV file
    /// doc.Save("data.csv");
    /// ]]>
    /// </code>
    /// </example>
    public sealed class CsvDocument : ICloneable, ICloneable<CsvDocument>, ICopyable<CsvDocument>
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="CsvDocument" />.
        /// </summary>
        public CsvDocument ()
        {
            this.Data = new List<IList<string>>();
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the CSV data.
        /// </summary>
        /// <value>
        ///     The CSV data.
        /// </value>
        public IList<IList<string>> Data { get; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Converts this CSV document to a string.
        /// </summary>
        /// <returns>
        ///     The string of CSV data created from this CSV document.
        /// </returns>
        public string AsString ()
        {
            return this.AsString(null);
        }

        /// <summary>
        ///     Converts this CSV document to a string.
        /// </summary>
        /// <param name="settings"> The used CSV writer settings or null if default values should be used. </param>
        /// <returns>
        ///     The string of CSV data created from this CSV document.
        /// </returns>
        public string AsString (CsvWriterSettings settings)
        {
            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                using (CsvWriter cw = new CsvWriter(sw, settings))
                {
                    this.Save(cw);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        ///     Removes all CSV data from <see cref="Data" />.
        /// </summary>
        public void Clear ()
        {
            this.Data.Clear();
        }

        /// <summary>
        ///     Loads CSV data from an existing CSV reader.
        /// </summary>
        /// <param name="reader"> The CSV reader from which the data is loaded. </param>
        /// <remarks>
        ///     <para>
        ///         All existing CSV data will be discarded before the new data is loaded.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="reader" /> is null. </exception>
        /// <exception cref="CsvParsingException"> The CSV data read by <paramref name="reader" /> contains invalid data. </exception>
        public void Load (CsvReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            List<IList<string>> rows = new List<IList<string>>();

            while (reader.ReadNext())
            {
                if (reader.CurrentError != CsvReaderError.None)
                {
                    throw new CsvParsingException(reader.CurrentLineNumber, reader.CurrentError);
                }

                rows.Add(new List<string>(reader.CurrentRow));
            }

            this.Data.Clear();
            this.Data.AddRange(rows);
        }

        /// <summary>
        ///     Loads CSV data from a string.
        /// </summary>
        /// <param name="data"> The CSV data to load. </param>
        /// <remarks>
        ///     <para>
        ///         All existing CSV data will be discarded before the new ata is loaded.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="data" /> is null. </exception>
        /// <exception cref="CsvParsingException"> The CSV data read from <paramref name="data" /> contains invalid data. </exception>
        public void Load (string data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.Load(data, (CsvReaderSettings)null);
        }

        /// <summary>
        ///     Loads CSV data from a string.
        /// </summary>
        /// <param name="data"> The CSV data to load. </param>
        /// <param name="settings"> The used CSV reader settings or null if default values should be used. </param>
        /// <remarks>
        ///     <para>
        ///         All existing CSV data will be discarded before the new data is loaded.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="data" /> is null. </exception>
        /// <exception cref="CsvParsingException"> The CSV data read from <paramref name="data" /> contains invalid data. </exception>
        public void Load (string data, CsvReaderSettings settings)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            using (StringReader sr = new StringReader(data))
            {
                using (CsvReader cr = new CsvReader(sr, settings))
                {
                    this.Load(cr);
                }
            }
        }

        /// <summary>
        ///     Loads CSV data from an existing CSV file.
        /// </summary>
        /// <param name="file"> The path of the CSV file to load. </param>
        /// <param name="encoding"> The encoding for reading the CSV file. </param>
        /// <remarks>
        ///     <para>
        ///         All existing CSV data will be discarded before the new data is loaded.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="file" /> or <paramref name="encoding" /> is null. </exception>
        /// <exception cref="CsvParsingException"> The CSV data read from <paramref name="file" /> contains invalid data. </exception>
        public void Load (string file, Encoding encoding)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            this.Load(file, encoding, null);
        }

        /// <summary>
        ///     Loads CSV data from an existing CSV file.
        /// </summary>
        /// <param name="file"> The path of the CSV file to load. </param>
        /// <param name="encoding"> The encoding for reading the CSV file. </param>
        /// <param name="settings"> The used CSV reader settings or null if default values should be used. </param>
        /// <remarks>
        ///     <para>
        ///         All existing CSV data will be discarded before the new data is loaded.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="file" /> or <paramref name="encoding" /> is null. </exception>
        /// <exception cref="CsvParsingException"> The CSV data read from <paramref name="file" /> contains invalid data. </exception>
        public void Load (string file, Encoding encoding, CsvReaderSettings settings)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using (StreamReader sr = new StreamReader(file, encoding))
            {
                using (CsvReader cr = new CsvReader(sr, settings))
                {
                    this.Load(cr);
                }
            }
        }

        /// <summary>
        ///     Saves all CSV data of this CSV document to an existing CSV writer.
        /// </summary>
        /// <param name="writer"> The CSV writer to which the data is saved. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public void Save (CsvWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            foreach (IList<string> row in this.Data)
            {
                writer.WriteRow(row);
            }
        }

        /// <summary>
        ///     Saves all CSV data of this CSV document to an existing text writer.
        /// </summary>
        /// <param name="writer"> The text writer to which the data is saved. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public void Save (TextWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            this.Save(writer, null);
        }

        /// <summary>
        ///     Saves all CSV data of this CSV document to an existing text writer.
        /// </summary>
        /// <param name="writer"> The text writer to which the data is saved. </param>
        /// <param name="settings"> The used CSV writer settings or null if default values should be used. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="writer" /> is null. </exception>
        public void Save (TextWriter writer, CsvWriterSettings settings)
        {
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            using (CsvWriter cw = new CsvWriter(writer, settings))
            {
                this.Save(cw);
            }
        }

        /// <summary>
        ///     Saves all CSV data of this CSV document to a CSV file.
        /// </summary>
        /// <param name="file"> The path of the CSV file to save. </param>
        /// <param name="encoding"> The encoding for writing the CSV file. </param>
        /// <remarks>
        ///     <para>
        ///         The CSV file will be overwritten with the CSV data from this CSV document.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="file" /> or <paramref name="encoding" /> is null. </exception>
        public void Save (string file, Encoding encoding)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            this.Save(file, encoding, null);
        }

        /// <summary>
        ///     Saves all CSV data of this CSV document to a CSV file.
        /// </summary>
        /// <param name="file"> The path of the CSV file to save. </param>
        /// <param name="encoding"> The encoding for writing the CSV file. </param>
        /// <param name="settings"> The used CSV writer settings or null if default values should be used. </param>
        /// <remarks>
        ///     <para>
        ///         The CSV file will be overwritten with the CSV data from this CSV document.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="file" /> or <paramref name="encoding" /> is null. </exception>
        public void Save (string file, Encoding encoding, CsvWriterSettings settings)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding));
            }

            using (StreamWriter sw = new StreamWriter(file, false, encoding))
            {
                using (CsvWriter cw = new CsvWriter(sw, settings))
                {
                    this.Save(cw);
                }
            }
        }

        #endregion




        #region Interface: ICloneable

        /// <inheritdoc />
        object ICloneable.Clone ()
        {
            return this.Clone();
        }

        #endregion




        #region Interface: ICloneable<CsvDocument>

        /// <inheritdoc />
        public CsvDocument Clone ()
        {
            CsvDocument clone = new CsvDocument();
            clone.Load(this.AsString());
            return clone;
        }

        #endregion




        #region Interface: ICopyable<CsvDocument>

        /// <inheritdoc />
        public void CopyTo (CsvDocument other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            other.Data.AddRange(this.Data.Select(x => new List<string>(x)));
        }

        #endregion
    }
}
