namespace SoftmakeAll.SDK.Asterisk.ARI.Middleware
{
  public enum ParameterType
  {
    RequestBody,
    QueryString
  }

  public interface IRestCommand
  {
    #region Properties
    public string Url { get; set; }
    public string Method { get; set; }
    #endregion

    #region Methods
    public void AddUrlSegment(string segName, string value);
    public void AddParameter(string name, object value, ParameterType type);
    #endregion
  }
}