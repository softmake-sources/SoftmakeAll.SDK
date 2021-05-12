using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using Microsoft.AspNetCore.SignalR.Client;
using System.Linq;

namespace SoftmakeAll.SDK.Fluent
{
  public sealed class SoftmakeWS
  {
    #region Fields
    private Microsoft.AspNetCore.SignalR.Client.HubConnection WSConnection;
    private System.Action<System.Text.Json.JsonElement> OnMessageReceivedAction;
    #endregion

    #region Constructor
    public SoftmakeWS() { }
    #endregion

    #region Events and Actions
    public event System.EventHandler<System.Text.Json.JsonElement> MessageReceived;
    public void OnMessageReceived(System.Action<System.Text.Json.JsonElement> Action) => this.OnMessageReceivedAction = Action;
    #endregion

    #region Properties
    internal System.Boolean Connected => ((this.WSConnection != null) && (this.WSConnection.State == HubConnectionState.Connected));
    #endregion

    #region Methods
    internal async System.Threading.Tasks.Task ConfigureAsync(System.String Authorization)
    {
      if (System.String.IsNullOrWhiteSpace(Authorization))
        return;

      await this.DestroyAsync();

      this.WSConnection = new Microsoft.AspNetCore.SignalR.Client.HubConnectionBuilder()
       .WithUrl(SoftmakeAll.SDK.Fluent.SDKContext.WebSocketBaseAddress, o =>
       {
         if (Authorization.StartsWith("Basic "))
           o.Headers.Add("Authorization", Authorization);
         else
           o.AccessTokenProvider = () => System.Threading.Tasks.Task.FromResult(Authorization);
       })
       .WithAutomaticReconnect()
       .Build();

      try
      {
        await WSConnection.StartAsync();
      }
      catch
      {
        this.WSConnection = null;
      }

      WSConnection.On<System.Text.Json.JsonElement>("_mReceived", JSONMessage =>
      {
        if (!(JSONMessage.IsValid())) return;
        try { this.MessageReceived?.Invoke(null, JSONMessage); } catch { }
        try { this.OnMessageReceivedAction?.Invoke(JSONMessage); } catch { }
      });
    }
    internal async System.Threading.Tasks.Task DestroyAsync()
    {
      if (this.WSConnection != null)
        try
        {
          await WSConnection.StopAsync();
          await WSConnection.DisposeAsync();
        }
        catch { }
    }
    #endregion
  }
}