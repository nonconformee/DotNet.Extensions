using System;
using System.IO;
using System.Security;
using System.Text;

using RI.Utilities.Collections;
using RI.Utilities.Exceptions;
using RI.Utilities.ObjectModel;
using RI.Utilities.Text;




namespace RI.Utilities.Paths
{
    /// <summary>
    ///     Describes a path to a file.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <see cref="FilePath" /> uses <see cref="PathProperties" /> to extract and store path information.
    ///         See <see cref="PathProperties" /> for more details about the supported types of file paths.
    ///     </para>
    ///     <para>
    ///         <see cref="FilePath" /> provides more file path specific functionalities compared to <see cref="string" /> and offers a more consistent way of working with paths than <see cref="Path" />.
    ///         It can be implicitly converted to a <see cref="string" /> to work seamless with APIs using <see cref="string" /> for paths.
    ///     </para>
    ///     <para>
    ///         See <see cref="PathProperties" /> for possible format strings for <see cref="PathString.ToString(string)" />.
    ///     </para>
    /// </remarks>
    /// <threadsafety static="false" instance="false" />
    /// <example>
    ///     <code language="cs">
    /// <![CDATA[
    /// // get a temporary file
    /// var tempFile = FilePath.GetTempFile();
    /// 
    /// // write some text
    /// tempFile.WriteText("some text");
    /// 
    /// // read some text
    /// string text = tempFile.ReadText();
    /// 
    /// // delete temporary file
    /// tempFile.Delete();
    /// 
    /// // create some paths
    /// var gamePath = new FilePath(@"C:\Program Files (x86)\Steam\steamapps\common\MyGame\MyGame.exe");
    /// var modPath  = new FilePath(@"C:\Program Files (x86)\Steam\steamapps\common\MyGame\data\mods\mods.ini");
    /// 
    /// // get directories
    /// var gameDir  = gamePath.Directory;                       // C:\Program Files (x86)\Steam\steamapps\common\MyGame
    /// var modDir   = modPath.Directory;                        // C:\Program Files (x86)\Steam\steamapps\common\MyGame\data\mods
    /// var otherDir = gameDir.AppendDirectory("other", "dir");  // C:\Program Files (x86)\Steam\steamapps\common\MyGame\other\dir
    /// var modRel   = modDir.MakeRelativeTo(otherDir);          // ..\..\data\mods
    /// ]]>
    /// </code>
    /// </example>
    [Serializable,]
    public sealed class FilePath : PathString, ICloneable<FilePath>, IEquatable<FilePath>, IComparable<FilePath>
    {
        #region Static Methods

        /// <summary>
        ///     Creates a temporary zero-byte file and returns its path.
        /// </summary>
        /// <returns>
        ///     The path to the newly created temporary file.
        /// </returns>
        public static FilePath GetTempFile ()
        {
            return new FilePath(Path.GetTempFileName(), false, false, null);
        }

        /// <summary>
        ///     Implicit conversion of a <see cref="string" /> to <see cref="FilePath" />.
        /// </summary>
        /// <param name="path"> The path to convert to a file path. </param>
        /// <returns>
        ///     The file path.
        /// </returns>
        public static implicit operator FilePath (string path)
        {
            if (path == null)
            {
                return null;
            }

            return new FilePath(path);
        }

        #endregion




        #region Instance Constructor/Destructor

        /// <summary>
        ///     Creates a new instance of <see cref="FilePath" />.
        /// </summary>
        /// <param name="path"> The path. </param>
        /// <remarks>
        ///     <para>
        ///         Using this constructor, wildcards and relative paths are allowed and the type of the path is assumed to be of the same type as used on the current system.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="path" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="path" /> is not a valid file path. </exception>
        public FilePath (string path)
            : this(PathProperties.FromPath(path, true, true, PathProperties.GetPathType(path, false) ?? PathProperties.GetSystemType())) { }

        /// <summary>
        ///     Creates a new instance of <see cref="FilePath" />.
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
        /// <exception cref="InvalidPathArgumentException"> <paramref name="path" /> is not a valid file path. </exception>
        public FilePath (string path, bool allowWildcards, bool allowRelatives, PathType? assumedType)
            : this(PathProperties.FromPath(path, allowWildcards, allowRelatives, assumedType)) { }

