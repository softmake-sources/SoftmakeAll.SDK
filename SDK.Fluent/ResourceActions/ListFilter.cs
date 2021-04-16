﻿namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public class ListFilter
  {
    #region Constructor
    public ListFilter() { }
    #endregion

    #region Properties
    public System.String Condition { get; set; }
    public System.String Field { get; set; }
    public System.String Operator { get; set; }
    public System.Object Value { get; set; }
    public System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Predicates { get; set; }
    #endregion
  }
}