using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public abstract class SupportsBase<T>
  {
    #region Fields
    protected readonly System.String Route;
    #endregion

    #region Constructor
    public SupportsBase(System.String Route) => this.Route = Route;
    #endregion

    #region Methods
    protected internal void SetLastOperationResult(SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult)
    {
      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.ExitCode = OperationResult.ExitCode;
      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.Message = OperationResult.Message;
      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.Count = OperationResult.Count;
      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.ID = OperationResult.ID;
    }
    protected internal T ProcessOperationResult(SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult, T Model)
    {
      this.SetLastOperationResult(OperationResult);

      if ((OperationResult.Success) && (OperationResult.Data.IsValid()))
        return OperationResult.Data[0].ToObject<T>();

      return Model;
    }
    protected internal SoftmakeAll.SDK.Fluent.ResourceList<T> ProcessOperationResult(SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult)
    {
      this.SetLastOperationResult(OperationResult);

      if ((OperationResult.Success) && (OperationResult.Data.IsValid()))
        return OperationResult.Data.ToObject<SoftmakeAll.SDK.Fluent.ResourceList<T>>();

      return new SoftmakeAll.SDK.Fluent.ResourceList<T>();
    }
    #endregion
  }
}