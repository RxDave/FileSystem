using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace FileSystem
{
  internal sealed class Folder : IFolder
  {
    public string Name
    {
      get
      {
        return folder.Name;
      }
    }

    public string FullPath
    {
      get
      {
        return folder.Path;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return folder.Attributes.HasFlag(FileAttributes.ReadOnly);
      }
    }

    public bool IsArchive
    {
      get
      {
        return folder.Attributes.HasFlag(FileAttributes.Archive);
      }
    }

    public bool IsTemporary
    {
      get
      {
        return folder.Attributes.HasFlag(FileAttributes.Temporary);
      }
    }

    public DateTimeOffset DateCreated
    {
      get
      {
        return folder.DateCreated;
      }
    }

    private StorageFolder folder;

    public Folder(StorageFolder folder)
    {
      Contract.Requires(folder != null);

      this.folder = folder;
    }

    [ContractInvariantMethod]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
    private void ObjectInvariant()
    {
      Contract.Invariant(folder != null);
    }

    public static async Task<IStorageFolder> GetStorageFolderFromAsync(IFolder folder)
    {
      Contract.Requires(folder != null);
      Contract.Ensures(Contract.Result<Task<IStorageFolder>>() != null);

      var instance = folder as Folder;

      return instance == null
           ? await StorageFolder.GetFolderFromPathAsync(folder.FullPath)
           : instance.folder;
    }

    public Task<IFile> CreateFileAsync(string name)
    {
      return CreateFileAsync(name, canReplace: false);
    }

    public async Task<IFile> CreateFileAsync(string name, bool canReplace)
    {
      return new File(await folder.CreateFileAsync(
        name,
        canReplace ? CreationCollisionOption.ReplaceExisting : CreationCollisionOption.FailIfExists));
    }

    public async Task<IFile> GetOrCreateFileAsync(string name)
    {
      return new File(await folder.CreateFileAsync(
        name,
        CreationCollisionOption.OpenIfExists));
    }

    public async Task<IFile> GetFileAsync(string name)
    {
      return new File(await folder.GetFileAsync(name));
    }

    public async Task<IEnumerable<IFile>> GetFilesAsync()
    {
      return from file in await folder.GetFilesAsync()
             select new File(file);
    }

    public async Task<IEnumerable<IFile>> GetFilesDeepAsync()
    {
      return from file in await folder.GetFilesAsync(CommonFileQuery.OrderBySearchRank)
             select new File(file);
    }

    public Task<IFolder> CreateFolderAsync(string name)
    {
      return CreateFolderAsync(name, canReplace: false);
    }

    public async Task<IFolder> CreateFolderAsync(string name, bool canReplace)
    {
      return new Folder(await folder.CreateFolderAsync(
        name,
        canReplace ? CreationCollisionOption.ReplaceExisting : CreationCollisionOption.FailIfExists));
    }

    public async Task<IFolder> GetOrCreateFolderAsync(string name)
    {
      return new Folder(await folder.CreateFolderAsync(
        name,
        CreationCollisionOption.OpenIfExists));
    }

    public async Task<IFolder> GetFolderAsync(string name)
    {
      return new Folder(await folder.GetFolderAsync(name));
    }

    public async Task<IEnumerable<IFolder>> GetFoldersAsync()
    {
      return from subfolder in await folder.GetFoldersAsync()
             select new Folder(subfolder);
    }

    public async Task<bool> ExistsAsync()
    {
      try
      {
        var properties = await folder.GetBasicPropertiesAsync();

        return properties.ItemDate != DateTimeOffset.MinValue;
      }
      catch (FileNotFoundException)
      {
        return false;
      }
    }

    public async Task RenameAsync(string newName)
    {
      await folder.RenameAsync(newName, NameCollisionOption.FailIfExists);

      var newPath = Path.Combine(Path.GetDirectoryName(folder.Path) ?? string.Empty, newName);

      folder = await StorageFolder.GetFolderFromPathAsync(newPath);
    }

    public Task DeleteAsync()
    {
      return folder.DeleteAsync().AsTask();
    }
  }
}