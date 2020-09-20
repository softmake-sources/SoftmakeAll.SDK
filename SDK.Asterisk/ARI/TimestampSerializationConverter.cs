namespace SoftmakeAll.SDK.Asterisk.ARI
{
  public class TimestampSerializationConverter : System.Text.Json.Serialization.JsonConverter<System.DateTimeOffset>
  {
    #region Constructor
    public TimestampSerializationConverter() { }
    #endregion

    #region Constants
    private const System.String Format = "yyyy-MM-ddTHH:mm:ss.fff+0000";
    #endregion

    #region Methods
    public override System.DateTimeOffset Read(ref System.Text.Json.Utf8JsonReader Utf8JsonReader, System.Type Type, System.Text.Json.JsonSerializerOptions JsonSerializerOptions)
    {
      System.String Value = Utf8JsonReader.GetString();
      if (System.String.IsNullOrWhiteSpace(Value))
        return new System.DateTimeOffset();

      System.DateTimeOffset Result;
      if (!(System.DateTimeOffset.TryParseExact(Value, Format, null, System.Globalization.DateTimeStyles.None, out Result)))
        throw new System.FormatException("The provided DateTimeOffset is invalid.");

      return Result;
    }
    public override void Write(System.Text.Json.Utf8JsonWriter Utf8JsonWriter, System.DateTimeOffset DateTimeOffset, System.Text.Json.JsonSerializerOptions JsonSerializerOptions) => Utf8JsonWriter.WriteStringValue($"{DateTimeOffset.ToString(Format)}");
    #endregion
  }
}