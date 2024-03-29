﻿using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// Supports updating resources.
  /// </summary>
  /// <typeparam name="T">The generic object that represents any resource.</typeparam>
  public class SupportsUpdating<T> : SoftmakeAll.SDK.Fluent.ResourceActions.SupportsBase<T>, SoftmakeAll.SDK.Fluent.ResourceActions.ISupportsUpdating<T>
  {
    #region Constructor
    /// <summary>
    /// Supports updating resources.
    /// </summary>
    /// <param name="Route">The address of the resource.</param>
    public SupportsUpdating(System.String Route) : base(Route) { }
    #endregion

    #region Methods
    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Byte ID, System.Object Model) => this.Modify(ID.ToString(), Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Int16 ID, System.Object Model) => this.Modify(ID.ToString(), Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Int32 ID, System.Object Model) => this.Modify(ID.ToString(), Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Int64 ID, System.Object Model) => this.Modify(ID.ToString(), Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.Char ID, System.Object Model) => this.Modify(ID.ToString(), Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public T Modify(System.String ID, System.Object Model)
    {
      if (System.String.IsNullOrWhiteSpace(ID))
        return default;

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult = SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequest(new SoftmakeAll.SDK.Communication.REST() { Method = "PATCH", URL = $"{base.Route}/{ID}", Body = Model.ToJsonElement() });

      if ((OperationResult.Success) && (OperationResult.Data.IsValid()))
        return OperationResult.Data[0].ToObject<T>();

      return default;
    }

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Byte ID, System.Object Model) => await this.ModifyAsync(ID.ToString(), Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int16 ID, System.Object Model) => await this.ModifyAsync(ID.ToString(), Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int32 ID, System.Object Model) => await this.ModifyAsync(ID.ToString(), Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Int64 ID, System.Object Model) => await this.ModifyAsync(ID.ToString(), Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.Char ID, System.Object Model) => await this.ModifyAsync(ID.ToString(), Model);

    /// <summary>
    /// Updates a resource partially.
    /// </summary>
    /// <param name="ID">The ID of the resource to be modified.</param>
    /// <param name="Model">The object that contains the fields and values ​​to be modified.</param>
    /// <returns>The modified resource.</returns>
    public async System.Threading.Tasks.Task<T> ModifyAsync(System.String ID, System.Object Model)
    {
      if (System.String.IsNullOrWhiteSpace(ID))
        return default;

      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult = await SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequestAsync(new SoftmakeAll.SDK.Communication.REST() { Method = "PATCH", URL = $"{base.Route}/{ID}", Body = Model.ToJsonElement() });

      if ((OperationResult.Success) && (OperationResult.Data.IsValid()))
        return OperationResult.Data[0].ToObject<T>();

      return default;
    }



    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.Byte ID, T Model) => this.Replace(ID.ToString(), Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.Int16 ID, T Model) => this.Replace(ID.ToString(), Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.Int32 ID, T Model) => this.Replace(ID.ToString(), Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.Int64 ID, T Model) => this.Replace(ID.ToString(), Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.Char ID, T Model) => this.Replace(ID.ToString(), Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.String ID, T Model) => this.Replace(ID, Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T ReplacePreventingOverwrite(System.Byte ID, T Model) => this.Replace(ID.ToString(), Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T ReplacePreventingOverwrite(System.Int16 ID, T Model) => this.Replace(ID.ToString(), Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T ReplacePreventingOverwrite(System.Int32 ID, T Model) => this.Replace(ID.ToString(), Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T ReplacePreventingOverwrite(System.Int64 ID, T Model) => this.Replace(ID.ToString(), Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T ReplacePreventingOverwrite(System.Char ID, T Model) => this.Replace(ID.ToString(), Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public T ReplacePreventingOverwrite(System.String ID, T Model) => this.Replace(ID, Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <param name="PreventingOverwrite">True: If the server resource model is newer, the system shows an error instead of update. False: Ignores the server resource model version and update it using the Model parameter values.</param>
    /// <returns>The replaced resource.</returns>
    public T Replace(System.String ID, T Model, System.Boolean PreventingOverwrite)
    {
      if (System.String.IsNullOrWhiteSpace(ID))
        return default;

      SoftmakeAll.SDK.Communication.REST REST = new SoftmakeAll.SDK.Communication.REST() { Method = "PUT", URL = $"{base.Route}/{ID}" };
      REST.Body = PreventingOverwrite ? Model.ToJsonElement() : Model.Serialize().Insert(1, "\"__Overwrite\": true, ").ToJsonElement();
      return base.ProcessOperationResult(SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequest(REST), Model);
    }

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Byte ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int16 ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int32 ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Int64 ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.Char ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.String ID, T Model) => await this.ReplaceAsync(ID, Model, false);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.Byte ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.Int16 ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.Int32 ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.Int64 ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.Char ID, T Model) => await this.ReplaceAsync(ID.ToString(), Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplacePreventingOverwriteAsync(System.String ID, T Model) => await this.ReplaceAsync(ID, Model, true);

    /// <summary>
    /// Updates a resource fully.
    /// </summary>
    /// <param name="ID">The ID of the resource to be replaced.</param>
    /// <param name="Model">The generic object that represents the new resource.</param>
    /// <param name="PreventingOverwrite">True: If the server resource model is newer, the system shows an error instead of update. False: Ignores the server resource model version and update it using the Model parameter values.</param>
    /// <returns>The replaced resource.</returns>
    public async System.Threading.Tasks.Task<T> ReplaceAsync(System.String ID, T Model, System.Boolean PreventingOverwrite)
    {
      if (System.String.IsNullOrWhiteSpace(ID))
        return default;

      SoftmakeAll.SDK.Communication.REST REST = new SoftmakeAll.SDK.Communication.REST() { Method = "PUT", URL = $"{base.Route}/{ID}" };
      REST.Body = PreventingOverwrite ? Model.ToJsonElement() : Model.Serialize().Insert(1, "\"__Overwrite\": true, ").ToJsonElement();
      return base.ProcessOperationResult(await SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequestAsync(REST), Model);
    }
    #endregion
  }
}