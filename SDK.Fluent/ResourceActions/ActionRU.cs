namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// Supports listing and updating resources.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public class ActionRU<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsUpdating<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsListing<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsUpdating<T>
  {
    #region Fields
    private readonly SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsListing<T> SupportsListing;
    #endregion

    #region Constructor
    /// <summary>
    /// Supports listing and updating resources.
    /// </summary>
    /// <param name="Route">The address of the resources.</param>
    public ActionRU(System.String Route) : base(Route) => this.SupportsListing = new SoftmakeAll.SDK.Fluent.ResourceActions.SupportsListing<T>(Route);
    #endregion

    #region Methods
    #region READ
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
      )
      => this.SupportsListing.List(Parameters, Fields, Filter, Group, Sort, Skip, Take);

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
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.Fluent.ResourceList<T>> ListAsync(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
      => await this.SupportsListing.ListAsync(Parameters, Fields, Filter, Group, Sort, Skip, Take);


    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Byte ID) => this.SupportsListing.Show(ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Int16 ID) => this.SupportsListing.Show(ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Int32 ID) => this.SupportsListing.Show(ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Int64 ID) => this.SupportsListing.Show(ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.Char ID) => this.SupportsListing.Show(ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public T Show(System.String ID) => this.SupportsListing.Show(ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Byte ID) => await this.SupportsListing.ShowAsync(ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int16 ID) => await this.SupportsListing.ShowAsync(ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int32 ID) => await this.SupportsListing.ShowAsync(ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Int64 ID) => await this.SupportsListing.ShowAsync(ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.Char ID) => await this.SupportsListing.ShowAsync(ID);

    /// <summary>
    /// Gets a single resource by ID.
    /// </summary>
    /// <param name="ID">The ID of the resource to show.</param>
    /// <returns>A resource represented by ID.</returns>
    public async System.Threading.Tasks.Task<T> ShowAsync(System.String ID) => await this.SupportsListing.ShowAsync(ID);
    #endregion
    #endregion
  }
}