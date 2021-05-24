using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.Communication
{
  public class ServerWebSocket : System.IDisposable
  {
    #region Fields
    private System.Net.HttpListener HttpListener;
    private System.Collections.Generic.Dictionary<System.String, SoftmakeAll.SDK.Communication.ServerWebSocket.ConnectionProperties> ActiveConnections;
    private System.Threading.Timer IdleDisconnectionTimer;
    private readonly System.Double KeepAliveIntervalTotalMilliseconds;
    private readonly System.Object SyncRoot = new System.Object();
    #endregion

    #region Constructor
    public ServerWebSocket(System.String URL) : this(URL, System.TimeSpan.Zero) { }
    public ServerWebSocket(System.String URL, System.TimeSpan KeepAliveInterval)
    {
      this.HttpListener = new System.Net.HttpListener();
      this.HttpListener.Prefixes.Add(URL);
      this.ActiveConnections = new System.Collections.Generic.Dictionary<System.String, SoftmakeAll.SDK.Communication.ServerWebSocket.ConnectionProperties>();

      if (KeepAliveInterval != System.TimeSpan.Zero)
      {
        this.IdleDisconnectionTimer = new System.Threading.Timer(this.DisconnectIdleClients, null, System.TimeSpan.Zero, KeepAliveInterval);
        this.KeepAliveIntervalTotalMilliseconds = KeepAliveInterval.TotalMilliseconds;
      }
    }
    #endregion

    #region Subclasses
    private class ConnectionProperties
    {
      #region Fields
      internal readonly System.Net.WebSockets.WebSocketContext WebSocketContext;
      public readonly System.String ConnectionID = System.Guid.NewGuid().ToString();
      #endregion

      #region Constructor
      public ConnectionProperties(System.Net.WebSockets.WebSocketContext WebSocketContext)
      {
        this.WebSocketContext = WebSocketContext;
        this.LastPingTime = System.DateTimeOffset.UtcNow;
      }
      #endregion

      #region Properties
      public System.DateTimeOffset LastPingTime { get; set; }
      #endregion
    }
    #endregion

    #region Actions
    private System.Func<System.String, System.String> ReceiveMessageFunc;
    #endregion

    #region Events
    public event System.Action<System.String> ClientConnected;
    public event System.Action<System.String> ClientDisconnected;
    #endregion

    #region Methods
    public async System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken CancellationToken = default)
    {
      this.HttpListener.Start();

      do
      {
        System.Net.HttpListenerContext HttpListenerContext = await this.HttpListener.GetContextAsync();
        if (HttpListenerContext.Request.IsWebSocketRequest)
        {
          try { _ = this.ProcessRequestAsync(HttpListenerContext, CancellationToken); } catch { }
        }
        else
        {
          HttpListenerContext.Response.StatusCode = 400;
          HttpListenerContext.Response.Close();
        }
      }
      while (true);
    }
    public void On(System.Func<System.String, System.String> ReceiveMessageFunc) => this.ReceiveMessageFunc = ReceiveMessageFunc;
    public void Stop()
    {
      this.IdleDisconnectionTimer?.Change(System.Threading.Timeout.Infinite, 0);
      this.HttpListener.Stop();
    }
    public void Dispose()
    {
      this.Stop();

      this.IdleDisconnectionTimer?.Dispose();

      this.HttpListener.Abort();
      this.HttpListener = null;
    }

    private async System.Threading.Tasks.Task ProcessRequestAsync(System.Net.HttpListenerContext HttpListenerContext, System.Threading.CancellationToken CancellationToken = default)
    {
      System.Net.WebSockets.WebSocketContext WebSocketContext = null;
      try
      {
        WebSocketContext = await HttpListenerContext.AcceptWebSocketAsync(null);
      }
      catch
      {
        HttpListenerContext.Response.StatusCode = 500;
        HttpListenerContext.Response.Close();
        return;
      }

      SoftmakeAll.SDK.Communication.ServerWebSocket.ConnectionProperties ConnectionProperties = new SoftmakeAll.SDK.Communication.ServerWebSocket.ConnectionProperties(WebSocketContext);

      lock (this.SyncRoot)
        this.ActiveConnections.Add(ConnectionProperties.ConnectionID, ConnectionProperties);

      this.ClientConnected?.Invoke(ConnectionProperties.ConnectionID);

      const System.Int16 BufferSize = 512;
      System.Net.WebSockets.WebSocketReceiveResult Result;
      do
      {
        System.Collections.Generic.IEnumerable<System.Byte> Data = new System.Byte[] { };
        do
        {
          System.Byte[] Buffer = new System.Byte[BufferSize];
          Result = await WebSocketContext.WebSocket.ReceiveAsync(new System.ArraySegment<System.Byte>(Buffer), CancellationToken);
          if (Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text)
            Data = Data.Concat(Buffer);
        }
        while (!(Result.EndOfMessage));

        if ((Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text) && (this.ReceiveMessageFunc != null))
          await this.ReplyAsync(ConnectionProperties, System.Text.Encoding.UTF8.GetString(Data.SkipLast(BufferSize - Result.Count).ToArray()), CancellationToken);
        else if (Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
          await this.DropConnectionAsync(ConnectionProperties, true, CancellationToken);
      }
      while (WebSocketContext.WebSocket.State == System.Net.WebSockets.WebSocketState.Open);
    }
    private async System.Threading.Tasks.Task ReplyAsync(SoftmakeAll.SDK.Communication.ServerWebSocket.ConnectionProperties ConnectionProperties, System.String Message, System.Threading.CancellationToken CancellationToken = default)
    {
      if (ConnectionProperties.WebSocketContext.WebSocket.State != System.Net.WebSockets.WebSocketState.Open)
        return;

      if (!(Message.StartsWith("{\"ping\":")))
        try { Message = this.ReceiveMessageFunc?.Invoke(Message); } catch { Message = "{\"error\":true}"; }
      else
      {
        /*
        #if DEBUG
        System.Console.WriteLine(Message); // Debug Ping Messages
        #endif
        */
        System.Int64 ClientUnixTime = Message.ToJsonElement().GetInt64("ping");
        ConnectionProperties.LastPingTime = System.DateTimeOffset.UtcNow;
        System.Int64 CurrentUnixTime = ConnectionProperties.LastPingTime.ToUnixTimeMilliseconds();
        Message = $"{{\"pong\":{CurrentUnixTime}{(ClientUnixTime > 0 ? $",\"lat\":{CurrentUnixTime - ClientUnixTime}" : "")}}}";
        /*
        #if DEBUG
        System.Console.WriteLine(Message); // Debug Ping Messages
        #endif
        */
      }

      await ConnectionProperties.WebSocketContext.WebSocket.SendAsync(new System.ArraySegment<System.Byte>(System.Text.Encoding.UTF8.GetBytes(Message)), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken);
    }
    private void DisconnectIdleClients(System.Object State)
    {
      lock (this.SyncRoot)
        foreach (System.String ConnectionID in this.ActiveConnections.Select(ac => ac.Key))
        {
          if (System.DateTimeOffset.UtcNow.Subtract(this.ActiveConnections[ConnectionID].LastPingTime).TotalMilliseconds > this.KeepAliveIntervalTotalMilliseconds)
            this.DropConnectionAsync(this.ActiveConnections[ConnectionID], false, System.Threading.CancellationToken.None).ConfigureAwait(false);
        }
    }
    private async System.Threading.Tasks.Task DropConnectionAsync(SoftmakeAll.SDK.Communication.ServerWebSocket.ConnectionProperties ConnectionProperties, System.Boolean NormalClosure, System.Threading.CancellationToken CancellationToken = default)
    {
      lock (this.SyncRoot)
        this.ActiveConnections.Remove(ConnectionProperties.ConnectionID);

      this.ClientDisconnected?.Invoke(ConnectionProperties.ConnectionID);

      try
      {
        if (NormalClosure)
          await ConnectionProperties.WebSocketContext.WebSocket.CloseOutputAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "", CancellationToken);
        else
          await ConnectionProperties.WebSocketContext.WebSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.ProtocolError, "Inactivity timeout has been reached.", CancellationToken);
      }
      catch { }
    }
    #endregion
  }
}