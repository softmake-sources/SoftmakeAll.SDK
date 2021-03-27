using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.Fluent
{
  public partial class CRUD<T>
  {
    #region Methods
    public T Create(T Model) =>
      this.ProcessOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequest<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "POST", URL = this.GenerateBaseURL(), Body = Model.ToJsonElement() }), Model);

    public async System.Threading.Tasks.Task<T> CreateAsync(T Model) =>
      this.ProcessOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequestAsync<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "POST", URL = this.GenerateBaseURL(), Body = Model.ToJsonElement() }), Model);
    #endregion
  }
}