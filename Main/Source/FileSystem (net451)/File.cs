using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;

namespace FileSystem
{
  internal sealed class File : IFile
  {
    public string Name
    {
      get
      {
        return file.Name;
      }
    }

    public string FullPath
    {
      get
      {
        return file.FullName;
      }
    }

    public string Extension
    {
      get
      {
        return file.Extension;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return file.Attributes.HasFlag(FileAttributes.ReadOnly);
      }
    }

    public bool IsArchive
    {
      get
      {
        return file.Attributes.HasFlag(FileAttributes.Archive);
      }
    }

    public bool IsTemporary
    {
      get
      {
        return file.Attributes.HasFlag(FileAttributes.Temporary);
      }
    }

    public DateTimeOffset DateCreated
    {
      get
      {
        return file.CreationTime;
      }
    }

    private const int defaultBufferSize = 4096;
    private FileInfo file;

    public File(FileInfo file)
    {
      Contract.Requires(file != null);

      this.file = file;
    }

    [ContractInvariantMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
    private void ObjectInvariant()
    {
      Contract.Invariant(file != null);
    }

    public async Task<IFile> CopyAsync(IFolder destination, bool canReplace)
    {
      var result = new File(new FileInfo(Path.Combine(destination.FullPath, Name)));

      using (var source = OpenSequentialRead())
      using (var target = result.OpenSequentialWrite(canReplace))
      {
        await source.CopyToAsync(target).ConfigureAwait(false);
      }

      return result;
    }

    public async Task<IFile> CopyAsync(IFolder destination, string newName, bool canReplace)
    {
      var result = new File(new FileInfo(Path.Combine(destination.FullPath, newName)));

      using (var source = OpenSequentialRead())
      using (var target = result.OpenSequentialWrite(canReplace))
      {
        await source.CopyToAsync(target).ConfigureAwait(false);
      }

      return result;
    }

    public async Task<IFile> MoveAsync(IFolder destination, bool canReplace)
    {
      var result = await CopyAsync(destination, canReplace).ConfigureAwait(false);

      file.Delete();

      return result;
    }

    public async Task<IFile> MoveAsync(IFolder destination, string newName, bool canReplace)
    {
      var result = await CopyAsync(destination, newName, canReplace).ConfigureAwait(false);

      file.Delete();

      return result;
    }

    public Task<bool> ExistsAsync()
    {
      return Task.FromResult(file.Exists);
    }

    public async Task RenameAsync(string newName)
    {
      var result = (File)await MoveAsync(new Folder(file.Directory), newName, canReplace: false).ConfigureAwait(false);

      file = result.file;
    }

    public Task DeleteAsync()
    {
      file.Delete();
      file.Refresh();

      return Task.FromResult(true);
    }

    public Task<Stream> OpenAsync(bool forWriting)
    {
      return Task.FromResult<Stream>(new FileStream(
        file.FullName,
        FileMode.Open,
        forWriting ? FileAccess.ReadWrite : FileAccess.Read,
        FileShare.None,
        defaultBufferSize,
        FileOptions.Asynchronous | FileOptions.RandomAccess));
    }

    public Task<Stream> OpenSequentialReadAsync()
    {
      return Task.FromResult<Stream>(OpenSequentialRead());
    }

    private Stream OpenSequentialRead()
    {
      Contract.Ensures(Contract.Result<Stream>() != null);

      return new FileStream(
        file.FullName,
        FileMode.Open,
        FileAccess.Read,
        FileShare.None,
        defaultBufferSize,
        FileOptions.Asynchronous | FileOptions.SequentialScan);
    }

    private Stream OpenSequentialWrite(bool canReplace)
    {
      Contract.Ensures(Contract.Result<Stream>() != null);

      return new FileStream(
        file.FullName,
        canReplace ? FileMode.Create : FileMode.CreateNew,
        FileAccess.Write,
        FileShare.None,
        defaultBufferSize,
        FileOptions.Asynchronous | FileOptions.SequentialScan);
    }
  }
}