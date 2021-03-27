namespace SoftmakeAll.SDK.Fluent.Authentication
{
  public static class CredentialsFactory
  {
    #region Methods
    public static SoftmakeAll.SDK.Fluent.Authentication.ICredentials FromAccessKey(System.Guid EnvironmentUniqueID, System.String ClientID, System.String ClientSecrect) => new SoftmakeAll.SDK.Fluent.Authentication.Credentials(EnvironmentUniqueID, ClientID, ClientSecrect, 'A');
    public static SoftmakeAll.SDK.Fluent.Authentication.ICredentials FromCredentials(System.Guid PublicClientApplicationID, System.String ClientID, System.String ClientSecrect) => new SoftmakeAll.SDK.Fluent.Authentication.Credentials(PublicClientApplicationID, ClientID, ClientSecrect, 'C');
    public static SoftmakeAll.SDK.Fluent.Authentication.ICredentials FromInteractive(System.Guid PublicClientApplicationID) => new SoftmakeAll.SDK.Fluent.Authentication.Credentials(PublicClientApplicationID);
    #endregion
  }
}