namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public class ActionCU<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsCreating<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsUpdating<T>
  {
    #region Fields
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.SupportsCreating<T> SupportsCreating;
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.SupportsUpdating<T> SupportsUpdating;
    #endregion

    #region Constructor
    public ActionCU()
    {
      this.SupportsCreating = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsCreating<T>();
      this.SupportsUpdating = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsUpdating<T>();
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
    #endregion
  }
}