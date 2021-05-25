namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// Updates a resource.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public interface ISupportsUpdating<T>
  {
    #region Methods
    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Byte ID, System.Object Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Int16 ID, System.Object Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Int32 ID, System.Object Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Int64 ID, System.Object Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Char ID, System.Object Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.String ID, System.Object Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public System.Threading.Tasks.Task<T> ModifyAsync(System.Byte ID, System.Object Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public System.Threading.Tasks.Task<T> ModifyAsync(System.Int16 ID, System.Object Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public System.Threading.Tasks.Task<T> ModifyAsync(System.Int32 ID, System.Object Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public System.Threading.Tasks.Task<T> ModifyAsync(System.Int64 ID, System.Object Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public System.Threading.Tasks.Task<T> ModifyAsync(System.Char ID, System.Object Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public System.Threading.Tasks.Task<T> ModifyAsync(System.String ID, System.Object Model);



    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.Byte ID, T Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.Int16 ID, T Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.Int32 ID, T Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.Int64 ID, T Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.Char ID, T Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.String ID, T Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public System.Threading.Tasks.Task<T> ReplaceAsync(System.Byte ID, T Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public System.Threading.Tasks.Task<T> ReplaceAsync(System.Int16 ID, T Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public System.Threading.Tasks.Task<T> ReplaceAsync(System.Int32 ID, T Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public System.Threading.Tasks.Task<T> ReplaceAsync(System.Int64 ID, T Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public System.Threading.Tasks.Task<T> ReplaceAsync(System.Char ID, T Model);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public System.Threading.Tasks.Task<T> ReplaceAsync(System.String ID, T Model);
    #endregion
  }
}