using System.Linq;

namespace SoftmakeAll.SDK.CloudStorage.AWS
{
  public class S3 : SoftmakeAll.SDK.CloudStorage.IFile
  {
    #region Constructor
    public S3() => this.ScopedS3Client = SoftmakeAll.SDK.CloudStorage.AWS.Environment._S3Client;
    #endregion

    #region Fields
    private Amazon.S3.IAmazonS3 ScopedS3Client;
    #endregion

    #region Methods
    public void SetScopedS3Client(Amazon.S3.IAmazonS3 S3Client) => this.ScopedS3Client = S3Client;
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> UploadAsync(System.String BucketName, System.String StorageFileName, System.IO.Stream FileContents)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();

      if (SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(BucketName, StorageFileName))
      {
        OperationResult.Message = "The BucketName and StorageFileName cannot be null.";
        return OperationResult;
      }

      try
      {
        using (Amazon.S3.Transfer.TransferUtility TransferUtility = new Amazon.S3.Transfer.TransferUtility(this.ScopedS3Client))
          await TransferUtility.UploadAsync(FileContents, BucketName, StorageFileName);
      }
      catch (System.Exception ex)
      {
        OperationResult.Message = ex.Message;
        return OperationResult;
      }

      OperationResult.ExitCode = 0;
      return OperationResult;
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String BucketName, System.String StorageFileName)
    {
      return await this.DownloadAsync(BucketName, new System.String[] { StorageFileName });
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String BucketName, System.String[] StorageFileNames)
    {
      System.Collections.Generic.Dictionary<System.String, System.String> StorageFileNamesDictionary = null;
      if ((StorageFileNames != null) && (StorageFileNames.Length > 0))
      {
        StorageFileNamesDictionary = new System.Collections.Generic.Dictionary<System.String, System.String>();
        foreach (System.String StorageFileName in StorageFileNames)
          StorageFileNamesDictionary.Add(StorageFileName, StorageFileName);
      }
      return await this.DownloadAsync(BucketName, StorageFileNamesDictionary);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String BucketName, System.Collections.Generic.Dictionary<System.String, System.String> StorageFileNames)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult<System.Byte[]> OperationResult = new SoftmakeAll.SDK.OperationResult<System.Byte[]>();

      if ((System.String.IsNullOrWhiteSpace(BucketName)) || (StorageFileNames == null) || (StorageFileNames.Count == 0))
      {
        OperationResult.Message = "The BucketName and StorageFileName cannot be null.";
        return OperationResult;
      }

      try
      {
        if (StorageFileNames.Count == 1)
        {
          Amazon.S3.Model.GetObjectRequest GetObjectRequest = new Amazon.S3.Model.GetObjectRequest { BucketName = BucketName, Key = StorageFileNames.First().Key };
          using (Amazon.S3.Model.GetObjectResponse GetObjectResponse = await this.ScopedS3Client.GetObjectAsync(GetObjectRequest))
          using (System.IO.Stream Stream = GetObjectResponse.ResponseStream)
          using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
          {
            await Stream.CopyToAsync(MemoryStream);
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
              await SemaphoreSlim.WaitAsync();
              DownloadTasks.Add(System.Threading.Tasks.Task.Run(async () =>
              {
                Amazon.S3.Model.GetObjectRequest GetObjectRequest = new Amazon.S3.Model.GetObjectRequest { BucketName = BucketName, Key = StorageFileName.Key };
                using (Amazon.S3.Model.GetObjectResponse GetObjectResponse = await this.ScopedS3Client.GetObjectAsync(GetObjectRequest))
                  await GetObjectResponse.WriteResponseStreamToFileAsync(StorageFileName.Key, false, new System.Threading.CancellationToken());
                SemaphoreSlim.Release();
              }));
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
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String BucketName, System.String StorageFileName)
    {
      return await this.DeleteAsync(BucketName, new System.String[] { StorageFileName });
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String BucketName, System.String[] StorageFileNames)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(BucketName)) || (StorageFileNames == null) || (StorageFileNames.Length == 0))
      {
        OperationResult.Message = "The BucketName and StorageFileName cannot be null.";
        return OperationResult;
      }

