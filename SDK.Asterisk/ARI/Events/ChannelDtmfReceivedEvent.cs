namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ChannelDtmfReceivedEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public string Digit { get; set; }
    public int Duration_ms { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    #endregion
  }
}