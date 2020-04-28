namespace SoftmakeAll.SDK.Libraries
{
  public static class Environment
  {
    #region Methods
    public static SoftmakeAll.SDK.DataAccess.ConnectorBase GetDatabaseInstance(Microsoft.AspNetCore.Http.HttpRequest HttpRequest)
    {
      if (HttpRequest == null)
        return null;

      System.String AppName = HttpRequest.Headers["AppName"];
      if (System.String.IsNullOrWhiteSpace(AppName))
        return null;

      System.String APIToken = HttpRequest.Headers["APIToken"];
      if (System.String.IsNullOrWhiteSpace(APIToken))
        return null;

      SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstance = new SoftmakeAll.SDK.DataAccess.SQLServer.Connector();
      DatabaseInstance.SessionContextVariables.Clear();
      DatabaseInstance.SessionContextVariables.Add("AppName", AppName);
      DatabaseInstance.SessionContextVariables.Add("APIToken", APIToken);
      return DatabaseInstance;
    }
    #endregion
  }
}