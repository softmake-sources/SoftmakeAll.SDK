namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public class ActionCD<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsCreating<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsDeleting<T>
  {
    #region Fields
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.SupportsCreating<T> SupportsCreating;
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.SupportsDeleting<T> SupportsDeleting;
    #endregion

    #region Constructor
    public ActionCD(System.String Route) : base(Route)
    {
      this.SupportsCreating = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsCreating<T>(Route);
      this.SupportsDeleting = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsDeleting<T>(Route);
    }
    #endregion

    #region Methods
    #region CREATE
    public T Create(T Model) => this.SupportsCreating.Create(Model);
    public async System.Threading.Tasks.Task<T> CreateAsync(T Model) => await this.SupportsCreating.CreateAsync(Model);
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