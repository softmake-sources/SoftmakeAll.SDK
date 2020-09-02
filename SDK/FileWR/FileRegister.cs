using System.Linq;

namespace SoftmakeAll.SDK.FileWR
{
  public class FileRegister
  {
    #region Constructor
    public FileRegister()
    {
      this.FileRegisterColumns = new System.Collections.Generic.List<SoftmakeAll.SDK.FileWR.FileRegisterColumn>();
    }
    #endregion

    #region Properties
    public System.String Name { get; set; }
    public System.Byte Type { get; set; }
    public System.Collections.Generic.List<SoftmakeAll.SDK.FileWR.FileRegisterColumn> FileRegisterColumns { get; }

    internal System.Int32 _Length;
    internal System.Int32 Length => this._Length;

    internal System.Int32 _TypePosition;
    public System.Int32 TypePosition => this._TypePosition;
    #endregion

    #region Methods
    internal void DefineTypePosition()
    {
      this._TypePosition = -1;

      if ((this.FileRegisterColumns == null) || (!(this.FileRegisterColumns.Any())))
        throw new System.Exception($"The FileRegisterColumns property cannot be null or empty.");

      if (this.FileRegisterColumns.Count(frc => frc.IsFileRegisterType) != 1)
        throw new System.Exception($"The '{this.Name}' FileRegister needs to contains the ONE FileRegisterColumn with the property 'IsFileRegisterType' = true.");

      foreach (SoftmakeAll.SDK.FileWR.FileRegisterColumn FileRegisterColumn in FileRegisterColumns)
      {
        FileRegisterColumn._StartPosition = this._Length;
        this._Length += FileRegisterColumn.ContentLength;
        if (FileRegisterColumn.IsFileRegisterType)
          this._TypePosition = FileRegisterColumn._StartPosition;
      }
    }
    #endregion
  }
}