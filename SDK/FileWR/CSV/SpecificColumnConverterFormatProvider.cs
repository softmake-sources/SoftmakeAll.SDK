namespace SoftmakeAll.SDK.FileWR.CSV
{
  public class SpecificColumnCultureInfo
  {
    #region Constructor
    public SpecificColumnCultureInfo() { }
    public SpecificColumnCultureInfo(System.Int32 ColumnIndex, System.Globalization.CultureInfo CultureInfo)
    {
      this.ColumnIndex = ColumnIndex;
      this.CultureInfo = CultureInfo;
    }
    #endregion

    #region Properties
    public System.Int32 ColumnIndex { get; set; }
    public System.Globalization.CultureInfo CultureInfo { get; set; }
    #endregion
  }
}