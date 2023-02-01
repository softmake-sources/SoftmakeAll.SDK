using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.Networking.WebSockets
{
    public class Server : System.IDisposable
    {
        #region Fields
        private System.Net.HttpListener HttpListener;
        private System.Threading.Timer IdleDisconnectionTimer;
        private readonly double KeepAliveIntervalTotalMilliseconds;
        private readonly object SyncRoot = new object();
        #endregion

        #region Constructor
        public Server(string URL) : this(URL, System.TimeSpan.Zero) { }
        public Server(string URL, System.TimeSpan KeepAliveInterval)
        {
            HttpListener = new System.Net.HttpListener();
            HttpListener.Prefixes.Add(URL);
            ActiveConnections = new System.Collections.Generic.Dictionary<string, ConnectionProperties>();

            if (KeepAliveInterval != System.TimeSpan.Zero)
            {
                IdleDisconnectionTimer = new System.Threading.Timer(DisconnectIdleClients, null, System.TimeSpan.Zero, KeepAliveInterval);
                KeepAliveIntervalTotalMilliseconds = KeepAliveInterval.TotalMilliseconds;
            }
        }
        #endregion

        #region Subclasses
        public class ConnectionProperties
        {
            #region Fields
            internal readonly string ConnectionID = System.Guid.NewGuid().ToString();
            #endregion

            #region Constructor
            public ConnectionProperties(System.Net.WebSockets.WebSocketContext WebSocketContext)
            {
                LastPingTime = System.DateTimeOffset.UtcNow;
                this.WebSocketContext = WebSocketContext;
            }
            #endregion

            #region Properties
            public System.DateTimeOffset LastPingTime { get; set; }
            public System.Net.WebSockets.WebSocketContext WebSocketContext { get; }
            #endregion
        }
        #endregion

        #region Actions
        private System.Func<string, string> ReceiveMessageFunc;
        #endregion

        #region Events
        public event System.Action<string> ClientConnected;
        public event System.Action<string> ClientDisconnected;
        #endregion


        #region Properties
        public System.Collections.Generic.Dictionary<string, ConnectionProperties> ActiveConnections { get; }
        #endregion

        #region Methods
        public async System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken CancellationToken = default)
        {
            HttpListener.Start();

            do
            {
                System.Net.HttpListenerContext HttpListenerContext = await HttpListener.GetContextAsync();
                if (HttpListenerContext.Request.IsWebSocketRequest)
                {
                    try { _ = ProcessRequestAsync(HttpListenerContext, CancellationToken); } catch { }
                }
                else
                {
                    HttpListenerContext.Response.StatusCode = 400;
                    HttpListenerContext.Response.Close();
                }
            }
            while (true);
        }
        public void On(System.Func<string, string> ReceiveMessageFunc) => this.ReceiveMessageFunc = ReceiveMessageFunc;
        public void SendToAllClients(string Message, System.Threading.CancellationToken CancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(Message))
                lock (SyncRoot)
                    foreach (ConnectionProperties ConnectionProperties in ActiveConnections.Values)
                        try { ConnectionProperties.WebSocketContext.WebSocket.SendAsync(new System.ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(Message)), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken).ConfigureAwait(false); } catch { }
        }
        public void SendToSpecificClients(string Message, System.Collections.Generic.List<string> ConnectionsID, System.Threading.CancellationToken CancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(Message) && ConnectionsID != null && ConnectionsID.Any())
                lock (SyncRoot)
                    foreach (ConnectionProperties ConnectionProperties in ActiveConnections.Where(ac => ConnectionsID.Contains(ac.Key)).Select(ac => ac.Value))
                        try { ConnectionProperties.WebSocketContext.WebSocket.SendAsync(new System.ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(Message)), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken).ConfigureAwait(false); } catch { }
        }
        public void SendToSpecificClient(string Message, string ConnectionID, System.Threading.CancellationToken CancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(Message) && !string.IsNullOrWhiteSpace(ConnectionID))
                lock (SyncRoot)
                {
                    ConnectionProperties ConnectionProperties = ActiveConnections.FirstOrDefault(ac => ac.Key == ConnectionID).Value;
                    if (ConnectionProperties != null)
                        try { ConnectionProperties.WebSocketContext.WebSocket.SendAsync(new System.ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(Message)), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken).ConfigureAwait(false); } catch { }
                }
        }
        public void Stop()
        {
            IdleDisconnectionTimer?.Change(System.Threading.Timeout.Infinite, 0);
            HttpListener.Stop();
        }
        public void Dispose()
        {
            Stop();

            IdleDisconnectionTimer?.Dispose();

            HttpListener.Abort();
            HttpListener = null;
        }

        private async System.Threading.Tasks.Task ProcessRequestAsync(System.Net.HttpListenerContext HttpListenerContext, System.Threading.CancellationToken CancellationToken = default)
        {
            System.Net.WebSockets.WebSocketContext WebSocketContext;
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

            ConnectionProperties ConnectionProperties = new ConnectionProperties(WebSocketContext);

            lock (SyncRoot)
                ActiveConnections.Add(ConnectionProperties.ConnectionID, ConnectionProperties);

            ClientConnected?.Invoke(ConnectionProperties.ConnectionID);

            const short BufferSize = 512;
            System.Net.WebSockets.WebSocketReceiveResult Result;
            do
            {
                System.Collections.Generic.IEnumerable<byte> Data = new byte[] { };
                do
                {
                    byte[] Buffer = new byte[BufferSize];
                    Result = await WebSocketContext.WebSocket.ReceiveAsync(new System.ArraySegment<byte>(Buffer), CancellationToken);
                    if (Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text)
                        Data = Data.Concat(Buffer);
                }
                while (!Result.EndOfMessage);

                if (Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text && ReceiveMessageFunc != null)
                    await ReplyAsync(ConnectionProperties, System.Text.Encoding.UTF8.GetString(Data.SkipLast(BufferSize - Result.Count).ToArray()), CancellationToken);
                else if (Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                    await DropConnectionAsync(ConnectionProperties, true, CancellationToken);
            }
            while (WebSocketContext.WebSocket.State == System.Net.WebSockets.WebSocketState.Open);
        }
        private async System.Threading.Tasks.Task ReplyAsync(ConnectionProperties ConnectionProperties, string Message, System.Threading.CancellationToken CancellationToken = default)
        {
            if (ConnectionProperties.WebSocketContext.WebSocket.State != System.Net.WebSockets.WebSocketState.Open)
                return;

            if (!Message.StartsWith("{\"ping\":"))
                try { Message = ReceiveMessageFunc?.Invoke(Message); } catch { Message = "{\"error\":true}"; }
            else
            {
                /*
                #if DEBUG
                System.Console.WriteLine(Message); // Debug Ping Messages
                #endif
                */
                long ClientUnixTime = Message.ToJsonElement().GetInt64("ping");
                ConnectionProperties.LastPingTime = System.DateTimeOffset.UtcNow;
                long CurrentUnixTime = ConnectionProperties.LastPingTime.ToUnixTimeMilliseconds();
                Message = $"{{\"pong\":{CurrentUnixTime}{(ClientUnixTime > 0 ? $",\"lat\":{CurrentUnixTime - ClientUnixTime}" : "")}}}";
                /*
                #if DEBUG
                System.Console.WriteLine(Message); // Debug Ping Messages
                #endif
                */
            }

            await ConnectionProperties.WebSocketContext.WebSocket.SendAsync(new System.ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(Message)), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken);
        }
        private void DisconnectIdleClients(object State)
        {
            lock (SyncRoot)
                foreach (string ConnectionID in ActiveConnections.Select(ac => ac.Key))
                    if (System.DateTimeOffset.UtcNow.Subtract(ActiveConnections[ConnectionID].LastPingTime).TotalMilliseconds > KeepAliveIntervalTotalMilliseconds)
                        DropConnectionAsync(ActiveConnections[ConnectionID], false, System.Threading.CancellationToken.None).ConfigureAwait(false);
        }
        private async System.Threading.Tasks.Task DropConnectionAsync(ConnectionProperties ConnectionProperties, bool NormalClosure, System.Threading.CancellationToken CancellationToken = default)
        {
            lock (SyncRoot)
                ActiveConnections.Remove(ConnectionProperties.ConnectionID);

            ClientDisconnected?.Invoke(ConnectionProperties.ConnectionID);

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