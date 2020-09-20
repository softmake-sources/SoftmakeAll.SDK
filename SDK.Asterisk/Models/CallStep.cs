namespace SoftmakeAll.SDK.Asterisk.Models
{
  public class CallStep
  {
    #region Constructor
    public CallStep(SoftmakeAll.SDK.Asterisk.Models.Channel Source)
    {
      this.Source = new Channel()
      {
        Timestamp = Source.Timestamp,
        ID = Source.ID,
        Name = Source.Name,
        Extension = Source.Extension,
        Destiny = Source.Destiny,
        State = Source.State,
        HangupCauseCode = Source.HangupCauseCode,
        HangupCauseText = Source.HangupCauseText,
        HangupCauseDetails = Source.HangupCauseDetails
      };

      this.Connections = new System.Collections.Generic.List<SoftmakeAll.SDK.Asterisk.Models.Channel>();
    }
    #endregion

    #region Properties
    public SoftmakeAll.SDK.Asterisk.Models.Channel Source { get; }
    public System.Collections.Generic.List<SoftmakeAll.SDK.Asterisk.Models.Channel> Connections { get; set; }
    #endregion
  }
}