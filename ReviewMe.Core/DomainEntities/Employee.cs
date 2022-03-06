namespace ReviewMe.Core.DomainEntities;

public class Employee : IEntity
{
    public int Id { get; set; }

    public int PpId { get; set; }

    public int TimurId { get; set; }

    public bool IsActive { get; set; }

    public string SurnameFirstName { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public string Login { get; set; } = string.Empty;

    public string TeamLeaderLogin { get; set; } = string.Empty;

    public DateTimeOffset PerformanceReviewMonth { get; set; }

    public IReadOnlyCollection<Assessment> Assessments { get; set; } = new List<Assessment>();

    public IReadOnlyCollection<AssessmentReviewer> AssessmentReviewers { get; set; } =
        new List<AssessmentReviewer>();

}