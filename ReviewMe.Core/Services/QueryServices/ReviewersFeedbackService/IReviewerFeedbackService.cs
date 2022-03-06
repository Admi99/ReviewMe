namespace ReviewMe.Core.Services.QueryServices.ReviewersFeedbackService;
public interface IReviewerFeedbackService
{
    GetReviewerFeedbackResponse GetByEmployeeId(int employeeId);

}

