using System;




namespace RI.Utilities.Paths
{
    /// <summary>
    ///     Specifies the error in an invalid path.
    /// </summary>
    [Serializable,]
    public enum PathError
    {
        /// <summary>
        ///     The path is valid.
        /// </summary>
        None = 0,

        /// <summary>
        ///     The path is empty or null.
        /// </summary>
        Empty = 1,

        /// <summary>
        ///     The path contains wildcards although they are not allowed.
        /// </summary>
        WildcardsNotAllowed = 2,

        /// <summary>
        ///     The path contains invalid characters.
        /// </summary>
        InvalidCharacter = 3,

        /// <summary>
        ///     The path contains an invalid directory separator (e.g. a forward slash in a Windows path).
        /// </summary>
        InvalidDirectorySeparator = 4,

        /// <summary>
        ///     The path contains two directory separators next to each other with no name in between.
        /// </summary>
        RepeatedDirectorySeparator = 5,

        /// <summary>
        ///     The type of the path is ambiguous as it cannot be clearly determined.
        /// </summary>
        AmbiguousType = 6,

        /// <summary>
        ///     The type of the path is not of the expected type.
        /// </summary>
        WrongType = 7,

        /// <summary>
        ///     The path contains parts with empty names (e.g. only whitespace between two directory separators).
        /// </summary>
        EmptyName = 8,

        /// <summary>
        ///     The path contains relative parts although they are not allowed.
        /// </summary>
        RelativesNotAllowed = 9,

        /// <summary>
        ///     The path contains relative parts which reference higher up than the root of the path (e.g. c:\data\..\..\dir).
        /// </summary>
        RelativeGoesBeyondRoot = 10,
    }
}
