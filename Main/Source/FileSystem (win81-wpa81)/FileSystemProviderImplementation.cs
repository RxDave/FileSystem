using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileSystem
{
  internal sealed class FileSystemProviderImplementation : FileSystemProvider
  {
    protected override async Task<IFile> GetFileCoreAsync(string fullPath)
    {
      return new File(await StorageFile.GetFileFromPathAsync(fullPath));
    }

    protected override async Task<IFolder> GetFolderCoreAsync(string fullPath)
    {
      return new Folder(await StorageFolder.GetFolderFromPathAsync(fullPath));
    }

    protected override IFolder GetLocalStorage()
    {
      return new Folder(ApplicationData.Current.LocalFolder);
    }

    protected override IFolder GetTemporaryStorage()
    {
      return new Folder(ApplicationData.Current.TemporaryFolder);
    }
  }
}