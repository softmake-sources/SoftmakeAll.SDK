namespace SoftmakeAll.SDK.FileWR
{
  public class FileRegisterColumn
  {
    #region Constructor
    public FileRegisterColumn() { }
    #endregion

    #region Properties
    public System.String Name { get; set; }
    public System.String Description { get; set; }
    public System.Char DataType { get; set; }
    public System.Int32 ContentLength { get; set; }
    public System.Boolean IsFileRegisterType { get; set; }
    public System.Boolean IgnoreValues { get; set; }

    internal System.Int32 _StartPosition = -1;
    public System.Int32 StartPosition => this._StartPosition;
    #endregion
  }
}