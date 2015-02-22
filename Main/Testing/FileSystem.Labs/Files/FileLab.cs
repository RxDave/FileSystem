using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DaveSexton.Labs;

namespace FileSystem.Labs.Files
{
  public sealed class FileLab : BaseConsoleLab
  {
    protected override void Main()
    {
      RunExperiments();
    }

    public async Task TemporaryFile()
    {
      var file = await FileSystemProvider.TemporaryStorage.CreateFileAsync("123.test");

      await TraceFileAsync(file);

      var contents = "Testing 123";

      await WriteToFileAsync(file, contents);

      TraceSuccess("Wrote: {0}", contents);

      file = await FileSystemProvider.TemporaryStorage.GetFileAsync("123.test");

      await TraceFileAsync(file);

      contents = await ReadFromFileAsync(file);

      TraceSuccess("Read: {0}", contents);

      await file.DeleteAsync();

      await TraceFileAsync(file);
    }

    public async Task LocalStorageFile()
    {
      var file = await FileSystemProvider.LocalStorage.CreateFileAsync("456.test");

      await TraceFileAsync(file);

      var contents = "Testing 456";

      await WriteToFileAsync(file, contents);

      TraceSuccess("Wrote: {0}", contents);

      file = await FileSystemProvider.LocalStorage.GetFileAsync("456.test");

      await TraceFileAsync(file);

      contents = await ReadFromFileAsync(file);

      TraceSuccess("Read: {0}", contents);

      await file.DeleteAsync();

      await TraceFileAsync(file);
    }

    public async Task MyDocumentsFile()
    {
      var folder = await FileSystemProvider.GetFolderAsync(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));

      await FolderLab.TraceFolderAsync(folder);

      var file = await folder.CreateFileAsync("789.test");

      await TraceFileAsync(file);

      var contents = "Testing 789";

      await WriteToFileAsync(file, contents);

      TraceSuccess("Wrote: {0}", contents);

      file = await folder.GetFileAsync("789.test");

      await TraceFileAsync(file);

      contents = await ReadFromFileAsync(file);

      TraceSuccess("Read: {0}", contents);

      await file.DeleteAsync();

      await TraceFileAsync(file);
    }

    private static async Task WriteToFileAsync(IFile file, string contents)
    {
      using (var stream = await file.OpenAsync(forWriting: true))
      {
        var bytes = Encoding.UTF8.GetBytes(contents);

        await stream.WriteAsync(bytes, 0, bytes.Length);
      }
    }

    private static async Task<string> ReadFromFileAsync(IFile file)
    {
      using (var stream = await file.OpenSequentialReadAsync())
      using (var reader = new StreamReader(stream))
      {
        return await reader.ReadToEndAsync();
      }
    }

    public static async Task TraceFileAsync(IFile file)
    {
      if (await file.ExistsAsync())
      {
        LabTraceSource.Default.TraceSuccess(file.FullPath);
      }
      else
      {
        LabTraceSource.Default.TraceFailure(file.FullPath);
      }
    }
  }
}