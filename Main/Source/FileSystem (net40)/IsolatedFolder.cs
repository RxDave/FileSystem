using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;
using FileSystem.Properties;

namespace FileSystem
{
  internal sealed class IsolatedFolder : IFolder
  {
    public string Name
    {
      get
      {
        return Path.GetFileName(path);
      }
    }

    public string FullPath
    {
      get
      {
        return path;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        throw new NotSupportedException();
      }
    }

    public bool IsArchive
    {
      get
      {
        throw new NotSupportedException();
      }
    }

    public bool IsTemporary
    {
      get
      {
        throw new NotSupportedException();
      }
    }

    public DateTimeOffset DateCreated
    {
      get
      {
        return root.GetCreationTime(path);
      }
    }

    private readonly IsolatedStorageFile root;
    private string path;

    public IsolatedFolder(IsolatedStorageFile root, string path)
    {
      Contract.Requires(root != null);
      Contract.Requires(!string.IsNullOrWhiteSpace(path));

      this.root = root;
      this.path = path;
    }

    [ContractInvariantMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
    private void ObjectInvariant()
    {
      Contract.Invariant(root != null);
      Contract.Invariant(!string.IsNullOrWhiteSpace(path));
    }

    public Task<IFile> CreateFileAsync(string name)
    {
      return CreateFileAsync(name, canReplace: false);
    }

    public Task<IFile> CreateFileAsync(string name, bool canReplace)
    {
      if (name.IndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }) > -1)
      {
        throw new ArgumentException(Errors.InvalidFileOrFolderName, "name");
      }

      name = Path.Combine(path, name);

      new IsolatedStorageFileStream(name, canReplace ? FileMode.Create : FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, root).Dispose();

      return TaskEx.FromResult<IFile>(new IsolatedFile(root, name));
    }

    public Task<IFile> GetOrCreateFileAsync(string name)
    {
      if (name.IndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }) > -1)
      {
        throw new ArgumentException(Errors.InvalidFileOrFolderName, "name");
      }

      name = Path.Combine(path, name);

      new IsolatedStorageFileStream(name, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, root).Dispose();

      return TaskEx.FromResult<IFile>(new IsolatedFile(root, name));
    }

    public Task<IFile> GetFileAsync(string name)
    {
      if (name.IndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }) > -1)
      {
        throw new ArgumentException(Errors.InvalidFileOrFolderName, "name");
      }

      name = Path.Combine(path, name);

      root.OpenFile(name, FileMode.Open, FileAccess.ReadWrite, FileShare.None).Dispose();

      return TaskEx.FromResult<IFile>(new IsolatedFile(root, name));
    }

    public Task<IEnumerable<IFile>> GetFilesAsync()
    {
      throw new NotSupportedException();
    }

    public Task<IEnumerable<IFile>> GetFilesDeepAsync()
    {
      throw new NotSupportedException();
    }

    public Task<IFolder> CreateFolderAsync(string name)
    {
      return CreateFolderAsync(name, canReplace: false);
    }

    public Task<IFolder> CreateFolderAsync(string name, bool canReplace)
    {
      if (name.IndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }) > -1)
      {
        throw new ArgumentException(Errors.InvalidFileOrFolderName, "name");
      }

      name = Path.Combine(path, name);

      if (canReplace && root.DirectoryExists(name))
      {
        root.DeleteDirectory(name);
      }

      root.CreateDirectory(name);

      return TaskEx.FromResult<IFolder>(new IsolatedFolder(root, name));
    }

    public Task<IFolder> GetOrCreateFolderAsync(string name)
    {
      if (name.IndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }) > -1)
      {
        throw new ArgumentException(Errors.InvalidFileOrFolderName, "name");
      }

      name = Path.Combine(path, name);

      root.CreateDirectory(name);

      return TaskEx.FromResult<IFolder>(new IsolatedFolder(root, name));
    }

    public Task<IFolder> GetFolderAsync(string name)
    {
      if (name.IndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }) > -1)
      {
        throw new ArgumentException(Errors.InvalidFileOrFolderName, "name");
      }

      name = Path.Combine(path, name);

      if (!root.DirectoryExists(name))
      {
        throw new DirectoryNotFoundException(Errors.DirectoryNotFound);
      }

      return TaskEx.FromResult<IFolder>(new IsolatedFolder(root, name));
    }

    public Task<IEnumerable<IFolder>> GetFoldersAsync()
    {
      throw new NotSupportedException();
    }

    public Task<bool> ExistsAsync()
    {
      return TaskEx.FromResult(root.DirectoryExists(path));
    }

    public Task RenameAsync(string newName)
    {
      var newPath = Path.Combine(Path.GetDirectoryName(path) ?? string.Empty, newName);

      root.MoveDirectory(path, newPath);

      path = newPath;

      return TaskEx.FromResult(true);
    }

    public Task DeleteAsync()
    {
      root.DeleteDirectory(path);

      return TaskEx.FromResult(true);
    }
  }
}