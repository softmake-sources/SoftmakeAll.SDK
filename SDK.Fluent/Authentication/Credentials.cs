namespace SoftmakeAll.SDK.Fluent.Authentication
{
  internal class Credentials : SoftmakeAll.SDK.Fluent.Authentication.ICredentials
  {
    #region Fields
    private readonly SoftmakeAll.SDK.Fluent.Authentication.ICredentials CredentialsContext;
    private readonly System.Guid _ObjectID = System.Guid.NewGuid();
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
    System.Guid SoftmakeAll.SDK.Fluent.Authentication.ICredentials.ObjectID => this._ObjectID;
    System.Guid SoftmakeAll.SDK.Fluent.Authentication.ICredentials.ContextIdentifier { get; set; }
    System.String SoftmakeAll.SDK.Fluent.Authentication.ICredentials.ClientID { get; set; }
    System.String SoftmakeAll.SDK.Fluent.Authentication.ICredentials.ClientSecret { get; set; }
    System.Char SoftmakeAll.SDK.Fluent.Authentication.ICredentials.AuthType { get; set; }
    System.String SoftmakeAll.SDK.Fluent.Authentication.ICredentials.Authorization { get; set; }
    #endregion
  }
}