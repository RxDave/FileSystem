using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;
using System.Threading.Tasks;

namespace FileSystem
{
  internal sealed class FileSystemProviderImplementation : FileSystemProvider
  {
    protected override Task<IFile> GetFileCoreAsync(string fullPath)
    {
      return TaskEx.FromResult((IFile)new File(new FileInfo(fullPath)));
    }

    protected override Task<IFolder> GetFolderCoreAsync(string fullPath)
    {
      return TaskEx.FromResult((IFolder)new Folder(new DirectoryInfo(fullPath)));
    }

    protected override IFolder GetLocalStorage()
    {
      return new IsolatedFolderRoot(IsolatedStorageFile.GetUserStoreForDomain());
    }

    protected override IFolder GetTemporaryStorage()
    {
      var info = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Assembly.GetEntryAssembly().GetName().Name));

      info.Create();

      return new Folder(info);
    }
  }
}