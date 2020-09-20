namespace SoftmakeAll.SDK.Asterisk.ARI.Middleware.Default
{
  public class WebSocketEventProducer : IEventProducer
  {
    #region Constructor
    public WebSocketEventProducer(StasisEndpoint connectionInfo, string application)
    {
      _connectionInfo = connectionInfo;
      _application = application;
    }
    #endregion

    #region Fields
    private readonly string _application;
    private readonly StasisEndpoint _connectionInfo;
    private WebSocket4Net.WebSocket _client;
    private WebSocket4Net.WebSocketState _lastKnownState;
    #endregion

    #region Events
    public event System.EventHandler<MessageEventArgs> OnMessageReceived;
    public event System.EventHandler OnConnectionStateChanged;
    #endregion

    #region Public Properties
    public bool Connected => _client != null && _client.State == WebSocket4Net.WebSocketState.Open;
    public ConnectionState State => _client == null ? ConnectionState.None : (ConnectionState)_client.State;
    #endregion

    #region Public Methods
    public void Connect(bool subscribeAll = false, bool ssl = false)
    {
      try
      {
        _client = new WebSocket4Net.WebSocket($"ws{(ssl ? "s" : "")}://{_connectionInfo.Host}:{_connectionInfo.Port}/ari/events?app={_application}&subscribeAll={subscribeAll}&api_key={($"{_connectionInfo.Username}:{_connectionInfo.Password}")}");
        _client.MessageReceived += _client_MessageReceived;
        _client.Opened += _client_Opened;
        _client.Error += _client_Error;
        _client.DataReceived += _client_DataReceived;
        _client.Closed += _client_Closed;
        _client.Open();
      }
      catch (System.Exception ex)
      {
        throw new AriException(ex.Message);
      }
    }
    public void Disconnect() => _client?.Close();
    #endregion

    #region SocketEvents
    private void _client_Closed(object sender, System.EventArgs e) => RaiseOnConnectionStateChanged();
    private void _client_DataReceived(object sender, WebSocket4Net.DataReceivedEventArgs e) { }
    private void _client_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e) { if (e.Exception is System.Net.Sockets.SocketException) RaiseOnConnectionStateChanged(); }
    private void _client_Opened(object sender, System.EventArgs e) => RaiseOnConnectionStateChanged();
    private void _client_MessageReceived(object sender, WebSocket4Net.MessageReceivedEventArgs e) => this.OnMessageReceived?.Invoke(this, new MessageEventArgs() { Message = e.Message });
    #endregion

    #region Private Methods
    protected virtual void RaiseOnConnectionStateChanged()
    {
      if (_client.State == _lastKnownState)
        return;

      _lastKnownState = _client.State;
      OnConnectionStateChanged?.Invoke(this, null);
    }
    #endregion
  }
}