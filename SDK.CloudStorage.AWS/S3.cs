using System.Linq;

namespace SoftmakeAll.SDK.CloudStorage.AWS
{
  public class S3 : SoftmakeAll.SDK.CloudStorage.Repository, SoftmakeAll.SDK.CloudStorage.IRepository
  {
    #region Constructor
    public S3() : base() { }
    #endregion

    #region Fields
    private Amazon.S3.IAmazonS3 _ScopedS3Client;
    private Amazon.S3.IAmazonS3 ScopedS3Client
    {
      get => this._ScopedS3Client ?? SoftmakeAll.SDK.CloudStorage.AWS.Environment._S3Client;
      set => this._ScopedS3Client = value;
    }
    #endregion

    #region Methods
    public void SetScopedS3Client(Amazon.S3.IAmazonS3 S3Client) => this.ScopedS3Client = S3Client;
    public override System.String GenerateDownloadURL(System.String BucketName, System.String EntryName, System.String OriginalName, System.DateTimeOffset ExpirationDateTime)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      if (!(SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(BucketName, EntryName)))
        try
        {
          return this.ScopedS3Client.GetPreSignedURL(new Amazon.S3.Model.GetPreSignedUrlRequest { BucketName = BucketName, Key = EntryName, Expires = System.Convert.ToDateTime(ExpirationDateTime) });
        }
        catch { }

      return null;
    }

