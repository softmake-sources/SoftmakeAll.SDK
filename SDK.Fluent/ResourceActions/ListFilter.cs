namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  /// <summary>
  /// The object to use to filter data on HTTP GET method.
  /// </summary>
  public class ListFilter
  {
    #region Constructor
    /// <summary>
    /// The object to use to filter data on HTTP GET method.
    /// </summary>
    public ListFilter() => this.Predicates = new System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter>();
    #endregion

    #region Properties
    /// <summary>
    /// Condition: AND, OR
    /// </summary>
    public System.String Condition { get; set; }

    /// <summary>
    /// Field name
    /// </summary>
    public System.String Field { get; set; }

    /// <summary>
    /// Operator: eq, lt, leq, gt, geq, ...
    /// </summary>
    public System.String Operator { get; set; }

    /// <summary>
    /// Value
    /// </summary>
    public System.Object Value { get; set; }

    /// <summary>
    /// Nested predicates
    /// </summary>
    public System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Predicates { get; set; }
    #endregion
  }
}