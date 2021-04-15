namespace SoftmakeAll.SDK.Fluent.ResourceActions
{
  public interface ISupportsDeleting<T>
  {
    #region Methods
    public void Delete(System.Byte ID);
    public void Delete(System.Int16 ID);
    public void Delete(System.Int32 ID);
    public void Delete(System.Int64 ID);
    public void Delete(System.Char ID);
    public void Delete(System.String ID);
    public void Delete(System.Byte[] IDs);
    public void Delete(System.Int16[] IDs);
    public void Delete(System.Int32[] IDs);
    public void Delete(System.Int64[] IDs);
    public void Delete(System.Char[] IDs);
    public void Delete(System.String[] IDs);

    public System.Threading.Tasks.Task DeleteAsync(System.Byte ID);
    public System.Threading.Tasks.Task DeleteAsync(System.Int16 ID);
    public System.Threading.Tasks.Task DeleteAsync(System.Int32 ID);
    public System.Threading.Tasks.Task DeleteAsync(System.Int64 ID);
    public System.Threading.Tasks.Task DeleteAsync(System.Char ID);
    public System.Threading.Tasks.Task DeleteAsync(System.String ID);
    public System.Threading.Tasks.Task DeleteAsync(System.Byte[] IDs);
    public System.Threading.Tasks.Task DeleteAsync(System.Int16[] IDs);
    public System.Threading.Tasks.Task DeleteAsync(System.Int32[] IDs);
    public System.Threading.Tasks.Task DeleteAsync(System.Int64[] IDs);
    public System.Threading.Tasks.Task DeleteAsync(System.Char[] IDs);
    public System.Threading.Tasks.Task DeleteAsync(System.String[] IDs);
    #endregion
  }
}