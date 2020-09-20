using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Asterisk.ARI
{
  public enum EventDispatchingStrategy
  {
    ThreadPool,
    DedicatedThread,
    AsyncTask
  }

  public class AriClient : BaseAriClient, System.IDisposable, IAriClient
  {
    #region Constructors
    public AriClient(SoftmakeAll.SDK.Asterisk.ARI.Middleware.IActionConsumer actionConsumer, SoftmakeAll.SDK.Asterisk.ARI.Middleware.IEventProducer eventProducer, string application, bool subscribeAllEvents = false, bool ssl = false)
    {
      _actionConsumer = actionConsumer;
      _eventProducer = eventProducer;
      EventDispatchingStrategy = DefaultEventDispatchingStrategy;

      Asterisk = new SoftmakeAll.SDK.Asterisk.ARI.Actions.AsteriskActions(_actionConsumer);
      Applications = new SoftmakeAll.SDK.Asterisk.ARI.Actions.ApplicationsActions(_actionConsumer);
      Bridges = new SoftmakeAll.SDK.Asterisk.ARI.Actions.BridgesActions(_actionConsumer);
      Channels = new SoftmakeAll.SDK.Asterisk.ARI.Actions.ChannelsActions(_actionConsumer);
      DeviceStates = new SoftmakeAll.SDK.Asterisk.ARI.Actions.DeviceStatesActions(_actionConsumer);
      Endpoints = new SoftmakeAll.SDK.Asterisk.ARI.Actions.EndpointsActions(_actionConsumer);
      Events = new SoftmakeAll.SDK.Asterisk.ARI.Actions.EventsActions(_actionConsumer);
      Mailboxes = new SoftmakeAll.SDK.Asterisk.ARI.Actions.MailboxesActions(_actionConsumer);
      Playbacks = new SoftmakeAll.SDK.Asterisk.ARI.Actions.PlaybacksActions(_actionConsumer);
      Recordings = new SoftmakeAll.SDK.Asterisk.ARI.Actions.RecordingsActions(_actionConsumer);
      Sounds = new SoftmakeAll.SDK.Asterisk.ARI.Actions.SoundsActions(_actionConsumer);

      _eventProducer.OnMessageReceived += _eventProducer_OnMessageReceived;
      _eventProducer.OnConnectionStateChanged += _eventProducer_OnConnectionStateChanged;

      _subscribeAllEvents = subscribeAllEvents;
      _ssl = ssl;
    }
    public AriClient(StasisEndpoint endPoint, string application, bool subscribeAllEvents = false, bool ssl = false) :
      this(new SoftmakeAll.SDK.Asterisk.ARI.Middleware.Default.RestActionConsumer(endPoint), new SoftmakeAll.SDK.Asterisk.ARI.Middleware.Default.WebSocketEventProducer(endPoint, application), application, subscribeAllEvents, ssl)
    { }
    #endregion

    #region Constants
    public const EventDispatchingStrategy DefaultEventDispatchingStrategy = EventDispatchingStrategy.ThreadPool;
    #endregion

    #region Events
    public delegate void ConnectionStateChangedHandler(object sender);
    public event ConnectionStateChangedHandler OnConnectionStateChanged;
    #endregion

    #region Fields
    private readonly SoftmakeAll.SDK.Asterisk.ARI.Middleware.IActionConsumer _actionConsumer;
    private readonly SoftmakeAll.SDK.Asterisk.ARI.Middleware.IEventProducer _eventProducer;
    private readonly object _syncRoot = new System.Object();
    private readonly bool _subscribeAllEvents;
    private readonly bool _ssl;
    private bool _autoReconnect;
    private System.TimeSpan _autoReconnectDelay;
    private IAriDispatcher _dispatcher;
    #endregion

    #region Public Properties
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IAsteriskActions Asterisk { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IApplicationsActions Applications { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IBridgesActions Bridges { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IChannelsActions Channels { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IDeviceStatesActions DeviceStates { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IEndpointsActions Endpoints { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IEventsActions Events { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IMailboxesActions Mailboxes { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IPlaybacksActions Playbacks { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.IRecordingsActions Recordings { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Actions.ISoundsActions Sounds { get; set; }
    public EventDispatchingStrategy EventDispatchingStrategy { get; set; }
    public bool Connected => _eventProducer.State == SoftmakeAll.SDK.Asterisk.ARI.Middleware.ConnectionState.Open;
    public SoftmakeAll.SDK.Asterisk.ARI.Middleware.ConnectionState ConnectionState => _eventProducer.State;
    #endregion

    #region Private and Protected Methods
    private void _eventProducer_OnConnectionStateChanged(object sender, System.EventArgs e)
    {
      if (_eventProducer.State != SoftmakeAll.SDK.Asterisk.ARI.Middleware.ConnectionState.Open)
        Reconnect();

      OnConnectionStateChanged?.Invoke(sender);
    }
    private void _eventProducer_OnMessageReceived(object sender, SoftmakeAll.SDK.Asterisk.ARI.Middleware.MessageEventArgs e)
    {
      System.Type type = System.Type.GetType("SoftmakeAll.SDK.Asterisk.ARI.Models." + e.Message.ToJsonElement().GetString("type") + "Event");
      var evnt =
          (type != null)
        ? (SoftmakeAll.SDK.Asterisk.ARI.Models.Event)System.Text.Json.JsonSerializer.Deserialize(e.Message.ToJsonElement().ToRawText(), type, SoftmakeAll.SDK.Asterisk.ARI.Serializations.JsonSerializerOptions)
        : (SoftmakeAll.SDK.Asterisk.ARI.Models.Event)System.Text.Json.JsonSerializer.Deserialize(e.Message.ToJsonElement().ToRawText(), typeof(SoftmakeAll.SDK.Asterisk.ARI.Models.Event), SoftmakeAll.SDK.Asterisk.ARI.Serializations.JsonSerializerOptions);

      lock (_syncRoot)
        _dispatcher?.QueueAction(() => { try { FireEvent(evnt.Type, evnt, this); } catch { } });
    }
    private void Reconnect()
    {
      System.TimeSpan reconnectDelay;

      lock (_syncRoot)
      {
        var shouldReconnect = _autoReconnect && _eventProducer.State != SoftmakeAll.SDK.Asterisk.ARI.Middleware.ConnectionState.Open && _eventProducer.State != SoftmakeAll.SDK.Asterisk.ARI.Middleware.ConnectionState.Connecting;
        if (!shouldReconnect)
          return;
        reconnectDelay = _autoReconnectDelay;
      }

      if (reconnectDelay != System.TimeSpan.Zero)
        System.Threading.Thread.Sleep(reconnectDelay);
      _eventProducer.Connect(_subscribeAllEvents, _ssl);
    }
    IAriDispatcher CreateDispatcher()
    {
      switch (EventDispatchingStrategy)
      {
        case EventDispatchingStrategy.DedicatedThread: return new SoftmakeAll.SDK.Asterisk.ARI.Dispatchers.DedicatedThreadDispatcher();
        case EventDispatchingStrategy.ThreadPool: return new SoftmakeAll.SDK.Asterisk.ARI.Dispatchers.ThreadPoolDispatcher();
        case EventDispatchingStrategy.AsyncTask: return new SoftmakeAll.SDK.Asterisk.ARI.Dispatchers.AsyncDispatcher();
      }

      throw new AriException(EventDispatchingStrategy.ToString());
    }
    #endregion

    #region Public Methods
    public void Connect(bool autoReconnect = true, int autoReconnectDelay = 5)
    {
      lock (_syncRoot)
      {
        _autoReconnect = autoReconnect;
        _autoReconnectDelay = System.TimeSpan.FromSeconds(autoReconnectDelay);
        if (_dispatcher == null)
          _dispatcher = CreateDispatcher();
      }
      _eventProducer.Connect(_subscribeAllEvents, _ssl);
    }
    public void Disconnect()
    {
      lock (_syncRoot)
      {
        _autoReconnect = false;
        if (_dispatcher != null)
        {
          _dispatcher.Dispose();
          _dispatcher = null;
        }
      }
      _eventProducer.Disconnect();
    }
    #endregion

    #region Destructors
    public void Dispose()
    {
      _eventProducer.OnConnectionStateChanged -= _eventProducer_OnConnectionStateChanged;
      _eventProducer.OnMessageReceived -= _eventProducer_OnMessageReceived;
      Disconnect();
    }
    #endregion
  }
}