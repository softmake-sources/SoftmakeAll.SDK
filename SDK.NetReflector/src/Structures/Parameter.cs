namespace SoftmakeAll.SDK.NETReflector.Structures
{
  public struct Parameter
  {
    #region Properties
    public System.Int32 Position { get; set; }
    public System.String TypeDescription { get; set; }
    public System.String Name { get; set; }
    public System.Boolean IsOutput { get; set; }
    public System.Boolean IsReference { get; set; }
    public System.Boolean IsParamArray { get; set; }
    public System.Boolean IsOptional { get; set; }
    #endregion
  }
}