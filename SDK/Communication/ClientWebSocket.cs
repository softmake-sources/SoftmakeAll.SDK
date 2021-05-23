using System.Linq;

namespace SoftmakeAll.SDK.Communication
{
  public class ClientWebSocket
  {
    #region Fields
    private readonly System.String URL;
    private readonly System.TimeSpan PingInterval;
    private readonly System.Boolean AutomaticReconnection;

    private System.Net.WebSockets.ClientWebSocket WebSocket = new System.Net.WebSockets.ClientWebSocket();
    private SoftmakeAll.SDK.Communication.ClientWebSocket.ConnectionStates _State;
    private SoftmakeAll.SDK.Communication.ClientWebSocket.ConnectionStates _LastState;
    private System.Boolean _IsReconnection = false;
    #endregion

    #region Constructor
    public ClientWebSocket(System.String URL) : this(URL, System.TimeSpan.Zero, false) { }
    public ClientWebSocket(System.String URL, System.TimeSpan PingInterval) : this(URL, PingInterval, true) { }
    public ClientWebSocket(System.String URL, System.TimeSpan PingInterval, System.Boolean AutomaticReconnection)
    {
      this.URL = URL;
      this.PingInterval = PingInterval;
      this.AutomaticReconnection = AutomaticReconnection;
      this._State = ConnectionStates.Disconnected;
      this._LastState = ConnectionStates.Disconnected;
    }
    #endregion

    #region Enums
    public enum ConnectionStates
    {
      Disconnected = 0,
      Connected = 1,
      Connecting = 2,
      Reconnecting = 3
    }
    #endregion

    #region Functions and Actions
    private System.Action<System.String> ReceiveMessageAction;
    #endregion

    #region Events
    public event System.Func<System.Exception, System.Threading.Tasks.Task> Closed;
    public event System.Func<System.String, System.Threading.Tasks.Task> Reconnected;
    public event System.Func<System.Exception, System.Threading.Tasks.Task> Reconnecting;
    #endregion

    #region Properties
    public SoftmakeAll.SDK.Communication.ClientWebSocket.ConnectionStates State
    {
      get
      {
        return this._State;
      }
      private set
      {
        this._LastState = this.State;
        this._State = value;
      }
    }
    #endregion

    #region Methods
    private void Reconnect(System.Object State)
    {
      if (this.WebSocket.State == System.Net.WebSockets.WebSocketState.Open)
      {
        this.SendAsync($"{{ \"ping\": \"{System.DateTimeOffset.UtcNow}\" }}").ConfigureAwait(false);
        return;
      }

      if (this.WebSocket.State == System.Net.WebSockets.WebSocketState.Connecting)
        return;

      if (this._LastState != this._State)
      {
        this.State = SoftmakeAll.SDK.Communication.ClientWebSocket.ConnectionStates.Disconnected;
        this.Closed?.Invoke(this.WebSocket.CloseStatus == System.Net.WebSockets.WebSocketCloseStatus.NormalClosure || this.WebSocket.CloseStatusDescription == null ? null : new System.Exception(this.WebSocket.CloseStatusDescription));
      }

      if ((!(this.AutomaticReconnection)) || (this.WebSocket.CloseStatus == System.Net.WebSockets.WebSocketCloseStatus.NormalClosure))
        return;

      this.WebSocket.Abort();
      this.WebSocket.Dispose();
      this.WebSocket = new System.Net.WebSockets.ClientWebSocket();
      try { this.StartAsync().ConfigureAwait(false); } catch { }
    }
    public async System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken CancellationToken = default)
    {
      switch (this.WebSocket.State)
      {
        case System.Net.WebSockets.WebSocketState.Connecting:
        case System.Net.WebSockets.WebSocketState.Open:
          return;
        default:
          break;
      }

      if (!(this._IsReconnection))
        this.State = SoftmakeAll.SDK.Communication.ClientWebSocket.ConnectionStates.Connecting;
      else
      {
        this.Reconnecting?.Invoke(null);
        this.State = SoftmakeAll.SDK.Communication.ClientWebSocket.ConnectionStates.Reconnecting;
      }

      try
      {
        await this.WebSocket.ConnectAsync(new System.Uri(this.URL), CancellationToken);
      }
      catch
      {
        this.State = SoftmakeAll.SDK.Communication.ClientWebSocket.ConnectionStates.Disconnected;
        throw;
      }

      if (this.WebSocket.State != System.Net.WebSockets.WebSocketState.Open)
      {
        this.State = SoftmakeAll.SDK.Communication.ClientWebSocket.ConnectionStates.Disconnected;
        return;
      }

      this.State = SoftmakeAll.SDK.Communication.ClientWebSocket.ConnectionStates.Connected;
      if (this._IsReconnection)
        this.Reconnected?.Invoke(null);

      _ = System.Threading.Tasks.Task.Run(async () =>
      {
        const System.Int16 BufferSize = 512;
        System.Net.WebSockets.WebSocketReceiveResult Result;
        do
        {
          System.Collections.Generic.IEnumerable<System.Byte> Data = new System.Byte[] { };

          do
          {
            System.Byte[] Buffer = new System.Byte[BufferSize];
            Result = await this.WebSocket.ReceiveAsync(new System.ArraySegment<System.Byte>(Buffer), System.Threading.CancellationToken.None);
            if (Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text)
              Data = Data.Concat(Buffer);
          }
          while (!(Result.EndOfMessage));

          if ((Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text) && (this.ReceiveMessageAction != null))
          {
            this.ReceiveMessageAction(System.Text.Encoding.UTF8.GetString(Data.SkipLast(BufferSize - Result.Count).ToArray()));
          }
          else if (Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
          {
            this.State = SoftmakeAll.SDK.Communication.ClientWebSocket.ConnectionStates.Disconnected;
            this.Closed?.Invoke(Result.CloseStatus == System.Net.WebSockets.WebSocketCloseStatus.NormalClosure || Result.CloseStatusDescription == null ? null : new System.Exception(Result.CloseStatusDescription));
          }
        }
        while (this.WebSocket.State == System.Net.WebSockets.WebSocketState.Open);
      });

      if ((!(this._IsReconnection)) && (this.PingInterval != System.TimeSpan.Zero))
        _ = new System.Threading.Timer(this.Reconnect, null, System.TimeSpan.Zero, this.PingInterval);

      this._IsReconnection = true;
    }
    public async System.Threading.Tasks.Task SendAsync(System.String Message, System.Threading.CancellationToken CancellationToken = default)
    {
      if (this.WebSocket.State == System.Net.WebSockets.WebSocketState.Open)
        await this.WebSocket.SendAsync(new System.ArraySegment<System.Byte>(System.Text.Encoding.UTF8.GetBytes(Message)), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken);
    }
    public void On(System.Action<System.String> ReceiveMessageAction) => this.ReceiveMessageAction = ReceiveMessageAction;
    public async System.Threading.Tasks.Task StopAsync(System.Threading.CancellationToken CancellationToken = default)
    {
      switch (this.WebSocket.State)
      {
        case System.Net.WebSockets.WebSocketState.Open:
          {
            await this.WebSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, null, CancellationToken);
            this.State = SoftmakeAll.SDK.Communication.ClientWebSocket.ConnectionStates.Disconnected;
            break;
          }
        default:
          break;
      }
    }
    public async System.Threading.Tasks.ValueTask DisposeAsync()
    {
      await this.StopAsync(new System.Threading.CancellationToken(true));
      this.WebSocket.Dispose();
    }
    #endregion
  }
}