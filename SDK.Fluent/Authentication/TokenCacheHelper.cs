namespace SoftmakeAll.SDK.Fluent
{
  internal static class TokenCacheHelper
  {
    #region Methods
    internal static void EnableSerialization(Microsoft.Identity.Client.ITokenCache ITokenCache)
    {
      ITokenCache.SetBeforeAccess(SoftmakeAll.SDK.Fluent.TokenCacheHelper.BeforeAccessNotification);
      ITokenCache.SetAfterAccess(SoftmakeAll.SDK.Fluent.TokenCacheHelper.AfterAccessNotification);
    }
    private static void BeforeAccessNotification(Microsoft.Identity.Client.TokenCacheNotificationArgs TokenCacheNotificationArgs)
    {
      try
      {
        TokenCacheNotificationArgs.TokenCache.DeserializeMsalV3(SoftmakeAll.SDK.Fluent.GeneralCacheHelper.ReadBytes());
      }
      catch
      {
        SoftmakeAll.SDK.Fluent.GeneralCacheHelper.Clear();
        TokenCacheNotificationArgs.TokenCache.DeserializeMsalV3(null);
      }
    }
    private static void AfterAccessNotification(Microsoft.Identity.Client.TokenCacheNotificationArgs TokenCacheNotificationArgs)
    {
      if (TokenCacheNotificationArgs.HasStateChanged)
        SoftmakeAll.SDK.Fluent.GeneralCacheHelper.WriteBytes(TokenCacheNotificationArgs.TokenCache.SerializeMsalV3());
    }
    #endregion
  }
}