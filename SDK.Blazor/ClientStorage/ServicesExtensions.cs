using Microsoft.Extensions.DependencyInjection;

namespace SoftmakeAll.SDK.Blazor.ClientStorage
{
  public static class ServicesExtensions
  {
    #region Methods
    public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSoftmakeBlazorLocalStorage(this Microsoft.Extensions.DependencyInjection.IServiceCollection Services) => Services.AddScoped<SoftmakeAll.SDK.Blazor.ClientStorage.Services.ILocalStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.LocalStorageService>();
    public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddSoftmakeBlazorSessionStorage(this Microsoft.Extensions.DependencyInjection.IServiceCollection Services) => Services.AddScoped<SoftmakeAll.SDK.Blazor.ClientStorage.Services.ISessionStorageService, SoftmakeAll.SDK.Blazor.ClientStorage.Services.SessionStorageService>();
    #endregion
  }
}