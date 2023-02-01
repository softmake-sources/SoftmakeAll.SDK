namespace SoftmakeAll.SDK.Networking.Http
{
  public class ResponseFileDetails
  {
    #region Constructors
    public ResponseFileDetails(System.Boolean IsAttachment, System.String FileName, System.String ContentType, System.String RecommendedContentType)
    {
      this.IsAttachment = IsAttachment;
      this.FileName = FileName;
      this.ContentType = ContentType;
      this.RecommendedContentType = RecommendedContentType;
    }
    #endregion

    #region Properties
    public System.Boolean IsAttachment { get; }
    public System.String FileName { get; }
    public System.String ContentType { get; }
    public System.String RecommendedContentType { get; }
    #endregion
  }
}