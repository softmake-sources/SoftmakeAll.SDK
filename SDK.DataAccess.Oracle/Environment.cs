﻿namespace SoftmakeAll.SDK.DataAccess.Oracle
{
  public static class Environment
  {
    #region Fields
    internal static System.String _ConnectionString;
    #endregion

    #region Properties
    public static System.Int32 CommandsTimeout { get; set; }
    #endregion

    #region Methods
    public static void Configure(System.String ConnectionString, System.Int32 CommandsTimeout) { SoftmakeAll.SDK.DataAccess.Oracle.Environment.CommandsTimeout = CommandsTimeout; SoftmakeAll.SDK.DataAccess.Oracle.Environment.Configure(ConnectionString); }
    public static void Configure(System.String ConnectionString)
    {
      if (System.String.IsNullOrWhiteSpace(ConnectionString))
        throw new System.Exception(SoftmakeAll.SDK.Environment.NullConnectionString);

      SoftmakeAll.SDK.DataAccess.Oracle.Environment._ConnectionString = ConnectionString.Trim();

      if (SoftmakeAll.SDK.DataAccess.Oracle.Environment.CommandsTimeout == 0)
        SoftmakeAll.SDK.DataAccess.Oracle.Environment.CommandsTimeout = 30;
    }
    #endregion
  }
}