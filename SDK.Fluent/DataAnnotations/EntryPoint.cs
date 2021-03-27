namespace SoftmakeAll.SDK.Fluent.DataAnnotations
{
  [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Property)]
  public class EntryPoint : System.Attribute
  {
    #region Fields
    public readonly System.Byte Version;
    public readonly System.String Name;
    #endregion

    #region Constructor
    public EntryPoint(System.String Name) : this(Name, 1) { }
    public EntryPoint(System.String Name, System.Byte Version)
    {
      this.Name = Name;
      this.Version = Version;
    }
    #endregion

    #region Methods
    public static SoftmakeAll.SDK.Fluent.DataAnnotations.EntryPoint FromType(System.Type Type)
    {
      foreach (System.Attribute ClassAttribute in System.Attribute.GetCustomAttributes(Type))
      {
        SoftmakeAll.SDK.Fluent.DataAnnotations.EntryPoint EntryPoint = ClassAttribute as SoftmakeAll.SDK.Fluent.DataAnnotations.EntryPoint;
        if (EntryPoint != null)
          return EntryPoint;
      }
      return null;
    }
    #endregion
  }
}