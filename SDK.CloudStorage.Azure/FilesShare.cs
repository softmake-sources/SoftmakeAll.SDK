using System.Linq;

namespace SoftmakeAll.SDK.CloudStorage.Azure
{
  public class FilesShare : SoftmakeAll.SDK.CloudStorage.IFile
  {
    #region Constructor
    public FilesShare() { }
    #endregion

    #region Methods
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> UploadAsync(System.String ShareName, System.String StorageFileName, System.IO.Stream FileContents)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate();

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(ShareName, StorageFileName))
      {
        OperationResult.Message = "The ShareName and StorageFileName cannot be null.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient ShareClient = new global::Azure.Storage.Files.Shares.ShareClient(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString, ShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient ShareDirectoryClient = ShareClient.GetRootDirectoryClient();
        global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(StorageFileName);
        await ShareFileClient.CreateAsync(FileContents.Length);
        await ShareFileClient.UploadAsync(FileContents);
      }
      catch (System.Exception ex)
      {
        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ShareName, System.String StorageFileName)
    {
      return await this.DownloadAsync(ShareName, new System.String[] { StorageFileName });
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ShareName, System.String[] StorageFileNames)
    {
      System.Collections.Generic.Dictionary<System.String, System.String> StorageFileNamesDictionary = null;
      if ((StorageFileNames != null) && (StorageFileNames.Length > 0))
      {
        StorageFileNamesDictionary = new System.Collections.Generic.Dictionary<System.String, System.String>();
        foreach (System.String StorageFileName in StorageFileNames)
          StorageFileNamesDictionary.Add(StorageFileName, StorageFileName);
      }
      return await this.DownloadAsync(ShareName, StorageFileNamesDictionary);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ShareName, System.Collections.Generic.Dictionary<System.String, System.String> StorageFileNames)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate();

      SoftmakeAll.SDK.OperationResult<System.Byte[]> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Byte[]>();

