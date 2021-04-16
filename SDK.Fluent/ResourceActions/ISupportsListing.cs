namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public interface ISupportsListing<T>
  {
    #region Methods
    public SoftmakeAll.SDK.Fluent.ResourceList<T> List(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      );

    public System.Threading.Tasks.Task<SoftmakeAll.SDK.Fluent.ResourceList<T>> ListAsync(
      System.Collections.Generic.Dictionary<System.String, System.String> Parameters = null,
      System.Collections.Generic.List<System.String> Fields = null,
      System.Collections.Generic.List<SoftmakeAll.SDK.Fluent.ResourceActions.ListFilter> Filter = null,
      System.Collections.Generic.List<System.String> Group = null,
      System.Collections.Generic.Dictionary<System.String, System.Boolean> Sort = null,
      System.Int32 Skip = 0, System.Int32 Take = 20
      );

    public T Show(System.Byte ID);
    public T Show(System.Int16 ID);
    public T Show(System.Int32 ID);
    public T Show(System.Int64 ID);
    public T Show(System.Char ID);
    public T Show(System.String ID);

    public System.Threading.Tasks.Task<T> ShowAsync(System.Byte ID);
    public System.Threading.Tasks.Task<T> ShowAsync(System.Int16 ID);
    public System.Threading.Tasks.Task<T> ShowAsync(System.Int32 ID);
    public System.Threading.Tasks.Task<T> ShowAsync(System.Int64 ID);
    public System.Threading.Tasks.Task<T> ShowAsync(System.Char ID);
    public System.Threading.Tasks.Task<T> ShowAsync(System.String ID);
    #endregion
  }
}