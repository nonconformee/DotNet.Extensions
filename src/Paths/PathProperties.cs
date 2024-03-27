using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using RI.Utilities.Collections;
using RI.Utilities.Exceptions;
using RI.Utilities.ObjectModel;
using RI.Utilities.Text;




namespace RI.Utilities.Paths
{
    /// <summary>
    ///     Describes a path and its properties.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="PathProperties" /> analyzes a path and contains, besides the path itself, several properties which describe the characteristics of the path.
    ///         For invalid paths, an error description is provided (<see cref="Error" />).
    ///     </para>
    ///     <para>
    ///         <see cref="PathProperties" /> supports Windows, Unix, and UNC style paths.
    ///     </para>
    ///     <para>
    ///         <see cref="ToString(string)" /> supports the following format strings; <c> o </c>, <c> O </c>, <c> g </c>, <c> G </c>, null or empty string: The original path (<see cref="PathOriginal" />) / <c> n </c>, <c> N </c>: The normalized path (<see cref="PathNormalized" />) / <c> r </c>, <c> R </c>: The resolved path (<see cref="PathResolved" />).
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    [Serializable,]
    public sealed class PathProperties : ICloneable<PathProperties>, ICloneable, IEquatable<PathProperties>, IComparable<PathProperties>, IComparable, IFormattable
    {
        #region Constants

        /// <summary>
        ///     The separator character between a file name and its extension.
        /// </summary>
        public const char FileExtensionSeparator = '.';

        /// <summary>
        ///     The string which can be used as a relative directory name to indicate the same level or current directory.
        /// </summary>
        public const string RelativeSame = ".";

        /// <summary>
        ///     The string which can be used as a relative directory name to point one level upwards.
        /// </summary>
        public const string RelativeUp = "..";

        /// <summary>
        ///     The directory separator used in UNC style paths.
        /// </summary>
        public const char UncDirectorySeparator = '\\';

        /// <summary>
        ///     The directory separator used in Unix style paths.
        /// </summary>
        public const char UnixDirectorySeparator = '/';

        /// <summary>
        ///     The wildcard character which is a placeholder for zero, one, or more characters.
        /// </summary>
        public const char WildcardMore = '*';

        /// <summary>
        ///     The wildcard character which is a placeholder for exactly one characters.
        /// </summary>
        public const char WildcardOne = '?';

        /// <summary>
        ///     The directory separator used in Windows style paths.
        /// </summary>
        public const char WindowsDirectorySeparator = '\\';

        /// <summary>
        ///     The separator character between the drive and the path in a rooted Windows style path.
        /// </summary>
        public const char WindowsDriveSeparator = ':';

        /// <summary>
        ///     All invalid path characters.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         These characters are invalid for both directory and file paths.
        ///     </para>
        /// </remarks>
        public static readonly char[] InvalidPathCharacters =
        {
            '\0',
            '\"',
            '|',
            '<',
            '>',
            PathProperties.WildcardOne,
            PathProperties.WildcardMore,
            PathProperties.WindowsDirectorySeparator,
            PathProperties.UnixDirectorySeparator,
            PathProperties.UncDirectorySeparator,
            PathProperties.WindowsDriveSeparator,
        };

        #endregion




        #region Static Methods

        /// <summary>
        ///     Analyzes a path and returns the results as an instance of <see cref="PathProperties" />.
        /// </summary>
        /// <param name="path"> The path to analyze. </param>
        /// <returns>
        ///     The instance of <see cref="PathProperties" /> which describes the path.
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         Invalid paths will return a valid <see cref="PathProperties" /> instance with the <see cref="Error" /> property set accordingly.
        ///     </note>
        ///     <para>
        ///         Using this method, wildcards and relative paths are allowed and the type of the path is assumed to be of the same type as used on the current system.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="path" /> is null. </exception>
        public static PathProperties FromPath (string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return PathProperties.FromPath(path, true, true, PathProperties.GetPathType(path, false) ?? PathProperties.GetSystemType());
        }

