using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using Microsoft.AspNetCore.SignalR.Client;

namespace SoftmakeAll.SDK.Fluent.Notifications
{
  /// <summary>
  /// ClientWebSocket based on SignalR.
  /// </summary>
  internal sealed class ClientSignalRWebSocket : SoftmakeAll.SDK.Fluent.Notifications.IClientWebSocket
  {
    #region Fields
    private Microsoft.AspNetCore.SignalR.Client.HubConnection WSConnection;
    private System.Action<System.Text.Json.JsonElement> OnMessageReceivedAction;
    private System.Action<System.Text.Json.JsonElement> OnConnectionStateChangedAction;
    #endregion

    #region Constructor
    /// <summary>
    /// Creates a new instance of SignalR based client.
    /// </summary>
    public ClientSignalRWebSocket() { }
    #endregion

    #region Events and Actions
    /// <summary>
    /// Occurs when a new message is received.
    /// </summary>
    public event System.EventHandler<System.Text.Json.JsonElement> MessageReceived;

    /// <summary>
    /// Occurs when a new message is received.
    /// </summary>
    /// <param name="Action">The action to be executed when a new message is received.</param>
    public void OnMessageReceived(System.Action<System.Text.Json.JsonElement> Action) => this.OnMessageReceivedAction = Action;

    /// <summary>
    /// Occurs when the connection state changes.
    /// </summary>
    public event System.EventHandler<System.Text.Json.JsonElement> ConnectionStateChanged;

    /// <summary>
    /// Occurs when the connection state changes.
    /// </summary>
    /// <param name="Action">The action to be executed when connection state changes.</param>
    public void OnConnectionStateChanged(System.Action<System.Text.Json.JsonElement> Action) => this.OnConnectionStateChangedAction = Action;
    #endregion

    #region Properties
    /// <summary>
    /// Indicates the current connection state.
    /// </summary>
    public System.Boolean Connected => ((this.WSConnection != null) && (this.WSConnection.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected));
    #endregion

    #region Methods
    /*
    /// <summary>
    /// Sends a new message to the server.
    /// </summary>
    /// <param name="Message">The message to be sent.</param>
    public async System.Threading.Tasks.Task SendAsync(System.Text.Json.JsonElement Message) => await this.WSConnection.SendAsync("_mSended", Message);
    */

    #region Internal and Private Methods
    /// <summary>
    /// Configures the ClientWebSocket.
    /// </summary>
    /// <param name="Authorization">Authorization Header.</param>
    /// <returns>A task to be wait.</returns>
    /// <example>Basic YourBase64String</example>
    /// <example>Bearer YourAuthToken</example>
    async System.Threading.Tasks.Task SoftmakeAll.SDK.Fluent.Notifications.IClientWebSocket.ConfigureAsync(System.String Authorization)
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
       .WithAutomaticReconnect(new SoftmakeAll.SDK.Fluent.Notifications.SignalRRandomRetryPolicy(86400)) // Stops reconnection attempts after 1 day
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

    /// <summary>
    /// Stops and disposes the ClientWebSocket.
    /// </summary>
    /// <returns>A task to be wait.</returns>
    async System.Threading.Tasks.ValueTask SoftmakeAll.SDK.Fluent.Notifications.IClientWebSocket.DisposeAsync() => await this.DisposeAsync();
    private async System.Threading.Tasks.ValueTask DisposeAsync()
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