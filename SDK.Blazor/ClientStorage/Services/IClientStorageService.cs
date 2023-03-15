namespace SoftmakeAll.SDK.Blazor.ClientStorage.Services
{
  public interface IClientStorageService
  {
    #region Events
    public event System.EventHandler<SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs> OnChanging;
    public event System.EventHandler<SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangedEventArgs> OnChanged;
    #endregion

    #region Methods
    public System.Threading.Tasks.Task SetItemAsync<T>(System.String Key, T Value, System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task<T> GetItemAsync<T>(System.String Key, System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task RemoveItemAsync(System.String Key, System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task ClearAsync(System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task<System.Boolean> ContainsKeyAsync(System.String Key, System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task<System.String> KeyAsync(System.Int32 Index, System.Threading.CancellationToken CancellationToken = default);
    public System.Threading.Tasks.Task<System.Int32> CountAsync(System.Threading.CancellationToken CancellationToken = default);

    public void SetItem<T>(System.String Key, T Value);
    public T GetItem<T>(System.String Key);
    public void RemoveItem(System.String Key);
    public void Clear();
    public System.Boolean ContainsKey(System.String Key);
    public System.String Key(System.Int32 Index);
    public System.Int32 Count();
    #endregion
  }
  public interface ILocalStorageService : SoftmakeAll.SDK.Blazor.ClientStorage.Services.IClientStorageService { }
  public interface ISessionStorageService : SoftmakeAll.SDK.Blazor.ClientStorage.Services.IClientStorageService { }
}