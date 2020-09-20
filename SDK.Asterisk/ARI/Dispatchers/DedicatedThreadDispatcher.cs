namespace SoftmakeAll.SDK.Asterisk.ARI.Dispatchers
{
  public sealed class DedicatedThreadDispatcher : SoftmakeAll.SDK.Asterisk.ARI.IAriDispatcher
  {
    #region Fields
    private readonly System.Collections.Concurrent.BlockingCollection<System.Action> _eventQueue = new System.Collections.Concurrent.BlockingCollection<System.Action>();
    private readonly System.Threading.CancellationTokenSource _threadCancellation = new System.Threading.CancellationTokenSource();
    #endregion

    #region Constructor
    public DedicatedThreadDispatcher() => new System.Threading.Thread(() => SoftmakeAll.SDK.Asterisk.ARI.Dispatchers.DedicatedThreadDispatcher.EventDispatcherThread(this._threadCancellation.Token, this._eventQueue)).Start();
    #endregion

    #region Methods
    public static void EventDispatcherThread(System.Threading.CancellationToken cancellationToken, System.Collections.Concurrent.BlockingCollection<System.Action> queue) { try { while (true) queue.Take(cancellationToken)(); } catch (System.OperationCanceledException) { } }
    public void QueueAction(System.Action action) => this._eventQueue.Add(action);
    public void Dispose() => _threadCancellation.Cancel();
    #endregion

    #region Desctructor
    ~DedicatedThreadDispatcher() => _threadCancellation.Cancel();
    #endregion
  }
}
