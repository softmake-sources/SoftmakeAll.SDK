namespace SoftmakeAll.SDK.MailServices
{
  public class EmailAddress
  {
    #region Constructor
    public EmailAddress() { }
    public EmailAddress(System.String Address)
    {
      this.Address = Address;
    }
    public EmailAddress(System.String Address, System.String Name)
    {
      this.Address = Address;
      this.Name = Name;
    }
    #endregion

    #region Properties
    public System.String Address { get; set; }
    public System.String Name { get; set; }
    #endregion
  }
}