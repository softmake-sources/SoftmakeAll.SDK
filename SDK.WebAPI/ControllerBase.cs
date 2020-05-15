namespace SoftmakeAll.SDK.WebAPI
{
  [Microsoft.AspNetCore.Mvc.ApiController]
  [Microsoft.AspNetCore.Authorization.Authorize]
  public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
  {
    #region Fields
    private readonly SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstance;
    #endregion

    #region Constructor
    public ControllerBase(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstanceContext)
    {
      this.DatabaseInstance = DatabaseInstanceContext;
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

      if (!(System.String.IsNullOrEmpty(Message)))
        Message = System.String.Concat(": ", Message);

      switch (StatusCode)
      {
        case Microsoft.AspNetCore.Http.StatusCodes.Status200OK:
          await this.DatabaseInstance.WriteApplicationDebugEventAsync(ProcedureName, System.String.Concat("200 - OK", Message));
          break;

        case Microsoft.AspNetCore.Http.StatusCodes.Status206PartialContent:
          await this.DatabaseInstance.WriteApplicationWarningEventAsync(ProcedureName, System.String.Concat("206 - PartialContent", Message));
          break;

        case Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest:
          await this.DatabaseInstance.WriteApplicationErrorEventAsync(ProcedureName, System.String.Concat("400 - BadRequest", Message));
          break;

        case Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict:
          await this.DatabaseInstance.WriteApplicationWarningEventAsync(ProcedureName, System.String.Concat("409 - Conflict", Message));
          break;

        case Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError:
          await this.DatabaseInstance.WriteApplicationErrorEventAsync(ProcedureName, System.String.Concat("500 - InternalServerError", Message));
          break;

        default:
          await this.DatabaseInstance.WriteApplicationWarningEventAsync(ProcedureName, System.String.Concat(StatusCode, Message));
          break;
      }
    }
    #endregion
  }
}