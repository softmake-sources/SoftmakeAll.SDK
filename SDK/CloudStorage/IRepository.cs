namespace SoftmakeAll.SDK.CloudStorage
{
  public interface IRepository
  {
    #region Methods
    System.String GenerateDownloadURL(System.String RepositoryName, System.String EntryName, System.String OriginalName);
    System.String GenerateDownloadURL(System.String RepositoryName, System.String EntryName, System.String OriginalName, System.DateTimeOffset ExpirationDateTime);

    SoftmakeAll.SDK.OperationResult<System.Byte[]> Upload(System.String RepositoryName, System.String EntryName, System.IO.Stream Contents);
    SoftmakeAll.SDK.OperationResult<System.Byte[]> Upload(System.String RepositoryName, System.String EntryName, System.Byte[] Contents);
    SoftmakeAll.SDK.OperationResult<System.Byte[]> Download(System.String RepositoryName, System.String EntryName);
    SoftmakeAll.SDK.OperationResult<System.Byte[]> Download(System.String RepositoryName, System.String[] EntriesNames);
    SoftmakeAll.SDK.OperationResult<System.Byte[]> Download(System.String RepositoryName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames);
    SoftmakeAll.SDK.OperationResult Download(System.String RepositoryName, System.String EntryName, System.IO.Stream Destination);
    SoftmakeAll.SDK.OperationResult Download(System.String RepositoryName, System.String[] EntriesNames, System.IO.Stream Destination);
    SoftmakeAll.SDK.OperationResult Download(System.String RepositoryName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames, System.IO.Stream Destination);
    SoftmakeAll.SDK.OperationResult Delete(System.String RepositoryName, System.String EntryName);
    SoftmakeAll.SDK.OperationResult Delete(System.String RepositoryName, System.String[] EntriesNames);
    SoftmakeAll.SDK.OperationResult Copy(System.String SourceRepositoryName, System.String SourceEntryName, System.String TargetEntryName, System.Boolean Overwrite);
    SoftmakeAll.SDK.OperationResult Copy(System.String SourceRepositoryName, System.String[] SourceEntriesNames, System.String[] TargetEntriesNames, System.Boolean Overwrite);
    SoftmakeAll.SDK.OperationResult Copy(System.String SourceRepositoryName, System.String SourceEntryName, System.String TargetRepositoryName, System.String TargetEntryName, System.Boolean Overwrite);
    SoftmakeAll.SDK.OperationResult Copy(System.String SourceRepositoryName, System.String[] SourceEntriesNames, System.String TargetRepositoryName, System.String[] TargetEntriesNames, System.Boolean Overwrite);

    #region Async Methods
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> UploadAsync(System.String RepositoryName, System.String EntryName, System.IO.Stream Contents);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> UploadAsync(System.String RepositoryName, System.String EntryName, System.Byte[] Contents);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String RepositoryName, System.String EntryName);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String RepositoryName, System.String[] EntriesNames);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String RepositoryName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DownloadAsync(System.String RepositoryName, System.String EntryName, System.IO.Stream Destination);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DownloadAsync(System.String RepositoryName, System.String[] EntriesNames, System.IO.Stream Destination);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DownloadAsync(System.String RepositoryName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames, System.IO.Stream Destination);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String RepositoryName, System.String EntryName);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String RepositoryName, System.String[] EntriesNames);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceRepositoryName, System.String SourceEntryName, System.String TargetEntryName, System.Boolean Overwrite);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceRepositoryName, System.String[] SourceEntriesNames, System.String[] TargetEntriesNames, System.Boolean Overwrite);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceRepositoryName, System.String SourceEntryName, System.String TargetRepositoryName, System.String TargetEntryName, System.Boolean Overwrite);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceRepositoryName, System.String[] SourceEntriesNames, System.String TargetRepositoryName, System.String[] TargetEntriesNames, System.Boolean Overwrite);
    #endregion
    #endregion
  }
}