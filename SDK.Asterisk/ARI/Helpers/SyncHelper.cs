namespace SoftmakeAll.SDK.Asterisk.ARI.Helpers
{
  public static class SyncHelper
  {
    #region Methods
    public static SoftmakeAll.SDK.Asterisk.ARI.Models.PlaybackFinishedEvent Wait(this SoftmakeAll.SDK.Asterisk.ARI.Models.Playback playback, SoftmakeAll.SDK.Asterisk.ARI.IAriEventClient client)
    {
      System.Threading.AutoResetEvent PlaybackFinished = new System.Threading.AutoResetEvent(false);
      SoftmakeAll.SDK.Asterisk.ARI.Models.PlaybackFinishedEvent PlaybackFinishedEvent = null;
      client.OnPlaybackFinishedEvent += (s, e) => { PlaybackFinishedEvent = e; PlaybackFinished.Set(); };
      PlaybackFinished.WaitOne();
      return PlaybackFinishedEvent;
    }
    #endregion
  }
}