        /// <summary>
        ///     Analyzes a path and returns the results as an instance of <see cref="PathProperties" />.
        /// </summary>
        /// <param name="path"> The path to analyze. </param>
        /// <param name="allowWildcards"> Specifies whether wildcards are allowed or not. </param>
        /// <param name="allowRelatives"> Specifies whether relative directory names are allowed or not. </param>
        /// <param name="assumedType"> Optionally specifies the type of the path which is assumed if the type cannot be clearly determined through analysis of <paramref name="path" />. </param>
        /// <returns>
        ///     The instance of <see cref="PathProperties" /> which describes the path.
        /// </returns>
        /// <remarks>
        ///     <note type="note">
        ///         Invalid paths will return a valid <see cref="PathProperties" /> instance with the <see cref="Error" /> property set accordingly.
        ///     </note>
        ///     <para>
        ///         If <paramref name="assumedType" /> is null and the path cannot be unambiguously determined from <paramref name="path" />, the path is considered invalid and <see cref="Error" /> is set to <see cref="PathError.AmbiguousType" />.
        ///         If <paramref name="assumedType" /> is not null and the path type determined through analysis of <paramref name="path" /> does not match with <paramref name="assumedType" />, the path is considered invalid and <see cref="Error" /> is set to <see cref="PathError.WrongType" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="path" /> is null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> <paramref name="assumedType" /> is <see cref="PathType.Invalid" />. </exception>
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute"),]
        [SuppressMessage("ReSharper", "PossibleNullReferenceException"),]
        public static PathProperties FromPath (string path, bool allowWildcards, bool allowRelatives, PathType? assumedType)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (assumedType.HasValue)
            {
                if (assumedType.Value == PathType.Invalid)
                {
                    throw new ArgumentOutOfRangeException(nameof(assumedType));
                }
            }

            PathType? type = null;
            PathError error = PathError.None;

            if (path.IsEmptyOrWhitespace())
            {
                type = PathType.Invalid;
                error = PathError.Empty;
            }

            int startIndex = 0;
            bool isRooted = false;

            if (type != PathType.Invalid)
            {
                PathType? typeCandidate = PathProperties.GetPathType(path, true);

                if (typeCandidate.HasValue)
                {
                    type = typeCandidate.Value;

                    switch (typeCandidate.Value)
                    {
                        case PathType.Unix:
                            isRooted = true;
                            startIndex = 1;
                            break;

                        case PathType.Windows:
                            isRooted = true;
                            startIndex = 2;
                            break;

                        case PathType.Unc:
                            isRooted = true;
                            startIndex = 2;
                            break;
                    }
                }
            }

            if (assumedType.HasValue && type.HasValue)
            {
                if ((type.Value != assumedType.Value) && (type.Value != PathType.Invalid))
                {
                    type = PathType.Invalid;
                    error = PathError.WrongType;
                }
            }

            bool hasWildcards = false;

            if (type != PathType.Invalid)
            {
                for (int i1 = startIndex; i1 < path.Length; i1++)
                {
                    char chr = path[i1];

                    if ((chr == PathProperties.WildcardOne) || (chr == PathProperties.WildcardMore))
                    {
                        hasWildcards = true;

                        if (allowWildcards)
                        {
                            continue;
                        }

                        type = PathType.Invalid;
                        error = PathError.WildcardsNotAllowed;
                        break;
                    }

                    if (chr == PathProperties.WindowsDriveSeparator)
                    {
                        type = PathType.Invalid;
                        error = PathError.InvalidCharacter;
                        break;
                    }

                    if (chr == PathProperties.UnixDirectorySeparator)
                    {
                        if (type.HasValue && (type != PathType.Unix))
                        {
                            type = PathType.Invalid;
                            error = PathError.InvalidDirectorySeparator;
                            break;
                        }

                        if ((path.Length >= (i1 + 2)) && (path[i1 + 1] == PathProperties.UnixDirectorySeparator))
                        {
                            type = PathType.Invalid;
                            error = PathError.RepeatedDirectorySeparator;
                            break;
                        }

                        type = PathType.Unix;
                        continue;
                    }

                    if ((chr == PathProperties.WindowsDirectorySeparator) || (chr == PathProperties.UncDirectorySeparator))
                    {
                        if (type.HasValue && (type != PathType.Windows) && (type != PathType.Unc))
                        {
                            type = PathType.Invalid;
                            error = PathError.InvalidDirectorySeparator;
                            break;
                        }

                        if ((path.Length >= (i1 + 2)) && ((path[i1 + 1] == PathProperties.WindowsDirectorySeparator) || (path[i1 + 1] == PathProperties.UncDirectorySeparator)))
                        {
                            type = PathType.Invalid;
                            error = PathError.RepeatedDirectorySeparator;
                            break;
                        }

                        if (assumedType.HasValue && ((assumedType.Value == PathType.Windows) || (assumedType.Value == PathType.Unc)))
                        {
                            type = assumedType.Value;
                        }
                        else if (type.HasValue && ((type.Value == PathType.Windows) || (type.Value == PathType.Unc)))
                        {
                            type = type.Value;
                        }
                        else
                        {
                            type = PathType.Windows;
                        }

                        continue;
                    }

                    if ((chr < (char)32) || PathProperties.InvalidPathCharacters.Contains(chr))
                    {
                        type = PathType.Invalid;
                        error = PathError.InvalidCharacter;
                        break;
                    }
                }
            }

