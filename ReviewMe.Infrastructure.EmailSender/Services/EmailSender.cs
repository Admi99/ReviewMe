using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using ReviewMe.Core.DomainEntities;
using ReviewMe.Core.Infrastructures;
using ReviewMe.Infrastructure.EmailSender.Settings;

namespace ReviewMe.Infrastructure.EmailSender.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;
    private readonly EmailSettings _emailSettings;

    public EmailSender(ILogger<EmailSettings> logger, IOptions<EmailSettings> emailSettings)
    {
        _logger = logger;
        _emailSettings = emailSettings.Value;
    }

    public async Task SendAsync(EmailMessage emailMessage)
    {
        if (!emailMessage.To.Any()) return;

        try
        {
            _logger.LogInformation("Sending email to: '{to}', Subject: '{subject}'", emailMessage.To,
                emailMessage.Subject);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailSettings.SmtpReviewMeNoReplyName!, _emailSettings.SmtpReviewMeNoReply!));
            message.To.AddRange(emailMessage.To.Select(MailboxAddress.Parse));
            message.Cc.AddRange(emailMessage.Cc.Select(MailboxAddress.Parse));
            message.Bcc.AddRange(emailMessage.Bcc.Select(MailboxAddress.Parse));
            message.Subject = emailMessage.Subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            using var emailClient = new SmtpClient();

            await emailClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort,
                _emailSettings.SecureSocketOptions);

            await emailClient.SendAsync(message);
            await emailClient.DisconnectAsync(true);

            _logger.LogInformation("Sending email finished.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Sending email failed.");

            throw;
        }
    }
}