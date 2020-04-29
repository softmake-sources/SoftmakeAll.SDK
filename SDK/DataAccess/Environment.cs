namespace SoftmakeAll.SDK.DataAccess
{
  public static class Environment
  {
    #region Properties
    public static System.Collections.Generic.Dictionary<System.String, System.String> ApplicationSessionContextVariables { get; set; }
    public static System.Collections.Generic.List<System.String> ClaimsToSessionContextVariables { get; set; }
    public static System.String DefineSessionContextProcedureName { get; set; }
    public static System.Boolean WriteDebugSystemEvents { get; set; }
    public static System.Boolean WriteInformationSystemEvents { get; set; }
    public static System.Boolean WriteWarningSystemEvents { get; set; }
    public static System.Boolean WriteErrorSystemEvents { get; set; }
    public static System.Boolean ReadSummaries { get; set; }
    public static System.String SystemEventsProcedureSchemaName { get; set; }
    #endregion
  }
}