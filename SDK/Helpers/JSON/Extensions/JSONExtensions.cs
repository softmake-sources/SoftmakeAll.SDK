namespace SoftmakeAll.SDK.Helpers.JSON.Extensions
{
  public static class JSONExtensions
  {
    #region Constants
    private const System.String InvalidElementNameErrorMessage = "Invalid ElementName.";
    private const System.String InvalidValueKindErrorMessage = "Invalid ValueKind.";
    private const System.String InvalidSystemTypeFullNameErrorMessage = "Invalid SystemTypeFullName.";
    private const System.String MismatchTypes = "The requested operation requires an element of type '{0}', but the target element has type '{1}'.";
    #endregion

    #region Methods
    public static System.Text.Json.JsonSerializerOptions CreateJsonSerializerOptions(System.Boolean UseCamelCase)
    {
      System.Text.Json.JsonSerializerOptions JsonSerializerOptions = new System.Text.Json.JsonSerializerOptions();
      if (UseCamelCase)
      {
        JsonSerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        JsonSerializerOptions.PropertyNameCaseInsensitive = false;
      }
      else
      {
        JsonSerializerOptions.DictionaryKeyPolicy = null;
        JsonSerializerOptions.PropertyNamingPolicy = null;
        JsonSerializerOptions.PropertyNameCaseInsensitive = true;
      }
      JsonSerializerOptions.Converters.Add(new SoftmakeAll.SDK.Helpers.JSON.Converters.JsonElementSerializationConverter());
      JsonSerializerOptions.Converters.Add(new SoftmakeAll.SDK.Helpers.JSON.Converters.TimeSpanSerializationConverter());
      JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
      JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.Never;
      return JsonSerializerOptions;
    }

    public static System.String Serialize<T>(this T Object) { return SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.Serialize(Object, false); }
    public static System.String Serialize<T>(this T Object, System.Boolean UseCamelCase)
    {
      if (Object == null)
        return null;

      return System.Text.Json.JsonSerializer.Serialize(Object, SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.CreateJsonSerializerOptions(UseCamelCase));
    }
    public static T Deserialize<T>(this System.String String) { return SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.Deserialize<T>(String, false); }
    public static T Deserialize<T>(this System.String String, System.Boolean UseCamelCase)
    {
      if (System.String.IsNullOrEmpty(String))
        return default(T);

      return System.Text.Json.JsonSerializer.Deserialize<T>(String, SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.CreateJsonSerializerOptions(UseCamelCase));
    }
    public static System.Text.Json.JsonElement ToJsonElement(this System.Object Object) { return Object.ToJsonElement(false); }
    public static System.Text.Json.JsonElement ToJsonElement(this System.Object Object, System.Boolean ThrowOnError)
    {
      if (Object == null)
        return new System.Text.Json.JsonElement();

      return SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.Serialize(Object).ToJsonElement(ThrowOnError);
    }
    public static System.Text.Json.JsonElement ToJsonElement(this System.String String) { return String.ToJsonElement(false); }
    public static System.Text.Json.JsonElement ToJsonElement(this System.String String, System.Boolean ThrowOnError)
    {
      if (System.String.IsNullOrEmpty(String))
        return new System.Text.Json.JsonElement();

      try
      {
        return System.Text.Json.JsonDocument.Parse(String).RootElement;
      }
      catch
      {
        if (ThrowOnError)
          throw;
      }

      return new System.Text.Json.JsonElement();
    }
    public static System.Boolean IsValid(this System.Text.Json.JsonElement JsonElement) { return JsonElement.IsValid(false); }
    public static System.Boolean IsValid(this System.Text.Json.JsonElement JsonElement, System.Boolean NullsOrEmptyArraysAndObjectsAreValid)
    {
      if ((JsonElement.ValueKind == System.Text.Json.JsonValueKind.Undefined) || (JsonElement.ValueKind == System.Text.Json.JsonValueKind.Null))
        return false;

      if (NullsOrEmptyArraysAndObjectsAreValid)
        return true;

      switch (JsonElement.ValueKind)
      {
        case System.Text.Json.JsonValueKind.Array:
          return JsonElement.GetArrayLength() > 0;
        case System.Text.Json.JsonValueKind.Object:
          System.String RawText = JsonElement.GetRawText();
          return ((!(System.String.IsNullOrWhiteSpace(RawText))) && (RawText != "{}"));
      }

      return true;
    }
    public static System.String ToRawText(this System.Text.Json.JsonElement JsonElement)
    {
      if (!(JsonElement.IsValid()))
        return null;

      return JsonElement.GetRawText();
    }
    public static T ToObject<T>(this System.Text.Json.JsonElement JsonElement)
    {
      if (!(JsonElement.IsValid()))
        return default(T);

      return SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.Deserialize<T>(JsonElement.ToRawText());
    }

    public static System.Text.Json.JsonElement GetJsonElement(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetJsonElement(ElementName, false); }
    public static System.Text.Json.JsonElement GetJsonElement(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement Result = new System.Text.Json.JsonElement();

      if ((System.String.IsNullOrWhiteSpace(ElementName)) || (!(JsonElement.IsValid())))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidElementNameErrorMessage);
        else
          return Result;
      }

      if ((JsonElement.ValueKind != System.Text.Json.JsonValueKind.Object) || (!(JsonElement.TryGetProperty(ElementName, out Result))))
        return Result;

      return Result;
    }
    public static System.String GetString(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetString(ElementName, false); }
    public static System.String GetString(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.String)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return FoundJsonElement.GetString();
    }
    public static System.Object GetValue(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.String SystemTypeFullName) { return JsonElement.GetValue(ElementName, SystemTypeFullName, false); }
    public static System.Object GetValue(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.String SystemTypeFullName, System.Boolean ThrowOnError)
    {
      if (System.String.IsNullOrWhiteSpace(SystemTypeFullName))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidSystemTypeFullNameErrorMessage);
        else
          return null;
      }

      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      return FoundJsonElement.GetValue(SystemTypeFullName, ThrowOnError);
    }
    public static System.Object GetValue(this System.Text.Json.JsonElement JsonElement, System.String SystemTypeFullName) { return JsonElement.GetValue(SystemTypeFullName, false); }
    public static System.Object GetValue(this System.Text.Json.JsonElement JsonElement, System.String SystemTypeFullName, System.Boolean ThrowOnError)
    {
      if (System.String.IsNullOrWhiteSpace(SystemTypeFullName))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidSystemTypeFullNameErrorMessage);
        else
          return null;
      }

      if (JsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      switch (SystemTypeFullName)
      {
        case "System.Boolean": return JsonElement.GetBoolean();
        case "System.Byte": return JsonElement.GetByte();
        case "System.DateTime": return JsonElement.GetDateTime();
        case "System.DateTimeOffset": return JsonElement.GetDateTimeOffset();
        case "System.Decimal": return JsonElement.GetDecimal();
        case "System.Double": return JsonElement.GetDouble();
        case "System.Guid": return JsonElement.GetGuid();
        case "System.Int16": return JsonElement.GetInt16();
        case "System.Int32": return JsonElement.GetInt32();
        case "System.Int64": return JsonElement.GetInt64();
        case "System.SByte": return JsonElement.GetSByte();
        case "System.Single": return JsonElement.GetSingle();
        case "System.String": return JsonElement.GetString();
        case "System.UInt16": return JsonElement.GetUInt16();
        case "System.UInt32": return JsonElement.GetUInt32();
        case "System.UInt64": return JsonElement.GetUInt64();
        case "System.Text.Json.JsonElement": return JsonElement;
      }

      if (ThrowOnError)
        throw new System.Text.Json.JsonException(System.String.Format(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.MismatchTypes, SystemTypeFullName, JsonElement.ValueKind.ToString()));

      return null;
    }
    public static System.Object GetValue(this System.Text.Json.JsonProperty JsonProperty, System.String SystemTypeFullName) { return JsonProperty.GetValue(SystemTypeFullName, false); }
    public static System.Object GetValue(this System.Text.Json.JsonProperty JsonProperty, System.String SystemTypeFullName, System.Boolean ThrowOnError)
    {
      if (System.String.IsNullOrWhiteSpace(SystemTypeFullName))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidSystemTypeFullNameErrorMessage);
        else
          return null;
      }

      if (JsonProperty.Value.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      switch (SystemTypeFullName)
      {
        case "System.Boolean": return JsonProperty.Value.GetBoolean();
        case "System.Byte": return JsonProperty.Value.GetByte();
        case "System.DateTime": return JsonProperty.Value.GetDateTime();
        case "System.DateTimeOffset": return JsonProperty.Value.GetDateTimeOffset();
        case "System.Decimal": return JsonProperty.Value.GetDecimal();
        case "System.Double": return JsonProperty.Value.GetDouble();
        case "System.Guid": return JsonProperty.Value.GetGuid();
        case "System.Int16": return JsonProperty.Value.GetInt16();
        case "System.Int32": return JsonProperty.Value.GetInt32();
        case "System.Int64": return JsonProperty.Value.GetInt64();
        case "System.SByte": return JsonProperty.Value.GetSByte();
        case "System.Single": return JsonProperty.Value.GetSingle();
        case "System.String": return JsonProperty.Value.GetString();
        case "System.UInt16": return JsonProperty.Value.GetUInt16();
        case "System.UInt32": return JsonProperty.Value.GetUInt32();
        case "System.UInt64": return JsonProperty.Value.GetUInt64();
        case "System.Text.Json.JsonElement": return JsonProperty.Value;
      }

      if (ThrowOnError)
        throw new System.Text.Json.JsonException(System.String.Format(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.MismatchTypes, SystemTypeFullName, JsonProperty.Value.ValueKind.ToString()));

      return JsonProperty.Value;
    }

    #region Nullable Values
    public static System.Nullable<System.Boolean> GetNullableBoolean(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableBoolean(ElementName, false); }
    public static System.Nullable<System.Boolean> GetNullableBoolean(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if ((FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.True) && (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.False))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return FoundJsonElement.GetBoolean();
    }
    public static System.Nullable<System.Byte> GetNullableByte(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableByte(ElementName, false); }
    public static System.Nullable<System.Byte> GetNullableByte(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.Byte Result;
      if (!(FoundJsonElement.TryGetByte(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.DateTime> GetNullableDateTime(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableDateTime(ElementName, false); }
    public static System.Nullable<System.DateTime> GetNullableDateTime(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.String)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.DateTime Result;
      if (!(FoundJsonElement.TryGetDateTime(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.DateTimeOffset> GetNullableDateTimeOffset(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableDateTimeOffset(ElementName, false); }
    public static System.Nullable<System.DateTimeOffset> GetNullableDateTimeOffset(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.String)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.DateTimeOffset Result;
      if (!(FoundJsonElement.TryGetDateTimeOffset(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.Decimal> GetNullableDecimal(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableDecimal(ElementName, false); }
    public static System.Nullable<System.Decimal> GetNullableDecimal(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.Decimal Result;
      if (!(FoundJsonElement.TryGetDecimal(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.Double> GetNullableDouble(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableDouble(ElementName, false); }
    public static System.Nullable<System.Double> GetNullableDouble(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.Double Result;
      if (!(FoundJsonElement.TryGetDouble(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.Guid> GetNullableGuid(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableGuid(ElementName, false); }
    public static System.Nullable<System.Guid> GetNullableGuid(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.String)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.Guid Result;
      if (!(FoundJsonElement.TryGetGuid(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.Int16> GetNullableInt16(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableInt16(ElementName, false); }
    public static System.Nullable<System.Int16> GetNullableInt16(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.Int16 Result;
      if (!(FoundJsonElement.TryGetInt16(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.Int32> GetNullableInt32(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableInt32(ElementName, false); }
    public static System.Nullable<System.Int32> GetNullableInt32(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.Int32 Result;
      if (!(FoundJsonElement.TryGetInt32(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.Int64> GetNullableInt64(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableInt64(ElementName, false); }
    public static System.Nullable<System.Int64> GetNullableInt64(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.Int64 Result;
      if (!(FoundJsonElement.TryGetInt64(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.SByte> GetNullableSByte(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableSByte(ElementName, false); }
    public static System.Nullable<System.SByte> GetNullableSByte(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.SByte Result;
      if (!(FoundJsonElement.TryGetSByte(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.Single> GetNullableSingle(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableSingle(ElementName, false); }
    public static System.Nullable<System.Single> GetNullableSingle(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.Single Result;
      if (!(FoundJsonElement.TryGetSingle(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.UInt16> GetNullableUInt16(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableUInt16(ElementName, false); }
    public static System.Nullable<System.UInt16> GetNullableUInt16(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.UInt16 Result;
      if (!(FoundJsonElement.TryGetUInt16(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.UInt32> GetNullableUInt32(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableUInt32(ElementName, false); }
    public static System.Nullable<System.UInt32> GetNullableUInt32(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.UInt32 Result;
      if (!(FoundJsonElement.TryGetUInt32(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    public static System.Nullable<System.UInt64> GetNullableUInt64(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetNullableUInt64(ElementName, false); }
    public static System.Nullable<System.UInt64> GetNullableUInt64(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return null;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return null;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      System.UInt64 Result;
      if (!(FoundJsonElement.TryGetUInt64(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return null;
      }

      return Result;
    }
    #endregion

    #region Non Nullable Values
    public static System.Boolean GetBoolean(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetBoolean(ElementName, false); }
    public static System.Boolean GetBoolean(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return false;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return false;

      if ((FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.True) && (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.False))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return false;
      }

      return FoundJsonElement.GetBoolean();
    }
    public static System.Byte GetByte(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetByte(ElementName, false); }
    public static System.Byte GetByte(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return 0;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return 0;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      System.Byte Result;
      if (!(FoundJsonElement.TryGetByte(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      return Result;
    }
    public static System.DateTime GetDateTime(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetDateTime(ElementName, false); }
    public static System.DateTime GetDateTime(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.DateTime Result = new System.DateTime();

      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return Result;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return Result;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.String)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return Result;
      }

      if (!(FoundJsonElement.TryGetDateTime(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return Result;
      }

      return Result;
    }
    public static System.DateTimeOffset GetDateTimeOffset(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetDateTimeOffset(ElementName, false); }
    public static System.DateTimeOffset GetDateTimeOffset(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.DateTimeOffset Result = new System.DateTimeOffset();

      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return Result;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return Result;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.String)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return Result;
      }

      if (!(FoundJsonElement.TryGetDateTimeOffset(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return Result;
      }

      return Result;
    }
    public static System.Decimal GetDecimal(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetDecimal(ElementName, false); }
    public static System.Decimal GetDecimal(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return 0.0M;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return 0.0M;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0.0M;
      }

      System.Decimal Result;
      if (!(FoundJsonElement.TryGetDecimal(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0.0M;
      }

      return Result;
    }
    public static System.Double GetDouble(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetDouble(ElementName, false); }
    public static System.Double GetDouble(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return 0.0D;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return 0.0D;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0.0D;
      }

      System.Double Result;
      if (!(FoundJsonElement.TryGetDouble(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0.0D;
      }

      return Result;
    }
    public static System.Guid GetGuid(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetGuid(ElementName, false); }
    public static System.Guid GetGuid(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Guid Result = new System.Guid();

      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return Result;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return Result;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.String)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return Result;
      }

      if (!(FoundJsonElement.TryGetGuid(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return Result;
      }

      return Result;
    }
    public static System.Int16 GetInt16(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetInt16(ElementName, false); }
    public static System.Int16 GetInt16(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return 0;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return 0;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      System.Int16 Result;
      if (!(FoundJsonElement.TryGetInt16(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      return Result;
    }
    public static System.Int32 GetInt32(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetInt32(ElementName, false); }
    public static System.Int32 GetInt32(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return 0;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return 0;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      System.Int32 Result;
      if (!(FoundJsonElement.TryGetInt32(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      return Result;
    }
    public static System.Int64 GetInt64(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetInt64(ElementName, false); }
    public static System.Int64 GetInt64(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return 0L;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return 0L;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0L;
      }

      System.Int64 Result;
      if (!(FoundJsonElement.TryGetInt64(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0L;
      }

      return Result;
    }
    public static System.SByte GetSByte(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetSByte(ElementName, false); }
    public static System.SByte GetSByte(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return 0;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return 0;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      System.SByte Result;
      if (!(FoundJsonElement.TryGetSByte(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      return Result;
    }
    public static System.Single GetSingle(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetSingle(ElementName, false); }
    public static System.Single GetSingle(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return 0.0F;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return 0.0F;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0.0F;
      }

      System.Single Result;
      if (!(FoundJsonElement.TryGetSingle(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0.0F;
      }

      return Result;
    }
    public static System.UInt16 GetUInt16(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetUInt16(ElementName, false); }
    public static System.UInt16 GetUInt16(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return 0;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return 0;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      System.UInt16 Result;
      if (!(FoundJsonElement.TryGetUInt16(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      return Result;
    }
    public static System.UInt32 GetUInt32(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetUInt32(ElementName, false); }
    public static System.UInt32 GetUInt32(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return 0U;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return 0U;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0U;
      }

      System.UInt32 Result;
      if (!(FoundJsonElement.TryGetUInt32(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0U;
      }

      return Result;
    }
    public static System.UInt64 GetUInt64(this System.Text.Json.JsonElement JsonElement, System.String ElementName) { return JsonElement.GetUInt64(ElementName, false); }
    public static System.UInt64 GetUInt64(this System.Text.Json.JsonElement JsonElement, System.String ElementName, System.Boolean ThrowOnError)
    {
      System.Text.Json.JsonElement FoundJsonElement = SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.GetJsonElement(JsonElement, ElementName, ThrowOnError);
      if (!(FoundJsonElement.IsValid()))
        return 0;

      if (FoundJsonElement.ValueKind == System.Text.Json.JsonValueKind.Null)
        return 0;

      if (FoundJsonElement.ValueKind != System.Text.Json.JsonValueKind.Number)
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      System.UInt64 Result;
      if (!(FoundJsonElement.TryGetUInt64(out Result)))
      {
        if (ThrowOnError)
          throw new System.Exception(SoftmakeAll.SDK.Helpers.JSON.Extensions.JSONExtensions.InvalidValueKindErrorMessage);
        else
          return 0;
      }

      return Result;
    }
    #endregion
    #endregion
  }
}