        /// <summary>
        ///     Creates a new instance of <see cref="FilePath" />.
        /// </summary>
        /// <param name="path"> The <see cref="PathProperties" /> object which describes the path. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="path" /> is null. </exception>
        public FilePath (PathProperties path)
            : base(path)
        {
            this.FileNameParts = path.Name.Split(PathProperties.FileExtensionSeparator);
            this.FileName = path.Name;

            if (this.FileNameParts.Length == 1)
            {
                this.ExtensionWithoutDot = null;
                this.ExtensionWithDot = null;
                this.FileNameWithoutExtension = this.FileName;
            }
            else
            {
                this.ExtensionWithoutDot = this.FileNameParts[this.FileNameParts.Length - 1];
                this.ExtensionWithDot = PathProperties.FileExtensionSeparator + this.ExtensionWithoutDot;

                this.FileNameWithoutExtension = this.FileNameParts.ToList(0, this.FileNameParts.Length - 1)
                                                    .Join(PathProperties.FileExtensionSeparator);
            }
        }

        #endregion




        #region Instance Properties/Indexer

        /// <summary>
        ///     Gets the directory path of the file.
        /// </summary>
        /// <value>
        ///     The directory path of the file or null if the file path does not specify a directory.
        /// </value>
        public DirectoryPath Directory
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

