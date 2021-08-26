namespace SoftmakeAll.SDK.CloudStorage.AWS
{
  public static class Environment
  {
    #region Fields
    internal static Amazon.S3.IAmazonS3 _S3Client;
    #endregion

    #region Methods
    public static void Configure(System.String AccessKeyID, System.String SecretAccessKey, System.String RegionEndpointName)
    {
      Amazon.RegionEndpoint RegionEndpoint = null;
      try { RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(RegionEndpointName); } catch { }
      SoftmakeAll.SDK.CloudStorage.AWS.Environment.Configure(AccessKeyID, SecretAccessKey, RegionEndpoint);
    }
    public static void Configure(System.String AccessKeyID, System.String SecretAccessKey, Amazon.RegionEndpoint RegionEndpoint)
    {
      if (System.String.IsNullOrWhiteSpace(AccessKeyID))
        throw new System.Exception("The AccessKeyID cannot be null or empty.");

      if (System.String.IsNullOrWhiteSpace(SecretAccessKey))
        throw new System.Exception("The SecretAccessKey cannot be null or empty.");

      if (RegionEndpoint == null)
        throw new System.Exception("The RegionEndpoint cannot be null.");

      SoftmakeAll.SDK.CloudStorage.AWS.Environment._S3Client = new Amazon.S3.AmazonS3Client(AccessKeyID, SecretAccessKey, RegionEndpoint);
    }
    internal static void Validate(Amazon.S3.IAmazonS3 S3Client)
    {
      if ((S3Client == null) && (SoftmakeAll.SDK.CloudStorage.AWS.Environment._S3Client == null))
        throw new System.Exception("Call SoftmakeAll.SDK.CloudStorage.AWS.Environment.Configure(...) to configure the SDK.");
    }
    #endregion
  }
}