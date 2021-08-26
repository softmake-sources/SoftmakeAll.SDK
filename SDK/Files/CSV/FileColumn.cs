namespace SoftmakeAll.SDK.Files.CSV
{
  public class FileColumn
  {
    #region Constructor
    public FileColumn() { }
    #endregion

    #region Properties
    public System.String Name { get; set; }
    public System.Int32 Index { get; set; }

    public System.String _DataTypeName;
    public System.String DataTypeName
    {
      get
      {
        return this._DataTypeName;
      }
      set
      {
        if ((this._DataTypeName == null) || (this._DataTypeName == value))
        {
          this._DataTypeName = value;
          return;
        }

        if ((value == "String") || (SoftmakeAll.SDK.Files.CSV.ConverterEngine.AllowedDataTypeNameReplacements.Exists(i => i.Item1 == this._DataTypeName && i.Item2 == value)))
          this._DataTypeName = value;
      }
    }

    private System.Int32 _MaxLength = -1;
    public System.Int32 MaxLength
    {
      get
      {
        if (this.DataTypeName == "String")
          return this._MaxLength;
        return -1;
      }
      set
      {
        this._MaxLength = System.Math.Max(this._MaxLength, value);
      }
    }

    private System.Boolean _AllowNulls = false;
    public System.Boolean AllowNulls
    {
      get
      {
        return this._AllowNulls;
      }
      set
      {
        if (!(this._AllowNulls))
          this._AllowNulls = value;
      }
    }

    public System.Globalization.CultureInfo CultureInfo { get; set; }
    #endregion
  }
}