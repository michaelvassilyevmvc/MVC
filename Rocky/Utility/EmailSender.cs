using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace Rocky.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public MailJetSettings _mailJetSettings { get; set; }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }

        public async Task Execute(string email, string subject, string htmlMessage)
        {
            _mailJetSettings = _configuration.GetSection("MailJet").Get<MailJetSettings>();

            MailjetClient client =
            new MailjetClient
                            (
                                _mailJetSettings.ApiKey,
                                _mailJetSettings.SecretKey
                            );

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource
            };

            var tmpEmail = new TransactionalEmailBuilder()
                   .WithFrom(new SendContact("rebusmv@proton.me"))
                   .WithSubject(subject)
                   .WithHtmlPart(htmlMessage)
                   .WithTo(new SendContact(email))
                   .Build();

            await client.SendTransactionalEmailAsync(tmpEmail);
        }
    }
}