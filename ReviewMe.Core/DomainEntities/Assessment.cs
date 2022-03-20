namespace ReviewMe.Core.DomainEntities;

public class Assessment : IEntity
{
    public int Id { get; set; }

    public DateTimeOffset AssessmentDueDate { get; set; }

    public DateTimeOffset PerformanceReviewDate { get; set; }

    public AssessmentState AssessmentState { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public int EmployeeId { get; set; }

    public Employee? Employee { get; set; }

    public int CreatedByEmployeeId { get; set; }

    public Employee? CreatedByEmployee { get; set; }

    public IReadOnlyCollection<AssessmentReviewer> AssessmentReviewers { get; set; } =
        new List<AssessmentReviewer>();

}