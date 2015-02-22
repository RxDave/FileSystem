using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FileSystem.Core.Properties;

namespace FileSystem
{
  /// <summary>
  /// Provides common file system functions on any platform.
  /// </summary>
  [ContractClass(typeof(FileSystemProviderContract))]
  public abstract class FileSystemProvider
  {
    /// <summary>
    /// Gets a local folder for persisting data based on the running application and the current user.
    /// </summary>
    public static IFolder LocalStorage
    {
      get
      {
        Contract.Ensures(Contract.Result<IFolder>() != null);

        return instance.Value.GetLocalStorage();
      }
    }

    /// <summary>
    /// Gets a local folder for temporary files based on the running application and the current user.
    /// </summary>
    public static IFolder TemporaryStorage
    {
      get
      {
        Contract.Ensures(Contract.Result<IFolder>() != null);

        return instance.Value.GetTemporaryStorage();
      }
    }

    private static readonly Lazy<FileSystemProvider> instance = new Lazy<FileSystemProvider>(LoadFileProvider);

    private static FileSystemProvider LoadFileProvider()
    {
      Contract.Ensures(Contract.Result<FileSystemProvider>() != null);

      var providerAssemblyName = typeof(FileSystemProvider).Assembly.FullName.Replace(".Core", string.Empty);

      Assembly assembly;
      try
      {
        assembly = Assembly.Load(providerAssemblyName);
      }
      catch (FileNotFoundException)
      {
        assembly = null;
      }

      FileSystemProvider provider = null;

      if (assembly != null)
      {
        provider = (from type in assembly.GetTypes()
                    where typeof(FileSystemProvider).IsAssignableFrom(type)
                    select (FileSystemProvider)type.GetConstructor(new Type[0]).Invoke(new object[0]))
                    .FirstOrDefault();
      }

      if (provider == null)
      {
        throw new InvalidOperationException(Errors.FileProviderImplementationNotFound);
      }
      else
      {
        return provider;
      }
    }

    /// <summary>
    /// Constructs a new instance of the <see cref="FileSystemProvider"/> class for derived types.
    /// </summary>
    protected FileSystemProvider()
    {
    }

    /// <summary>
    /// An optional method that allows an application to control exactly when <see cref="FileSystemProvider"/> is initialized,
    /// such as in a startup routine. Not calling this method results in lazy initialization upon first-time use of any 
    /// of the <see cref="FileSystemProvider"/> members.
    /// </summary>
    public static void EnsureInitialized()
    {
      var provider = instance.Value;
    }

    /// <summary>
    /// Gets the file at the specified location.
    /// </summary>
    /// <param name="fullPath">The full path and name of the file to be retrieved.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFile"/> object representing the file at the specified location.</returns>
    public static Task<IFile> GetFileAsync(Uri fullPath)
    {
      Contract.Requires(fullPath != null);
      Contract.Requires(fullPath.IsAbsoluteUri);
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);

      return instance.Value.GetFileCoreAsync(fullPath.LocalPath);
    }

    /// <summary>
    /// Gets the file at the specified location.
    /// </summary>
    /// <param name="fullPath">The full path and name of the file to be retrieved.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFile"/> object representing the file at the specified location.</returns>
    public static Task<IFile> GetFileAsync(string fullPath)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(fullPath));
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);

      return instance.Value.GetFileCoreAsync(fullPath);
    }

    /// <summary>
    /// Gets the folder at the specified location.
    /// </summary>
    /// <param name="fullPath">The full path and name of the folder to be retrieved.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFolder"/> object representing the folder at the specified location.</returns>
    public static Task<IFolder> GetFolderAsync(Uri fullPath)
    {
      Contract.Requires(fullPath != null);
      Contract.Requires(fullPath.IsAbsoluteUri);
      Contract.Ensures(Contract.Result<Task<IFolder>>() != null);

      return instance.Value.GetFolderCoreAsync(fullPath.LocalPath);
    }

    /// <summary>
    /// Gets the folder at the specified location.
    /// </summary>
    /// <param name="fullPath">The full path and name of the folder to be retrieved.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFolder"/> object representing the folder at the specified location.</returns>
    public static Task<IFolder> GetFolderAsync(string fullPath)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(fullPath));
      Contract.Ensures(Contract.Result<Task<IFolder>>() != null);

      return instance.Value.GetFolderCoreAsync(fullPath);
    }

    /// <summary>
    /// Gets the file at the specified location.
    /// </summary>
    /// <param name="fullPath">The full path and name of the file to be retrieved.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFile"/> object representing the file at the specified location.</returns>
    protected abstract Task<IFile> GetFileCoreAsync(string fullPath);

    /// <summary>
    /// Gets the folder at the specified location.
    /// </summary>
    /// <param name="fullPath">The full path and name of the folder to be retrieved.</param>
    /// <returns>A <see cref="Task{T}"/> containing an <see cref="IFolder"/> object representing the folder at the specified location.</returns>
    protected abstract Task<IFolder> GetFolderCoreAsync(string fullPath);

    /// <summary>
    /// Gets a local folder for persisting data based on the running application and the current user.
    /// </summary>
    protected abstract IFolder GetLocalStorage();

    /// <summary>
    /// Gets a local folder for temporary files based on the running application and the current user.
    /// </summary>
    protected abstract IFolder GetTemporaryStorage();
  }

  [ContractClassFor(typeof(FileSystemProvider))]
  internal abstract class FileSystemProviderContract : FileSystemProvider
  {
    protected override Task<IFile> GetFileCoreAsync(string fullPath)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(fullPath));
      Contract.Ensures(Contract.Result<Task<IFile>>() != null);
      return null;
    }

    protected override Task<IFolder> GetFolderCoreAsync(string fullPath)
    {
      Contract.Requires(!string.IsNullOrWhiteSpace(fullPath));
      Contract.Ensures(Contract.Result<Task<IFolder>>() != null);
      return null;
    }

    protected override IFolder GetLocalStorage()
    {
      Contract.Ensures(Contract.Result<IFolder>() != null);
      return null;
    }

    protected override IFolder GetTemporaryStorage()
    {
      Contract.Ensures(Contract.Result<IFolder>() != null);
      return null;
    }
  }
}