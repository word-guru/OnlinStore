using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace OnlinStore;

public class MailKitEmailSender : IEmailSender
{
    public void Send(string fromName,string fromEmail,string toEmail,string subject, string bodyHTML)
    {
        var message = new MimeMessage ();
        message.From.Add (new MailboxAddress (fromName,fromEmail));
        message.To.Add (MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        message.Body = new TextPart (TextFormat.Html) {Text = bodyHTML};

        using (var client = new SmtpClient ()) {
            client.Connect ("smtp.beget.com", 25, false);

            // Note: only needed if the SMTP server requires authentication
            client.Authenticate ("asp2022pd011@rodion-m.ru", "6WU4x2be");

            client.Send (message);
            client.Disconnect (true);
        }
    }
}