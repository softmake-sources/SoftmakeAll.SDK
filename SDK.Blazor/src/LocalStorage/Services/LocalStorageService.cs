using Microsoft.JSInterop;

namespace SoftmakeAll.SDK.Blazor.LocalStorage.Services
{
  public class LocalStorageService : SoftmakeAll.SDK.Blazor.LocalStorage.Services.IAsyncLocalStorageService, SoftmakeAll.SDK.Blazor.LocalStorage.Services.ISyncLocalStorageService
  {
    #region Fields
    private readonly Microsoft.JSInterop.IJSRuntime JSRuntime;
    private readonly Microsoft.JSInterop.IJSInProcessRuntime JSInProcessRuntime;
    private readonly System.Text.Json.JsonSerializerOptions JsonSerializerOptions;
    #endregion

    #region Constructor
    public LocalStorageService(Microsoft.JSInterop.IJSRuntime JSRuntimeContext)
    {
      this.JSRuntime = JSRuntimeContext;
      this.JSInProcessRuntime = JSRuntimeContext as Microsoft.JSInterop.IJSInProcessRuntime;
      this.JsonSerializerOptions = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.CreateJsonSerializerOptions(false, true);
    }
    #endregion

    #region Events
    public event System.EventHandler<SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangingEventArgs> OnChanging;
    public event System.EventHandler<SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangedEventArgs> OnChanged;
    #endregion

    #region Constants
    private const System.String PrefixName = "localStorage.";
    private const System.String SuffixName = "Item";
    private const System.String SetItemAction = PrefixName + "set" + SuffixName;
    private const System.String GetItemAction = PrefixName + "get" + SuffixName;
    private const System.String RemoveItemAction = PrefixName + "remove" + SuffixName;
    private const System.String ClearAction = PrefixName + "clear";
    private const System.String ContainsKeyProperty = PrefixName + "hasOwnProperty";
    private const System.String KeyProperty = PrefixName + "key";
    private const System.String LengthProperty = PrefixName + "length";
    #endregion

    #region Methods
    private T ConvertSerializedValue<T>(System.String SerializedValue)
    {
      if (System.String.IsNullOrWhiteSpace(SerializedValue))
        return default;

      if ((SerializedValue.StartsWith("{") && SerializedValue.EndsWith("}")) || (SerializedValue.StartsWith("\"") && SerializedValue.EndsWith("\"")))
        return System.Text.Json.JsonSerializer.Deserialize<T>(SerializedValue, JsonSerializerOptions);

      return (T)(System.Object)SerializedValue;
    }
    private void RaiseOnChanged(System.String Key, System.Object OldValue, System.Object NewValue)
    {
      SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangedEventArgs ChangedEventArgs = new SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangedEventArgs();
      ChangedEventArgs.Key = Key;
      ChangedEventArgs.OldValue = OldValue;
      ChangedEventArgs.NewValue = NewValue;
      this.OnChanged?.Invoke(this, ChangedEventArgs);
    }

    #region IAsyncLocalStorageService
    private void ValidateJSRuntime() { if (this.JSRuntime == null) throw new System.InvalidOperationException("JSRuntime not available."); }
    private void ValidateJSRuntime(System.String Key) { this.ValidateJSRuntime(); if (System.String.IsNullOrWhiteSpace(Key)) throw new System.ArgumentNullException("The Key parameter cannot be null or empty."); }
    private async System.Threading.Tasks.Task<SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangingEventArgs> RaiseOnChangingAsync(System.String Key, System.Object Value)
    {
      SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangingEventArgs ChangingEventArgs = new SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangingEventArgs();
      ChangingEventArgs.Key = Key;
      ChangingEventArgs.OldValue = await this.GetItemAsync<System.Object>(Key);
      ChangingEventArgs.NewValue = Value;
      this.OnChanging?.Invoke(this, ChangingEventArgs);
      return ChangingEventArgs;
    }
    public async System.Threading.Tasks.Task SetItemAsync<T>(System.String Key, T Value)
    {
      this.ValidateJSRuntime(Key);

      SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangingEventArgs ChangingEventArgs = await this.RaiseOnChangingAsync(Key, Value);
      if (ChangingEventArgs.Cancel) return;

      if (Value is System.String)
        await this.JSRuntime.InvokeVoidAsync(SetItemAction, Key, Value);
      else
        await this.JSRuntime.InvokeVoidAsync(SetItemAction, Key, System.Text.Json.JsonSerializer.Serialize(Value, this.JsonSerializerOptions));

      this.RaiseOnChanged(Key, ChangingEventArgs.OldValue, Value);
    }
    public async System.Threading.Tasks.Task<T> GetItemAsync<T>(System.String Key) { this.ValidateJSRuntime(Key); return this.ConvertSerializedValue<T>(await this.JSRuntime.InvokeAsync<System.String>(GetItemAction, Key)); }
    public async System.Threading.Tasks.Task RemoveItemAsync(System.String Key) { this.ValidateJSRuntime(Key); await this.JSRuntime.InvokeVoidAsync(RemoveItemAction, Key); }
    public async System.Threading.Tasks.Task ClearAsync() => await this.JSRuntime.InvokeVoidAsync(ClearAction);
    public async System.Threading.Tasks.Task<System.Boolean> ContainsKeyAsync(System.String Key) => await this.JSRuntime.InvokeAsync<System.Boolean>(ContainsKeyProperty, Key);
    public async System.Threading.Tasks.Task<System.String> KeyAsync(System.Int32 Index) => await this.JSRuntime.InvokeAsync<System.String>(KeyProperty, Index);
    public async System.Threading.Tasks.Task<System.Int32> LengthAsync() => await this.JSRuntime.InvokeAsync<System.Int32>("eval", LengthProperty);
    #endregion

    #region ISyncLocalStorageService
    private void ValidateJSInProcessRuntime() { if (this.JSInProcessRuntime == null) throw new System.InvalidOperationException("JSInProcessRuntime not available."); }
    private void ValidateJSInProcessRuntime(System.String Key) { this.ValidateJSInProcessRuntime(); if (System.String.IsNullOrWhiteSpace(Key)) throw new System.ArgumentNullException("The Key parameter cannot be null or empty."); }
    private SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangingEventArgs RaiseOnChanging(System.String Key, System.Object Value)
    {
      SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangingEventArgs ChangingEventArgs = new SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangingEventArgs();
      ChangingEventArgs.Key = Key;
      ChangingEventArgs.OldValue = this.GetItem<System.Object>(Key);
      ChangingEventArgs.NewValue = Value;
      this.OnChanging?.Invoke(this, ChangingEventArgs);
      return ChangingEventArgs;
    }
    public void SetItem<T>(System.String Key, T Value)
    {
      this.ValidateJSInProcessRuntime(Key);

      SoftmakeAll.SDK.Blazor.LocalStorage.EventArgs.ChangingEventArgs ChangingEventArgs = this.RaiseOnChanging(Key, Value);
      if (ChangingEventArgs.Cancel) return;

      if (Value is System.String)
        this.JSInProcessRuntime.InvokeVoid(SetItemAction, Key, Value);
      else
        this.JSInProcessRuntime.InvokeVoid(SetItemAction, Key, System.Text.Json.JsonSerializer.Serialize(Value, this.JsonSerializerOptions));

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
}