using System.Linq;

namespace SoftmakeAll.SDK.DataAccess
{
  public abstract class ConnectorBase
  {
    #region Constructors
    public ConnectorBase()
    {
      this.SessionContextVariables = SoftmakeAll.SDK.DataAccess.Environment.ApplicationSessionContextVariables;
      if (this.SessionContextVariables == null)
        this.SessionContextVariables = new System.Collections.Generic.Dictionary<System.String, System.String>();
    }
    public ConnectorBase(System.Collections.Generic.Dictionary<System.String, System.String> SessionContextVariables)
    {
      this.SessionContextVariables = SessionContextVariables;
      if (this.SessionContextVariables == null)
      {
        this.SessionContextVariables = SoftmakeAll.SDK.DataAccess.Environment.ApplicationSessionContextVariables;
        if (this.SessionContextVariables == null)
          this.SessionContextVariables = new System.Collections.Generic.Dictionary<System.String, System.String>();
      }
    }
    public ConnectorBase(System.Security.Claims.ClaimsPrincipal ApplicationUser)
    {
      this.ApplicationUser = ApplicationUser;
      this.SessionContextVariables = SoftmakeAll.SDK.DataAccess.Environment.ApplicationSessionContextVariables;
      if (this.SessionContextVariables == null)
        this.SessionContextVariables = new System.Collections.Generic.Dictionary<System.String, System.String>();
      this.FillSessionContextVariables(SoftmakeAll.SDK.DataAccess.Environment.ClaimsToSessionContextVariables);
    }
    #endregion

    #region Subclasses
    public abstract class ConnectorObjectsBase
    {
      #region Constructor
      public ConnectorObjectsBase(System.String ConnectionString, ref System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType, System.Boolean ExecuteForJSON, System.Boolean ShowPlan, System.Boolean ReadSummaries)
      {
        if (System.String.IsNullOrWhiteSpace(ConnectionString))
          throw new System.Exception(SoftmakeAll.SDK.Environment.NullConnectionString);

        if (System.String.IsNullOrWhiteSpace(ProcedureNameOrCommandText))
          throw new System.Exception(SoftmakeAll.SDK.Environment.NullProcedureNameOrCommandText);
      }
      #endregion
    }
    #endregion

    #region Constants
    protected const System.String ErrorOnSetBaseCommands = "Could not set the ARITHABORT, XACT_ABORT and NOCOUNT on database. Details: {0}";
    protected const System.String ErrorOnSetSessionContext = "Could not set the SessionContext variables. Details: {0}";
    protected const System.String ErrorOnCallProcedure = "Error on call procedure '{0}': {1}";
    protected const System.String ResultSetsNotExpected = "The returned result sets are not expected. Command: {0}";
    protected const System.String EmptyResultSets = "The returned result set is empty. Command: {0}";
    protected const System.String NoInformationReceived = "No information was received from the Database Server. Command: {0}";
    protected const System.String ExecutionError = "The command could not be executed. Details: {0} Command: {1}";
    protected const System.String ConnectionError = "Could not connect to the Database Server. Details: {0}";
    #endregion

    #region Properties
    public System.Collections.Generic.Dictionary<System.String, System.String> SessionContextVariables { get; }
    public System.Security.Claims.ClaimsPrincipal ApplicationUser { get; }

    public System.String Server { get; set; }
    public System.Int32 Port { get; set; }
    public System.String UserID { get; set; }
    public System.String Password { get; set; }
    public System.Boolean IntegratedSecurity { get; set; }
    public System.String Database { get; set; }
    public System.String ConnectionString { get; set; }
    public System.Boolean ShowPlan { get; set; }

    private System.Nullable<System.Boolean> _ReadSummaries = null;
    public System.Nullable<System.Boolean> ReadSummaries
    {
      get
      {
        return this._ReadSummaries ?? SoftmakeAll.SDK.DataAccess.Environment.ReadSummaries;
      }
      set
      {
        this._ReadSummaries = value;
      }
    }

    private System.String _SystemEventsProcedureSchemaName = null;
    public System.String SystemEventsProcedureSchemaName
    {
      get
      {
        return System.String.IsNullOrWhiteSpace(this._SystemEventsProcedureSchemaName) ? SoftmakeAll.SDK.DataAccess.Environment.SystemEventsProcedureSchemaName : "";
      }
      set
      {
        this._SystemEventsProcedureSchemaName = value;
      }
    }
    #endregion

    #region Methods
    #region Helpers
    public abstract System.String BuildConnectionString();
    private void FillSessionContextVariables(System.Collections.Generic.List<System.String> ClaimsToSessionContextVariables)
    {
      this.SessionContextVariables.Clear();
      if ((this.ApplicationUser == null) || (!(this.ApplicationUser.Claims.Any())) || (ClaimsToSessionContextVariables == null) || (!(ClaimsToSessionContextVariables.Any())))
        return;

      foreach (System.String ClaimsToSessionContextVariable in ClaimsToSessionContextVariables)
      {
        System.String Value = this.ApplicationUser.FindFirst(ClaimsToSessionContextVariable)?.Value;
        if (!(System.String.IsNullOrWhiteSpace(Value)))
          this.SessionContextVariables.Add(ClaimsToSessionContextVariable, Value);
      }
    }
    #endregion

