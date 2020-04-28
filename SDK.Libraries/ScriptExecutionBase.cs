using System.Linq;

namespace SoftmakeAll.SDK.Libraries
{
  public abstract class ScriptExecutionBase
  {
    #region Constructor
    public ScriptExecutionBase() { }
    #endregion

    #region Properties
    protected System.Reflection.Assembly ContextAssembly { get; set; }
    protected System.String ProcedureName { get; set; }
    protected System.String ActionName { get; set; }
    #endregion

    #region Methods
    public virtual async System.Threading.Tasks.Task ExecuteAsync(System.Security.Claims.ClaimsPrincipal ApplicationUser)
    {
      if (this.ContextAssembly == null)
        throw new System.Exception("ContextAssembly must be valid.");

      SoftmakeAll.SDK.DataAccess.ConnectorBase Database = new SoftmakeAll.SDK.DataAccess.SQLServer.Connector(ApplicationUser);

      await Database.WriteApplicationInformationEventAsync(ProcedureName, "-- Start --".Insert(9, System.String.IsNullOrWhiteSpace(ActionName) ? "" : $"{ActionName} "));

      foreach (System.String ManifestResourceName in this.ContextAssembly.GetManifestResourceNames().Where(mrn => mrn.Contains("._Scripts.Install.SQLFiles.")))
      {
        await Database.WriteApplicationInformationEventAsync(ProcedureName, ManifestResourceName);
        using (System.IO.Stream Stream = this.ContextAssembly.GetManifestResourceStream(ManifestResourceName))
          await this.ExecuteSQLFileStreamAsync(Database, Stream);
      }

      await Database.WriteApplicationInformationEventAsync(ProcedureName, "-- Finish --".Insert(10, System.String.IsNullOrWhiteSpace(ActionName) ? "" : $"{ActionName} "));
    }
    private async System.Threading.Tasks.Task<System.Boolean> ExecuteSQLFileStreamAsync(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstance, System.IO.Stream Stream)
    {
      if (Stream == null)
        return false;

      System.IO.StreamReader StreamReader = new System.IO.StreamReader(Stream, System.Text.Encoding.UTF8);
      System.Text.StringBuilder Buffer = new System.Text.StringBuilder();
      while (StreamReader.Peek() > -1)
      {
        System.String Line = await StreamReader.ReadLineAsync();
        if (System.String.IsNullOrWhiteSpace(Line))
        {
          Buffer.AppendLine();
          continue;
        }

        if (Line.Trim() == "/*")
        {
          do
            Line = await StreamReader.ReadLineAsync();
          while (Line.Trim() != "*/");
          Line = await StreamReader.ReadLineAsync();
        }
        if (System.String.IsNullOrWhiteSpace(Line))
        {
          Buffer.AppendLine();
          continue;
        }

        if ((Line.ToUpper().StartsWith("USE ")) || (Line.StartsWith("--"))) // To ignore USE commands or inline comments
          continue;

        if (Line.Trim().ToUpper() == "GO")
        {
          if (!(await this.ExecuteBufferAsync(DatabaseInstance, Buffer.ToString())))
          {
            Buffer.Clear();
            StreamReader.Close();
            StreamReader.Dispose();
            return false;
          }

          Buffer.Clear();
        }
        else
        {
          if (!(System.String.IsNullOrWhiteSpace(Line)))
            Buffer.AppendLine(Line);
        }
      }
      StreamReader.Close();
      StreamReader.Dispose();

      if ((Buffer.Length > 0) && (!(await this.ExecuteBufferAsync(DatabaseInstance, Buffer.ToString()))))
      {
        Buffer.Clear();
        return false;
      }

      return true;
    }
    private async System.Threading.Tasks.Task<System.Boolean> ExecuteBufferAsync(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstance, System.String Buffer)
    {
      if (System.String.IsNullOrWhiteSpace(Buffer))
        return true;

      SoftmakeAll.SDK.OperationResult<System.Data.DataSet> Result = await DatabaseInstance.ExecuteTextAsync(Buffer.ToString());
      if (Result.ExitCode != 0)
        return false;

      return true;
    }
    #endregion
  }
}