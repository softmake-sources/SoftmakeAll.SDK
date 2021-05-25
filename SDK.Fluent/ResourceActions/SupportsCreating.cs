using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// Supports creating resources.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public class SupportsCreating<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsCreating<T>
  {
    #region Constructor
    /// <summary>
    /// Supports creating resources.
    /// </summary>
    /// <param name="Route">The address of the resource.</param>
    public SupportsCreating(System.String Route) : base(Route) { }
    #endregion

    #region Methods
    /// <summary>
    /// Creates a new resource.
    /// </summary>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The created resource.</returns>
    public T Create(T Model) => base.ProcessOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequest(new SoftmakeAll.SDK.Communication.REST() { Method = "POST", URL = base.Route, Body = Model.ToJsonElement() }), Model);

    /// <summary>
    /// Creates a new resource.
    /// </summary>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The created resource.</returns>
    public async System.Threading.Tasks.Task<T> CreateAsync(T Model) => base.ProcessOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequestAsync(new SoftmakeAll.SDK.Communication.REST() { Method = "POST", URL = base.Route, Body = Model.ToJsonElement() }), Model);
    #endregion
  }
}