using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

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
        return file.Path;
      }
    }

    public string Extension
    {
      get
      {
        return file.FileType;
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
        return file.DateCreated;
      }
    }

    private StorageFile file;

    public File(StorageFile file)
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
      var folder = await Folder.GetStorageFolderFromAsync(destination).ConfigureAwait(false);

      return new File(await file.CopyAsync(
        folder,
        Name,
        canReplace ? NameCollisionOption.ReplaceExisting : NameCollisionOption.FailIfExists));
    }

    public async Task<IFile> CopyAsync(IFolder destination, string newName, bool canReplace)
    {
      var folder = await Folder.GetStorageFolderFromAsync(destination).ConfigureAwait(false);

      return new File(await file.CopyAsync(
        folder,
        newName,
        canReplace ? NameCollisionOption.ReplaceExisting : NameCollisionOption.FailIfExists));
    }

    public async Task<IFile> MoveAsync(IFolder destination, bool canReplace)
    {
      var folder = await Folder.GetStorageFolderFromAsync(destination).ConfigureAwait(false);

      await file.MoveAsync(
        folder,
        Name,
        canReplace ? NameCollisionOption.ReplaceExisting : NameCollisionOption.FailIfExists);

      return new File(await folder.GetFileAsync(Name));
    }

    public async Task<IFile> MoveAsync(IFolder destination, string newName, bool canReplace)
    {
      var folder = await Folder.GetStorageFolderFromAsync(destination).ConfigureAwait(false);

      await file.MoveAsync(
        folder,
        newName,
        canReplace ? NameCollisionOption.ReplaceExisting : NameCollisionOption.FailIfExists);

      return new File(await folder.GetFileAsync(newName));
    }

    public async Task<bool> ExistsAsync()
    {
      try
      {
        var properties = await file.GetBasicPropertiesAsync();

        return properties.ItemDate != DateTimeOffset.MinValue;
      }
      catch (FileNotFoundException)
      {
        return false;
      }
    }

    public async Task RenameAsync(string newName)
    {
      await file.RenameAsync(newName, NameCollisionOption.FailIfExists);

      var newPath = Path.Combine(Path.GetDirectoryName(file.Path) ?? string.Empty, newName);

      file = await StorageFile.GetFileFromPathAsync(newPath);
    }

    public Task DeleteAsync()
    {
      return file.DeleteAsync().AsTask();
    }

    public async Task<Stream> OpenAsync(bool forWriting)
    {
      var stream = await file.OpenAsync(forWriting ? FileAccessMode.ReadWrite : FileAccessMode.Read);

      return forWriting ? stream.AsStream() : stream.AsStreamForRead();
    }

    public async Task<Stream> OpenSequentialReadAsync()
    {
      var stream = await file.OpenSequentialReadAsync();

      return stream.AsStreamForRead();
    }
  }
}