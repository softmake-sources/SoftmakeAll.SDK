namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ChannelUsereventEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public string Eventname { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    public Bridge Bridge { get; set; }
    public Endpoint Endpoint { get; set; }
    public object Userevent { get; set; }
    #endregion
  }
}
