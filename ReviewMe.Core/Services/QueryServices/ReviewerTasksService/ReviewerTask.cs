
namespace ReviewMe.Core.Services.QueryServices.ReviewerTasksService;

public class ReviewerTask
{
    public int AssessmentId { get; set; }

    public string SurnameFirstName { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Position { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string ImageSrc { get; set; } = string.Empty;

    public DateTimeOffset AssessmentDueDate { get; set; }

    public AssessmentReviewerState ReviewerTaskState { get; set; }

    public string Feedback { get; set; } = string.Empty;

    public string TeamLeaderName { get; set; } = string.Empty;

    public string AreasForImprovements { get; set; } = string.Empty;
}