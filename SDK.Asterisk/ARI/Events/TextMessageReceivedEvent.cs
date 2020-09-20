namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class TextMessageReceivedEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public TextMessage Message { get; set; }
    public Endpoint Endpoint { get; set; }
    #endregion
  }
}
