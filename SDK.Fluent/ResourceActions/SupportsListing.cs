using System.Linq;

namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public class SupportsListing<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsListing<T>
  {
    #region Constructor
    public SupportsListing(System.String Route) : base(Route) { }
    #endregion

    #region Methods
    private System.String GenerateListURL(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null, // NOT YET IMPLEMENTED
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
    {
      System.String URL = $"{base.Route}?skip={Skip}&take={Take}";

      if ((Parameters != null) && (Parameters.Any())) URL = $"{URL}&{System.String.Join('&', Parameters.Select(p => $"{p.Key}={p.Value}"))}";
      if ((Fields != null) && (Fields.Any())) URL = $"{URL}&fields={System.String.Join(',', Fields)}";
      if ((Group != null) && (Group.Any())) URL = $"{URL}&group={System.String.Join(',', Group)}";
      if ((Sort != null) && (Sort.Any())) URL = $"{URL}&sort={System.String.Join('&', Sort.Select(p => $"{(p.Value ? '-' : '+')}{p.Key}"))}";

      return URL;
    }

    public SoftmakeAll.SDK.Fluent.ResourceList<T> List(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null, // NOT YET IMPLEMENTED
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
      => base.ProcessOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequest(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = this.GenerateListURL(Parameters, Fields, Filter, Group, Sort, Skip, Take) }));

    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.Fluent.ResourceList<T>> ListAsync(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null, // NOT YET IMPLEMENTED
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
      => base.ProcessOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequestAsync(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = this.GenerateListURL(Parameters, Fields, Filter, Group, Sort, Skip, Take) }));

    public T Show(System.Byte ID) => this.Show(ID.ToString());
    public T Show(System.Int16 ID) => this.Show(ID.ToString());
    public T Show(System.Int32 ID) => this.Show(ID.ToString());
    public T Show(System.Int64 ID) => this.Show(ID.ToString());
    public T Show(System.Char ID) => this.Show(ID.ToString());
    public T Show(System.String ID)
    {
      if (!(System.String.IsNullOrWhiteSpace(ID)))
        return base.ProcessOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequest(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = $"{base.Route}/{ID}" }), default);

      return default;
    }

    public async System.Threading.Tasks.Task<T> ShowAsync(System.Byte ID) => await this.ShowAsync(ID.ToString());
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int16 ID) => await this.ShowAsync(ID.ToString());
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int32 ID) => await this.ShowAsync(ID.ToString());
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int64 ID) => await this.ShowAsync(ID.ToString());
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Char ID) => await this.ShowAsync(ID.ToString());
    public async System.Threading.Tasks.Task<T> ShowAsync(System.String ID)
    {
      if (!(System.String.IsNullOrWhiteSpace(ID)))
        return base.ProcessOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequestAsync(new SoftmakeAll.SDK.Communication.REST() { Method = "GET", URL = $"{base.Route}/{ID}" }), default);

      return default;
    }
    #endregion
  }
}