namespace SoftmakeAll.SDK.Asterisk.ARI
{
  public interface IAriEventClient
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
  }
}