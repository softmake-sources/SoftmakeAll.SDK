using System.Linq;

namespace SoftmakeAll.SDK.CloudStorage.Azure
{
  public class Blobs : SoftmakeAll.SDK.CloudStorage.IFile
  {
    #region Constructor
    public Blobs() { }
    #endregion

    #region Methods
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> UploadAsync(System.String ContainerName, System.String StorageFileName, System.IO.Stream FileContents)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate();

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(ContainerName, StorageFileName))
      {
        OperationResult.Message = "The ContainerName and StorageFileName cannot be null.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient BlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString, ContainerName);
        await BlobContainerClient.CreateIfNotExistsAsync();
        global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(StorageFileName);
        await BlobClient.UploadAsync(FileContents, true);
      }
      catch (System.Exception ex)
      {
        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ContainerName, System.String StorageFileName)
    {
      return await this.DownloadAsync(ContainerName, new System.String[] { StorageFileName });
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ContainerName, System.String[] StorageFileNames)
    {
      System.Collections.Generic.Dictionary<System.String, System.String> StorageFileNamesDictionary = null;
      if ((StorageFileNames != null) && (StorageFileNames.Length > 0))
      {
        StorageFileNamesDictionary = new System.Collections.Generic.Dictionary<System.String, System.String>();
        foreach (System.String StorageFileName in StorageFileNames)
          StorageFileNamesDictionary.Add(StorageFileName, StorageFileName);
      }
      return await this.DownloadAsync(ContainerName, StorageFileNamesDictionary);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ContainerName, System.Collections.Generic.Dictionary<System.String, System.String> StorageFileNames)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate();

      SoftmakeAll.SDK.OperationResult<System.Byte[]> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Byte[]>();

      if ((System.String.IsNullOrWhiteSpace(ContainerName)) || (StorageFileNames == null) || (StorageFileNames.Count == 0))
      {
        OperationResult.Message = "The ContainerName and StorageFileName cannot be null.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient BlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString, ContainerName);
        if (StorageFileNames.Count == 1)
        {
          global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(StorageFileNames.First().Key);
          if (!(await BlobClient.ExistsAsync()))
          {
            OperationResult.Message = "The file could not be found.";
            return OperationResult;
          }

          using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
          {
            await BlobClient.DownloadToAsync(MemoryStream);
            OperationResult.Data = MemoryStream.ToArray();
          }
        }
        else
        {
          using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(StorageFileNames.Count))
          {
            System.Collections.Generic.List<System.Threading.Tasks.Task> DownloadTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
            foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> StorageFileName in StorageFileNames)
            {
              global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(StorageFileName.Key);
              if (!(await BlobClient.ExistsAsync()))
                continue;

              await SemaphoreSlim.WaitAsync();
              DownloadTasks.Add(System.Threading.Tasks.Task.Run(async () => { await BlobClient.DownloadToAsync(StorageFileName.Key); SemaphoreSlim.Release(); }));
            }

            if (DownloadTasks.Any())
              await System.Threading.Tasks.Task.WhenAll(DownloadTasks);
          }

