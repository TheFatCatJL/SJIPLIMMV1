using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using SJIP_LIMMV1.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace SJIP_LIMMV1.Services
{
    public class EmailSender : IEmailSender
    {
        private string apiKey = ""; // change this to your SendGrid APIKey (full one, not the one that is used for website identifier)
        private string mykey { get; set; }
        //unused for now, leaving open for future implementations
        public ApplicationUser userFrom { get; set; }
        public string userFromEmail {get;set;}
        public string userFromName { get; set; }

        public EmailSender()
        {
            // Future Implementations
        }
        public EmailSender(ApplicationUser user)
        {
            mykey = apiKey;
            userFromEmail = "ia-jason.limeh@surbanajurong.com";
            userFromName = "LIMM Administrator";
            userFrom = user;
        }

        public Task SendEmailAsync(string emailSendTo, string subject, string message)
        {
            return Execute(mykey, subject, message, emailSendTo, userFromEmail, userFromName);
        }

        public async Task Execute(string apiKey, string subject, string message, string emailSendTo, string userFromEmail, string userFromName)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(userFromEmail, userFromName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(emailSendTo));

            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }
    }
}