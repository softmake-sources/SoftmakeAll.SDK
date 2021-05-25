namespace SoftmakeAll.SDK.Fluent.Authentication
{
  /// <summary>
  /// Credentials to use in authentication process.
  /// </summary>
  public interface ICredentials
  {
    #region Properties
    /// <summary>
    /// The PublicClientApplicationID or EnvironmentUniqiueID.
    /// </summary>
    internal System.Guid ContextIdentifier { get; set; }

    /// <summary>
    /// Username.
    /// </summary>
    internal System.String ClientID { get; set; }

    /// <summary>
    /// Password.
    /// </summary>
    internal System.String ClientSecret { get; set; }

    /// <summary>
    /// Authentication Type.
    /// </summary>
    internal SoftmakeAll.SDK.Fluent.Authentication.AuthenticationTypes AuthenticationType { get; set; }

    /// <summary>
    /// Authorization Header.
    /// </summary>
    internal System.String Authorization { get; set; }
    #endregion

    #region Methods
    /// <summary>
    /// Writes the credentials in local disk.
    /// </summary>
    internal void Store();

    /// <summary>
    /// Clears local disk credentials data.
    /// </summary>
    internal void Delete();
    #endregion
  }
}