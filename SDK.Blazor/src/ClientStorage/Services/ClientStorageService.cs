using Microsoft.JSInterop;

namespace SoftmakeAll.SDK.Blazor.ClientStorage.Services
{
  public abstract class ClientStorageService : SoftmakeAll.SDK.Blazor.ClientStorage.Services.IAsyncClientStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.ISyncClientStorageService
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

    #region Properties
    private System.String StorageTypeName = "";
    protected SoftmakeAll.SDK.Blazor.ClientStorage.StorageTypes StorageType
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
    private System.String SetItemAction => StorageTypeName + "set" + SuffixName;
    private System.String GetItemAction => StorageTypeName + "get" + SuffixName;
    private System.String RemoveItemAction => StorageTypeName + "remove" + SuffixName;
    private System.String ClearAction => StorageTypeName + "clear";
    private System.String ContainsKeyProperty => StorageTypeName + "hasOwnProperty";
    private System.String KeyProperty => StorageTypeName + "key";
    private System.String LengthProperty => StorageTypeName + "length";
    #endregion

    #region Methods
    private T ConvertSerializedValue<T>(System.String SerializedValue)
    {
      if (System.String.IsNullOrWhiteSpace(SerializedValue))
        return default;

      return (T)(System.Object)SerializedValue;
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
    private void ValidateJSRuntime() { if (this.JSRuntime == null) throw new System.InvalidOperationException("JSRuntime not available."); }
    private void ValidateJSRuntime(System.String Key) { this.ValidateJSRuntime(); if (System.String.IsNullOrWhiteSpace(Key)) throw new System.ArgumentNullException("The Key parameter cannot be null or empty."); }
    private async System.Threading.Tasks.Task<SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs> RaiseOnChangingAsync(System.String Key, System.Object Value)
    {
      SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs ChangingEventArgs = new SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs();
      ChangingEventArgs.Key = Key;
      ChangingEventArgs.OldValue = await this.GetItemAsync<System.Object>(Key);
      ChangingEventArgs.NewValue = Value;
      this.OnChanging?.Invoke(this, ChangingEventArgs);
      return ChangingEventArgs;
    }
    public async System.Threading.Tasks.Task SetItemAsync<T>(System.String Key, T Value)
    {
      this.ValidateJSRuntime(Key);

      SoftmakeAll.SDK.Blazor.ClientStorage.EventArgs.ChangingEventArgs ChangingEventArgs = await this.RaiseOnChangingAsync(Key, Value);
      if (ChangingEventArgs.Cancel) return;

      await this.JSRuntime.InvokeVoidAsync(SetItemAction, Key, Value);

      this.RaiseOnChanged(Key, ChangingEventArgs.OldValue, Value);
    }
    public async System.Threading.Tasks.Task<T> GetItemAsync<T>(System.String Key) { this.ValidateJSRuntime(Key); return this.ConvertSerializedValue<T>(await this.JSRuntime.InvokeAsync<System.String>(GetItemAction, Key)); }
    public async System.Threading.Tasks.Task RemoveItemAsync(System.String Key) { this.ValidateJSRuntime(Key); await this.JSRuntime.InvokeVoidAsync(RemoveItemAction, Key); }
    public async System.Threading.Tasks.Task ClearAsync() => await this.JSRuntime.InvokeVoidAsync(ClearAction);
    public async System.Threading.Tasks.Task<System.Boolean> ContainsKeyAsync(System.String Key) => await this.JSRuntime.InvokeAsync<System.Boolean>(ContainsKeyProperty, Key);
    public async System.Threading.Tasks.Task<System.String> KeyAsync(System.Int32 Index) => await this.JSRuntime.InvokeAsync<System.String>(KeyProperty, Index);
    public async System.Threading.Tasks.Task<System.Int32> LengthAsync() => await this.JSRuntime.InvokeAsync<System.Int32>("eval", LengthProperty);
    #endregion

    #region ISyncClientStorageService
    private void ValidateJSInProcessRuntime() { if (this.JSInProcessRuntime == null) throw new System.InvalidOperationException("JSInProcessRuntime not available."); }
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

      this.JSInProcessRuntime.InvokeVoid(SetItemAction, Key, Value);

      this.RaiseOnChanged(Key, ChangingEventArgs.OldValue, Value);
    }
    public T GetItem<T>(System.String Key) { this.ValidateJSInProcessRuntime(Key); return this.ConvertSerializedValue<T>(this.JSInProcessRuntime.Invoke<System.String>(GetItemAction, Key)); }
    public void RemoveItem(System.String Key) { this.ValidateJSInProcessRuntime(Key); this.JSInProcessRuntime.InvokeVoid(RemoveItemAction, Key); }
    public void Clear() { this.ValidateJSInProcessRuntime(); this.JSInProcessRuntime.InvokeVoid(ClearAction); }
    public System.Boolean ContainsKey(System.String Key) { this.ValidateJSInProcessRuntime(Key); return this.JSInProcessRuntime.Invoke<System.Boolean>(ContainsKeyProperty, Key); }
    public System.String Key(System.Int32 Index) { this.ValidateJSInProcessRuntime(); return this.JSInProcessRuntime.Invoke<System.String>(KeyProperty, Index); }
    public System.Int32 Length() { this.ValidateJSInProcessRuntime(); return this.JSInProcessRuntime.Invoke<System.Int32>("eval", LengthProperty); }
    #endregion
    #endregion
  }
  public class LocalStorageService : SoftmakeAll.SDK.Blazor.ClientStorage.Services.ClientStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.IAsyncLocalStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.ISyncLocalStorageService
  {
    public LocalStorageService(Microsoft.JSInterop.IJSRuntime JSRuntimeContext) : base(JSRuntimeContext)
    {
      base.StorageType = SoftmakeAll.SDK.Blazor.ClientStorage.StorageTypes.LocalStorage;
    }
  }
  public class SessionStorageService : SoftmakeAll.SDK.Blazor.ClientStorage.Services.ClientStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.IAsyncSessionStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.ISyncSessionStorageService
  {
    public SessionStorageService(Microsoft.JSInterop.IJSRuntime JSRuntimeContext) : base(JSRuntimeContext)
    {
      base.StorageType = SoftmakeAll.SDK.Blazor.ClientStorage.StorageTypes.SessionStorage;
    }
  }
}