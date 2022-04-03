using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReviewMe.Core.DomainEntities.EmailTemplateModels;
using ReviewMe.Core.Helpers;

namespace ReviewMe.Core.Services.EmailNotificationServices;

public class AssessmentsNotificationService : IAssessmentsNotificationService
{
    private readonly IEmailSendingService _emailSendingService;
    private readonly IEmployeesRepository _employeesRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public AssessmentsNotificationService(
        IEmailSendingService emailSendingService,
        IEmployeesRepository employeesRepository,
        ICurrentUserService currentUserService,
        IConfiguration configuration, 
        ILogger<AssessmentsNotificationService> logger)
    {
        _emailSendingService = emailSendingService;
        _employeesRepository = employeesRepository;
        _currentUserService = currentUserService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task NotifyOnOpenAssessment(int employeeId, IReadOnlyCollection<int> reviewers)
    {
        var employee = _employeesRepository.Get(employeeId);

        var assessment =
            employee.Assessments.FirstOrDefault(assessment => assessment.AssessmentState == AssessmentState.Open);

        if (assessment == null)
        {
            _logger.LogError("Cannot send email because assessment for employee: {employeeId} is not open", employeeId);
            return;
        }
        
        var hakkastackUrl = _configuration.GetSection("HakkastackUrl");

        var (recipientEmails, recipientNames) = GetRecipientsLoginAndName(reviewers);

        var subjectReviewerReminder = "ReviewMe - " + employee.SurnameFirstName;

        var recipients = recipientEmails.Zip(recipientNames);

        foreach (var (to, name) in recipients)
        {
            var t = "adam.michalek221@gmail.com";
            await SendTo(EmailTemplate.ReviewerFeedbackRequested, new List<string> { t }, subjectReviewerReminder, new ReviewerFeedbackRequestedModel
            {
                ReviewerName = name,
                AssessedPersonName = employee.SurnameFirstName,
                FeedbackUrl = hakkastackUrl["FeedbackUrl"] + "/" + assessment.Id,
                AssessmentDueDate = assessment.AssessmentDueDate,
                DeclineUrl = hakkastackUrl["DeclineUrl"] + assessment.Id
            });
        }

        var emailDomain = _configuration.GetSection("EmailDomain").Value;
        var prSessionRecipients = new List<string>
        {
            employee.Login + emailDomain,
            _currentUserService.UserNameWithoutDomain + emailDomain,
            // TODO - Here add Line manager
            // TODO - Here add Project manager
        };

        var subjectPerformanceReview = employee.SurnameFirstName + " - " + "Performance review";

        foreach (var prSessionRecipient in prSessionRecipients)
        {
            await SendTo(EmailTemplate.PerformanceReviewSessionReminder, new List<string> { prSessionRecipient }, subjectPerformanceReview, new PerformanceReviewSessionReminderModel
            {
                ReviewersNames = recipientNames,
                IsAdmin = prSessionRecipient.Contains(_currentUserService.UserNameWithoutDomain),
                AssessmentUrl = hakkastackUrl["AssessmentUrl"] + "/" + employeeId
            });
        }
    }

    public async Task NotifyOnUpdateAssessment(int employeeId, IReadOnlyCollection<int> reviewers,
        IReadOnlyCollection<int> canceledReviewers)
    {
        var employee = _employeesRepository.Get(employeeId);

        var assessment =
            employee.Assessments.FirstOrDefault(assessment => assessment.AssessmentState == AssessmentState.Open);

        if (assessment == null)
        {
            _logger.LogError($"Cannot send email because assessment for employee: {employeeId} is not open");
            return;
        }

        var hakkastackUrl = _configuration.GetSection("HakkastackUrl");

        var (recipientEmails, recipientName) = GetRecipientsLoginAndName(reviewers);
        var (canceledRecipientEmails, canceledRecipientName) = GetRecipientsLoginAndName(canceledReviewers);

        var subject = "ReviewMe - " + employee.SurnameFirstName;

        var recipients = recipientEmails.Zip(recipientName);

        foreach (var (to, name) in recipients)
        {
            await SendTo(EmailTemplate.ReviewerFeedbackRequested, new List<string> { to }, subject, new ReviewerFeedbackRequestedModel
            {
                ReviewerName = name,
                AssessedPersonName = employee.SurnameFirstName,
                FeedbackUrl = hakkastackUrl["FeedbackUrl"] + "/" + assessment.Id,
                AssessmentDueDate = assessment.AssessmentDueDate,
                DeclineUrl = hakkastackUrl["DeclineUrl"] + assessment.Id
            });
        }

        var canceledRecipients = canceledRecipientEmails.Zip(canceledRecipientName);

        foreach (var (to, name) in canceledRecipients)
        {
            await SendTo(EmailTemplate.ReviewerFeedbackCanceled, new List<string> { to }, subject, new ReviewerFeedbackCancelledModel
            {
                AssessedPersonName = employee.SurnameFirstName,
                ReviewerName = name
            });
        }

    }

    public async Task NotifyOnDeleteAssessment(int employeeId, IReadOnlyCollection<int> canceledReviewers)
    {
        var employee = _employeesRepository.Get(employeeId);
        var (recipientEmails, recipientNames) = GetRecipientsLoginAndName(canceledReviewers);

        var subject = "ReviewMe - " + employee.SurnameFirstName;

        var recipients = recipientEmails.Zip(recipientNames);

        foreach (var (to, name) in recipients)
        {
            await SendTo(EmailTemplate.ReviewerFeedbackCanceled, new List<string> { to }, subject,
                new ReviewerFeedbackCancelledModel
                {
                    AssessedPersonName = employee.SurnameFirstName,
                    ReviewerName = name
                });
        }
    }

    private async Task SendTo<T>(EmailTemplate emailTemplate, IReadOnlyCollection<string> recipients, string subject,
        T reviewerReminderModel)
    {
        if (recipients.Any() == false)
            return;

        var template = emailTemplate.ToTemplate();

        await _emailSendingService.SendEmail(recipients, new List<string>(), new List<string>(), template, subject, reviewerReminderModel);
    }

    private (IReadOnlyCollection<string>, IReadOnlyCollection<string>) GetRecipientsLoginAndName(IReadOnlyCollection<int> reviewers)
    {
        var recipientsEmail = new List<string>();
        var recipientsName = new List<string>();
        reviewers.ToList().ForEach(employeeId =>
        {
            var employee = _employeesRepository.Get(employeeId);
            recipientsEmail.Add(employee.Login + _configuration.GetSection("EmailDomain").Value);
            recipientsName.Add(employee.SurnameFirstName);
        });
        return (recipientsEmail, recipientsName);
    }
}