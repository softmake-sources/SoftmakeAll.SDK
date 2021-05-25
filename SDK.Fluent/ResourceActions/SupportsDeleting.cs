using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// Supports deleting resources.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public class SupportsDeleting<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsDeleting<T>
  {
    #region Constructor
    /// <summary>
    /// Supports deleting resources.
    /// </summary>
    /// <param name="Route">The address of the resource.</param>
    public SupportsDeleting(System.String Route) : base(Route) { }
    #endregion

    #region Methods
    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.Byte ID) => this.Delete(ID.ToString());

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.Int16 ID) => this.Delete(ID.ToString());

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.Int32 ID) => this.Delete(ID.ToString());

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.Int64 ID) => this.Delete(ID.ToString());

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.Char ID) => this.Delete(ID.ToString());

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public void Delete(System.String ID) => this.Delete(new System.String[] { ID });

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.Byte[] IDs) => this.Delete(System.Array.ConvertAll(IDs, ID => ID.ToString()));

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.Int16[] IDs) => this.Delete(System.Array.ConvertAll(IDs, ID => ID.ToString()));

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.Int32[] IDs) => this.Delete(System.Array.ConvertAll(IDs, ID => ID.ToString()));

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.Int64[] IDs) => this.Delete(System.Array.ConvertAll(IDs, ID => ID.ToString()));

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.Char[] IDs) => this.Delete(System.Array.ConvertAll(IDs, ID => ID.ToString()));

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public void Delete(System.String[] IDs)
    {
      if ((IDs != null) && (IDs.Any()))
        SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequest(new SoftmakeAll.SDK.Communication.REST() { Method = "DELETE", URL = base.Route, Body = IDs.ToJsonElement() });
    }

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.Byte ID) => await this.DeleteAsync(ID.ToString());

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.Int16 ID) => await this.DeleteAsync(ID.ToString());

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.Int32 ID) => await this.DeleteAsync(ID.ToString());

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.Int64 ID) => await this.DeleteAsync(ID.ToString());

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.Char ID) => await this.DeleteAsync(ID.ToString());

    /// <summary>
    /// Deletes a existing resource.
    /// </summary>
    /// <param name="ID">The ID of the resource to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.String ID) => await this.DeleteAsync(new System.String[] { ID });

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.Byte[] IDs) => await this.DeleteAsync(System.Array.ConvertAll(IDs, ID => ID.ToString()));

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.Int16[] IDs) => await this.DeleteAsync(System.Array.ConvertAll(IDs, ID => ID.ToString()));

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.Int32[] IDs) => await this.DeleteAsync(System.Array.ConvertAll(IDs, ID => ID.ToString()));

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.Int64[] IDs) => await this.DeleteAsync(System.Array.ConvertAll(IDs, ID => ID.ToString()));

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.Char[] IDs) => await this.DeleteAsync(System.Array.ConvertAll(IDs, ID => ID.ToString()));

    /// <summary>
    /// Deletes a multiple existing resources.
    /// </summary>
    /// <param name="IDs">The IDs of the resources to be deleted.</param>
    public async System.Threading.Tasks.Task DeleteAsync(System.String[] IDs)
    {
      if ((IDs != null) && (IDs.Any()))
        await SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequestAsync(new SoftmakeAll.SDK.Communication.REST() { Method = "DELETE", URL = base.Route, Body = IDs.ToJsonElement() });
    }
    #endregion
  }
}