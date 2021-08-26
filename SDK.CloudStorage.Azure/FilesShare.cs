using System.Linq;

namespace SoftmakeAll.SDK.CloudStorage.Azure
{
  public class FilesShare : SoftmakeAll.SDK.CloudStorage.Repository, SoftmakeAll.SDK.CloudStorage.IRepository
  {
    #region Constructor
    public FilesShare() : base() { }
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
    public override System.String GenerateDownloadURL(System.String ShareName, System.String EntryName, System.String OriginalName, System.DateTimeOffset ExpirationDateTime)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(ShareName, EntryName, OriginalName))
        return null;

      global::Azure.Storage.Sas.ShareSasBuilder ShareSasBuilder = new global::Azure.Storage.Sas.ShareSasBuilder();
      ShareSasBuilder.ShareName = ShareName;
      ShareSasBuilder.Resource = "f";
      ShareSasBuilder.FilePath = EntryName;
      ShareSasBuilder.ExpiresOn = ExpirationDateTime;
      ShareSasBuilder.ContentDisposition = $"attachment; EntryName={OriginalName}";
      ShareSasBuilder.SetPermissions(global::Azure.Storage.Sas.ShareFileSasPermissions.Read);

      System.String DefaultEndpointsProtocol = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue(this.ScopedConnectionString, "DefaultEndpointsProtocol");
      System.String EndpointSuffix = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue(this.ScopedConnectionString, "EndpointSuffix");
      System.String AccountName = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue(this.ScopedConnectionString, "AccountName");
      System.String AccountKey = SoftmakeAll.SDK.CloudStorage.Azure.Environment.GetConnectionStringPropertyValue(this.ScopedConnectionString, "AccountKey");

      try
      {
        return new System.UriBuilder($"{DefaultEndpointsProtocol}://{AccountName}.file.{EndpointSuffix}/{ShareName}/{EntryName}") { Query = ShareSasBuilder.ToSasQueryParameters(new global::Azure.Storage.StorageSharedKeyCredential(AccountName, AccountKey)).ToString() }.Uri.AbsoluteUri;
      }
      catch { }

