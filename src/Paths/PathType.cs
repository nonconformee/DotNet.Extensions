using System;




namespace RI.Utilities.Paths
{
    /// <summary>
    ///     Specifies the type of a path.
    /// </summary>
    [Serializable,]
    public enum PathType
    {
        /// <summary>
        ///     The path is not a valid path.
        /// </summary>
        Invalid = 0,

        /// <summary>
        ///     The path is a Windows-style path (e.g. d:\data\file.ext).
        /// </summary>
        Windows = 1,

        /// <summary>
        ///     The path is a UNIX style path (e.g. /dev/null).
        /// </summary>
        Unix = 2,

        /// <summary>
        ///     The path is a UNC style path (e.g. \\server\share).
        /// </summary>
        Unc = 3,
    }
}
