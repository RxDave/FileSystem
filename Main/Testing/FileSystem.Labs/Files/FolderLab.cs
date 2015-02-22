using System;
using System.Threading.Tasks;
using DaveSexton.Labs;

namespace FileSystem.Labs.Files
{
  public sealed class FolderLab : BaseConsoleLab
  {
    protected override void Main()
    {
      RunExperiments();
    }

    public async Task TemporaryFolder()
    {
      var folder = await FileSystemProvider.TemporaryStorage.CreateFolderAsync("123");

      await TraceFolderAsync(folder);

      folder = await FileSystemProvider.TemporaryStorage.GetFolderAsync("123");

      await TraceFolderAsync(folder);

      await folder.DeleteAsync();

      await TraceFolderAsync(folder);
    }

    public async Task LocalStorageFolder()
    {
      var folder = await FileSystemProvider.LocalStorage.CreateFolderAsync("456");

      await TraceFolderAsync(folder);

      folder = await FileSystemProvider.LocalStorage.GetFolderAsync("456");

      await TraceFolderAsync(folder);

      await folder.DeleteAsync();

      await TraceFolderAsync(folder);
    }

    public async Task MyDocumentsFolder()
    {
      var documents = await FileSystemProvider.GetFolderAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

      await TraceFolderAsync(documents);

      var folder = await documents.CreateFolderAsync("789");

      await TraceFolderAsync(folder);

      folder = await documents.GetFolderAsync("789");

      await TraceFolderAsync(folder);

      await folder.DeleteAsync();

      await TraceFolderAsync(folder);
    }

    public static async Task TraceFolderAsync(IFolder folder)
    {
      if (await folder.ExistsAsync())
      {
        LabTraceSource.Default.TraceSuccess(folder.FullPath);
      }
      else
      {
        LabTraceSource.Default.TraceFailure(folder.FullPath);
      }
    }
  }
}