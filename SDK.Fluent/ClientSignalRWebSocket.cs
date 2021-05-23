using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using Microsoft.AspNetCore.SignalR.Client;

namespace SoftmakeAll.SDK.Fluent
{
  public sealed class ClientSignalRWebSocket
  {
    #region Fields
    private Microsoft.AspNetCore.SignalR.Client.HubConnection WSConnection;
    private System.Action<System.Text.Json.JsonElement> OnMessageReceivedAction;
    private System.Action<System.Text.Json.JsonElement> OnConnectionStateChangedAction;
    #endregion

    #region Constructor
    public ClientSignalRWebSocket() { }
    #endregion

    #region Events and Actions
    public event System.EventHandler<System.Text.Json.JsonElement> MessageReceived;
    public void OnMessageReceived(System.Action<System.Text.Json.JsonElement> Action) => this.OnMessageReceivedAction = Action;

    public event System.EventHandler<System.Text.Json.JsonElement> ConnectionStateChanged;
    public void OnConnectionStateChanged(System.Action<System.Text.Json.JsonElement> Action) => this.OnConnectionStateChangedAction = Action;
    #endregion

    #region Properties
    internal System.Boolean Connected => ((this.WSConnection != null) && (this.WSConnection.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected));
    #endregion

    #region Methods
    internal async System.Threading.Tasks.Task ConfigureAsync(System.String Authorization)
    {
      if (System.String.IsNullOrWhiteSpace(Authorization))
        return;

      await this.DisposeAsync();

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

      this.WSConnection.Closed += this.WSConnection_Closed;
      this.WSConnection.Reconnecting += this.WSConnection_Reconnecting;
      this.WSConnection.Reconnected += this.WSConnection_Reconnected;
      this.WSConnection.On<System.Text.Json.JsonElement>("_mReceived", Message =>
      {
        if (!(Message.IsValid())) return;
        try { this.MessageReceived?.Invoke(null, Message); } catch { }
        try { this.OnMessageReceivedAction?.Invoke(Message); } catch { }
      });

      try
      {
        await this.WSConnection.StartAsync();
        await this.InvokeConnectionStateChangedEvents("Connected", null, null);
      }
      catch
      {
        await this.DisposeAsync();
      }
    }
    internal async System.Threading.Tasks.Task SendAsync(System.Text.Json.JsonElement Message) => await this.WSConnection.SendAsync("_mSended", Message);
    internal async System.Threading.Tasks.ValueTask DisposeAsync()
    {
      if (this.WSConnection == null)
        return;

      this.WSConnection.Reconnecting -= this.WSConnection_Reconnecting;
      this.WSConnection.Reconnected -= this.WSConnection_Reconnected;

      try { await this.WSConnection.StopAsync(); } catch { }

      try
      {
        this.WSConnection.Closed -= this.WSConnection_Closed;
        await this.WSConnection.DisposeAsync();
      }
      catch { }
    }
    #endregion

    #region Event Handlers
    private System.Threading.Tasks.Task InvokeConnectionStateChangedEvents(System.String Event, System.String Arguments, System.Exception Exception)
    {
      System.Text.Json.JsonElement Message = new { Event, Arguments, Exception?.Message }.ToJsonElement();
      try { this.ConnectionStateChanged?.Invoke(null, Message); } catch { }
      try { this.OnConnectionStateChangedAction?.Invoke(Message); } catch { }
      return System.Threading.Tasks.Task.CompletedTask;
    }
    private System.Threading.Tasks.Task WSConnection_Reconnected(System.String Arguments) => this.InvokeConnectionStateChangedEvents("Reconnected", Arguments, null);
    private System.Threading.Tasks.Task WSConnection_Reconnecting(System.Exception Exception) => this.InvokeConnectionStateChangedEvents("Reconnecting", null, Exception);
    private System.Threading.Tasks.Task WSConnection_Closed(System.Exception Exception) => this.InvokeConnectionStateChangedEvents("Closed", null, Exception);
    #endregion
  }
}