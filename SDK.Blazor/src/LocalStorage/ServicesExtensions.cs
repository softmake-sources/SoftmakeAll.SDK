using Microsoft.Extensions.DependencyInjection;

namespace SoftmakeAll.SDK.Blazor.LocalStorage
{
  public static class ServicesExtensions
  {
    #region Methods
    public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSoftmakeBlazorLocalStorage(this Microsoft.Extensions.DependencyInjection.IServiceCollection Services) =>
      Services
      .AddScoped<SoftmakeAll.SDK.Blazor.LocalStorage.Services.IAsyncLocalStorageService, SoftmakeAll.SDK.Blazor.LocalStorage.Services.LocalStorageService>()
      .AddScoped<SoftmakeAll.SDK.Blazor.LocalStorage.Services.ISyncLocalStorageService, SoftmakeAll.SDK.Blazor.LocalStorage.Services.LocalStorageService>();
    #endregion
  }
}