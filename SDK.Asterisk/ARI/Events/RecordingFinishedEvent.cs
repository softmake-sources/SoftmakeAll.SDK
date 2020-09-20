namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class RecordingFinishedEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public LiveRecording Recording { get; set; }
    #endregion
  }
}