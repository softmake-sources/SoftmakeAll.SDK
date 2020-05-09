using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using SoftmakeAll.SDK.Helpers.String.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.DataAccess.SQLServer
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
        this.Connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
        this.Command = new System.Data.SqlClient.SqlCommand(ProcedureNameOrCommandText, this.Connection)
        {
          CommandTimeout = SoftmakeAll.SDK.DataAccess.SQLServer.Environment.CommandsTimeout,
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
          this.Command.Parameters.Add(new System.Data.SqlClient.SqlParameter
          {
            Direction = System.Data.ParameterDirection.Input,
            ParameterName = "ShowResultsTable",
            SqlDbType = System.Data.SqlDbType.Bit,
            Size = 0,
            Value = true
          });
      }
      #endregion

      #region Properties
      public System.Data.SqlClient.SqlConnection Connection { get; }
      public System.Data.SqlClient.SqlCommand Command { get; }
      #endregion

      #region Fields
      private const System.String Summaries = "SELECT NULL AS [ID], NULL AS [Message], 0 AS [ExitCode]";
      protected override System.String SummariesSQL => Summaries + ";";
      protected override System.String SummariesSQLAsJSON => Summaries + " FOR JSON PATH, WITHOUT_ARRAY_WRAPPER;";
      #endregion
    }
    #endregion

    #region Methods
    #region Helpers
    public override System.String BuildConnectionString()
    {
      if (base.Port == 0) base.Port = 1433;

      if (System.String.IsNullOrWhiteSpace(base.Server))
        return "";

      if (System.String.IsNullOrWhiteSpace(base.Database))
        return "";

      if ((!(base.IntegratedSecurity)) && ((System.String.IsNullOrWhiteSpace(base.UserID)) || (System.String.IsNullOrWhiteSpace(base.Password))))
        return "";

      return $"Server={base.Server},{base.Port}; Initial Catalog={base.Database}; {(base.IntegratedSecurity ? "Integrated Security=SSPI;" : $"User Id={base.UserID}; Password={base.Password};")}";
    }
    private System.Collections.Generic.List<System.String> ReadBlocks(System.String CommandText)
    {
      System.Boolean IsRowCountDefined = false;
      System.Collections.Generic.List<System.String> Result = new System.Collections.Generic.List<System.String>();
      System.Text.StringBuilder Buffer = new System.Text.StringBuilder();
      using (System.IO.StringReader StringReader = new System.IO.StringReader(CommandText))
        while (StringReader.Peek() > -1)
        {
          System.String Line = StringReader.ReadLine();
          if (System.String.IsNullOrWhiteSpace(Line))
          {
            Buffer.AppendLine();
            continue;
          }

          if (Line.Trim().ToLower().Contains("rowcount"))
          {
            if (IsRowCountDefined)
              continue;
            IsRowCountDefined = true;
          }

          if (Line.Trim() == "/*")
          {
            do
              Line = StringReader.ReadLine();
            while (Line.Trim() != "*/");
            Line = StringReader.ReadLine();
          }
          if (System.String.IsNullOrWhiteSpace(Line))
          {
            Buffer.AppendLine();
            continue;
          }

          if (Line.StartsWith("--")) // To ignore inline comments
            continue;

          if (Line.Trim().ToUpper() == "GO")
          {
            if (Buffer.Length > 0)
              Result.Add(Buffer.ToString());
            Buffer.Clear();
          }
          else
          {
            if (!(System.String.IsNullOrWhiteSpace(Line)))
              Buffer.AppendLine(Line);
          }
        }

      if (Buffer.Length > 0)
        Result.Add(Buffer.ToString());

      Buffer.Clear();

      return Result;
    }
    private async System.Threading.Tasks.Task<System.Collections.Generic.List<System.String>> ReadBlocksAsync(System.String CommandText)
    {
      System.Boolean IsRowCountDefined = false;
      System.Collections.Generic.List<System.String> Result = new System.Collections.Generic.List<System.String>();
      System.Text.StringBuilder Buffer = new System.Text.StringBuilder();
      using (System.IO.StringReader StringReader = new System.IO.StringReader(CommandText))
        while (StringReader.Peek() > -1)
        {
          System.String Line = await StringReader.ReadLineAsync();
          if (System.String.IsNullOrWhiteSpace(Line))
          {
            Buffer.AppendLine();
            continue;
          }

          if (Line.Trim().ToLower().Contains("rowcount"))
          {
            if (IsRowCountDefined)
              continue;
            IsRowCountDefined = true;
          }

          if (Line.Trim() == "/*")
          {
            do
              Line = await StringReader.ReadLineAsync();
            while (Line.Trim() != "*/");
            Line = await StringReader.ReadLineAsync();
          }
          if (System.String.IsNullOrWhiteSpace(Line))
          {
            Buffer.AppendLine();
            continue;
          }

          if (Line.StartsWith("--")) // To ignore inline comments
            continue;

          if (Line.Trim().ToUpper() == "GO")
          {
            if (Buffer.Length > 0)
              Result.Add(Buffer.ToString());
            Buffer.Clear();
          }
          else
          {
            if (!(System.String.IsNullOrWhiteSpace(Line)))
              Buffer.AppendLine(Line);
          }
        }

      if (Buffer.Length > 0)
        Result.Add(Buffer.ToString());

      Buffer.Clear();

      return Result;
    }
    #endregion

    #region Parameters
    protected override System.Data.Common.DbParameter CreateParameter(System.String Name, System.Int32 Type, System.Int32 Size, System.Object Value, System.Data.ParameterDirection Direction)
    {
      return new System.Data.SqlClient.SqlParameter
      {
        ParameterName = Name,
        SqlDbType = (System.Data.SqlDbType)Type,
        Direction = Direction,
        Size = Size,
        Value = Value
      };
    }
    #endregion

    #region PreCommands
    private void ExecutePreCommands(SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects ConnectorObjects)
    {
      if (ConnectorObjects == null)
        return;

      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.SQLServer.SQLServer.ExecutePreCommands";


      System.Data.SqlClient.SqlCommand OptionsCommand = new System.Data.SqlClient.SqlCommand();
      OptionsCommand.CommandType = System.Data.CommandType.Text;
      OptionsCommand.Connection = ConnectorObjects.Connection;


      OptionsCommand.CommandText = "SET ARITHABORT ON; SET XACT_ABORT OFF; SET NOCOUNT ON;";
      try
      {
        OptionsCommand.ExecuteNonQuery();
      }
      catch (System.Exception ex)
      {
        base.WriteApplicationWarningEvent(ThisProcedureName, System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ErrorOnSetBaseCommands, ex.Message));
      }


      if (base.SessionContextVariables.Any())
      {
        System.Text.StringBuilder SetSessionContextVariables = new System.Text.StringBuilder();
        foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> SessionContextVariable in base.SessionContextVariables)
          if (!(System.String.IsNullOrWhiteSpace(SessionContextVariable.Key)))
            SetSessionContextVariables.AppendFormat("EXECUTE SP_SET_SESSION_CONTEXT N'{0}', {1}; ", SessionContextVariable.Key.Replace("'", "''"), SessionContextVariable.Value == null ? "NULL" : $"'{SessionContextVariable.Value.Replace("'", "''")}'");

        OptionsCommand.CommandText = SetSessionContextVariables.ToString();
        try
        {
          OptionsCommand.ExecuteNonQuery();
        }
        catch (System.Exception ex)
        {
          base.WriteApplicationWarningEvent(ThisProcedureName, System.String.Concat(SoftmakeAll.SDK.DataAccess.ConnectorBase.ErrorOnSetSessionContext, ex.Message));
        }
      }

      if (!(System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName)))
      {
        OptionsCommand.CommandText = SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName.ToString();
        try
        {
          OptionsCommand.ExecuteNonQuery();
        }
        catch (System.Exception ex)
        {
          base.WriteApplicationWarningEvent(ThisProcedureName, System.String.Format(ErrorOnCallProcedure, SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName, ex.Message));
        }
      }

      if (base.ShowPlan)
      {
        OptionsCommand.CommandText = "SET SHOWPLAN_ALL ON;";
        try
        {
          OptionsCommand.ExecuteNonQuery();
        }
        catch (System.Exception ex)
        {
          base.WriteApplicationWarningEvent(ThisProcedureName, System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ErrorOnSetBaseCommands, ex.Message));
        }
      }

      OptionsCommand.Dispose();
    }
    private async System.Threading.Tasks.Task ExecutePreCommandsAsync(SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects ConnectorObjects)
    {
      if (ConnectorObjects == null)
        return;

      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.SQLServer.SQLServer.ExecutePreCommandsAsync";


      System.Data.SqlClient.SqlCommand OptionsCommand = new System.Data.SqlClient.SqlCommand();
      OptionsCommand.CommandType = System.Data.CommandType.Text;
      OptionsCommand.Connection = ConnectorObjects.Connection;


      OptionsCommand.CommandText = "SET ARITHABORT ON; SET XACT_ABORT OFF; SET NOCOUNT ON;";
      try
      {
        await OptionsCommand.ExecuteNonQueryAsync();
      }
      catch (System.Exception ex)
      {
        await base.WriteApplicationWarningEventAsync(ThisProcedureName, System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ErrorOnSetBaseCommands, ex.Message));
      }


      if (base.SessionContextVariables.Any())
      {
        System.Text.StringBuilder SetSessionContextVariables = new System.Text.StringBuilder();
        foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> SessionContextVariable in base.SessionContextVariables)
          if (!(System.String.IsNullOrWhiteSpace(SessionContextVariable.Key)))
            SetSessionContextVariables.AppendFormat("EXECUTE SP_SET_SESSION_CONTEXT N'{0}', {1}; ", SessionContextVariable.Key.Replace("'", "''"), SessionContextVariable.Value == null ? "NULL" : $"'{SessionContextVariable.Value.Replace("'", "''")}'");

        OptionsCommand.CommandText = SetSessionContextVariables.ToString();
        try
        {
          await OptionsCommand.ExecuteNonQueryAsync();
        }
        catch (System.Exception ex)
        {
          await base.WriteApplicationWarningEventAsync(ThisProcedureName, System.String.Concat(SoftmakeAll.SDK.DataAccess.ConnectorBase.ErrorOnSetSessionContext, ex.Message));
        }
      }

      if (!(System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName)))
      {
        OptionsCommand.CommandText = SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName.ToString();
        try
        {
          await OptionsCommand.ExecuteNonQueryAsync();
        }
        catch (System.Exception ex)
        {
          await base.WriteApplicationWarningEventAsync(ThisProcedureName, System.String.Format(ErrorOnCallProcedure, SoftmakeAll.SDK.DataAccess.Environment.DefineSessionContextProcedureName, ex.Message));
        }
      }

      if (base.ShowPlan)
      {
        OptionsCommand.CommandText = "SET SHOWPLAN_ALL ON;";
        try
        {
          await OptionsCommand.ExecuteNonQueryAsync();
        }
        catch (System.Exception ex)
        {
          await base.WriteApplicationWarningEventAsync(ThisProcedureName, System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ErrorOnSetBaseCommands, ex.Message));
        }
      }

      await OptionsCommand.DisposeAsync();
    }
    #endregion

    #region Command Execution
    protected override SoftmakeAll.SDK.OperationResult<System.Data.DataSet> ExecuteCommand(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType)
    {
      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.SQLServer.SQLServer.ExecuteCommand";

      SoftmakeAll.SDK.OperationResult<System.Data.DataSet> Result = new SoftmakeAll.SDK.OperationResult<System.Data.DataSet>();

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.SQLServer.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects(ConnectionString, ProcedureNameOrCommandText, Parameters, CommandType, false, base.ShowPlan, base.ReadSummaries.Value);

      System.Collections.Generic.List<System.String> Blocks = null;
      if (CommandType == System.Data.CommandType.Text)
        Blocks = this.ReadBlocks(ConnectorObjects.Command.CommandText);
      else
        Blocks = new System.Collections.Generic.List<System.String>() { ConnectorObjects.Command.CommandText };

      try
      {
        ConnectorObjects.Connection.Open();

        this.ExecutePreCommands(ConnectorObjects);

        System.Data.SqlClient.SqlDataAdapter Adapter = null;

        try
        {
          System.Data.DataSet DataSet = new System.Data.DataSet();
          DataSet.EnforceConstraints = false;

          Adapter = new System.Data.SqlClient.SqlDataAdapter(ConnectorObjects.Command);
          if ((base.ShowPlan) || (CommandType == System.Data.CommandType.StoredProcedure) || (Blocks.Count == 1))
          {
            ConnectorObjects.Command.CommandText = Blocks[0];
            Adapter.Fill(DataSet);
          }
          else
            foreach (System.String Block in Blocks)
            {
              System.Data.DataTable DataTable = new System.Data.DataTable();
              ConnectorObjects.Command.CommandText = Block;
              Adapter.Fill(DataTable);
              DataSet.Tables.Add(DataTable);
            }

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
            if ((base.ShowPlan) || (!(base.ReadSummaries.Value)))
            {
              Result.ID = null;
              Result.Message = null;
              Result.ExitCode = 0;
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
      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.SQLServer.SQLServer.ExecuteCommandAsync";

      SoftmakeAll.SDK.OperationResult<System.Data.DataSet> Result = new SoftmakeAll.SDK.OperationResult<System.Data.DataSet>();

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.SQLServer.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects(ConnectionString, ProcedureNameOrCommandText, Parameters, CommandType, false, base.ShowPlan, base.ReadSummaries.Value);

      System.Collections.Generic.List<System.String> Blocks = null;
      if (CommandType == System.Data.CommandType.Text)
        Blocks = await this.ReadBlocksAsync(ConnectorObjects.Command.CommandText);
      else
        Blocks = new System.Collections.Generic.List<System.String>() { ConnectorObjects.Command.CommandText };

      try
      {
        await ConnectorObjects.Connection.OpenAsync();

        await this.ExecutePreCommandsAsync(ConnectorObjects);

        System.Data.SqlClient.SqlDataAdapter Adapter = null;

        await base.WriteApplicationDebugEventAsync(ThisProcedureName, ConnectorObjects.Command.CommandText);

        try
        {
          System.Data.DataSet DataSet = new System.Data.DataSet();
          DataSet.EnforceConstraints = false;

          Adapter = new System.Data.SqlClient.SqlDataAdapter(ConnectorObjects.Command);
          if ((base.ShowPlan) || (CommandType == System.Data.CommandType.StoredProcedure) || (Blocks.Count == 1))
          {
            ConnectorObjects.Command.CommandText = Blocks[0];
            Adapter.Fill(DataSet);
          }
          else
            foreach (System.String Block in Blocks)
            {
              System.Data.DataTable DataTable = new System.Data.DataTable();
              ConnectorObjects.Command.CommandText = Block;
              Adapter.Fill(DataTable);
              DataSet.Tables.Add(DataTable);
            }

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
            if ((base.ShowPlan) || (!(base.ReadSummaries.Value)))
            {
              Result.ID = null;
              Result.Message = null;
              Result.ExitCode = 0;
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

    protected override SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> ExecuteCommandForJSON(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType)
    {
      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.SQLServer.SQLServer.ExecuteCommandForJSON";

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> Result = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();
      if (base.ShowPlan)
      {
        SoftmakeAll.SDK.OperationResult<System.Data.DataSet> ShowPlanResult = this.ExecuteCommand(ProcedureNameOrCommandText, Parameters, CommandType);

        Result.ID = ShowPlanResult.ID;
        Result.Message = ShowPlanResult.Message;
        Result.ExitCode = ShowPlanResult.ExitCode;
        if (ShowPlanResult.ExitCode == 0)
        {
          foreach (System.Data.DataRow Row in ShowPlanResult.Data.Tables[0].Rows)
            Row[0] = System.Convert.ToString(Row[0]).ReplaceLeftWhiteSpacesUntil('.', '|').Replace("..", ".");
          Result.Data = SoftmakeAll.SDK.DataAccess.DatabaseValues.DataTableToJSON(ShowPlanResult.Data.Tables[0], true);
        }
        return Result;
      }


      if ((CommandType == System.Data.CommandType.Text) && (this.ReadBlocks(ProcedureNameOrCommandText).Count > 1))
        throw new System.Exception("To execute multiple blocks, use the method 'ExecuteTextAsync(...)'.");


      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.SQLServer.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects(ConnectionString, ProcedureNameOrCommandText, Parameters, CommandType, true, base.ShowPlan, base.ReadSummaries.Value);

      try
      {
        ConnectorObjects.Connection.Open();

        this.ExecutePreCommands(ConnectorObjects);

        base.WriteApplicationDebugEvent(ThisProcedureName, ConnectorObjects.Command.CommandText);

        try
        {
          using (System.Data.Common.DbDataReader DataReader = ConnectorObjects.Command.ExecuteReader())
          {
            System.Collections.Generic.List<System.Text.Json.JsonElement> AllResults = new System.Collections.Generic.List<System.Text.Json.JsonElement>();
            do
            {
              System.Text.StringBuilder JSONData = new System.Text.StringBuilder();
              while (DataReader.Read())
                JSONData.Append(DataReader[0]);
              AllResults.Add(JSONData.ToString().ToJsonElement());
            } while (DataReader.NextResult());

            System.Boolean ResultsTableOK = false;
            System.Text.Json.JsonElement LastResultSet = AllResults.LastOrDefault();
            System.Text.Json.JsonElement ExitCodeJsonElement = LastResultSet.GetJsonElement("ExitCode");

            if (ExitCodeJsonElement.IsValid())
            {
              Result = LastResultSet.ToObject<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>>();
              if (AllResults.Count >= 3)
                Result.Data = AllResults.SkipLast(1).ToJsonElement();
              else
                Result.Data = AllResults[0];
              ResultsTableOK = true;
            }

            if (!(ResultsTableOK))
            {
              Result.ID = null;
              Result.Message = null;
              Result.ExitCode = 0;
              if (base.ReadSummaries.Value)
              {
                Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ResultSetsNotExpected, ConnectorObjects.Command.CommandText);
                Result.ExitCode = -3;
                base.WriteApplicationDebugEvent(ThisProcedureName, Result.Message);
              }
              else
              {
                if (AllResults.Count == 1)
                  Result.Data = AllResults[0];
                else
                  Result.Data = AllResults.ToJsonElement();
              }
            }
          }
        }
        catch (System.Exception ex)
        {
          Result.ExitCode = -5;
          Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ExecutionError, ex.Message, ConnectorObjects.Command.CommandText);
          base.WriteApplicationErrorEvent(ThisProcedureName, Result.Message);
        }

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
    protected override async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> ExecuteCommandForJSONAsync(System.String ProcedureNameOrCommandText, System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters, System.Data.CommandType CommandType)
    {
      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.SQLServer.SQLServer.ExecuteCommandForJSONAsync";

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> Result = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();
      if (base.ShowPlan)
      {
        SoftmakeAll.SDK.OperationResult<System.Data.DataSet> ShowPlanResult = await this.ExecuteCommandAsync(ProcedureNameOrCommandText, Parameters, CommandType);

        Result.ID = ShowPlanResult.ID;
        Result.Message = ShowPlanResult.Message;
        Result.ExitCode = ShowPlanResult.ExitCode;
        if (ShowPlanResult.ExitCode == 0)
        {
          foreach (System.Data.DataRow Row in ShowPlanResult.Data.Tables[0].Rows)
            Row[0] = System.Convert.ToString(Row[0]).ReplaceLeftWhiteSpacesUntil('.', '|').Replace("..", ".");
          Result.Data = SoftmakeAll.SDK.DataAccess.DatabaseValues.DataTableToJSON(ShowPlanResult.Data.Tables[0], true);
        }
        return Result;
      }


      if ((CommandType == System.Data.CommandType.Text) && ((await this.ReadBlocksAsync(ProcedureNameOrCommandText)).Count > 1))
        throw new System.Exception("To execute multiple blocks, use the method 'ExecuteTextAsync(...)'.");


      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.SQLServer.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects(ConnectionString, ProcedureNameOrCommandText, Parameters, CommandType, true, base.ShowPlan, base.ReadSummaries.Value);

      try
      {
        await ConnectorObjects.Connection.OpenAsync();

        await this.ExecutePreCommandsAsync(ConnectorObjects);


        await base.WriteApplicationDebugEventAsync(ThisProcedureName, ConnectorObjects.Command.CommandText);

        try
        {
          using (System.Data.Common.DbDataReader DataReader = await ConnectorObjects.Command.ExecuteReaderAsync())
          {
            System.Collections.Generic.List<System.Text.Json.JsonElement> AllResults = new System.Collections.Generic.List<System.Text.Json.JsonElement>();
            do
            {
              System.Text.StringBuilder JSONData = new System.Text.StringBuilder();
              while (await DataReader.ReadAsync())
                JSONData.Append(DataReader[0]);
              AllResults.Add(JSONData.ToString().ToJsonElement());
            } while (await DataReader.NextResultAsync());

            System.Boolean ResultsTableOK = false;
            System.Text.Json.JsonElement LastResultSet = AllResults.LastOrDefault();
            System.Text.Json.JsonElement ExitCodeJsonElement = LastResultSet.GetJsonElement("ExitCode");

            if (ExitCodeJsonElement.IsValid())
            {
              Result = LastResultSet.ToObject<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>>();
              if (AllResults.Count >= 3)
                Result.Data = AllResults.SkipLast(1).ToJsonElement();
              else
                Result.Data = AllResults[0];
              ResultsTableOK = true;
            }

            if (!(ResultsTableOK))
            {
              Result.ID = null;
              Result.Message = null;
              Result.ExitCode = 0;
              if (base.ReadSummaries.Value)
              {
                Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ResultSetsNotExpected, ConnectorObjects.Command.CommandText);
                Result.ExitCode = -3;
                await base.WriteApplicationDebugEventAsync(ThisProcedureName, Result.Message);
              }
              else
              {
                if (AllResults.Count == 1)
                  Result.Data = AllResults[0];
                else
                  Result.Data = AllResults.ToJsonElement();
              }
            }
          }
        }
        catch (System.Exception ex)
        {
          Result.ExitCode = -5;
          Result.Message = System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ExecutionError, ex.Message, ConnectorObjects.Command.CommandText);
          await base.WriteApplicationErrorEventAsync(ThisProcedureName, Result.Message);
        }

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

    public SoftmakeAll.SDK.OperationResult ImportData(System.Data.DataTable DataTable) { return this.ImportData(DataTable, 0); }
    public SoftmakeAll.SDK.OperationResult ImportData(System.Data.DataTable DataTable, System.Int32 BatchSize)
    {
      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.SQLServer.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;

      if (System.String.IsNullOrWhiteSpace(ConnectionString))
        throw new System.Exception(SoftmakeAll.SDK.Environment.NullConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();

      try
      {
        using (System.Data.SqlClient.SqlBulkCopy SqlBulkCopy = new System.Data.SqlClient.SqlBulkCopy(ConnectionString, System.Data.SqlClient.SqlBulkCopyOptions.TableLock | System.Data.SqlClient.SqlBulkCopyOptions.UseInternalTransaction))
        {
          SqlBulkCopy.DestinationTableName = DataTable.TableName;
          if (BatchSize > 0) SqlBulkCopy.BatchSize = BatchSize;
          Stopwatch.Start();
          SqlBulkCopy.WriteToServer(DataTable);
          Stopwatch.Stop();
        }

        OperationResult.ExitCode = 0;
        OperationResult.Message = $"{DataTable.Rows.Count} records imported in {Stopwatch.ElapsedMilliseconds} milliseconds.";
      }
      catch (System.Exception ex)
      {
        Stopwatch.Stop();
        OperationResult.Message = ex.Message;
      }

      return OperationResult;
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> ImportDataAsync(System.Data.DataTable DataTable) { return await this.ImportDataAsync(DataTable, 0); }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult> ImportDataAsync(System.Data.DataTable DataTable, System.Int32 BatchSize)
    {
      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.SQLServer.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;

      if (System.String.IsNullOrWhiteSpace(ConnectionString))
        throw new System.Exception(SoftmakeAll.SDK.Environment.NullConnectionString);

      SoftmakeAll.SDK.OperationResult OperationResult = new SoftmakeAll.SDK.OperationResult();

      System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();

      try
      {
        using (System.Data.SqlClient.SqlBulkCopy SqlBulkCopy = new System.Data.SqlClient.SqlBulkCopy(ConnectionString, System.Data.SqlClient.SqlBulkCopyOptions.TableLock | System.Data.SqlClient.SqlBulkCopyOptions.UseInternalTransaction))
        {
          SqlBulkCopy.DestinationTableName = DataTable.TableName;
          if (BatchSize > 0) SqlBulkCopy.BatchSize = BatchSize;
          Stopwatch.Start();
          await SqlBulkCopy.WriteToServerAsync(DataTable);
          Stopwatch.Stop();
        }

        OperationResult.ExitCode = 0;
        OperationResult.Message = $"{DataTable.Rows.Count} records imported in {Stopwatch.ElapsedMilliseconds} milliseconds.";
      }
      catch (System.Exception ex)
      {
        Stopwatch.Stop();
        OperationResult.Message = ex.Message;
      }

      return OperationResult;
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

      System.String SystemEventWriteProcedureName = "{0}[Write{1}{2}Event]";
      SystemEventWriteProcedureName = System.String.Format(SystemEventWriteProcedureName, System.String.IsNullOrWhiteSpace(base.SystemEventsProcedureSchemaName) ? "" : $"[{base.SystemEventsProcedureSchemaName}].", Source, Type);

      SoftmakeAll.SDK.OperationResult Result = new SoftmakeAll.SDK.OperationResult();

      System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters = new System.Collections.Generic.List<System.Data.Common.DbParameter>();
      Parameters.Add(this.CreateInputParameter("ProcedureName", (int)System.Data.SqlDbType.VarChar, 261, ProcedureName));
      Parameters.Add(this.CreateInputParameter("Description", (int)System.Data.SqlDbType.VarChar, 0, Description));
      Parameters.Add(this.CreateInputParameter("ShowResultsTable", (int)System.Data.SqlDbType.Bit, true));

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.SQLServer.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects(ConnectionString, SystemEventWriteProcedureName, Parameters, System.Data.CommandType.StoredProcedure, false, false, base.ReadSummaries.Value);


      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.SQLServer.SQLServer.WriteEvent";

      try
      {
        ConnectorObjects.Connection.Open();

        System.Data.SqlClient.SqlCommand OptionsCommand = new System.Data.SqlClient.SqlCommand("SET ARITHABORT ON; SET XACT_ABORT OFF; SET NOCOUNT ON;", ConnectorObjects.Connection);
        try
        {
          OptionsCommand.ExecuteNonQuery();
        }
        catch (System.Exception ex)
        {
          Result.ID = null;
          Result.Message = System.String.Concat(ThisProcedureName, " -> ", System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ErrorOnSetBaseCommands, ex.Message));
          Result.ExitCode = -5;
          base.WriteErrorFile(Result);
        }
        OptionsCommand.Dispose();

        System.Data.SqlClient.SqlDataAdapter Adapter = null;

        try
        {
          System.Data.DataTable ResultsTable = new System.Data.DataTable();

          Adapter = new System.Data.SqlClient.SqlDataAdapter(ConnectorObjects.Command);
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

      System.String SystemEventWriteProcedureName = "{0}[Write{1}{2}Event]";
      SystemEventWriteProcedureName = System.String.Format(SystemEventWriteProcedureName, System.String.IsNullOrWhiteSpace(base.SystemEventsProcedureSchemaName) ? "" : $"[{base.SystemEventsProcedureSchemaName}].", Source, Type);

      SoftmakeAll.SDK.OperationResult Result = new SoftmakeAll.SDK.OperationResult();

      System.Collections.Generic.List<System.Data.Common.DbParameter> Parameters = new System.Collections.Generic.List<System.Data.Common.DbParameter>();
      Parameters.Add(this.CreateInputParameter("ProcedureName", (int)System.Data.SqlDbType.VarChar, 261, ProcedureName));
      Parameters.Add(this.CreateInputParameter("Description", (int)System.Data.SqlDbType.VarChar, 0, Description));
      Parameters.Add(this.CreateInputParameter("ShowResultsTable", (int)System.Data.SqlDbType.Bit, true));

      System.String ConnectionString = SoftmakeAll.SDK.DataAccess.SQLServer.Environment._ConnectionString;
      if (!(System.String.IsNullOrEmpty(base.ConnectionString)))
        ConnectionString = base.ConnectionString;
      SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects ConnectorObjects = new SoftmakeAll.SDK.DataAccess.SQLServer.Connector.ConnectorObjects(ConnectionString, SystemEventWriteProcedureName, Parameters, System.Data.CommandType.StoredProcedure, false, false, base.ReadSummaries.Value);


      const System.String ThisProcedureName = "SoftmakeAll.SDK.DataAccess.SQLServer.SQLServer.WriteEventAsync";

      try
      {
        await ConnectorObjects.Connection.OpenAsync();

        System.Data.SqlClient.SqlCommand OptionsCommand = new System.Data.SqlClient.SqlCommand("SET ARITHABORT ON; SET XACT_ABORT OFF; SET NOCOUNT ON;", ConnectorObjects.Connection);
        try
        {
          await OptionsCommand.ExecuteNonQueryAsync();
        }
        catch (System.Exception ex)
        {
          Result.ID = null;
          Result.Message = System.String.Concat(ThisProcedureName, " -> ", System.String.Format(SoftmakeAll.SDK.DataAccess.ConnectorBase.ErrorOnSetBaseCommands, ex.Message));
          Result.ExitCode = -5;
          await base.WriteErrorFileAsync(Result);
        }
        await OptionsCommand.DisposeAsync();

        System.Data.SqlClient.SqlDataAdapter Adapter = null;

        try
        {
          System.Data.DataTable ResultsTable = new System.Data.DataTable();

          Adapter = new System.Data.SqlClient.SqlDataAdapter(ConnectorObjects.Command);
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