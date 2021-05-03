using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public class SupportsCreating<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsCreating<T>
  {
    #region Constructor
    public SupportsCreating(System.String Route) : base(Route) { }
    #endregion

    #region Methods
    public T Create(T Model) => base.ProcessOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequest(new SoftmakeAll.SDK.Communication.REST() { Method = "POST", URL = base.Route, Body = Model.ToJsonElement() }), Model);
    public async System.Threading.Tasks.Task<T> CreateAsync(T Model) => base.ProcessOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequestAsync(new SoftmakeAll.SDK.Communication.REST() { Method = "POST", URL = base.Route, Body = Model.ToJsonElement() }), Model);
    #endregion
  }
}