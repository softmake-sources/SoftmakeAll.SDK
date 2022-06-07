using SoftmakeAll.SDK.Helpers.JSON.Extensions;
using System.Linq;

namespace SoftmakeAll.SDK.Fluent
{
  /// <summary>
  /// The SDK Context to authenticate and perform requests.
  /// </summary>
  public static class SDKContext
  {
    #region Constants
    /// <summary>
    /// Allows to acquire a new access token from the time specified before expiration.
    /// </summary>
    private const System.Double ExpirationTime = 5.0D;
    #endregion

    #region Fields
    /// <summary>
    /// Credentials to use after authentication process.
    /// </summary>
    private static SoftmakeAll.SDK.Fluent.Authentication.ICredentials InMemoryCredentials = null;

    /// <summary>
    /// PublicClientApplication to perform Authentication.
    /// </summary>
    private static Microsoft.Identity.Client.IPublicClientApplication PublicClientApplication = null;

    /// <summary>
    /// Scopes requested to access a protected API
    /// </summary>
    private static System.String[] Scopes = new System.String[] { "https://softmakeb2c.onmicrosoft.com/48512da7-b030-4e62-be61-9e19b2c52d8a/user_impersonation", "openid", "offline_access" };

    /// <summary>
    /// Authentication result from PublicClientApplication.
    /// </summary>
    private static Microsoft.Identity.Client.AuthenticationResult AuthenticationResult = null;

    /// <summary>
    /// Softmake All WebSocket Connection.
    /// </summary>
    public static readonly SoftmakeAll.SDK.Fluent.Notifications.IClientWebSocket ClientWebSocket = new SoftmakeAll.SDK.Fluent.Notifications.ClientSignalRWebSocket();

    /// <summary>
    /// The last resource operation result. This object will be changed when action is performed. Actions: List, Show, Create, Modify, Replace and Delete.
    /// </summary>
    public static readonly SoftmakeAll.SDK.OperationResult LastOperationResult = new SoftmakeAll.SDK.OperationResult();
    #endregion

    #region Properties
    /// <summary>
    /// The Softmake All server address.
    /// </summary>
    private static System.String DefaultServer = "smallservices01.azurewebsites.net";

    /// <summary>
    /// The Softmake All API base address.
    /// </summary>
    internal static System.String APIBaseAddress { get => $"https://{SoftmakeAll.SDK.Fluent.SDKContext.DefaultServer}"; }

    /// <summary>
    /// The Softmake All WebSocket base address.
    /// </summary>
    internal static System.String WebSocketBaseAddress { get => $"wss://{SoftmakeAll.SDK.Fluent.SDKContext.DefaultServer}/nhb"; }
    #endregion

    #region Methods
    /// <summary>
    /// Sets the new Server Address.
    /// </summary>
    public static void ConfigureConnections(System.String ServerAddress) => SoftmakeAll.SDK.Fluent.SDKContext.DefaultServer = ServerAddress;

    /// <summary>
    /// Creates a PublicClientApplication.
    /// </summary>
    /// <param name="ContextIdentifier">Your Public Application Client ID.</param>
    /// <param name="PolicyName">The Azure AD B2C policy name.</param>
    /// <returns>A PublicClientApplication.</returns>
    private static Microsoft.Identity.Client.IPublicClientApplication CreatePublicClientApplication(System.Guid ContextIdentifier, System.String PolicyName) => SoftmakeAll.SDK.Fluent.SDKContext.CreatePublicClientApplication(ContextIdentifier, PolicyName, null);

