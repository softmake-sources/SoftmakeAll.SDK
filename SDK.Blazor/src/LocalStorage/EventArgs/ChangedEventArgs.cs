namespace SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs
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