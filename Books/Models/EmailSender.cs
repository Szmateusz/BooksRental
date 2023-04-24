using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace Books.Models
{
    public class EmailSender
    {
        public readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> emailSettings) { 
            _emailSettings = emailSettings.Value;
        }

        public  bool SendEmail(string email, string subject, string message)
        {
            try
            {
                var fromAddress = new MailAddress(_emailSettings.FromAddress, "Book Rental");
                var toAddress = new MailAddress(email);
                string fromPassword =_emailSettings.FromPassword;
                string smtpHost = _emailSettings.SmtpHost;
                int smtpPort = _emailSettings.SmtpPort;

                var smtp = new SmtpClient
                {
                    Host = smtpHost,
                    Port = smtpPort,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
                };

                var mailMessage = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = message
                };

                smtp.Send(mailMessage);

                return true;

            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex);
                return false;
            }
          
        }
    }
}
