using SoftmakeAll.SDK.Helpers.JSON.Extensions;

namespace SoftmakeAll.SDK.Fluent.Authentication
{
  internal class Credentials : SoftmakeAll.SDK.Fluent.Authentication.ICredentials
  {
    #region Fields
    private readonly SoftmakeAll.SDK.Fluent.Authentication.ICredentials CredentialsContext;
    #endregion

    #region Constructor
    internal Credentials(System.Guid ContextIdentifier)
    {
      this.CredentialsContext = this;
      this.CredentialsContext.ContextIdentifier = ContextIdentifier;
      this.CredentialsContext.AuthType = 'I';
    }
    internal Credentials(System.Guid ContextIdentifier, System.String ClientID, System.String ClientSecret, System.Char AuthType)
    {
      this.CredentialsContext = this;
      this.CredentialsContext.ContextIdentifier = ContextIdentifier;
      this.CredentialsContext.ClientID = ClientID;
      this.CredentialsContext.ClientSecret = ClientSecret;
      this.CredentialsContext.AuthType = AuthType;
    }
    #endregion

    #region Properties
    System.Guid SoftmakeAll.SDK.Fluent.Authentication.ICredentials.ContextIdentifier { get; set; }
    System.String SoftmakeAll.SDK.Fluent.Authentication.ICredentials.ClientID { get; set; }
    System.String SoftmakeAll.SDK.Fluent.Authentication.ICredentials.ClientSecret { get; set; }
    System.Char SoftmakeAll.SDK.Fluent.Authentication.ICredentials.AuthType { get; set; }
    System.String SoftmakeAll.SDK.Fluent.Authentication.ICredentials.Authorization { get; set; }
    #endregion

    #region Methods
    void SoftmakeAll.SDK.Fluent.Authentication.ICredentials.Store() => SoftmakeAll.SDK.Fluent.GeneralCacheHelper.WriteString(new { this.CredentialsContext.AuthType, this.CredentialsContext.Authorization, this.CredentialsContext.ClientID, this.CredentialsContext.ContextIdentifier }.ToJsonElement().ToRawText());
    void SoftmakeAll.SDK.Fluent.Authentication.ICredentials.Delete() => SoftmakeAll.SDK.Fluent.GeneralCacheHelper.Clear();
    #endregion
  }
}