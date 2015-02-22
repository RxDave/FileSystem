using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using FileSystem.Properties;

namespace FileSystem
{
  internal sealed class IsolatedFolderRoot : IFolder
  {
    public string Name
    {
      get
      {
        throw new NotSupportedException();
      }
    }

    public string FullPath
    {
      get
      {
        throw new NotSupportedException();
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
        throw new NotSupportedException();
      }
    }

    private readonly IsolatedStorageFile root;

    public IsolatedFolderRoot(IsolatedStorageFile root)
    {
      Contract.Requires(root != null);

      this.root = root;
    }

    [ContractInvariantMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
    private void ObjectInvariant()
    {
      Contract.Invariant(root != null);
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

      new IsolatedStorageFileStream(name, canReplace ? FileMode.Create : FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None, root).Dispose();

      return TaskEx.FromResult<IFile>(new IsolatedFile(root, name));
    }

    public Task<IFile> GetOrCreateFileAsync(string name)
    {
      if (name.IndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }) > -1)
      {
        throw new ArgumentException(Errors.InvalidFileOrFolderName, "name");
      }

      new IsolatedStorageFileStream(name, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, root).Dispose();

      return TaskEx.FromResult<IFile>(new IsolatedFile(root, name));
    }

    public Task<IFile> GetFileAsync(string name)
    {
      return TaskEx.FromResult(
        (from file in root.GetFileNames(name)
         select (IFile)new IsolatedFile(root, file))
         .Single());
    }

    public Task<IEnumerable<IFile>> GetFilesAsync()
    {
      return TaskEx.FromResult(
        from file in root.GetFileNames()
        select (IFile)new IsolatedFile(root, file));
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

      root.CreateDirectory(name);

      return TaskEx.FromResult<IFolder>(new IsolatedFolder(root, name));
    }

    public Task<IFolder> GetFolderAsync(string name)
    {
      if (name.IndexOfAny(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }) > -1)
      {
        throw new ArgumentException(Errors.InvalidFileOrFolderName, "name");
      }

      if (!root.DirectoryExists(name))
      {
        throw new DirectoryNotFoundException(Errors.DirectoryNotFound);
      }

      return TaskEx.FromResult<IFolder>(new IsolatedFolder(root, name));
    }

    public Task<IEnumerable<IFolder>> GetFoldersAsync()
    {
      return TaskEx.FromResult(
        from folder in root.GetDirectoryNames()
        select (IFolder)new IsolatedFolder(root, folder));
    }

    public Task<bool> ExistsAsync()
    {
      return TaskEx.FromResult(true);
    }

    public Task RenameAsync(string newName)
    {
      throw new NotSupportedException();
    }

    public Task DeleteAsync()
    {
      throw new NotSupportedException();
    }
  }
}