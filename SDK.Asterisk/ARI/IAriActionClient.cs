namespace SoftmakeAll.SDK.Asterisk.ARI
{
  public interface IAriActionClient
  {
    #region Properties
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IAsteriskActions Asterisk { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IApplicationsActions Applications { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IBridgesActions Bridges { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IChannelsActions Channels { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IDeviceStatesActions DeviceStates { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IEndpointsActions Endpoints { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IEventsActions Events { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IMailboxesActions Mailboxes { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IPlaybacksActions Playbacks { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IRecordingsActions Recordings { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.ISoundsActions Sounds { get; set; }
    #endregion
  }
}