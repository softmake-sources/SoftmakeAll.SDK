namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class Bridge
  {
    #region Properties
    public string Id { get; set; }
    public string Technology { get; set; }
    public string Bridge_type { get; set; }
    public string Bridge_class { get; set; }
    public string Creator { get; set; }
    public string Name { get; set; }
    public System.Collections.Generic.List<string> Channels { get; set; }
    public string Video_mode { get; set; }
    public string Video_source_id { get; set; }
    public System.DateTimeOffset Creationtime { get; set; }
    #endregion
  }
}