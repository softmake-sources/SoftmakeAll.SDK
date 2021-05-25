namespace SoftmakeAll.SDK.Fluent
{
  /// <summary>
  /// Helps to Write, Read and Clear cache data.
  /// </summary>
  internal static class GeneralCacheHelper
  {
    #region Fields
    private static readonly System.Object SyncRoot = new System.Object();
    private static readonly Microsoft.AspNetCore.DataProtection.IDataProtector DataProtector = Microsoft.AspNetCore.DataProtection.DataProtectionProvider.Create("SoftmakeAll.SDK.Fluent").CreateProtector("GeneralCacheHelper");
    #endregion

    #region Properties
    private static System.String CacheFilePath => $"{System.Reflection.Assembly.GetExecutingAssembly().Location}.smapp.bin3";
    #endregion

    #region Methods
    /// <summary>
    /// Reads the cache content bytes.
    /// </summary>
    /// <returns>The bytes of readed content.</returns>
    internal static System.Byte[] ReadBytes()
    {
      try
      {
        System.String CacheFilePath = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.CacheFilePath;
        if (!(System.IO.File.Exists(CacheFilePath)))
          return null;

        System.Byte[] UnprotectedData = null;

        lock (SoftmakeAll.SDK.Fluent.GeneralCacheHelper.SyncRoot)
          if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            UnprotectedData = System.Security.Cryptography.ProtectedData.Unprotect(System.IO.File.ReadAllBytes(CacheFilePath), null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
          else
            UnprotectedData = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.DataProtector.Unprotect(System.IO.File.ReadAllBytes(CacheFilePath));

        return UnprotectedData;
      }
      catch
      {
        SoftmakeAll.SDK.Fluent.GeneralCacheHelper.Clear();
      }
      return null;
    }

    /// <summary>
    /// Reads the cache contents.
    /// </summary>
    /// <returns>The readed content.</returns>
    internal static System.String ReadString()
    {
      System.Byte[] Result = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.ReadBytes();
      if (Result == null)
        return null;

      return System.Text.Encoding.UTF8.GetString(Result);
    }

    /// <summary>
    /// Writes the content bytes in cahce.
    /// </summary>
    /// <param name="Data">The bytes of content to be written.</param>
    internal static void WriteBytes(System.Byte[] Data)
    {
      System.String CacheFilePath = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.CacheFilePath;

      lock (SoftmakeAll.SDK.Fluent.GeneralCacheHelper.SyncRoot)
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
          Data = System.Security.Cryptography.ProtectedData.Protect(Data, null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
        else
          Data = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.DataProtector.Protect(Data);

      System.IO.File.WriteAllBytes(CacheFilePath, Data);
    }

    /// <summary>
    /// /// Writes the content in cahce.
    /// </summary>
    /// <param name="Data">The content to be written.</param>
    internal static void WriteString(System.String Data)
    {
      if (Data != null)
        SoftmakeAll.SDK.Fluent.GeneralCacheHelper.WriteBytes(System.Text.Encoding.UTF8.GetBytes(Data));
    }

    /// <summary>
    /// Clears cache data.
    /// </summary>
    internal static void Clear()
    {
      try
      {
        System.String CacheFilePath = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.CacheFilePath;
        if (!(System.IO.File.Exists(CacheFilePath)))
          return;

        lock (SoftmakeAll.SDK.Fluent.GeneralCacheHelper.SyncRoot)
          System.IO.File.Delete(CacheFilePath);
      }
      catch { }
    }
    #endregion
  }
}