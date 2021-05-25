namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// List and Show resources.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public interface ISupportsListing<T>
  {
    #region Methods
    /// <summary>
    /// Fetch a list of resources.
    /// </summary>
    /// <param name="Parameters">Parameters to send as a query string.</param>
    /// <param name="Fields">List of field names to retrieve.</param>
    /// <param name="Filter">List of filters to apply.</param>
    /// <param name="Group">List of field names to group and perform aggregates if available.</param>
    /// <param name="Sort">List of fields to apply sorting.</param>
    /// <param name="Skip">Number of records to skip.</param>
    /// <param name="Take">Number of records to take.</param>
    /// <returns>The list of the resources.</returns>
    public SoftmakeAll.SDK.Fluent.ResourceList<T> List(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      );

    /// <summary>
    /// Fetch a list of resources.
    /// </summary>
    /// <param name="Parameters">Parameters to send as a query string.</param>
    /// <param name="Fields">List of field names to retrieve.</param>
    /// <param name="Filter">List of filters to apply.</param>
    /// <param name="Group">List of field names to group and perform aggregates if available.</param>
    /// <param name="Sort">List of fields to apply sorting.</param>
    /// <param name="Skip">Number of records to skip.</param>
    /// <param name="Take">Number of records to take.</param>
    /// <returns>The list of the resources.</returns>
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.Fluent.ResourceList<T>> ListAsync(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      );

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Byte ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Int16 ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Int32 ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Int64 ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Char ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.String ID);


    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public System.Threading.Tasks.Task<T> ShowAsync(System.Byte ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public System.Threading.Tasks.Task<T> ShowAsync(System.Int16 ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public System.Threading.Tasks.Task<T> ShowAsync(System.Int32 ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public System.Threading.Tasks.Task<T> ShowAsync(System.Int64 ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public System.Threading.Tasks.Task<T> ShowAsync(System.Char ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public System.Threading.Tasks.Task<T> ShowAsync(System.String ID);
    #endregion
  }
}