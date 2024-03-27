using System;

using RI.Utilities.Exceptions;
using RI.Utilities.ObjectModel;
using RI.Utilities.Text;




namespace RI.Utilities.DataFormats.Ini.Elements
{
    /// <summary>
    ///     Represents a section header in INI data.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         See <see cref="IniDocument" /> for more general and detailed information about working with INI data.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    public sealed class SectionIniElement : IniElement, ICloneable<SectionIniElement>, ICloneable, ICopyable<SectionIniElement>
    {
        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="SectionIniElement" />.
        /// </summary>
        /// <param name="sectionName"> The section name. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="sectionName" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="sectionName" /> is an empty string. </exception>
        public SectionIniElement (string sectionName)
        {
            if (sectionName == null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (sectionName.IsEmptyOrWhitespace())
            {
                throw new EmptyStringArgumentException(nameof(sectionName));
            }

            this.SectionName = sectionName;
        }

        #endregion




        #region Instance Fields

        private string _sectionName;

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets or sets the section name.
        /// </summary>
        /// <value>
        ///     The section name.
        /// </value>
        /// <exception cref="ArgumentNullException"> <paramref name="value" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="value" /> is an empty string. </exception>
        public string SectionName
        {
            get => this._sectionName;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value.IsEmptyOrWhitespace())
                {
                    throw new EmptyStringArgumentException(nameof(value));
                }

                this._sectionName = value;
            }
        }

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override string ToString ()
        {
            return IniSettings.DefaultSectionStart + this.SectionName + IniSettings.DefaultSectionEnd;
        }

        #endregion




        #region Interface: ICloneable<SectionIniElement>

        /// <inheritdoc />
        public SectionIniElement Clone ()
        {
            return new SectionIniElement(this.SectionName);
        }

        /// <inheritdoc />
        object ICloneable.Clone ()
        {
            return this.Clone();
        }

        #endregion




        #region Interface: ICopyable<SectionIniElement>

        /// <inheritdoc />
        public void CopyTo (SectionIniElement other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            other.SectionName = this.SectionName;
        }

        #endregion
    }
}
