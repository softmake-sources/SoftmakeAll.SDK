namespace SoftmakeAll.SDK.Asterisk.ARI
{
  public class AriException : System.Exception
  {
    #region Constructors
    public AriException(string message) : base(message) { }
    public AriException(string message, int StatusCode) : base(message) => this.StatusCode = StatusCode;
    #endregion

    #region Properties
    public System.Int32 StatusCode { get; set; }
    #endregion
  }
}