    #region Parameters
    public System.Data.Common.DbParameter CreateInputParameter(System.String Name, System.Int32 Type, System.Object Value) { return this.CreateInputParameter(Name, Type, 0, Value); }
    public System.Data.Common.DbParameter CreateInputParameter(System.String Name, System.Int32 Type, System.Int32 Size, System.Object Value) { return this.CreateParameter(Name, Type, Size, Value, System.Data.ParameterDirection.Input); }

    public System.Data.Common.DbParameter CreateOutputParameter(System.String Name, System.Int32 Type) { return this.CreateOutputParameter(Name, Type, null); }
    public System.Data.Common.DbParameter CreateOutputParameter(System.String Name, System.Int32 Type, System.Object Value) { return this.CreateOutputParameter(Name, Type, 0, Value); }
    public System.Data.Common.DbParameter CreateOutputParameter(System.String Name, System.Int32 Type, System.Int32 Size, System.Object Value) { return this.CreateParameter(Name, Type, Size, Value, System.Data.ParameterDirection.Output); }

    protected abstract System.Data.Common.DbParameter CreateParameter(System.String Name, System.Int32 Type, System.Int32 Size, System.Object Value, System.Data.ParameterDirection Direction);
    #endregion

    #region Command Execution
    #region Syncronous Command Execution
    public SoftmakeAll.SDK.OperationResult<System.Data.DataSet> ExecuteText(System.String Text) { return this.ExecuteText(Text, null); }
    public SoftmakeAll.SDK.OperationResult<System.Data.DataSet> ExecuteText(System.String Text, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters) { return this.ExecuteCommand(Text, Parameters, System.Data.CommandType.Text); }
    public SoftmakeAll.SDK.OperationResult<System.Data.DataSet> ExecuteProcedure(System.String Name) { return this.ExecuteProcedure(Name, null); }
    public SoftmakeAll.SDK.OperationResult<System.Data.DataSet> ExecuteProcedure(System.String Name, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters) { return this.ExecuteCommand(Name, Parameters, System.Data.CommandType.StoredProcedure); }
    protected abstract SoftmakeAll.SDK.OperationResult<System.Data.DataSet> ExecuteCommand(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType);

    public SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> ExecuteTextForJSON(System.String Text) { return this.ExecuteTextForJSON(Text, null); }
    public SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> ExecuteTextForJSON(System.String Text, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters) { return this.ExecuteCommandForJSON(Text, Parameters, System.Data.CommandType.Text); }
    public SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> ExecuteProcedureForJSON(System.String Name) { return this.ExecuteProcedureForJSON(Name, null); }
    public SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> ExecuteProcedureForJSON(System.String Name, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters) { return this.ExecuteCommandForJSON(Name, Parameters, System.Data.CommandType.StoredProcedure); }
    protected virtual SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> ExecuteCommandForJSON(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType)
    {
      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> Result = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();

      SoftmakeAll.SDK.OperationResult<System.Data.DataSet> DatabaseOperationResult = this.ExecuteCommand(ProcedureNameOrCommandText, Parameters, CommandType);
      Result.Count = DatabaseOperationResult.Count;
      Result.ID = DatabaseOperationResult.ID;
      Result.Message = DatabaseOperationResult.Message;
      Result.ExitCode = DatabaseOperationResult.ExitCode;
      if (DatabaseOperationResult.ExitCode != 0)
        return Result;

      Result.Data = SoftmakeAll.SDK.DataAccess.DatabaseValues.DataSetToJSON(DatabaseOperationResult.Data);

      return Result;
    }
    #endregion

    #region Asyncronous Command Execution
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Data.DataSet>> ExecuteTextAsync(System.String Text) { return await this.ExecuteTextAsync(Text, null); }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Data.DataSet>> ExecuteTextAsync(System.String Text, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters) { return await this.ExecuteCommandAsync(Text, Parameters, System.Data.CommandType.Text); }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Data.DataSet>> ExecuteProcedureAsync(System.String Name) { return await this.ExecuteProcedureAsync(Name, null); }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Data.DataSet>> ExecuteProcedureAsync(System.String Name, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters) { return await this.ExecuteCommandAsync(Name, Parameters, System.Data.CommandType.StoredProcedure); }
    protected abstract System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Data.DataSet>> ExecuteCommandAsync(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType);

    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> ExecuteTextForJSONAsync(System.String Text) { return await this.ExecuteTextForJSONAsync(Text, null); }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> ExecuteTextForJSONAsync(System.String Text, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters) { return await this.ExecuteCommandForJSONAsync(Text, Parameters, System.Data.CommandType.Text); }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> ExecuteProcedureForJSONAsync(System.String Name) { return await this.ExecuteProcedureForJSONAsync(Name, null); }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> ExecuteProcedureForJSONAsync(System.String Name, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters) { return await this.ExecuteCommandForJSONAsync(Name, Parameters, System.Data.CommandType.StoredProcedure); }
    protected virtual async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> ExecuteCommandForJSONAsync(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType)
    {
      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> Result = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();

      SoftmakeAll.SDK.OperationResult<System.Data.DataSet> DatabaseOperationResult = await this.ExecuteCommandAsync(ProcedureNameOrCommandText, Parameters, CommandType);
      Result.Count = DatabaseOperationResult.Count;
      Result.ID = DatabaseOperationResult.ID;
      Result.Message = DatabaseOperationResult.Message;
      Result.ExitCode = DatabaseOperationResult.ExitCode;
      if (DatabaseOperationResult.ExitCode != 0)
        return Result;

      Result.Data = SoftmakeAll.SDK.DataAccess.DatabaseValues.DataSetToJSON(DatabaseOperationResult.Data);

      return Result;
    }
    #endregion
    #endregion