        /// <summary>
        ///     Gets whether the file exists.
        /// </summary>
        /// <value>
        ///     true if the file exists, false otherwise.
        /// </value>
        /// <remarks>
        ///     <note type="note">
        ///         <see cref="Exists" /> does not throw exceptions besides <see cref="InvalidOperationException" />.
        ///         For example, if the file exists but the user does not have access permissions, the file is not of a compatible path type used on the current system, etc., false is returned.
        ///     </note>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        public bool Exists
        {
            get
            {
                this.VerifyRealFile();

                try
                {
                    return File.Exists(this);
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///     Gets the extension of the file name (with the dot).
        /// </summary>
        /// <value>
        ///     The extension of the file name (with the dot) or null if the file name does not have an extension.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         If the file name ends with a dot, this property has the value of an empty string.
        ///     </para>
        /// </remarks>
        public string ExtensionWithDot { get; private set; }

        /// <summary>
        ///     Gets the extension of the file name (without the dot).
        /// </summary>
        /// <value>
        ///     The extension of the file name (without the dot) or null if the file name does not have an extension.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         If the file name ends with a dot, this property has the value of an empty string.
        ///     </para>
        /// </remarks>
        public string ExtensionWithoutDot { get; private set; }

        /// <summary>
        ///     Gets the file name of the file path.
        /// </summary>
        /// <value>
        ///     The file name of the file path, including name and extension.
        /// </value>
        public string FileName { get; private set; }

        /// <summary>
        ///     Gets the file name without its extension.
        /// </summary>
        /// <value>
        ///     The file name without its extension.
        /// </value>
        public string FileNameWithoutExtension { get; private set; }

        /// <summary>
        ///     Gets whether the file path is a &quot;real&quot; usable file.
        /// </summary>
        /// <value>
        ///     true if the file path is a real usable file, false otherwise.
        /// </value>
        /// <remarks>
        ///     <para>
        ///         A real usable file is a file which has not wildcards.
        ///     </para>
        /// </remarks>
        public bool IsRealFile => !this.HasWildcards;

        /// <summary>
        ///     Gets the size of the file in bytes.
        /// </summary>
        /// <value>
        ///     The size of the file in bytes or null if the file does not exist or cannot be accessed.
        /// </value>
        /// <remarks>
        ///     <note type="note">
        ///         <see cref="Size" /> does not throw exceptions besides <see cref="InvalidOperationException" />.
        ///         For example, if the file exists but the user does not have access permissions, the file is not of a compatible path type used on the current system, etc., null is returned.
        ///     </note>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        public long? Size
        {
            get
            {
                this.VerifyRealFile();

                try
                {
                    FileInfo fi = new FileInfo(this);
                    return fi.Length;
                }
                catch
                {
                    return null;
                }
            }
        }

        private string[] FileNameParts { get; set; }

        #endregion




        #region Instance Methods

        /// <summary>
        ///     Creates a new file path with this file name but another directory.
        /// </summary>
        /// <param name="directory"> The new directory path. </param>
        /// <returns>
        ///     The new file path.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="directory" /> is null. </exception>
        public FilePath ChangeDirectory (DirectoryPath directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(nameof(directory));
            }

            return directory.AppendFile(new FilePath(this.FileName, this.PathInternal.AllowWildcards, this.PathInternal.AllowRelatives, this.Type));
        }

        /// <summary>
        ///     Creates a new file path with this file name and directory but another extension.
        /// </summary>
        /// <param name="extension"> The new extension (with or without a leading dot). </param>
        /// <returns>
        ///     The new file path.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If <paramref name="extension" /> is an empty string, the resulting file name will have a dot at its end but no extension.
        ///         If <paramref name="extension" /> is null, the extension (including dot) will be removed.
        ///     </para>
        ///     <note type="note">
        ///         All leading dots will be trimmed to a single leading dot when combined with the rest of the file name.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="extension" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> The existing file name (without extension) plus <paramref name="extension" /> do not form a valid new file name. </exception>
        public FilePath ChangeExtension (string extension)
        {
            if (extension != null)
            {
                extension = extension.TrimStart();
                extension = extension.TrimStart(PathProperties.FileExtensionSeparator);
            }

            try
            {
                return this.Directory.AppendFile(new FilePath(extension == null ? this.FileNameWithoutExtension : this.FileNameWithoutExtension + PathProperties.FileExtensionSeparator + extension, this.PathInternal.AllowWildcards, this.PathInternal.AllowRelatives, this.Type));
            }
            catch (InvalidPathArgumentException exception)
            {
                throw new InvalidPathArgumentException(nameof(extension), exception.Message);
            }
        }

        /// <summary>
        ///     Creates a new file path with this directory but another file name (including extension).
        /// </summary>
        /// <param name="fileName"> The new file name including its extension. </param>
        /// <returns>
        ///     The new file path.
        /// </returns>
        /// <exception cref="ArgumentNullException"> <paramref name="fileName" /> is null. </exception>
        /// <exception cref="EmptyStringArgumentException"> <paramref name="fileName" /> is empty. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="fileName" /> is not a valid new file name. </exception>
        public FilePath ChangeFileName (string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (fileName.IsEmptyOrWhitespace())
            {
                throw new EmptyStringArgumentException(nameof(fileName));
            }

            try
            {
                return this.Directory.AppendFile(new FilePath(fileName, this.PathInternal.AllowWildcards, this.PathInternal.AllowRelatives, this.Type));
            }
            catch (InvalidPathArgumentException exception)
            {
                throw new InvalidPathArgumentException(nameof(fileName), exception.Message);
            }
        }

        /// <summary>
        ///     Creates a new file path with this directory but another file name (keeping this extension).
        /// </summary>
        /// <param name="fileNameWithoutExtension"> The new file name without its extension. </param>
        /// <returns>
        ///     The new file path.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If <paramref name="fileNameWithoutExtension" /> is an empty string, the resulting file name will consist of only the extension (including its dot).
        ///         If <paramref name="fileNameWithoutExtension" /> is null, the resulting file name will consist of only the extension (without its dot).
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="fileNameWithoutExtension" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="fileNameWithoutExtension" /> plus the existing extension do not form a valid new file name. </exception>
        public FilePath ChangeFileNameWithoutExtension (string fileNameWithoutExtension)
        {
            try
            {
                return this.Directory.AppendFile(new FilePath(fileNameWithoutExtension == null ? this.ExtensionWithoutDot : fileNameWithoutExtension + PathProperties.FileExtensionSeparator + this.ExtensionWithoutDot, this.PathInternal.AllowWildcards, this.PathInternal.AllowRelatives, this.Type));
            }
            catch (InvalidPathArgumentException exception)
            {
                throw new InvalidPathArgumentException(nameof(fileNameWithoutExtension), exception.Message);
            }
        }

        /// <inheritdoc cref="PathString.ChangeType" />
        public new FilePath ChangeType (PathType type)
        {
            PathProperties properties = this.PathInternal.ChangeType(type);
            return new FilePath(properties);
        }

        /// <summary>
        ///     Copies the file.
        /// </summary>
        /// <param name="destination"> The destination file. </param>
        /// <param name="overwrite"> Specifies whether an already existing destination file should be overwritten (true) or not (false). </param>
        /// <returns>
        ///     true if the file was copied, false otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions, the file is read-only, or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        /// <exception cref="FileNotFoundException"> The source file does not exist. </exception>
        /// <exception cref="DirectoryNotFoundException"> The files directory does not exist or is not available. </exception>
        /// <exception cref="NotSupportedException"> The file is not of a compatible path type used on the current system. </exception>
        public bool Copy (FilePath destination, bool overwrite)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (destination.HasWildcards)
            {
                throw new InvalidPathArgumentException(nameof(destination));
            }

            this.VerifyRealFile();

            if (!overwrite && destination.Exists)
            {
                return false;
            }

            File.Copy(this, destination, true);
            return true;
        }

        /// <summary>
        ///     Copies the file to a directory, keeping its file name.
        /// </summary>
        /// <param name="destination"> The destination directory. </param>
        /// <param name="overwrite"> Specifies whether an already existing destination file should be overwritten (true) or not (false). </param>
        /// <returns>
        ///     true if the file was copied, false otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions, the file is read-only, or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        /// <exception cref="FileNotFoundException"> The source file does not exist. </exception>
        /// <exception cref="DirectoryNotFoundException"> The files directory does not exist or is not available. </exception>
        /// <exception cref="NotSupportedException"> The file is not of a compatible path type used on the current system. </exception>
        public bool Copy (DirectoryPath destination, bool overwrite)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (destination.HasWildcards)
            {
                throw new InvalidPathArgumentException(nameof(destination));
            }

            FilePath target = destination.AppendFile(this.FileName);

            return this.Copy(target, overwrite);
        }

