using System.Linq;

namespace SoftmakeAll.SDK.Asterisk
{
  public class WSClient : SoftmakeAll.SDK.Asterisk.ARI.AriClient
  {
    #region Fields
    private System.Collections.Generic.List<SoftmakeAll.SDK.Asterisk.Models.Call> Calls { get; }
    private System.Collections.Generic.IEnumerable<SoftmakeAll.SDK.Asterisk.Models.Call> ActiveCalls => this.Calls.Where(c => c.CurrentState != "Finished");
    #endregion

    #region Constructors
    public WSClient(SoftmakeAll.SDK.Asterisk.ARI.Middleware.IActionConsumer ActionConsumer, SoftmakeAll.SDK.Asterisk.ARI.Middleware.IEventProducer EventProducer, System.String Application, System.Boolean SubscribeAllEvents = false, System.Boolean SSL = false) : base(ActionConsumer, EventProducer, Application, SubscribeAllEvents, SSL)
    {
      this.Calls = new System.Collections.Generic.List<SoftmakeAll.SDK.Asterisk.Models.Call>();
      base.OnChannelCreatedEvent += this.NotifyOnChannelCreatedEvent;
      base.OnChannelStateChangeEvent += this.NotifyOnChannelStateChangeEvent;
      base.OnBridgeBlindTransferEvent += this.NotifyOnBridgeBlindTransferEvent;
      base.OnBridgeAttendedTransferEvent += this.NotifyOnBridgeAttendedTransferEvent;
      base.OnChannelHoldEvent += this.NotifyOnChannelHoldEvent;
      base.OnChannelUnholdEvent += this.NotifyOnChannelUnholdEvent;
      base.OnChannelDestroyedEvent += this.NotifyOnChannelDestroyedEvent;
    }
    public WSClient(SoftmakeAll.SDK.Asterisk.ARI.StasisEndpoint Endpoint, System.String Application, System.Boolean SubscribeAllEvents = false, System.Boolean SSL = false) : this(new SoftmakeAll.SDK.Asterisk.ARI.Middleware.Default.RestActionConsumer(Endpoint), new SoftmakeAll.SDK.Asterisk.ARI.Middleware.Default.WebSocketEventProducer(Endpoint, Application), Application, SubscribeAllEvents, SSL) { }
    #endregion

    #region Events
    public event System.EventHandler<SoftmakeAll.SDK.Asterisk.Models.Call> OnCallChanged;
    #endregion

