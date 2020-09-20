namespace SoftmakeAll.SDK.Asterisk.ARI.Middleware
{
  public enum HttpMethod
  {
    GET,
    POST,
    PUT,
    DELETE,
    HEAD,
    OPTIONS,
    PATCH,
    MERGE
  }

  public interface IActionConsumer
  {
    #region Methods
    public IRestCommand GetRestCommand(HttpMethod method, string path);
    public IRestCommandResult<T> ProcessRestCommand<T>(IRestCommand command) where T : new();
    public IRestCommandResult ProcessRestCommand(IRestCommand command);
    public System.Threading.Tasks.Task<IRestCommandResult<T>> ProcessRestTaskCommand<T>(IRestCommand command) where T : new();
    public System.Threading.Tasks.Task<IRestCommandResult> ProcessRestTaskCommand(IRestCommand command);
    #endregion
  }
}