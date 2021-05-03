namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public class ActionCR<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsCreating<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsListing<T>
  {
    #region Fields
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.SupportsCreating<T> SupportsCreating;
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.SupportsListing<T> SupportsListing;
    #endregion

    #region Constructor
    public ActionCR(System.String Route) : base(Route)
    {
      this.SupportsCreating = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsCreating<T>(Route);
      this.SupportsListing = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsListing<T>(Route);
    }
    #endregion

    #region Methods
    #region CREATE
    public T Create(T Model) => this.SupportsCreating.Create(Model);
    public async System.Threading.Tasks.Task<T> CreateAsync(T Model) => await this.SupportsCreating.CreateAsync(Model);
    #endregion

    #region READ
    public SoftmakeAll.SDK.Fluent.ResourceList<T> List(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
      => this.SupportsListing.List(Parameters, Fields, Filter, Group, Sort, Skip, Take);

    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.Fluent.ResourceList<T>> ListAsync(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
      => await this.SupportsListing.ListAsync(Parameters, Fields, Filter, Group, Sort, Skip, Take);

    public T Show(System.Byte ID) => this.SupportsListing.Show(ID);
    public T Show(System.Int16 ID) => this.SupportsListing.Show(ID);
    public T Show(System.Int32 ID) => this.SupportsListing.Show(ID);
    public T Show(System.Int64 ID) => this.SupportsListing.Show(ID);
    public T Show(System.Char ID) => this.SupportsListing.Show(ID);
    public T Show(System.String ID) => this.SupportsListing.Show(ID);

    public async System.Threading.Tasks.Task<T> ShowAsync(System.Byte ID) => await this.SupportsListing.ShowAsync(ID);
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int16 ID) => await this.SupportsListing.ShowAsync(ID);
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int32 ID) => await this.SupportsListing.ShowAsync(ID);
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int64 ID) => await this.SupportsListing.ShowAsync(ID);
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Char ID) => await this.SupportsListing.ShowAsync(ID);
    public async System.Threading.Tasks.Task<T> ShowAsync(System.String ID) => await this.SupportsListing.ShowAsync(ID);
    #endregion
    #endregion
  }
}