namespace SoftmakeAll.SDK.Helpers.JSON.Converters
{
  public class TimeSpanSerializationConverter : System.Text.Json.Serialization.JsonConverter<System.TimeSpan>
  {
    #region Constructor
    public TimeSpanSerializationConverter() { }
    #endregion

    #region Constants
    private const System.String Format = @"d\.hh\:mm\:ss\:FFF";
    #endregion

    #region Methods
    public override System.TimeSpan Read(ref System.Text.Json.Utf8JsonReader Utf8JsonReader, System.Type Type, System.Text.Json.JsonSerializerOptions JsonSerializerOptions)
    {
      System.String Value = Utf8JsonReader.GetString();
      if (System.String.IsNullOrWhiteSpace(Value))
        return System.TimeSpan.Zero;

      System.TimeSpan Result;
      if (!(System.TimeSpan.TryParseExact(Value, Format, null, out Result)))
        throw new System.FormatException("The provided TimeSpan is invalid.");

      return Result;
    }
    public override void Write(System.Text.Json.Utf8JsonWriter Utf8JsonWriter, System.TimeSpan TimeSpan, System.Text.Json.JsonSerializerOptions JsonSerializerOptions) => Utf8JsonWriter.WriteStringValue($"{TimeSpan.ToString(Format)}");
    #endregion
  }
}