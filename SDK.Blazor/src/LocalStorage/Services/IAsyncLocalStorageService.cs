namespace SoftmakeAll.SDK.Blazor.LocalStorage.Services
{
  public interface IAsyncLocalStorageService
  {
    #region Events
    public event System.EventHandler<SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangingEventArgs> OnChanging;
    public event System.EventHandler<SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangedEventArgs> OnChanged;
    #endregion

    #region Methods
    public System.Threading.Tasks.Task SetItemAsync<T>(System.String Key, T Value);
    public System.Threading.Tasks.Task<T> GetItemAsync<T>(System.String Key);
    public System.Threading.Tasks.Task RemoveItemAsync(System.String Key);
    public System.Threading.Tasks.Task ClearAsync();
    public System.Threading.Tasks.Task<System.Boolean> ContainsKeyAsync(System.String Key);
    public System.Threading.Tasks.Task<System.String> KeyAsync(System.Int32 Index);
    public System.Threading.Tasks.Task<System.Int32> LengthAsync();
    #endregion
  }
}