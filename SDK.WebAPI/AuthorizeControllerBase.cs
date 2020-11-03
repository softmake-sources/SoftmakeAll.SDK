namespace SoftmakeAll.SDK.WebAPI
{
  [Microsoft.AspNetCore.Authorization.Authorize()]
  public abstract class AuthorizeControllerBase : SoftmakeAll.SDK.WebAPI.ControllerBase
  {
    #region Constructors
    public AuthorizeControllerBase() : base() { }
    public AuthorizeControllerBase(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstanceContext) : base(DatabaseInstanceContext) { }
    public AuthorizeControllerBase(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstanceContext, SoftmakeAll.SDK.WebAPI.ICRUDOperations ICRUDOperationsContext) : base(DatabaseInstanceContext, ICRUDOperationsContext) { }
    public AuthorizeControllerBase(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstanceContext, SoftmakeAll.SDK.WebAPI.INotificationEmitter INotificationEmitterContext) : base(DatabaseInstanceContext, INotificationEmitterContext) { }
    public AuthorizeControllerBase(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstanceContext, SoftmakeAll.SDK.WebAPI.ICRUDOperations ICRUDOperationsContext, SoftmakeAll.SDK.WebAPI.INotificationEmitter INotificationEmitterContext) : base(DatabaseInstanceContext, ICRUDOperationsContext, INotificationEmitterContext) { }
    #endregion
  }
}