namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class BridgeVideoSourceChangedEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public Bridge Bridge { get; set; }
    public string Old_video_source_id { get; set; }
    #endregion
  }
}