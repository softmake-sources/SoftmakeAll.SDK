namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ChannelCallerIdEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public int Caller_presentation { get; set; }
    public string Caller_presentation_txt { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    #endregion
  }
}