using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SoftmakeAll.SDK.Networking.Http
{
  public class Request : SoftmakeAll.SDK.Networking.Http.Message
  {
    #region Fields
    private readonly System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>> _URLQueryParameters;
    private readonly System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>> _UserQueryParameters;
    private readonly System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>> _UserFormUrlEncodedParameters;
    private readonly System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter>> _UserMultipartFormDataParameters;
    #endregion

    #region Constructors
    public Request() : base()
    {
      this._URLQueryParameters = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>>();
      this._UserQueryParameters = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>>();
      this._UserFormUrlEncodedParameters = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>>();
      this._UserMultipartFormDataParameters = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter>>();

      this.Method = System.Net.Http.HttpMethod.Get;
      this.URL = null;
    }
    public Request(System.Uri URL) : this() => this.URL = URL;
    public Request(System.String URL) : this(new System.Uri(URL)) { }
    #endregion

    #region Properties
    public System.Net.Http.HttpMethod Method { get; set; }

    private System.Uri _URL;
    public System.Uri URL
    {
      get => this._URL;
      set
      {
        if (this._URL == value)
          return;

        this._URL = value;
        this._URLQueryParameters.Clear();

        if ((value == null) || (System.String.IsNullOrWhiteSpace(value.Query)))
          return;

        System.Collections.Specialized.NameValueCollection QueryString = System.Web.HttpUtility.ParseQueryString(value.Query);
        foreach (var QueryParameter in QueryString.AllKeys.SelectMany(QueryString.GetValues, (Key, Value) => new { Key, Value }))
          if (QueryParameter.Key != null)
            this._URLQueryParameters.Add(new System.Collections.Generic.KeyValuePair<System.String, System.String>(QueryParameter.Key, QueryParameter.Value));
      }
    }
    #endregion

    #region Methods
    private System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>> GetAllQueryParameters()
    {
      System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>> Result = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>>();
      Result.AddRange(this._URLQueryParameters);
      Result.AddRange(this._UserQueryParameters);
      return Result;
    }
    public void AddQueryParameter(System.String Key, System.String Value) => this.AddQueryParameter(Key, Value, false);
    public void AddQueryParameter(System.String Key, System.String Value, System.Boolean Overwrite)
    {
      if (System.String.IsNullOrWhiteSpace(Key))
        return;

      if (Overwrite)
        this.RemoveQueryParameter(Key);

      this._UserQueryParameters.Add(new System.Collections.Generic.KeyValuePair<System.String, System.String>(Key, Value));
    }
    public System.Collections.Generic.List<System.String> GetQueryParameter(System.String Key) => System.String.IsNullOrWhiteSpace(Key) ? null : this.GetAllQueryParameters().Where(p => p.Key == Key).Select(p => p.Value).ToList();
    public System.String GetQueryParameter(System.String Key, System.String Separator)
    {
      System.Collections.Generic.List<System.String> QueryParameters = this.GetQueryParameter(Key);
      return QueryParameters == null ? null : System.String.Join(Separator, QueryParameters);
    }
    public System.String GetQueryParameterValue(System.String Key) => this.GetQueryParameter(Key)?.FirstOrDefault();
    public System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>> GetQueryParameters() => this.GetAllQueryParameters();
    public void RemoveQueryParameter(System.String Key)
    {
      if (System.String.IsNullOrWhiteSpace(Key))
        return;

      this._URLQueryParameters.RemoveAll(p => p.Key == Key);
      this._UserQueryParameters.RemoveAll(p => p.Key == Key);
    }
    public void ClearQueryParameter()
    {
      this._URLQueryParameters.Clear();
      this._UserQueryParameters.Clear();
    }
    public System.String GetQueryString()
    {
      System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>> AllQueryParameters = this.GetAllQueryParameters();
      return (!(AllQueryParameters.Any())) ? null : $"?{System.String.Join('&', AllQueryParameters.Select(p => $"{System.Web.HttpUtility.UrlEncode(p.Key)}={System.Web.HttpUtility.UrlEncode(p.Value)}"))}";
    }

    public void AddBasicAuth(System.String Username, System.String Password) => this.AddBasicAuth(System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{Username}:{Password}")));
    public void AddBasicAuth(System.String Base64Authorization) => base.AddHeader("Authorization", $"Basic {Base64Authorization}", true);
    public void AddBearerAuth(System.String BearerAuthorization) => base.AddHeader("Authorization", $"Bearer {BearerAuthorization}", true);
    public void AddDefaultHeaders() => this.AddDefaultHeaders(true);
    public void AddDefaultHeaders(System.Boolean Overwrite)
    {
      base.AddHeader("Accept", "*/*", Overwrite);
      base.AddHeader("Accept-Encoding", "gzip, deflate, br", Overwrite);
      base.AddHeader("Connection", "keep-alive", Overwrite);
      base.AddHeader("Softmake-Request-Token", System.Guid.NewGuid().ToString().ToLower(), Overwrite);
      base.AddHeader("User-Agent", $"SoftmakeSDK/{System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}", Overwrite);
    }

    public override void AddCookie(System.String Name, System.String Value)
    {
      if (System.String.IsNullOrWhiteSpace(Name))
        return;

      base.AddCookie(new System.Net.Cookie(Name, Value));
    }

    public void SetBodyText(System.String Body) => this.SetBodyText(System.String.IsNullOrWhiteSpace(Body) ? default : System.Text.Encoding.UTF8.GetBytes(Body));
    public void SetBodyText(System.Byte[] Body)
    {
      if ((Body == null) || (Body.Length < 1))
        throw new System.ArgumentNullException("Body");

      base.SetBody(Body, "text/plain");
    }

    public void SetBodyJavaScript(System.String Body) => this.SetBodyJavaScript(System.String.IsNullOrWhiteSpace(Body) ? default : System.Text.Encoding.UTF8.GetBytes(Body));
    public void SetBodyJavaScript(System.Byte[] Body)
    {
      if ((Body == null) || (Body.Length < 2))
        throw new System.ArgumentNullException("Body");

      base.SetBody(Body, "application/javascript");
    }

    public void SetBodyJSON(System.String Body) => this.SetBodyJSON(System.String.IsNullOrWhiteSpace(Body) ? default : Body.ToJsonElement());
    public void SetBodyJSON(System.Byte[] Body) => this.SetBodyJSON(((Body == null) || (Body.Length < 2)) ? default : System.Text.Encoding.UTF8.GetString(Body).ToJsonElement());
    public void SetBodyJSON(System.Text.Json.JsonElement Body)
    {
      if (Body.ValueKind == System.Text.Json.JsonValueKind.Undefined)
        throw new System.FormatException("Invalid JSON format.");

      base.SetBody(Body.Serialize(), "application/json");
    }

    public void SetBodyHTML(System.String Body) => this.SetBodyHTML(System.String.IsNullOrWhiteSpace(Body) ? default : System.Text.Encoding.UTF8.GetBytes(Body));
    public void SetBodyHTML(System.Byte[] Body)
    {
      if ((Body == null) || (Body.Length < 3))
        throw new System.ArgumentNullException("Body");

      base.SetBody(Body, "text/html");
    }

    private System.Xml.Linq.XElement ToXML(System.String Body)
    {
      if (System.String.IsNullOrWhiteSpace(Body))
        return null;

      try { return System.Xml.Linq.XElement.Parse(Body); } catch { }
      return null;
    }
    public void SetBodyXML(System.String Body) => this.SetBodyXML(System.String.IsNullOrWhiteSpace(Body) ? null : this.ToXML(Body));
    public void SetBodyXML(System.Byte[] Body) => this.SetBodyXML(((Body == null) || (Body.Length < 2)) ? null : this.ToXML(System.Text.Encoding.UTF8.GetString(Body)));
    public void SetBodyXML(System.Xml.Linq.XElement Body)
    {
      if (Body == null)
        throw new System.FormatException("Invalid XML format.");

      base.SetBody(Body.ToString(), "application/xml");
    }

    public void AddFormUrlEncodedParameter(System.String Key, System.String Value) => this.AddFormUrlEncodedParameter(Key, Value, false);
    public void AddFormUrlEncodedParameter(System.String Key, System.String Value, System.Boolean Overwrite)
    {
      if (System.String.IsNullOrWhiteSpace(Key))
        return;

      if (Overwrite)
        this.RemoveFormUrlEncodedParameter(Key);

      this._UserFormUrlEncodedParameters.Add(new System.Collections.Generic.KeyValuePair<System.String, System.String>(Key, Value));
    }
    public System.Collections.Generic.List<System.String> GetFormUrlEncodedParameter(System.String Key) => System.String.IsNullOrWhiteSpace(Key) ? null : this._UserFormUrlEncodedParameters.Where(p => p.Key == Key).Select(p => p.Value).ToList();
    public System.String GetFormUrlEncodedParameter(System.String Key, System.String Separator)
    {
      System.Collections.Generic.List<System.String> FormUrlEncodedParameters = this.GetFormUrlEncodedParameter(Key);
      return FormUrlEncodedParameters == null ? null : System.String.Join(Separator, FormUrlEncodedParameters);
    }
    public System.String GetFormUrlEncodedParameterValue(System.String Key) => this.GetFormUrlEncodedParameter(Key)?.FirstOrDefault();
    public System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, System.String>> GetFormUrlEncodedParameters() => this._UserFormUrlEncodedParameters;
    public void RemoveFormUrlEncodedParameter(System.String Key)
    {
      if (!(System.String.IsNullOrWhiteSpace(Key)))
        this._UserFormUrlEncodedParameters.RemoveAll(p => p.Key == Key);
    }
    public void ClearFormUrlEncodedParameter() => this._UserFormUrlEncodedParameters.Clear();
    public void SetBodyFormUrlEncoded()
    {
      base.ClearBody();
      base.SetBody(new System.Byte[] { }, "application/x-www-form-urlencoded");
    }

    public void AddMultipartFormDataParameter(System.String Key, System.String Value) => this.AddMultipartFormDataParameter(Key, Value, false);
    public void AddMultipartFormDataParameter(System.String Key, System.String Value, System.Boolean Overwrite) => this.AddMultipartFormDataParameter(Key, new SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter(Value), Overwrite);
    public void AddMultipartFormDataParameter(System.String Key, SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter MultipartFormDataParameter) => this.AddMultipartFormDataParameter(Key, MultipartFormDataParameter, false);
    public void AddMultipartFormDataParameter(System.String Key, SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter MultipartFormDataParameter, System.Boolean Overwrite)
    {
      if (System.String.IsNullOrWhiteSpace(Key))
        return;

      if (Overwrite)
        this.RemoveMultipartFormDataParameter(Key);

      this._UserMultipartFormDataParameters.Add(new System.Collections.Generic.KeyValuePair<System.String, SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter>(Key, MultipartFormDataParameter));
    }
    public System.Collections.Generic.List<SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter> GetMultipartFormDataParameter(System.String Key) => System.String.IsNullOrWhiteSpace(Key) ? null : this._UserMultipartFormDataParameters.Where(p => p.Key == Key).Select(p => p.Value).ToList();
    public System.String GetMultipartFormDataParameter(System.String Key, System.String Separator)
    {
      System.Collections.Generic.List<SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter> MultipartFormDataParameters = this.GetMultipartFormDataParameter(Key);
      //return MultipartFormDataParameters == null ? null : System.String.Join(Separator, MultipartFormDataParameters.Select(p => $"{p.FileNameOrValue}")); // TODO: Verificar
      return MultipartFormDataParameters == null ? null : System.String.Join(Separator, MultipartFormDataParameters.Select(p => p.FileNameOrValue));
    }
    public SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter GetMultipartFormDataParameterValue(System.String Key) => this.GetMultipartFormDataParameter(Key)?.FirstOrDefault();
    public System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<System.String, SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter>> GetMultipartFormDataParameters() => this._UserMultipartFormDataParameters;
    public void RemoveMultipartFormDataParameter(System.String Key)
    {
      if (!(System.String.IsNullOrWhiteSpace(Key)))
        this._UserMultipartFormDataParameters.RemoveAll(p => p.Key == Key);
    }
    public void ClearMultipartFormDataParameter() => this._UserMultipartFormDataParameters.Clear();
    public void SetBodyMultipartFormData()
    {
      foreach (System.Collections.Generic.KeyValuePair<System.String, SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter> BodyMultipartFormDataParameter in this._UserMultipartFormDataParameters)
        if ((BodyMultipartFormDataParameter.Value.Data != null) && ((System.String.IsNullOrWhiteSpace(BodyMultipartFormDataParameter.Value.FileNameOrValue)) || (System.String.IsNullOrWhiteSpace(BodyMultipartFormDataParameter.Value.DataContentType))))
          throw new System.NullReferenceException($"FileNameOrValue and DataContentType are required. {BodyMultipartFormDataParameter.Value.FileNameOrValue}.");

      base.ClearBody();
      base.SetBody(new System.Byte[] { }, "multipart/form-data");
    }

    #region Static Methods
    public static SoftmakeAll.SDK.Networking.Http.Request CreateGetRequest(System.String URL) => SoftmakeAll.SDK.Networking.Http.Request.CreateRequest(System.Net.Http.HttpMethod.Get, URL, null, true, null, null);
    public static SoftmakeAll.SDK.Networking.Http.Request CreateGetRequest(System.String URL, System.String AuthorizationHeader) => SoftmakeAll.SDK.Networking.Http.Request.CreateRequest(System.Net.Http.HttpMethod.Get, URL, AuthorizationHeader, true, null, null);
    public static SoftmakeAll.SDK.Networking.Http.Request CreateGetRequest(System.String URL, System.Boolean AddDefaultHeaders) => SoftmakeAll.SDK.Networking.Http.Request.CreateRequest(System.Net.Http.HttpMethod.Get, URL, null, AddDefaultHeaders, null, null);
    public static SoftmakeAll.SDK.Networking.Http.Request CreateGetRequest(System.String URL, System.String AuthorizationHeader, System.Boolean AddDefaultHeaders) => SoftmakeAll.SDK.Networking.Http.Request.CreateRequest(System.Net.Http.HttpMethod.Get, URL, AuthorizationHeader, AddDefaultHeaders, null, null);
    public static SoftmakeAll.SDK.Networking.Http.Request CreateJsonRequest(System.Net.Http.HttpMethod Method, System.String URL, System.Text.Json.JsonElement Body) => SoftmakeAll.SDK.Networking.Http.Request.CreateRequest(Method, URL, null, true, Body.ToRawText(), "application/json");
    public static SoftmakeAll.SDK.Networking.Http.Request CreateJsonRequest(System.Net.Http.HttpMethod Method, System.String URL, System.String AuthorizationHeader, System.Text.Json.JsonElement Body) => SoftmakeAll.SDK.Networking.Http.Request.CreateRequest(Method, URL, AuthorizationHeader, true, Body.ToRawText(), "application/json");
    public static SoftmakeAll.SDK.Networking.Http.Request CreateJsonRequest(System.Net.Http.HttpMethod Method, System.String URL, System.Boolean AddDefaultHeaders, System.Text.Json.JsonElement Body) => SoftmakeAll.SDK.Networking.Http.Request.CreateRequest(Method, URL, null, AddDefaultHeaders, Body.ToRawText(), "application/json");
    public static SoftmakeAll.SDK.Networking.Http.Request CreateJsonRequest(System.Net.Http.HttpMethod Method, System.String URL, System.String AuthorizationHeader, System.Boolean AddDefaultHeaders, System.Text.Json.JsonElement Body) => SoftmakeAll.SDK.Networking.Http.Request.CreateRequest(Method, URL, AuthorizationHeader, AddDefaultHeaders, Body.ToRawText(), "application/json");
    public static SoftmakeAll.SDK.Networking.Http.Request CreateRequest(System.Net.Http.HttpMethod Method, System.String URL, System.String Body, System.String ContentType) => SoftmakeAll.SDK.Networking.Http.Request.CreateRequest(Method, URL, null, true, null, null);
    public static SoftmakeAll.SDK.Networking.Http.Request CreateRequest(System.Net.Http.HttpMethod Method, System.String URL, System.String AuthorizationHeader, System.String Body, System.String ContentType) => SoftmakeAll.SDK.Networking.Http.Request.CreateRequest(Method, URL, AuthorizationHeader, true, null, null);
    public static SoftmakeAll.SDK.Networking.Http.Request CreateRequest(System.Net.Http.HttpMethod Method, System.String URL, System.Boolean AddDefaultHeaders, System.String Body, System.String ContentType) => SoftmakeAll.SDK.Networking.Http.Request.CreateRequest(Method, URL, null, AddDefaultHeaders, null, null);
    public static SoftmakeAll.SDK.Networking.Http.Request CreateRequest(System.Net.Http.HttpMethod Method, System.String URL, System.String AuthorizationHeader, System.Boolean AddDefaultHeaders, System.String Body, System.String ContentType)
    {
      SoftmakeAll.SDK.Networking.Http.Request Request = new SoftmakeAll.SDK.Networking.Http.Request(URL) { Method = Method };

      if (AddDefaultHeaders)
        Request.AddDefaultHeaders();

      if (!(System.String.IsNullOrWhiteSpace(AuthorizationHeader)))
        Request.AddHeader("Authorization", AuthorizationHeader, true);

      Request.SetBody(Body, ContentType);

      return Request;
    }
    #endregion
    #endregion
  }
}