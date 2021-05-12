namespace SoftmakeAll.SDK.NetReflector.Structures
{
  public struct Method
  {
    #region Properties
    public System.String Name { get; set; }
    public System.Boolean IsConstructor { get; set; }
    public System.Boolean IsStatic { get; set; }
    public System.Boolean IsAsync { get; set; }
    public System.Byte InheritanceLevel { get; set; }
    public System.String TypeDescription { get; set; }
    public System.Collections.Generic.List<System.String> CustomAttributes { get; set; }
    public System.Collections.Generic.List<SoftmakeAll.SDK.NetReflector.Structures.Parameter> Parameters { get; set; }
    #endregion
  }
}