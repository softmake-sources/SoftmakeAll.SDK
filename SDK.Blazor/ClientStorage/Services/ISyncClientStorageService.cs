namespace SoftmakeAll.SDK.Blazor.ClientStorage.Services
{
  public interface ISyncClientStorageService
  {
    #region Events
    public event System.EventHandler<SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs> OnChanging;
    public event System.EventHandler<SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangedEventArgs> OnChanged;
    #endregion

    #region Methods
    public void SetItem<T>(System.String Key, T Value);
    public T GetItem<T>(System.String Key);
    public void RemoveItem(System.String Key);
    public void Clear();
    public System.Boolean ContainsKey(System.String Key);
    public System.String Key(System.Int32 Index);
    public System.Int32 Length();
    #endregion
  }
  public interface ISyncLocalStorageService : SoftmakeAll.SDK.Blazor.ClientStorage.Services.ISyncClientStorageService { }
  public interface ISyncSessionStorageService : SoftmakeAll.SDK.Blazor.ClientStorage.Services.ISyncClientStorageService { }
}