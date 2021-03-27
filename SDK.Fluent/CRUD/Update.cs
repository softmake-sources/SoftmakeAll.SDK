using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Fluent
{
  public partial class CRUD<T>
  {
    #region Methods
    public T Modify(System.Byte ID, System.Object Model) => this.Modify(ID.ToString(), Model);
    public T Modify(System.Int16 ID, System.Object Model) => this.Modify(ID.ToString(), Model);
    public T Modify(System.Int32 ID, System.Object Model) => this.Modify(ID.ToString(), Model);
    public T Modify(System.Int64 ID, System.Object Model) => this.Modify(ID.ToString(), Model);
    public T Modify(System.Char ID, System.Object Model) => this.Modify(ID.ToString(), Model);
    public T Modify(System.String ID, System.Object Model)
    {
      if (System.String.IsNullOrWhiteSpace(ID))
        return default;

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult = SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequest<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "PATCH", URL = $"{this.GenerateBaseURL()}/{ID}", Body = Model.ToJsonElement() });

      this.SetLastOperationResult(OperationResult);

      if ((OperationResult.Success) && (OperationResult.Data.IsValid()))
        return OperationResult.Data[0].ToObject<T>();

      return default;
    }

    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Byte ID, System.Object Model) => await this.ModifyAsync(ID.ToString(), Model);
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int16 ID, System.Object Model) => await this.ModifyAsync(ID.ToString(), Model);
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int32 ID, System.Object Model) => await this.ModifyAsync(ID.ToString(), Model);
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int64 ID, System.Object Model) => await this.ModifyAsync(ID.ToString(), Model);
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Char ID, System.Object Model) => await this.ModifyAsync(ID.ToString(), Model);
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.String ID, System.Object Model)
    {
      if (System.String.IsNullOrWhiteSpace(ID))
        return default;

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult = await SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequestAsync<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "PATCH", URL = $"{this.GenerateBaseURL()}/{ID}", Body = Model.ToJsonElement() });

      this.SetLastOperationResult(OperationResult);

      if ((OperationResult.Success) && (OperationResult.Data.IsValid()))
        return OperationResult.Data[0].ToObject<T>();

      return default;
    }

    public T Replace(System.Byte ID, T Model) => this.Replace(ID.ToString(), Model);
    public T Replace(System.Int16 ID, T Model) => this.Replace(ID.ToString(), Model);
    public T Replace(System.Int32 ID, T Model) => this.Replace(ID.ToString(), Model);
    public T Replace(System.Int64 ID, T Model) => this.Replace(ID.ToString(), Model);
    public T Replace(System.Char ID, T Model) => this.Replace(ID.ToString(), Model);
    public T Replace(System.String ID, T Model)
    {
      if (System.String.IsNullOrWhiteSpace(ID))
        return default;

      return this.ProcessOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequest<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "PUT", URL = $"{this.GenerateBaseURL()}/{ID}", Body = Model.ToJsonElement() }), Model);
    }

    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Byte ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model);
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int16 ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model);
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int32 ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model);
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int64 ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model);
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Char ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model);
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.String ID, T Model)
    {
      if (System.String.IsNullOrWhiteSpace(ID))
        return default;

      return this.ProcessOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequestAsync<T>(new SoftmakeAll.SDK.Communication.REST() { Method = "PUT", URL = $"{this.GenerateBaseURL()}/{ID}", Body = Model.ToJsonElement() }), Model);
    }
    #endregion
  }
}