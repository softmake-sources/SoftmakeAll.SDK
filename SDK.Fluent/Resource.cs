namespace SoftmakeAll.SDK.Fluent
{
  /// <summary>
  /// The base class of the Softmake Resource.
  /// </summary>
  public abstract class Resource
  {
    #region Constructor
    /// <summary>
    /// The base class of the Softmake Resource.
    /// </summary>
    public Resource() { }
    #endregion

    #region Properties
    /// <summary>
    /// The unique identifier for the Resource.
    /// </summary>
    [System.Text.Json.Serialization.JsonInclude]
    public System.Guid UniqueID { get; private set; }

    /// <summary>
    /// The current version for the database row.
    /// </summary>
    [System.Text.Json.Serialization.JsonInclude]
    public System.String RowVersion { get; private set; }

    /// <summary>
    /// The date the record was created.
    /// </summary>
    [System.Text.Json.Serialization.JsonInclude]
    public System.DateTimeOffset Inserted { get; private set; }

    /// <summary>
    /// The date the record was updated.
    /// </summary>
    [System.Text.Json.Serialization.JsonInclude]
    public System.DateTimeOffset? Updated { get; private set; }

    /// <summary>
    /// Field for storing unstructured data.
    /// </summary>
    public System.Text.Json.JsonElement JSONData { get; set; }
    #endregion

    #region Methods
    /// <summary>
    /// Validates all properties of the current object.
    /// </summary>
    /// <returns></returns>
    public virtual System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult> Validate()
    {
      System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult> ValidationResult = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
      System.ComponentModel.DataAnnotations.ValidationContext ValidationContext = new System.ComponentModel.DataAnnotations.ValidationContext(this);
      System.ComponentModel.DataAnnotations.Validator.TryValidateObject(this, ValidationContext, ValidationResult, true);
      return ValidationResult;
    }
    #endregion
  }
}