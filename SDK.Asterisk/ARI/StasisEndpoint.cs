namespace SoftmakeAll.SDK.Asterisk.ARI
{
  public class StasisEndpoint
  {
    #region Constructor
    public StasisEndpoint(string host, int port, string username, string password, bool ssl = false)
    {
      this.Host = host;
      this.Port = port;
      this.Username = username;
      this.Password = password;
      this.Ssl = ssl;
    }
    #endregion

    #region Properties
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public bool Ssl { get; set; }
    public string AriEndPoint => $"http{(this.Ssl ? "s" : "")}://{this.Host}:{this.Port}/ari";
    #endregion
  }
}