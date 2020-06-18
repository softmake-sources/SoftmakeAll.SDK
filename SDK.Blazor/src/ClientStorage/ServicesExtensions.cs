using Microsoft.Extensions.DependencyInjection;

namespace SoftmakeAll.SDK.Blazor.ClientStorage
{
  public static class ServicesExtensions
  {
    #region Methods
    public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSoftmakeBlazorLocalStorage(this Microsoft.Extensions.DependencyInjection.IServiceCollection Services) =>
      Services
      .AddScoped<SoftmakeAll.SDK.Blazor.ClientStorage.Services.IAsyncLocalStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.LocalStorageService>()
      .AddScoped<SoftmakeAll.SDK.Blazor.ClientStorage.Services.ISyncLocalStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.LocalStorageService>();

    public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSoftmakeBlazorSessionStorage(this Microsoft.Extensions.DependencyInjection.IServiceCollection Services) =>
      Services
      .AddScoped<SoftmakeAll.SDK.Blazor.ClientStorage.Services.IAsyncSessionStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.SessionStorageService>()
      .AddScoped<SoftmakeAll.SDK.Blazor.ClientStorage.Services.ISyncSessionStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.SessionStorageService>();
    #endregion
  }
}