    #region Event Handlers
    private void NotifyOnChannelCreatedEvent(SoftmakeAll.SDK.Asterisk.ARI.IAriClient sender, SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelCreatedEvent e)
    {
      System.String ChannelID = e.Channel.Id;
      System.String ChannelCallID = e.Channel.CallID;
      System.String ChannelContactName = e.Channel.Caller.Name;
      System.String ChannelContactNumber = e.Channel.Caller.Number;
      System.String ChannelState = e.Channel.State;

      if (ChannelState == "Ring")
      {
        SoftmakeAll.SDK.Asterisk.Models.Call Call = new SoftmakeAll.SDK.Asterisk.Models.Call(new SoftmakeAll.SDK.Asterisk.Models.Channel() { Timestamp = e.Timestamp, ID = ChannelID, Name = ChannelContactName, Extension = ChannelContactNumber, Destiny = e.Channel.Dialplan.Exten, State = ChannelState }) { StartDateTime = e.Timestamp };
        Calls.Add(Call);
        OnCallChanged?.Invoke(this, Call);
        return;
      }

      if (ChannelState == "Down")
      {
        SoftmakeAll.SDK.Asterisk.Models.Call Call = FindCallByChannelCallID(ChannelCallID);
        if ((Call ??= FindCallByChannelContactNumber("Originate")) == null)
        {
          Call = new SoftmakeAll.SDK.Asterisk.Models.Call(new SoftmakeAll.SDK.Asterisk.Models.Channel() { Timestamp = e.Timestamp, ID = ChannelID, Name = "SoftmakeAll", Extension = "Originate", Destiny = ChannelContactNumber, State = "Ring" }) { StartDateTime = e.Timestamp };
          Calls.Add(Call);
        }

        SoftmakeAll.SDK.Asterisk.Models.Channel Destiny = new SoftmakeAll.SDK.Asterisk.Models.Channel();
        Destiny.Timestamp = e.Timestamp;
        Destiny.ID = ChannelID;
        Destiny.Name = ChannelContactName;
        Destiny.Extension = ChannelContactNumber;
        Destiny.State = ChannelState;
        Call.CallSteps.Last().Connections.Add(Destiny);
        OnCallChanged?.Invoke(this, Call);
        return;
      }
    }
    private void NotifyOnChannelStateChangeEvent(SoftmakeAll.SDK.Asterisk.ARI.IAriClient sender, SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelStateChangeEvent e)
    {
      System.String ChannelID = e.Channel.Id;
      System.String ChannelCallID = e.Channel.CallID;
      System.String ChannelContactName = e.Channel.Caller.Name;
      System.String ChannelContactNumber = e.Channel.Caller.Number;
      System.String ChannelState = e.Channel.State;

      SoftmakeAll.SDK.Asterisk.Models.Channel Channel = new SoftmakeAll.SDK.Asterisk.Models.Channel();
      Channel.Timestamp = e.Timestamp;
      Channel.ID = ChannelID;
      Channel.Name = ChannelContactName;
      Channel.Extension = ChannelContactNumber;
      Channel.State = ChannelState;

      SoftmakeAll.SDK.Asterisk.Models.Call Call = FindCallByChannelCallID(ChannelCallID);
      if (Call == null)
      {
        Call = FindCallByChannelContactNumber(e.Channel.Connected.Number);
        if (Call == null)
          return;
      }

      SoftmakeAll.SDK.Asterisk.Models.CallStep LastStep = Call.AddStepIfNotExists(Channel.State, false);

      if (ChannelState == "Ringing")
      {
        if ((Call.Transferred) || (Call.Originated))
        {
          SoftmakeAll.SDK.Asterisk.Models.Channel NewSource = Call.CallSteps.SelectMany(cs => cs.Connections).FirstOrDefault(d => d.Extension == e.Channel.Connected.Number);
          if (NewSource == null)
            return;

          Call.Originated = false;

          LastStep.Source.ID = NewSource.ID;
          LastStep.Source.Name = NewSource.Name;
          LastStep.Source.Extension = NewSource.Extension;
          LastStep.Source.Destiny = ChannelContactNumber;
          LastStep.Source.State = "Ring";
          LastStep.Source.HangupCauseText = null;
          LastStep.Source.HangupCauseCode = null;
          LastStep.Source.HangupCauseDetails = null;
        }

        Call.CallSteps.Last().Connections.Add(Channel);
        OnCallChanged?.Invoke(this, Call);
        return;
      }

      if (ChannelState == "Up")
      {
        LastStep.Source.State = ChannelState;

        if (ChannelContactNumber == LastStep.Source.Extension)
        {
          LastStep.Source.Timestamp = e.Timestamp;
          return;
        }

        if (LastStep.Source.Extension == "Originate")
        {
          Call.Originated = true;
          LastStep.Source.Timestamp = e.Timestamp;
        }
        else
          LastStep.Connections.Add(Channel);

        if (Call.Transferred)
        {
          Call.TransferKind = null;
          Call.Transferred = false;
        }

        OnCallChanged?.Invoke(this, Call);
        return;
      }
    }
    private void NotifyOnBridgeBlindTransferEvent(SoftmakeAll.SDK.Asterisk.ARI.IAriClient sender, SoftmakeAll.SDK.Asterisk.ARI.Models.BridgeBlindTransferEvent e)
    {
      if (e.Result != "Success")
        return;

      SoftmakeAll.SDK.Asterisk.Models.Call Call = FindCallByChannelCallID(e.Channel.CallID);
      Call.TransferKind = "Blind Transfer";
      Call.Transferred = true;
    }
    private void NotifyOnBridgeAttendedTransferEvent(SoftmakeAll.SDK.Asterisk.ARI.IAriClient sender, SoftmakeAll.SDK.Asterisk.ARI.Models.BridgeAttendedTransferEvent e)
    {
      if (e.Result != "Success")
        return;

    }
    private void NotifyOnChannelHoldEvent(SoftmakeAll.SDK.Asterisk.ARI.IAriClient sender, SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelHoldEvent e) { }
    private void NotifyOnChannelUnholdEvent(SoftmakeAll.SDK.Asterisk.ARI.IAriClient sender, SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelUnholdEvent e) { }
    private void NotifyOnChannelDestroyedEvent(SoftmakeAll.SDK.Asterisk.ARI.IAriClient sender, SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelDestroyedEvent e)
    {
      System.String ChannelID = e.Channel.Id;
      System.String ChannelCallID = e.Channel.CallID;
      System.String ChannelContactName = e.Channel.Caller.Name;
      System.String ChannelContactNumber = e.Channel.Caller.Number;

      SoftmakeAll.SDK.Asterisk.Models.Channel Channel = new SoftmakeAll.SDK.Asterisk.Models.Channel();
      Channel.Timestamp = e.Timestamp;
      Channel.ID = ChannelID;
      Channel.Name = ChannelContactName;
      Channel.Extension = ChannelContactNumber;
      Channel.State = "Hangup";
      Channel.HangupCauseCode = e.Cause;
      Channel.HangupCauseText = e.Cause_txt;
      Channel.HangupCauseDetails = null;

      SoftmakeAll.SDK.Asterisk.Models.Call Call = FindCallByChannelCallID(ChannelCallID);
      if (Call == null)
        return;

      if (Call.Transferred)
        Channel.HangupCauseDetails = Call.TransferKind;

      SoftmakeAll.SDK.Asterisk.Models.CallStep LastStep = Call.AddStepIfNotExists(Channel.State, true);

      if ((ChannelContactNumber == LastStep.Source.Extension) || (LastStep.Source.Extension == "Originate"))
      {
        LastStep.Source.Timestamp = e.Timestamp;
        LastStep.Source.State = Channel.State;
        LastStep.Source.HangupCauseCode = Channel.HangupCauseCode;
        LastStep.Source.HangupCauseText = Channel.HangupCauseText;
        LastStep.Source.HangupCauseDetails = Channel.HangupCauseDetails;
        OnCallChanged?.Invoke(this, Call);
      }
      else
      {
        if (e.Cause == 26) // Answered elsewhere
          LastStep = Call.CallSteps.SkipLast(1).Last(cs => cs.Source.State == "Up");
        LastStep.Connections.Add(Channel);
        OnCallChanged?.Invoke(this, Call);
      }
    }
    #endregion

    #region Methods
    #region Helpers
    private SoftmakeAll.SDK.Asterisk.Models.Call FindCallByChannelCallID(System.String ChannelCallID) => this.ActiveCalls?.Where(c => c.CallSteps.Exists(c2 => ((c2.Source.CallID == ChannelCallID) || (c2.Connections.Exists(c3 => c3.CallID == ChannelCallID))))).LastOrDefault();
    private SoftmakeAll.SDK.Asterisk.Models.Call FindCallByChannelContactNumber(System.String ChannelContactNumber) => this.ActiveCalls?.Where(c => c.CallSteps.Exists(c2 => ((c2.Source.Extension == ChannelContactNumber) || (c2.Connections.Exists(c3 => c3.Extension == ChannelContactNumber))))).LastOrDefault();
    #endregion

    public System.Collections.Generic.List<SoftmakeAll.SDK.Asterisk.Models.Call> GetCalls() => this.Calls.ToList();
    public void ClearAllCalls() => this.Calls.Clear();
    public void ClearFinishedCalls() => this.Calls.RemoveAll(c => c.CurrentState == "Finished");
    #endregion
  }
}