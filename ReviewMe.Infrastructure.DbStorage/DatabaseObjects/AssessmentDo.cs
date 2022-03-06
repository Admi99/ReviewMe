namespace ReviewMe.Infrastructure.DbStorage.DatabaseObjects;

public class AssessmentDo
{
    public int Id { get; set; }

    public DateTimeOffset AssessmentDueDate { get; set; }

    public DateTimeOffset PerformanceReviewDate { get; set; }

    public AssessmentState AssessmentState { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string AdditionalFeedback { get; set; } = string.Empty;

    public IReadOnlyCollection<AssessmentReviewerDo> AssessmentReviewers { get; set; } =
        new List<AssessmentReviewerDo>();

    public int EmployeeDoId { get; set; }

    public EmployeeDo? EmployeeDo { get; set; }

    public int CreatedByEmployeeDoId { get; set; }

    public EmployeeDo? CreatedByEmployeeDo { get; set; }

}