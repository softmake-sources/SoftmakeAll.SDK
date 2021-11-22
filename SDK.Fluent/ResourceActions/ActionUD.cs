namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// Supports updating and deleting resources.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public class ActionUD<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsDeleting<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsUpdating<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsDeleting<T>
  {
    #region Fields
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsUpdating<T> SupportsUpdating;
    #endregion

    #region Constructor
    /// <summary>
    /// Supports updating and deleting resources.
    /// </summary>
    /// <param name="Route">The address of the resources.</param>
    public ActionUD(System.String Route) : base(Route) => this.SupportsUpdating = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsUpdating<T>(Route);
    #endregion

    #region Methods
    #region UPDATE
    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Byte ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Int16 ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Int32 ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Int64 ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Char ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.String ID, System.Object Model) => this.SupportsUpdating.Modify(ID, Model);


    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Byte ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int16 ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int32 ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int64 ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Char ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.String ID, System.Object Model) => await this.SupportsUpdating.ModifyAsync(ID, Model);




    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T Replace(System.Byte ID, T Model) => this.SupportsUpdating.Replace(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T Replace(System.Int16 ID, T Model) => this.SupportsUpdating.Replace(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T Replace(System.Int32 ID, T Model) => this.SupportsUpdating.Replace(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T Replace(System.Int64 ID, T Model) => this.SupportsUpdating.Replace(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T Replace(System.Char ID, T Model) => this.SupportsUpdating.Replace(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T Replace(System.String ID, T Model) => this.SupportsUpdating.Replace(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T ReplacePreventingOverwrite(System.Byte ID, T Model) => this.SupportsUpdating.ReplacePreventingOverwrite(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T ReplacePreventingOverwrite(System.Int16 ID, T Model) => this.SupportsUpdating.ReplacePreventingOverwrite(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T ReplacePreventingOverwrite(System.Int32 ID, T Model) => this.SupportsUpdating.ReplacePreventingOverwrite(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T ReplacePreventingOverwrite(System.Int64 ID, T Model) => this.SupportsUpdating.ReplacePreventingOverwrite(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T ReplacePreventingOverwrite(System.Char ID, T Model) => this.SupportsUpdating.ReplacePreventingOverwrite(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public T ReplacePreventingOverwrite(System.String ID, T Model) => this.SupportsUpdating.ReplacePreventingOverwrite(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <param name="PreventingOverwrite">True: If the server resource model is newer, the system shows an error instead of update. False: Ignores the server resource model version and update it using the Model parameter values.</param>
    /// <returns>The modified resource.</returns>
    public T Replace(System.String ID, T Model, System.Boolean PreventingOverwrite) => this.SupportsUpdating.Replace(ID, Model, PreventingOverwrite);


    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Byte ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int16 ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int32 ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int64 ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Char ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.String ID, T Model) => await this.SupportsUpdating.ReplaceAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.Byte ID, T Model) => await this.SupportsUpdating.ReplacePreventingOverwriteAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.Int16 ID, T Model) => await this.SupportsUpdating.ReplacePreventingOverwriteAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.Int32 ID, T Model) => await this.SupportsUpdating.ReplacePreventingOverwriteAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.Int64 ID, T Model) => await this.SupportsUpdating.ReplacePreventingOverwriteAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.Char ID, T Model) => await this.SupportsUpdating.ReplacePreventingOverwriteAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.String ID, T Model) => await this.SupportsUpdating.ReplacePreventingOverwriteAsync(ID, Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <param name="PreventingOverwrite">True: If the server resource model is newer, the system shows an error instead of update. False: Ignores the server resource model version and update it using the Model parameter values.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.String ID, T Model, System.Boolean PreventingOverwrite) => await this.SupportsUpdating.ReplaceAsync(ID, Model, PreventingOverwrite);
    #endregion
    #endregion
  }
}