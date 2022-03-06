namespace ReviewMe.Core.Infrastructures;

public interface IEmailSender
{
    Task SendAsync(EmailMessage emailMessage);
}