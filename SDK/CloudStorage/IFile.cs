namespace SoftmakeAll.SDK.CloudStorage
{
  public interface IFile
  {
    #region Methods
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> UploadAsync(System.String ContainerName, System.String StorageFileName, System.IO.Stream FileContents);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ContainerName, System.String StorageFileName);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ContainerName, System.String[] StorageFileNames);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ContainerName, System.Collections.Generic.Dictionary<System.String, System.String> StorageFileNames);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String ContainerName, System.String StorageFileName);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String ContainerName, System.String[] StorageFileNames);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceContainerName, System.String SourceStorageFileName, System.String TargetStorageFileName, System.Boolean Overwrite);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceContainerName, System.String[] SourceStorageFileNames, System.String[] TargetStorageFileNames, System.Boolean Overwrite);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceContainerName, System.String SourceStorageFileName, System.String TargetContainerName, System.String TargetStorageFileName, System.Boolean Overwrite);
    System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceContainerName, System.String[] SourceStorageFileNames, System.String TargetContainerName, System.String[] TargetStorageFileNames, System.Boolean Overwrite);
    #endregion
  }
}