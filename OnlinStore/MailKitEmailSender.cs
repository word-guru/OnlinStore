using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace OnlinStore;

public class MailKitEmailSender : IEmailSender, IAsyncDisposable
{
    private readonly SmtpClient _client;
    private readonly SmtpConfig _smtpConfig;
    public MailKitEmailSender(IOptions<SmtpConfig> options)
    {
        _client = new SmtpClient();
         _smtpConfig = options.Value;
    }
    public async Task SendAsync(string fromName,string toEmail,string subject, string bodyHTML)
    {
        var message = new MimeMessage ();
        message.From.Add (new MailboxAddress (fromName,_smtpConfig.UserName));
        message.To.Add (MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart (TextFormat.Html) {Text = bodyHTML};

        if (!_client.IsConnected)
        {
           await _client.ConnectAsync(_smtpConfig.Host, _smtpConfig.Port, _smtpConfig.UseSsl);
        }
        if (!_client.IsAuthenticated)
        {
            // Note: only needed if the SMTP server requires authentication
            await _client.AuthenticateAsync(_smtpConfig.UserName, _smtpConfig.Password);
        }
       await _client.SendAsync(message);
        //Disconect();
    }
    
    public async ValueTask DisposeAsync()
    {
        await _client.DisconnectAsync(true);
        _client.Dispose();
    }
}
