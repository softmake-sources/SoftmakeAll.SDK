using System.Linq;

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
    internal static void Validate(System.String ConnectionString)
    {
      if ((System.String.IsNullOrWhiteSpace(ConnectionString)) && (System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.CloudStorage.Azure.Environment._ConnectionString)))
        throw new System.Exception("Call SoftmakeAll.SDK.CloudStorage.Azure.Environment.Configure(...) to configure the SDK.");
    }
    internal static System.String GetConnectionStringPropertyValue(System.String ConnectionString, System.String PropertyName)
    {
      if ((System.String.IsNullOrWhiteSpace(ConnectionString)) || (System.String.IsNullOrWhiteSpace(PropertyName)))
        return null;

      System.String[] Properties = ConnectionString.Split(';');
      if ((Properties == null) || (!(Properties.Any())))
        return null;

      System.String Value = Properties.FirstOrDefault(p => p.StartsWith($"{PropertyName}="));
      if (System.String.IsNullOrWhiteSpace(Value))
        return null;

      return Value[(PropertyName.Length + 1)..^0];
    }
    #endregion
  }
}