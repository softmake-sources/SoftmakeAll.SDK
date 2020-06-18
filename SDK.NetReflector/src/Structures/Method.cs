namespace SoftmakeAll.SDK.NETReflector.Structures
{
  public struct Method
  {
    #region Properties
    public System.Boolean IsConstructor { get; set; }
    public System.Boolean IsStatic { get; set; }
    public System.Boolean IsAsync { get; set; }
    public System.String TypeDescription { get; set; }
    public System.String Name { get; set; }
    public System.Collections.Generic.List<SoftmakeAll.SDK.NETReflector.Structures.Parameter> Parameters { get; set; }
    #endregion
  }
}