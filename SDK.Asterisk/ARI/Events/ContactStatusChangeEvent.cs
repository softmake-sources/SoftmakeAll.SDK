namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ContactStatusChangeEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public Endpoint Endpoint { get; set; }
    public ContactInfo Contact_info { get; set; }
    #endregion
  }
}
