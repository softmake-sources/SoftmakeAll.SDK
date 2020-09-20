namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class Sound
  {
    #region Properties
    public string Id { get; set; }
    public string Text { get; set; }
    public System.Collections.Generic.List<FormatLangPair> Formats { get; set; }
    #endregion
  }
}
