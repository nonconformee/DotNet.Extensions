using System;
using System.Collections.Generic;




namespace RI.Utilities.DataFormats.Ini
{
    /// <summary>
    ///     Defines how sections are added to an <see cref="IniDocument" /> using <see cref="IniDocument.AddSection(string,IniSectionAddMode,IDictionary{string,string})" /> and <see cref="IniDocument.AddSection(string,IniSectionAddMode,IDictionary{string,IList{string}})" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="AppendSame" />: If no other section with the same name exists, the behaviour is the same as <see cref="AppendEnd" />.
    ///     </para>
    ///     <para>
    ///         <see cref="MergeSame" />: If no other section with the same name exists, the behaviour is the same as <see cref="AppendEnd" />.
    ///     </para>
    /// </remarks>
    [Serializable,]
    public enum IniSectionAddMode
    {
        /// <summary>
        ///     Appends the section, including a new section header, at the end of the <see cref="IniDocument" />.
        /// </summary>
        AppendEnd = 0,

        /// <summary>
        ///     Appends the section, including a new section header, after the last section of the same name.
        /// </summary>
        AppendSame = 1,

        /// <summary>
        ///     Adds the elements of the section at the end of the first already existing section with the same name.
        /// </summary>
        MergeSame = 2,
    }
}
