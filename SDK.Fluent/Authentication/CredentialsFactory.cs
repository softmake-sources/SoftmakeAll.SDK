namespace SoftmakeAll.SDK.Fluent.Authentication
{
  /// <summary>
  /// Provide methods to create Credentials instances.
  /// </summary>
  public static class CredentialsFactory
  {
    #region Methods
    /// <summary>
    /// Creates a new instance of Credentials using application data.
    /// </summary>
    /// <param name="EnvironmentUniqueID">The unique identifier from existing Softmake All Environment</param>
    /// <param name="ClientID">Username</param>
    /// <param name="ClientSecrect">Password</param>
    /// <returns>A new instance of Credentials object.</returns>
    public static SoftmakeAll.SDK.Fluent.Authentication.ICredentials FromAccessKey(System.Guid EnvironmentUniqueID, System.String ClientID, System.String ClientSecrect) =>
      new SoftmakeAll.SDK.Fluent.Authentication.Credentials(EnvironmentUniqueID, ClientID, ClientSecrect, SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes.Application);

    /// <summary>
    /// Creates a new instance of Credentials using PublicClientApplication data.
    /// </summary>
    /// <param name="PublicClientApplicationID">The Azure ADB2C PublicClientApplicationID.</param>
    /// <param name="ClientID">Username</param>
    /// <param name="ClientSecrect">Password</param>
    /// <returns>A new instance of Credentials object.</returns>
    public static SoftmakeAll.SDK.Fluent.Authentication.ICredentials FromCredentials(System.Guid PublicClientApplicationID, System.String ClientID, System.String ClientSecrect) =>
      new SoftmakeAll.SDK.Fluent.Authentication.Credentials(PublicClientApplicationID, ClientID, ClientSecrect, SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes.Credentials);

    /// <summary>
    /// Creates a new instance of Credentials using PublicClientApplication data. The authentication process will be done through the browser.
    /// </summary>
    /// <param name="PublicClientApplicationID">The Azure ADB2C PublicClientApplicationID.</param>
    /// <returns>A new instance of Credentials object.</returns>
    public static SoftmakeAll.SDK.Fluent.Authentication.ICredentials FromInteractive(System.Guid PublicClientApplicationID) => new SoftmakeAll.SDK.Fluent.Authentication.Credentials(PublicClientApplicationID);
    #endregion
  }
}