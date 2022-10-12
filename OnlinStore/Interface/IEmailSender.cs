namespace OnlinStore;

public interface IEmailSender
{
    void Send(string fromName,string toEmail,string subject, string bodyHTML);
}