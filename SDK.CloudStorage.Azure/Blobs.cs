using System.Linq;

namespace SoftmakeAll.SDK.CloudStorage.Azure
{
  public class Blobs : SoftmakeAll.SDK.CloudStorage.Repository, SoftmakeAll.SDK.CloudStorage.IRepository
  {
    #region Constructor
    public Blobs() : base() { }
    #endregion

    #region Properties
    private System.String _ScopedConnectionString;
    private System.String ScopedConnectionString
    {
      get => System.String.IsNullOrWhiteSpace(this._ScopedConnectionString) ? SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString : this._ScopedConnectionString;
      set => this._ScopedConnectionString = value;
    }
    #endregion

    #region Methods
    public void SetScopedConnectionString(System.String ConnectionString) => this.ScopedConnectionString = ConnectionString;
    public override System.String GenerateDownloadURL(System.String ContainerName, System.String EntryName, System.String OriginalName, System.DateTimeOffset ExpirationDateTime)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(ContainerName, EntryName, OriginalName))
        return null;

      global::Azure.Storage.Sas.BlobSasBuilder ShareSasBuilder = new global::Azure.Storage.Sas.BlobSasBuilder();
      ShareSasBuilder.BlobContainerName = ContainerName;
      ShareSasBuilder.Resource = "b";
      ShareSasBuilder.BlobName = EntryName;
      ShareSasBuilder.ExpiresOn = ExpirationDateTime;
      ShareSasBuilder.ContentDisposition = $"attachment; EntryName={OriginalName}";
      ShareSasBuilder.SetPermissions(global::Azure.Storage.Sas.BlobSasPermissions.Read);

      System.String DefaultEndpointsProtocol = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue(this.ScopedConnectionString, "DefaultEndpointsProtocol");
      System.String EndpointSuffix = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue(this.ScopedConnectionString, "EndpointSuffix");
      System.String AccountName = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue(this.ScopedConnectionString, "AccountName");
      System.String AccountKey = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue(this.ScopedConnectionString, "AccountKey");

      try
      {
        return new System.UriBuilder($"{DefaultEndpointsProtocol}://{AccountName}.blob.{EndpointSuffix}/{ContainerName}/{EntryName}") { Query = ShareSasBuilder.ToSasQueryParameters(new global::Azure.Storage.StorageSharedKeyCredential(AccountName, AccountKey)).ToString() }.Uri.AbsoluteUri;
      }
      catch { }

