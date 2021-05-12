using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public class SupportsDeleting<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsDeleting<T>
  {
    #region Constructor
    public SupportsDeleting(System.String Route) : base(Route) { }
    #endregion

    #region Methods
    public void Delete(System.Byte ID) => this.Delete(ID.ToString());
    public void Delete(System.Int16 ID) => this.Delete(ID.ToString());
    public void Delete(System.Int32 ID) => this.Delete(ID.ToString());
    public void Delete(System.Int64 ID) => this.Delete(ID.ToString());
    public void Delete(System.Char ID) => this.Delete(ID.ToString());
    public void Delete(System.String ID) => this.Delete(new System.String[] { ID });
    public void Delete(System.Byte[] IDs) => this.Delete(System.Array.ConvertAll(IDs, ID => ID.ToString()));
    public void Delete(System.Int16[] IDs) => this.Delete(System.Array.ConvertAll(IDs, ID => ID.ToString()));
    public void Delete(System.Int32[] IDs) => this.Delete(System.Array.ConvertAll(IDs, ID => ID.ToString()));
    public void Delete(System.Int64[] IDs) => this.Delete(System.Array.ConvertAll(IDs, ID => ID.ToString()));
    public void Delete(System.Char[] IDs) => this.Delete(System.Array.ConvertAll(IDs, ID => ID.ToString()));
    public void Delete(System.String[] IDs)
    {
      if ((IDs != null) && (IDs.Any()))
        SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequest(new SoftmakeAll.SDK.Communication.REST() { Method = "DELETE", URL = base.Route, Body = IDs.ToJsonElement() });
    }

    public async System.Threading.Tasks.Task DeleteAsync(System.Byte ID) => await this.DeleteAsync(ID.ToString());
    public async System.Threading.Tasks.Task DeleteAsync(System.Int16 ID) => await this.DeleteAsync(ID.ToString());
    public async System.Threading.Tasks.Task DeleteAsync(System.Int32 ID) => await this.DeleteAsync(ID.ToString());
    public async System.Threading.Tasks.Task DeleteAsync(System.Int64 ID) => await this.DeleteAsync(ID.ToString());
    public async System.Threading.Tasks.Task DeleteAsync(System.Char ID) => await this.DeleteAsync(ID.ToString());
    public async System.Threading.Tasks.Task DeleteAsync(System.String ID) => await this.DeleteAsync(new System.String[] { ID });
    public async System.Threading.Tasks.Task DeleteAsync(System.Byte[] IDs) => await this.DeleteAsync(System.Array.ConvertAll(IDs, ID => ID.ToString()));
    public async System.Threading.Tasks.Task DeleteAsync(System.Int16[] IDs) => await this.DeleteAsync(System.Array.ConvertAll(IDs, ID => ID.ToString()));
    public async System.Threading.Tasks.Task DeleteAsync(System.Int32[] IDs) => await this.DeleteAsync(System.Array.ConvertAll(IDs, ID => ID.ToString()));
    public async System.Threading.Tasks.Task DeleteAsync(System.Int64[] IDs) => await this.DeleteAsync(System.Array.ConvertAll(IDs, ID => ID.ToString()));
    public async System.Threading.Tasks.Task DeleteAsync(System.Char[] IDs) => await this.DeleteAsync(System.Array.ConvertAll(IDs, ID => ID.ToString()));
    public async System.Threading.Tasks.Task DeleteAsync(System.String[] IDs)
    {
      if ((IDs != null) && (IDs.Any()))
        await SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequestAsync(new SoftmakeAll.SDK.Communication.REST() { Method = "DELETE", URL = base.Route, Body = IDs.ToJsonElement() });
    }
    #endregion
  }
}