            if (!type.HasValue)
            {
                if (!assumedType.HasValue)
                {
                    type = PathType.Invalid;
                    error = PathError.AmbiguousType;
                }
                else
                {
                    type = assumedType.Value;
                }
            }
            else
            {
                if (assumedType.HasValue)
                {
                    if ((type.Value != assumedType.Value) && (type.Value != PathType.Invalid))
                    {
                        type = PathType.Invalid;
                        error = PathError.WrongType;
                    }
                }
            }

            bool hasRelatives = false;
            List<string> partsNormalized = null;

            if (type != PathType.Invalid)
            {
                List<string> parts = path.Split(PathProperties.UnixDirectorySeparator, PathProperties.WindowsDirectorySeparator, PathProperties.UncDirectorySeparator)
                                         .ToList();

                partsNormalized = new List<string>(parts.Count);

                for (int i1 = 0; i1 < parts.Count; i1++)
                {
                    string part = parts[i1];

                    if ((type == PathType.Unix) && (i1 <= 0) && isRooted && (part.Length == 0))
                    {
                        continue;
                    }

                    if ((type == PathType.Windows) && (i1 <= 0) && !isRooted && (part.Length == 0))
                    {
                        continue;
                    }

                    if ((type == PathType.Unc) && (i1 <= 1) && isRooted && (part.Length == 0))
                    {
                        continue;
                    }

                    if ((type == PathType.Unc) && (i1 <= 0) && !isRooted && (part.Length == 0))
                    {
                        continue;
                    }

                    if ((i1 == (parts.Count - 1)) && (part.Length == 0))
                    {
                        continue;
                    }

                    string trimmed = PathProperties.Trim(part);

                    if (trimmed.Length == 0)
                    {
                        type = PathType.Invalid;
                        error = PathError.EmptyName;
                        break;
                    }

                    if (string.Equals(trimmed, PathProperties.RelativeSame, StringComparison.Ordinal) || string.Equals(trimmed, PathProperties.RelativeUp, StringComparison.Ordinal))
                    {
                        hasRelatives = true;

                        if (!allowRelatives)
                        {
                            type = PathType.Invalid;
                            error = PathError.RelativesNotAllowed;
                            break;
                        }
                    }

                    partsNormalized.Add(trimmed);
                }
            }

            string pathNormalized = PathProperties.CreatePath(partsNormalized, type.Value, isRooted);

            List<string> partsResolved = null;

