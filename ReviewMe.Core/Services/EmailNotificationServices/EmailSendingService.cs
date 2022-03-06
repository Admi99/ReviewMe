using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ReviewMe.Core.Settings;

namespace ReviewMe.Core.Services.EmailNotificationServices;

public class EmailSendingService : IEmailSendingService
{
    private readonly IEmailSender _emailSender;
    private readonly IRazorTemplateParsingService _razorTemplateParsingService;
    private readonly ILogger _logger;
    private readonly ApplicationSettings _applicationSettings;

    public EmailSendingService(
        IEmailSender emailSender,
        ILogger<EmailSendingService> logger,
        IOptions<ApplicationSettings> applicationSettings,
        IRazorTemplateParsingService razorTemplateParsingService)
    {
        _emailSender = emailSender;
        _logger = logger;
        _razorTemplateParsingService = razorTemplateParsingService;
        _applicationSettings = applicationSettings.Value;
    }

    public async Task SendEmail<T>(IEnumerable<string> recipientsTo, IEnumerable<string> recipientsCc, IEnumerable<string> recipientsBcc, string template,
         string subject, T model)
    {
        var mailMessage =
            await GetEmailMessage(template, model, subject, recipientsTo, recipientsCc, recipientsBcc);
        await _emailSender.SendAsync(mailMessage);
    }

    private async Task<EmailMessage> GetEmailMessage<T>(
        string razorTemplate, T model, string subject,
        IEnumerable<string> recipientsTo,
        IEnumerable<string> recipientsCc,
        IEnumerable<string> recipientsBcc)
    {
        var layout = await _razorTemplateParsingService.GetEmbeddedStaticTemplate("Layout.cshtml", "EmailTemplates");

        var body = await GetBodyFromEmailRazorTemplate(razorTemplate, model);

        var emailMessage = new EmailMessage
        {
            To = recipientsTo,
            Cc = recipientsCc,
            Bcc = recipientsBcc
        };

        if (_applicationSettings.UseTestEmailAddresses && _applicationSettings.TestEmailAddresses.Count > 0)
        {
            _logger.LogDebug("Modifying to test email.");

            var testFooterBody = await GetBodyFromEmailRazorTemplate(
                "/EmailTemplates/TestEmailFooter.cshtml",
                emailMessage);

            emailMessage.To = _applicationSettings.TestEmailAddresses;
            emailMessage.Cc = new List<string>(0);
            emailMessage.Bcc = new List<string>(0);

            subject = string.Join(
                " - ",
                subject,
                subject, "[" + "Develop" + "]");
            body += testFooterBody;
        }

        emailMessage.Subject = subject;
        emailMessage.Content = layout
            .Replace(@"{{tableBody}}", body);

        return emailMessage;
    }

    private async Task<string> GetBodyFromEmailRazorTemplate<T>(
        string razorTemplate, T model)
        => await _razorTemplateParsingService.RenderAsync(razorTemplate, model);
}