using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace FileSystem
{
  /// <summary>
  /// Represents a file on any platform.
  /// </summary>
  [ContractClass(typeof(IFileContract))]
  public interface IFile
  {
    /// <summary>
    /// Gets the name of the file including any extension.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the file extension including a leading period, or an empty string if there is no extension.
    /// </summary>
    string Extension { get; }

    /// <summary>
    /// Gets the full path and name of the file including any extension.
    /// </summary>
    string FullPath { get; }

    /// <summary>
    /// Gets a value indicating whether the file is read-only.
    /// </summary>
    bool IsReadOnly { get; }

    /// <summary>
    /// Gets a value indicating whether the file is archived.
    /// </summary>
    bool IsArchive { get; }

    /// <summary>
    /// Gets a value indicating whether the file is temporary.
    /// </summary>
    bool IsTemporary { get; }

    /// <summary>
    /// Gets the creation date of the file.
    /// </summary>
    DateTimeOffset DateCreated { get; }

    /// <summary>
    /// Copies the file to the specified location.
    /// </summary>
    /// <param name="destination">The folder to which the file will be copied.</param>
    /// <param name="replace">Indicates whether an existing file should be replaced.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFile"/> object representing the copied file in the specified location.</returns>
    Task<IFile> CopyAsync(IFolder destination, bool canReplace);

    /// <summary>
    /// Copies the file to the specified location and with the specified name.
    /// </summary>
    /// <param name="destination">The folder to which the file will be copied.</param>
    /// <param name="newName">The name of the file at the copied location.</param>
    /// <param name="replace">Indicates whether an existing file should be replaced.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFile"/> object representing the copied file in the specified location and with the specified name.</returns>
    Task<IFile> CopyAsync(IFolder destination, string newName, bool canReplace);

    /// <summary>
    /// Moves the file to the specified location.
    /// </summary>
    /// <param name="destination">The folder to which the file will be moved.</param>
    /// <param name="replace">Indicates whether an existing file should be replaced.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFile"/> object representing the moved file in the specified location.</returns>
    Task<IFile> MoveAsync(IFolder destination, bool canReplace);

    /// <summary>
    /// Moves the file to the specified location and with the specified name.
    /// </summary>
    /// <param name="destination">The folder to which the file will be moved.</param>
    /// <param name="newName">The name of the file at the new location.</param>
    /// <param name="replace">Indicates whether an existing file should be replaced.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFile"/> object representing the moved file in the specified location and with the specified name.</returns>
    Task<IFile> MoveAsync(IFolder destination, string newName, bool canReplace);

    /// <summary>
    /// Gets a value indicating whether the file exists.
    /// </summary>
    /// <returns><see langword="True"/> if the file exists; otherwise, <see langword="false"/>.</returns>
    Task<bool> ExistsAsync();

    /// <summary>
    /// Renames the file and refreshes this file object to reference the new file name.
    /// </summary>
    /// <param name="newName">The new name of the file.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous renaming operation.</returns>
    Task RenameAsync(string newName);

    /// <summary>
    /// Deletes the file.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous deletion.</returns>
    Task DeleteAsync();

    /// <summary>
    /// Opens the file for reading and writing, or just reading.
    /// </summary>
    /// <param name="forWriting">Indicate whether the returned <see cref="Stream"/> must be writable.</param>
    /// <returns>A <see cref="Task{T}"/> containing the opened <see cref="Stream"/>.</returns>
    Task<Stream> OpenAsync(bool forWriting);

    /// <summary>
    /// Opens the file for efficient sequential reading.
    /// </summary>
    /// <returns>A <see cref="Task{T}"/> containing the opened <see cref="Stream"/>.</returns>
    Task<Stream> OpenSequentialReadAsync();
  }

  [ContractClassFor(typeof(IFile))]
  internal abstract class IFileContract : IFile
  {
    public string Name
    {
      get
      {
        Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
        return null;
      }
    }

    public string Extension
    {
      get
      {
        Contract.Ensures(Contract.Result<string>() != null);
        return null;
      }
    }

    public string FullPath
    {
      get
      {
        Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
        return null;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return default(bool);
      }
    }

    public bool IsArchive
    {
      get
      {
        return default(bool);
      }
    }

    public bool IsTemporary
    {
      get
      {
        return default(bool);
      }
    }

    public DateTimeOffset DateCreated
    {
      get
      {
        return default(DateTimeOffset);
      }
    }

    public Task<IFile> CopyAsync(IFolder destination, bool canReplace)
    {
      Contract.Requires(destination != null);
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);
      return null;
    }

    public Task<IFile> CopyAsync(IFolder destination, string newName, bool canReplace)
    {
      Contract.Requires(destination != null);
      Contract.Requires(!string.IsNullOrWhiteSpace(newName));
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);
      return null;
    }

    public Task<IFile> MoveAsync(IFolder destination, bool canReplace)
    {
      Contract.Requires(destination != null);
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);
      return null;
    }

    public Task<IFile> MoveAsync(IFolder destination, string newName, bool canReplace)
    {
      Contract.Requires(destination != null);
      Contract.Requires(!string.IsNullOrWhiteSpace(newName));
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);
      return null;
    }

    public Task<bool> ExistsAsync()
    {
      Contract.Ensures(Contract.Result<Task<bool>>() != null);
      return null;
    }

    public Task RenameAsync(string newName)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(newName));
      Contract.Ensures(Contract.Result<Task>() != null);
      return null;
    }

    public Task DeleteAsync()
    {
      Contract.Ensures(Contract.Result<Task>() != null);
      return null;
    }

    public Task<Stream> OpenAsync(bool forWriting)
    {
      Contract.Ensures(Contract.Result<Task<Stream>>() != null);
      return null;
    }

    public Task<Stream> OpenSequentialReadAsync()
    {
      Contract.Ensures(Contract.Result<Task<Stream>>() != null);
      return null;
    }
  }
}