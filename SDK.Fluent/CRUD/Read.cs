using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Fluent
{
  public partial class CRUD<T>
  {
    #region Methods
    public SoftmakeAll.SDK.Fluent.ResourceList<T> List(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
      => this.ProcessOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequest<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = this.GenerateListURL(Parameters, Fields, Filter, Group, Sort, Skip, Take) }));

    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.Fluent.ResourceList<T>> ListAsync(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
      => this.ProcessOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequestAsync<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = this.GenerateListURL(Parameters, Fields, Filter, Group, Sort, Skip, Take) }));

    public T Show(System.Byte ID) => this.Show(ID.ToString());
    public T Show(System.Int16 ID) => this.Show(ID.ToString());
    public T Show(System.Int32 ID) => this.Show(ID.ToString());
    public T Show(System.Int64 ID) => this.Show(ID.ToString());
    public T Show(System.Char ID) => this.Show(ID.ToString());
    public T Show(System.String ID)
    {
      if (System.String.IsNullOrWhiteSpace(ID))
        return default;

      return this.ProcessOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequest<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = $"{this.GenerateBaseURL()}/{ID}" }), default);
    }

    public async System.Threading.Tasks.Task<T> ShowAsync(System.Byte ID) => await this.ShowAsync(ID.ToString());
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int16 ID) => await this.ShowAsync(ID.ToString());
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int32 ID) => await this.ShowAsync(ID.ToString());
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int64 ID) => await this.ShowAsync(ID.ToString());
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Char ID) => await this.ShowAsync(ID.ToString());
    public async System.Threading.Tasks.Task<T> ShowAsync(System.String ID)
    {
      if (System.String.IsNullOrWhiteSpace(ID))
        return default;

      return this.ProcessOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequestAsync<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = $"{this.GenerateBaseURL()}/{ID}" }), default);
    }
    #endregion
  }
}