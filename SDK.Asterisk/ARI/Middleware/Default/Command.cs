namespace SoftmakeAll.SDK.Asterisk.ARI.Middleware.Default
{
  public class Command : IRestCommand
  {
    #region Constructor
    public Command(StasisEndpoint info, string path) => this.Client = new SoftmakeAll.SDK.Communication.REST() { URL = $"{info.AriEndPoint}/{path}", AuthorizationBasicBase64 = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{info.Username}:{info.Password}")) };
    #endregion

    #region Properties
    internal SoftmakeAll.SDK.Communication.REST Client { get; }
    public string Url { get; set; }
    public string Method
    {
      get => this.Client.Method;
      set => this.Client.Method = value;
    }
    #endregion

    #region Methods
    public void AddUrlSegment(string segName, string value) => this.Client.URL = this.Client.URL.Replace(System.String.Concat("{", segName, "}"), value);
    public void AddParameter(string name, object value, Middleware.ParameterType type)
    {
      switch (type)
      {
        case ParameterType.QueryString:
          this.Client.URL = $"{this.Client.URL}{(this.Client.URL.IndexOf('?') == -1 ? '?' : '&')}{name}={value}";
          break;

        case ParameterType.RequestBody:
          break;

        default:
          break;
      }
    }
    #endregion
  }
}
