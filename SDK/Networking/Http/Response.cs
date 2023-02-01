using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.Networking.Http
{
  public class Response : SoftmakeAll.SDK.Networking.Http.Message
  {
    #region Constructor
    public Response(System.Net.HttpStatusCode StatusCode) : this(StatusCode, System.TimeSpan.FromMilliseconds(0.0D)) { }
    public Response(System.Net.HttpStatusCode StatusCode, System.TimeSpan ElapsedTime) : base()
    {
      this.StatusCode = StatusCode;
      this.ElapsedTime = ElapsedTime;
    }
    #endregion

    #region Properties
    public System.Net.HttpStatusCode StatusCode { get; }
    public System.TimeSpan ElapsedTime { get; }
    public System.Boolean IsSuccessStatusCode { get => ((int)this.StatusCode >= 200) && ((int)this.StatusCode <= 299); }
    public System.String StatusDescription { get => this.StatusCode.ToString(); }
    public System.String Status { get => $"{(int)this.StatusCode} - {this.StatusDescription}"; }

    private SoftmakeAll.SDK.Networking.Http.ResponseFileDetails _FileDetails;
    public SoftmakeAll.SDK.Networking.Http.ResponseFileDetails FileDetails { get => this._FileDetails; }
    #endregion

    #region Methods
    public override void AddCookie(System.Net.Cookie Cookie)
    {
      if ((Cookie != null) && (!(System.String.IsNullOrWhiteSpace(Cookie.Name))))
        base.AddCookie(Cookie);
    }
    internal void SetFileDetails(System.String ContentDisposition, System.String ContentType)
    {
      this._FileDetails = null;
      if (System.String.IsNullOrWhiteSpace(ContentDisposition))
        return;

      System.String FileExtension = null;
      System.String[] SplittedContentDisposition = ContentDisposition.Split("; ");
      System.String FileName = SplittedContentDisposition.FirstOrDefault(d => d.ToLower().StartsWith("filename="));
      if (!(System.String.IsNullOrWhiteSpace(FileName)))
      {
        FileName = FileName[9..^0];
        FileExtension = new System.IO.FileInfo(FileName).Extension;
      }

      this._FileDetails = new SoftmakeAll.SDK.Networking.Http.ResponseFileDetails(SplittedContentDisposition.Any(d => d == "attachment"), FileName, (System.String.IsNullOrWhiteSpace(ContentType) ? "application/octet-stream" : ContentType), SoftmakeAll.SDK.Networking.MIMETypes.GetMIMEType(FileExtension));
    }

    public System.String ReadBodyAsString() => this.ReadBodyAsString(false);
    public System.String ReadBodyAsString(System.Boolean KeepBody)
    {
      if (base.Body == null)
        return null;

      if (base.Body.Length == 0)
        return "";

      if (KeepBody)
        return System.Text.Encoding.UTF8.GetString(base.Body);

      System.String Result = System.Text.Encoding.UTF8.GetString(base.Body);
      base.ClearBody();
      return Result;
    }

    public System.Text.Json.JsonElement ReadBodyAsJSON() => this.ReadBodyAsJSON(false);
    public System.Text.Json.JsonElement ReadBodyAsJSON(System.Boolean KeepBody) => this.ReadBodyAsString(KeepBody).ToJsonElement();

    public System.Xml.Linq.XElement ReadBodyAsXML() => this.ReadBodyAsXML(false);
    public System.Xml.Linq.XElement ReadBodyAsXML(System.Boolean KeepBody)
    {
      try { return System.Xml.Linq.XElement.Parse(this.ReadBodyAsString(KeepBody)); } catch { }
      return null;
    }
    #endregion
  }
}