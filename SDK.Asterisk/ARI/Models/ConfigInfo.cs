namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class ConfigInfo
  {
    #region Properties
    public string Name { get; set; }
    public string Default_language { get; set; }
    public int Max_channels { get; set; }
    public int Max_open_files { get; set; }
    public double Max_load { get; set; }
    public SetId Setid { get; set; }
    #endregion
  }
}