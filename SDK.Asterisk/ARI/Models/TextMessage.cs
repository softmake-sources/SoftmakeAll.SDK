namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class TextMessage
  {
    #region Properties
    public string From { get; set; }
    public string To { get; set; }
    public string Body { get; set; }
    public object Variables { get; set; }
    #endregion
  }
}