namespace ReviewMe.Core.Infrastructures;

public interface IEmailSendingService
{
    Task SendEmail<T>(IEnumerable<string> recipientsTo, IEnumerable<string> recipientsCc,
        IEnumerable<string> recipientsBcc, string template, string subject, T model);
}