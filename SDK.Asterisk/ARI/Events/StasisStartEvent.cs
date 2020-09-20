namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class StasisStartEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public System.Collections.Generic.List<string> Args { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Replace_channel { get; set; }
    #endregion
  }
}
