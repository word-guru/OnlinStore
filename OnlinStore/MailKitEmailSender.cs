using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace OnlinStore;

public class MailKitEmailSender : IEmailSender, IDisposable
{
    private readonly SmtpClient _client;
    private readonly SmtpConfig _smtpConfig;
    public MailKitEmailSender(IOptions<SmtpConfig> options)
    {
        _client = new SmtpClient();
         _smtpConfig = options.Value;
    }
    public void Send(string fromName,string toEmail,string subject, string bodyHTML)
    {
        var message = new MimeMessage ();
        message.From.Add (new MailboxAddress (fromName,_smtpConfig.UserName));
        message.To.Add (MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart (TextFormat.Html) {Text = bodyHTML};

        if (!_client.IsConnected)
        {
            _client.Connect(_smtpConfig.Host, _smtpConfig.Port, _smtpConfig.UseSsl);
        }
        if (!_client.IsAuthenticated)
        {
            // Note: only needed if the SMTP server requires authentication
            _client.Authenticate (_smtpConfig.UserName, _smtpConfig.Password);
        }
        _client.Send (message);
        //Disconect();
    }
    public void Dispose()
    {
        _client.Disconnect(true);
        _client.Dispose();
    }
}
