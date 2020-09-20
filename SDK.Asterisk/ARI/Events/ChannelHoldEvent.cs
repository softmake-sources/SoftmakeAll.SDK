namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ChannelHoldEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    public string Musicclass { get; set; }
    #endregion
  }
}