namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ApplicationMoveFailedEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    public string Destination { get; set; }
    public System.Collections.Generic.List<string> Args { get; set; }
    #endregion
  }
}