using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Networking.Http
{
  public class Response : SoftmakeAll.SDK.Networking.Http.Message
  {
    #region Constructor
    public Response() : base()
    {
      this.StatusCode = System.Net.HttpStatusCode.OK;
      this.ElapsedTime = System.TimeSpan.FromMilliseconds(0.0D);
    }
    #endregion

    #region Properties
    public System.Net.HttpStatusCode StatusCode { get; set; }
    public System.TimeSpan ElapsedTime { get; set; }

    #region Read Only
    public System.Boolean IsSuccessStatusCode { get => ((int)this.StatusCode >= 200) && ((int)this.StatusCode <= 299); }
    public System.String StatusDescription { get => this.StatusCode.ToString(); }
    public System.String Status { get => $"{(int)this.StatusCode} - {this.StatusDescription}"; }
    #endregion
    #endregion

    #region Methods
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

    // TODO: Fazer
    public System.Text.Json.JsonElement ReadBodyAsXML() => this.ReadBodyAsXML(false);
    public System.Text.Json.JsonElement ReadBodyAsXML(System.Boolean KeepBody) => this.ReadBodyAsString(KeepBody).ToJsonElement();
    // TODO: Fazer
    #endregion
  }
}