namespace SoftmakeAll.SDK.NetReflector.Structures
{
  public struct Property
  {
    #region Properties
    public System.String Name { get; set; }
    public System.Boolean IsStatic { get; set; }
    public System.Boolean CanRead { get; set; }
    public System.Boolean CanWrite { get; set; }
    public System.Byte InheritanceLevel { get; set; }
    public System.String TypeDescription { get; set; }
    public System.Collections.Generic.List<System.String> CustomAttributes { get; set; }
    #endregion
  }
}