using System.Linq;

namespace SoftmakeAll.SDK.FileWR
{
  public struct Data
  {
    #region Properties
    public System.Byte RegisterType { get; set; }
    public System.Collections.Generic.List<System.Byte[]> ColumnValues { get; set; }
    #endregion

    #region Methods
    public System.Collections.Generic.IEnumerable<System.String> ConvertColumnValuesToString()
    {
      return this.ColumnValues.Select(cv => System.Text.Encoding.UTF8.GetString(cv));
    }
    #endregion
  }
}