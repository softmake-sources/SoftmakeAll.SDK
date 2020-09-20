namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class Event : Message
  {
    #region Properties
    public string Application { get; set; }
    public System.DateTimeOffset Timestamp { get; set; }
    #endregion
  }
}