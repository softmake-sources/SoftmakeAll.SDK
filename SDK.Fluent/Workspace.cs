namespace SoftmakeAll.SDK.Fluent
{
  public static class Workspace
  {
    #region Fields
    internal static System.String _ConnectionString;
    internal static System.String DefaultEndpointsProtocol;
    internal static System.String InstanceEndpointName;
    internal static System.String InstanceEndpoint;
    internal static System.String AccountName;
    internal static System.String AccountKey;
    #endregion

    #region Methods
    public static void Configure(System.String ConnectionString)
    {
      if (System.String.IsNullOrWhiteSpace(ConnectionString))
        throw new System.Exception(SoftmakeAll.SDK.Environment.NullConnectionString);

      SoftmakeAll.SDK.Fluent.Workspace._ConnectionString = ConnectionString.Trim();

      SoftmakeAll.SDK.Fluent.Workspace.DefaultEndpointsProtocol = "";
      SoftmakeAll.SDK.Fluent.Workspace.InstanceEndpointName = "";
      SoftmakeAll.SDK.Fluent.Workspace.InstanceEndpoint = "";
      SoftmakeAll.SDK.Fluent.Workspace.AccountName = "";
      SoftmakeAll.SDK.Fluent.Workspace.AccountKey = "";
    }
    internal static void Validate()
    {
      if (System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.Fluent.Workspace._ConnectionString))
        throw new System.Exception("Call SoftmakeAll.SDK.Fluent.Workspace.Configure(...) to configure the SDK.");
    }
    #endregion
  }
}