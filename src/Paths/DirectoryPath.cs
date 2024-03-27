using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using RI.Utilities.Exceptions;
using RI.Utilities.ObjectModel;
using RI.Utilities.Text;




namespace RI.Utilities.Paths
{
    /// <summary>
    ///     Describes a path to a directory.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="DirectoryPath" /> uses <see cref="PathProperties" /> to extract and store path information.
    ///         See <see cref="PathProperties" /> for more details about the supported types of directory paths.
    ///     </para>
    ///     <para>
    ///         <see cref="DirectoryPath" /> provides more directory path specific functionalities compared to <see cref="string" /> and offers a more consistent way of working with paths than <see cref="Path" />.
    ///         It can be implicitly converted to a <see cref="string" /> to work seamless with APIs using <see cref="string" /> for paths.
    ///     </para>
    ///     <para>
    ///         See <see cref="PathProperties" /> for possible format strings for <see cref="PathString.ToString(string)" />.
    ///     </para>
    ///     <para>
    ///         See <see cref="FilePath" /> for an example how to use <see cref="DirectoryPath" /> and <see cref="FilePath" />.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    /// <example>
    ///     <code language="cs">
    /// <![CDATA[
    /// // see FilePath for examples
    /// ]]>
    /// </code>
    /// </example>
    [Serializable,]
    public sealed class DirectoryPath : PathString, ICloneable<DirectoryPath>, IEquatable<DirectoryPath>, IComparable<DirectoryPath>
    {
        #region Constants

        /// <summary>
        ///     The file extension for temporary files used by <see cref="GetTempFile" />.
        /// </summary>
        public const string TemporaryExtension = ".tmp";

        #endregion




        #region Static Methods

        /// <summary>
        ///     Gets the path to the current working directory.
        /// </summary>
        /// <returns>
        ///     The path to the current working directory.
        /// </returns>
        public static DirectoryPath GetCurrentDirectory ()
        {
            return new DirectoryPath(Environment.CurrentDirectory);
        }

        /// <summary>
        ///     Gets the path to the current temporary directory.
        /// </summary>
        /// <returns>
        ///     The path to the current temporary directory.
        /// </returns>
        public static DirectoryPath GetTempDirectory ()
        {
            return new DirectoryPath(Path.GetTempPath());
        }

        /// <summary>
        ///     Implicit conversion of a <see cref="string" /> to <see cref="DirectoryPath" />.
        /// </summary>
        /// <param name="path"> The path to convert to a directory path. </param>
        /// <returns>
        ///     The directory path.
        /// </returns>
        public static implicit operator DirectoryPath (string path)
        {
            if (path == null)
            {
                return null;
            }

            return new DirectoryPath(path);
        }

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="DirectoryPath" />.
        /// </summary>
        /// <param name="path"> The path. </param>
        /// <remarks>
        ///     <para>
        ///         Using this constructor, wildcards and relative paths are allowed and the type of the path is assumed to be of the same type as used on the current system.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="path" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="path" /> is not a valid directory path. </exception>
        public DirectoryPath (string path)
            : this(PathProperties.FromPath(path, true, true, PathProperties.GetPathType(path, false) ?? PathProperties.GetSystemType())) { }

        /// <summary>
        ///     Creates a new instance of <see cref="DirectoryPath" />.
        /// </summary>
        /// <param name="path"> The path. </param>
        /// <param name="allowWildcards"> Specifies whether wildcards are allowed or not. </param>
        /// <param name="allowRelatives"> Specifies whether relative directory names are allowed or not. </param>
        /// <param name="assumedType"> Optionally specifies the type of the path which is assumed if the type cannot be clearly determined through analysis of <paramref name="path" />. </param>
        /// <remarks>
        ///     <para>
        ///         See <see cref="PathProperties.FromPath(string,bool,bool,PathType?)" /> for more details about the parameters, especially <paramref name="assumedType" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="path" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="path" /> is not a valid directory path. </exception>
        public DirectoryPath (string path, bool allowWildcards, bool allowRelatives, PathType? assumedType)
            : this(PathProperties.FromPath(path, allowWildcards, allowRelatives, assumedType)) { }

