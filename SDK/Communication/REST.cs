using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Communication
{
  public class REST
  {
    #region Constructor
    public REST()
    {
      this.Headers = new System.Collections.Generic.Dictionary<System.String, System.String>();
      this.ClearProperties();
    }
    #endregion

    #region SubClasses
    public class File
    {
      #region Constructor
      public File() { }
      #endregion

      #region Properties
      public System.String Name { get; set; }
      public System.Byte[] Contents { get; set; }
      #endregion
    }
    #endregion

    #region Fields and Properties
    public System.String URL { get; set; }
    public System.String Method { get; set; }
    public System.Collections.Generic.Dictionary<System.String, System.String> Headers { get; }
    public System.String AuthorizationBearerToken { get; set; }
    public System.Text.Json.JsonElement Body { get; set; }
    public System.Int32 Timeout { get; set; }

    private System.Boolean _HasRequestErrors;
    public System.Boolean HasRequestErrors => this._HasRequestErrors;

    private System.Net.HttpStatusCode _StatusCode;
    public System.Net.HttpStatusCode StatusCode => this._StatusCode;
    #endregion

    #region Constants
    private const System.String EmptyURLErrorMessage = "The 'URL' property must have a value.";
    private const System.String DefaultMethod = "GET";
    private const System.String DefaultContentType = "application/json";
    #endregion

    #region Methods
    public System.Text.Json.JsonElement Send()
    {
      if (System.String.IsNullOrWhiteSpace(this.URL))
        throw new System.Exception(SoftmakeAll.SDK.Communication.REST.EmptyURLErrorMessage);

      this._HasRequestErrors = false;

      if (System.String.IsNullOrWhiteSpace(this.Method))
        this.Method = SoftmakeAll.SDK.Communication.REST.DefaultMethod;

      System.Text.Json.JsonElement Result = new System.Text.Json.JsonElement();
      try
      {
        System.Net.HttpWebRequest HttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(this.URL);
        HttpWebRequest.Method = this.Method;

        if (this.Headers.Count > 0)
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> Header in this.Headers)
            if (Header.Key != "Content-Type")
              HttpWebRequest.Headers.Add(Header.Key, Header.Value);
            else
              HttpWebRequest.ContentType = Header.Value;

        if (!(System.String.IsNullOrWhiteSpace(this.AuthorizationBearerToken)))
        {
          HttpWebRequest.UseDefaultCredentials = false;
          HttpWebRequest.PreAuthenticate = true;
          HttpWebRequest.Headers.Add("Authorization", "Bearer " + this.AuthorizationBearerToken);
        }

        if (this.Body.ValueKind == System.Text.Json.JsonValueKind.Undefined)
          HttpWebRequest.ContentLength = 0;
        else
        {
          System.Byte[] BodyBytes = new System.Text.UTF8Encoding().GetBytes(Body.ToString());
          HttpWebRequest.ContentType = SoftmakeAll.SDK.Communication.REST.DefaultContentType;
          HttpWebRequest.ContentLength = BodyBytes.Length;
          HttpWebRequest.GetRequestStream().Write(BodyBytes, 0, BodyBytes.Length);
        }

        if (this.Timeout > 0)
          HttpWebRequest.Timeout = this.Timeout;

        using (System.Net.HttpWebResponse HttpWebResponse = (System.Net.HttpWebResponse)HttpWebRequest.GetResponse())
        {
          this._StatusCode = HttpWebResponse.StatusCode;
          Result = this.ReadResponseStream(HttpWebResponse);
        }
      }
      catch (System.Net.WebException ex)
      {
        this._HasRequestErrors = true;
        System.Net.HttpWebResponse HttpWebResponse = ex.Response as System.Net.HttpWebResponse;
        if (HttpWebResponse == null)
        {
          this._StatusCode = System.Net.HttpStatusCode.InternalServerError;
          Result = new { Error = true, Message = ex.Message }.ToJsonElement();
          return Result;
        }
        this._StatusCode = HttpWebResponse.StatusCode;
        Result = this.ReadResponseStream(HttpWebResponse);
      }
      catch (System.Exception ex)
      {
        this._HasRequestErrors = true;
        this._StatusCode = System.Net.HttpStatusCode.InternalServerError;
        Result = new { Error = true, Message = ex.Message }.ToJsonElement();
      }

      return Result;
    }
    public async System.Threading.Tasks.Task<System.Text.Json.JsonElement> SendAsync()
    {
      if (System.String.IsNullOrWhiteSpace(this.URL))
        throw new System.Exception(SoftmakeAll.SDK.Communication.REST.EmptyURLErrorMessage);

      this._HasRequestErrors = false;

      if (System.String.IsNullOrWhiteSpace(this.Method))
        this.Method = SoftmakeAll.SDK.Communication.REST.DefaultMethod;

      System.Text.Json.JsonElement Result = new System.Text.Json.JsonElement();
      try
      {
        System.Net.HttpWebRequest HttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(this.URL);
        HttpWebRequest.Method = this.Method;

        if (this.Headers.Count > 0)
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> Header in this.Headers)
            if (Header.Key != "Content-Type")
              HttpWebRequest.Headers.Add(Header.Key, Header.Value);
            else
              HttpWebRequest.ContentType = Header.Value;

        if (!(System.String.IsNullOrWhiteSpace(this.AuthorizationBearerToken)))
        {
          HttpWebRequest.UseDefaultCredentials = false;
          HttpWebRequest.PreAuthenticate = true;
          HttpWebRequest.Headers.Add("Authorization", "Bearer " + this.AuthorizationBearerToken);
        }

        if (this.Body.ValueKind == System.Text.Json.JsonValueKind.Undefined)
          HttpWebRequest.ContentLength = 0;
        else
        {
          System.Byte[] BodyBytes = new System.Text.UTF8Encoding().GetBytes(Body.ToString());
          HttpWebRequest.ContentType = SoftmakeAll.SDK.Communication.REST.DefaultContentType;
          HttpWebRequest.ContentLength = BodyBytes.Length;
          HttpWebRequest.GetRequestStream().Write(BodyBytes, 0, BodyBytes.Length);
        }

        if (this.Timeout > 0)
          HttpWebRequest.Timeout = this.Timeout;

        using (System.Net.HttpWebResponse HttpWebResponse = (System.Net.HttpWebResponse)await HttpWebRequest.GetResponseAsync())
        {
          this._StatusCode = HttpWebResponse.StatusCode;
          Result = await this.ReadResponseStreamAsync(HttpWebResponse);
        }
      }
      catch (System.Net.WebException ex)
      {
        this._HasRequestErrors = true;
        System.Net.HttpWebResponse HttpWebResponse = ex.Response as System.Net.HttpWebResponse;
        if (HttpWebResponse == null)
        {
          this._StatusCode = System.Net.HttpStatusCode.InternalServerError;
          Result = new { Error = true, Message = ex.Message }.ToJsonElement();
          return Result;
        }
        this._StatusCode = HttpWebResponse.StatusCode;
        Result = await this.ReadResponseStreamAsync(HttpWebResponse);
      }
      catch (System.Exception ex)
      {
        this._HasRequestErrors = true;
        this._StatusCode = System.Net.HttpStatusCode.InternalServerError;
        Result = new { Error = true, Message = ex.Message }.ToJsonElement();
      }

      return Result;
    }
    public async System.Threading.Tasks.Task<System.Text.Json.JsonElement> SendFilesAsync(System.Collections.Generic.Dictionary<System.String, SoftmakeAll.SDK.Communication.REST.File> FormData)
    {
      if (System.String.IsNullOrWhiteSpace(this.URL))
        throw new System.Exception(SoftmakeAll.SDK.Communication.REST.EmptyURLErrorMessage);

      this._HasRequestErrors = false;

      using (System.Net.Http.HttpClient HttpClient = new System.Net.Http.HttpClient())
      {
        if (this.Headers.Count > 0)
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> Header in this.Headers)
            HttpClient.DefaultRequestHeaders.Add(Header.Key, Header.Value);

        if (!(System.String.IsNullOrWhiteSpace(this.AuthorizationBearerToken)))
          HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + this.AuthorizationBearerToken);

        using (System.Net.Http.MultipartFormDataContent MultipartFormDataContent = new System.Net.Http.MultipartFormDataContent())
        {
          if ((FormData != null) && (FormData.Count > 0))
            foreach (System.Collections.Generic.KeyValuePair<System.String, SoftmakeAll.SDK.Communication.REST.File> Item in FormData)
              if ((System.String.IsNullOrWhiteSpace(Item.Key)) || (Item.Value == null))
                continue;
              else
                MultipartFormDataContent.Add(new System.Net.Http.ByteArrayContent(Item.Value.Contents), Item.Key, Item.Value.Name);

          if (this.Timeout > 0)
            HttpClient.Timeout = System.TimeSpan.FromMilliseconds(this.Timeout);

          System.Net.Http.HttpResponseMessage HttpResponseMessage = await HttpClient.PostAsync(this.URL, MultipartFormDataContent);
          try
          {
            System.Byte[] APIResult = await HttpResponseMessage.Content.ReadAsByteArrayAsync();
            if (APIResult == null)
              return new System.Text.Json.JsonElement();

            return System.Text.Encoding.UTF8.GetString(APIResult).ToJsonElement();
          }
          catch
          {
            return new System.Text.Json.JsonElement();
          }
        }
      }
    }
    public async System.Threading.Tasks.Task<SoftmakeAll.SDK.Communication.REST.File> DownloadFileAsync()
    {
      if (System.String.IsNullOrWhiteSpace(this.URL))
        throw new System.Exception(SoftmakeAll.SDK.Communication.REST.EmptyURLErrorMessage);

      this._HasRequestErrors = false;

      this.Method = SoftmakeAll.SDK.Communication.REST.DefaultMethod;

      SoftmakeAll.SDK.Communication.REST.File Result = null;
      try
      {
        System.Net.HttpWebRequest HttpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(this.URL);
        HttpWebRequest.Method = this.Method;

        if (this.Headers.Count > 0)
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.String> Header in this.Headers)
            HttpWebRequest.Headers.Add(Header.Key, Header.Value);

        if (!(System.String.IsNullOrWhiteSpace(this.AuthorizationBearerToken)))
        {
          HttpWebRequest.UseDefaultCredentials = false;
          HttpWebRequest.PreAuthenticate = true;
          HttpWebRequest.Headers.Add("Authorization", "Bearer " + this.AuthorizationBearerToken);
        }

        if (this.Body.ValueKind == System.Text.Json.JsonValueKind.Undefined)
          HttpWebRequest.ContentLength = 0;
        else
        {
          System.Byte[] BodyBytes = new System.Text.UTF8Encoding().GetBytes(Body.ToString());
          HttpWebRequest.ContentType = SoftmakeAll.SDK.Communication.REST.DefaultContentType;
          HttpWebRequest.ContentLength = BodyBytes.Length;
          System.IO.Stream RequestStream = await HttpWebRequest.GetRequestStreamAsync();
          RequestStream.Write(BodyBytes, 0, BodyBytes.Length);
        }

        if (this.Timeout > 0)
          HttpWebRequest.Timeout = this.Timeout;

        using (System.Net.HttpWebResponse HttpWebResponse = (System.Net.HttpWebResponse)await HttpWebRequest.GetResponseAsync())
        {
          this._StatusCode = HttpWebResponse.StatusCode;

          System.Net.WebClient WebClient = new System.Net.WebClient();
          if (!(System.String.IsNullOrWhiteSpace(this.AuthorizationBearerToken)))
          {
            WebClient.UseDefaultCredentials = false;
            WebClient.Headers.Add("Authorization", "Bearer " + this.AuthorizationBearerToken);
          }
          System.Byte[] FileContents = WebClient.DownloadData(this.URL);

          if ((FileContents == null) || (FileContents.Length == 0))
            return null;

          System.String FileName = "";

          try
          {
            const System.String Key = "filename=";
            FileName = HttpWebResponse.Headers.Get("Content-Disposition");
            FileName = FileName.Substring(FileName.IndexOf(Key) + Key.Length);
            FileName = FileName.Substring(0, FileName.IndexOf(';'));
          }
          catch
          {
            FileName = "";
          }

          return new SoftmakeAll.SDK.Communication.REST.File() { Name = FileName, Contents = FileContents };
        }
      }
      catch (System.Net.WebException ex)
      {
        this._HasRequestErrors = true;
        System.Net.HttpWebResponse HttpWebResponse = ex.Response as System.Net.HttpWebResponse;
        if (HttpWebResponse == null)
        {
          this._StatusCode = System.Net.HttpStatusCode.InternalServerError;
          Result = null;
          return Result;
        }
        this._StatusCode = HttpWebResponse.StatusCode;
        Result = null;
      }
      catch
      {
        this._HasRequestErrors = true;
        this._StatusCode = System.Net.HttpStatusCode.InternalServerError;
        Result = null;
      }

      return Result;
    }
    private System.Text.Json.JsonElement ReadResponseStream(System.Net.HttpWebResponse HttpWebResponse)
    {
      System.Text.Json.JsonElement Result = new System.Text.Json.JsonElement();

      if (HttpWebResponse == null)
        return Result;

      using (System.IO.Stream Stream = HttpWebResponse.GetResponseStream())
        if (Stream.CanRead)
          using (System.IO.StreamReader StreamReader = new System.IO.StreamReader(Stream))
            return StreamReader.ReadToEnd().ToJsonElement();

      return Result;
    }
    private async System.Threading.Tasks.Task<System.Text.Json.JsonElement> ReadResponseStreamAsync(System.Net.HttpWebResponse HttpWebResponse)
    {
      System.Text.Json.JsonElement Result = new System.Text.Json.JsonElement();

      if (HttpWebResponse == null)
        return Result;

      using (System.IO.Stream Stream = HttpWebResponse.GetResponseStream())
        if (Stream.CanRead)
          using (System.IO.StreamReader StreamReader = new System.IO.StreamReader(Stream))
          {
            System.String ResultString = await StreamReader.ReadToEndAsync();
            return ResultString.ToJsonElement();
          }

      return Result;
    }
    public void ClearProperties() { this.ClearProperties(false); }
    public void ClearProperties(System.Boolean PreserveHeaders)
    {
      this.URL = null;
      this.Method = SoftmakeAll.SDK.Communication.REST.DefaultMethod;

      if (!(PreserveHeaders))
        this.Headers.Clear();

      this.AuthorizationBearerToken = null;
      this.Body = new System.Text.Json.JsonElement();
      this.Timeout = 0;
      this._HasRequestErrors = false;
      this._StatusCode = System.Net.HttpStatusCode.OK;
    }
    #endregion
  }
}