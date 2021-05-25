namespace SoftmakeAll.SDK.Fluent
{
  /// <summary>
  /// PublicClientApplication user tokens serialization.
  /// </summary>
  internal static class TokenCacheHelper
  {
    #region Methods
    /// <summary>
    /// Enables serialization of PublicClientApplication user tokens.
    /// </summary>
    /// <param name="UserTokenCache">User token cache from PublicClientApplication.</param>
    internal static void EnableSerialization(Microsoft.Identity.Client.ITokenCache UserTokenCache)
    {
      UserTokenCache.SetBeforeAccess(SoftmakeAll.SDK.Fluent.TokenCacheHelper.BeforeAccessNotification);
      UserTokenCache.SetAfterAccess(SoftmakeAll.SDK.Fluent.TokenCacheHelper.AfterAccessNotification);
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