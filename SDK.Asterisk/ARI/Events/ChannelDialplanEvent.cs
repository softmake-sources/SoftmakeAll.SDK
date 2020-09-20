namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ChannelDialplanEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    public string Dialplan_app { get; set; }
    public string Dialplan_app_data { get; set; }
    #endregion
  }
}