using Microsoft.JSInterop;

namespace SoftmakeAll.SDK.Blazor.ClientStorage.Services
{
  public abstract class ClientStorageService : SoftmakeAll.SDK.Blazor.ClientStorage.Services.IClientStorageService
  {
    #region Fields
    private readonly Microsoft.JSInterop.IJSRuntime JSRuntime;
    private readonly Microsoft.JSInterop.IJSInProcessRuntime JSInProcessRuntime;
    #endregion

    #region Constructor
    public ClientStorageService(Microsoft.JSInterop.IJSRuntime JSRuntimeContext)
    {
      this.JSRuntime = JSRuntimeContext;
      this.JSInProcessRuntime = JSRuntimeContext as Microsoft.JSInterop.IJSInProcessRuntime;
    }
    #endregion

    #region Events
    public event System.EventHandler<SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs> OnChanging;
    public event System.EventHandler<SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangedEventArgs> OnChanged;
    #endregion

    #region Private Properties
    private System.String StorageTypeName = "";
    protected internal SoftmakeAll.SDK.Blazor.ClientStorage.StorageTypes StorageType
    {
      get
      {
        switch (this.StorageTypeName)
        {
          case "localStorage.": return SoftmakeAll.SDK.Blazor.ClientStorage.StorageTypes.LocalStorage;
          case "sessionStorage.": return SoftmakeAll.SDK.Blazor.ClientStorage.StorageTypes.SessionStorage;
        }
        throw new System.Exception("Invalid StorageType. Valid types: LocalStorage or SessionStorage.");
      }
      set
      {
        switch (value)
        {
          case SoftmakeAll.SDK.Blazor.ClientStorage.StorageTypes.LocalStorage: this.StorageTypeName = "localStorage."; return;
          case SoftmakeAll.SDK.Blazor.ClientStorage.StorageTypes.SessionStorage: this.StorageTypeName = "sessionStorage."; return;
        }
        throw new System.Exception("Invalid StorageType. Valid types: LocalStorage or SessionStorage.");
      }
    }
    private System.String SuffixName => "Item";
    private System.String SetItemAction => $"{this.StorageTypeName}set{this.SuffixName}";
    private System.String GetItemAction => $"{this.StorageTypeName}get{this.SuffixName}";
    private System.String RemoveItemAction => $"{this.StorageTypeName}remove{this.SuffixName}";
    private System.String ClearAction => $"{this.StorageTypeName}clear";
    private System.String ContainsKeyProperty => $"{this.StorageTypeName}hasOwnProperty";
    private System.String KeyProperty => $"{this.StorageTypeName}key";
    private System.String CountProperty => $"{this.StorageTypeName}length";
    #endregion

    #region Methods
    private T ConvertSerializedValue<T>(System.String SerializedValue)
    {
      if (System.String.IsNullOrWhiteSpace(SerializedValue))
        return default;

      return (T)((System.Object)SerializedValue);
    }
    private void RaiseOnChanged(System.String Key, System.Object OldValue, System.Object NewValue)
    {
      SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangedEventArgs ChangedEventArgs = new SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangedEventArgs();
      ChangedEventArgs.Key = Key;
      ChangedEventArgs.OldValue = OldValue;
      ChangedEventArgs.NewValue = NewValue;
      this.OnChanged?.Invoke(this, ChangedEventArgs);
    }

    #region IAsyncClientStorageService
    private void ValidateJSRuntime() { if (this.JSRuntime == null) throw new System.InvalidOperationException("JSRuntime is not available."); }
    private void ValidateJSRuntime(System.String Key) { this.ValidateJSRuntime(); if (System.String.IsNullOrWhiteSpace(Key)) throw new System.ArgumentNullException("The Key parameter cannot be null or empty."); }
    private async System.Threading.Tasks.Task<SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs> RaiseOnChangingAsync(System.String Key, System.Object Value, System.Threading.CancellationToken CancellationToken = default)
    {
      SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs ChangingEventArgs = new SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs();
      ChangingEventArgs.Key = Key;
      ChangingEventArgs.OldValue = await this.GetItemAsync<System.Object>(Key, CancellationToken);
      ChangingEventArgs.NewValue = Value;
      this.OnChanging?.Invoke(this, ChangingEventArgs);
      return ChangingEventArgs;
    }
    public async System.Threading.Tasks.Task SetItemAsync<T>(System.String Key, T Value, System.Threading.CancellationToken CancellationToken = default)
    {
      this.ValidateJSRuntime(Key);

      SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs ChangingEventArgs = await this.RaiseOnChangingAsync(Key, Value, CancellationToken);
      if (ChangingEventArgs.Cancel) return;

      await this.JSRuntime.InvokeVoidAsync(this.SetItemAction, CancellationToken, Key, Value);

      this.RaiseOnChanged(Key, ChangingEventArgs.OldValue, Value);
    }
    public async System.Threading.Tasks.Task<T> GetItemAsync<T>(System.String Key, System.Threading.CancellationToken CancellationToken = default) { this.ValidateJSRuntime(Key); return this.ConvertSerializedValue<T>(await this.JSRuntime.InvokeAsync<System.String>(this.GetItemAction, CancellationToken, Key)); }
    public async System.Threading.Tasks.Task RemoveItemAsync(System.String Key, System.Threading.CancellationToken CancellationToken = default) { this.ValidateJSRuntime(Key); await this.JSRuntime.InvokeVoidAsync(this.RemoveItemAction, CancellationToken, Key); }
    public async System.Threading.Tasks.Task ClearAsync(System.Threading.CancellationToken CancellationToken = default) => await this.JSRuntime.InvokeVoidAsync(this.ClearAction, CancellationToken, null);
    public async System.Threading.Tasks.Task<System.Boolean> ContainsKeyAsync(System.String Key, System.Threading.CancellationToken CancellationToken = default) => await this.JSRuntime.InvokeAsync<System.Boolean>(this.ContainsKeyProperty, CancellationToken, Key);
    public async System.Threading.Tasks.Task<System.String> KeyAsync(System.Int32 Index, System.Threading.CancellationToken CancellationToken = default) => await this.JSRuntime.InvokeAsync<System.String>(this.KeyProperty, CancellationToken, Index);
    public async System.Threading.Tasks.Task<System.Int32> CountAsync(System.Threading.CancellationToken CancellationToken = default) => await this.JSRuntime.InvokeAsync<System.Int32>("eval", CancellationToken, this.CountProperty);
    #endregion

