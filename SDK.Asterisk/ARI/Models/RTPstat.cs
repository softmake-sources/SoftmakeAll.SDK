namespace SoftmakeAll.SDK.Asterisk.ARI.Models
{
  public class RTPstat
  {
    #region Properties
    public int Txcount { get; set; }
    public int Rxcount { get; set; }
    public double Txjitter { get; set; }
    public double Rxjitter { get; set; }
    public double Remote_maxjitter { get; set; }
    public double Remote_minjitter { get; set; }
    public double Remote_normdevjitter { get; set; }
    public double Remote_stdevjitter { get; set; }
    public double Local_maxjitter { get; set; }
    public double Local_minjitter { get; set; }
    public double Local_normdevjitter { get; set; }
    public double Local_stdevjitter { get; set; }
    public int Txploss { get; set; }
    public int Rxploss { get; set; }
    public double Remote_maxrxploss { get; set; }
    public double Remote_minrxploss { get; set; }
    public double Remote_normdevrxploss { get; set; }
    public double Remote_stdevrxploss { get; set; }
    public double Local_maxrxploss { get; set; }
    public double Local_minrxploss { get; set; }
    public double Local_normdevrxploss { get; set; }
    public double Local_stdevrxploss { get; set; }
    public double Rtt { get; set; }
    public double Maxrtt { get; set; }
    public double Minrtt { get; set; }
    public double Normdevrtt { get; set; }
    public double Stdevrtt { get; set; }
    public int Local_ssrc { get; set; }
    public int Remote_ssrc { get; set; }
    public int Txoctetcount { get; set; }
    public int Rxoctetcount { get; set; }
    public string Channel_uniqueid { get; set; }
    #endregion
  }
}
