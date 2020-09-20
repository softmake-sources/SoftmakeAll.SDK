namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class DialEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Caller { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Peer { get; set; }
    public string Forward { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Forwarded { get; set; }
    public string Dialstring { get; set; }
    public string Dialstatus { get; set; }
    #endregion
  }
}
