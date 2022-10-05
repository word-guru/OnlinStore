namespace OnlinStore;

public interface IEmailSender
{
    void Send(string fromName,string fromEmail,string toEmail,string subject, string bodyHTML);
}