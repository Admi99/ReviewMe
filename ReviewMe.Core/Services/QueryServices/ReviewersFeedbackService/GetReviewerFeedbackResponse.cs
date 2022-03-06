namespace ReviewMe.Core.Services.QueryServices.ReviewersFeedbackService;

public class GetReviewerFeedbackResponse
{
    public IReadOnlyCollection<ReviewerFeedback> Feedbacks { get; set; }

    public GetReviewerFeedbackResponse()
    {
        Feedbacks = new List<ReviewerFeedback>();
    }
}