    /// <summary>
    /// Creates a PublicClientApplication with redirect URI.
    /// </summary>
    /// <param name="ContextIdentifier">Your Public Application Client ID.</param>
    /// <param name="PolicyName">The Azure AD B2C policy name.</param>
    /// <param name="RedirectURI">URI to redirect after login succeeded.</param>
    /// <returns>A PublicClientApplication.</returns>
    private static Microsoft.Identity.Client.IPublicClientApplication CreatePublicClientApplication(System.Guid ContextIdentifier, System.String PolicyName, System.String RedirectURI)
    {
      Microsoft.Identity.Client.IPublicClientApplication PublicClientApplication;

      if (System.String.IsNullOrWhiteSpace(RedirectURI))
        PublicClientApplication = Microsoft.Identity.Client.PublicClientApplicationBuilder
        .Create(ContextIdentifier.ToString())
        .WithB2CAuthority($"https://softmakeb2c.b2clogin.com/tfp/softmakeb2c.onmicrosoft.com/B2C_1{PolicyName}")
        .WithRedirectUri(RedirectURI)
        .Build();
      else
        PublicClientApplication = Microsoft.Identity.Client.PublicClientApplicationBuilder
        .Create(ContextIdentifier.ToString())
        .WithB2CAuthority($"https://softmakeb2c.b2clogin.com/tfp/softmakeb2c.onmicrosoft.com/B2C_1{PolicyName}")
        .WithRedirectUri(RedirectURI)
        .Build();

      SoftmakeAll.SDK.Fluent.TokenCacheHelper.EnableSerialization(PublicClientApplication.UserTokenCache);
      return PublicClientApplication;
    }

    /// <summary>
    /// Authenticate user/application using stored cache Credentials.
    /// </summary>
    public static void Authenticate() => SoftmakeAll.SDK.Fluent.SDKContext.Authenticate(true);
    private static void Authenticate(System.Boolean ConfigureWebSocket) => SoftmakeAll.SDK.Fluent.SDKContext.Authenticate(null, ConfigureWebSocket);

    /// <summary>
    /// Authenticate user/application using Credentials.
    /// </summary>
    /// <param name="Credentials">Credentials to use during authentication process.</param>
    public static void Authenticate(SoftmakeAll.SDK.Fluent.Authentication.ICredentials Credentials) => SoftmakeAll.SDK.Fluent.SDKContext.AuthenticateAsync(Credentials).Wait();
    private static void Authenticate(SoftmakeAll.SDK.Fluent.Authentication.ICredentials Credentials, System.Boolean ConfigureWebSocket) => SoftmakeAll.SDK.Fluent.SDKContext.AuthenticateAsync(Credentials, ConfigureWebSocket).Wait();

    /// <summary>
    /// Authenticate user/application using stored cache Credentials.
    /// </summary>
    public static async System.Threading.Tasks.Task AuthenticateAsync() => await SoftmakeAll.SDK.Fluent.SDKContext.AuthenticateAsync(true);
    private static async System.Threading.Tasks.Task AuthenticateAsync(System.Boolean ConfigureWebSocket) => await SoftmakeAll.SDK.Fluent.SDKContext.AuthenticateAsync(null, ConfigureWebSocket);

    /// <summary>
    /// Authenticate user/application using Credentials.
    /// </summary>
    /// <param name="Credentials">Credentials to use during authentication process.</param>
    public static async System.Threading.Tasks.Task AuthenticateAsync(SoftmakeAll.SDK.Fluent.Authentication.ICredentials Credentials) => await SoftmakeAll.SDK.Fluent.SDKContext.AuthenticateAsync(Credentials, true);
    private static async System.Threading.Tasks.Task AuthenticateAsync(SoftmakeAll.SDK.Fluent.Authentication.ICredentials Credentials, System.Boolean ConfigureWebSocket)
    {
      if (Credentials != null)
      {
        SoftmakeAll.SDK.Fluent.SDKContext.SignOut();
        SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials = Credentials;

        // From AccessKey
        if (Credentials.AuthenticationType == SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes.Application)
        {
          SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.Authorization = $"Basic {System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{Credentials.ClientID}@{Credentials.ContextIdentifier.ToString().ToLower()}:{Credentials.ClientSecret}"))}";
          SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.Store();

          if (!((await SoftmakeAll.SDK.Fluent.SDKContext.GetEnvironmentsAsync()).IsValid()))
          {
            SoftmakeAll.SDK.Fluent.SDKContext.SignOut();
            return;
          }

