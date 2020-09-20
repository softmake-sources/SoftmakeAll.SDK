namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class BridgeAttendedTransferEvent : SoftmakeAll.SDK.Asterisk.ARI.Models.Event
  {
    #region Properties
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Transferer_first_leg { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Transferer_second_leg { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Replace_channel { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Transferee { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Transfer_target { get; set; }
    public string Result { get; set; }
    public bool Is_external { get; set; }
    public Bridge Transferer_first_leg_bridge { get; set; }
    public Bridge Transferer_second_leg_bridge { get; set; }
    public string Destination_type { get; set; }
    public string Destination_bridge { get; set; }
    public string Destination_application { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Destination_link_first_leg { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Destination_link_second_leg { get; set; }
    public SoftmakeAll.SDK.Asterisk.ARI.Models.Channel Destination_threeway_channel { get; set; }
    public Bridge Destination_threeway_bridge { get; set; }
    #endregion
  }
}