    #region System Events (Log)
    #region Syncronous System Events (Log)
    public void WriteApplicationDebugEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("A", "D", ProcedureName, Description); }
    public void WriteApplicationInformationEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("A", "I", ProcedureName, Description); }
    public void WriteApplicationWarningEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("A", "W", ProcedureName, Description); }
    public void WriteApplicationErrorEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("A", "E", ProcedureName, Description); }
    public void WriteDatabaseDebugEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("D", "D", ProcedureName, Description); }
    public void WriteDatabaseInformationEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("D", "I", ProcedureName, Description); }
    public void WriteDatabaseWarningEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("D", "W", ProcedureName, Description); }
    public void WriteDatabaseErrorEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("D", "E", ProcedureName, Description); }
    public void WriteFunctionsDebugEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("F", "D", ProcedureName, Description); }
    public void WriteFunctionsInformationEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("F", "I", ProcedureName, Description); }
    public void WriteFunctionsWarningEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("F", "W", ProcedureName, Description); }
    public void WriteFunctionsErrorEvent(System.String ProcedureName, System.String Description) { this.WriteEvent("F", "E", ProcedureName, Description); }
    protected abstract void WriteEvent(System.String Source, System.String Type, System.String ProcedureName, System.String Description);
    protected virtual void WriteErrorFile(SoftmakeAll.SDK.OperationResult OperationResult)
    {
      if (OperationResult == null)
        return;

      try
      {
        System.String FileContents = System.String.Format("{0:dd/MM/yyyy HH:mm:ss}: {1} -> {2}{3}", System.DateTimeOffset.Now, OperationResult.ExitCode, OperationResult.Message, System.Environment.NewLine);
        System.IO.File.AppendAllText(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "SoftmakeAll_DataAccess.log"), FileContents);
      }
      catch { }
    }
    #endregion

    #region Asyncronous System Events (Log)
    public async System.Threading.Tasks.Task WriteApplicationDebugEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("A", "D", ProcedureName, Description); }
    public async System.Threading.Tasks.Task WriteApplicationInformationEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("A", "I", ProcedureName, Description); }
    public async System.Threading.Tasks.Task WriteApplicationWarningEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("A", "W", ProcedureName, Description); }
    public async System.Threading.Tasks.Task WriteApplicationErrorEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("A", "E", ProcedureName, Description); }
    public async System.Threading.Tasks.Task WriteDatabaseDebugEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("D", "D", ProcedureName, Description); }
    public async System.Threading.Tasks.Task WriteDatabaseInformationEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("D", "I", ProcedureName, Description); }
    public async System.Threading.Tasks.Task WriteDatabaseWarningEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("D", "W", ProcedureName, Description); }
    public async System.Threading.Tasks.Task WriteDatabaseErrorEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("D", "E", ProcedureName, Description); }
    public async System.Threading.Tasks.Task WriteFunctionsDebugEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("F", "D", ProcedureName, Description); }
    public async System.Threading.Tasks.Task WriteFunctionsInformationEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("F", "I", ProcedureName, Description); }
    public async System.Threading.Tasks.Task WriteFunctionsWarningEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("F", "W", ProcedureName, Description); }
    public async System.Threading.Tasks.Task WriteFunctionsErrorEventAsync(System.String ProcedureName, System.String Description) { await this.WriteEventAsync("F", "E", ProcedureName, Description); }
    protected abstract System.Threading.Tasks.Task WriteEventAsync(System.String Source, System.String Type, System.String ProcedureName, System.String Description);
    protected virtual async System.Threading.Tasks.Task WriteErrorFileAsync(SoftmakeAll.SDK.OperationResult OperationResult)
    {
      if (OperationResult == null)
        return;

      try
      {
        System.String FileContents = System.String.Format("{0:dd/MM/yyyy HH:mm:ss}: {1} -> {2}{3}", System.DateTimeOffset.Now, OperationResult.ExitCode, OperationResult.Message, System.Environment.NewLine);
        await System.IO.File.AppendAllTextAsync(System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "SoftmakeAll_DataAccess.log"), FileContents);
      }
      catch { }
    }
    #endregion

    #endregion
    #endregion
  }
}