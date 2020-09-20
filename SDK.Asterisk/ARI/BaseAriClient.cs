namespace SoftmakeAll.SDK.Asterisk.ARI
{
  public abstract class BaseAriClient : IAriEventClient
  {
    #region Events
    public event DeviceStateChangedEventHandler OnDeviceStateChangedEvent;
    public event PlaybackStartedEventHandler OnPlaybackStartedEvent;
    public event PlaybackContinuingEventHandler OnPlaybackContinuingEvent;
    public event PlaybackFinishedEventHandler OnPlaybackFinishedEvent;
    public event RecordingStartedEventHandler OnRecordingStartedEvent;
    public event RecordingFinishedEventHandler OnRecordingFinishedEvent;
    public event RecordingFailedEventHandler OnRecordingFailedEvent;
    public event ApplicationMoveFailedEventHandler OnApplicationMoveFailedEvent;
    public event ApplicationReplacedEventHandler OnApplicationReplacedEvent;
    public event BridgeCreatedEventHandler OnBridgeCreatedEvent;
    public event BridgeDestroyedEventHandler OnBridgeDestroyedEvent;
    public event BridgeMergedEventHandler OnBridgeMergedEvent;
    public event BridgeVideoSourceChangedEventHandler OnBridgeVideoSourceChangedEvent;
    public event BridgeBlindTransferEventHandler OnBridgeBlindTransferEvent;
    public event BridgeAttendedTransferEventHandler OnBridgeAttendedTransferEvent;
    public event ChannelCreatedEventHandler OnChannelCreatedEvent;
    public event ChannelDestroyedEventHandler OnChannelDestroyedEvent;
    public event ChannelEnteredBridgeEventHandler OnChannelEnteredBridgeEvent;
    public event ChannelLeftBridgeEventHandler OnChannelLeftBridgeEvent;
    public event ChannelStateChangeEventHandler OnChannelStateChangeEvent;
    public event ChannelDtmfReceivedEventHandler OnChannelDtmfReceivedEvent;
    public event ChannelDialplanEventHandler OnChannelDialplanEvent;
    public event ChannelCallerIdEventHandler OnChannelCallerIdEvent;
    public event ChannelUsereventEventHandler OnChannelUsereventEvent;
    public event ChannelHangupRequestEventHandler OnChannelHangupRequestEvent;
    public event ChannelVarsetEventHandler OnChannelVarsetEvent;
    public event ChannelHoldEventHandler OnChannelHoldEvent;
    public event ChannelUnholdEventHandler OnChannelUnholdEvent;
    public event ChannelTalkingStartedEventHandler OnChannelTalkingStartedEvent;
    public event ChannelTalkingFinishedEventHandler OnChannelTalkingFinishedEvent;
    public event ContactStatusChangeEventHandler OnContactStatusChangeEvent;
    public event PeerStatusChangeEventHandler OnPeerStatusChangeEvent;
    public event EndpointStateChangeEventHandler OnEndpointStateChangeEvent;
    public event DialEventHandler OnDialEvent;
    public event StasisEndEventHandler OnStasisEndEvent;
    public event StasisStartEventHandler OnStasisStartEvent;
    public event TextMessageReceivedEventHandler OnTextMessageReceivedEvent;
    public event ChannelConnectedLineEventHandler OnChannelConnectedLineEvent;
    public event UnhandledEventHandler OnUnhandledEvent;
    #endregion

    #region Methods
    protected void FireEvent(string eventName, object eventArgs, IAriClient sender)
    {
      switch (eventName)
      {
        case "DeviceStateChanged": if (OnDeviceStateChangedEvent == null) break; OnDeviceStateChangedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.DeviceStateChangedEvent)eventArgs); return;
        case "PlaybackStarted": if (OnPlaybackStartedEvent == null) break; OnPlaybackStartedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.PlaybackStartedEvent)eventArgs); return;
        case "PlaybackContinuing": if (OnPlaybackContinuingEvent == null) break; OnPlaybackContinuingEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.PlaybackContinuingEvent)eventArgs); return;
        case "PlaybackFinished": if (OnPlaybackFinishedEvent == null) break; OnPlaybackFinishedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.PlaybackFinishedEvent)eventArgs); return;
        case "RecordingStarted": if (OnRecordingStartedEvent == null) break; OnRecordingStartedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.RecordingStartedEvent)eventArgs); return;
        case "RecordingFinished": if (OnRecordingFinishedEvent == null) break; OnRecordingFinishedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.RecordingFinishedEvent)eventArgs); return;
        case "RecordingFailed": if (OnRecordingFailedEvent == null) break; OnRecordingFailedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.RecordingFailedEvent)eventArgs); return;
        case "ApplicationMoveFailed": if (OnApplicationMoveFailedEvent == null) break; OnApplicationMoveFailedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ApplicationMoveFailedEvent)eventArgs); return;
        case "ApplicationReplaced": if (OnApplicationReplacedEvent == null) break; OnApplicationReplacedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ApplicationReplacedEvent)eventArgs); return;
        case "BridgeCreated": if (OnBridgeCreatedEvent == null) break; OnBridgeCreatedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.BridgeCreatedEvent)eventArgs); return;
        case "BridgeDestroyed": if (OnBridgeDestroyedEvent == null) break; OnBridgeDestroyedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.BridgeDestroyedEvent)eventArgs); return;
        case "BridgeMerged": if (OnBridgeMergedEvent == null) break; OnBridgeMergedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.BridgeMergedEvent)eventArgs); return;
        case "BridgeVideoSourceChanged": if (OnBridgeVideoSourceChangedEvent == null) break; OnBridgeVideoSourceChangedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.BridgeVideoSourceChangedEvent)eventArgs); return;
        case "BridgeBlindTransfer": if (OnBridgeBlindTransferEvent == null) break; OnBridgeBlindTransferEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.BridgeBlindTransferEvent)eventArgs); return;
        case "BridgeAttendedTransfer": if (OnBridgeAttendedTransferEvent == null) break; OnBridgeAttendedTransferEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.BridgeAttendedTransferEvent)eventArgs); return;
        case "ChannelCreated": if (OnChannelCreatedEvent == null) break; OnChannelCreatedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelCreatedEvent)eventArgs); return;
        case "ChannelDestroyed": if (OnChannelDestroyedEvent == null) break; OnChannelDestroyedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelDestroyedEvent)eventArgs); return;
        case "ChannelEnteredBridge": if (OnChannelEnteredBridgeEvent == null) break; OnChannelEnteredBridgeEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelEnteredBridgeEvent)eventArgs); return;
        case "ChannelLeftBridge": if (OnChannelLeftBridgeEvent == null) break; OnChannelLeftBridgeEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelLeftBridgeEvent)eventArgs); return;
        case "ChannelStateChange": if (OnChannelStateChangeEvent == null) break; OnChannelStateChangeEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelStateChangeEvent)eventArgs); return;
        case "ChannelDtmfReceived": if (OnChannelDtmfReceivedEvent == null) break; OnChannelDtmfReceivedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelDtmfReceivedEvent)eventArgs); return;
        case "ChannelDialplan": if (OnChannelDialplanEvent == null) break; OnChannelDialplanEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelDialplanEvent)eventArgs); return;
        case "ChannelCallerId": if (OnChannelCallerIdEvent == null) break; OnChannelCallerIdEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelCallerIdEvent)eventArgs); return;
        case "ChannelUserevent": if (OnChannelUsereventEvent == null) break; OnChannelUsereventEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelUsereventEvent)eventArgs); return;
        case "ChannelHangupRequest": if (OnChannelHangupRequestEvent == null) break; OnChannelHangupRequestEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelHangupRequestEvent)eventArgs); return;
        case "ChannelVarset": if (OnChannelVarsetEvent == null) break; OnChannelVarsetEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelVarsetEvent)eventArgs); return;
        case "ChannelHold": if (OnChannelHoldEvent == null) break; OnChannelHoldEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelHoldEvent)eventArgs); return;
        case "ChannelUnhold": if (OnChannelUnholdEvent == null) break; OnChannelUnholdEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelUnholdEvent)eventArgs); return;
        case "ChannelTalkingStarted": if (OnChannelTalkingStartedEvent == null) break; OnChannelTalkingStartedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelTalkingStartedEvent)eventArgs); return;
        case "ChannelTalkingFinished": if (OnChannelTalkingFinishedEvent == null) break; OnChannelTalkingFinishedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelTalkingFinishedEvent)eventArgs); return;
        case "ContactStatusChange": if (OnContactStatusChangeEvent == null) break; OnContactStatusChangeEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ContactStatusChangeEvent)eventArgs); return;
        case "PeerStatusChange": if (OnPeerStatusChangeEvent == null) break; OnPeerStatusChangeEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.PeerStatusChangeEvent)eventArgs); return;
        case "EndpointStateChange": if (OnEndpointStateChangeEvent == null) break; OnEndpointStateChangeEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.EndpointStateChangeEvent)eventArgs); return;
        case "Dial": if (OnDialEvent == null) break; OnDialEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.DialEvent)eventArgs); return;
        case "StasisEnd": if (OnStasisEndEvent == null) break; OnStasisEndEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.StasisEndEvent)eventArgs); return;
        case "StasisStart": if (OnStasisStartEvent == null) break; OnStasisStartEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.StasisStartEvent)eventArgs); return;
        case "TextMessageReceived": if (OnTextMessageReceivedEvent == null) break; OnTextMessageReceivedEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.TextMessageReceivedEvent)eventArgs); return;
        case "ChannelConnectedLine": if (OnChannelConnectedLineEvent == null) break; OnChannelConnectedLineEvent(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.ChannelConnectedLineEvent)eventArgs); return;
      }

      this.OnUnhandledEvent?.Invoke(sender, (SoftmakeAll.SDK.Asterisk.ARI.Models.Event)eventArgs);
    }
    #endregion
  }
}