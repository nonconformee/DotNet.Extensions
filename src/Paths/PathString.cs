using System;

using RI.Utilities.Exceptions;
using RI.Utilities.ObjectModel;




namespace RI.Utilities.Paths
{
    /// <summary>
    ///     Base class for specialized path objects (<see cref="FilePath" />, <see cref="DirectoryPath" />).
    /// </summary>
    /// <threadsafety static="false" instance="false" />
    [Serializable,]
    public abstract class PathString : IEquatable<PathString>, IComparable, IComparable<PathString>, ICloneable, ICloneable<PathString>, IFormattable
    {
        #region Static Methods

        /// <summary>
        ///     Compares two <see cref="PathString" />s for order.
        /// </summary>
        /// <param name="x"> The first <see cref="PathString" />. </param>
        /// <param name="y"> The second <see cref="PathString" />. </param>
        /// <returns> The order of <paramref name="y" /> compared to <paramref name="x" />. </returns>
        /// <remarks>
        ///     See <see cref="CompareTo(PathString)" /> for more details.
        /// </remarks>
        public static int Compare (PathString x, PathString y)
        {
            if ((x == null) && (y == null))
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            return x.CompareTo(y);
        }

        /// <summary>
        ///     Compares two <see cref="PathString" />s for equality.
        /// </summary>
        /// <param name="x"> The first <see cref="PathString" />. </param>
        /// <param name="y"> The second <see cref="PathString" />. </param>
        /// <returns> The equality of <paramref name="y" /> compared to <paramref name="x" />. </returns>
        /// <remarks>
        ///     See <see cref="Equals(PathString)" /> for more details.
        /// </remarks>
        public static bool Equals (PathString x, PathString y)
        {
            if (ReferenceEquals(x, null) && ReferenceEquals(y, null))
            {
                return true;
            }

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
            {
                return false;
            }

            return x.Equals(y);
        }

        /// <summary>
        ///     Compares two <see cref="PathString" />s for equality.
        /// </summary>
        /// <param name="x"> The first <see cref="PathString" />. </param>
        /// <param name="y"> The second <see cref="PathString" />. </param>
        /// <returns> The equality of <paramref name="y" /> compared to <paramref name="x" />. </returns>
        /// <remarks>
        ///     See <see cref="Equals(PathString)" /> for more details.
        /// </remarks>
        public static bool operator == (PathString x, PathString y)
        {
            return PathString.Equals(x, y);
        }

        /// <summary>
        ///     Compares two <see cref="PathString" />s for order.
        /// </summary>
        /// <param name="x"> The first <see cref="PathString" />. </param>
        /// <param name="y"> The second <see cref="PathString" />. </param>
        /// <returns> The order of <paramref name="y" /> compared to <paramref name="x" />. </returns>
        /// <remarks>
        ///     See <see cref="CompareTo(PathString)" /> for more details.
        /// </remarks>
        public static bool operator > (PathString x, PathString y)
        {
            return PathString.Compare(x, y) > 0;
        }

        /// <summary>
        ///     Compares two <see cref="PathString" />s for order.
        /// </summary>
        /// <param name="x"> The first <see cref="PathString" />. </param>
        /// <param name="y"> The second <see cref="PathString" />. </param>
        /// <returns> The order of <paramref name="y" /> compared to <paramref name="x" />. </returns>
        /// <remarks>
        ///     See <see cref="CompareTo(PathString)" /> for more details.
        /// </remarks>
        public static bool operator >= (PathString x, PathString y)
        {
            return PathString.Compare(x, y) >= 0;
        }

        /// <summary>
        ///     Implicit conversion of a <see cref="PathString" /> to <see cref="string" />.
        /// </summary>
        /// <param name="path"> The path to convert to a string. </param>
        /// <returns>
        ///     The string.
        /// </returns>
        public static implicit operator string (PathString path)
        {
            if (path == null)
            {
                return null;
            }

            return path.PathResolved;
        }

        /// <summary>
        ///     Compares two <see cref="PathString" />s for equality.
        /// </summary>
        /// <param name="x"> The first <see cref="PathString" />. </param>
        /// <param name="y"> The second <see cref="PathString" />. </param>
        /// <returns> The equality of <paramref name="y" /> compared to <paramref name="x" />. </returns>
        /// <remarks>
        ///     See <see cref="Equals(PathString)" /> for more details.
        /// </remarks>
        public static bool operator != (PathString x, PathString y)
        {
            return !PathString.Equals(x, y);
        }

        /// <summary>
        ///     Compares two <see cref="PathString" />s for order.
        /// </summary>
        /// <param name="x"> The first <see cref="PathString" />. </param>
        /// <param name="y"> The second <see cref="PathString" />. </param>
        /// <returns> The order of <paramref name="y" /> compared to <paramref name="x" />. </returns>
        /// <remarks>
        ///     See <see cref="CompareTo(PathString)" /> for more details.
        /// </remarks>
        public static bool operator < (PathString x, PathString y)
        {
            return PathString.Compare(x, y) < 0;
        }

        /// <summary>
        ///     Compares two <see cref="PathString" />s for order.
        /// </summary>
        /// <param name="x"> The first <see cref="PathString" />. </param>
        /// <param name="y"> The second <see cref="PathString" />. </param>
        /// <returns> The order of <paramref name="y" /> compared to <paramref name="x" />. </returns>
        /// <remarks>
        ///     See <see cref="CompareTo(PathString)" /> for more details.
        /// </remarks>
        public static bool operator <= (PathString x, PathString y)
        {
            return PathString.Compare(x, y) <= 0;
        }

