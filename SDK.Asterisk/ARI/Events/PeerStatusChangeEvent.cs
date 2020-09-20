namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class PeerStatusChangeEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public Endpoint Endpoint { get; set; }
    public Peer Peer { get; set; }
    #endregion
  }
}
