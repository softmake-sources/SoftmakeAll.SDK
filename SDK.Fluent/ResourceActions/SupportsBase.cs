using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// The abstraction to process the results of the requests.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public abstract class SupportsBase<T>
  {
    #region Fields
    /// <summary>
    /// The address of the resource.
    /// </summary>
    protected readonly System.String Route;
    #endregion

    #region Constructor
    /// <summary>
    /// The abstraction to process the results of the requests.
    /// </summary>
    /// <param name="Route">The address of the resource.</param>
    public SupportsBase(System.String Route) => this.Route = Route;
    #endregion

    #region Methods
    /// <summary>
    /// Process operation result from POST, GET (with ID), PATCH, PUT and DELETE.
    /// </summary>
    /// <param name="OperationResult">REST operation result.</param>
    /// <param name="Model">The generic object that represents the any resource to result if operation is not valid.</param>
    /// <returns>Operation result as object when valid. Otherwise the Model parameter.</returns>
    protected internal T ProcessOperationResult(SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult, T Model)
    {
      if ((OperationResult.Success) && (OperationResult.Data.IsValid()))
        return OperationResult.Data[0].ToObject<T>();

      return Model;
    }

    /// <summary>
    /// Process operation result from GET (without ID).
    /// </summary>
    /// <param name="OperationResult">REST operation result.</param>
    /// <returns>A list of resources.</returns>
    protected internal SoftmakeAll.SDK.Fluent.ResourceList<T> ProcessListOperationResult(SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult)
    {
      if ((OperationResult.Success) && (OperationResult.Data.IsValid()))
        return OperationResult.Data.ToObject<SoftmakeAll.SDK.Fluent.ResourceList<T>>();

      return new SoftmakeAll.SDK.Fluent.ResourceList<T>();
    }
    #endregion
  }
}