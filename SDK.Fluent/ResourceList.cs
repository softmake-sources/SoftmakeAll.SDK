namespace SoftmakeAll.SDK.Fluent
{
  public class ResourceList<T>
  {
    #region Constructor
    public ResourceList()
    {
      this.Aggregates = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.Text.Json.JsonElement>>();
      this.Result = new System.Collections.Generic.List<T>();
    }
    #endregion

    #region Properties
    public System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.Text.Json.JsonElement>> Aggregates { get; set; }
    public System.Collections.Generic.List<T> Result { get; set; }
    #endregion
  }
}