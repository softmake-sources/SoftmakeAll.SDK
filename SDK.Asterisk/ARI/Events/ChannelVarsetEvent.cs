namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ChannelVarsetEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public string Variable { get; set; }
    public string Value { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    #endregion
  }
}
