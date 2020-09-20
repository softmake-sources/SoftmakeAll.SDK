namespace SoftmakeAll.SDK.Asterisk.ARI.Middleware
{
  public enum ConnectionState
  {
    None = WebSocket4Net.WebSocketState.None,
    Connecting = WebSocket4Net.WebSocketState.Connecting,
    Open = WebSocket4Net.WebSocketState.Open,
    Closing = WebSocket4Net.WebSocketState.Closing,
    Closed = WebSocket4Net.WebSocketState.Closed
  }

  public class MessageEventArgs
  {
    #region Properties
    public string Message { get; set; }
    #endregion
  }

  public interface IEventProducer
  {
    #region Properties
    public ConnectionState State { get; }
    #endregion

    #region Events
    public event System.EventHandler<MessageEventArgs> OnMessageReceived;
    public event System.EventHandler OnConnectionStateChanged;
    #endregion

    #region Methods
    public void Connect(bool subscribeAll = false, bool ssl = false);
    public void Disconnect();
    #endregion
  }
}