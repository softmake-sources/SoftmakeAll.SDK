namespace SoftmakeAll.SDK.Asterisk.ARI.Dispatchers
{
  public sealed class ThreadPoolDispatcher : SoftmakeAll.SDK.Asterisk.ARI.IAriDispatcher
  {
    #region Methods
    public void QueueAction(System.Action action) => System.Threading.ThreadPool.QueueUserWorkItem(_ => action());
    public void Dispose() { }
    #endregion
  }
}