        /// <summary>
        ///     Creates a new instance of <see cref="DirectoryPath" />.
        /// </summary>
        /// <param name="path"> The <see cref="PathProperties" /> object which describes the path. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="path" /> is null. </exception>
        public DirectoryPath (PathProperties path)
            : base(path) { }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the name of the directory.
        /// </summary>
        /// <value>
        ///     The name of the directory.
        /// </value>
        public string DirectoryName => this.PathInternal.Name;

        /// <summary>
        ///     Gets whether the directory exists.
        /// </summary>
        /// <value>
        ///     true if the directory exists, false otherwise.
        /// </value>
        /// <remarks>
        ///     <note type="note"> <see cref="Exists" /> does not throw exceptions besides <see cref="InvalidOperationException" />. For example, if the directory exists but the user does not have access permissions, the directory is not of a compatible path type used on the current system, etc., false is returned. </note>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The directory contains wildcards. </exception>
        public bool Exists
        {
            get
            {
                this.VerifyRealDirectory();

                try
                {
                    return Directory.Exists(this);
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///     Gets whether the directory path is a &quot;real&quot; usable directory.
        /// </summary>
        /// <value>
        ///     true if the directory path is a real usable directory, false otherwise.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         A real usable directory is a directory which has not wildcards.
        ///     </para>
        /// </remarks>
        public bool IsRealDirectory => !this.HasWildcards;

        /// <summary>
        ///     Gets the parent directory.
        /// </summary>
        /// <value>
        ///     The parent directory or null if this directory is a root or does not have a parent directory.
        /// </value>
        public DirectoryPath Parent
        {
            get
            {
                string parent = this.PathInternal.Parent;

                if (parent == null)
                {
                    return null;
                }

                return new DirectoryPath(parent, this.PathInternal.AllowWildcards, this.PathInternal.AllowRelatives, this.Type);
            }
        }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Creates a new directory path by appending one or more additional directory paths.
        /// </summary>
        /// <param name="directories"> A sequence with one or more directory paths to append. </param>
        /// <returns>
        ///     The new directory path with all appended directories.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If <paramref name="directories" /> is an empty sequence, the same instance as this directory path is returned without any changes.
        ///     </para>
        ///     <para>
        ///         <paramref name="directories" /> is only enumerated once.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="directories" /> is null or contains a null value. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="directories" /> contains at least one <see cref="DirectoryPath" /> which is rooted. </exception>
        public DirectoryPath AppendDirectory (IEnumerable<DirectoryPath> directories)
        {
            if (directories == null)
            {
                throw new ArgumentNullException(nameof(directories));
            }

            return this.AppendDirectory(directories.ToArray());
        }

        /// <summary>
        ///     Creates a new directory path by appending one or more additional directory paths.
        /// </summary>
        /// <param name="directories"> An array with one or more directory paths to append. </param>
        /// <returns>
        ///     The new directory path with all appended directories.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If <paramref name="directories" /> is an empty array, the same instance as this directory path is returned without any changes.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="directories" /> is null or contains a null value. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="directories" /> contains at least one <see cref="DirectoryPath" /> which is rooted. </exception>
        public DirectoryPath AppendDirectory (params DirectoryPath[] directories)
        {
            if (directories == null)
            {
                throw new ArgumentNullException(nameof(directories));
            }

            List<string> parts = new List<string>();
            parts.Add(this.PathNormalized);

            foreach (DirectoryPath directory in directories)
            {
                if (directory == null)
                {
                    throw new ArgumentNullException(nameof(directories));
                }

                if (directory.IsRooted)
                {
                    throw new InvalidPathArgumentException(nameof(directories));
                }

                parts.Add(directory.PathNormalized);
            }

            string path = PathProperties.CreatePath(parts, this.Type, this.IsRooted);
            return new DirectoryPath(path, this.PathInternal.AllowWildcards, this.PathInternal.AllowRelatives, this.Type);
        }

        /// <summary>
        ///     Creates a new file path by appending an existing relative file path to this directory.
        /// </summary>
        /// <param name="file"> The file path to append. </param>
        /// <returns>
        ///     The new file path with this directory and the appended file path.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="file" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="file" /> is a rooted file path. </exception>
        public FilePath AppendFile (FilePath file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (file.IsRooted)
            {
                throw new InvalidPathArgumentException(nameof(file));
            }

            List<string> parts = new List<string>(2);
            parts.Add(this.PathNormalized);
            parts.Add(file.PathNormalized);

            string path = PathProperties.CreatePath(parts, this.Type, this.IsRooted);
            return new FilePath(path, this.PathInternal.AllowWildcards, this.PathInternal.AllowRelatives, this.Type);
        }

        /// <summary>
        ///     Creates a new directory path with this directories parent directory but another directory name.
        /// </summary>
        /// <param name="directoryName"> The new directory name. </param>
        /// <returns>
        ///     The new directory path.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="directoryName" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="directoryName" /> is empty. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="directoryName" /> is not a valid new directory name. </exception>
        public DirectoryPath ChangeDirectoryName (string directoryName)
        {
            if (directoryName == null)
            {
                throw new ArgumentNullException(nameof(directoryName));
            }

            if (directoryName.IsEmptyOrWhitespace())
            {
                throw new EmptyStringArgumentException(nameof(directoryName));
            }

            try
            {
                if (this.Parent == null)
                {
                    return new DirectoryPath(directoryName, this.PathInternal.AllowWildcards, this.PathInternal.AllowRelatives, this.Type);
                }

                return this.Parent.AppendDirectory(new DirectoryPath(directoryName, this.PathInternal.AllowWildcards, this.PathInternal.AllowRelatives, this.Type));
            }
            catch (InvalidPathArgumentException exception)
            {
                throw new InvalidPathArgumentException(nameof(directoryName), exception.Message);
            }
        }

        /// <summary>
        ///     Creates a new directory path with this directory name but another parent directory.
        /// </summary>
        /// <param name="newParent"> The new parent directory. </param>
        /// <returns>
        ///     The new directory path.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If <paramref name="newParent" /> is null, the resulting directory is a relative directory only consisting of this <see cref="DirectoryName" /> where the whole parent directory part is removed.
        ///     </para>
        /// </remarks>
        public DirectoryPath ChangeParent (DirectoryPath newParent)
        {
            if (newParent == null)
            {
                return new DirectoryPath(this.DirectoryName, this.PathInternal.AllowWildcards, this.PathInternal.AllowRelatives, this.Type);
            }

            return newParent.AppendDirectory(new DirectoryPath(this.DirectoryName));
        }

        /// <inheritdoc cref="PathString.ChangeType" />
        public new DirectoryPath ChangeType (PathType type)
        {
            PathProperties properties = this.PathInternal.ChangeType(type);
            return new DirectoryPath(properties);
        }

        /// <summary>
        ///     Creates the directory if it does not exists or leaves an existing directory unchanged.
        /// </summary>
        /// <returns>
        ///     true if the directory was newly created, false if the directory already existed.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The directory contains wildcards. </exception>
        /// <exception cref="IOException"> The directory path is actually an existing file or a part of its parent is not available. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid directory path, the directory path is too long for the current system to be used. </exception>
        /// <exception cref="NotSupportedException"> The directory is not of a compatible path type used on the current system. </exception>
        public bool Create ()
        {
            this.VerifyRealDirectory();

            if (this.Exists)
            {
                return false;
            }

            try
            {
                Directory.CreateDirectory(this);
            }
            catch (DirectoryNotFoundException exception)
            {
                throw new IOException(exception.Message, exception);
            }

            return true;
        }

        /// <summary>
        ///     Deletes the directory and all its files and subdirectories.
        /// </summary>
        /// <returns>
        ///     true if the directory existed and was deleted, false otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The directory contains wildcards. </exception>
        /// <exception cref="IOException"> The directory path is actually an existing file, the directory is read-only, the directory is the current working directory, the directory contains files which cannot be deleted, or the directory is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid directory path, the directory path is too long for the current system to be used. </exception>
        public bool Delete ()
        {
            this.VerifyRealDirectory();

            if (!this.Exists)
            {
                return false;
            }

            Directory.Delete(this, true);
            return true;
        }

        /// <summary>
        ///     Gets a list of all files in this directory path.
        /// </summary>
        /// <param name="relativePaths"> Specifies whether the list of files should be relative to this directory path or not. </param>
        /// <param name="recursive"> Specifies whether only immediate files in this directory path should be returned or recursively all files, including files in subdirectories. </param>
        /// <returns>
        ///     The list of files of this directory path.
        ///     If no files are available, an empty list is returned.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The directory contains wildcards. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid directory path, the directory path is too long for the current system to be used. </exception>
        /// <exception cref="IOException"> The directory path is actually an existing file or a part of its parent is not available. </exception>
        /// <exception cref="DirectoryNotFoundException"> The directory path does not exist. </exception>
        public List<FilePath> GetFiles (bool relativePaths, bool recursive)
        {
            return this.GetFiles(relativePaths, recursive, null);
        }

        /// <summary>
        ///     Gets a list of all files in this directory path which matches a specified file name pattern.
        /// </summary>
        /// <param name="relativePaths"> Specifies whether the list of files should be relative to this directory path or not. </param>
        /// <param name="recursive"> Specifies whether only immediate files in this directory path should be returned or recursively all files, including files in subdirectories. </param>
        /// <param name="pattern"> The pattern for the file names (can be null to return all files). </param>
        /// <returns>
        ///     The list of files of this directory path.
        ///     If no files are available, an empty list is returned.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The directory contains wildcards. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid directory path, the directory path is too long for the current system to be used. </exception>
        /// <exception cref="IOException"> The directory path is actually an existing file or a part of its parent is not available. </exception>
        /// <exception cref="DirectoryNotFoundException"> The directory path does not exist. </exception>
        public List<FilePath> GetFiles (bool relativePaths, bool recursive, string pattern)
        {
            this.VerifyRealDirectory();

            string[] entries = Directory.GetFiles(this, pattern ?? "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            List<FilePath> files = new List<FilePath>(from x in entries
                                                      select relativePaths ? new FilePath(x).MakeRelativeTo(this) : new FilePath(x));

            return files;
        }

        /// <summary>
        ///     Gets a list of all subdirectories in this directory path.
        /// </summary>
        /// <param name="relativePaths"> Specifies whether the list of subdirectories should be relative to this directory path or not. </param>
        /// <param name="recursive"> Specifies whether only immediate subdirectories should be returned or recursively all subdirectories. </param>
        /// <returns>
        ///     The list of subdirectories of this directory path.
        ///     If no subdirectories are available, an empty list is returned.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The directory contains wildcards. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid directory path, the directory path is too long for the current system to be used. </exception>
        /// <exception cref="IOException"> The directory path is actually an existing file or a part of its parent is not available. </exception>
        /// <exception cref="DirectoryNotFoundException"> The directory path does not exist. </exception>
        public List<DirectoryPath> GetSubdirectories (bool relativePaths, bool recursive)
        {
            return this.GetSubdirectories(relativePaths, recursive, null);
        }

        /// <summary>
        ///     Gets a list of all subdirectories in this directory path which matches a specified directory name pattern.
        /// </summary>
        /// <param name="relativePaths"> Specifies whether the list of subdirectories should be relative to this directory path or not. </param>
        /// <param name="recursive"> Specifies whether only immediate subdirectories should be returned or recursively all subdirectories. </param>
        /// <param name="pattern"> The pattern for the directory names (can be null to return all subdirectories). </param>
        /// <returns>
        ///     The list of subdirectories of this directory path.
        ///     If no subdirectories are available, an empty list is returned.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The directory contains wildcards. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid directory path, the directory path is too long for the current system to be used. </exception>
        /// <exception cref="IOException"> The directory path is actually an existing file or a part of its parent is not available. </exception>
        /// <exception cref="DirectoryNotFoundException"> The directory path does not exist. </exception>
        public List<DirectoryPath> GetSubdirectories (bool relativePaths, bool recursive, string pattern)
        {
            this.VerifyRealDirectory();

            string[] entries = Directory.GetDirectories(this, pattern ?? "*", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            List<DirectoryPath> subdirectories = new List<DirectoryPath>(from x in entries
                                                                         select relativePaths ? new DirectoryPath(x).MakeRelativeTo(this) : new DirectoryPath(x));

            return subdirectories;
        }

        /// <summary>
        ///     Creates a temporary zero-byte file.
        /// </summary>
        /// <returns>
        ///     The path to the newly created temporary file.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         <see cref="GetTempFile" /> creates a new file name consisting of a <see cref="Guid" /> and <see cref="TemporaryExtension" /> and then creates the file using <see cref="FilePath.CreateNew" />.
        ///         If a file with the created <see cref="Guid" /> (veeeery unlikely...) already exists, it tries another <see cref="Guid" />, as long as necessary to find an unused <see cref="Guid" />.
        ///     </para>
        /// </remarks>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions, the file is read-only, or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        /// <exception cref="DirectoryNotFoundException"> The files directory does not exist or is not available. </exception>
        /// <exception cref="NotSupportedException"> The file is not of a compatible path type used on the current system. </exception>
        public FilePath GetTempFile ()
        {
            while (true)
            {
                FilePath file = this.AppendFile(new FilePath(Guid.NewGuid()
                                                                 .ToString("N") + DirectoryPath.TemporaryExtension));

                if (file.Exists)
                {
                    continue;
                }

                file.CreateNew();
                return file;
            }
        }

        /// <summary>
        ///     Creates an absolute directory path out of this directory path relative to a specified root path.
        /// </summary>
        /// <param name="root"> The root path. </param>
        /// <returns>
        ///     The absolute directory path using <paramref name="root" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If this directory path is already absolute, nothing is done and the same directory path is returned.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="root" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="root" /> is not a rooted path. </exception>
        public DirectoryPath MakeAbsoluteFrom (DirectoryPath root)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (!root.IsRooted)
            {
                throw new InvalidPathArgumentException(nameof(root));
            }

            if (this.IsRooted)
            {
                return this;
            }

            return new DirectoryPath(PathProperties.MakeAbsolute(root.PathInternal, this.PathInternal));
        }

        /// <summary>
        ///     Creates a relative directory path out of this directory path relative to a specified root path.
        /// </summary>
        /// <param name="root"> The root path. </param>
        /// <returns>
        ///     The relative directory path relative to <paramref name="root" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If this directory path is already relative, nothing is done and the same directory path is returned.
        ///     </para>
        ///     <note type="important">
        ///         If this directory path and <paramref name="root" /> do not have the same root, the same value as this directory path is returned, still being an absolute path.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="root" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="root" /> is not a rooted path. </exception>
        public DirectoryPath MakeRelativeTo (DirectoryPath root)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (!root.IsRooted)
            {
                throw new InvalidPathArgumentException(nameof(root));
            }

            if (!this.IsRooted)
            {
                return this;
            }

            return new DirectoryPath(PathProperties.MakeRelative(root.PathInternal, this.PathInternal));
        }

        /// <summary>
        ///     Verifies that the directory path is a &quot;real&quot; usable directory.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If the directory path is not a real usable directory, <see cref="InvalidOperationException" /> is thrown.
        ///     </para>
        ///     <para>
        ///         <see cref="IsRealDirectory" /> is used to determine whether it is a real usable directory.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> This directory is not a real usable directory. </exception>
        public void VerifyRealDirectory ()
        {
            if (!this.IsRealDirectory)
            {
                throw new InvalidOperationException("The directory is not a real usable directory.");
            }
        }

        #endregion




        #region Overrides

        /// <inheritdoc />
        protected override PathString ChangeTypeInternal (PathType type)
        {
            return this.ChangeType(type);
        }

        /// <inheritdoc />
        protected override PathString CloneInternal ()
        {
            return this.Clone();
        }

        #endregion




        #region Interface: ICloneable<DirectoryPath>

        /// <inheritdoc />
        public DirectoryPath Clone ()
        {
            return new DirectoryPath(this.GetPathProperties());
        }

        #endregion




        #region Interface: IComparable<DirectoryPath>

        /// <inheritdoc cref="PathString.CompareTo(PathString)" />
        public int CompareTo (DirectoryPath other)
        {
            return this.CompareTo((PathString)other);
        }

        #endregion




        #region Interface: IEquatable<DirectoryPath>

        /// <inheritdoc cref="PathString.Equals(PathString)" />
        public bool Equals (DirectoryPath other)
        {
            return this.Equals((PathString)other);
        }

        #endregion
    }
}