        #endregion




        #region Instance Constructor/Destructor

        internal PathString (PathProperties path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (!path.IsValid)
            {
                throw new InvalidPathArgumentException(nameof(path), path.Error);
            }

            this.PathInternal = path;
        }

        #endregion




        #region Instance Properties/Indexer

        /// <inheritdoc cref="PathProperties.HasRelatives" />
        public bool HasRelatives => this.PathInternal.HasRelatives;

        /// <inheritdoc cref="PathProperties.HasWildcards" />
        public bool HasWildcards => this.PathInternal.HasWildcards;

        /// <inheritdoc cref="PathProperties.IsRoot" />
        public bool IsRoot => this.PathInternal.IsRoot;

        /// <inheritdoc cref="PathProperties.IsRooted" />
        public bool IsRooted => this.PathInternal.IsRooted;

        /// <inheritdoc cref="PathProperties.PathNormalized" />
        public string PathNormalized => this.PathInternal.PathNormalized;

        /// <inheritdoc cref="PathProperties.PathOriginal" />
        public string PathOriginal => this.PathInternal.PathOriginal;

        /// <inheritdoc cref="PathProperties.PathResolved" />
        public string PathResolved => this.PathInternal.PathResolved;

        /// <inheritdoc cref="PathProperties.Root" />
        public DirectoryPath Root
        {
            get
            {
                if (this.PathInternal.Root == null)
                {
                    return null;
                }

                return new DirectoryPath(this.PathInternal.Root);
            }
        }

        /// <inheritdoc cref="PathProperties.Type" />
        public PathType Type => this.PathInternal.Type;

        internal PathProperties PathInternal { get; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Creates a copy of this directory path using a different path type.
        /// </summary>
        /// <param name="type"> The directory path type to use for the copy. </param>
        /// <returns>
        ///     The copy of this directory path with the specified path type.
        /// </returns>
        /// <exception cref="ArgumentException"> <paramref name="type" /> is <see cref="PathType.Invalid" />. </exception>
        public PathString ChangeType (PathType type)
        {
            return this.ChangeTypeInternal(type);
        }

        /// <inheritdoc cref="PathString.CompareTo(PathString)" />
        public int CompareTo (PathProperties other)
        {
            return this.PathInternal.CompareTo(other);
        }

        /// <inheritdoc cref="PathString.Equals(PathString)" />
        public bool Equals (PathProperties other)
        {
            return this.PathInternal.Equals(other);
        }

        /// <summary>
        ///     Gets the properties of the path represented by this <see cref="PathString" />.
        /// </summary>
        /// <returns>
        ///     The properties of the path represented by this <see cref="PathString" />
        /// </returns>
        public PathProperties GetPathProperties ()
        {
            return this.PathInternal.Clone();
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

            return this.PathInternal.IsCompatibleWith(other);
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

            return this.PathInternal.IsCompatibleWith(other);
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
            return this.PathInternal.IsCompatibleWith(type);
        }

        /// <summary>
        ///     Determines whether this path is compatible with the current system.
        /// </summary>
        /// <returns>
        ///     true if the path is compatible, false otherwise.
        /// </returns>
        public bool IsCompatibleWith ()
        {
            return this.PathInternal.IsCompatibleWith();
        }

        /// <inheritdoc cref="IFormattable.ToString(string,IFormatProvider)" />
        public string ToString (string format)
        {
            return this.PathInternal.ToString(format);
        }

        #endregion




        #region Abstracts

        /// <inheritdoc cref="ChangeType" />
        protected abstract PathString ChangeTypeInternal (PathType type);

        /// <inheritdoc cref="ICloneable{PathString}.Clone()" />
        protected abstract PathString CloneInternal ();

        #endregion




        #region Overrides

        /// <inheritdoc />
        public override bool Equals (object obj)
        {
            if (obj is PathProperties)
            {
                return this.Equals((PathProperties)obj);
            }

            return this.Equals(obj as PathString);
        }

        /// <inheritdoc />
        public override int GetHashCode ()
        {
            return this.PathInternal.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString ()
        {
            return this.PathInternal.ToString();
        }

        #endregion




        #region Interface: ICloneable

        /// <inheritdoc />
        object ICloneable.Clone ()
        {
            return this.CloneInternal();
        }

        #endregion




        #region Interface: ICloneable<PathString>

        /// <inheritdoc />
        PathString ICloneable<PathString>.Clone ()
        {
            return this.CloneInternal();
        }

        #endregion




        #region Interface: IComparable

        /// <inheritdoc />
        public int CompareTo (object obj)
        {
            if (obj is PathProperties)
            {
                return this.CompareTo((PathProperties)obj);
            }

            return this.CompareTo(obj as PathString);
        }

        #endregion




        #region Interface: IComparable<PathString>

        /// <inheritdoc />
        public int CompareTo (PathString other)
        {
            return this.CompareTo(other?.PathInternal);
        }

        #endregion




        #region Interface: IEquatable<PathString>

        /// <inheritdoc />
        public bool Equals (PathString other)
        {
            return this.Equals(other?.PathInternal);
        }

        #endregion




        #region Interface: IFormattable

        /// <inheritdoc />
        string IFormattable.ToString (string format, IFormatProvider formatProvider)
        {
            return ((IFormattable)this.PathInternal).ToString(format, formatProvider);
        }

        #endregion
    }
}