        /// <summary>
        ///     Creates the file if it does not exist with a new file of zero length or keeps an already existing file.
        /// </summary>
        /// <returns>
        ///     true if the file was newly created, false if the file already existed.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions, the file is read-only, or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        /// <exception cref="DirectoryNotFoundException"> The files directory does not exist or is not available. </exception>
        /// <exception cref="NotSupportedException"> The file is not of a compatible path type used on the current system. </exception>
        public bool CreateIfNotExist ()
        {
            this.VerifyRealFile();

            if (this.Exists)
            {
                return false;
            }

            this.CreateNew();
            return true;
        }

        /// <summary>
        ///     Creates the file if it does not exist or overwrites an existing file with a new file of zero length.
        /// </summary>
        /// <returns>
        ///     true if the file was newly created, false if the file already existed and was reset to zero length.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions, the file is read-only, or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        /// <exception cref="DirectoryNotFoundException"> The files directory does not exist or is not available. </exception>
        /// <exception cref="NotSupportedException"> The file is not of a compatible path type used on the current system. </exception>
        public bool CreateNew ()
        {
            this.VerifyRealFile();
            bool result = !this.Exists;
            this.Directory.Create();

            using (File.Create(this))
            {
                return result;
            }
        }

        /// <summary>
        ///     Deletes the file.
        /// </summary>
        /// <returns>
        ///     true if the file existed and was deleted, false otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions, the file is read-only, or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        public bool Delete ()
        {
            this.VerifyRealFile();

            if (!this.Exists)
            {
                return false;
            }

            File.Delete(this);
            return true;
        }

        /// <summary>
        ///     Creates an absolute file path out of this file path relative to a specified root path.
        /// </summary>
        /// <param name="root"> The root path. </param>
        /// <returns>
        ///     The absolute file path using <paramref name="root" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If this file path is already absolute, nothing is done and the same file path is returned.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="root" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="root" /> is not a rooted path. </exception>
        public FilePath MakeAbsoluteFrom (DirectoryPath root)
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

            return new FilePath(PathProperties.MakeAbsolute(root.PathInternal, this.PathInternal));
        }

