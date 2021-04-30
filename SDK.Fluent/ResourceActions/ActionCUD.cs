namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public class ActionCUD<T> : SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsCreating<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsUpdating<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsDeleting<T>
  {
    #region Fields
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsCreating<T> SupportsCreating;
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsUpdating<T> SupportsUpdating;
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsDeleting<T> SupportsDeleting;
    #endregion

    #region Constructor
    public ActionCUD()
    {
      this.SupportsCreating = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsCreating<T>();
      this.SupportsUpdating = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsUpdating<T>();
      this.SupportsDeleting = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsDeleting<T>();
    }
    #endregion

    #region Methods
    #region CREATE
    public T Create(T Model) => this.SupportsCreating.Create(Model);
    public async System.Threading.Tasks.Task<T> CreateAsync(T Model) => await this.SupportsCreating.CreateAsync(Model);
    #endregion

    #region UPDATE
    public T Modify(System.Byte ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);
    public T Modify(System.Int16 ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);
    public T Modify(System.Int32 ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);
    public T Modify(System.Int64 ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);
    public T Modify(System.Char ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);
    public T Modify(System.String ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);

    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Byte ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int16 ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int32 ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int64 ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Char ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.String ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);

    public T Replace(System.Byte ID, T Model) => this.SupportsUpdating.Replace(ID, Model);
    public T Replace(System.Int16 ID, T Model) => this.SupportsUpdating.Replace(ID, Model);
    public T Replace(System.Int32 ID, T Model) => this.SupportsUpdating.Replace(ID, Model);
    public T Replace(System.Int64 ID, T Model) => this.SupportsUpdating.Replace(ID, Model);
    public T Replace(System.Char ID, T Model) => this.SupportsUpdating.Replace(ID, Model);
    public T Replace(System.String ID, T Model) => this.SupportsUpdating.Replace(ID, Model);

    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Byte ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int16 ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int32 ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int64 ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Char ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.String ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);
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