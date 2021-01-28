namespace SoftmakeAll.SDK.WebAPI
{
  public interface INotificationEmitter
  {
    #region Methods
    public void WebSocketNotify(System.Text.Json.JsonElement Body);
    public void WebSocketNotify(System.Text.Json.JsonElement Body, System.String Method);
    public void WebSocketNotify(System.Text.Json.JsonElement Body, System.String Method, System.Collections.Generic.List<System.String> Usernames);
    public void WebSocketDirectNotify(System.Text.Json.JsonElement Body);
    public void WebSocketDirectNotify(System.Text.Json.JsonElement Body, System.String Method);
    public void WebSocketDirectNotify(System.Text.Json.JsonElement Body, System.String Method, System.Collections.Generic.List<System.String> ConnectionsID);
    public void Raise(System.Text.Json.JsonElement Body);
    #endregion
  }
}