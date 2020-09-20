namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class AsteriskInfo
  {
    #region Properties
    public BuildInfo Build { get; set; }
    public SystemInfo System { get; set; }
    public ConfigInfo Config { get; set; }
    public StatusInfo Status { get; set; }
    #endregion
  }
}