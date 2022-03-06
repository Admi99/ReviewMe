namespace ReviewMe.Core.Helpers;

public static class EmailNotificationTemplateTypeExtensions
{
    private static readonly string ReviewerFeedbackRequested = "/EmailTemplates/ReviewerFeedbackRequested.cshtml";

    private static readonly string PerformanceReviewSessionReminderTemplate = "/EmailTemplates/PerformanceReviewSessionReminder.cshtml";

    private static readonly string ReviewerFeedbackCanceled = "/EmailTemplates/ReviewerFeedbackCanceled.cshtml";

    public static string ToTemplate(this EmailTemplate emailTemplate) => emailTemplate switch
    {
        EmailTemplate.ReviewerFeedbackRequested => ReviewerFeedbackRequested,
        EmailTemplate.PerformanceReviewSessionReminder => PerformanceReviewSessionReminderTemplate,
        EmailTemplate.ReviewerFeedbackCanceled => ReviewerFeedbackCanceled,
        _ => throw new Exception("Template not found !")
    };

}