using System.Linq;
using System.Net.Http;

namespace SoftmakeAll.SDK.Networking.Http
{
  public static class Client
  {
    #region Methods
    //public static SoftmakeAll.SDK.Networking.Http.Response Send(SoftmakeAll.SDK.Networking.Http.Request Request) => SoftmakeAll.SDK.Networking.Http.Client.Send(Request);
    public static async System.Threading.Tasks.Task<SoftmakeAll.SDK.Networking.Http.Response> SendAsync(SoftmakeAll.SDK.Networking.Http.Request Request) => await SoftmakeAll.SDK.Networking.Http.Client.SendAsync(Request, System.Threading.CancellationToken.None);
    public static async System.Threading.Tasks.Task<SoftmakeAll.SDK.Networking.Http.Response> SendAsync(SoftmakeAll.SDK.Networking.Http.Request Request, System.Threading.CancellationToken CancellationToken)
    {
      if (Request == null)
        throw new System.ArgumentNullException("Request");

      if (Request.URL == null)
        throw new System.ArgumentNullException("Request.URL");

      // QueryParameters
      System.Uri RequestURL = Request.URL;
      System.String QueryString = Request.GetQueryString();
      if (!(System.String.IsNullOrWhiteSpace(QueryString)))
        RequestURL = new System.Uri($"{Request.URL.OriginalString[0..(Request.URL.OriginalString.IndexOf('?'))]}{QueryString}");

      if (CancellationToken.IsCancellationRequested)
        return null;

      try
      {
        using (System.Net.Http.HttpClient HttpClient = new System.Net.Http.HttpClient() { BaseAddress = RequestURL })
        using (System.Net.Http.HttpRequestMessage HttpRequestMessage = new System.Net.Http.HttpRequestMessage(Request.Method, RequestURL.PathAndQuery))
        {
          // Request Headers
          foreach (System.Collections.Generic.KeyValuePair<System.String, System.Collections.Generic.List<System.String>> RequestHeader in Request.GetHeaders().Where(h => ((h.Key != "Content-Length") && (h.Key != "Content-Type"))))
            HttpRequestMessage.Headers.Add(RequestHeader.Key, RequestHeader.Value);

          // Ignore Request Body for these Methods
          HttpRequestMessage.Content = null;
          System.String[] IgnoreRequestBodyMethods = new System.String[] { "GET", "HEAD", "CONNECT", "OPTIONS", "DELETE" };
          if ((!(IgnoreRequestBodyMethods.Any(i => i == Request.Method.Method.ToUpper()))) && (Request.Body != null))
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

          System.Diagnostics.Stopwatch Stopwatch = new System.Diagnostics.Stopwatch();
          Stopwatch.Start();

          if (CancellationToken.IsCancellationRequested)
            return null;

          using (System.Net.Http.HttpResponseMessage HttpResponseMessage = await HttpClient.SendAsync(HttpRequestMessage, CancellationToken))
          {
            Stopwatch.Stop();

            if (CancellationToken.IsCancellationRequested)
              return null;


            SoftmakeAll.SDK.Networking.Http.Response Response = new SoftmakeAll.SDK.Networking.Http.Response() { ElapsedTime = Stopwatch.Elapsed };

            // Response Headers
            if (HttpResponseMessage.Headers != null)
              foreach (var ResponseHeader in HttpResponseMessage.Headers)
                if (ResponseHeader.Value != null)
                {
                  System.String Value = System.String.Join("|", ResponseHeader.Value);
                  if (!(System.String.IsNullOrWhiteSpace(Value)))
                    Response.AddHeader(ResponseHeader.Key, Value);
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
                  {
                    System.String Value = System.String.Join("|", ResponseHeader.Value);
                    if (!(System.String.IsNullOrWhiteSpace(Value)))
                      Response.AddHeader(ResponseHeader.Key, Value);
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
                  Response.SetBody(Data, "empty");
                  Response.RemoveHeader("Content-Type");
                }
                else if ((ContentType != null) && (Data != null))
                {
                  Response.SetBody(Data, ContentType);
                }
              }
            }
            catch { }

            return Response;
          }
        }
      }
      catch { }

      return null;
    }
    #endregion
  }
}