using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.DataAccess.PostgreSQL
{
  public sealed class Connector : SoftmakeAll.SDK.DataAccess.ConnectorBase
  {
    #region Constructors
    public Connector() : base() { }
    public Connector(System.Collections.Generic.Dictionary<System.String, System.String> SessionContextVariables) : base(SessionContextVariables) { }
    public Connector(System.Security.Claims.ClaimsPrincipal ApplicationUser) : base(ApplicationUser) { }
    #endregion

    #region Subclasses
    internal class ConnectorObjects : SoftmakeAll.SDK.DataAccess.ConnectorBase.ConnectorObjectsBase
    {
      #region Constructor
      public ConnectorObjects(System.String ConnectionString, System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType, System.Boolean ExecuteForJSON, System.Boolean ShowPlan, System.Boolean ReadSummaries) : base(ConnectionString, ref ProcedureNameOrCommandText, Parameters, CommandType, ExecuteForJSON, ShowPlan, ReadSummaries)
      {
        this.Connection = new Npgsql.NpgsqlConnection(ConnectionString);
        this.Command = new Npgsql.NpgsqlCommand(ProcedureNameOrCommandText, this.Connection)
        {
          CommandTimeout = SoftmakeAll.SDK.DataAccess.PostgreSQL.Environment.CommandsTimeout,
          CommandType = CommandType,
        };

        System.Boolean HasShowResultsTableParameter = false;
        if ((Parameters != null) && (Parameters.Any()))
          foreach (System.Data.Common.DbParameter Parameter in Parameters)
          {
            if (Parameter.ParameterName == "ShowResultsTable")
              HasShowResultsTableParameter = true;

            Parameter.Value = Parameter.Value ?? System.Convert.DBNull;
            this.Command.Parameters.Add(Parameter);
          }

        if (!(ReadSummaries))
          return;

        if ((CommandType == System.Data.CommandType.StoredProcedure) && (!(HasShowResultsTableParameter)))
          this.Command.Parameters.Add(new Npgsql.NpgsqlParameter
          {
            Direction = System.Data.ParameterDirection.Input,
            ParameterName = "ShowResultsTable",
            NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bit,
            Size = 0,
            Value = true
          });
      }
      #endregion

      #region Properties
      public Npgsql.NpgsqlConnection Connection { get; }
      public Npgsql.NpgsqlCommand Command { get; }
      #endregion
    }
    #endregion

    #region Methods
    #region Helpers
    public override System.String BuildConnectionString()
    {
      if (base.Port == 0) base.Port = 5432;

      if (System.String.IsNullOrWhiteSpace(base.Server))
        return "";

      if (System.String.IsNullOrWhiteSpace(base.Database))
        return "";

      if ((!(base.IntegratedSecurity)) && ((System.String.IsNullOrWhiteSpace(base.UserID)) || (System.String.IsNullOrWhiteSpace(base.Password))))
        return "";

      return $"Server={base.Server}; Port={base.Port}; Database={base.Database}; {(base.IntegratedSecurity ? "Integrated Security=true;" : $"User Id={base.UserID}; Password={base.Password};")}";
    }
    #endregion

    #region Parameters
    protected override System.Data.Common.DbParameter CreateParameter(System.String Name, System.Int32 Type, System.Int32 Size, System.Object Value, System.Data.ParameterDirection Direction)
    {
      return new Npgsql.NpgsqlParameter
      {
        ParameterName = Name,
        NpgsqlDbType = (NpgsqlTypes.NpgsqlDbType)Type,
        Direction = Direction,
        Size = Size,
        Value = Value
      };
    }
    #endregion

    #region PreCommands
    private void ExecutePreCommands(SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ConnectorObjects ConnectorObjects)
    {
      if (ConnectorObjects == null)
        return;

      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ExecutePreCommands";


      if (!(System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName)))
      {
        Npgsql.NpgsqlCommand SessionContextCommand = new Npgsql.NpgsqlCommand(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName, ConnectorObjects.Connection);
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
    private async System.Threading.Tasks.Task ExecutePreCommandsAsync(SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ConnectorObjects ConnectorObjects)
    {
      if (ConnectorObjects == null)
        return;

      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ExecutePreCommandsAsync";


      if (!(System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName)))
      {
        Npgsql.NpgsqlCommand SessionContextCommand = new Npgsql.NpgsqlCommand(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName, ConnectorObjects.Connection);
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
    protected override SoftmakeAll.SDK.OperationResult<System.Data.DataSet> ExecuteCommand(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType, System.Int32 Timeout)
    {
      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ExecuteCommand";

      SoftmakeAll.SDK.OperationResult<System.Data.DataSet> Result = new SoftmakeAll.SDK.OperationResult<System.Data.DataSet>();

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.PostgreSQL.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ConnectorObjects(ConnectionString, ProcedureNameOrCommandText, Parameters, CommandType, false, base.ShowPlan, base.ReadSummaries.Value);

      try
      {
        ConnectorObjects.Connection.Open();

        this.ExecutePreCommands(ConnectorObjects);

        Npgsql.NpgsqlDataAdapter Adapter = null;

        try
        {
          System.Data.DataSet DataSet = new System.Data.DataSet();
          DataSet.EnforceConstraints = false;

          Adapter = new Npgsql.NpgsqlDataAdapter(ConnectorObjects.Command);
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
              Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ResultsTable.Rows[0]["ID"]);
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
    protected override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Data.DataSet>> ExecuteCommandAsync(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType, System.Int32 Timeout)
    {
      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ExecuteCommandAsync";

      SoftmakeAll.SDK.OperationResult<System.Data.DataSet> Result = new SoftmakeAll.SDK.OperationResult<System.Data.DataSet>();

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.PostgreSQL.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ConnectorObjects(ConnectionString, ProcedureNameOrCommandText, Parameters, CommandType, false, base.ShowPlan, base.ReadSummaries.Value);

      try
      {
        await ConnectorObjects.Connection.OpenAsync();

        await this.ExecutePreCommandsAsync(ConnectorObjects);

        Npgsql.NpgsqlDataAdapter Adapter = null;

        await base.WriteApplicationDebugEventAsync(ThisProcedureName, ConnectorObjects.Command.CommandText);

        try
        {
          System.Data.DataSet DataSet = new System.Data.DataSet();
          DataSet.EnforceConstraints = false;

          Adapter = new Npgsql.NpgsqlDataAdapter(ConnectorObjects.Command);
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
              Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ResultsTable.Rows[0]["ID"]);
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
      if (base.ShowPlan) return;

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

      System.String SystemEventWriteProcedureName = "{0}[Write{1}{2}Event]";
      SystemEventWriteProcedureName = System.String.Format(SystemEventWriteProcedureName, System.String.IsNullOrWhiteSpace(base.SystemEventsProcedureSchemaName) ? "" : $"[{base.SystemEventsProcedureSchemaName}].", Source, Type);

      SoftmakeAll.SDK.OperationResult Result = new SoftmakeAll.SDK.OperationResult();

      System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters = new System.Collections.Generic.List<System.Data.Common.DbParameter>();
      Parameters.Add(this.CreateInputParameter("ProcedureName", (int)System.Data.SqlDbType.VarChar, 261, ProcedureName));
      Parameters.Add(this.CreateInputParameter("Description", (int)System.Data.SqlDbType.VarChar, 0, Description));

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.PostgreSQL.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ConnectorObjects(ConnectionString, SystemEventWriteProcedureName, Parameters, System.Data.CommandType.StoredProcedure, false, false, base.ReadSummaries.Value);


      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.WriteEvent";

      try
      {
        ConnectorObjects.Connection.Open();

        this.ExecutePreCommands(ConnectorObjects);

        try
        {
          ConnectorObjects.Command.ExecuteNonQuery();
          Result.ExitCode = System.Convert.ToInt16(SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt16(ConnectorObjects.Command.Parameters["$ExitCode"].Value));
          Result.Message = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["$Message"].Value);
          Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["$ID"].Value);
          Result.Count = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt32(ConnectorObjects.Command.Parameters["$Count"].Value);

          if (Result.ExitCode != 0)
            base.WriteErrorFile(Result);
        }
        catch (System.Exception ex)
        {
          Result.ExitCode = 500;
          Result.Message = System.String.Concat(ThisProcedureName, " -> ", System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ExecutionError, ex.Message, ConnectorObjects.Command.CommandText));
          base.WriteErrorFile(Result);
        }

        ConnectorObjects.Connection.Close();
      }
      catch (System.Exception ex)
      {
        Result.ExitCode = 503;
        Result.Message = System.String.Concat(ThisProcedureName, " -> ", System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ConnectionError, ex.Message));
        base.WriteErrorFile(Result);
      }

      ConnectorObjects.Command.Dispose();
      ConnectorObjects.Connection.Dispose();
    }
    protected override async System.Threading.Tasks.Task WriteEventAsync(System.String Source, System.String Type, System.String ProcedureName, System.String Description)
    {
      if (base.ShowPlan) return;

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

      System.String SystemEventWriteProcedureName = "{0}[Write{1}{2}Event]";
      SystemEventWriteProcedureName = System.String.Format(SystemEventWriteProcedureName, System.String.IsNullOrWhiteSpace(base.SystemEventsProcedureSchemaName) ? "" : $"[{base.SystemEventsProcedureSchemaName}].", Source, Type);

      SoftmakeAll.SDK.OperationResult Result = new SoftmakeAll.SDK.OperationResult();

      System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters = new System.Collections.Generic.List<System.Data.Common.DbParameter>();
      Parameters.Add(this.CreateInputParameter("ProcedureName", (int)System.Data.SqlDbType.VarChar, 261, ProcedureName));
      Parameters.Add(this.CreateInputParameter("Description", (int)System.Data.SqlDbType.VarChar, 0, Description));

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.PostgreSQL.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.ConnectorObjects(ConnectionString, SystemEventWriteProcedureName, Parameters, System.Data.CommandType.StoredProcedure, false, false, base.ReadSummaries.Value);


      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.PostgreSQL.Connector.WriteEventAsync";

      try
      {
        await ConnectorObjects.Connection.OpenAsync();

        await this.ExecutePreCommandsAsync(ConnectorObjects);

        try
        {
          ConnectorObjects.Command.ExecuteNonQuery();
          Result.ExitCode = System.Convert.ToInt16(SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt16(ConnectorObjects.Command.Parameters["$ExitCode"].Value));
          Result.Message = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["$Message"].Value);
          Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["$ID"].Value);
          Result.Count = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt32(ConnectorObjects.Command.Parameters["$Count"].Value);

          if (Result.ExitCode != 0)
            base.WriteErrorFile(Result);
        }
        catch (System.Exception ex)
        {
          Result.ExitCode = 500;
          Result.Message = System.String.Concat(ThisProcedureName, " -> ", System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ExecutionError, ex.Message, ConnectorObjects.Command.CommandText));
          await base.WriteErrorFileAsync(Result);
        }

        await ConnectorObjects.Connection.CloseAsync();
      }
      catch (System.Exception ex)
      {
        Result.ExitCode = 503;
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