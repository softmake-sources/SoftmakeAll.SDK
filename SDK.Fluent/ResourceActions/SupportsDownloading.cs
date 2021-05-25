namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// Supports downloading resources.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public class SupportsDownloading<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>
  {
    #region Constructor
    /// <summary>
    /// Supports downloading resources.
    /// </summary>
    /// <param name="Route">The address of the resource.</param>
    public SupportsDownloading(System.String Route) : base(Route) { }
    #endregion

    #region Methods
    /// <summary>
    /// Download the file.
    /// </summary>
    /// <param name="Parameters">Query string parameters.</param>
    /// <returns>The resource as a file.</returns>
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.Communication.REST.File> DownloadFileAsync(System.String Parameters)
    {
      SoftmakeAll.SDK.Communication.REST REST = new SoftmakeAll.SDK.Communication.REST();
      REST.Method = "GET";
      REST.URL = $"{SoftmakeAll.SDK.Fluent.SDKContext.APIBaseAddress}/{base.Route}/{Parameters}";
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