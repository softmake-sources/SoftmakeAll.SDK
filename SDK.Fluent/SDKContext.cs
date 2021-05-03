using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Fluent
{
  public static class SDKContext
  {
    #region Fields
    internal static System.String APIBaseAddress = "https://smallservices01.azurewebsites.net";
    private static Microsoft.Identity.Client.IPublicClientApplication PublicClientApplication = null;
    private static Microsoft.Identity.Client.AuthenticationResult AuthenticationResult = null;
    private static SoftmakeAll.SDK.Fluent.Authentication.ICredentials Credentials = null;

    public static readonly SoftmakeAll.SDK.OperationResult LastOperationResult = new SoftmakeAll.SDK.OperationResult();
    #endregion

    #region Methods
    private static Microsoft.Identity.Client.IPublicClientApplication CreatePublicClientApplication(System.Guid ContextIdentifier, System.String PolicyName) => SoftmakeAll.SDK.Fluent.SDKContext.CreatePublicClientApplication(ContextIdentifier, PolicyName, null);
    private static Microsoft.Identity.Client.IPublicClientApplication CreatePublicClientApplication(System.Guid ContextIdentifier, System.String PolicyName, System.String RedirectURI)
      => Microsoft.Identity.Client.PublicClientApplicationBuilder
      .Create(ContextIdentifier.ToString())
      .WithB2CAuthority($"https://softmakeb2c.b2clogin.com/tfp/softmakeb2c.onmicrosoft.com/B2C_1{PolicyName}")
      .WithRedirectUri(RedirectURI)
      .Build();

    public static void Authenticate(SoftmakeAll.SDK.Fluent.Authentication.ICredentials Credentials) => SoftmakeAll.SDK.Fluent.SDKContext.AuthenticateAsync(Credentials).Wait();
    public static async System.Threading.Tasks.Task AuthenticateAsync(SoftmakeAll.SDK.Fluent.Authentication.ICredentials Credentials)
    {
      if (SoftmakeAll.SDK.Fluent.SDKContext.Credentials?.ObjectID != Credentials.ObjectID)
      {
        SoftmakeAll.SDK.Fluent.SDKContext.Credentials = Credentials;
        SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult = null;
        SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication = null;

        if (Credentials.AuthType == 'A') // FromAccessKey
          Credentials.Authorization = $"Basic {System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{Credentials.ClientID}:{Credentials.ClientSecret}"))}";
      }

      if ((Credentials.AuthType != 'A') && ((SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult == null) || (SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult.ExpiresOn.Subtract(System.DateTimeOffset.UtcNow).TotalMinutes <= 5.0D)))
      {
        System.String[] Scopes = new System.String[] { "openid", "https://softmakeb2c.onmicrosoft.com/48512da7-b030-4e62-be61-9e19b2c52d8a/user_impersonation" };

        if (Credentials.AuthType == 'C') // FromCredentials
        {
          System.Security.SecureString Password = new System.Security.SecureString();
          foreach (System.Char Char in Credentials.ClientSecret)
            Password.AppendChar(Char);
          Password.MakeReadOnly();

          if (SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication == null)
            SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication = SoftmakeAll.SDK.Fluent.SDKContext.CreatePublicClientApplication(Credentials.ContextIdentifier, "_ROPC");

          try
          {
            SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult = await SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication.AcquireTokenByUsernamePassword(Scopes, Credentials.ClientID, Password).ExecuteAsync();
            Password?.Dispose();
            Password = null;
          }
          catch
          {
            Password?.Dispose();
            Password = null;
            SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication = null;
            SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult = null;
            throw new System.Exception();
          }
        }
        else if (Credentials.AuthType == 'I') // FromInteractive
        {
          if (SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication == null)
            SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication = SoftmakeAll.SDK.Fluent.SDKContext.CreatePublicClientApplication(Credentials.ContextIdentifier, "A_signup_signin", "http://localhost:1435");

          try
          {
            SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult = await SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication.AcquireTokenInteractive(Scopes).WithPrompt(Microsoft.Identity.Client.Prompt.ForceLogin).ExecuteAsync();
          }
          catch
          {
            SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication = null;
            SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult = null;
            throw new System.Exception();
          }
        }

        Credentials.Authorization = $"Bearer {SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult.AccessToken}";
      }
    }
    public static SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> MakeRESTRequest(SoftmakeAll.SDK.Communication.REST REST)
    {
      if (REST == null)
        return null;

      System.Boolean RemoveAuthorization = false;
      if ((REST.Headers != null) && (!(REST.Headers.ContainsKey("Authorization"))))
        try
        {
          SoftmakeAll.SDK.Fluent.SDKContext.Authenticate(SoftmakeAll.SDK.Fluent.SDKContext.Credentials);
          REST.Headers.Add("Authorization", SoftmakeAll.SDK.Fluent.SDKContext.Credentials.Authorization);
          RemoveAuthorization = true;
        }
        catch { }

      REST.URL = $"{SoftmakeAll.SDK.Fluent.SDKContext.APIBaseAddress}/API/{REST.URL}";
      System.Text.Json.JsonElement RESTResult = REST.Send();

      if (RemoveAuthorization)
        REST.Headers.Remove("Authorization");

      return SoftmakeAll.SDK.Fluent.SDKContext.ProcessRESTRequestResult(REST, RESTResult);
    }
    public static async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> MakeRESTRequestAsync(SoftmakeAll.SDK.Communication.REST REST)
    {
      if (REST == null)
        return null;

      System.Boolean RemoveAuthorization = false;
      if ((REST.Headers != null) && (!(REST.Headers.ContainsKey("Authorization"))))
        try
        {
          await SoftmakeAll.SDK.Fluent.SDKContext.AuthenticateAsync(SoftmakeAll.SDK.Fluent.SDKContext.Credentials);
          REST.Headers.Add("Authorization", SoftmakeAll.SDK.Fluent.SDKContext.Credentials.Authorization);
          RemoveAuthorization = true;
        }
        catch { }

      REST.URL = $"{SoftmakeAll.SDK.Fluent.SDKContext.APIBaseAddress}/API/{REST.URL}";
      System.Text.Json.JsonElement RESTResult = await REST.SendAsync();

      if (RemoveAuthorization)
        REST.Headers.Remove("Authorization");

      return SoftmakeAll.SDK.Fluent.SDKContext.ProcessRESTRequestResult(REST, RESTResult);
    }
    private static SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> ProcessRESTRequestResult(SoftmakeAll.SDK.Communication.REST REST, System.Text.Json.JsonElement RESTResult)
    {
      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> Result = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();
      Result.ExitCode = RESTResult.GetInt32("ExitCode");
      Result.Message = RESTResult.GetString("Message");
      Result.ID = RESTResult.GetString("ID");
      Result.Count = RESTResult.GetInt32("Count");
      Result.Data = RESTResult.GetJsonElement("Data").ToObject<System.Text.Json.JsonElement>();

      if ((REST.HasRequestErrors) && (Result.ExitCode == 0))
        Result.ExitCode = (int)REST.StatusCode;

      return Result;
    }
    #endregion
  }
}