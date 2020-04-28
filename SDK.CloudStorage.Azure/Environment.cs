namespace SoftmakeAll.SDK.CloudStorage.Azure
{
  public static class Environment
  {
    #region Fields
    internal static System.String _ConnectionString;
    #endregion

    #region Methods
    public static void Configure(System.String ConnectionString)
    {
      if (System.String.IsNullOrWhiteSpace(ConnectionString))
        throw new System.Exception(SoftmakeAll.SDK.Environment.NullConnectionString);

      SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString = ConnectionString.Trim();
    }
    internal static void Validate()
    {
      if (System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString))
        throw new System.Exception("Call SoftmakeAll.SDK.CloudStorage.Azure.Environment.Configure(...) to configure the SDK.");
    }
    #endregion
  }
}