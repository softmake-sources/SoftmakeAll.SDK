namespace SoftmakeAll.SDK.Fluent
{
  public abstract class BaseResource
  {
    #region Constructor
    public BaseResource() { }
    #endregion

    #region Properties
    public System.Guid UniqueID { get; set; }
    public System.String RowVersion { get; set; }
    public System.DateTimeOffset Inserted { get; set; }
    public System.Nullable<System.DateTimeOffset> Updated { get; set; }
    public System.Text.Json.JsonElement JSONData { get; set; }
    #endregion

    #region Methods
    public virtual System.Boolean Validate() => true;
    #endregion
  }
}