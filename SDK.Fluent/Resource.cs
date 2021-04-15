namespace SoftmakeAll.SDK.Fluent
{
  public abstract class Resource
  {
    #region Constructor
    public Resource() { }
    #endregion

    #region Properties
    [System.Text.Json.Serialization.JsonInclude]
    public System.Guid UniqueID { get; private set; }

    [System.Text.Json.Serialization.JsonInclude]
    public System.String RowVersion { get; private set; }

    [System.Text.Json.Serialization.JsonInclude]
    public System.DateTimeOffset Inserted { get; private set; }

    [System.Text.Json.Serialization.JsonInclude]
    public System.Nullable<System.DateTimeOffset> Updated { get; private set; }

    public System.Text.Json.JsonElement JSONData { get; set; }
    #endregion

    #region Methods
    public virtual System.Boolean Validate() => true;
    #endregion
  }
}