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
      [Microsoft.AspNetCore.Mvc.FromBody()]System.Text.Json.JsonElement Object
      )
    {
      return await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);
    }

    [Microsoft.AspNetCore.Mvc.HttpPost()]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> PostAsync
      (
      System.Collections.Generic.List<Microsoft.AspNetCore.Http.IFormFile> Files
      )
    {
      return await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);
    }

    [Microsoft.AspNetCore.Mvc.HttpGet("{ID:long?}")]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAsync
      (
      [Microsoft.AspNetCore.Mvc.FromRoute()]System.Int64 ID
      )
    {
      return await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);
    }

    [Microsoft.AspNetCore.Mvc.HttpHead("{ID:long?}")]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> HeadAsync
      (
      [Microsoft.AspNetCore.Mvc.FromRoute()]System.Int64 ID
      )
    {
      await this.GetAsync(ID);
      return await this.StatusCodeAsync(200);
    }

    [Microsoft.AspNetCore.Mvc.HttpGet()]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> GetAsync
      (
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Fields")]System.String Fields,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Filter")]System.String Filter,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Sort")]System.String Sort,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Skip")]System.Int32 Skip,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Take")]System.Int32 Take
      )
    {
      return await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);
    }

    [Microsoft.AspNetCore.Mvc.HttpHead()]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> HeadAsync
      (
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Fields")]System.String Fields,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Filter")]System.String Filter,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Sort")]System.String Sort,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Skip")]System.Int32 Skip,
      [Microsoft.AspNetCore.Mvc.FromQuery(Name = "Take")]System.Int32 Take
      )
    {
      await this.GetAsync(Fields, Filter, Sort, Skip, Take);
      return await this.StatusCodeAsync(200);
    }

    [Microsoft.AspNetCore.Mvc.HttpPatch("{ID:long}")]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> PatchAsync
      (
      [Microsoft.AspNetCore.Mvc.FromRoute()]System.Int64 ID,
      [Microsoft.AspNetCore.Mvc.FromBody()]System.Text.Json.JsonElement Object
      )
    {
      return await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);
    }

    [Microsoft.AspNetCore.Mvc.HttpPut("{ID:long}")]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> PutAsync
      (
      [Microsoft.AspNetCore.Mvc.FromRoute()]System.Int64 ID,
      [Microsoft.AspNetCore.Mvc.FromBody()]System.Text.Json.JsonElement Object
      )
    {
      return await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);
    }

    [Microsoft.AspNetCore.Mvc.HttpPut("{ID:long}")]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> PutAsync
      (
      [Microsoft.AspNetCore.Mvc.FromRoute()]System.Int64 ID,
      System.Collections.Generic.List<Microsoft.AspNetCore.Http.IFormFile> Files
      )
    {
      return await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);
    }

    [Microsoft.AspNetCore.Mvc.HttpDelete("{ID:long}")]
    public virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> DeleteAsync
      (
      [Microsoft.AspNetCore.Mvc.FromRoute()]System.Int64 ID
      )
    {
      return await this.StatusCodeAsync((int)System.Net.HttpStatusCode.MethodNotAllowed);
    }
    #endregion

    #region Methods
    protected virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> StatusCodeAsync()
    {
      return await this.StatusCodeAsync(new SoftmakeAll.SDK.OperationResult() { ExitCode = 0, Message = "" });
    }
    protected virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> StatusCodeAsync(SoftmakeAll.SDK.OperationResult ServiceResult)
    {
      if (ServiceResult == null)
        return this.BadRequest();

      System.Int32 DefinedStatusCode = this.DefineHTTPStatusCode(ServiceResult.ExitCode);
      await this.WriteEventByStatusCodeAsync(DefinedStatusCode, ServiceResult.Message);

      return this.StatusCode(DefinedStatusCode, ServiceResult);
    }
    protected virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> StatusCodeAsync(System.Exception Exception)
    {
      if (Exception == null)
        return await this.StatusCodeAsync();

      return await this.StatusCodeAsync(new SoftmakeAll.SDK.OperationResult() { Message = Exception.Message });
    }
    protected virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> StatusCodeAsync(System.Int32 StatusCode) { return await this.StatusCodeAsync(StatusCode, null); }
    protected virtual async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> StatusCodeAsync(System.Int32 StatusCode, System.String Message)
    {
      await this.WriteEventByStatusCodeAsync(StatusCode, Message);
      return this.StatusCode(StatusCode, null);
    }

    private System.Int32 DefineHTTPStatusCode(System.Int32 ExitCode)
    {
      switch (ExitCode)
      {
        case -6:
        case -5:
        case -4:
        case -3:
        case -2:
          return Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError;

        case -1:
          return Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest;

        case 0:
          return Microsoft.AspNetCore.Http.StatusCodes.Status200OK;
      }
      return ExitCode;
    }
    private async System.Threading.Tasks.Task WriteEventByStatusCodeAsync(System.Int32 StatusCode, System.String Message)
    {
      if (this.DatabaseInstance == null)
        return;

      const System.String ProcedureName = "SoftmakeAll.SDK.WebAPI.ControllerBase.WriteEventByStatusCodeAsync";

      System.String Description = $"{StatusCode}{(System.String.IsNullOrWhiteSpace(Message) ? Message : $": {Message}")}";

      switch (StatusCode)
      {
        case Microsoft.AspNetCore.Http.StatusCodes.Status200OK:
          await this.DatabaseInstance.WriteApplicationDebugEventAsync(ProcedureName, Description);
          return;

        case Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent:
        case Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict:
          await this.DatabaseInstance.WriteApplicationWarningEventAsync(ProcedureName, Description);
          return;

        case Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest:
        case Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError:
          await this.DatabaseInstance.WriteApplicationErrorEventAsync(ProcedureName, Description);
          return;
      }

      await this.DatabaseInstance.WriteApplicationInformationEventAsync(ProcedureName, Description);
    }
    #endregion
  }
}