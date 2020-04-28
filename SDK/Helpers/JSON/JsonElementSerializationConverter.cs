namespace SoftmakeAll.SDK.Helpers.JSON
{
  public class JsonElementSerializationConverter : System.Text.Json.Serialization.JsonConverter<System.Text.Json.JsonElement>
  {
    #region Constructor
    public JsonElementSerializationConverter() { }
    #endregion

    #region Overrides
    public override System.Text.Json.JsonElement Read(ref System.Text.Json.Utf8JsonReader Utf8JsonReader, System.Type Type, System.Text.Json.JsonSerializerOptions JsonSerializerOptions)
    {
      try
      {
        return System.Text.Json.JsonDocument.ParseValue(ref Utf8JsonReader).RootElement;
      }
      catch
      {
        return new System.Text.Json.JsonElement();
      }
    }
    public override void Write(System.Text.Json.Utf8JsonWriter Utf8JsonWriter, System.Text.Json.JsonElement JsonElement, System.Text.Json.JsonSerializerOptions JsonSerializerOptions)
    {
      if (JsonElement.ValueKind != System.Text.Json.JsonValueKind.Undefined)
        JsonElement.WriteTo(Utf8JsonWriter);
      else
        Utf8JsonWriter.WriteNullValue();
    }
    #endregion
  }
}