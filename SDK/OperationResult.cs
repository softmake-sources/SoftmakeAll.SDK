namespace SoftmakeAll.SDK
{
  public class OperationResult
  {
    #region Constructor
    public OperationResult() { this.ClearObjects(); }
    public OperationResult(System.Boolean ConfigureForSuccess) { this.ClearObjects(ConfigureForSuccess); }
    #endregion

    #region Properties
    public System.Nullable<System.Int32> Count { get; set; }
    public System.Nullable<System.Int64> ID { get; set; }
    public System.String Message { get; set; }
    public System.Int16 ExitCode { get; set; }
    #endregion

    #region Methods
    public virtual void ClearObjects(System.Boolean ConfigureForSuccess) { this.ID = null; this.Message = null; this.ExitCode = ConfigureForSuccess ? (short)0 : (short)-1; }
    public virtual void ClearObjects() { this.ClearObjects(false); }
    #endregion
  }

  public class OperationResult<T> : SoftmakeAll.SDK.OperationResult
  {
    #region Constructor
    public OperationResult() : base() { }
    public OperationResult(System.Boolean ConfigureForSuccess) : base(ConfigureForSuccess) { }
    #endregion

    #region Properties
    public T Data { get; set; }
    #endregion

    #region Methods
    public override void ClearObjects(System.Boolean ConfigureForSuccess) { base.ClearObjects(ConfigureForSuccess); this.Data = default; }
    public override void ClearObjects() { this.ClearObjects(false); }
    #endregion
  }
}