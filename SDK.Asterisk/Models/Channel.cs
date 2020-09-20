namespace SoftmakeAll.SDK.Asterisk.Models
{
  public class Channel
  {
    #region Constructor
    public Channel() { }
    #endregion

    #region Properties
    public System.DateTimeOffset Timestamp { get; set; }
    public System.String ID { get; set; }
    public System.String Name { get; set; }
    public System.String Extension { get; set; }
    public System.String Destiny { get; set; }
    public System.String State { get; set; }
    public System.Nullable<System.Int32> HangupCauseCode { get; set; }
    public System.String HangupCauseText { get; set; }
    public System.String HangupCauseDetails { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public System.String CallID => System.String.IsNullOrWhiteSpace(ID) ? null : ID.Split('.')[0];
    #endregion
  }
}