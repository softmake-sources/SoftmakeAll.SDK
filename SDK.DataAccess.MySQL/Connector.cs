﻿using System.Linq;

namespace SoftmakeAll.SDK.DataAccess.MySQL
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
        this.Connection = new MySql.Data.MySqlClient.MySqlConnection(ConnectionString);
        this.Command = new MySql.Data.MySqlClient.MySqlCommand(ProcedureNameOrCommandText, this.Connection)
        {
          CommandTimeout = SoftmakeAll.SDK.DataAccess.MySQL.Environment.CommandsTimeout,
          CommandType = CommandType,
        };

        if (ReadSummaries)
        {
          this.Command.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@$Count", MySql.Data.MySqlClient.MySqlDbType.Int32) { Direction = System.Data.ParameterDirection.Output });
          this.Command.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@$ID", MySql.Data.MySqlClient.MySqlDbType.VarChar, 255) { Direction = System.Data.ParameterDirection.Output });
          this.Command.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@$Message", MySql.Data.MySqlClient.MySqlDbType.VarString, -1) { Direction = System.Data.ParameterDirection.Output });
          this.Command.Parameters.Add(new MySql.Data.MySqlClient.MySqlParameter("@$ExitCode", MySql.Data.MySqlClient.MySqlDbType.Int16) { Direction = System.Data.ParameterDirection.Output });
        }

        if ((Parameters != null) && (Parameters.Any()))
          foreach (System.Data.Common.DbParameter Parameter in Parameters)
          {
            Parameter.Value = Parameter.Value ?? System.Convert.DBNull;
            this.Command.Parameters.Add(Parameter);
          }
      }
      #endregion

      #region Properties
      public MySql.Data.MySqlClient.MySqlConnection Connection { get; }
      public MySql.Data.MySqlClient.MySqlCommand Command { get; }
      #endregion
    }
    #endregion

    #region Methods
    #region Helpers
    public override System.String BuildConnectionString()
    {
      if (base.Port == 0) base.Port = 3306;

      if (System.String.IsNullOrWhiteSpace(base.Server))
        return "";

      if (System.String.IsNullOrWhiteSpace(base.Database))
        return "";

      if ((!(base.IntegratedSecurity)) && ((System.String.IsNullOrWhiteSpace(base.UserID)) || (System.String.IsNullOrWhiteSpace(base.Password))))
        return "";

      return $"Server={base.Server}; Port={base.Port}; Database={base.Database}; {(base.IntegratedSecurity ? "IntegratedSecurity=yes; Uid=auth_windows;" : $"Uid={base.UserID}; Pwd={base.Password};")}";
    }
    #endregion

    #region Parameters
    protected override System.Data.Common.DbParameter CreateParameter(System.String Name, System.Int32 Type, System.Int32 Size, System.Object Value, System.Data.ParameterDirection Direction)
    {
      return new MySql.Data.MySqlClient.MySqlParameter
      {
        ParameterName = Name,
        MySqlDbType = (MySql.Data.MySqlClient.MySqlDbType)Type,
        Direction = Direction,
        Size = ((Size == 0) && (Type == (int)MySql.Data.MySqlClient.MySqlDbType.LongText)) ? -1 : Size,
        Value = Value
      };
    }
    #endregion

    #region PreCommands
    private void ExecutePreCommands(SoftmakeAll.SDK.DataAccess.MySQL.Connector.ConnectorObjects ConnectorObjects)
    {
      if (ConnectorObjects == null)
        return;

      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.MySQL.Connector.ExecutePreCommands";

      if (!(System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName)))
      {
        MySql.Data.MySqlClient.MySqlCommand SessionContextCommand = new MySql.Data.MySqlClient.MySqlCommand(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName, ConnectorObjects.Connection);
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
    private async System.Threading.Tasks.Task ExecutePreCommandsAsync(SoftmakeAll.SDK.DataAccess.MySQL.Connector.ConnectorObjects ConnectorObjects)
    {
      if (ConnectorObjects == null)
        return;

      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.MySQL.Connector.ExecutePreCommandsAsync";

      if (!(System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName)))
      {
        MySql.Data.MySqlClient.MySqlCommand SessionContextCommand = new MySql.Data.MySqlClient.MySqlCommand(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName, ConnectorObjects.Connection);
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
      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.MySQL.Connector.ExecuteCommand";

      SoftmakeAll.SDK.OperationResult<System.Data.DataSet> Result = new SoftmakeAll.SDK.OperationResult<System.Data.DataSet>();

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.MySQL.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.MySQL.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.MySQL.Connector.ConnectorObjects(ConnectionString, ProcedureNameOrCommandText, Parameters, CommandType, false, base.ShowPlan, base.ReadSummaries.Value);

      try
      {
        ConnectorObjects.Connection.Open();

        this.ExecutePreCommands(ConnectorObjects);

        MySql.Data.MySqlClient.MySqlDataAdapter Adapter = null;

        base.WriteApplicationDebugEvent(ThisProcedureName, ConnectorObjects.Command.CommandText);

        try
        {
          System.Data.DataSet DataSet = new System.Data.DataSet();
          DataSet.EnforceConstraints = false;

          Adapter = new MySql.Data.MySqlClient.MySqlDataAdapter(ConnectorObjects.Command);
          base.WriteApplicationDebugEvent(ThisProcedureName, ConnectorObjects.Command.CommandText);
          Adapter.Fill(DataSet);

          if ((base.ReadSummaries.Value) && (!(base.ShowPlan)))
          {
            System.Int16? DatabaseExitCode = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt16(ConnectorObjects.Command.Parameters["@$ExitCode"].Value);
            Result.ExitCode = System.Convert.ToInt16(DatabaseExitCode);
            Result.Message = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["@$Message"].Value);
            Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["@$ID"].Value);
            Result.Count = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt32(ConnectorObjects.Command.Parameters["@$Count"].Value);

            if ((DataSet.Tables.Count == 0) && (DatabaseExitCode == null))
            {
              Result.ExitCode = 204;
              if (System.String.IsNullOrWhiteSpace(Result.Message))
                Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.NoInformationReceived, ConnectorObjects.Command.CommandText);
              base.WriteApplicationWarningEvent(ThisProcedureName, Result.Message);
            }
          }
          if (DataSet.Tables.Count == 0) DataSet = null;
          Result.Data = DataSet;
        }
        catch (System.Exception ex)
        {
          Result.ExitCode = 500;
          Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ExecutionError, ex.Message, ConnectorObjects.Command.CommandText);
          base.WriteApplicationErrorEvent(ThisProcedureName, Result.Message);
        }

        Adapter?.Dispose();

        ConnectorObjects.Connection.Close();
      }
      catch (System.Exception ex)
      {
        Result.ExitCode = 503;
        Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ConnectionError, ex.Message);
        base.WriteErrorFile(Result);
      }

      ConnectorObjects.Command.Dispose();
      ConnectorObjects.Connection.Dispose();

      return Result;
    }
    protected override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Data.DataSet>> ExecuteCommandAsync(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType)
    {
      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.MySQL.Connector.ExecuteCommandAsync";

      SoftmakeAll.SDK.OperationResult<System.Data.DataSet> Result = new SoftmakeAll.SDK.OperationResult<System.Data.DataSet>();

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.MySQL.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.MySQL.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.MySQL.Connector.ConnectorObjects(ConnectionString, ProcedureNameOrCommandText, Parameters, CommandType, false, base.ShowPlan, base.ReadSummaries.Value);

      try
      {
        await ConnectorObjects.Connection.OpenAsync();

        await this.ExecutePreCommandsAsync(ConnectorObjects);

        MySql.Data.MySqlClient.MySqlDataAdapter Adapter = null;

        await base.WriteApplicationDebugEventAsync(ThisProcedureName, ConnectorObjects.Command.CommandText);

        try
        {
          System.Data.DataSet DataSet = new System.Data.DataSet();
          DataSet.EnforceConstraints = false;

          Adapter = new MySql.Data.MySqlClient.MySqlDataAdapter(ConnectorObjects.Command);
          Adapter.Fill(DataSet);

          if ((base.ReadSummaries.Value) && (!(base.ShowPlan)))
          {
            System.Int16? DatabaseExitCode = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt16(ConnectorObjects.Command.Parameters["@$ExitCode"].Value);
            Result.ExitCode = System.Convert.ToInt16(DatabaseExitCode);
            Result.Message = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["@$Message"].Value);
            Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["@$ID"].Value);
            Result.Count = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt32(ConnectorObjects.Command.Parameters["@$Count"].Value);

            if ((DataSet.Tables.Count == 0) && (DatabaseExitCode == null))
            {
              Result.ExitCode = 204;
              if (System.String.IsNullOrWhiteSpace(Result.Message))
                Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.NoInformationReceived, ConnectorObjects.Command.CommandText);
              await base.WriteApplicationWarningEventAsync(ThisProcedureName, Result.Message);
            }
          }
          if (DataSet.Tables.Count == 0) DataSet = null;
          Result.Data = DataSet;
        }
        catch (System.Exception ex)
        {
          Result.ExitCode = 500;
          Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ExecutionError, ex.Message, ConnectorObjects.Command.CommandText);
          await base.WriteApplicationErrorEventAsync(ThisProcedureName, Result.Message);
        }

        Adapter?.Dispose();

        await ConnectorObjects.Connection.CloseAsync();
      }
      catch (System.Exception ex)
      {
        Result.ExitCode = 503;
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

      System.String SystemEventWriteProcedureName = "`Write{0}{1}Event`";
      SystemEventWriteProcedureName = System.String.Format(SystemEventWriteProcedureName, Source, Type);

      SoftmakeAll.SDK.OperationResult Result = new SoftmakeAll.SDK.OperationResult();

      System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters = new System.Collections.Generic.List<System.Data.Common.DbParameter>();
      Parameters.Add(this.CreateInputParameter("ProcedureName", (int)MySql.Data.MySqlClient.MySqlDbType.VarChar, 128, ProcedureName));
      Parameters.Add(this.CreateInputParameter("Description", (int)MySql.Data.MySqlClient.MySqlDbType.LongText, Description));

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.MySQL.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.MySQL.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.MySQL.Connector.ConnectorObjects(ConnectionString, SystemEventWriteProcedureName, Parameters, System.Data.CommandType.StoredProcedure, false, false, base.ReadSummaries.Value);


      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.MySQL.Connector.WriteEvent";

      try
      {
        ConnectorObjects.Connection.Open();

        try
        {
          ConnectorObjects.Command.ExecuteNonQuery();
          Result.ExitCode = System.Convert.ToInt16(SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt16(ConnectorObjects.Command.Parameters["@$ExitCode"].Value));
          Result.Message = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["@$Message"].Value);
          Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["@$ID"].Value);
          Result.Count = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt32(ConnectorObjects.Command.Parameters["@$Count"].Value);

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

      System.String SystemEventWriteProcedureName = "`Write{0}{1}Event`";
      SystemEventWriteProcedureName = System.String.Format(SystemEventWriteProcedureName, Source, Type);

      SoftmakeAll.SDK.OperationResult Result = new SoftmakeAll.SDK.OperationResult();

      System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters = new System.Collections.Generic.List<System.Data.Common.DbParameter>();
      Parameters.Add(this.CreateInputParameter("ProcedureName", (int)MySql.Data.MySqlClient.MySqlDbType.VarChar, 128, ProcedureName));
      Parameters.Add(this.CreateInputParameter("Description", (int)MySql.Data.MySqlClient.MySqlDbType.LongText, Description));

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.MySQL.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.MySQL.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.MySQL.Connector.ConnectorObjects(ConnectionString, SystemEventWriteProcedureName, Parameters, System.Data.CommandType.StoredProcedure, false, false, base.ReadSummaries.Value);


      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.MySQL.Connector.WriteEventAsync";

      try
      {
        await ConnectorObjects.Connection.OpenAsync();

        try
        {
          ConnectorObjects.Command.ExecuteNonQuery();
          Result.ExitCode = System.Convert.ToInt16(SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt16(ConnectorObjects.Command.Parameters["@$ExitCode"].Value));
          Result.Message = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["@$Message"].Value);
          Result.ID = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableString(ConnectorObjects.Command.Parameters["@$ID"].Value);
          Result.Count = SoftmakeAll.SDK.DataAccess.DatabaseValues.GetNullableInt32(ConnectorObjects.Command.Parameters["@$Count"].Value);

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