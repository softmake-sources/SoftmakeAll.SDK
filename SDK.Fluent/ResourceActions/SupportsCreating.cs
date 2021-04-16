using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public class SupportsCreating<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsCreating<T>
  {
    #region Constructor
    public SupportsCreating() { }
    #endregion

    #region Methods
    public T Create(T Model) => base.ProcessOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequest<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "POST", URL = base.GenerateBaseURL(), Body = Model.ToJsonElement() }), Model);
    public async System.Threading.Tasks.Task<T> CreateAsync(T Model) => base.ProcessOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequestAsync<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "POST", URL = base.GenerateBaseURL(), Body = Model.ToJsonElement() }), Model);
    #endregion
  }
}