using System.Linq;

namespace SoftmakeAll.SDK.CloudStorage
{
  public abstract class Repository : SoftmakeAll.SDK.CloudStorage.IRepository
  {
    #region Constructor
    public Repository() { }
    #endregion

    #region Methods
    #region Virtual Methods
    public virtual System.String GenerateDownloadURL(System.String RepositoryName, System.String EntryName, System.String OriginalName) => this.GenerateDownloadURL(RepositoryName, EntryName, OriginalName, System.DateTimeOffset.UtcNow.AddMinutes(5));
    public virtual SoftmakeAll.SDK.OperationResult<System.Byte[]> Upload(System.String RepositoryName, System.String EntryName, System.Byte[] Contents)
    {
      using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
      {
        MemoryStream.Write(Contents, 0, Contents.Length);
        return this.Upload(RepositoryName, EntryName, MemoryStream);
      }
    }
    public virtual SoftmakeAll.SDK.OperationResult<System.Byte[]> Download(System.String RepositoryName, System.String EntryName) => this.Download(RepositoryName, new System.String[] { EntryName });
    public virtual SoftmakeAll.SDK.OperationResult<System.Byte[]> Download(System.String RepositoryName, System.String[] EntriesNames) => this.Download(RepositoryName, EntriesNames?.ToDictionary(k => k, v => v));
    public virtual SoftmakeAll.SDK.OperationResult<System.Byte[]> Download(System.String RepositoryName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames)
    {
      SoftmakeAll.SDK.OperationResult<System.Byte[]> Result = new SoftmakeAll.SDK.OperationResult<System.Byte[]>();
      System.String FilePath = System.IO.Path.GetTempFileName();
      using (System.IO.FileStream FileStream = new System.IO.FileStream(FilePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Delete, 4096, System.IO.FileOptions.DeleteOnClose))
      {
        SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();
        if ((OperationResult = this.Download(RepositoryName, EntriesNames, FileStream)).ExitCode == 0)
          using (System.IO.BinaryReader BinaryReader = new System.IO.BinaryReader(FileStream))
            Result.Data = BinaryReader.ReadBytes((System.Int32)FileStream.Length);

        Result.ExitCode = OperationResult.ExitCode;
        Result.Message = OperationResult.Message;
      }

      return Result;
    }
    public virtual SoftmakeAll.SDK.OperationResult Download(System.String RepositoryName, System.String EntryName, System.IO.Stream Destination) => this.Download(RepositoryName, new System.String[] { EntryName }, Destination);
    public virtual SoftmakeAll.SDK.OperationResult Download(System.String RepositoryName, System.String[] EntriesNames, System.IO.Stream Destination) => this.Download(RepositoryName, EntriesNames?.ToDictionary(k => k, v => v), Destination);
    public virtual SoftmakeAll.SDK.OperationResult Delete(System.String RepositoryName, System.String EntryName) => this.Delete(RepositoryName, new System.String[] { EntryName });
    public virtual SoftmakeAll.SDK.OperationResult Copy(System.String SourceRepositoryName, System.String SourceEntryName, System.String TargetRepositoryName, System.Boolean Overwrite) => this.Copy(SourceRepositoryName, new System.String[] { SourceEntryName }, SourceRepositoryName, new System.String[] { TargetRepositoryName }, Overwrite);
    public virtual SoftmakeAll.SDK.OperationResult Copy(System.String SourceRepositoryName, System.String[] SourceEntriesNames, System.String[] TargetRepositoryName, System.Boolean Overwrite) => this.Copy(SourceRepositoryName, SourceEntriesNames, SourceRepositoryName, TargetRepositoryName, Overwrite);
    public virtual SoftmakeAll.SDK.OperationResult Copy(System.String SourceRepositoryName, System.String SourceEntryName, System.String TargetRepositoryName, System.String TargetEntryName, System.Boolean Overwrite) => this.Copy(SourceRepositoryName, new System.String[] { SourceEntryName }, TargetRepositoryName, new System.String[] { TargetEntryName }, Overwrite);
    #endregion

