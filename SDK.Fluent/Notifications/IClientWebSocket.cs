namespace SoftmakeAll.SDK.Fluent.Notifications
{
  /// <summary>
  /// ClientWebSocket.
  /// </summary>
  public interface IClientWebSocket
  {
    #region Events and Actions
    /// <summary>
    /// Occurs when a new message is received.
    /// </summary>
    event System.EventHandler<System.Text.Json.JsonElement> MessageReceived;

    /// <summary>
    /// Occurs when a new message is received.
    /// </summary>
    /// <param name="Action">The action to be executed when a new message is received.</param>
    void OnMessageReceived(System.Action<System.Text.Json.JsonElement> Action);

    /// <summary>
    /// Occurs when the connection state changes.
    /// </summary>
    event System.EventHandler<System.Text.Json.JsonElement> ConnectionStateChanged;

    /// <summary>
    /// Occurs when the connection state changes.
    /// </summary>
    /// <param name="Action">The action to be executed when connection state changes.</param>
    void OnConnectionStateChanged(System.Action<System.Text.Json.JsonElement> Action);
    #endregion

    #region Properties
    /// <summary>
    /// Indicates the current connection state.
    /// </summary>
    System.Boolean Connected { get; }
    #endregion

    #region Methods
    /*
    /// <summary>
    /// Sends a new message to the server.
    /// </summary>
    /// <param name="Message">The message to be sent.</param>
    System.Threading.Tasks.Task SendAsync(System.Text.Json.JsonElement Message);
    */

    #region Internal Methods
    /// <summary>
    /// Configures the ClientWebSocket.
    /// </summary>
    /// <param name="Authorization">Authorization Header.</param>
    /// <returns>A task to be wait.</returns>
    /// <example>Basic YourBase64String</example>
    /// <example>Bearer YourAuthToken</example>
    internal System.Threading.Tasks.Task ConfigureAsync(System.String Authorization);

    /// <summary>
    /// Stops and disposes the ClientWebSocket.
    /// </summary>
    /// <returns>A task to be wait.</returns>
    internal System.Threading.Tasks.ValueTask DisposeAsync();
    #endregion
    #endregion
  }
}