namespace SoftmakeAll.SDK.Fluent
{
  /// <summary>
  /// A generic list of resources. Used to catch the List methods output.
  /// </summary>
  /// <typeparam name="T">Resource.</typeparam>
  public class ResourceList<T>
  {
    #region Constructor
    /// <summary>
    /// A generic list of resources. Used to catch the List methods output.
    /// </summary>
    public ResourceList()
    {
      this.Aggregates = new System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.Text.Json.JsonElement>>();
      this.Result = new System.Collections.Generic.List<T>();
    }
    #endregion

    #region Properties
    /// <summary>
    /// The list of aggregates about the resources. E.g.: The AVG/COUNT/SUM of specific property.
    /// </summary>
    public System.Collections.Generic.List<System.Collections.Generic.Dictionary<System.String, System.Text.Json.JsonElement>> Aggregates { get; set; }

    /// <summary>
    /// The list of resources.
    /// </summary>
    public System.Collections.Generic.List<T> Result { get; set; }
    #endregion
  }
}