          OperationResult.Data = SoftmakeAll.SDK.Files.Compression.CreateZipArchive(StorageFileNames);

          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> StorageFileName in StorageFileNames)
            if (System.IO.File.Exists(StorageFileName.Key)) try { System.IO.File.Delete(StorageFileName.Key); } catch { }
        }
      }
      catch (System.Exception ex)
      {
        if (StorageFileNames.Count > 0)
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> StorageFileName in StorageFileNames)
            if (System.IO.File.Exists(StorageFileName.Key)) try { System.IO.File.Delete(StorageFileName.Key); } catch { }

        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String ContainerName, System.String StorageFileName)
    {
      return await this.DeleteAsync(ContainerName, new System.String[] { StorageFileName });
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String ContainerName, System.String[] StorageFileNames)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate();

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(ContainerName)) || (StorageFileNames == null) || (StorageFileNames.Length == 0))
      {
        OperationResult.Message = "The ContainerName and StorageFileName cannot be null.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient BlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString, ContainerName);

        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(StorageFileNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> DeleteTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          foreach (System.String StorageFileName in StorageFileNames)
          {
            global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(StorageFileName);
            if (!(await BlobClient.ExistsAsync()))
              continue;

            await SemaphoreSlim.WaitAsync();
            DeleteTasks.Add(System.Threading.Tasks.Task.Run(async () => { await BlobClient.DeleteAsync(); SemaphoreSlim.Release(); }));
          }

          if (DeleteTasks.Any())
            await System.Threading.Tasks.Task.WhenAll(DeleteTasks);
        }
      }
      catch (System.Exception ex)
      {
        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceContainerName, System.String SourceStorageFileName, System.String TargetStorageFileName, System.Boolean Overwrite)
    {
      return await this.CopyAsync(SourceContainerName, new System.String[] { SourceStorageFileName }, SourceContainerName, new System.String[] { TargetStorageFileName }, Overwrite);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceContainerName, System.String[] SourceStorageFileNames, System.String[] TargetStorageFileNames, System.Boolean Overwrite)
    {
      return await this.CopyAsync(SourceContainerName, SourceStorageFileNames, SourceContainerName, TargetStorageFileNames, Overwrite);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceContainerName, System.String SourceStorageFileName, System.String TargetContainerName, System.String TargetStorageFileName, System.Boolean Overwrite)
    {
      return await this.CopyAsync(SourceContainerName, new System.String[] { SourceStorageFileName }, TargetContainerName, new System.String[] { TargetStorageFileName }, Overwrite);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceContainerName, System.String[] SourceStorageFileNames, System.String TargetContainerName, System.String[] TargetStorageFileNames, System.Boolean Overwrite)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate();

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(SourceContainerName)) || (SourceStorageFileNames == null) || (SourceStorageFileNames.Length == 0) || (System.String.IsNullOrWhiteSpace(TargetContainerName)) || (TargetStorageFileNames == null) || (TargetStorageFileNames.Length == 0))
      {
        OperationResult.Message = "The SourceContainerName, SourceStorageFileNames, TargetContainerName and TargetStorageFileNames cannot be null.";
        return OperationResult;
      }

      if (SourceStorageFileNames.Length != TargetStorageFileNames.Length)
      {
        OperationResult.Message = "The SourceStorageFileNames and TargetStorageFileNames must be the same length.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient SourceBlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString, SourceContainerName);
        global::Azure.Storage.Blobs.BlobContainerClient TargetBlobContainerClient;
        if (SourceContainerName != TargetContainerName)
          TargetBlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString, TargetContainerName);
        else
          TargetBlobContainerClient = SourceBlobContainerClient;


        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(SourceStorageFileNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> CopyTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          for (System.Int32 i = 0; i < SourceStorageFileNames.Length; i++)
          {
            System.String SourceStorageFileName = SourceStorageFileNames[i];
            global::Azure.Storage.Blobs.BlobClient SourceBlobClient = SourceBlobContainerClient.GetBlobClient(SourceStorageFileName);
            if (!(await SourceBlobClient.ExistsAsync()))
              continue;

            System.String TargetStorageFileName = TargetStorageFileNames[i];
            global::Azure.Storage.Blobs.BlobClient TargetBlobClient = TargetBlobContainerClient.GetBlobClient(TargetStorageFileName);
            if ((!(Overwrite)) && (await TargetBlobClient.ExistsAsync()))
              continue;

            await SemaphoreSlim.WaitAsync();
            CopyTasks.Add(System.Threading.Tasks.Task.Run(async () => { await TargetBlobClient.StartCopyFromUriAsync(SourceBlobClient.Uri); SemaphoreSlim.Release(); }));
          }

          if (CopyTasks.Any())
            await System.Threading.Tasks.Task.WhenAll(CopyTasks);
        }
      }
      catch (System.Exception ex)
      {
        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    #endregion
  }
}