using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.Networking.WebSockets
{
    public class Client : System.IAsyncDisposable
    {
        #region Fields
        private readonly string URL;
        private readonly System.TimeSpan PingInterval;
        private readonly bool AutomaticReconnection;

        private System.Net.WebSockets.ClientWebSocket WebSocket;
        private ConnectionStates _State;
        private ConnectionStates _LastState;
        private long _Latency;
        private System.Threading.Timer _ReconnectionTimer;
        private bool _IsReconnection = false;
        #endregion

        #region Constructor
        public Client(string URL) : this(URL, System.TimeSpan.Zero, false) { }
        public Client(string URL, System.TimeSpan PingInterval) : this(URL, PingInterval, true) { }
        public Client(string URL, System.TimeSpan PingInterval, bool AutomaticReconnection)
        {
            this.URL = URL;
            this.PingInterval = PingInterval;
            if (this.PingInterval != System.TimeSpan.Zero && this.PingInterval.TotalSeconds < 1.0D)
                throw new System.Exception("Invalid ping interval. The minimum value is 1 second.");

            this.AutomaticReconnection = AutomaticReconnection;
            Headers = new System.Collections.Generic.Dictionary<string, string>();
            _State = ConnectionStates.Disconnected;
            _LastState = ConnectionStates.Disconnected;
            _Latency = 0;
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

        #region Actions
        private System.Action<string> ReceiveMessageAction;
        #endregion

        #region Events
        public event System.Action<System.Exception> Closed;
        public event System.Action<string> Reconnected;
        public event System.Action<System.Exception> Reconnecting;
        #endregion

        #region Properties
        public System.Collections.Generic.Dictionary<string, string> Headers { get; }
        public ConnectionStates State
        {
            get
            {
                return _State;
            }
            private set
            {
                _LastState = State;
                _State = value;
            }
        }
        public long Latency => _Latency;
        #endregion

        #region Methods
        public async System.Threading.Tasks.Task StartAsync(System.Threading.CancellationToken CancellationToken = default)
        {
            if (WebSocket == null)
            {
                WebSocket = new System.Net.WebSockets.ClientWebSocket();
                foreach (System.Collections.Generic.KeyValuePair<string, string> Header in Headers)
                    WebSocket.Options.SetRequestHeader(Header.Key, Header.Value);

                if (PingInterval != System.TimeSpan.Zero)
                    WebSocket.Options.SetRequestHeader("pingInterval", PingInterval.TotalMilliseconds.ToString());
            }

            switch (WebSocket.State)
            {
                case System.Net.WebSockets.WebSocketState.Connecting:
                case System.Net.WebSockets.WebSocketState.Open:
                    return;
                default:
                    break;
            }

            if (!_IsReconnection)
                State = ConnectionStates.Connecting;
            else
            {
                Reconnecting?.Invoke(null);
                State = ConnectionStates.Reconnecting;
            }

            try
            {
                await WebSocket.ConnectAsync(new System.Uri(URL), CancellationToken);
            }
            catch
            {
                State = ConnectionStates.Disconnected;
                throw;
            }

            if (WebSocket.State != System.Net.WebSockets.WebSocketState.Open)
            {
                State = ConnectionStates.Disconnected;
                return;
            }

            State = ConnectionStates.Connected;
            if (_IsReconnection)
                Reconnected?.Invoke(null);

            _ = System.Threading.Tasks.Task.Run(async () =>
            {
                const short BufferSize = 512;
                System.Net.WebSockets.WebSocketReceiveResult Result;
                do
                {
                    System.Collections.Generic.IEnumerable<byte> Data = new byte[] { };
                    do
                    {
                        byte[] Buffer = new byte[BufferSize];
                        Result = await WebSocket.ReceiveAsync(new System.ArraySegment<byte>(Buffer), System.Threading.CancellationToken.None);
                        if (Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text)
                            Data = Data.Concat(Buffer);
                    }
                    while (!Result.EndOfMessage);

                    if (Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Text && ReceiveMessageAction != null)
                    {
                        string Message = System.Text.Encoding.UTF8.GetString(Data.SkipLast(BufferSize - Result.Count).ToArray());

                        if (!Message.StartsWith("{\"pong\":"))
                            ReceiveMessageAction?.Invoke(Message);
                        else
                        {
                            long ServerUnixTime = Message.ToJsonElement().GetInt64("pong");
                            _Latency = ServerUnixTime > 0 ? System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - ServerUnixTime : 0;
                            /*
                            #if DEBUG
                            System.Console.WriteLine(Message); // Debug Pong Messages
                            #endif
                            */
                        }

                    }
                    else if (Result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
                    {
                        State = ConnectionStates.Disconnected;
                        Closed?.Invoke(Result.CloseStatus == System.Net.WebSockets.WebSocketCloseStatus.NormalClosure || Result.CloseStatusDescription == null ? null : new System.Exception(Result.CloseStatusDescription));
                    }
                }
                while (WebSocket.State == System.Net.WebSockets.WebSocketState.Open);
            });

            if (!_IsReconnection && PingInterval != System.TimeSpan.Zero)
                _ReconnectionTimer = new System.Threading.Timer(Reconnect, null, System.TimeSpan.Zero, PingInterval);

            _IsReconnection = true;
        }
        public void On(System.Action<string> ReceiveMessageAction) => this.ReceiveMessageAction = ReceiveMessageAction;
        public async System.Threading.Tasks.Task SendAsync(string Message, System.Threading.CancellationToken CancellationToken = default)
        {
            if (WebSocket.State == System.Net.WebSockets.WebSocketState.Open)
                await WebSocket.SendAsync(new System.ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(Message)), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken);
        }
        public async System.Threading.Tasks.Task StopAsync(System.Threading.CancellationToken CancellationToken = default)
        {
            _ReconnectionTimer?.Change(System.Threading.Timeout.Infinite, 0);

            switch (WebSocket.State)
            {
                case System.Net.WebSockets.WebSocketState.Open:
                    {
                        await WebSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "", CancellationToken);
                        State = ConnectionStates.Disconnected;
                        break;
                    }
                default:
                    break;
            }
        }
        public async System.Threading.Tasks.ValueTask DisposeAsync()
        {
            await StopAsync();

            _ReconnectionTimer?.Dispose();
            WebSocket.Dispose();
        }
        #endregion

        #region Reconnection
        private void Reconnect(object State)
        {
            if (WebSocket.State == System.Net.WebSockets.WebSocketState.Open)
            {
                string Message = $"{{\"ping\":{System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}}";
                /*
                #if DEBUG
                System.Console.WriteLine(Message); // Debug Ping Messages
                #endif
                */
                WebSocket.SendAsync(new System.ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(Message)), System.Net.WebSockets.WebSocketMessageType.Text, true, System.Threading.CancellationToken.None).ConfigureAwait(false);
                return;
            }

            if (WebSocket.State == System.Net.WebSockets.WebSocketState.Connecting)
                return;

            if (_LastState != _State)
            {
                this.State = ConnectionStates.Disconnected;
                Closed?.Invoke(WebSocket.CloseStatus == System.Net.WebSockets.WebSocketCloseStatus.NormalClosure || WebSocket.CloseStatusDescription == null ? null : new System.Exception(WebSocket.CloseStatusDescription));
            }

            if (!AutomaticReconnection || WebSocket.CloseStatus == System.Net.WebSockets.WebSocketCloseStatus.NormalClosure)
                return;

            WebSocket.Abort();
            WebSocket.Dispose();
            WebSocket = null;
            try { StartAsync().ConfigureAwait(false); } catch { }
        }
        #endregion
    }
}