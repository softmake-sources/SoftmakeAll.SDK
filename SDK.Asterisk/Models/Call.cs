using System.Linq;

namespace SoftmakeAll.SDK.Asterisk.Models
{
  public class Call
  {
    #region Constructor
    public Call(SoftmakeAll.SDK.Asterisk.Models.Channel Source)
    {
      this.CallSteps = new System.Collections.Generic.List<SoftmakeAll.SDK.Asterisk.Models.CallStep>();
      this.CallSteps.Add(new SoftmakeAll.SDK.Asterisk.Models.CallStep(Source));
    }
    #endregion

    #region Fields
    private readonly System.Object _syncRoot = new System.Object();
    #endregion

    #region Readonly Properties
    public System.DateTimeOffset StartDateTime { get; set; }
    public System.DateTimeOffset? EndDateTime
    {
      get
      {
        if (CurrentState != "Finished")
          return null;
        return this.StartDateTime.AddSeconds(this.TotalDurationSeconds);
      }
    }
    public System.Double TotalDurationSeconds => this.CallSteps.Any() ? this.CallSteps.Last().Source.Timestamp.Subtract(this.StartDateTime).TotalSeconds : 0.0D;
    public System.Double TotalWaitingTime => this.CallSteps.Any() && this.CallSteps.Exists(c => c.Source.State == "Up") ? this.CallSteps.FirstOrDefault(c => c.Source.State == "Up").Source.Timestamp.Subtract(this.StartDateTime).TotalSeconds : this.TotalDurationSeconds;
    public System.Double TotalSpeakingTime => this.TotalDurationSeconds - this.TotalWaitingTime;
    public System.String CurrentState
    {
      get
      {
        System.String CurrentState = "Ringing";

        if (this.CallSteps.Any())
          CurrentState = this.CallSteps.Last().Source.State;

        switch (CurrentState)
        {
          case "Up": return "Speaking";
          case "Hangup": return "Finished";
        }

        return CurrentState;
      }
    }
    public System.Collections.Generic.List<Channel> HangupOrder
    {
      get
      {
        if (!(this.CallSteps.Any()))
          return null;

        SoftmakeAll.SDK.Asterisk.Models.CallStep LastStep = this.CallSteps.Last();

        System.Collections.Generic.List<Channel> Result = new System.Collections.Generic.List<Channel>();
        if (LastStep.Source.State == "Hangup")
          Result.Add(LastStep.Source);
        Result.AddRange(this.CallSteps.Last().Connections.Where(d => d.State == "Hangup"));

        if (Result.Any())
          return Result.OrderBy(d => d.Timestamp).ToList();

        return null;
      }
    }

    public System.Collections.Generic.List<SoftmakeAll.SDK.Asterisk.Models.CallStep> CallSteps { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public System.String TransferKind { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public System.Boolean Transferred { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public System.Boolean Originated { get; set; }
    #endregion

    #region Methods
    public SoftmakeAll.SDK.Asterisk.Models.CallStep AddStepIfNotExists(System.String State, System.Boolean CheckSourceState)
    {
      lock (_syncRoot)
      {
        if ((!(this.CallSteps.Last().Connections.Exists(c => c.State == State))) && ((!(CheckSourceState)) || (!(this.CallSteps.Last().Source.State == State))))
          this.CallSteps.Add(new SoftmakeAll.SDK.Asterisk.Models.CallStep(this.CallSteps.Last().Source));
      }
      return this.CallSteps.Last();
    }
    #endregion
  }
}