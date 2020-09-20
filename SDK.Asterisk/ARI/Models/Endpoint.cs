namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class Endpoint
  {
    #region Properties
    public string Technology { get; set; }
    public string Resource { get; set; }
    public string State { get; set; }
    public System.Collections.Generic.List<string> Channel_ids { get; set; }
    #endregion
  }
}