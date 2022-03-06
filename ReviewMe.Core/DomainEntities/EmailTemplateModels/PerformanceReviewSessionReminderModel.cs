namespace ReviewMe.Core.DomainEntities.EmailTemplateModels;

public class PerformanceReviewSessionReminderModel
{
    public IReadOnlyCollection<string> ReviewersNames { get; set; } = new List<string>();

    public bool IsAdmin { get; set; }

    public string AssessmentUrl { get; set; } = string.Empty;

}