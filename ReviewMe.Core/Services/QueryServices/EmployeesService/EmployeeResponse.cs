namespace ReviewMe.Core.Services.QueryServices.EmployeesService;

public class EmployeeResponse
{
    public int Id { get; set; }

    public string SurnameFirstName { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public string ImageSrc { get; set; } = string.Empty;

    public string TeamLeaderName { get; set; } = string.Empty;

    public DateTimeOffset PerformanceReviewMonth { get; set; }

    public DateTimeOffset AssessmentDueDate { get; set; }

    public DateTimeOffset PerformanceReviewDate { get; set; }

    public bool HasOpenAssessment { get; set; }

    public string AdditionalFeedback { get; set; } = string.Empty;
}