namespace SoftmakeAll.SDK.CloudStorage
{
  public interface IFile
  {
    #region Methods
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> UploadAsync(System.String ContainerName, System.String StorageFileName, System.IO.Stream FileContents);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ContainerName, System.String StorageFileName);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ContainerName, System.String[] StorageFileNames);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String ContainerName, System.Collections.Generic.Dictionary<System.String, System.String> StorageFileNames);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String ContainerName, System.String StorageFileName);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String ContainerName, System.String[] StorageFileNames);
    #endregion
  }
}