namespace SoftmakeAll.SDK.Asterisk.ARI
{
  public interface IAriDispatcher : System.IDisposable
  {
    #region Methods
    public void QueueAction(System.Action action);
    #endregion
  }
}