namespace ReviewMe.Infrastructure.DbStorage.DatabaseObjects;

public class AssessmentReviewerDo
{
    public AssessmentReviewerState AssessmentReviewerState { get; set; }

    public string Feedback { get; set; } = string.Empty;

    public string AreasForImprovements { get; set; } = string.Empty;

    public int EmployeeDoId { get; set; }

    public int AssessmentDoId { get; set; }

    public AssessmentDo? AssessmentDo { get; set; }

    public EmployeeDo? EmployeeDo { get; set; }


}