          if (ConfigureWebSocket)
            await SoftmakeAll.SDK.Fluent.SDKContext.ClientWebSocket.ConfigureAsync(SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.Authorization);
          return;
        }
      }
      else if (SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials == null)
      {
        try
        {
          System.Text.Json.JsonElement CacheData = SoftmakeAll.SDK.Fluent.GeneralCacheHelper.ReadString().ToJsonElement();
          if (!(CacheData.IsValid()))
            throw new System.Exception();

          // From AccessKey
          if (CacheData.GetInt32("AuthenticationType") == (int)SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes.Application)
          {
            SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials = new SoftmakeAll.SDK.Fluent.Authentication.Credentials(CacheData.GetGuid("ContextIdentifier"), CacheData.GetString("ClientID"), null, (SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes)CacheData.GetInt32("AuthenticationType"));
            SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.Authorization = CacheData.GetString("Authorization");
            if (System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.Authorization))
              throw new System.Exception();

            if (ConfigureWebSocket)
              await SoftmakeAll.SDK.Fluent.SDKContext.ClientWebSocket.ConfigureAsync(SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.Authorization);
            return;
          }
          else
          {
            SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials = new SoftmakeAll.SDK.Fluent.Authentication.Credentials(CacheData.GetJsonElement("AppMetadata").EnumerateObject().First().Value.GetGuid("client_id"));
            SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.AuthenticationType = SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes.Interactive;
          }
        }
        catch { }
      }

      if (SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials == null)
      {
        SoftmakeAll.SDK.Fluent.SDKContext.SignOut();
        throw new System.Exception("Invalid Credentials from cache.");
      }


      // From AccessKey
      if (SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.AuthenticationType == SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes.Application)
        return;


      // From Public Client Application
      if ((SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult == null) || (SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult.ExpiresOn.Subtract(System.DateTimeOffset.UtcNow).TotalMinutes <= SoftmakeAll.SDK.Fluent.SDKContext.ExpirationTime))
      {
        if (SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication == null)
        {
          if (SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.AuthenticationType == SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes.Interactive) // From Interactive
            SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication = SoftmakeAll.SDK.Fluent.SDKContext.CreatePublicClientApplication(SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.ContextIdentifier, "A_SIGNUP_SIGNIN", "http://localhost:1435");
          else if (SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.AuthenticationType == SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes.Credentials) // From Username and Password
            SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication = SoftmakeAll.SDK.Fluent.SDKContext.CreatePublicClientApplication(SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.ContextIdentifier, "_ROPC");
          else
            throw new System.Exception("Invalid authentication type.");
        }

        // Getting existing Account in cache
        try
        {
          System.Collections.Generic.IEnumerable<Microsoft.Identity.Client.IAccount> Accounts = await SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication.GetAccountsAsync();
          if (Accounts.Any())
          {
            SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult = await SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication.AcquireTokenSilent(SoftmakeAll.SDK.Fluent.SDKContext.Scopes, Accounts.FirstOrDefault()).ExecuteAsync();
            if (SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult != null)
            {
              SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.Authorization = $"Bearer {SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult.AccessToken}";
              if (ConfigureWebSocket)
                await SoftmakeAll.SDK.Fluent.SDKContext.ClientWebSocket.ConfigureAsync(SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult.AccessToken);
              return;
            }
          }
        }
        catch
        {
          SoftmakeAll.SDK.Fluent.GeneralCacheHelper.Clear();
        }


        if (SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.AuthenticationType == SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes.Interactive) // From Interactive
        {
          try
          {
            SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult = await SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication.AcquireTokenInteractive(Scopes).WithPrompt(Microsoft.Identity.Client.Prompt.ForceLogin).ExecuteAsync();
          }
          catch { }
        }
        else if (SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.AuthenticationType == SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes.Credentials) // From Username and Password
        {
          if (System.String.IsNullOrWhiteSpace(SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.ClientSecret))
          {
            SoftmakeAll.SDK.Fluent.SDKContext.SignOut();
            throw new System.Exception("Authentication aborted. Please, re-enter credentials.");
          }

          System.Security.SecureString Password = new System.Security.SecureString();
          foreach (System.Char Char in SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.ClientSecret)
            Password.AppendChar(Char);
          Password.MakeReadOnly();

          try
          {
            SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult = await SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication.AcquireTokenByUsernamePassword(Scopes, SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.ClientID, Password).ExecuteAsync();
            Password.Dispose();
          }
          catch (System.Exception ex)
          {
            System.Console.Write(ex.Message);
            Password.Dispose();
            SoftmakeAll.SDK.Fluent.SDKContext.SignOut();
            throw new System.Exception("Invalid username or password.");
          }
        }

        if (SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult == null)
        {
          SoftmakeAll.SDK.Fluent.SDKContext.SignOut();
          throw new System.Exception("Authentication aborted.");
        }


        SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.Authorization = $"Bearer {SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult.AccessToken}";

        if (ConfigureWebSocket)
          await SoftmakeAll.SDK.Fluent.SDKContext.ClientWebSocket.ConfigureAsync(SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult.AccessToken);
        return;
      }
    }

    /// <summary>
    /// Perform the HTTP Request based on REST object information.
    /// </summary>
    /// <param name="REST">REST object that contains the HTTP Request information.</param>
    /// <param name="SkipAuthorizationHeader">Removes the Authentication Header before sending. The default value is false.</param>
    /// <returns>A OperationResult with JSON property Data.</returns>
    public static SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> PerformRESTRequest(SoftmakeAll.SDK.Communication.REST REST, System.Boolean SkipAuthorizationHeader = false)
    {
      if (REST == null)
        return null;

      System.Boolean RemoveAuthorization = false;
      if ((!(SkipAuthorizationHeader)) && (REST.Headers != null) && (!(REST.Headers.ContainsKey("Authorization"))))
        try
        {
          SoftmakeAll.SDK.Fluent.SDKContext.Authenticate();
          REST.Headers.Add("Authorization", SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.Authorization);
          RemoveAuthorization = true;
        }
        catch { }
      else if ((SkipAuthorizationHeader) && (REST.Headers != null) && (REST.Headers.ContainsKey("Authorization")))
        REST.Headers.Remove("Authorization");

      REST.URL = $"{SoftmakeAll.SDK.Fluent.SDKContext.APIBaseAddress}/API/{REST.URL}";
      System.Text.Json.JsonElement RESTResult = REST.Send();

      if (RemoveAuthorization)
        REST.Headers.Remove("Authorization");

      return SoftmakeAll.SDK.Fluent.SDKContext.ProcessRESTRequestResult(REST, RESTResult);
    }

    /// <summary>
    /// Perform the HTTP Request based on REST object information.
    /// </summary>
    /// <param name="REST">REST object that contains the HTTP Request information.</param>
    /// <param name="SkipAuthorizationHeader">Removes the Authentication Header before sending. The default value is false.</param>
    /// <returns>A OperationResult with JSON property Data.</returns>
    public static async System.Threading.Tasks.Task<SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>> PerformRESTRequestAsync(SoftmakeAll.SDK.Communication.REST REST, System.Boolean SkipAuthorizationHeader = false)
    {
      if (REST == null)
        return null;

      System.Boolean RemoveAuthorization = false;
      if ((!(SkipAuthorizationHeader)) && (REST.Headers != null) && (!(REST.Headers.ContainsKey("Authorization"))))
        try
        {
          await SoftmakeAll.SDK.Fluent.SDKContext.AuthenticateAsync();
          REST.Headers.Add("Authorization", SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.Authorization);
          RemoveAuthorization = true;
        }
        catch { }
      else if ((SkipAuthorizationHeader) && (REST.Headers != null) && (REST.Headers.ContainsKey("Authorization")))
        REST.Headers.Remove("Authorization");

      REST.URL = $"{SoftmakeAll.SDK.Fluent.SDKContext.APIBaseAddress}/API/{REST.URL}";
      System.Text.Json.JsonElement RESTResult = await REST.SendAsync();

      if (RemoveAuthorization)
        REST.Headers.Remove("Authorization");

      return SoftmakeAll.SDK.Fluent.SDKContext.ProcessRESTRequestResult(REST, RESTResult);
    }

    /// <summary>
    /// Creates a OperationResult object based on HTTP Request.
    /// </summary>
    /// <param name="REST">REST object that contains the HTTP Request information.</param>
    /// <param name="RESTResult">The output of Send method.</param>
    /// <returns>A OperationResult with JSON property Data.</returns>
    private static SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> ProcessRESTRequestResult(SoftmakeAll.SDK.Communication.REST REST, System.Text.Json.JsonElement RESTResult)
    {
      SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement> Result = new SoftmakeAll.SDK.OperationResult<System.Text.Json.JsonElement>();
      if (REST.HasRequestErrors)
        Result.ExitCode = (int)REST.StatusCode;
      else
        Result.ExitCode = RESTResult.GetInt32("ExitCode");

      if (RESTResult.IsValid())
      {
        Result.Message = RESTResult.GetString("Message");
        Result.ID = RESTResult.GetString("ID");
        Result.Count = RESTResult.GetInt32("Count");
        Result.Data = RESTResult.GetJsonElement("Data").ToObject<System.Text.Json.JsonElement>();
      }

      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.ExitCode = Result.ExitCode;
      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.Message = Result.Message;
      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.Count = Result.Count;
      SoftmakeAll.SDK.Fluent.SDKContext.LastOperationResult.ID = Result.ID;

      return Result;
    }

    /// <summary>
    /// Gets the current authorization header.
    /// </summary>
    /// <returns>The current authorization token.</returns>
    public static async System.Threading.Tasks.Task<System.Text.Json.JsonElement> GetAuthorizationAsync()
    {
      try
      {
        await SoftmakeAll.SDK.Fluent.SDKContext.AuthenticateAsync(false);
        if (SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials != null)
          return new { SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials.Authorization }.ToJsonElement();
      }
      catch { }
      return new System.Text.Json.JsonElement();
    }

    /// <summary>
    /// Gets the all environments of current user.
    /// </summary>
    /// <returns>The json array containing the Environments.</returns>
    public static System.Text.Json.JsonElement GetEnvironments()
    {
      SoftmakeAll.SDK.Communication.REST REST = new SoftmakeAll.SDK.Communication.REST();
      REST.URL = "v1/core/EnvironmentsOfLoggedEnvironmentSystemAccount?fields=EnvironmentUniqueID,EnvironmentName,IsDefaultEnvironment,EnvironmentSystemAccountsID,SystemAccountFullName,SystemAccountEmail,SystemAccountsID";
      return SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequest(REST).Data.GetJsonElement("Result");
    }

    /// <summary>
    /// Gets the all environments of current user.
    /// </summary>
    /// <returns>The json array containing the Environments.</returns>
    public static async System.Threading.Tasks.Task<System.Text.Json.JsonElement> GetEnvironmentsAsync()
    {
      SoftmakeAll.SDK.Communication.REST REST = new SoftmakeAll.SDK.Communication.REST();
      REST.URL = "v1/core/EnvironmentsOfLoggedEnvironmentSystemAccount?fields=EnvironmentUniqueID,EnvironmentName,IsDefaultEnvironment,EnvironmentSystemAccountsID,SystemAccountFullName,SystemAccountEmail,SystemAccountsID";
      return (await SoftmakeAll.SDK.Fluent.SDKContext.PerformRESTRequestAsync(REST)).Data.GetJsonElement("Result");
    }

    /// <summary>
    /// Clears the authentication objects and cache data.
    /// </summary>
    public static void SignOut()
    {
      SoftmakeAll.SDK.Fluent.GeneralCacheHelper.Clear();
      SoftmakeAll.SDK.Fluent.SDKContext.ClientWebSocket.DisposeAsync().ConfigureAwait(false);
      SoftmakeAll.SDK.Fluent.SDKContext.InMemoryCredentials = null;
      SoftmakeAll.SDK.Fluent.SDKContext.AuthenticationResult = null;
      SoftmakeAll.SDK.Fluent.SDKContext.PublicClientApplication = null;
    }
    #endregion
  }
}