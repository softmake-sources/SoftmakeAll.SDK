namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// Creates a resource.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public interface ISupportsCreating<T>
  {
    #region Methods
    /// <summary>
    /// Creates a new resource.
    /// </summary>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The created resource.</returns>
    public T Create(T Model);

    /// <summary>
    /// Creates a new resource.
    /// </summary>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The created resource.</returns>
    public System.Threading.Tasks.Task<T> CreateAsync(T Model);
    #endregion
  }
}