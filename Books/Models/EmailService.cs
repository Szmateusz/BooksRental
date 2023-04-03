using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Net.Mail;

public class EmailService
{
    private readonly string _gmailUsername;
    private readonly string _gmailPassword;

    public EmailService(string gmailUsername, string gmailPassword)
    {
        _gmailUsername = gmailUsername;
        _gmailPassword = gmailPassword;
    }

    public async Task SendReminderEmail(string emailTo, string bookTitle, DateTime dueDate)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Wypożyczalnia książek", _gmailUsername));
        message.To.Add(new MailboxAddress("", emailTo));
        message.Subject = "Przypomnienie o zwrocie książki";

        var builder = new BodyBuilder();
        builder.TextBody = $"Przypominamy, że masz zwrócić książkę \"{bookTitle}\" do {dueDate.ToShortDateString()}";

        message.Body = builder.ToMessageBody();

        using var client = new MailKit.Net.Smtp.SmtpClient();
        await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(_gmailUsername, _gmailPassword);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
