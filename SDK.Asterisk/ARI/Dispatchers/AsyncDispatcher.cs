namespace SoftmakeAll.SDK.Asterisk.ARI.Dispatchers
{
  public sealed class AsyncDispatcher : SoftmakeAll.SDK.Asterisk.ARI.IAriDispatcher
  {
    #region Methods
    public async void QueueAction(System.Action action) => await System.Threading.Tasks.Task.Run(action);
    public void Dispose() { }
    #endregion
  }
}