        /// <summary>
        ///     Creates a relative file path out of this file path relative to a specified root path.
        /// </summary>
        /// <param name="root"> The root path. </param>
        /// <returns>
        ///     The relative file path relative to <paramref name="root" />.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         If this file path is already relative, nothing is done and the same file path is returned.
        ///     </para>
        ///     <note type="important">
        ///         If this file path and <paramref name="root" /> do not have the same root, the same value as this file path is returned, still being an absolute path.
        ///     </note>
        /// </remarks>
        /// <exception cref="ArgumentNullException"> <paramref name="root" /> is null. </exception>
        /// <exception cref="InvalidPathArgumentException"> <paramref name="root" /> is not a rooted path. </exception>
        public FilePath MakeRelativeTo (DirectoryPath root)
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

            return new FilePath(PathProperties.MakeRelative(root.PathInternal, this.PathInternal));
        }

        /// <summary>
        ///     Moves the file.
        /// </summary>
        /// <param name="destination"> The destination file. </param>
        /// <param name="overwrite"> Specifies whether an already existing destination file should be overwritten (true) or not (false). </param>
        /// <returns>
        ///     true if the file was moved, false otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions, the file is read-only, or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        /// <exception cref="FileNotFoundException"> The source file does not exist. </exception>
        /// <exception cref="DirectoryNotFoundException"> The files directory does not exist or is not available. </exception>
        /// <exception cref="NotSupportedException"> The file is not of a compatible path type used on the current system. </exception>
        public bool Move (FilePath destination, bool overwrite)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (destination.HasWildcards)
            {
                throw new InvalidPathArgumentException(nameof(destination));
            }

            this.VerifyRealFile();

            if (!overwrite && destination.Exists)
            {
                return false;
            }

            File.Move(this, destination);
            return true;
        }

        /// <summary>
        ///     Moves the file to a directory, keeping its file name.
        /// </summary>
        /// <param name="destination"> The destination directory. </param>
        /// <param name="overwrite"> Specifies whether an already existing destination file should be overwritten (true) or not (false). </param>
        /// <returns>
        ///     true if the file was moved, false otherwise.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions, the file is read-only, or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        /// <exception cref="FileNotFoundException"> The source file does not exist. </exception>
        /// <exception cref="DirectoryNotFoundException"> The files directory does not exist or is not available. </exception>
        /// <exception cref="NotSupportedException"> The file is not of a compatible path type used on the current system. </exception>
        public bool Move (DirectoryPath destination, bool overwrite)
        {
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            if (destination.HasWildcards)
            {
                throw new InvalidPathArgumentException(nameof(destination));
            }

            FilePath target = destination.AppendFile(this.FileName);

            return this.Move(target, overwrite);
        }

        /// <summary>
        ///     Reads all binary data from the file.
        /// </summary>
        /// <returns>
        ///     All binary data from the file or null if the file does not exist.
        /// </returns>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        public byte[] ReadBytes ()
        {
            this.VerifyRealFile();

            if (!this.Exists)
            {
                return null;
            }

            try
            {
                return File.ReadAllBytes(this);
            }
            catch (SecurityException exception)
            {
                throw new UnauthorizedAccessException(exception.Message, exception);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        ///     Reads all text from the file.
        /// </summary>
        /// <returns>
        ///     All text from the file or null if the file does not exist.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The current systems default encoding (<see cref="Encoding.Default" />) is used.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        public string ReadText ()
        {
            return this.ReadText(null);
        }

        /// <summary>
        ///     Reads all text from the file.
        /// </summary>
        /// <param name="encoding"> The encoding used to read the file. </param>
        /// <returns>
        ///     All text from the file or null if the file does not exist.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The current systems default encoding (<see cref="Encoding.Default" />) is used if <paramref name="encoding" /> is null.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        public string ReadText (Encoding encoding)
        {
            this.VerifyRealFile();

            if (!this.Exists)
            {
                return null;
            }

            try
            {
                return File.ReadAllText(this, encoding ?? Encoding.Default);
            }
            catch (SecurityException exception)
            {
                throw new UnauthorizedAccessException(exception.Message, exception);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        /// <summary>
        ///     Verifies that the file path is a &quot;real&quot; usable file.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         If the file path is not a real usable file, <see cref="InvalidOperationException" /> is thrown.
        ///     </para>
        ///     <para>
        ///         <see cref="IsRealFile" /> is used to determine whether it is a real usable file.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> This directory is not a real usable file. </exception>
        public void VerifyRealFile ()
        {
            if (!this.IsRealFile)
            {
                throw new InvalidOperationException("The file is not a real usable file.");
            }
        }

        /// <summary>
        ///     Writes binary data to the file.
        /// </summary>
        /// <param name="data"> The data to write (can be null to write zero bytes). </param>
        /// <returns>
        ///     true if the file was newly created, false if it already existed and was overwritten with the specified data.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The file is created if it does not already exist.
        ///         If it already exists, the file is overwritten with a new file.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions, the file is read-only, or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        /// <exception cref="DirectoryNotFoundException"> The files directory does not exist or is not available. </exception>
        /// <exception cref="NotSupportedException"> The file is not of a compatible path type used on the current system. </exception>
        public bool WriteBytes (byte[] data)
        {
            this.VerifyRealFile();

            bool result = !this.Exists;

            try
            {
                File.WriteAllBytes(this, data ?? new byte[0]);
            }
            catch (SecurityException exception)
            {
                throw new UnauthorizedAccessException(exception.Message, exception);
            }

            return result;
        }

        /// <summary>
        ///     Writes text to the file.
        /// </summary>
        /// <param name="text"> The text to write. </param>
        /// <returns>
        ///     true if the file was newly created, false if it already existed and was overwritten with the specified text.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The current systems default encoding (<see cref="Encoding.Default" />) is used.
        ///     </para>
        ///     <para>
        ///         The file is created if it does not already exist.
        ///         If it already exists, the file is overwritten with a new file.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions, the file is read-only, or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        /// <exception cref="DirectoryNotFoundException"> The files directory does not exist or is not available. </exception>
        /// <exception cref="NotSupportedException"> The file is not of a compatible path type used on the current system. </exception>
        public bool WriteText (string text)
        {
            return this.WriteText(text, null);
        }

        /// <summary>
        ///     Writes text to the file.
        /// </summary>
        /// <param name="text"> The text to write (can be null to write an empty string). </param>
        /// <param name="encoding"> The encoding used to write the file. </param>
        /// <returns>
        ///     true if the file was newly created, false if it already existed and was overwritten with the specified text.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The current systems default encoding (<see cref="Encoding.Default" />) is used if <paramref name="encoding" /> is null.
        ///     </para>
        ///     <para>
        ///         The file is created if it does not already exist.
        ///         If it already exists, the file is overwritten with a new file.
        ///     </para>
        /// </remarks>
        /// <exception cref="InvalidOperationException"> The file contains wildcards. </exception>
        /// <exception cref="IOException"> The file is in use. </exception>
        /// <exception cref="UnauthorizedAccessException"> The user does not have the required permissions, the file is read-only, or the file is an executable which is in use. </exception>
        /// <exception cref="PathTooLongException"> Although being a valid file path, the file path is too long for the current system to be used. </exception>
        /// <exception cref="DirectoryNotFoundException"> The files directory does not exist or is not available. </exception>
        /// <exception cref="NotSupportedException"> The file is not of a compatible path type used on the current system. </exception>
        public bool WriteText (string text, Encoding encoding)
        {
            this.VerifyRealFile();

            bool result = !this.Exists;

            try
            {
                File.WriteAllText(this, text ?? string.Empty, encoding ?? Encoding.Default);
            }
            catch (SecurityException exception)
            {
                throw new UnauthorizedAccessException(exception.Message, exception);
            }

            return result;
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




        #region Interface: ICloneable<FilePath>

        /// <inheritdoc />
        public FilePath Clone ()
        {
            return new FilePath(this.GetPathProperties());
        }

        #endregion




        #region Interface: IComparable<FilePath>

        /// <inheritdoc cref="PathString.CompareTo(PathString)" />
        public int CompareTo (FilePath other)
        {
            return this.CompareTo((PathString)other);
        }

        #endregion




        #region Interface: IEquatable<FilePath>

        /// <inheritdoc cref="PathString.Equals(PathString)" />
        public bool Equals (FilePath other)
        {
            return this.Equals((PathString)other);
        }

        #endregion
    }
}
