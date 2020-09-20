namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class Playback
  {
    #region Properties
    public string Id { get; set; }
    public string Media_uri { get; set; }
    public string Next_media_uri { get; set; }
    public string Target_uri { get; set; }
    public string Language { get; set; }
    public string State { get; set; }
    #endregion
  }
}