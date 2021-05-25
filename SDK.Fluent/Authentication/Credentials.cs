using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Fluent.Authentication
{
  /// <summary>
  /// Credentials to use in authentication process.
  /// </summary>
  internal class Credentials : SoftmakeAll.SDK.Fluent.Authentication.ICredentials
  {
    #region Fields
    private readonly SoftmakeAll.SDK.Fluent.Authentication.ICredentials CredentialsContext;
    #endregion

    #region Constructor
    /// <summary>
    /// Creates a new instance of credentials to use in authentication process.
    /// </summary>
    /// <param name="ContextIdentifier">The PublicClientApplicationID or EnvironmentUniqiueID.</param>
    internal Credentials(System.Guid ContextIdentifier)
    {
      this.CredentialsContext = this;
      this.CredentialsContext.ContextIdentifier = ContextIdentifier;
      this.CredentialsContext.AuthenticationType = SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes.Interactive;
    }

    /// <summary>
    /// Creates a new instance of credentials to use in authentication process.
    /// </summary>
    /// <param name="ContextIdentifier">The PublicClientApplicationID or EnvironmentUniqiueID.</param>
    /// <param name="ClientID">Username.</param>
    /// <param name="ClientSecret">Password.</param>
    /// <param name="AuthenticationType">Authentication Type.</param>
    internal Credentials(System.Guid ContextIdentifier, System.String ClientID, System.String ClientSecret, SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes AuthenticationType)
    {
      this.CredentialsContext = this;
      this.CredentialsContext.ContextIdentifier = ContextIdentifier;
      this.CredentialsContext.ClientID = ClientID;
      this.CredentialsContext.ClientSecret = ClientSecret;
      this.CredentialsContext.AuthenticationType = AuthenticationType;
    }
    #endregion

    #region Properties
    /// <summary>
    /// The PublicClientApplicationID or EnvironmentUniqiueID.
    /// </summary>
    System.Guid SoftmakeAll.SDK.Fluent.Authentication.ICredentials.ContextIdentifier { get; set; }

    /// <summary>
    /// Username.
    /// </summary>
    System.String SoftmakeAll.SDK.Fluent.Authentication.ICredentials.ClientID { get; set; }

    /// <summary>
    /// Password.
    /// </summary>
    System.String SoftmakeAll.SDK.Fluent.Authentication.ICredentials.ClientSecret { get; set; }

    /// <summary>
    /// Authentication Type.
    /// </summary>
    SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes SoftmakeAll.SDK.Fluent.Authentication.ICredentials.AuthenticationType { get; set; }

    /// <summary>
    /// Authorization Header.
    /// </summary>
    System.String SoftmakeAll.SDK.Fluent.Authentication.ICredentials.Authorization { get; set; }
    #endregion

    #region Methods
    /// <summary>
    /// Writes the credentials in local disk.
    /// </summary>
    void SoftmakeAll.SDK.Fluent.Authentication.ICredentials.Store() => SoftmakeAll.SDK.Fluent.GeneralCacheHelper.WriteString(new { AuthenticationType = (int)this.CredentialsContext.AuthenticationType, this.CredentialsContext.Authorization, this.CredentialsContext.ClientID, this.CredentialsContext.ContextIdentifier }.ToJsonElement().ToRawText());

    /// <summary>
    /// Clears local disk credentials data.
    /// </summary>
    void SoftmakeAll.SDK.Fluent.Authentication.ICredentials.Delete() => SoftmakeAll.SDK.Fluent.GeneralCacheHelper.Clear();
    #endregion
  }
}