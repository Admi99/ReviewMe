namespace ReviewMe.Core.DomainEntities.EmailTemplateModels;

public class ReviewerFeedbackRequestedModel
{
    public string ReviewerName { get; set; } = string.Empty;

    public string AssessedPersonName { get; set; } = string.Empty;

    public string FeedbackUrl { get; set; } = string.Empty;

    public string DeclineUrl { get; set; } = string.Empty;

    public DateTimeOffset AssessmentDueDate { get; set; }
}