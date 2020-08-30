using System.Linq;

namespace SoftmakeAll.SDK.Libraries
{
  public abstract class ScriptExecutionBase
  {
    #region Fields
    private readonly SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstance;
    #endregion

    #region Constructor
    public ScriptExecutionBase(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstanceContext)
    {
      this.DatabaseInstance = DatabaseInstanceContext;
    }
    #endregion

    #region Properties
    protected System.Reflection.Assembly ContextAssembly { get; set; }
    protected System.String ProcedureName { get; set; }
    protected System.String ActionName { get; set; }
    #endregion

    #region Methods
    public virtual async System.Threading.Tasks.Task ExecuteAsync()
    {
      if (this.ContextAssembly == null)
        throw new System.Exception("ContextAssembly must be valid.");

      await this.DatabaseInstance.WriteApplicationInformationEventAsync(ProcedureName, "-- Start --".Insert(9, System.String.IsNullOrWhiteSpace(ActionName) ? "" : $"{ActionName} "));

      foreach (System.String ManifestResourceName in this.ContextAssembly.GetManifestResourceNames().Where(mrn => mrn.Contains("._Scripts.Install.DatabaseFiles.")))
      {
        await this.DatabaseInstance.WriteApplicationInformationEventAsync(ProcedureName, ManifestResourceName);
        System.String StreamContents = await new System.IO.StreamReader(this.ContextAssembly.GetManifestResourceStream(ManifestResourceName), System.Text.Encoding.UTF8).ReadToEndAsync();
        await this.DatabaseInstance.ExecuteTextAsync(StreamContents);
      }

      await this.DatabaseInstance.WriteApplicationInformationEventAsync(ProcedureName, "-- Finish --".Insert(10, System.String.IsNullOrWhiteSpace(ActionName) ? "" : $"{ActionName} "));
    }
    #endregion
  }
}