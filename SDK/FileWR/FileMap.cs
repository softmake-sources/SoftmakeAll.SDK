using System.Linq;
using SoftmakeAll.SDK.Helpers.LINQ.Extensions;

namespace SoftmakeAll.SDK.FileWR
{
  public class FileMap
  {
    #region Constructor
    public FileMap(System.Int32 FileRegistersLength)
    {
      if (FileRegistersLength <= 0)
        throw new System.Exception("The parameter FileRegistersLength must be greather then 0.");

      this.FileRegistersLength = FileRegistersLength;
      this.FileRegisters = new System.Collections.Generic.List<SoftmakeAll.SDK.FileWR.FileRegister>();
    }
    #endregion

    #region Properties
    public System.Int32 FileRegistersLength { get; }
    public System.Collections.Generic.List<SoftmakeAll.SDK.FileWR.FileRegister> FileRegisters { get; }

    private System.Boolean _Builded;
    internal System.Boolean Builded => this._Builded;
    #endregion

    #region Methods
    public void BuildMap()
    {
      this._Builded = false;

      if ((this.FileRegisters == null) || (!(this.FileRegisters.Any())))
        throw new System.Exception($"The FileRegisters property cannot be null or empty.");

      if (this.FileRegisters.Count != this.FileRegisters.DistinctBy(fr => fr.Type).Count())
        throw new System.Exception($"The file register types must be unique.");

      foreach (SoftmakeAll.SDK.FileWR.FileRegister FileRegister in FileRegisters)
      {
        FileRegister.DefineTypePosition();

        if (FileRegister.FileRegisterColumns.Sum(frc => frc.ContentLength) != FileRegistersLength)
          throw new System.Exception($"The file registers length must be {FileRegistersLength}.");
      }

      this._Builded = true;
    }
    #endregion
  }
}