namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ChannelTalkingFinishedEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    public int Duration { get; set; }
    #endregion
  }
}