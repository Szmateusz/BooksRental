using System.Net;
using System.Net.Mail;

namespace Books.Models
{
    public class EmailSender
    {
        public static void SendEmail(string email, string subject, string message)
        {
            var fromAddress = new MailAddress("your-email@gmail.com", "Book Rental");
            var toAddress = new MailAddress(email);
            const string fromPassword = "your-email-password";
            const string smtpHost = "smtp.gmail.com";
            const int smtpPort = 587;

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
        }
    }
}
