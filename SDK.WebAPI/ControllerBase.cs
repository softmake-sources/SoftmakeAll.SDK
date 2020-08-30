namespace SoftmakeAll.SDK.WebAPI
{
  [Microsoft.AspNetCore.Mvc.ApiController]
  public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
  {
    #region Fields
    private readonly SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstance;
    #endregion

    #region Constructor
    public ControllerBase() { }
    public ControllerBase(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstanceContext)
    {
      this.DatabaseInstance = DatabaseInstanceContext;
    }
    #endregion

    #region Endpoints
    [Microsoft.AspNetCore.Mvc.HttpOptions()]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> OptionsAsync
      (
      )
    {
      base.HttpContext.Response.Headers["Allow"] = "OPTIONS, POST, GET, HEAD, PATCH, PUT, DELETE";
      return await this.StatusCodeAsync();
    }

    [Microsoft.AspNetCore.Mvc.HttpPost()]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> PostAsync
      (
      [Microsoft.AspNetCore.Mvc.FromBody()] System.Text.Json.JsonElement Object
      )
      => await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);

    [Microsoft.AspNetCore.Mvc.HttpPost("{ID}")]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> PostAsync
      (
      [Microsoft.AspNetCore.Mvc.FromRoute()] System.String ID,
      System.Collections.Generic.List<Microsoft.AspNetCore.Http.IFormFile> Files
      )
      => await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);

    [Microsoft.AspNetCore.Mvc.HttpGet("{ID}")]
    [Microsoft.AspNetCore.Mvc.HttpHead("{ID}")]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAsync
      (
      [Microsoft.AspNetCore.Mvc.FromRoute()] System.String ID
      )
      => await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);

    [Microsoft.AspNetCore.Mvc.HttpGet()]
    [Microsoft.AspNetCore.Mvc.HttpHead()]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAsync
      (
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Fields")] System.String Fields,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Filter")] System.String Filter,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Group")] System.String Group,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Sort")] System.String Sort,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Skip")] System.Int32 Skip,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Take")] System.Int32 Take
      )
      => await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);

    [Microsoft.AspNetCore.Mvc.HttpPatch("{ID}")]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> PatchAsync
      (
      [Microsoft.AspNetCore.Mvc.FromRoute()] System.String ID,
      [Microsoft.AspNetCore.Mvc.FromBody()] System.Text.Json.JsonElement Object
      )
      => await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);

    [Microsoft.AspNetCore.Mvc.HttpPut("{ID}")]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> PutAsync
      (
      [Microsoft.AspNetCore.Mvc.FromRoute()] System.String ID,
      [Microsoft.AspNetCore.Mvc.FromBody()] System.Text.Json.JsonElement Object
      )
      => await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);

    [Microsoft.AspNetCore.Mvc.HttpDelete("{ID}")]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> DeleteAsync
      (
      [Microsoft.AspNetCore.Mvc.FromRoute()] System.String ID
      )
      => await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);

    [Microsoft.AspNetCore.Mvc.HttpDelete()]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> DeleteAsync
      (
      [Microsoft.AspNetCore.Mvc.FromBody()] System.Text.Json.JsonElement Array
      )
      => await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);
    #endregion

    #region Methods
    protected virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> StatusCodeAsync() { return await this.StatusCodeAsync(new SoftmakeAll.SDK.OperationResult()); }
    protected virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> StatusCodeAsync(System.Int32 ExitCode) { return await this.StatusCodeAsync(ExitCode, null); }
    protected virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> StatusCodeAsync(System.Int32 ExitCode, System.String Message) { return await this.StatusCodeAsync(new SoftmakeAll.SDK.OperationResult() { ExitCode = ExitCode, Message = Message }); }
    protected virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> StatusCodeAsync(SoftmakeAll.SDK.OperationResult OperationResult)
    {
      if (OperationResult == null) return this.BadRequest();

      await this.WriteEventAsync(OperationResult);

      if (OperationResult.ExitCode == 204) OperationResult.Message = null;

      System.Int32 HTTPStatusCode = this.ConvertExitCodeToHTTPStatusCode(OperationResult.ExitCode);
      if ((OperationResult.ExitCode == 401) || (OperationResult.ExitCode == 403) || (OperationResult.ExitCode == 404) || (OperationResult.ExitCode == 405)) OperationResult = null;
      return this.StatusCode(HTTPStatusCode, OperationResult);
    }

    private System.Int32 ConvertExitCodeToHTTPStatusCode(System.Int32 ExitCode)
    {
      if (ExitCode < 0)
        return Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;
      else if ((ExitCode == 0) || (ExitCode == 204))
        return Microsoft.AspNetCore.Http.StatusCodes.Status200OK;
      return ExitCode;
    }
    private async System.Threading.Tasks.Task WriteEventAsync(SoftmakeAll.SDK.OperationResult OperationResult)
    {
      if (this.DatabaseInstance == null)
        return;

      const System.String ProcedureName = "SoftmakeAll.SDK.WebAPI.ControllerBase.WriteEventByStatusCodeAsync";

      System.String Description = $"{OperationResult.ExitCode}{(System.String.IsNullOrWhiteSpace(OperationResult.Message) ? OperationResult.Message : $": {OperationResult.Message}")}";

      switch (OperationResult.ExitCode)
      {
        case 200:
          await this.DatabaseInstance.WriteApplicationInformationEventAsync(ProcedureName, Description);
          return;

        case 206:
        case 409:
          await this.DatabaseInstance.WriteApplicationWarningEventAsync(ProcedureName, Description);
          return;

        case 500:
          await this.DatabaseInstance.WriteApplicationErrorEventAsync(ProcedureName, Description);
          return;
      }

      await this.DatabaseInstance.WriteApplicationDebugEventAsync(ProcedureName, Description);
    }
    #endregion
  }
}