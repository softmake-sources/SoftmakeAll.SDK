namespace SoftmakeAll.SDK.WebAPI
{
  public interface INotificationEmitter
  {
    #region Methods
    public void Raise(System.Text.Json.JsonElement Body);
    #endregion
  }
}