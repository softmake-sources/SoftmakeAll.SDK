namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class Application
  {
    #region Properties
    public string Name { get; set; }
    public System.Collections.Generic.List<string> Channel_ids { get; set; }
    public System.Collections.Generic.List<string> Bridge_ids { get; set; }
    public System.Collections.Generic.List<string> Endpoint_ids { get; set; }
    public System.Collections.Generic.List<string> Device_names { get; set; }
    public System.Collections.Generic.List<object> Events_allowed { get; set; }
    public System.Collections.Generic.List<object> Events_disallowed { get; set; }
    #endregion
  }
}