            if (type != PathType.Invalid)
            {
                List<string> parts = new List<string>(partsNormalized);

                partsResolved = new List<string>(parts.Count);

                for (int i1 = 0; i1 < parts.Count; i1++)
                {
                    string part = parts[i1];

                    if (string.Equals(part, PathProperties.RelativeSame, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    if (string.Equals(part, PathProperties.RelativeUp, StringComparison.Ordinal))
                    {
                        if (isRooted && (partsResolved.Count <= 1))
                        {
                            type = PathType.Invalid;
                            error = PathError.RelativeGoesBeyondRoot;
                            break;
                        }

                        if (partsResolved.Count > 0)
                        {
                            if (!string.Equals(partsResolved[partsResolved.Count - 1], PathProperties.RelativeUp, StringComparison.Ordinal))
                            {
                                partsResolved.RemoveAt(partsResolved.Count - 1);
                                continue;
                            }
                        }
                    }

                    partsResolved.Add(part);
                }

                if (partsResolved.Count == 0)
                {
                    partsResolved.Add(PathProperties.RelativeSame);
                }
            }

            string pathResolved = PathProperties.CreatePath(partsResolved, type.Value, isRooted);

            StringComparison comparison = StringComparison.OrdinalIgnoreCase;
            string pathNormalizedComparable = null;

            switch (type.Value)
            {
                case PathType.Unix:
                {
                    comparison = StringComparison.Ordinal;
                    pathNormalizedComparable = pathNormalized;
                    break;
                }

                case PathType.Windows:
                {
                    comparison = StringComparison.OrdinalIgnoreCase;
                    pathNormalizedComparable = pathNormalized.ToUpperInvariant();
                    break;
                }

                case PathType.Unc:
                {
                    comparison = StringComparison.OrdinalIgnoreCase;
                    pathNormalizedComparable = pathNormalized.ToUpperInvariant();
                    break;
                }
            }

            string root = null;

            if (type != PathType.Invalid)
            {
                if (isRooted)
                {
                    root = PathProperties.CreatePath(new List<string>
                    {
                        partsNormalized[0],
                    }, type.Value, true);
                }
            }

            bool isRoot = string.Equals(pathNormalized, root, comparison);

            string parent = null;

            if (type != PathType.Invalid)
            {
                if (!isRoot && (partsNormalized.Count > 1))
                {
                    parent = PathProperties.CreatePath(partsNormalized.ToList(0, partsNormalized.Count - 1), type.Value, isRooted);
                }
            }

            string name = null;

            if (type != PathType.Invalid)
            {
                name = partsNormalized[partsNormalized.Count - 1];
            }

            int hashcode;

            if (type == PathType.Invalid)
            {
                hashcode = error.GetHashCode();
            }
            else
            {
                hashcode = pathNormalizedComparable.GetHashCode();
            }

            PathProperties pathProperties = new PathProperties();
            pathProperties.Type = type.Value;
            pathProperties.Error = error;
            pathProperties.Hashcode = hashcode;
            pathProperties.PathOriginal = path;
            pathProperties.PathNormalized = pathNormalized;
            pathProperties.PathResolved = pathResolved;
            pathProperties.Comparison = comparison;
            pathProperties.PathNormalizedComparable = pathNormalizedComparable;
            pathProperties.PartsNormalized = type == PathType.Invalid ? null : partsNormalized.ToArray();
            pathProperties.PartsResolved = type == PathType.Invalid ? null : partsResolved.ToArray();
            pathProperties.IsRooted = isRooted;
            pathProperties.IsRoot = isRoot;
            pathProperties.Root = root;
            pathProperties.Parent = parent;
            pathProperties.Name = name;
            pathProperties.HasWildcards = hasWildcards;
            pathProperties.HasRelatives = hasRelatives;
            pathProperties.AllowWildcards = allowWildcards;
            pathProperties.AllowRelatives = allowRelatives;
            return pathProperties;
        }

        /// <summary>
        ///     Gets the <see cref="PathType" /> which is used by the current system.
        /// </summary>
        /// <returns>
        ///     The <see cref="PathType" /> which is used by the current system or null if the type cannot be determined.
        /// </returns>
        public static PathType? GetSystemType ()
        {
            switch (Environment.OSVersion.Platform)
            {
                default:
                {
                    return null;
                }

                case PlatformID.Win32NT:
                {
                    return PathType.Windows;
                }

                case PlatformID.Win32Windows:
                {
                    return PathType.Windows;
                }

                case PlatformID.Win32S:
                {
                    return PathType.Windows;
                }

                case PlatformID.WinCE:
                {
                    return PathType.Windows;
                }

                case PlatformID.Xbox:
                {
                    return PathType.Windows;
                }

                case PlatformID.MacOSX:
                {
                    return PathType.Unix;
                }

                case PlatformID.Unix:
                {
                    return PathType.Unix;
                }
            }
        }

        /// <summary>
        ///     Makes an absolute path out of a path relative to a rooted path.
        /// </summary>
        /// <param name="root"> The rooted path. </param>
        /// <param name="path"> The path relative to <paramref name="root" />. </param>
        /// <returns>
        ///     The instance of <see cref="PathProperties" /> which describes the absolute path.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If <paramref name="path" /> is already absolute, nothing is done and the same path is returned.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="root" /> or <paramref name="path" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="root" /> or <paramref name="path" /> is an invalid path, <paramref name="root" /> is not a rooted path, or <paramref name="root" /> and <paramref name="path" /> are not of the same type. </exception>
        public static PathProperties MakeAbsolute (PathProperties root, PathProperties path)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!root.IsValid)
            {
                throw new InvalidPathArgumentException(nameof(root));
            }

