namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ChannelDestroyedEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public int Cause { get; set; }
    public string Cause_txt { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    #endregion
  }
}
