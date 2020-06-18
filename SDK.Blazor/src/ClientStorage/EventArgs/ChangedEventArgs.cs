namespace SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs
{
  public class ChangedEventArgs
  {
    #region Properties
    public System.String Key { get; set; }
    public System.Object OldValue { get; set; }
    public System.Object NewValue { get; set; }
    #endregion
  }
}