      if ((System.String.IsNullOrWhiteSpace(ShareName)) || (StorageFileNames == null) || (StorageFileNames.Count == 0))
      {
        OperationResult.Message = "The ShareName and StorageFileName cannot be null.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient ShareClient = new global::Azure.Storage.Files.Shares.ShareClient(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString, ShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient ShareDirectoryClient = ShareClient.GetRootDirectoryClient();
        if (StorageFileNames.Count == 1)
        {
          global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(StorageFileNames.First().Key);

          if (!(await ShareFileClient.ExistsAsync()))
          {
            OperationResult.Message = "File not found.";
            return OperationResult;
          }

          global::Azure.Storage.Files.Shares.Models.ShareFileDownloadInfo ShareFileDownloadInfo = await ShareFileClient.DownloadAsync();
          using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
          {
            ShareFileDownloadInfo.Content.CopyTo(MemoryStream);
            OperationResult.Data = MemoryStream.ToArray();
          }
        }
        else
        {
          System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(StorageFileNames.Count);
          System.Collections.Generic.List<System.Threading.Tasks.Task> DownloadTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> StorageFileName in StorageFileNames)
          {
            global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(StorageFileName.Key);
            if (!(await ShareFileClient.ExistsAsync()))
              continue;

            await SemaphoreSlim.WaitAsync();
            DownloadTasks.Add(System.Threading.Tasks.Task.Run(async () =>
            {
              global::Azure.Storage.Files.Shares.Models.ShareFileDownloadInfo ShareFileDownloadInfo = await ShareFileClient.DownloadAsync();
              using (System.IO.FileStream FileStream = System.IO.File.OpenWrite(StorageFileName.Key))
                ShareFileDownloadInfo.Content.CopyTo(FileStream);

              SemaphoreSlim.Release();
            }
            ));
          }
          await System.Threading.Tasks.Task.WhenAll(DownloadTasks);
          SemaphoreSlim.Dispose();

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
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String ShareName, System.String StorageFileName)
    {
      return await this.DeleteAsync(ShareName, new System.String[] { StorageFileName });
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String ShareName, System.String[] StorageFileNames)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate();

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(ShareName)) || (StorageFileNames == null) || (StorageFileNames.Length == 0))
      {
        OperationResult.Message = "The ShareName and StorageFileName cannot be null.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient ShareClient = new global::Azure.Storage.Files.Shares.ShareClient(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString, ShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient ShareDirectoryClient = ShareClient.GetRootDirectoryClient();

        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(StorageFileNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> DeleteTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          foreach (System.String StorageFileName in StorageFileNames)
          {
            global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(StorageFileName);
            if (!(await ShareFileClient.ExistsAsync()))
              continue;

            await SemaphoreSlim.WaitAsync();
            DeleteTasks.Add(System.Threading.Tasks.Task.Run(async () => { await ShareFileClient.DeleteAsync(); SemaphoreSlim.Release(); }));
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
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceShareName, System.String SourceStorageFileName, System.String TargetStorageFileName, System.Boolean Overwrite)
    {
      return await this.CopyAsync(SourceShareName, new System.String[] { SourceStorageFileName }, SourceShareName, new System.String[] { TargetStorageFileName }, Overwrite);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceShareName, System.String[] SourceStorageFileNames, System.String[] TargetStorageFileNames, System.Boolean Overwrite)
    {
      return await this.CopyAsync(SourceShareName, SourceStorageFileNames, SourceShareName, TargetStorageFileNames, Overwrite);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceShareName, System.String SourceStorageFileName, System.String TargetShareName, System.String TargetStorageFileName, System.Boolean Overwrite)
    {
      return await this.CopyAsync(SourceShareName, new System.String[] { SourceStorageFileName }, TargetShareName, new System.String[] { TargetStorageFileName }, Overwrite);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceShareName, System.String[] SourceStorageFileNames, System.String TargetShareName, System.String[] TargetStorageFileNames, System.Boolean Overwrite)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate();

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(SourceShareName)) || (SourceStorageFileNames == null) || (SourceStorageFileNames.Length == 0) || (System.String.IsNullOrWhiteSpace(TargetShareName)) || (TargetStorageFileNames == null) || (TargetStorageFileNames.Length == 0))
      {
        OperationResult.Message = "The SourceShareName, SourceStorageFileNames, TargetShareName and TargetStorageFileNames cannot be null.";
        return OperationResult;
      }

      if (SourceStorageFileNames.Length != TargetStorageFileNames.Length)
      {
        OperationResult.Message = "The SourceStorageFileNames and TargetStorageFileNames must be the same length.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient SourceShareClient = new global::Azure.Storage.Files.Shares.ShareClient(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString, SourceShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient SourceShareDirectoryClient = SourceShareClient.GetRootDirectoryClient();

        global::Azure.Storage.Files.Shares.ShareClient TargetShareClient;
        if (SourceShareName != TargetShareName)
          TargetShareClient = new global::Azure.Storage.Files.Shares.ShareClient(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString, TargetShareName);
        else
          TargetShareClient = SourceShareClient;
        global::Azure.Storage.Files.Shares.ShareDirectoryClient TargetShareDirectoryClient = TargetShareClient.GetRootDirectoryClient();


        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(SourceStorageFileNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> CopyTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          for (System.Int32 i = 0; i < SourceStorageFileNames.Length; i++)
          {
            System.String SourceStorageFileName = SourceStorageFileNames[i];
            global::Azure.Storage.Files.Shares.ShareFileClient SourceShareFileClient = SourceShareDirectoryClient.GetFileClient(SourceStorageFileName);
            if (!(await SourceShareFileClient.ExistsAsync()))
              continue;

            System.String TargetStorageFileName = TargetStorageFileNames[i];
            global::Azure.Storage.Files.Shares.ShareFileClient TargetShareFileClient = SourceShareDirectoryClient.GetFileClient(TargetStorageFileName);
            if ((!(Overwrite)) && (await TargetShareFileClient.ExistsAsync()))
              continue;

            await SemaphoreSlim.WaitAsync();
            CopyTasks.Add(System.Threading.Tasks.Task.Run(async () => { await TargetShareFileClient.StartCopyAsync(SourceShareFileClient.Uri); SemaphoreSlim.Release(); }));
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
    public System.String GenerateDownloadURL(System.String ShareName, System.String StorageFileName, System.String OriginalName) => this.GenerateDownloadURL(ShareName, StorageFileName, OriginalName, System.DateTimeOffset.UtcNow.AddMinutes(5));
    public System.String GenerateDownloadURL(System.String ShareName, System.String StorageFileName, System.String OriginalName, System.DateTimeOffset ExpirationDateTime)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate();

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(ShareName, StorageFileName, OriginalName))
        return null;

      global::Azure.Storage.Sas.ShareSasBuilder ShareSasBuilder = new global::Azure.Storage.Sas.ShareSasBuilder();
      ShareSasBuilder.ShareName = ShareName;
      ShareSasBuilder.Resource = "f";
      ShareSasBuilder.FilePath = StorageFileName;
      ShareSasBuilder.ExpiresOn = ExpirationDateTime;
      ShareSasBuilder.ContentDisposition = $"attachment; filename={OriginalName}";
      ShareSasBuilder.SetPermissions(global::Azure.Storage.Sas.ShareFileSasPermissions.Read);

      System.String DefaultEndpointsProtocol = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue("DefaultEndpointsProtocol");
      System.String EndpointSuffix = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue("EndpointSuffix");
      System.String AccountName = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue("AccountName");
      System.String AccountKey = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue("AccountKey");

      try
      {
        return new System.UriBuilder($"{DefaultEndpointsProtocol}://{AccountName}.file.{EndpointSuffix}/{ShareName}/{StorageFileName}") { Query = ShareSasBuilder.ToSasQueryParameters(new global::Azure.Storage.StorageSharedKeyCredential(AccountName, AccountKey)).ToString() }.Uri.AbsoluteUri;
      }
      catch { }

      return null;
    }
    #endregion
  }
}