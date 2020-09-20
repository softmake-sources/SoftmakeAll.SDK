namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class BridgeBlindTransferEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Channel { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Replace_channel { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Transferee { get; set; }
    public string Exten { get; set; }
    public string Context { get; set; }
    public string Result { get; set; }
    public bool Is_external { get; set; }
    public Bridge Bridge { get; set; }
    #endregion
  }
}