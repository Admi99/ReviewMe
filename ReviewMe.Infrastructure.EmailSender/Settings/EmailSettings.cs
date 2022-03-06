using MailKit.Security;

namespace ReviewMe.Infrastructure.EmailSender.Settings;

public class EmailSettings
{
    public string? SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string? SmtpReviewMeNoReply { get; set; }
    public string? SmtpReviewMeNoReplyName { get; set; }
    public SecureSocketOptions SecureSocketOptions { get; set; }
}