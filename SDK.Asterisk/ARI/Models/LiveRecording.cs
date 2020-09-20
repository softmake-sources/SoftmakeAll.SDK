namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class LiveRecording
  {
    #region Properties
    public string Name { get; set; }
    public string Format { get; set; }
    public string Target_uri { get; set; }
    public string State { get; set; }
    public int Duration { get; set; }
    public int Talking_duration { get; set; }
    public int Silence_duration { get; set; }
    public string Cause { get; set; }
    #endregion
  }
}