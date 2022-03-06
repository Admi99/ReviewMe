namespace ReviewMe.Core.DomainEntities;
using Enums;

public class AssessmentReviewer : IEntity
{
    public int Id { get; set; }

    public AssessmentReviewerState AssessmentReviewerState { get; set; }

    public string Feedback { get; set; } = string.Empty;

    public string AreasForImprovements { get; set; } = string.Empty;

    public int EmployeeId { get; set; }

    public int AssessmentId { get; set; }

    public Assessment? Assessment { get; set; }

    public Employee? Employee { get; set; }
}