namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public class SupportsDownloading<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>
  {
    #region Constructor
    public SupportsDownloading(System.String Route) : base(Route) { }
    #endregion

    #region Methods
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.Communication.REST.File> DownloadFileAsync(System.String QueryString)
    {
      SoftmakeAll.SDK.Communication.REST REST = new SoftmakeAll.SDK.Communication.REST();
      REST.Method = "GET";
      REST.URL = $"{SoftmakeAll.SDK.Fluent.SDKContext.APIBaseAddress}/{base.Route}/{QueryString}";
      SoftmakeAll.SDK.Communication.REST.File File = await REST.DownloadFileAsync();

      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.ExitCode = 0;
      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.Count = 1;
      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.Message = "";
      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.ID = null;

      if (REST.HasRequestErrors)
      {
        SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.ExitCode = (int)REST.StatusCode;
        SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.Count = 0;
      }

      return File;
    }
    #endregion
  }
}