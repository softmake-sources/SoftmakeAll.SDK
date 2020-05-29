namespace SoftmakeAll.SDK
{
  public class OperationResult
  {
    #region Constructor
    public OperationResult() { this.ClearObjects(); }
    #endregion

    #region Properties
    public System.Nullable<System.Int32> Count { get; set; }
    public System.String ID { get; set; }
    public System.String Message { get; set; }
    public System.Int32 ExitCode { get; set; }

    [System.Text.Json.Serialization.JsonIgnore]
    public System.Boolean Success => ((this.ExitCode == 0) || (this.ExitCode == 200) || (this.ExitCode == 204));
    #endregion

    #region Methods
    public virtual void ClearObjects() { this.Count = null; this.ID = null; this.Message = null; this.ExitCode = 0; }
    #endregion
  }

  public class OperationResult<T> : SoftmakeAll.SDK.OperationResult
  {
    #region Constructor
    public OperationResult() : base() { }
    #endregion

    #region Properties
    public T Data { get; set; }
    #endregion

    #region Methods
    public override void ClearObjects() { base.ClearObjects(); this.Data = default; }
    #endregion
  }
}