      return null;
    }

    public override SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> Upload(System.String ShareName, System.String EntryName, System.IO.Stream Contents)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(ShareName, EntryName))
      {
        OperationResult.Message = "The ShareName and EntriesNames cannot be null.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient ShareClient = new global::Azure.Storage.Files.Shares.ShareClient(this.ScopedConnectionString, ShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient ShareDirectoryClient = ShareClient.GetRootDirectoryClient();
        global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(EntryName);
        ShareFileClient.Create(Contents.Length);
        ShareFileClient.Upload(Contents);
      }
      catch (System.Exception ex)
      {
        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public override SoftmakeAll.SDK.OperationResult Download(System.String ShareName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames, System.IO.Stream Destination)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(ShareName)) || (EntriesNames == null) || (EntriesNames.Count == 0) || (Destination == null))
      {
        OperationResult.Message = "ShareName, EntryName and Contents are required.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient ShareClient = new global::Azure.Storage.Files.Shares.ShareClient(this.ScopedConnectionString, ShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient ShareDirectoryClient = ShareClient.GetRootDirectoryClient();
        if (EntriesNames.Count == 1)
        {
          global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(EntriesNames.First().Key);

          if (!(ShareFileClient.Exists()))
          {
            OperationResult.Message = "File not found.";
            return OperationResult;
          }

          (ShareFileClient.Download()).Value.Content.CopyTo(Destination);
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
              global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(EntryName.Key);
              if (!(ShareFileClient.Exists()))
                continue;

              SemaphoreSlim.Wait();
              DownloadTasks.Add(System.Threading.Tasks.Task.Run(() =>
              {
                System.String FileName = System.IO.Path.Combine(TempPath, System.IO.Path.GetRandomFileName());

                using (System.IO.FileStream FileStream = System.IO.File.OpenWrite(FileName))
                  ShareFileClient.Download().Value.Content.CopyTo(FileStream);

                FilesToCompression.Add(FileName, (FilesToCompression.ContainsValue(EntryName.Value) ? $"{System.DateTimeOffset.UtcNow:ddMMyyyyHHmmssfff}_{EntryName.Value}" : EntryName.Value));

                SemaphoreSlim.Release();
              }
              ));
            }
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
    public override SoftmakeAll.SDK.OperationResult Delete(System.String ShareName, System.String[] EntriesNames)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(ShareName)) || (EntriesNames == null) || (EntriesNames.Length == 0))
      {
        OperationResult.Message = "The ShareName and EntriesNames cannot be null.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient ShareClient = new global::Azure.Storage.Files.Shares.ShareClient(this.ScopedConnectionString, ShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient ShareDirectoryClient = ShareClient.GetRootDirectoryClient();

        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(EntriesNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> DeleteTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          foreach (System.String EntryName in EntriesNames)
          {
            global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(EntryName);
            if (!(ShareFileClient.Exists()))
              continue;

            SemaphoreSlim.Wait();
            DeleteTasks.Add(System.Threading.Tasks.Task.Run(() => { ShareFileClient.Delete(); SemaphoreSlim.Release(); }));
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
    public override SoftmakeAll.SDK.OperationResult Copy(System.String SourceShareName, System.String[] SourceEntriesNames, System.String TargetShareName, System.String[] TargetEntriesNames, System.Boolean Overwrite)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(SourceShareName)) || (SourceEntriesNames == null) || (SourceEntriesNames.Length == 0) || (System.String.IsNullOrWhiteSpace(TargetShareName)) || (TargetEntriesNames == null) || (TargetEntriesNames.Length == 0))
      {
        OperationResult.Message = "The SourceShareName, SourceEntriesNames, TargetShareName and TargetEntriesNames cannot be null.";
        return OperationResult;
      }

      if (SourceEntriesNames.Length != TargetEntriesNames.Length)
      {
        OperationResult.Message = "The SourceEntriesNames and TargetEntriesNames must be the same length.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient SourceShareClient = new global::Azure.Storage.Files.Shares.ShareClient(this.ScopedConnectionString, SourceShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient SourceShareDirectoryClient = SourceShareClient.GetRootDirectoryClient();

        global::Azure.Storage.Files.Shares.ShareClient TargetShareClient;
        if (SourceShareName != TargetShareName)
          TargetShareClient = new global::Azure.Storage.Files.Shares.ShareClient(this.ScopedConnectionString, TargetShareName);
        else
          TargetShareClient = SourceShareClient;
        global::Azure.Storage.Files.Shares.ShareDirectoryClient TargetShareDirectoryClient = TargetShareClient.GetRootDirectoryClient();


        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(SourceEntriesNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> CopyTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          for (System.Int32 i = 0; i < SourceEntriesNames.Length; i++)
          {
            System.String SourceEntryName = SourceEntriesNames[i];
            global::Azure.Storage.Files.Shares.ShareFileClient SourceShareFileClient = SourceShareDirectoryClient.GetFileClient(SourceEntryName);
            if (!(SourceShareFileClient.Exists()))
              continue;

            System.String TargetEntryName = TargetEntriesNames[i];
            global::Azure.Storage.Files.Shares.ShareFileClient TargetShareFileClient = SourceShareDirectoryClient.GetFileClient(TargetEntryName);
            if ((!(Overwrite)) && (TargetShareFileClient.Exists()))
              continue;

            SemaphoreSlim.Wait();
            CopyTasks.Add(System.Threading.Tasks.Task.Run(() => { TargetShareFileClient.StartCopy(SourceShareFileClient.Uri); SemaphoreSlim.Release(); }));
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
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> UploadAsync(System.String ShareName, System.String EntryName, System.IO.Stream Contents)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(ShareName, EntryName))
      {
        OperationResult.Message = "ShareName, EntryName and Contents are required.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient ShareClient = new global::Azure.Storage.Files.Shares.ShareClient(this.ScopedConnectionString, ShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient ShareDirectoryClient = ShareClient.GetRootDirectoryClient();
        global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(EntryName);
        await ShareFileClient.CreateAsync(Contents.Length);
        await ShareFileClient.UploadAsync(Contents);
      }
      catch (System.Exception ex)
      {
        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DownloadAsync(System.String ShareName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames, System.IO.Stream Destination)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(ShareName)) || (EntriesNames == null) || (EntriesNames.Count == 0) || (Destination == null))
      {
        OperationResult.Message = "ShareName, EntryName and Destination are required.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient ShareClient = new global::Azure.Storage.Files.Shares.ShareClient(this.ScopedConnectionString, ShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient ShareDirectoryClient = ShareClient.GetRootDirectoryClient();
        if (EntriesNames.Count == 1)
        {
          global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(EntriesNames.First().Key);

          if (!(await ShareFileClient.ExistsAsync()))
          {
            OperationResult.Message = "File not found.";
            return OperationResult;
          }

          await (await ShareFileClient.DownloadAsync()).Value.Content.CopyToAsync(Destination);
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
              global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(EntryName.Key);
              if (!(await ShareFileClient.ExistsAsync()))
                continue;

              await SemaphoreSlim.WaitAsync();
              DownloadTasks.Add(System.Threading.Tasks.Task.Run(async () =>
              {
                System.String FileName = System.IO.Path.Combine(TempPath, System.IO.Path.GetRandomFileName());

                using (System.IO.FileStream FileStream = System.IO.File.OpenWrite(FileName))
                  await (await ShareFileClient.DownloadAsync()).Value.Content.CopyToAsync(FileStream);

                FilesToCompression.Add(FileName, (FilesToCompression.ContainsValue(EntryName.Value) ? $"{System.DateTimeOffset.UtcNow:ddMMyyyyHHmmssfff}_{EntryName.Value}" : EntryName.Value));

                SemaphoreSlim.Release();
              }
              ));
            }
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
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String ShareName, System.String[] EntriesNames)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(ShareName)) || (EntriesNames == null) || (EntriesNames.Length == 0))
      {
        OperationResult.Message = "The ShareName and EntriesNames cannot be null.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient ShareClient = new global::Azure.Storage.Files.Shares.ShareClient(this.ScopedConnectionString, ShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient ShareDirectoryClient = ShareClient.GetRootDirectoryClient();

        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(EntriesNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> DeleteTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          foreach (System.String EntryName in EntriesNames)
          {
            global::Azure.Storage.Files.Shares.ShareFileClient ShareFileClient = ShareDirectoryClient.GetFileClient(EntryName);
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
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceShareName, System.String[] SourceEntriesNames, System.String TargetShareName, System.String[] TargetEntriesNames, System.Boolean Overwrite)
    {
      SoftmakeAll.SDK.CloudStorage.Azure.Environment.Validate(this.ScopedConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(SourceShareName)) || (SourceEntriesNames == null) || (SourceEntriesNames.Length == 0) || (System.String.IsNullOrWhiteSpace(TargetShareName)) || (TargetEntriesNames == null) || (TargetEntriesNames.Length == 0))
      {
        OperationResult.Message = "The SourceShareName, SourceEntriesNames, TargetShareName and TargetEntriesNames cannot be null.";
        return OperationResult;
      }

      if (SourceEntriesNames.Length != TargetEntriesNames.Length)
      {
        OperationResult.Message = "The SourceEntriesNames and TargetEntriesNames must be the same length.";
        return OperationResult;
      }

      try
      {
        global::Azure.Storage.Files.Shares.ShareClient SourceShareClient = new global::Azure.Storage.Files.Shares.ShareClient(this.ScopedConnectionString, SourceShareName);
        global::Azure.Storage.Files.Shares.ShareDirectoryClient SourceShareDirectoryClient = SourceShareClient.GetRootDirectoryClient();

        global::Azure.Storage.Files.Shares.ShareClient TargetShareClient;
        if (SourceShareName != TargetShareName)
          TargetShareClient = new global::Azure.Storage.Files.Shares.ShareClient(this.ScopedConnectionString, TargetShareName);
        else
          TargetShareClient = SourceShareClient;
        global::Azure.Storage.Files.Shares.ShareDirectoryClient TargetShareDirectoryClient = TargetShareClient.GetRootDirectoryClient();


        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(SourceEntriesNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> CopyTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          for (System.Int32 i = 0; i < SourceEntriesNames.Length; i++)
          {
            System.String SourceEntryName = SourceEntriesNames[i];
            global::Azure.Storage.Files.Shares.ShareFileClient SourceShareFileClient = SourceShareDirectoryClient.GetFileClient(SourceEntryName);
            if (!(await SourceShareFileClient.ExistsAsync()))
              continue;

            System.String TargetEntryName = TargetEntriesNames[i];
            global::Azure.Storage.Files.Shares.ShareFileClient TargetShareFileClient = SourceShareDirectoryClient.GetFileClient(TargetEntryName);
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
    #endregion
    #endregion
  }
}