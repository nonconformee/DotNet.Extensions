using System;




namespace RI.Utilities.DataFormats.Ini
{
    /// <summary>
    ///     Describes an error which ocurred during reading INI data using an <see cref="IniReader" />.
    /// </summary>
    [Serializable,]
    public enum IniReaderError
    {
        /// <summary>
        ///     No error (no line read or the last line which was read is valid).
        /// </summary>
        None = 0,

        /// <summary>
        ///     The last line read is a section header containing an invalid section name.
        /// </summary>
        InvalidSectionName = 1,

        /// <summary>
        ///     The last line read is a name-value-pair with an invalid name.
        /// </summary>
        InvalidValueName = 2,
    }
}
