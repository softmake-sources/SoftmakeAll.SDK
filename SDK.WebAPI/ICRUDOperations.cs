﻿namespace SoftmakeAll.SDK.WebAPI
{
  public interface ICRUDOperations
  {
    #region Methods
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> CreateAsync(System.Text.Json.JsonElement Object);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> ReadAsync(System.String ID);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> ReadAsync(System.String Fields, System.String Filter, System.String Group, System.String Sort, System.Int32 Skip, System.Int32 Take);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> UpdatePartialAsync(System.String ID, System.Text.Json.JsonElement Object);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> UpdateAsync(System.String ID, System.Text.Json.JsonElement Object);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> DeleteAsync(System.String ID);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> DeleteAsync(System.Text.Json.JsonElement Array);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> ImportAsync(System.Collections.Generic.List<Microsoft.AspNetCore.Http.IFormFile> Files);
    public System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Data.DataSet>> ExportAsync(System.String Fields, System.String Filter, System.String Group, System.String Sort, System.Int32 Skip, System.Int32 Take);
    #endregion
  }
}