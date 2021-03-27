using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.Fluent
{
  public partial class CRUD<T>
  {
    #region Fields
    public readonly SoftmakeAll.SDK.OperationResult LastOperationResult;
    #endregion

    #region Constructor
    public CRUD() => this.LastOperationResult = new SoftmakeAll.SDK.OperationResult();
    #endregion

    #region Methods
    private System.String GenerateBaseURL()
    {
      const System.String BaseAddress = "API/v";
      System.Type Type = typeof(T);
      System.String Module = Type.FullName.Split('.')[3].ToLower();
      SoftmakeAll.SDK.Fluent.DataAnnotations.EntryPoint EntryPoint = SoftmakeAll.SDK.Fluent.DataAnnotations.EntryPoint.FromType(Type);
      if (EntryPoint != null)
        return $"{BaseAddress}{EntryPoint.Version}/{Module}/{EntryPoint.Name}";
      return $"{BaseAddress}1/{Module}/{Type.Name}";
    }
    private System.String GenerateListURL(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      )
    {
      System.String URL = $"{this.GenerateBaseURL()}?skip={Skip}&take={Take}";

      if ((Parameters != null) && (Parameters.Any())) URL = $"{URL}&{System.String.Join('&', Parameters.Select(p => $"{p.Key}={p.Value}"))}";
      if ((Fields != null) && (Fields.Any())) URL = $"{URL}&fields={System.String.Join(',', Fields)}";
      if ((Group != null) && (Group.Any())) URL = $"{URL}&group={System.String.Join(',', Group)}";
      if ((Sort != null) && (Sort.Any())) URL = $"{URL}&sort={System.String.Join('&', Sort.Select(p => $"{(p.Value ? '-' : '+')}{p.Key}"))}";

      return URL;
    }

    public SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> MakeRESTRequest(SoftmakeAll.SDK.Communication.REST REST) => SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequest<T>(REST);
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> MakeRESTRequestAsync(SoftmakeAll.SDK.Communication.REST REST) => await SoftmakeAll.SDK.Fluent.SDKContext.MakeRESTRequestAsync<T>(REST);

    private void SetLastOperationResult(SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult)
    {
      this.LastOperationResult.ExitCode = OperationResult.ExitCode;
      this.LastOperationResult.Message = OperationResult.Message;
      this.LastOperationResult.Count = OperationResult.Count;
      this.LastOperationResult.ID = OperationResult.ID;
    }
    private T ProcessOperationResult(SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult, T Model)
    {
      this.SetLastOperationResult(OperationResult);

      if ((OperationResult.Success) && (OperationResult.Data.IsValid()))
        return OperationResult.Data[0].ToObject<T>();

      return Model;
    }
    private SoftmakeAll.SDK.Fluent.ResourceList<T> ProcessOperationResult(SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> OperationResult)
    {
      this.SetLastOperationResult(OperationResult);

      if ((OperationResult.Success) && (OperationResult.Data.IsValid()))
        return OperationResult.Data.ToObject<SoftmakeAll.SDK.Fluent.ResourceList<T>>();

      return null;
    }
    #endregion
  }
}