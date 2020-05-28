namespace SoftmakeAll.SDK.WebAPI
{
  [Microsoft.AspNetCore.Authorization.Authorize()]
  public abstract class AuthorizeControllerBase : SoftmakeAll.SDK.WebAPI.ControllerBase
  {
    #region Constructors
    public AuthorizeControllerBase() : base() { }
    public AuthorizeControllerBase(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstanceContext) : base(DatabaseInstanceContext) { }
    #endregion
  }
}