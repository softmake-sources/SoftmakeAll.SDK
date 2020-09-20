namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class Channel
  {
    #region Properties
    public string Id { get; set; }
    public System.String CallID => System.String.IsNullOrWhiteSpace(Id) ? null : Id.Split('.')[0];
    public string Name { get; set; }
    public string State { get; set; }
    public CallerID Caller { get; set; }
    public CallerID Connected { get; set; }
    public string Accountcode { get; set; }
    public DialplanCEP Dialplan { get; set; }
    public System.DateTimeOffset Creationtime { get; set; }
    public string Language { get; set; }
    public object Channelvars { get; set; }
    #endregion
  }
}