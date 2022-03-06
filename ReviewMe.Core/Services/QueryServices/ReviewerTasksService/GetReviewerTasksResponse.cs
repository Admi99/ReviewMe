namespace ReviewMe.Core.Services.QueryServices.ReviewerTasksService;

public class GetReviewerTasksResponse
{
    public IReadOnlyCollection<ReviewerTask> ReviewerTasks { get; set; } = new List<ReviewerTask>();
}