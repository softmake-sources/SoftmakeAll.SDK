namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public interface ISupportsCreating<T>
  {
    #region Methods
    public T Create(T Model);
    public System.Threading.Tasks.Task<T> CreateAsync(T Model);
    #endregion
  }
}