    public override SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> Upload(System.String BucketName, System.String EntryName, System.IO.Stream Contents)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(BucketName, EntryName))
      {
        OperationResult.Message = "BucketName, EntryName and Contents are required.";
        return OperationResult;
      }

      try
      {
        using (Amazon.S3.Transfer.TransferUtility TransferUtility = new Amazon.S3.Transfer.TransferUtility(this.ScopedS3Client))
          TransferUtility.Upload(Contents, BucketName, EntryName);
      }
      catch (System.Exception ex)
      {
        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public override SoftmakeAll.SDK.OperationResult Download(System.String BucketName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames, System.IO.Stream Destination)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(BucketName)) || (EntriesNames == null) || (EntriesNames.Count == 0) || (Destination == null))
      {
        OperationResult.Message = "BucketName, EntryName and Destination are required.";
        return OperationResult;
      }

      try
      {
        if (EntriesNames.Count == 1)
        {
          Amazon.S3.Model.GetObjectRequest GetObjectRequest = new Amazon.S3.Model.GetObjectRequest { BucketName = BucketName, Key = EntriesNames.First().Key };

          System.Threading.Tasks.Task.Run(async () =>
          {
            using (Amazon.S3.Model.GetObjectResponse GetObjectResponse = await this.ScopedS3Client.GetObjectAsync(GetObjectRequest))
            using (System.IO.Stream Stream = GetObjectResponse.ResponseStream)
              await Stream.CopyToAsync(Destination);
          }).Wait();

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
              SemaphoreSlim.Wait();
              DownloadTasks.Add(System.Threading.Tasks.Task.Run(async () =>
             {
               Amazon.S3.Model.GetObjectRequest GetObjectRequest = new Amazon.S3.Model.GetObjectRequest { BucketName = BucketName, Key = EntryName.Key };

               System.String FileName = System.IO.Path.Combine(TempPath, System.IO.Path.GetRandomFileName());

               using (Amazon.S3.Model.GetObjectResponse GetObjectResponse = await this.ScopedS3Client.GetObjectAsync(GetObjectRequest))
                 await GetObjectResponse.WriteResponseStreamToFileAsync(FileName, false, new System.Threading.CancellationToken());

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
    public override SoftmakeAll.SDK.OperationResult Delete(System.String BucketName, System.String[] EntriesNames)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(BucketName)) || (EntriesNames == null) || (EntriesNames.Length == 0))
      {
        OperationResult.Message = "The BucketName and EntriesNames cannot be null.";
        return OperationResult;
      }

      System.Collections.Generic.List<Amazon.S3.Model.KeyVersion> Objects = new System.Collections.Generic.List<Amazon.S3.Model.KeyVersion>();
      foreach (System.String EntryName in EntriesNames)
        Objects.Add(new Amazon.S3.Model.KeyVersion() { Key = EntryName });
      Amazon.S3.Model.DeleteObjectsRequest DeleteObjectsRequest = new Amazon.S3.Model.DeleteObjectsRequest { BucketName = BucketName, Objects = Objects };

      try
      {
        this.ScopedS3Client.DeleteObjectsAsync(DeleteObjectsRequest).Wait();
      }
      catch (System.Exception ex)
      {
        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public override SoftmakeAll.SDK.OperationResult Copy(System.String SourceBucketName, System.String[] SourceEntriesNames, System.String TargetBucketName, System.String[] TargetEntriesNames, System.Boolean Overwrite)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(SourceBucketName)) || (SourceEntriesNames == null) || (SourceEntriesNames.Length == 0) || (System.String.IsNullOrWhiteSpace(TargetBucketName)) || (TargetEntriesNames == null) || (TargetEntriesNames.Length == 0))
      {
        OperationResult.Message = "The SourceBucketName, SourceEntriesNames, TargetBucketName and TargetEntriesNames cannot be null.";
        return OperationResult;
      }

      if (SourceEntriesNames.Length != TargetEntriesNames.Length)
      {
        OperationResult.Message = "The SourceEntriesNames and TargetEntriesNames must be the same length.";
        return OperationResult;
      }

      try
      {
        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(SourceEntriesNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> CopyTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          for (System.Int32 i = 0; i < SourceEntriesNames.Length; i++)
          {
            SemaphoreSlim.Wait();
            CopyTasks.Add(System.Threading.Tasks.Task.Run(async () =>
           {
             await this.ScopedS3Client.CopyObjectAsync(new Amazon.S3.Model.CopyObjectRequest { SourceBucket = SourceBucketName, SourceKey = SourceEntriesNames[i], DestinationBucket = TargetBucketName, DestinationKey = TargetEntriesNames[i] });
             SemaphoreSlim.Release();
           }));
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
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> UploadAsync(System.String BucketName, System.String EntryName, System.IO.Stream Contents)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(BucketName, EntryName))
      {
        OperationResult.Message = "BucketName, EntryName and Contents are required.";
        return OperationResult;
      }

      try
      {
        using (Amazon.S3.Transfer.TransferUtility TransferUtility = new Amazon.S3.Transfer.TransferUtility(this.ScopedS3Client))
          await TransferUtility.UploadAsync(Contents, BucketName, EntryName);
      }
      catch (System.Exception ex)
      {
        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DownloadAsync(System.String BucketName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames, System.IO.Stream Destination)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(BucketName)) || (EntriesNames == null) || (EntriesNames.Count == 0) || (Destination == null))
      {
        OperationResult.Message = "BucketName, EntryName and Destination are required.";
        return OperationResult;
      }

      try
      {
        if (EntriesNames.Count == 1)
        {
          Amazon.S3.Model.GetObjectRequest GetObjectRequest = new Amazon.S3.Model.GetObjectRequest { BucketName = BucketName, Key = EntriesNames.First().Key };
          using (Amazon.S3.Model.GetObjectResponse GetObjectResponse = await this.ScopedS3Client.GetObjectAsync(GetObjectRequest))
          using (System.IO.Stream Stream = GetObjectResponse.ResponseStream)
            await Stream.CopyToAsync(Destination);
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
              await SemaphoreSlim.WaitAsync();
              DownloadTasks.Add(System.Threading.Tasks.Task.Run(async () =>
              {
                Amazon.S3.Model.GetObjectRequest GetObjectRequest = new Amazon.S3.Model.GetObjectRequest { BucketName = BucketName, Key = EntryName.Key };

                System.String FileName = System.IO.Path.Combine(TempPath, System.IO.Path.GetRandomFileName());

                using (Amazon.S3.Model.GetObjectResponse GetObjectResponse = await this.ScopedS3Client.GetObjectAsync(GetObjectRequest))
                  await GetObjectResponse.WriteResponseStreamToFileAsync(FileName, false, new System.Threading.CancellationToken());

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
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String BucketName, System.String[] EntriesNames)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(BucketName)) || (EntriesNames == null) || (EntriesNames.Length == 0))
      {
        OperationResult.Message = "The BucketName and EntriesNames cannot be null.";
        return OperationResult;
      }

      System.Collections.Generic.List<Amazon.S3.Model.KeyVersion> Objects = new System.Collections.Generic.List<Amazon.S3.Model.KeyVersion>();
      foreach (System.String EntryName in EntriesNames)
        Objects.Add(new Amazon.S3.Model.KeyVersion() { Key = EntryName });
      Amazon.S3.Model.DeleteObjectsRequest DeleteObjectsRequest = new Amazon.S3.Model.DeleteObjectsRequest { BucketName = BucketName, Objects = Objects };

      try
      {
        await this.ScopedS3Client.DeleteObjectsAsync(DeleteObjectsRequest);
      }
      catch (System.Exception ex)
      {
        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceBucketName, System.String[] SourceEntriesNames, System.String TargetBucketName, System.String[] TargetEntriesNames, System.Boolean Overwrite)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(SourceBucketName)) || (SourceEntriesNames == null) || (SourceEntriesNames.Length == 0) || (System.String.IsNullOrWhiteSpace(TargetBucketName)) || (TargetEntriesNames == null) || (TargetEntriesNames.Length == 0))
      {
        OperationResult.Message = "The SourceBucketName, SourceEntriesNames, TargetBucketName and TargetEntriesNames cannot be null.";
        return OperationResult;
      }

      if (SourceEntriesNames.Length != TargetEntriesNames.Length)
      {
        OperationResult.Message = "The SourceEntriesNames and TargetEntriesNames must be the same length.";
        return OperationResult;
      }

      try
      {
        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(SourceEntriesNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> CopyTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          for (System.Int32 i = 0; i < SourceEntriesNames.Length; i++)
          {
            await SemaphoreSlim.WaitAsync();
            CopyTasks.Add(System.Threading.Tasks.Task.Run(async () =>
            {
              await this.ScopedS3Client.CopyObjectAsync(new Amazon.S3.Model.CopyObjectRequest { SourceBucket = SourceBucketName, SourceKey = SourceEntriesNames[i], DestinationBucket = TargetBucketName, DestinationKey = TargetEntriesNames[i] });
              SemaphoreSlim.Release();
            }));
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