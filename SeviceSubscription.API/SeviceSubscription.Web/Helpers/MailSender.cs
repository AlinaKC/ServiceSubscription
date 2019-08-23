using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SeviceSubscription.Web
{
    public class MailSender : IMailSender
    {
        private readonly IOptions<SmtpSettings> _options;

        public MailSender(IOptions<SmtpSettings> options)
        {
            _options = options;
        }
        public string SendEmail(string Email, string Message)
        {

            try
            {
                // Credentials
                var credentials = new NetworkCredential(_options.Value.UserName, _options.Value.Password);
                // Mail message
                var mail = new MailMessage()
                {
                    From = new MailAddress("noreply@coding.com"),
                    Subject = "Invoice of Service Subcription",
                    Body = Message
                };
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress(Email));
                // Smtp client
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };
                client.Send(mail);
                return "Email Sent Successfully!";
            }
            catch (System.Exception e)
            {
                return e.Message;
            }

        }
    }
}
