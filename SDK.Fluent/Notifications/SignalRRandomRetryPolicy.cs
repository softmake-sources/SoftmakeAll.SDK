namespace SoftmakeAll.SDK.Fluent.Notifications
{
  internal class SignalRRandomRetryPolicy : Microsoft.AspNetCore.SignalR.Client.IRetryPolicy
  {
    #region Fields
    private readonly System.Random Random;
    private readonly System.Int32 StopAfterSeconds;
    #endregion

    #region Constructor
    public SignalRRandomRetryPolicy() : this(System.Int32.MaxValue) { }
    public SignalRRandomRetryPolicy(System.Int32 StopAfterSeconds)
    {
      this.Random = new System.Random();
      this.StopAfterSeconds = StopAfterSeconds;
    }
    #endregion

    #region Methods
    public System.TimeSpan? NextRetryDelay(Microsoft.AspNetCore.SignalR.Client.RetryContext RetryContext) => RetryContext.ElapsedTime < System.TimeSpan.FromSeconds(this.StopAfterSeconds) ? System.TimeSpan.FromSeconds(this.Random.NextDouble() * 10) : null;
    #endregion
  }
}