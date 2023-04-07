using System.Net;
using System.Net.Mail;

namespace Books.Models
{
    public class EmailSender
    {
        public static bool SendEmail(string email, string subject, string message)
        {
            try
            {
                var fromAddress = new MailAddress("warmachine3001wm@gmail.com", "Book Rental");
                var toAddress = new MailAddress(email);
                const string fromPassword = "aslfraaxhvbggcrs";
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
