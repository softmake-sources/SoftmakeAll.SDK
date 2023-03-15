namespace SoftmakeAll.SDK
{
  public class CustomEventArgs<T> : System.EventArgs
  {
    #region Constructor
    public CustomEventArgs(T Param) : base() => this.Data = Param;
    #endregion

    #region Properties
    public T Data { get; }
    #endregion
  }
}