    #region ISyncClientStorageService
    private void ValidateJSInProcessRuntime() { if (this.JSInProcessRuntime == null) throw new System.InvalidOperationException("JSInProcessRuntime is not available."); }
    private void ValidateJSInProcessRuntime(System.String Key) { this.ValidateJSInProcessRuntime(); if (System.String.IsNullOrWhiteSpace(Key)) throw new System.ArgumentNullException("The Key parameter cannot be null or empty."); }
    private SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs RaiseOnChanging(System.String Key, System.Object Value)
    {
      SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs ChangingEventArgs = new SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs();
      ChangingEventArgs.Key = Key;
      ChangingEventArgs.OldValue = this.GetItem<System.Object>(Key);
      ChangingEventArgs.NewValue = Value;
      this.OnChanging?.Invoke(this, ChangingEventArgs);
      return ChangingEventArgs;
    }
    public void SetItem<T>(System.String Key, T Value)
    {
      this.ValidateJSInProcessRuntime(Key);

      SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs ChangingEventArgs = this.RaiseOnChanging(Key, Value);
      if (ChangingEventArgs.Cancel) return;

      this.JSInProcessRuntime.InvokeVoid(this.SetItemAction, Key, Value);

      this.RaiseOnChanged(Key, ChangingEventArgs.OldValue, Value);
    }
    public T GetItem<T>(System.String Key) { this.ValidateJSInProcessRuntime(Key); return this.ConvertSerializedValue<T>(this.JSInProcessRuntime.Invoke<System.String>(this.GetItemAction, Key)); }
    public void RemoveItem(System.String Key) { this.ValidateJSInProcessRuntime(Key); this.JSInProcessRuntime.InvokeVoid(this.RemoveItemAction, Key); }
    public void Clear() { this.ValidateJSInProcessRuntime(); this.JSInProcessRuntime.InvokeVoid(this.ClearAction); }
    public System.Boolean ContainsKey(System.String Key) { this.ValidateJSInProcessRuntime(Key); return this.JSInProcessRuntime.Invoke<System.Boolean>(this.ContainsKeyProperty, Key); }
    public System.String Key(System.Int32 Index) { this.ValidateJSInProcessRuntime(); return this.JSInProcessRuntime.Invoke<System.String>(this.KeyProperty, Index); }
    public System.Int32 Count() { this.ValidateJSInProcessRuntime(); return this.JSInProcessRuntime.Invoke<System.Int32>("eval", this.CountProperty); }
    #endregion
    #endregion
  }
  public class LocalStorageService : SoftmakeAll.SDK.Blazor.ClientStorage.Services.ClientStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.ILocalStorageService
  {
    public LocalStorageService(Microsoft.JSInterop.IJSRuntime JSRuntimeContext) : base(JSRuntimeContext)
    {
      base.StorageType = SoftmakeAll.SDK.Blazor.ClientStorage.StorageTypes.LocalStorage;
    }
  }
  public class SessionStorageService : SoftmakeAll.SDK.Blazor.ClientStorage.Services.ClientStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.ISessionStorageService
  {
    public SessionStorageService(Microsoft.JSInterop.IJSRuntime JSRuntimeContext) : base(JSRuntimeContext)
    {
      base.StorageType = SoftmakeAll.SDK.Blazor.ClientStorage.StorageTypes.SessionStorage;
    }
  }
}