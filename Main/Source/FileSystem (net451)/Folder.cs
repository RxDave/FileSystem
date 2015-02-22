using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileSystem.Properties;

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
        return folder.FullName;
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
        return folder.CreationTime;
      }
    }

    private DirectoryInfo folder;

    public Folder(DirectoryInfo folder)
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

    public Task<IFile> CreateFileAsync(string name)
    {
      return CreateFileAsync(name, canReplace: false);
    }

    public Task<IFile> CreateFileAsync(string name, bool canReplace)
    {
      var file = new File(new FileInfo(Path.Combine(folder.FullName, name)));

      System.IO.File.Open(file.FullPath, canReplace ? FileMode.Create : FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None)
                    .Dispose();

      return Task.FromResult<IFile>(file);
    }

    public Task<IFile> GetOrCreateFileAsync(string name)
    {
      var file = new File(new FileInfo(Path.Combine(folder.FullName, name)));

      System.IO.File.Open(file.FullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None)
                    .Dispose();

      return Task.FromResult<IFile>(file);
    }

    public Task<IFile> GetFileAsync(string name)
    {
      return Task.FromResult(
        (from file in folder.EnumerateFiles(name)
         select (IFile)new File(file))
         .Single());
    }

    public Task<IEnumerable<IFile>> GetFilesAsync()
    {
      return Task.FromResult(
        from file in folder.EnumerateFiles()
        select (IFile)new File(file));
    }

    public Task<IEnumerable<IFile>> GetFilesDeepAsync()
    {
      return Task.FromResult(
        from file in folder.EnumerateFiles("*", SearchOption.AllDirectories)
        select (IFile)new File(file));
    }

    public Task<IFolder> CreateFolderAsync(string name)
    {
      return CreateFolderAsync(name, canReplace: false);
    }

    public Task<IFolder> CreateFolderAsync(string name, bool canReplace)
    {
      var subfolder = new Folder(new DirectoryInfo(Path.Combine(folder.FullName, name)));

      if (Directory.Exists(subfolder.FullPath))
      {
        throw new IOException(Errors.DirectoryAlreadyExists);
      }

      Directory.CreateDirectory(subfolder.FullPath);

      return Task.FromResult<IFolder>(subfolder);
    }

    public Task<IFolder> GetOrCreateFolderAsync(string name)
    {
      var subfolder = new Folder(new DirectoryInfo(Path.Combine(folder.FullName, name)));

      Directory.CreateDirectory(subfolder.FullPath);

      return Task.FromResult<IFolder>(subfolder);
    }

    public Task<IFolder> GetFolderAsync(string name)
    {
      return Task.FromResult(
         (from subfolder in folder.EnumerateDirectories(name)
          select (IFolder)new Folder(subfolder))
          .Single());
    }

    public Task<IEnumerable<IFolder>> GetFoldersAsync()
    {
      return Task.FromResult(
        from subfolder in folder.EnumerateDirectories()
        select (IFolder)new Folder(subfolder));
    }

    public Task<bool> ExistsAsync()
    {
      return Task.FromResult(folder.Exists);
    }

    public Task RenameAsync(string newName)
    {
      var newPath = Path.Combine(Path.GetDirectoryName(folder.FullName) ?? string.Empty, newName);

      folder.MoveTo(newPath);

      folder = new DirectoryInfo(newPath);

      return Task.FromResult(true);
    }

    public Task DeleteAsync()
    {
      folder.Delete(recursive: true);
      folder.Refresh();

      return Task.FromResult(true);
    }
  }
}