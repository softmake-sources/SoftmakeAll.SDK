using System.Linq;

namespace SoftmakeAll.SDK.MailServices
{
  public class Mail
  {
    #region Constructor
    public Mail()
    {
      this.From = new SoftmakeAll.SDK.MailServices.EmailAddress();
      this.To = new System.Collections.Generic.List<SoftmakeAll.SDK.MailServices.EmailAddress>();
    }
    #endregion

    #region Properties
    public System.String APIKeyPassword { get; set; }
    public SoftmakeAll.SDK.MailServices.EmailAddress From { get; set; }
    public System.Collections.Generic.List<SoftmakeAll.SDK.MailServices.EmailAddress> To { get; set; }
    public System.String Subject { get; set; }
    public System.String PlainTextContent { get; set; }
    public System.String HTMLContent { get; set; }
    #endregion

    #region Methods
    public async System.Threading.Tasks.Task<System.Net.HttpStatusCode> SendAsync()
    {
      if (System.String.IsNullOrWhiteSpace(this.APIKeyPassword))
        return System.Net.HttpStatusCode.Unauthorized;

      if ((this.From == null) || (System.String.IsNullOrWhiteSpace(this.From.Address)) || (this.To == null) || (!(To.Any())) || (To.Any(i => System.String.IsNullOrWhiteSpace(i.Address))))
        return System.Net.HttpStatusCode.BadRequest;

      SendGrid.SendGridClient Client = new SendGrid.SendGridClient(this.APIKeyPassword);
      SendGrid.Helpers.Mail.EmailAddress FromEmailAddress = new SendGrid.Helpers.Mail.EmailAddress(this.From.Address, this.From.Name);

      SendGrid.Helpers.Mail.SendGridMessage Message;
      if (this.To.Count == 1)
        Message = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmail(FromEmailAddress, new SendGrid.Helpers.Mail.EmailAddress(this.To[0].Address, this.To[0].Name), this.Subject, this.PlainTextContent, this.HTMLContent);
      else
        Message = SendGrid.Helpers.Mail.MailHelper.CreateSingleEmailToMultipleRecipients(FromEmailAddress, To.Select(i => new SendGrid.Helpers.Mail.EmailAddress(i.Address, i.Name)).ToList(), this.Subject, this.PlainTextContent, this.HTMLContent);

      return (await Client.SendEmailAsync(Message)).StatusCode;
    }
    #endregion
  }
}