      return null;
    }

    public override SoftmakeAll.SDK.OperationResult<System.Byte[]> Upload(System.String ContainerName, System.String EntryName, System.IO.Stream Contents)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult<System.Byte[]> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Byte[]>() { ExitCode = 400 };

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(ContainerName, EntryName))
      {
        OperationResult.Message = "ContainerName, EntryName and Destination are required.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient BlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(this.ScopedConnectionString, ContainerName);
        BlobContainerClient.CreateIfNotExists();
        global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(EntryName);
        global::Azure.Response<global::Azure.Storage.Blobs.Models.BlobContentInfo> AzureResponse = BlobClient.Upload(Contents, true);

        if (AzureResponse == null)
          throw new System.Exception("AzureResponse is NULL.");

        if (AzureResponse.Value == null)
          throw new System.Exception("AzureResponse.Value is NULL.");

        if (AzureResponse.Value.ContentHash == null)
          throw new System.Exception("AzureResponse.Value.ContentHash is NULL.");

        if (AzureResponse.Value.ContentHash.Length == 0)
          throw new System.Exception("AzureResponse.Value.ContentHash.Length = 0.");

        OperationResult.ExitCode = 0;
        OperationResult.Data = AzureResponse.Value.ContentHash;
      }
      catch (System.Exception ex)
      {
        OperationResult.ExitCode = 500;
        OperationResult.Message = ex.Message;
      }

      return OperationResult;
    }
    public override SoftmakeAll.SDK.OperationResult Download(System.String ContainerName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames, System.IO.Stream Destination)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(ContainerName)) || (EntriesNames == null) || (EntriesNames.Count == 0) || (Destination == null))
      {
        OperationResult.Message = "ContainerName, EntryName and Destination are required.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient BlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(this.ScopedConnectionString, ContainerName);
        if (EntriesNames.Count == 1)
        {
          global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(EntriesNames.First().Key);
          if (!(BlobClient.Exists()))
          {
            OperationResult.Message = "File not found.";
            return OperationResult;
          }

          BlobClient.DownloadTo(Destination);
        }
        else
        {
          System.Collections.Generic.Dictionary<System.String, System.String> FilesToCompression = new System.Collections.Generic.Dictionary<System.String, System.String>();
          using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(EntriesNames.Count))
          {
            System.Collections.Generic.List<System.Threading.Tasks.Task> DownloadTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
            System.String TempPath = System.IO.Path.GetTempPath();
            foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> EntryName in EntriesNames)
            {
              global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(EntryName.Key);
              if (!(BlobClient.Exists()))
                continue;

              SemaphoreSlim.Wait();
              DownloadTasks.Add(System.Threading.Tasks.Task.Run(() =>
              {
                System.String FileName = System.IO.Path.Combine(TempPath, System.IO.Path.GetRandomFileName());
                BlobClient.DownloadTo(FileName);
                FilesToCompression.Add(FileName, (FilesToCompression.ContainsValue(EntryName.Value) ? $"{System.DateTimeOffset.UtcNow:ddMMyyyyHHmmssfff}_{EntryName.Value}" : EntryName.Value));
                SemaphoreSlim.Release();
              }));
            }

            if (DownloadTasks.Any())
              System.Threading.Tasks.Task.WhenAll(DownloadTasks).Wait();
          }

          SoftmakeAll.SDK.Files.Compression.CreateZipArchive(FilesToCompression, Destination);

          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> EntryName in EntriesNames)
            if (System.IO.File.Exists(EntryName.Key)) try { System.IO.File.Delete(EntryName.Key); } catch { }
        }
      }
      catch (System.Exception ex)
      {
        if (EntriesNames.Count > 0)
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> EntryName in EntriesNames)
            if (System.IO.File.Exists(EntryName.Key)) try { System.IO.File.Delete(EntryName.Key); } catch { }

        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public override SoftmakeAll.SDK.OperationResult Delete(System.String ContainerName, System.String[] EntriesNames)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(ContainerName)) || (EntriesNames == null) || (EntriesNames.Length == 0))
      {
        OperationResult.Message = "The ContainerName and EntriesNames cannot be null.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient BlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(this.ScopedConnectionString, ContainerName);

        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(EntriesNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> DeleteTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          foreach (System.String EntryName in EntriesNames)
          {
            global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(EntryName);
            if (!(BlobClient.Exists()))
              continue;

            SemaphoreSlim.Wait();
            DeleteTasks.Add(System.Threading.Tasks.Task.Run(() => { BlobClient.Delete(); SemaphoreSlim.Release(); }));
          }

          if (DeleteTasks.Any())
            System.Threading.Tasks.Task.WhenAll(DeleteTasks).Wait();
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
    public override SoftmakeAll.SDK.OperationResult Copy(System.String SourceContainerName, System.String[] SourceEntriesNames, System.String TargetContainerName, System.String[] TargetEntriesNames, System.Boolean Overwrite)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(SourceContainerName)) || (SourceEntriesNames == null) || (SourceEntriesNames.Length == 0) || (System.String.IsNullOrWhiteSpace(TargetContainerName)) || (TargetEntriesNames == null) || (TargetEntriesNames.Length == 0))
      {
        OperationResult.Message = "The SourceContainerName, SourceEntriesNames, TargetContainerName and TargetEntriesNames cannot be null.";
        return OperationResult;
      }

      if (SourceEntriesNames.Length != TargetEntriesNames.Length)
      {
        OperationResult.Message = "The SourceEntriesNames and TargetEntriesNames must be the same length.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient SourceBlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(this.ScopedConnectionString, SourceContainerName);
        global::Azure.Storage.Blobs.BlobContainerClient TargetBlobContainerClient;
        if (SourceContainerName != TargetContainerName)
          TargetBlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(this.ScopedConnectionString, TargetContainerName);
        else
          TargetBlobContainerClient = SourceBlobContainerClient;


        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(SourceEntriesNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> CopyTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          for (System.Int32 i = 0; i < SourceEntriesNames.Length; i++)
          {
            System.String SourceEntryName = SourceEntriesNames[i];
            global::Azure.Storage.Blobs.BlobClient SourceBlobClient = SourceBlobContainerClient.GetBlobClient(SourceEntryName);
            if (!(SourceBlobClient.Exists()))
              continue;

            System.String TargetEntryName = TargetEntriesNames[i];
            global::Azure.Storage.Blobs.BlobClient TargetBlobClient = TargetBlobContainerClient.GetBlobClient(TargetEntryName);
            if ((!(Overwrite)) && (TargetBlobClient.Exists()))
              continue;

            SemaphoreSlim.Wait();
            CopyTasks.Add(System.Threading.Tasks.Task.Run(() => { TargetBlobClient.StartCopyFromUri(SourceBlobClient.Uri); SemaphoreSlim.Release(); }));
          }

          if (CopyTasks.Any())
            System.Threading.Tasks.Task.WhenAll(CopyTasks).Wait();
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

    #region Async Methods
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> UploadAsync(System.String ContainerName, System.String EntryName, System.IO.Stream Contents)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult<System.Byte[]> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Byte[]>() { ExitCode = 400 };

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(ContainerName, EntryName))
      {
        OperationResult.Message = "ContainerName, EntryName and Destination are required.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient BlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(this.ScopedConnectionString, ContainerName);
        await BlobContainerClient.CreateIfNotExistsAsync();
        global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(EntryName);
        global::Azure.Response<global::Azure.Storage.Blobs.Models.BlobContentInfo> AzureResponse = await BlobClient.UploadAsync(Contents, true);

        if (AzureResponse == null)
          throw new System.Exception("AzureResponse is NULL.");

        if (AzureResponse.Value == null)
          throw new System.Exception("AzureResponse.Value is NULL.");

        if (AzureResponse.Value.ContentHash == null)
          throw new System.Exception("AzureResponse.Value.ContentHash is NULL.");

        if (AzureResponse.Value.ContentHash.Length == 0)
          throw new System.Exception("AzureResponse.Value.ContentHash.Length = 0.");

        OperationResult.ExitCode = 0;
        OperationResult.Data = AzureResponse.Value.ContentHash;
      }
      catch (System.Exception ex)
      {
        OperationResult.ExitCode = 500;
        OperationResult.Message = ex.Message;
      }

      return OperationResult;
    }
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DownloadAsync(System.String ContainerName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames, System.IO.Stream Destination)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(ContainerName)) || (EntriesNames == null) || (EntriesNames.Count == 0) || (Destination == null))
      {
        OperationResult.Message = "ContainerName, EntryName and Destination are required.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient BlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(this.ScopedConnectionString, ContainerName);
        if (EntriesNames.Count == 1)
        {
          global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(EntriesNames.First().Key);
          if (!(await BlobClient.ExistsAsync()))
          {
            OperationResult.Message = "File not found.";
            return OperationResult;
          }

          await BlobClient.DownloadToAsync(Destination);
        }
        else
        {
          System.Collections.Generic.Dictionary<System.String, System.String> FilesToCompression = new System.Collections.Generic.Dictionary<System.String, System.String>();
          using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(EntriesNames.Count))
          {
            System.Collections.Generic.List<System.Threading.Tasks.Task> DownloadTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
            System.String TempPath = System.IO.Path.GetTempPath();
            foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> EntryName in EntriesNames)
            {
              global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(EntryName.Key);
              if (!(await BlobClient.ExistsAsync()))
                continue;

              await SemaphoreSlim.WaitAsync();
              DownloadTasks.Add(System.Threading.Tasks.Task.Run(async () =>
              {
                System.String FileName = System.IO.Path.Combine(TempPath, System.IO.Path.GetRandomFileName());
                await BlobClient.DownloadToAsync(FileName);
                FilesToCompression.Add(FileName, (FilesToCompression.ContainsValue(EntryName.Value) ? $"{System.DateTimeOffset.UtcNow:ddMMyyyyHHmmssfff}_{EntryName.Value}" : EntryName.Value));
                SemaphoreSlim.Release();
              }));

            }

            if (DownloadTasks.Any())
              await System.Threading.Tasks.Task.WhenAll(DownloadTasks);
          }

          SoftmakeAll.SDK.Files.Compression.CreateZipArchive(FilesToCompression, Destination);

          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> EntryName in EntriesNames)
            if (System.IO.File.Exists(EntryName.Key)) try { System.IO.File.Delete(EntryName.Key); } catch { }
        }
      }
      catch (System.Exception ex)
      {
        if (EntriesNames.Count > 0)
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> EntryName in EntriesNames)
            if (System.IO.File.Exists(EntryName.Key)) try { System.IO.File.Delete(EntryName.Key); } catch { }

        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String ContainerName, System.String[] EntriesNames)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(ContainerName)) || (EntriesNames == null) || (EntriesNames.Length == 0))
      {
        OperationResult.Message = "The ContainerName and EntriesNames cannot be null.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient BlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(this.ScopedConnectionString, ContainerName);

        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(EntriesNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> DeleteTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          foreach (System.String EntryName in EntriesNames)
          {
            global::Azure.Storage.Blobs.BlobClient BlobClient = BlobContainerClient.GetBlobClient(EntryName);
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
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceContainerName, System.String[] SourceEntriesNames, System.String TargetContainerName, System.String[] TargetEntriesNames, System.Boolean Overwrite)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(SourceContainerName)) || (SourceEntriesNames == null) || (SourceEntriesNames.Length == 0) || (System.String.IsNullOrWhiteSpace(TargetContainerName)) || (TargetEntriesNames == null) || (TargetEntriesNames.Length == 0))
      {
        OperationResult.Message = "The SourceContainerName, SourceEntriesNames, TargetContainerName and TargetEntriesNames cannot be null.";
        return OperationResult;
      }

      if (SourceEntriesNames.Length != TargetEntriesNames.Length)
      {
        OperationResult.Message = "The SourceEntriesNames and TargetEntriesNames must be the same length.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Blobs.BlobContainerClient SourceBlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(this.ScopedConnectionString, SourceContainerName);
        global::Azure.Storage.Blobs.BlobContainerClient TargetBlobContainerClient;
        if (SourceContainerName != TargetContainerName)
          TargetBlobContainerClient = new global::Azure.Storage.Blobs.BlobContainerClient(this.ScopedConnectionString, TargetContainerName);
        else
          TargetBlobContainerClient = SourceBlobContainerClient;


        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(SourceEntriesNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> CopyTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          for (System.Int32 i = 0; i < SourceEntriesNames.Length; i++)
          {
            System.String SourceEntryName = SourceEntriesNames[i];
            global::Azure.Storage.Blobs.BlobClient SourceBlobClient = SourceBlobContainerClient.GetBlobClient(SourceEntryName);
            if (!(await SourceBlobClient.ExistsAsync()))
              continue;

            System.String TargetEntryName = TargetEntriesNames[i];
            global::Azure.Storage.Blobs.BlobClient TargetBlobClient = TargetBlobContainerClient.GetBlobClient(TargetEntryName);
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
    #endregion
  }
}