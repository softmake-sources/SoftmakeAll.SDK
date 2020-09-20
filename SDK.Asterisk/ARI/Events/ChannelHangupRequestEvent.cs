namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ChannelHangupRequestEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public int Cause { get; set; }
    public bool Soft { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    #endregion
  }
}
