namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ChannelLeftBridgeEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public Bridge Bridge { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    #endregion
  }
}
