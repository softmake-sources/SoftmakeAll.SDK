namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public interface ISupportsUpdating<T>
  {
    #region Methods
    public T Modify(System.Byte ID, System.Object Model);
    public T Modify(System.Int16 ID, System.Object Model);
    public T Modify(System.Int32 ID, System.Object Model);
    public T Modify(System.Int64 ID, System.Object Model);
    public T Modify(System.Char ID, System.Object Model);
    public T Modify(System.String ID, System.Object Model);

    public System.Threading.Tasks.Task<T> ModifyAsync(System.Byte ID, System.Object Model);
    public System.Threading.Tasks.Task<T> ModifyAsync(System.Int16 ID, System.Object Model);
    public System.Threading.Tasks.Task<T> ModifyAsync(System.Int32 ID, System.Object Model);
    public System.Threading.Tasks.Task<T> ModifyAsync(System.Int64 ID, System.Object Model);
    public System.Threading.Tasks.Task<T> ModifyAsync(System.Char ID, System.Object Model);
    public System.Threading.Tasks.Task<T> ModifyAsync(System.String ID, System.Object Model);

    public T Replace(System.Byte ID, T Model);
    public T Replace(System.Int16 ID, T Model);
    public T Replace(System.Int32 ID, T Model);
    public T Replace(System.Int64 ID, T Model);
    public T Replace(System.Char ID, T Model);
    public T Replace(System.String ID, T Model);

    public System.Threading.Tasks.Task<T> ReplaceAsync(System.Byte ID, T Model);
    public System.Threading.Tasks.Task<T> ReplaceAsync(System.Int16 ID, T Model);
    public System.Threading.Tasks.Task<T> ReplaceAsync(System.Int32 ID, T Model);
    public System.Threading.Tasks.Task<T> ReplaceAsync(System.Int64 ID, T Model);
    public System.Threading.Tasks.Task<T> ReplaceAsync(System.Char ID, T Model);
    public System.Threading.Tasks.Task<T> ReplaceAsync(System.String ID, T Model);
    #endregion
  }
}