namespace SoftmakeAll.SDK.Asterisk.ARI.Middleware.Default
{
  public class CommandResult<T> : IRestCommandResult<T> where T : new()
  {
    #region Properties
    public string UniqueId { get; set; }
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public T Data { get; set; }
    #endregion
  }

  public class CommandResult : IRestCommandResult
  {
    #region Properties
    public string UniqueId { get; set; }
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public byte[] RawData { get; set; }
    #endregion
  }
}