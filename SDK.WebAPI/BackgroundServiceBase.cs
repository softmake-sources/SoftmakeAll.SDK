namespace SoftmakeAll.SDK.WebAPI
{
  public abstract class BackgroundServiceBase : Microsoft.Extensions.Hosting.BackgroundService
  {
    #region Fields
    protected readonly SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstance;
    protected readonly SoftmakeAll.SDK.WebAPI.INotificationEmitter INotificationEmitter;
    #endregion

    #region Constructor
    public BackgroundServiceBase() : base() { }
    public BackgroundServiceBase(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstanceContext) => this.DatabaseInstance = DatabaseInstanceContext;
    public BackgroundServiceBase(SoftmakeAll.SDK.WebAPI.INotificationEmitter INotificationEmitterContext) => this.INotificationEmitter = INotificationEmitterContext;
    public BackgroundServiceBase(SoftmakeAll.SDK.DataAccess.ConnectorBase DatabaseInstanceContext, SoftmakeAll.SDK.WebAPI.INotificationEmitter INotificationEmitterContext)
    {
      this.DatabaseInstance = DatabaseInstanceContext;
      this.INotificationEmitter = INotificationEmitterContext;
    }
    #endregion
  }
}