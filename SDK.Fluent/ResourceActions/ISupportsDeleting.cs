namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// Deletes a resource.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public interface ISupportsDeleting<T>
  {
    #region Methods
    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.Byte ID);

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.Int16 ID);

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.Int32 ID);

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.Int64 ID);

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.Char ID);

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.String ID);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.Byte[] IDs);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.Int16[] IDs);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.Int32[] IDs);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.Int64[] IDs);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.Char[] IDs);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.String[] IDs);


    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.Byte ID);

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.Int16 ID);

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.Int32 ID);

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.Int64 ID);

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.Char ID);

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.String ID);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.Byte[] IDs);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.Int16[] IDs);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.Int32[] IDs);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.Int64[] IDs);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.Char[] IDs);

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public System.Threading.Tasks.Task DeleteAsync(System.String[] IDs);
    #endregion
  }
}