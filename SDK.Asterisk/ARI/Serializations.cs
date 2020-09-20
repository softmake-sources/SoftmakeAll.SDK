namespace SoftmakeAll.SDK.Asterisk.ARI
{
  public static class Serializations
  {
    #region Fields
    private static System.Text.Json.JsonSerializerOptions _JsonSerializerOptions = null;
    #endregion

    #region Properties
    public static System.Text.Json.JsonSerializerOptions JsonSerializerOptions
    {
      get
      {
        if (SoftmakeAll.SDK.Asterisk.ARI.Serializations._JsonSerializerOptions != null)
          return SoftmakeAll.SDK.Asterisk.ARI.Serializations._JsonSerializerOptions;

        SoftmakeAll.SDK.Asterisk.ARI.Serializations._JsonSerializerOptions = new System.Text.Json.JsonSerializerOptions();
        SoftmakeAll.SDK.Asterisk.ARI.Serializations._JsonSerializerOptions.Converters.Add(new SoftmakeAll.SDK.Asterisk.ARI.TimestampSerializationConverter());
        SoftmakeAll.SDK.Asterisk.ARI.Serializations._JsonSerializerOptions.DictionaryKeyPolicy = null;
        SoftmakeAll.SDK.Asterisk.ARI.Serializations._JsonSerializerOptions.PropertyNamingPolicy = null;
        SoftmakeAll.SDK.Asterisk.ARI.Serializations._JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        SoftmakeAll.SDK.Asterisk.ARI.Serializations._JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        return SoftmakeAll.SDK.Asterisk.ARI.Serializations._JsonSerializerOptions;
      }
    }
    #endregion
  }
}