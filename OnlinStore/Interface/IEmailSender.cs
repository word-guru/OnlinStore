namespace OnlinStore;

public interface IEmailSender
{
    Task SendAsync(string fromName,string toEmail,string subject, string bodyHTML);
}