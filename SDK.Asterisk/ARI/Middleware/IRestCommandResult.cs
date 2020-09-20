namespace SoftmakeAll.SDK.Asterisk.ARI.Middleware
{
  public interface IRestCommandResult<T> where T : new()
  {
    #region Properties
    public string UniqueId { get; set; }
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public T Data { get; set; }
    #endregion
  }

  public interface IRestCommandResult
  {
    #region Properties
    public string UniqueId { get; set; }
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public byte[] RawData { get; set; }
    #endregion
  }
}