namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public class ActionRD<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsListing<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsDeleting<T>
  {
    #region Fields
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsListing<T> SupportsListing;
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsDeleting<T> SupportsDeleting;
    #endregion

    #region Constructor
    public ActionRD(System.String Route) : base(Route)
    {
      this.SupportsListing = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsListing<T>(Route);
      this.SupportsDeleting = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsDeleting<T>(Route);
    }
    #endregion

    #region Methods
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

    #region DELETE
    public void Delete(System.Byte ID) => this.SupportsDeleting.Delete(ID);
    public void Delete(System.Int16 ID) => this.SupportsDeleting.Delete(ID);
    public void Delete(System.Int32 ID) => this.SupportsDeleting.Delete(ID);
    public void Delete(System.Int64 ID) => this.SupportsDeleting.Delete(ID);
    public void Delete(System.Char ID) => this.SupportsDeleting.Delete(ID);
    public void Delete(System.String ID) => this.SupportsDeleting.Delete(ID);
    public void Delete(System.Byte[] IDs) => this.SupportsDeleting.Delete(IDs);
    public void Delete(System.Int16[] IDs) => this.SupportsDeleting.Delete(IDs);
    public void Delete(System.Int32[] IDs) => this.SupportsDeleting.Delete(IDs);
    public void Delete(System.Int64[] IDs) => this.SupportsDeleting.Delete(IDs);
    public void Delete(System.Char[] IDs) => this.SupportsDeleting.Delete(IDs);
    public void Delete(System.String[] IDs) => this.SupportsDeleting.Delete(IDs);

    public async System.Threading.Tasks.Task DeleteAsync(System.Byte ID) => await this.SupportsDeleting.DeleteAsync(ID);
    public async System.Threading.Tasks.Task DeleteAsync(System.Int16 ID) => await this.SupportsDeleting.DeleteAsync(ID);
    public async System.Threading.Tasks.Task DeleteAsync(System.Int32 ID) => await this.SupportsDeleting.DeleteAsync(ID);
    public async System.Threading.Tasks.Task DeleteAsync(System.Int64 ID) => await this.SupportsDeleting.DeleteAsync(ID);
    public async System.Threading.Tasks.Task DeleteAsync(System.Char ID) => await this.SupportsDeleting.DeleteAsync(ID);
    public async System.Threading.Tasks.Task DeleteAsync(System.String ID) => await this.SupportsDeleting.DeleteAsync(ID);
    public async System.Threading.Tasks.Task DeleteAsync(System.Byte[] IDs) => await this.SupportsDeleting.DeleteAsync(IDs);
    public async System.Threading.Tasks.Task DeleteAsync(System.Int16[] IDs) => await this.SupportsDeleting.DeleteAsync(IDs);
    public async System.Threading.Tasks.Task DeleteAsync(System.Int32[] IDs) => await this.SupportsDeleting.DeleteAsync(IDs);
    public async System.Threading.Tasks.Task DeleteAsync(System.Int64[] IDs) => await this.SupportsDeleting.DeleteAsync(IDs);
    public async System.Threading.Tasks.Task DeleteAsync(System.Char[] IDs) => await this.SupportsDeleting.DeleteAsync(IDs);
    public async System.Threading.Tasks.Task DeleteAsync(System.String[] IDs) => await this.SupportsDeleting.DeleteAsync(IDs);
    #endregion
    #endregion
  }
}