            if (!path.IsValid)
            {
                throw new InvalidPathArgumentException(nameof(path));
            }

            if (!root.IsRooted)
            {
                throw new InvalidPathArgumentException(nameof(root));
            }

            if (root.Type != path.Type)
            {
                throw new InvalidPathArgumentException(nameof(path));
            }

            if (path.IsRooted)
            {
                return path.GetResolved();
            }

            List<string> parts = new List<string>();
            parts.AddRange(root.PartsResolved);
            parts.AddRange(path.PartsResolved);

            string resultPath = PathProperties.CreatePath(parts, root.Type, true);

            PathProperties result = PathProperties.FromPath(resultPath, root.AllowWildcards || path.AllowWildcards, root.AllowRelatives || path.AllowRelatives, null)
                                                  .GetResolved();

            return result;
        }

        /// <summary>
        ///     Makes a relative path out of a rooted path compared to another rooted path.
        /// </summary>
        /// <param name="root"> The rooted path. </param>
        /// <param name="path"> The path to be made relative compared to <paramref name="root" />. </param>
        /// <returns>
        ///     The instance of <see cref="PathProperties" /> which describes the relative path.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If <paramref name="path" /> is already relative, nothing is done and the same path is returned.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="root" /> or <paramref name="path" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="root" /> or <paramref name="path" /> is an invalid path, <paramref name="root" /> is not a rooted path, or <paramref name="root" /> and <paramref name="path" /> are not of the same type. </exception>
        public static PathProperties MakeRelative (PathProperties root, PathProperties path)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!root.IsValid)
            {
                throw new InvalidPathArgumentException(nameof(root));
            }

            if (!path.IsValid)
            {
                throw new InvalidPathArgumentException(nameof(path));
            }

            if (!root.IsRooted)
            {
                throw new InvalidPathArgumentException(nameof(root));
            }

            if (root.Type != path.Type)
            {
                throw new InvalidPathArgumentException(nameof(path));
            }

            if (!path.IsRooted)
            {
                return path.GetResolved();
            }

            string[] rootParts = root.PartsResolved;
            string[] pathParts = path.PartsResolved;

            List<string> leadingMatch = new List<string>();

            for (int i1 = 0; i1 < Math.Min(rootParts.Length, pathParts.Length); i1++)
            {
                if (string.Equals(rootParts[i1], pathParts[i1], root.Comparison))
                {
                    leadingMatch.Add(rootParts[i1]);
                }
                else
                {
                    break;
                }
            }

            //List<string> trailingMatch = new List<string>();
            //for (int i1 = 0; i1 < Math.Min(rootParts.Length, pathParts.Length); i1++)
            //{
            //	int rootIndex = rootParts.Length - ( 1 + i1 );
            //	int pathIndex = pathParts.Length - ( 1 + i1 );

            //	if (string.Equals(rootParts[rootIndex], pathParts[pathIndex], root.Comparison))
            //	{
            //		trailingMatch.Add(rootParts[rootIndex]);
            //	}
            //	else
            //	{
            //		break;
            //	}
            //}

            if (leadingMatch.Count == 0)
            {
                return path.GetResolved();
            }

            List<string> upLinks = new List<string>();

            for (int i1 = 0; i1 < (rootParts.Length - leadingMatch.Count); i1++)
            {
                upLinks.Add(PathProperties.RelativeUp);
            }

            List<string> downLinks = new List<string>();

            for (int i1 = leadingMatch.Count; i1 < pathParts.Length; i1++)
            {
                downLinks.Add(pathParts[i1]);
            }

            if (rootParts.Length == leadingMatch.Count)
            {
                if (downLinks.Count == 0)
                {
                    return PathProperties.FromPath(PathProperties.RelativeSame, root.AllowWildcards || path.AllowWildcards, root.AllowRelatives || path.AllowRelatives, root.Type)
                                         .GetResolved();
                }

                string downLinkPath = PathProperties.CreatePath(downLinks, root.Type, false);

                return PathProperties.FromPath(downLinkPath, root.AllowWildcards || path.AllowWildcards, root.AllowRelatives || path.AllowRelatives, root.Type)
                                     .GetResolved();
            }

            if (pathParts.Length == leadingMatch.Count)
            {
                string upLinkPath = PathProperties.CreatePath(upLinks, root.Type, false);

                return PathProperties.FromPath(upLinkPath, root.AllowWildcards || path.AllowWildcards, root.AllowRelatives || path.AllowRelatives, root.Type)
                                     .GetResolved();
            }

            List<string> allLinks = new List<string>();
            allLinks.AddRange(upLinks);
            allLinks.AddRange(downLinks);

            string allLinkPath = PathProperties.CreatePath(allLinks, root.Type, false);

            return PathProperties.FromPath(allLinkPath, root.AllowWildcards || path.AllowWildcards, root.AllowRelatives || path.AllowRelatives, root.Type)
                                 .GetResolved();
        }

        internal static string CreatePath (List<string> parts, PathType type, bool isRooted)
        {
            if (parts == null)
            {
                return null;
            }

            if (type == PathType.Invalid)
            {
                return null;
            }

            if (parts.Count == 0)
            {
                return string.Empty;
            }

            StringBuilder builder = new StringBuilder();

            if (isRooted)
            {
                switch (type)
                {
                    case PathType.Unix:
                    {
                        if (parts[0][0] != PathProperties.UnixDirectorySeparator)
                        {
                            builder.Append(PathProperties.UnixDirectorySeparator);
                        }

                        break;
                    }

                    case PathType.Unc:
                    {
                        if ((parts[0][0] != PathProperties.UncDirectorySeparator) || (parts[0][1] != PathProperties.UncDirectorySeparator))
                        {
                            builder.Append(PathProperties.UncDirectorySeparator);
                            builder.Append(PathProperties.UncDirectorySeparator);
                        }

                        break;
                    }
                }
            }

            for (int i1 = 0; i1 < parts.Count; i1++)
            {
                if (i1 > 0)
                {
                    builder.Append(type == PathType.Unix ? PathProperties.UnixDirectorySeparator : PathProperties.WindowsDirectorySeparator);
                }

                builder.Append(parts[i1]);
            }

            return builder.ToString();
        }

        internal static PathType? GetPathType (string path, bool rootOnly)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if ((path.Length >= 1) && (path[0] == PathProperties.UnixDirectorySeparator))
            {
                return PathType.Unix;
            }

            if ((path.Length >= 2) && PathProperties.IsAtoZ(path[0]) && (path[1] == PathProperties.WindowsDriveSeparator))
            {
                return PathType.Windows;
            }

            if ((path.Length >= 2) && (path[0] == PathProperties.UncDirectorySeparator) && (path[1] == PathProperties.UncDirectorySeparator))
            {
                return PathType.Unc;
            }

            if (rootOnly)
            {
                return null;
            }

            foreach (char chr in path)
            {
                if (chr == PathProperties.UnixDirectorySeparator)
                {
                    return PathType.Unix;
                }

                if (chr == PathProperties.WindowsDirectorySeparator)
                {
                    return PathType.Windows;
                }
            }

            return null;
        }

        private static bool IsAtoZ (char chr)
        {
            chr = char.ToUpperInvariant(chr);
            return (chr >= 'A') && (chr <= 'Z');
        }

        private static string Trim (string part)
        {
            return part.TrimEnd('\t', '\n', '\v', '\f', '\r', (char)0x20, (char)0x85, (char)0xA0);
        }

        #endregion




        #region Instance Constructor/Destructor

        private PathProperties ()
        {
            this.Type = PathType.Invalid;
            this.Error = PathError.None;
            this.PathOriginal = null;
            this.PathNormalized = null;
            this.PathResolved = null;
            this.PartsNormalized = null;
            this.PartsResolved = null;
            this.IsRooted = false;
            this.IsRoot = false;
            this.Root = null;
            this.Parent = null;
            this.Name = null;
            this.HasWildcards = false;
            this.HasRelatives = false;
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the error of the path.
        /// </summary>
        /// <value>
        ///     The error of the path.
        ///     <see cref="PathError.None" /> if the path is valid.
        /// </value>
        public PathError Error { get; private set; }

        /// <summary>
        ///     Gets whether the path contains relative directory names.
        /// </summary>
        /// <value>
        ///     true if the path contains relative directory names, false otherwise
        /// </value>
        public bool HasRelatives { get; private set; }

        /// <summary>
        ///     Gets whether the path contains wildcards.
        /// </summary>
        /// <value>
        ///     true if the path contains wildcards, false otherwise.
        /// </value>
        public bool HasWildcards { get; private set; }

        /// <summary>
        ///     Gets whether the path is a root.
        /// </summary>
        /// <value>
        ///     true if the path is a root, false otherwise.
        /// </value>
        public bool IsRoot { get; private set; }

        /// <summary>
        ///     Gets whether the path is rooted or absolute respectively.
        /// </summary>
        /// <value>
        ///     true if the path is rooted, false otherwise.
        /// </value>
        public bool IsRooted { get; private set; }

        /// <summary>
        ///     Gets whether the path is valid.
        /// </summary>
        /// <value>
        ///     true if the path is valid, false otherwise.
        /// </value>
        public bool IsValid => this.Type != PathType.Invalid;

        /// <summary>
        ///     Gets the name of the path.
        /// </summary>
        /// <value>
        ///     The name of the path.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         The name of a path is the last element in the path, e.g. the file name of a file path or the directory name of a directory path.
        ///     </para>
        /// </remarks>
        public string Name { get; private set; }

        /// <summary>
        ///     Gets the parent of the path.
        /// </summary>
        /// <value>
        ///     The parent of the path or null if the path is a root or does not have a parent.
        /// </value>
        public string Parent { get; private set; }

        /// <summary>
        ///     Gets the array with all normalized (but still unresolved) parts of the path.
        /// </summary>
        /// <value>
        ///     The array with all normalized (but still unresolved) parts of the path.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         Each part of the path is either the root or a directory.
        ///     </para>
        ///     <para>
        ///         Normalization includes removal of unnecessary leading or trailing directory separators.
        ///     </para>
        /// </remarks>
        public string[] PartsNormalized { get; private set; }

        /// <summary>
        ///     Gets the array with all normalized and resolved parts of the path.
        /// </summary>
        /// <value>
        ///     The array with all normalized and resolved parts of the path.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         Each part of the path is either the root or a directory.
        ///     </para>
        ///     <para>
        ///         Resolving includes normalization and resolving of relative directory names.
        ///     </para>
        /// </remarks>
        public string[] PartsResolved { get; private set; }

        /// <summary>
        ///     Gets the normalized path.
        /// </summary>
        /// <value>
        ///     The normalized path.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         Normalization includes removal of unnecessary leading or trailing directory separators.
        ///     </para>
        /// </remarks>
        public string PathNormalized { get; private set; }

        /// <summary>
        ///     Gets the original path.
        /// </summary>
        /// <value>
        ///     The original path.
        /// </value>
        public string PathOriginal { get; private set; }

        /// <summary>
        ///     Gets the resolved path.
        /// </summary>
        /// <value>
        ///     The resolved path.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         Resolving includes normalization and resolving of relative directory names.
        ///     </para>
        /// </remarks>
        public string PathResolved { get; private set; }

        /// <summary>
        ///     Gets the root of the path.
        /// </summary>
        /// <value>
        ///     The root of the path or null if the path has no root or is not rooted respectively.
        /// </value>
        public string Root { get; private set; }

        /// <summary>
        ///     Gets the type of the path.
        /// </summary>
        /// <value>
        ///     The type of the path.
        /// </value>
        public PathType Type { get; private set; }

        internal bool AllowRelatives { get; private set; }

        internal bool AllowWildcards { get; private set; }

        private StringComparison Comparison { get; set; }

        private int Hashcode { get; set; }

        private string PathNormalizedComparable { get; set; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Creates a copy of this path using a different path type.
        /// </summary>
        /// <param name="type"> The path type to use for the copy. </param>
        /// <returns>
        ///     The copy of this path with the specified path type.
        /// </returns>
        /// <exception cref="ArgumentException"> <paramref name="type" /> is <see cref="PathType.Invalid" />. </exception>
        public PathProperties ChangeType (PathType type)
        {
            if (type == PathType.Invalid)
            {
                throw new ArgumentException("Path type is invalid.", nameof(type));
            }

            string path = PathProperties.CreatePath(this.PartsNormalized.ToList(), type, this.IsRooted);
            PathProperties copy = PathProperties.FromPath(path, this.HasWildcards, this.HasRelatives, type);
            return copy;
        }

        /// <summary>
        ///     Determines whether this path is compatible with another path.
        /// </summary>
        /// <param name="other"> The other path. </param>
        /// <returns>
        ///     true if the paths are compatible, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="other" /> is null. </exception>
        public bool IsCompatibleWith (PathString other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return this.IsCompatibleWith(other.Type, other.PathInternal);
        }

        /// <summary>
        ///     Determines whether this path is compatible with another path.
        /// </summary>
        /// <param name="other"> The other path. </param>
        /// <returns>
        ///     true if the paths are compatible, false otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="other" /> is null. </exception>
        public bool IsCompatibleWith (PathProperties other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return this.IsCompatibleWith(other.Type, other);
        }

        /// <summary>
        ///     Determines whether this path is compatible with a specifed path type.
        /// </summary>
        /// <param name="type"> The path type to check compatibility with. </param>
        /// <returns>
        ///     true if the path is compatible, false otherwise.
        /// </returns>
        public bool IsCompatibleWith (PathType type)
        {
            return this.IsCompatibleWith(type, null);
        }

        /// <summary>
        ///     Determines whether this path is compatible with the current system.
        /// </summary>
        /// <returns>
        ///     true if the path is compatible, false otherwise.
        /// </returns>
        public bool IsCompatibleWith ()
        {
            PathType? type = PathProperties.GetSystemType();

            if (!type.HasValue)
            {
                return false;
            }

            return this.IsCompatibleWith(type.Value, null);
        }

        /// <inheritdoc cref="IFormattable.ToString(string,IFormatProvider)" />
        public string ToString (string format)
        {
            switch (format?.ToLowerInvariant())
            {
                case {} when format.IsNullOrEmpty() || (format == "o"):
                    return this.PathOriginal;

                case { } when format.IsNullOrEmpty() || (format == "n"):
                    return this.PathNormalized;

                case { } when format.IsNullOrEmpty() || (format == "r"):
                    return this.PathResolved;

                default:
                    throw new ArgumentException($"Invalid format string: {format}", nameof(format));
            }
        }

        private PathProperties GetResolved ()
        {
            return PathProperties.FromPath(this.PathResolved, this.AllowWildcards, this.AllowRelatives, this.Type);
        }

        private bool IsCompatibleWith (PathType type, PathProperties other)
        {
            bool oneIsInvalid = (this.Type == PathType.Invalid) || (type == PathType.Invalid);
            bool oneIsWindows = (this.Type == PathType.Windows) || (type == PathType.Windows);
            bool oneIsUnix = (this.Type == PathType.Unix) || (type == PathType.Unix);
            bool oneIsUnc = (this.Type == PathType.Unc) || (type == PathType.Unc);

            bool bothAreRelative = !this.IsRooted && (other?.IsRooted ?? false);

            if (oneIsInvalid)
            {
                return false;
            }

            if (oneIsUnix && (oneIsWindows || oneIsUnc))
            {
                return false;
            }

            if (bothAreRelative && oneIsWindows && oneIsUnc)
            {
                return true;
            }

            if (oneIsWindows && oneIsUnc)
            {
                return false;
            }

            return true;
        }

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override bool Equals (object obj)
        {
            return this.Equals(obj as PathProperties);
        }

        /// <inheritdoc />
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode"),]
        public override int GetHashCode ()
        {
            return this.Hashcode;
        }

        /// <inheritdoc />
        public override string ToString ()
        {
            return this.ToString(null);
        }

        #endregion




        #region Interface: ICloneable<PathProperties>

        /// <inheritdoc />
        public PathProperties Clone ()
        {
            return PathProperties.FromPath(this.PathOriginal, this.AllowWildcards, this.AllowRelatives, this.Type);
        }

        /// <inheritdoc />
        object ICloneable.Clone ()
        {
            return this.Clone();
        }

        #endregion




        #region Interface: IComparable

        /// <inheritdoc />
        public int CompareTo (object obj)
        {
            return this.CompareTo(obj as PathProperties);
        }

        #endregion




        #region Interface: IComparable<PathProperties>

        /// <inheritdoc />
        public int CompareTo (PathProperties other)
        {
            if (other == null)
            {
                return 1;
            }

            return string.Compare(this.PathNormalizedComparable, other.PathNormalizedComparable, StringComparison.Ordinal);
        }

        #endregion




        #region Interface: IEquatable<PathProperties>

        /// <inheritdoc />
        public bool Equals (PathProperties other)
        {
            if (other == null)
            {
                return false;
            }

            return string.Equals(this.PathNormalizedComparable, other.PathNormalizedComparable, StringComparison.Ordinal);
        }

        #endregion




        #region Interface: IFormattable

        /// <inheritdoc />
        string IFormattable.ToString (string format, IFormatProvider formatProvider)
        {
            return this.ToString(format);
        }

        #endregion
    }
}
