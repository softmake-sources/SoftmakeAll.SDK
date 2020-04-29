﻿using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.DataAccess.Oracle
{
  public sealed class Connector : SoftmakeAll.SDK.DataAccess.ConnectorBase
  {
    #region Constructors
    public Connector() : base() { }
    public Connector(System.Collections.Generic.Dictionary<System.String, System.String> SessionContextVariables) : base(SessionContextVariables) { }
    public Connector(System.Security.Claims.ClaimsPrincipal ApplicationUser) : base(ApplicationUser) { }
    #endregion

    #region Subclasses
    private class ConnectorObjects : SoftmakeAll.SDK.DataAccess.ConnectorBase.ConnectorObjectsBase
    {
      #region Constructor
      public ConnectorObjects(System.String ConnectionString, System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType, System.Boolean ExecuteForJSON, System.Boolean ShowPlan, System.Boolean ReadSummaries) : base(ConnectionString, ref ProcedureNameOrCommandText, Parameters, CommandType, ExecuteForJSON, ShowPlan, ReadSummaries)
      {
        this.Connection = new global::Oracle.ManagedDataAccess.Client.OracleConnection(ConnectionString);
        this.Command = new global::Oracle.ManagedDataAccess.Client.OracleCommand(ProcedureNameOrCommandText, this.Connection)
        {
          CommandTimeout = SoftmakeAll.SDK.DataAccess.Oracle.Environment.CommandsTimeout,
          CommandType = CommandType,
        };

        System.Boolean HasShowResultsTableParameter = false;
        if ((Parameters != null) && (Parameters.Any()))
          foreach (System.Data.Common.DbParameter Parameter in Parameters)
          {
            if (Parameter.ParameterName == "ShowResultsTable")
              HasShowResultsTableParameter = true;

            Parameter.Value = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetCorrectParameterValue(Parameter.Value);
            this.Command.Parameters.Add(Parameter);
          }

        if (!(ReadSummaries))
          return;

        if ((CommandType == System.Data.CommandType.StoredProcedure) && (!(HasShowResultsTableParameter)))
          this.Command.Parameters.Add(new global::Oracle.ManagedDataAccess.Client.OracleParameter
          {
            Direction = System.Data.ParameterDirection.Input,
            ParameterName = "ShowResultsTable",
            OracleDbType = global::Oracle.ManagedDataAccess.Client.OracleDbType.Int32,
            Size = 0,
            Value = true
          });
      }
      #endregion

      #region Properties
      public global::Oracle.ManagedDataAccess.Client.OracleConnection Connection { get; }
      public global::Oracle.ManagedDataAccess.Client.OracleCommand Command { get; }
      #endregion

      #region Fields
      private const System.String Summaries = "SELECT NULL AS \"ID\", NULL AS \"Message\", 0 AS \"ExitCode\" FROM DUAL";
      protected override System.String SummariesSQL => Summaries + ";";
      protected override System.String SummariesSQLAsJSON => SummariesSQL;
      #endregion
    }
    #endregion

    #region Methods
    #region Helpers
    public override System.String BuildConnectionString()
    {
      if (System.String.IsNullOrWhiteSpace(base.Server))
        return "";

      if ((!(base.IntegratedSecurity)) && ((System.String.IsNullOrWhiteSpace(base.UserID)) || (System.String.IsNullOrWhiteSpace(base.Password))))
        return "";

      return $"Data Source={base.Server}; {(base.IntegratedSecurity ? "Integrated Security=yes;" : $"User Id={base.UserID}; Password={base.Password}; ")}";
    }
    #endregion

    #region Parameters
    protected override System.Data.Common.DbParameter CreateParameter(System.String Name, System.Int32 Type, System.Int32 Size, System.Object Value, System.Data.ParameterDirection Direction)
    {
      return new global::Oracle.ManagedDataAccess.Client.OracleParameter
      {
        ParameterName = Name,
        OracleDbType = (global::Oracle.ManagedDataAccess.Client.OracleDbType)Type,
        Direction = Direction,
        Size = Size,
        Value = Value
      };
    }
    #endregion

    #region PreCommands
    private void ExecutePreCommands(SoftmakeAll.SDK.DataAccess.Oracle.Connector.ConnectorObjects ConnectorObjects)
    {
      if (ConnectorObjects == null)
        return;

      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.Oracle.Oracle.ExecutePreCommands";

      if (!(System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName)))
      {
        global::Oracle.ManagedDataAccess.Client.OracleCommand SessionContextCommand = new global::Oracle.ManagedDataAccess.Client.OracleCommand(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName, ConnectorObjects.Connection);
        try
        {
          SessionContextCommand.ExecuteNonQuery();
        }
        catch (System.Exception ex)
        {
          base.WriteApplicationWarningEvent(ThisProcedureName, System.String.Format(ErrorOnCallProcedure, SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName, ex.Message));
        }
        SessionContextCommand.Dispose();
      }
    }
    private async System.Threading.Tasks.Task ExecutePreCommandsAsync(SoftmakeAll.SDK.DataAccess.Oracle.Connector.ConnectorObjects ConnectorObjects)
    {
      if (ConnectorObjects == null)
        return;

      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.Oracle.Oracle.ExecutePreCommandsAsync";

      if (!(System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName)))
      {
        global::Oracle.ManagedDataAccess.Client.OracleCommand SessionContextCommand = new global::Oracle.ManagedDataAccess.Client.OracleCommand(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName, ConnectorObjects.Connection);
        try
        {
          await SessionContextCommand.ExecuteNonQueryAsync();
        }
        catch (System.Exception ex)
        {
          await base.WriteApplicationWarningEventAsync(ThisProcedureName, System.String.Format(ErrorOnCallProcedure, SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName, ex.Message));
        }
        await SessionContextCommand.DisposeAsync();
      }
    }
    #endregion

    #region Command Execution
    protected override SoftmakeAll.SDK.OperationResult<System.Data.DataSet> ExecuteCommand(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType)
    {
      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.Oracle.Oracle.ExecuteCommand";

      SoftmakeAll.SDK.OperationResult<System.Data.DataSet> Result = new SoftmakeAll.SDK.OperationResult<System.Data.DataSet>();

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.Oracle.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.Oracle.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.Oracle.Connector.ConnectorObjects(ConnectionString, ProcedureNameOrCommandText, Parameters, CommandType, false, base.ShowPlan, base.ReadSummaries.Value);

      try
      {
        ConnectorObjects.Connection.Open();

        this.ExecutePreCommands(ConnectorObjects);

        global::Oracle.ManagedDataAccess.Client.OracleDataAdapter Adapter = null;

        try
        {
          System.Data.DataSet DataSet = new System.Data.DataSet();
          DataSet.EnforceConstraints = false;

          Adapter = new global::Oracle.ManagedDataAccess.Client.OracleDataAdapter(ConnectorObjects.Command);
          base.WriteApplicationDebugEvent(ThisProcedureName, ConnectorObjects.Command.CommandText);
          Adapter.Fill(DataSet);

          if (DataSet.Tables.Count == 0)
          {
            DataSet.Tables.Add(new System.Data.DataTable());
            Result.ID = null;
            if (!(base.ReadSummaries.Value))
            {
              Result.Message = null;
              Result.ExitCode = 0;
            }
            else
            {
              Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.NoInformationReceived, ConnectorObjects.Command.CommandText);
              Result.ExitCode = -4;
              base.WriteApplicationDebugEvent(ThisProcedureName, Result.Message);
            }
          }
          else
          {
            System.Data.DataTable ResultsTable = DataSet.Tables[DataSet.Tables.Count - 1];

            try
            {
              Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt64(ResultsTable.Rows[0]["ID"]);
              Result.Message = System.Convert.ToString(ResultsTable.Rows[0]["Message"]);
              Result.ExitCode = System.Convert.ToInt16(SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt16(ResultsTable.Rows[0]["ExitCode"]));
            }
            catch
            {
              Result.ID = null;
              Result.Message = null;
              Result.ExitCode = 0;
              if ((!(base.ShowPlan)) && (base.ReadSummaries.Value))
              {
                Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ResultSetsNotExpected, ConnectorObjects.Command.CommandText);
                Result.ExitCode = -3;
                base.WriteApplicationDebugEvent(ThisProcedureName, Result.Message);
              }
            }
          }
          Result.Data = DataSet;
        }
        catch (System.Exception ex)
        {
          Result.ExitCode = -5;
          Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ExecutionError, ex.Message, ConnectorObjects.Command.CommandText);
          base.WriteApplicationErrorEvent(ThisProcedureName, Result.Message);
        }

        Adapter?.Dispose();

        ConnectorObjects.Connection.Close();
      }
      catch (System.Exception ex)
      {
        Result.ExitCode = -6;
        Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ConnectionError, ex.Message);
        base.WriteErrorFile(Result);
      }

      ConnectorObjects.Command.Dispose();
      ConnectorObjects.Connection.Dispose();

      return Result;
    }
    protected override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Data.DataSet>> ExecuteCommandAsync(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType)
    {
      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.Oracle.Oracle.ExecuteCommandAsync";

      SoftmakeAll.SDK.OperationResult<System.Data.DataSet> Result = new SoftmakeAll.SDK.OperationResult<System.Data.DataSet>();

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.Oracle.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.Oracle.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.Oracle.Connector.ConnectorObjects(ConnectionString, ProcedureNameOrCommandText, Parameters, CommandType, false, base.ShowPlan, base.ReadSummaries.Value);

      try
      {
        await ConnectorObjects.Connection.OpenAsync();

        await this.ExecutePreCommandsAsync(ConnectorObjects);

        global::Oracle.ManagedDataAccess.Client.OracleDataAdapter Adapter = null;

        await base.WriteApplicationDebugEventAsync(ThisProcedureName, ConnectorObjects.Command.CommandText);

        try
        {
          System.Data.DataSet DataSet = new System.Data.DataSet();
          DataSet.EnforceConstraints = false;

          Adapter = new global::Oracle.ManagedDataAccess.Client.OracleDataAdapter(ConnectorObjects.Command);
          Adapter.Fill(DataSet);

          if (DataSet.Tables.Count == 0)
          {
            DataSet.Tables.Add(new System.Data.DataTable());
            Result.ID = null;
            if (!(base.ReadSummaries.Value))
            {
              Result.Message = null;
              Result.ExitCode = 0;
            }
            else
            {
              Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.NoInformationReceived, ConnectorObjects.Command.CommandText);
              Result.ExitCode = -4;
              await base.WriteApplicationDebugEventAsync(ThisProcedureName, Result.Message);
            }
          }
          else
          {
            System.Data.DataTable ResultsTable = DataSet.Tables[DataSet.Tables.Count - 1];

            try
            {
              Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt64(ResultsTable.Rows[0]["ID"]);
              Result.Message = System.Convert.ToString(ResultsTable.Rows[0]["Message"]);
              Result.ExitCode = System.Convert.ToInt16(SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt16(ResultsTable.Rows[0]["ExitCode"]));
            }
            catch
            {
              Result.ID = null;
              Result.Message = null;
              Result.ExitCode = 0;
              if ((!(base.ShowPlan)) && (base.ReadSummaries.Value))
              {
                Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ResultSetsNotExpected, ConnectorObjects.Command.CommandText);
                Result.ExitCode = -3;
                await base.WriteApplicationDebugEventAsync(ThisProcedureName, Result.Message);
              }
            }
          }
          Result.Data = DataSet;
        }
        catch (System.Exception ex)
        {
          Result.ExitCode = -5;
          Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ExecutionError, ex.Message, ConnectorObjects.Command.CommandText);
          await base.WriteApplicationErrorEventAsync(ThisProcedureName, Result.Message);
        }

        Adapter?.Dispose();

        await ConnectorObjects.Connection.CloseAsync();
      }
      catch (System.Exception ex)
      {
        Result.ExitCode = -6;
        Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ConnectionError, ex.Message);
        await base.WriteErrorFileAsync(Result);
      }

      await ConnectorObjects.Command.DisposeAsync();
      await ConnectorObjects.Connection.DisposeAsync();

      return Result;
    }
    #endregion

    #region System Events (Log)
    protected override void WriteEvent(System.String Source, System.String Type, System.String ProcedureName, System.String Description)
    {
      if (
              ((Type == "D") && (!(DataAccess.Environment.WriteDebugSystemEvents)))
           || ((Type == "I") && (!(DataAccess.Environment.WriteInformationSystemEvents)))
           || ((Type == "W") && (!(DataAccess.Environment.WriteWarningSystemEvents)))
           || ((Type == "E") && (!(DataAccess.Environment.WriteErrorSystemEvents)))
         )
        return;


      switch (Source)
      {
        case "A": { Source = "Application"; break; }
        case "D": { Source = "Database"; break; }
        case "F": { Source = "Functions"; break; }
        default: { return; }
      }

      switch (Type)
      {
        case "D": { Type = "Debug"; break; }
        case "I": { Type = "Information"; break; }
        case "W": { Type = "Warning"; break; }
        case "E": { Type = "Error"; break; }
        default: { return; }
      }

      System.String SystemEventWriteProcedureName = "{0}Write{1}{2}Event";
      SystemEventWriteProcedureName = System.String.Format(SystemEventWriteProcedureName, System.String.IsNullOrWhiteSpace(base.SystemEventsProcedureSchemaName) ? "" : $"{base.SystemEventsProcedureSchemaName}.", Source, Type);

      SoftmakeAll.SDK.OperationResult Result = new SoftmakeAll.SDK.OperationResult();

      System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters = new System.Collections.Generic.List<System.Data.Common.DbParameter>();
      Parameters.Add(this.CreateInputParameter("ProcedureName", (int)global::Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, 261, ProcedureName));
      Parameters.Add(this.CreateInputParameter("Description", (int)global::Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, 0, Description));
      Parameters.Add(this.CreateInputParameter("ShowResultsTable", (int)global::Oracle.ManagedDataAccess.Client.OracleDbType.Int32, true));

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.Oracle.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.Oracle.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.Oracle.Connector.ConnectorObjects(ConnectionString, SystemEventWriteProcedureName, Parameters, System.Data.CommandType.StoredProcedure, false, false, base.ReadSummaries.Value);


      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.Oracle.Oracle.WriteEvent";

      try
      {
        ConnectorObjects.Connection.Open();

        global::Oracle.ManagedDataAccess.Client.OracleDataAdapter Adapter = null;

        try
        {
          System.Data.DataTable ResultsTable = new System.Data.DataTable();

          Adapter = new global::Oracle.ManagedDataAccess.Client.OracleDataAdapter(ConnectorObjects.Command);
          Adapter.Fill(ResultsTable);

          if (!(base.ReadSummaries.Value))
          {
            Result.ID = null;
            Result.Message = null;
            Result.ExitCode = 0;
          }
          else
          {
            Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt64(ResultsTable.Rows[0]["ID"]);
            Result.Message = System.Convert.ToString(ResultsTable.Rows[0]["Message"]);
            Result.ExitCode = System.Convert.ToInt16(SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt16(ResultsTable.Rows[0]["ExitCode"]));

            if (Result.ExitCode == 2) // Error when writing the system event
              base.WriteErrorFile(Result);
          }
        }
        catch (System.Exception ex)
        {
          Result.ExitCode = -5;
          Result.Message = System.String.Concat(ThisProcedureName, " -> ", System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ExecutionError, ex.Message, ConnectorObjects.Command.CommandText));
          base.WriteErrorFile(Result);
        }

        Adapter?.Dispose();

        ConnectorObjects.Connection.Close();
      }
      catch (System.Exception ex)
      {
        Result.ExitCode = -6;
        Result.Message = System.String.Concat(ThisProcedureName, " -> ", System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ConnectionError, ex.Message));
        base.WriteErrorFile(Result);
      }

      ConnectorObjects.Command.Dispose();
      ConnectorObjects.Connection.Dispose();
    }
    protected override async System.Threading.Tasks.Task WriteEventAsync(System.String Source, System.String Type, System.String ProcedureName, System.String Description)
    {
      if (
              ((Type == "D") && (!(DataAccess.Environment.WriteDebugSystemEvents)))
           || ((Type == "I") && (!(DataAccess.Environment.WriteInformationSystemEvents)))
           || ((Type == "W") && (!(DataAccess.Environment.WriteWarningSystemEvents)))
           || ((Type == "E") && (!(DataAccess.Environment.WriteErrorSystemEvents)))
         )
        return;


      switch (Source)
      {
        case "A": { Source = "Application"; break; }
        case "D": { Source = "Database"; break; }
        case "F": { Source = "Functions"; break; }
        default: { return; }
      }

      switch (Type)
      {
        case "D": { Type = "Debug"; break; }
        case "I": { Type = "Information"; break; }
        case "W": { Type = "Warning"; break; }
        case "E": { Type = "Error"; break; }
        default: { return; }
      }

      System.String SystemEventWriteProcedureName = "{0}Write{1}{2}Event";
      SystemEventWriteProcedureName = System.String.Format(SystemEventWriteProcedureName, System.String.IsNullOrWhiteSpace(base.SystemEventsProcedureSchemaName) ? "" : $"{base.SystemEventsProcedureSchemaName}.", Source, Type);

      SoftmakeAll.SDK.OperationResult Result = new SoftmakeAll.SDK.OperationResult();

      System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters = new System.Collections.Generic.List<System.Data.Common.DbParameter>();
      Parameters.Add(this.CreateInputParameter("ProcedureName", (int)global::Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, 261, ProcedureName));
      Parameters.Add(this.CreateInputParameter("Description", (int)global::Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, 0, Description));
      Parameters.Add(this.CreateInputParameter("ShowResultsTable", (int)global::Oracle.ManagedDataAccess.Client.OracleDbType.Int32, true));

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.Oracle.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.Oracle.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.Oracle.Connector.ConnectorObjects(ConnectionString, SystemEventWriteProcedureName, Parameters, System.Data.CommandType.StoredProcedure, false, false, base.ReadSummaries.Value);


      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.Oracle.Oracle.WriteEventAsync";

      try
      {
        await ConnectorObjects.Connection.OpenAsync();

        global::Oracle.ManagedDataAccess.Client.OracleDataAdapter Adapter = null;

        try
        {
          System.Data.DataTable ResultsTable = new System.Data.DataTable();

          Adapter = new global::Oracle.ManagedDataAccess.Client.OracleDataAdapter(ConnectorObjects.Command);
          Adapter.Fill(ResultsTable);

          if (!(base.ReadSummaries.Value))
          {
            Result.ID = null;
            Result.Message = null;
            Result.ExitCode = 0;
          }
          else
          {
            Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt64(ResultsTable.Rows[0]["ID"]);
            Result.Message = System.Convert.ToString(ResultsTable.Rows[0]["Message"]);
            Result.ExitCode = System.Convert.ToInt16(SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt16(ResultsTable.Rows[0]["ExitCode"]));

            if (Result.ExitCode == 2) // Error when writing the system event
              await base.WriteErrorFileAsync(Result);
          }
        }
        catch (System.Exception ex)
        {
          Result.ExitCode = -5;
          Result.Message = System.String.Concat(ThisProcedureName, " -> ", System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ExecutionError, ex.Message, ConnectorObjects.Command.CommandText));
          await base.WriteErrorFileAsync(Result);
        }

        Adapter?.Dispose();

        await ConnectorObjects.Connection.CloseAsync();
      }
      catch (System.Exception ex)
      {
        Result.ExitCode = -6;
        Result.Message = System.String.Concat(ThisProcedureName, " -> ", System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ConnectionError, ex.Message));
        await base.WriteErrorFileAsync(Result);
      }

      await ConnectorObjects.Command.DisposeAsync();
      await ConnectorObjects.Connection.DisposeAsync();
    }
    #endregion
    #endregion
  }
}