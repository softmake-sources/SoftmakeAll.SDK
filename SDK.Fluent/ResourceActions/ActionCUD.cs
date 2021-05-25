namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// Supports creating, updating and deleting resources.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public class ActionCUD<T> : SoftmakeAll.SDK.Fluent.ResourceActions.ActionUD<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsCreating<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsUpdating<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsDeleting<T>
  {
    #region Fields
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsCreating<T> SupportsCreating;
    #endregion

    #region Constructor
    /// <summary>
    /// Supports creating, updating and deleting resources.
    /// </summary>
    /// <param name="Route">The address of the resources.</param>
    public ActionCUD(System.String Route) : base(Route) => this.SupportsCreating = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsCreating<T>(Route);
    #endregion

    #region Methods
    #region CREATE
    /// <summary>
    /// Creates a new resource.
    /// </summary>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The created resource.</returns>
    public T Create(T Model) => this.SupportsCreating.Create(Model);

    /// <summary>
    /// Creates a new resource.
    /// </summary>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The created resource.</returns>
    public async System.Threading.Tasks.Task<T> CreateAsync(T Model) => await this.SupportsCreating.CreateAsync(Model);
    #endregion
    #endregion
  }
}