    #region Abstract Methods
    public abstract System.String GenerateDownloadURL(System.String RepositoryName, System.String EntryName, System.String OriginalName, System.DateTimeOffset ExpirationDateTime);
    public abstract SoftmakeAll.SDK.OperationResult<System.Byte[]> Upload(System.String RepositoryName, System.String EntryName, System.IO.Stream Contents);
    public abstract SoftmakeAll.SDK.OperationResult Download(System.String RepositoryName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames, System.IO.Stream Destination);
    public abstract SoftmakeAll.SDK.OperationResult Delete(System.String RepositoryName, System.String[] EntriesNames);
    public abstract SoftmakeAll.SDK.OperationResult Copy(System.String SourceRepositoryName, System.String[] SourceEntriesNames, System.String TargetRepositoryName, System.String[] TargetEntriesNames, System.Boolean Overwrite);
    #endregion

    #region Async Methods
    #region Virtual Methods
    public virtual async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> UploadAsync(System.String RepositoryName, System.String EntryName, System.Byte[] Contents)
    {
      using (System.IO.MemoryStream MemoryStream = new System.IO.MemoryStream())
      {
        await MemoryStream.WriteAsync(Contents, 0, Contents.Length);
        return await this.UploadAsync(RepositoryName, EntryName, MemoryStream);
      }
    }
    public virtual async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String RepositoryName, System.String EntryName) => await this.DownloadAsync(RepositoryName, new System.String[] { EntryName });
    public virtual async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String RepositoryName, System.String[] EntriesNames) => await this.DownloadAsync(RepositoryName, EntriesNames?.ToDictionary(k => k, v => v));
    public virtual async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> DownloadAsync(System.String RepositoryName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames)
    {
      SoftmakeAll.SDK.OperationResult<System.Byte[]> Result = new SoftmakeAll.SDK.OperationResult<System.Byte[]>();
      System.String FilePath = System.IO.Path.GetTempFileName();
      using (System.IO.FileStream FileStream = new System.IO.FileStream(FilePath, System.IO.FileMode.CreateNew, System.IO.FileAccess.ReadWrite, System.IO.FileShare.Delete, 4096, System.IO.FileOptions.DeleteOnClose))
      {
        SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();
        if ((OperationResult = await this.DownloadAsync(RepositoryName, EntriesNames, FileStream)).ExitCode == 0)
          using (System.IO.BinaryReader BinaryReader = new System.IO.BinaryReader(FileStream))
            Result.Data = BinaryReader.ReadBytes((System.Int32)FileStream.Length);

        Result.ExitCode = OperationResult.ExitCode;
        Result.Message = OperationResult.Message;
      }

      return Result;
    }
    public virtual async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DownloadAsync(System.String RepositoryName, System.String EntryName, System.IO.Stream Destination) => await this.DownloadAsync(RepositoryName, new System.String[] { EntryName }, Destination);
    public virtual async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DownloadAsync(System.String RepositoryName, System.String[] EntriesNames, System.IO.Stream Destination) => await this.DownloadAsync(RepositoryName, EntriesNames?.ToDictionary(k => k, v => v), Destination);
    public virtual async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String RepositoryName, System.String EntryName) => await this.DeleteAsync(RepositoryName, new System.String[] { EntryName });
    public virtual async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceRepositoryName, System.String SourceEntryName, System.String TargetRepositoryName, System.Boolean Overwrite) => await this.CopyAsync(SourceRepositoryName, new System.String[] { SourceEntryName }, SourceRepositoryName, new System.String[] { TargetRepositoryName }, Overwrite);
    public virtual async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceRepositoryName, System.String[] SourceEntriesNames, System.String[] TargetRepositoryName, System.Boolean Overwrite) => await this.CopyAsync(SourceRepositoryName, SourceEntriesNames, SourceRepositoryName, TargetRepositoryName, Overwrite);
    public virtual async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceRepositoryName, System.String SourceEntryName, System.String TargetRepositoryName, System.String TargetEntryName, System.Boolean Overwrite) => await this.CopyAsync(SourceRepositoryName, new System.String[] { SourceEntryName }, TargetRepositoryName, new System.String[] { TargetEntryName }, Overwrite);
    #endregion

    #region Abstract Methods
    public abstract System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Byte[]>> UploadAsync(System.String RepositoryName, System.String EntryName, System.IO.Stream Contents);
    public abstract System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DownloadAsync(System.String RepositoryName, System.Collections.Generic.Dictionary<System.String, System.String> EntriesNames, System.IO.Stream Destination);
    public abstract System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> DeleteAsync(System.String RepositoryName, System.String[] EntriesNames);
    public abstract System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> CopyAsync(System.String SourceRepositoryName, System.String[] SourceEntriesNames, System.String TargetRepositoryName, System.String[] TargetEntriesNames, System.Boolean Overwrite);
    #endregion
    #endregion

    #endregion
  }
}