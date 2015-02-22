using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading.Tasks;

namespace FileSystem
{
  internal sealed class IsolatedFile : IFile
  {
    public string Name
    {
      get
      {
        return Path.GetFileName(path);
      }
    }

    public string Extension
    {
      get
      {
        return Path.GetExtension(path);
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

    public IsolatedFile(IsolatedStorageFile root, string path)
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

    public Task<IFile> CopyAsync(IFolder destination, bool canReplace)
    {
      var file = new IsolatedFile(root, Path.Combine(destination.FullPath, Name));

      root.CopyFile(path, file.FullPath, canReplace);

      return TaskEx.FromResult<IFile>(file);
    }

    public Task<IFile> CopyAsync(IFolder destination, string newName, bool canReplace)
    {
      var file = new IsolatedFile(root, Path.Combine(destination.FullPath, newName));

      root.CopyFile(path, file.FullPath, canReplace);

      return TaskEx.FromResult<IFile>(file);
    }

    public Task<IFile> MoveAsync(IFolder destination, bool canReplace)
    {
      return MoveAsync(destination.FullPath, Name, canReplace);
    }

    public Task<IFile> MoveAsync(IFolder destination, string newName, bool canReplace)
    {
      return MoveAsync(destination.FullPath, newName, canReplace);
    }

    private Task<IFile> MoveAsync(string destination, string newName, bool canReplace)
    {
      Contract.Requires(destination != null);
      Contract.Requires(!string.IsNullOrWhiteSpace(newName));
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);

      var file = new IsolatedFile(root, Path.Combine(destination, newName));

      if (canReplace && root.FileExists(file.FullPath))
      {
        root.DeleteFile(file.FullPath);
      }

      root.MoveFile(path, file.FullPath);

      return TaskEx.FromResult<IFile>(file);
    }

    public Task<bool> ExistsAsync()
    {
      return TaskEx.FromResult(root.FileExists(path));
    }

    public async Task RenameAsync(string newName)
    {
      var file = await MoveAsync(Path.GetDirectoryName(path) ?? string.Empty, newName, canReplace: false).ConfigureAwait(false);

      path = file.FullPath;
    }

    public Task DeleteAsync()
    {
      root.DeleteFile(path);

      return TaskEx.FromResult(true);
    }

    public Task<Stream> OpenAsync(bool forWriting)
    {
      return TaskEx.FromResult<Stream>(root.OpenFile(path, FileMode.Open, forWriting ? FileAccess.ReadWrite : FileAccess.Read, FileShare.None));
    }

    public Task<Stream> OpenSequentialReadAsync()
    {
      return OpenAsync(forWriting: false);
    }
  }
}