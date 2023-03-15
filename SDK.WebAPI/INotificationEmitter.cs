namespace SoftmakeAll.SDK.WebAPI
{
  public interface INotificationEmitter
  {
    #region Methods
    public System.Threading.Tasks.Task WebSocketNotifyAsync(System.Text.Json.JsonElement Body, System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task WebSocketNotifyAsync(System.Text.Json.JsonElement Body, System.String Method, System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task WebSocketNotifyAsync(System.Text.Json.JsonElement Body, System.String Method, System.Collections.Generic.List<System.String> Usernames, System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task WebSocketDirectNotifyAsync(System.Text.Json.JsonElement Body, System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task WebSocketDirectNotifyAsync(System.Text.Json.JsonElement Body, System.String Method, System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task WebSocketDirectNotifyAsync(System.Text.Json.JsonElement Body, System.String Method, System.Collections.Generic.List<System.String> ConnectionsID, System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task RaiseAsync(System.Text.Json.JsonElement Body, System.Threading.CancellationToken CancellationToken = default);
    #endregion
  }
}