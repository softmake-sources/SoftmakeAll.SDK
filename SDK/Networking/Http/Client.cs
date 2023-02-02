using System.Linq;

namespace SoftmakeAll.SDK.Networking.Http
{
  public static class Client
  {
    #region Methods
    public static SoftmakeAll.SDK.Networking.Http.Response Send(SoftmakeAll.SDK.Networking.Http.Request Request) => SoftmakeAll.SDK.Networking.Http.Client.SendAsync(Request, System.TimeSpan.FromSeconds(100.0D), System.Threading.CancellationToken.None).Result;
    public static SoftmakeAll.SDK.Networking.Http.Response Send(SoftmakeAll.SDK.Networking.Http.Request Request, System.TimeSpan Timeout) => SoftmakeAll.SDK.Networking.Http.Client.SendAsync(Request, Timeout, System.Threading.CancellationToken.None).Result;

    public static async System.Threading.Tasks.Task<SoftmakeAll.SDK.Networking.Http.Response> SendAsync(SoftmakeAll.SDK.Networking.Http.Request Request) => await SoftmakeAll.SDK.Networking.Http.Client.SendAsync(Request, System.TimeSpan.FromSeconds(100.0D), System.Threading.CancellationToken.None);
    public static async System.Threading.Tasks.Task<SoftmakeAll.SDK.Networking.Http.Response> SendAsync(SoftmakeAll.SDK.Networking.Http.Request Request, System.TimeSpan Timeout) => await SoftmakeAll.SDK.Networking.Http.Client.SendAsync(Request, Timeout, System.Threading.CancellationToken.None);
    public static async System.Threading.Tasks.Task<SoftmakeAll.SDK.Networking.Http.Response> SendAsync(SoftmakeAll.SDK.Networking.Http.Request Request, System.Threading.CancellationToken CancellationToken) => await SoftmakeAll.SDK.Networking.Http.Client.SendAsync(Request, System.TimeSpan.FromSeconds(100.0D), System.Threading.CancellationToken.None);
    public static async System.Threading.Tasks.Task<SoftmakeAll.SDK.Networking.Http.Response> SendAsync(SoftmakeAll.SDK.Networking.Http.Request Request, System.TimeSpan Timeout, System.Threading.CancellationToken CancellationToken)
    {
      /*
       * REQUEST
       */

      if (Request == null)
        throw new System.ArgumentNullException("Request");

      if (System.String.IsNullOrWhiteSpace(Request.URI))
        throw new System.ArgumentNullException("Request.URI");

      // Request QueryParameters
      System.Uri RequestURL;
      if (!(System.Uri.TryCreate(Request.URI, System.UriKind.Absolute, out RequestURL)))
        throw new System.UriFormatException("Request.URI");

      System.String QueryString = Request.GetQueryString();
      if (!(System.String.IsNullOrWhiteSpace(QueryString)))
        RequestURL = new System.Uri($"{Request.URI[0..(Request.URI.IndexOf('?'))]}{QueryString}");

      if (CancellationToken.IsCancellationRequested)
        return null;

      try
      {
        using (System.Net.Http.HttpClient HttpClient = new System.Net.Http.HttpClient() { BaseAddress = RequestURL })
        using (System.Net.Http.HttpRequestMessage HttpRequestMessage = new System.Net.Http.HttpRequestMessage(Request.Method, RequestURL.PathAndQuery))
        {
          // Request Headers
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.Collections.Generic.List<System.String>> RequestHeader in Request.GetHeaders().Where(h => ((h.Key != "Content-Length") && (h.Key != "Content-Type") && (h.Key != "Cookie"))))
            HttpRequestMessage.Headers.Add(RequestHeader.Key, RequestHeader.Value);

          System.String CookieHeaderValue = Request.GetHeader("Cookie", ";");
          if (!(System.String.IsNullOrWhiteSpace(CookieHeaderValue)))
            HttpRequestMessage.Headers.Add("Cookie", CookieHeaderValue);

          // Ignore Request Body for these Methods
          HttpRequestMessage.Content = null;
          if ((!(new System.String[] { "GET", "HEAD", "CONNECT", "OPTIONS", "DELETE" }.Any(i => i == Request.Method.Method.ToUpper()))) && (Request.Body != null))
          {
            System.String ContentType = (Request.GetHeaderValue("Content-Type") ?? "").ToLower();

            switch (ContentType)
            {
              case "application/x-www-form-urlencoded":
                HttpRequestMessage.Content = new System.Net.Http.FormUrlEncodedContent(Request.GetFormUrlEncodedParameters());
                break;

              case "multipart/form-data":
                System.Net.Http.MultipartFormDataContent MultipartFormDataContent = new System.Net.Http.MultipartFormDataContent();
                foreach (System.Collections.Generic.KeyValuePair<System.String, SoftmakeAll.SDK.Networking.Http.MultipartFormDataParameter> MultipartFormDataParameter in Request.GetMultipartFormDataParameters())
                  if (MultipartFormDataParameter.Value.Data == null)
                    MultipartFormDataContent.Add(new System.Net.Http.StringContent(MultipartFormDataParameter.Value.FileNameOrValue), MultipartFormDataParameter.Key);
                  else
                  {
                    System.Net.Http.HttpContent ByteArrayContent = new System.Net.Http.ByteArrayContent(MultipartFormDataParameter.Value.Data);
                    ByteArrayContent.Headers.Add("Content-Type", MultipartFormDataParameter.Value.DataContentType);
                    MultipartFormDataContent.Add(ByteArrayContent, MultipartFormDataParameter.Key, MultipartFormDataParameter.Value.FileNameOrValue);
                  }

                HttpRequestMessage.Content = MultipartFormDataContent;
                break;

              default:
                HttpRequestMessage.Content = new System.Net.Http.ByteArrayContent(Request.Body);

                if (!(System.String.IsNullOrWhiteSpace(ContentType)))
                  HttpRequestMessage.Content.Headers.Add("Content-Type", ContentType);

                System.String ContentLength = Request.GetHeaderValue("Content-Length");
                if (!(System.String.IsNullOrWhiteSpace(ContentLength)))
                  HttpRequestMessage.Content.Headers.Add("Content-Length", ContentLength);

                break;
            }
          }

          if (CancellationToken.IsCancellationRequested)
            return null;


          /*
           * RESPONSE
           */

          HttpClient.Timeout = Timeout;
          System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();
          Stopwatch.Start();
          using (System.Net.Http.HttpResponseMessage HttpResponseMessage = await HttpClient.SendAsync(HttpRequestMessage, CancellationToken))
          {
            Stopwatch.Stop();

            if (CancellationToken.IsCancellationRequested)
              return null;


            SoftmakeAll.SDK.Networking.Http.Response Response = new SoftmakeAll.SDK.Networking.Http.Response(HttpResponseMessage.StatusCode, Stopwatch.Elapsed);

            // Response Headers
            if (HttpResponseMessage.Headers != null)
              foreach (var ResponseHeader in HttpResponseMessage.Headers)
                if ((ResponseHeader.Value != null) && (ResponseHeader.Value.Any()))
                {
                  if (ResponseHeader.Key.ToLower() != "set-cookie")
                    foreach (System.String ResponseHeaderValue in ResponseHeader.Value)
                      Response.AddHeader(ResponseHeader.Key, ResponseHeaderValue);
                  else
                    foreach (System.String ResponseHeaderValue in ResponseHeader.Value)
                      try
                      {
                        System.String[] SplittedCookieProperties = ResponseHeaderValue.Split("; ");
                        System.String[] SplittedCookieID = SplittedCookieProperties[0].Split('=');
                        System.String Name = SplittedCookieID[0];
                        System.String Value = SplittedCookieID.Length < 2 ? null : SplittedCookieID[1];

                        System.String Expires = SplittedCookieProperties.FirstOrDefault(c => c.StartsWith("expires"));
                        Expires = System.String.IsNullOrWhiteSpace(Expires) ? null : Expires[8..^0];

                        System.String Domain = SplittedCookieProperties.FirstOrDefault(c => c.StartsWith("domain"));
                        Domain = System.String.IsNullOrWhiteSpace(Domain) ? null : Domain[7..^0];

                        System.String Path = SplittedCookieProperties.FirstOrDefault(c => c.StartsWith("path"));
                        Path = System.String.IsNullOrWhiteSpace(Path) ? null : Path[5..^0];

                        System.Boolean Secure = ResponseHeaderValue.Contains("; secure");
                        System.Boolean HttpOnly = ResponseHeaderValue.Contains("; httponly");

                        System.Net.Cookie Cookie = new System.Net.Cookie(Name, Value, Path, Domain) { Secure = Secure, HttpOnly = HttpOnly };
                        if (System.DateTime.TryParse(Expires, out System.DateTime ExpiresDateTime))
                          Cookie.Expires = ExpiresDateTime;

                        Response.AddCookie(Cookie);
                        Request.AddCookie(Cookie);

                        /*
                        //NOT EXISTS on the Cookie object.... :
                        //System.String MaxAge = SplittedCookieProperties.FirstOrDefault(c => c.StartsWith("max-age"));
                        //MaxAge = System.String.IsNullOrWhiteSpace(MaxAge) ? null : MaxAge[8..^0];
                        //Cookie.MaxAge = MaxAge;

                        //System.String SameSite = SplittedCookieProperties.FirstOrDefault(c => c.StartsWith("samesite"));
                        //SameSite = System.String.IsNullOrWhiteSpace(SameSite) ? null : SameSite[9..^0];
                        //Cookie.SameSite = SameSite;
                        */
                      }
                      catch (System.Exception ex)
                      {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                      }
                }

            try
            {
              // Response Body
              using (System.Net.Http.HttpContent HttpContent = HttpResponseMessage.Content)
              {
                System.Byte[] Data = null;
                if (HttpContent != null)
                {
                  Data = await HttpContent.ReadAsByteArrayAsync(CancellationToken);
                  foreach (var ResponseHeader in HttpContent.Headers)
                    if ((ResponseHeader.Value != null) && (ResponseHeader.Value.Any()))
                      foreach (System.String ResponseHeaderValue in ResponseHeader.Value)
                        Response.AddHeader(ResponseHeader.Key, ResponseHeaderValue);

                  try
                  {
                    System.String ContentEncoding = Response.GetHeaderValue("Content-Encoding")?.ToLower();
                    if (new System.String[] { "gzip", "deflate" }.Any(i => i == ContentEncoding))
                    {
                      using (System.IO.MemoryStream CompressedMemoryStream = new System.IO.MemoryStream(Data))
                      using (System.IO.MemoryStream DecompressedMemoryStream = new System.IO.MemoryStream())
                      {
                        System.IO.Stream DecompressionStream = null;
                        switch (ContentEncoding)
                        {
                          case "gzip": DecompressionStream = new System.IO.Compression.GZipStream(CompressedMemoryStream, System.IO.Compression.CompressionMode.Decompress, false); break;
                          case "deflate": DecompressionStream = new System.IO.Compression.DeflateStream(CompressedMemoryStream, System.IO.Compression.CompressionMode.Decompress, false); break;
                        }

                        if (DecompressionStream != null)
                        {
                          await DecompressionStream.CopyToAsync(DecompressedMemoryStream, CancellationToken);
                          if (CancellationToken.IsCancellationRequested)
                          {
                            await DecompressionStream.DisposeAsync();
                            return null;
                          }

                          Data = DecompressedMemoryStream.ToArray();
                          await DecompressionStream.DisposeAsync();
                        }
                      }
                    }
                  }
                  catch (System.Exception ex)
                  {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                  }
                }

                System.String ContentType = Response.GetHeaderValue("Content-Type");
                ContentType = System.String.IsNullOrWhiteSpace(ContentType) ? null : ContentType.ToLower();

                if ((ContentType == null) && (Data == null))
                {
                  Response.RemoveHeader("Content-Type");
                }
                else if ((ContentType == null) && (Data != null))
                {
                  Response.SetBody(Data, "/");
                  Response.RemoveHeader("Content-Type");
                }
                else if ((ContentType != null) && (Data != null))
                {
                  Response.SetBody(Data, ContentType);
                }

                System.String ContentDisposition = Response.GetHeaderValue("Content-Disposition");
                if (!(System.String.IsNullOrWhiteSpace(ContentDisposition)))
                  Response.SetFileDetails(ContentDisposition, ContentType);
              }
            }
            catch (System.Exception ex)
            {
              System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return CancellationToken.IsCancellationRequested ? null : Response;
          }
        }
      }
      catch (System.Exception ex)
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
      }

      return null;
    }
    #endregion
  }
}