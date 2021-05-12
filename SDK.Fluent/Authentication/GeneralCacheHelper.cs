namespace SoftmakeAll.SDK.Fluent
{
  internal static class GeneralCacheHelper
  {
    #region Fields
    private static readonly System.Object FileLock = new System.Object();
    private static readonly Microsoft.AspNetCore.DataProtection.IDataProtector DataProtector = Microsoft.AspNetCore.DataProtection.DataProtectionProvider.Create("SoftmakeAll.SDK.Fluent").CreateProtector("GeneralCacheHelper");
    #endregion

    #region Properties
    private static System.String CacheFilePath => $"{System.Reflection.Assembly.GetExecutingAssembly().Location}.smapp.bin3";
    #endregion

    #region Methods
    internal static System.Byte[] ReadBytes()
    {
      try
      {
        System.String CacheFilePath = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.CacheFilePath;
        if (!(System.IO.File.Exists(CacheFilePath)))
          return null;

        System.Byte[] UnprotectedData = null;

        lock (FileLock)
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
    internal static System.String ReadString()
    {
      System.Byte[] Result = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.ReadBytes();
      if (Result == null)
        return null;

      return System.Text.Encoding.UTF8.GetString(Result);
    }
    internal static void WriteBytes(System.Byte[] Data)
    {
      System.String CacheFilePath = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.CacheFilePath;

      lock (FileLock)
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
          Data = System.Security.Cryptography.ProtectedData.Protect(Data, null, System.Security.Cryptography.DataProtectionScope.CurrentUser);
        else
          Data = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.DataProtector.Protect(Data);

      System.IO.File.WriteAllBytes(CacheFilePath, Data);
    }
    internal static void WriteString(System.String Data)
    {
      if (Data != null)
        SoftmakeAll.SDK.Fluent.GeneralCacheHelper.WriteBytes(System.Text.Encoding.UTF8.GetBytes(Data));
    }
    internal static void Clear()
    {
      try
      {
        System.String CacheFilePath = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.CacheFilePath;
        if (!(System.IO.File.Exists(CacheFilePath)))
          return;

        lock (FileLock)
          System.IO.File.Delete(CacheFilePath);
      }
      catch { }
    }
    #endregion
  }
}