      System.Collections.Generic.List<Amazon.S3.Model.KeyVersion> Objects = new System.Collections.Generic.List<Amazon.S3.Model.KeyVersion>();
      foreach (System.String StorageFileName in StorageFileNames)
        Objects.Add(new Amazon.S3.Model.KeyVersion() { Key = StorageFileName });
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
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceBucketName, System.String SourceStorageFileName, System.String TargetStorageFileName, System.Boolean Overwrite)
    {
      return await this.CopyAsync(SourceBucketName, new System.String[] { SourceStorageFileName }, SourceBucketName, new System.String[] { TargetStorageFileName }, Overwrite);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceBucketName, System.String[] SourceStorageFileNames, System.String[] TargetStorageFileNames, System.Boolean Overwrite)
    {
      return await this.CopyAsync(SourceBucketName, SourceStorageFileNames, SourceBucketName, TargetStorageFileNames, Overwrite);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceBucketName, System.String SourceStorageFileName, System.String TargetBucketName, System.String TargetStorageFileName, System.Boolean Overwrite)
    {
      return await this.CopyAsync(SourceBucketName, new System.String[] { SourceStorageFileName }, TargetBucketName, new System.String[] { TargetStorageFileName }, Overwrite);
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceBucketName, System.String[] SourceStorageFileNames, System.String TargetBucketName, System.String[] TargetStorageFileNames, System.Boolean Overwrite)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      if ((System.String.IsNullOrWhiteSpace(SourceBucketName)) || (SourceStorageFileNames == null) || (SourceStorageFileNames.Length == 0) || (System.String.IsNullOrWhiteSpace(TargetBucketName)) || (TargetStorageFileNames == null) || (TargetStorageFileNames.Length == 0))
      {
        OperationResult.Message = "The SourceBucketName, SourceStorageFileNames, TargetBucketName and TargetStorageFileNames cannot be null.";
        return OperationResult;
      }

      if (SourceStorageFileNames.Length != TargetStorageFileNames.Length)
      {
        OperationResult.Message = "The SourceStorageFileNames and TargetStorageFileNames must be the same length.";
        return OperationResult;
      }

      try
      {
        using (System.Threading.SemaphoreSlim SemaphoreSlim = new System.Threading.SemaphoreSlim(SourceStorageFileNames.Length))
        {
          System.Collections.Generic.List<System.Threading.Tasks.Task> CopyTasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
          for (System.Int32 i = 0; i < SourceStorageFileNames.Length; i++)
          {
            await SemaphoreSlim.WaitAsync();
            CopyTasks.Add(System.Threading.Tasks.Task.Run(async () =>
            {
              await this.ScopedS3Client.CopyObjectAsync(new Amazon.S3.Model.CopyObjectRequest { SourceBucket = SourceBucketName, SourceKey = SourceStorageFileNames[i], DestinationBucket = TargetBucketName, DestinationKey = TargetStorageFileNames[i] });
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
    public System.String GenerateDownloadURL(System.String BucketName, System.String StorageFileName, System.String OriginalName) => this.GenerateDownloadURL(BucketName, StorageFileName, OriginalName, System.DateTimeOffset.UtcNow.AddMinutes(5));
    public System.String GenerateDownloadURL(System.String BucketName, System.String StorageFileName, System.String OriginalName, System.DateTimeOffset ExpirationDateTime)
    {
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Validate(this.ScopedS3Client);

      if (!(SoftmakeAll.SDK.Helpers.String.Extensions.StringExtensions.IsNullOrWhiteSpace(BucketName, StorageFileName)))
        try
        {
          return this.ScopedS3Client.GetPreSignedURL(new Amazon.S3.Model.GetPreSignedUrlRequest { BucketName = BucketName, Key = StorageFileName, Expires = System.Convert.ToDateTime(ExpirationDateTime) });
        }
        catch { }

      return null;
    }
    #endregion
  }
}