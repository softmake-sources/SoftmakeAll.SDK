using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Asterisk.ARI.Middleware.Default
{
  public class RestActionConsumer : IActionConsumer
  {
    #region Fields
    private readonly StasisEndpoint _connectionInfo;
    #endregion

    #region Constructor
    public RestActionConsumer(StasisEndpoint connectionInfo) => this._connectionInfo = connectionInfo;
    #endregion

    #region Methods
    public IRestCommand GetRestCommand(HttpMethod method, string path) => new Command(this._connectionInfo, path) { Method = method.ToString() };
    public IRestCommandResult<T> ProcessRestCommand<T>(IRestCommand command) where T : new()
    {
      var cmd = (Command)command;
      var result = cmd.Client.Send();
      T data = default;
      System.String resultText = result.ToRawText();
      if (!(System.String.IsNullOrWhiteSpace(resultText)))
      {
        resultText = resultText.ToJsonElement().ToRawText();
        if (!(System.String.IsNullOrWhiteSpace(resultText)))
          data = System.Text.Json.JsonSerializer.Deserialize<T>(resultText, SoftmakeAll.SDK.Asterisk.ARI.Serializations.JsonSerializerOptions);
      }
      var rtn = new CommandResult<T> { StatusCode = cmd.Client.StatusCode, Data = data };
      return rtn;
    }
    public IRestCommandResult ProcessRestCommand(IRestCommand command)
    {
      var cmd = (Command)command;
      var result = cmd.Client.Send();
      System.Byte[] rawData = null;
      System.String resultText = result.ToRawText();
      if (!(System.String.IsNullOrWhiteSpace(resultText)))
      {
        resultText = resultText.ToJsonElement().ToRawText();
        if (!(System.String.IsNullOrWhiteSpace(resultText)))
          rawData = System.Text.Encoding.UTF8.GetBytes(resultText);
      }
      var rtn = new CommandResult { StatusCode = cmd.Client.StatusCode, RawData = rawData };
      return rtn;
    }
    public async System.Threading.Tasks.Task<IRestCommandResult<T>> ProcessRestTaskCommand<T>(IRestCommand command) where T : new()
    {
      var cmd = (Command)command;
      var result = await cmd.Client.SendAsync();
      T data = default;
      System.String resultText = result.ToRawText();
      if (!(System.String.IsNullOrWhiteSpace(resultText)))
      {
        resultText = resultText.ToJsonElement().ToRawText();
        if (!(System.String.IsNullOrWhiteSpace(resultText)))
          data = System.Text.Json.JsonSerializer.Deserialize<T>(resultText, SoftmakeAll.SDK.Asterisk.ARI.Serializations.JsonSerializerOptions);
      }
      var rtn = new CommandResult<T> { StatusCode = cmd.Client.StatusCode, Data = data };
      return rtn;
    }
    public async System.Threading.Tasks.Task<IRestCommandResult> ProcessRestTaskCommand(IRestCommand command)
    {
      var cmd = (Command)command;
      var result = await cmd.Client.SendAsync();

      System.Byte[] rawData = null;
      System.String resultText = result.ToRawText();
      if (!(System.String.IsNullOrWhiteSpace(resultText)))
      {
        resultText = resultText.ToJsonElement().ToRawText();
        if (!(System.String.IsNullOrWhiteSpace(resultText)))
          rawData = System.Text.Encoding.UTF8.GetBytes(resultText);
      }
      var rtn = new CommandResult { StatusCode = cmd.Client.StatusCode, RawData = rawData };
      return rtn;
    }
    #endregion
  }
}