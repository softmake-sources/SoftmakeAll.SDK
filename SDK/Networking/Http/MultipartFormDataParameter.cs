namespace SoftmakeAll.SDK.Networking.Http
{
  public class MultipartFormDataParameter
  {
    #region Constructors
    public MultipartFormDataParameter() : this(null) { }
    public MultipartFormDataParameter(System.String Value) : this(Value, null, null) { }
    public MultipartFormDataParameter(System.String Value, System.String DataContentType, System.Byte[] Data)
    {
      this.FileNameOrValue = Value;
      this.DataContentType = DataContentType;
      this.Data = Data;
    }
    #endregion

    #region Properties
    public System.String FileNameOrValue { get; set; }
    public System.String DataContentType { get; set; }
    public System.Byte[] Data { get; set; }
    #endregion
  }
}