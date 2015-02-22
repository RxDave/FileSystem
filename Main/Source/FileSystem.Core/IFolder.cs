using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace FileSystem
{
  /// <summary>
  /// Represents a folder on any platform.
  /// </summary>
  [ContractClass(typeof(IFolderContract))]
  public interface IFolder
  {
    /// <summary>
    /// Gets the name of the folder.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the full path and name of the folder.
    /// </summary>
    string FullPath { get; }

    /// <summary>
    /// Gets a value indicating whether the folder is read-only.
    /// </summary>
    bool IsReadOnly { get; }

    /// <summary>
    /// Gets a value indicating whether the folder is archived.
    /// </summary>
    bool IsArchive { get; }

    /// <summary>
    /// Gets a value indicating whether the folder is temporary.
    /// </summary>
    bool IsTemporary { get; }

    /// <summary>
    /// Gets the creation date of the folder.
    /// </summary>
    DateTimeOffset DateCreated { get; }

    /// <summary>
    /// Creates a new file with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the file to be created.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFile"/> object representing the new file.</returns>
    Task<IFile> CreateFileAsync(string name);

    /// <summary>
    /// Creates or replaces a file with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the file to be created or replaced.</param>
    /// <param name="replace">Indicates whether an existing file of the same <paramref name="name"/> must be replaced.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFile"/> object representing the created or replaced file.</returns>
    Task<IFile> CreateFileAsync(string name, bool canReplace);

    /// <summary>
    /// Gets or creates a file with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the file to be retrieved or created.</param> 
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFile"/> object representing the retrieved or created file.</returns>
    Task<IFile> GetOrCreateFileAsync(string name);

    /// <summary>
    /// Gets an existing file with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the file to be retrieved.</param> 
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFile"/> object representing the retrieved file.</returns>
    Task<IFile> GetFileAsync(string name);

    /// <summary>
    /// Gets all of the files in the folder.
    /// </summary>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IEnumerable{T}"/> of <see cref="IFile"/> objects representing each file in the folder.</returns> 
    Task<IEnumerable<IFile>> GetFilesAsync();

    /// <summary>
    /// Gets all of the files in the folder and all of its subfolders, at any depth.
    /// </summary>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IEnumerable{T}"/> of <see cref="IFile"/> objects representing each file in the folder or a descendant folder.</returns> 
    Task<IEnumerable<IFile>> GetFilesDeepAsync();

    /// <summary>
    /// Creates a new subfolder with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the subfolder to be created.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFolder"/> object representing the new subfolder.</returns>
    Task<IFolder> CreateFolderAsync(string name);

    /// <summary>
    /// Creates or replaces a subfolder with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the subfolder to be created or replaced.</param>
    /// <param name="replace">Indicates whether an existing subfolder of the same <paramref name="name"/> must be replaced.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFolder"/> object representing the created or replaced subfolder.</returns>
    Task<IFolder> CreateFolderAsync(string name, bool canReplace);

    /// <summary>
    /// Gets or creates a subfolder with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the subfolder to be retrieved or created.</param> 
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFolder"/> object representing the retrieved or created subfolder.</returns>
    Task<IFolder> GetOrCreateFolderAsync(string name);

    /// <summary>
    /// Gets an existing subfolder with the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">Name of the subfolder to be retrieved.</param> 
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFolder"/> object representing the retrieved subfolder.</returns>
    Task<IFolder> GetFolderAsync(string name);

    /// <summary>
    /// Gets all of the subfolders in the folder.
    /// </summary>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IEnumerable{T}"/> of <see cref="IFolder"/> objects representing each subfolder in the folder.</returns> 
    Task<IEnumerable<IFolder>> GetFoldersAsync();

    /// <summary>
    /// Gets a value indicating whether the folder exists.
    /// </summary>
    /// <returns><see langword="True"/> if the folder exists; otherwise, <see langword="false"/>.</returns>
    Task<bool> ExistsAsync();

    /// <summary>
    /// Renames the folder and refreshes this folder object to reference the new folder name.
    /// </summary>
    /// <param name="newName">The new name of the folder.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous renaming operation.</returns>
    Task RenameAsync(string newName);

    /// <summary>
    /// Deletes the folder.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous deletion.</returns>
    Task DeleteAsync();
  }

  [ContractClassFor(typeof(IFolder))]
  internal abstract class IFolderContract : IFolder
  {
    public string Name
    {
      get
      {
        Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));
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

    public Task<IFile> CreateFileAsync(string name)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(name));
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);
      return null;
    }

    public Task<IFile> CreateFileAsync(string name, bool canReplace)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(name));
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);
      return null;
    }

    public Task<IFile> GetOrCreateFileAsync(string name)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(name));
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);
      return null;
    }

    public Task<IFile> GetFileAsync(string name)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(name));
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);
      return null;
    }

    public Task<IEnumerable<IFile>> GetFilesAsync()
    {
      Contract.Ensures(Contract.Result<Task<IEnumerable<IFile>>>() != null);
      return null;
    }

    public Task<IEnumerable<IFile>> GetFilesDeepAsync()
    {
      Contract.Ensures(Contract.Result<Task<IEnumerable<IFile>>>() != null);
      return null;
    }

    public Task<IFolder> CreateFolderAsync(string name)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(name));
      Contract.Ensures(Contract.Result<Task<IFolder>>() != null);
      return null;
    }

    public Task<IFolder> CreateFolderAsync(string name, bool canReplace)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(name));
      Contract.Ensures(Contract.Result<Task<IFolder>>() != null);
      return null;
    }

    public Task<IFolder> GetOrCreateFolderAsync(string name)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(name));
      Contract.Ensures(Contract.Result<Task<IFolder>>() != null);
      return null;
    }

    public Task<IFolder> GetFolderAsync(string name)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(name));
      Contract.Ensures(Contract.Result<Task<IFolder>>() != null);
      return null;
    }

    public Task<IEnumerable<IFolder>> GetFoldersAsync()
    {
      Contract.Ensures(Contract.Result<Task<IEnumerable<IFolder>>>() != null);
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
  }
}