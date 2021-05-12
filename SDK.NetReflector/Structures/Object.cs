namespace SoftmakeAll.SDK.NetReflector.Structures
{
  public struct Object
  {
    #region Properties
    public System.String Name { get; set; }
    public System.Boolean IsStatic { get; set; }
    public System.Boolean IsClass { get; set; }
    public System.Boolean IsStructure { get; set; }
    public System.Byte InheritanceLevel { get; set; }
    public System.String BaseObjectName { get; set; }
    public System.Collections.Generic.List<System.String> CustomAttributes { get; set; }
    public System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Property> Properties { get; set; }
    public System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Method> Methods { get; set; }
    #endregion
  }
}