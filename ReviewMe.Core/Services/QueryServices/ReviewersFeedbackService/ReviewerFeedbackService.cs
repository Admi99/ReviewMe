namespace ReviewMe.Core.Services.QueryServices.ReviewersFeedbackService;

public class ReviewerFeedbackService : IReviewerFeedbackService
{
    private readonly IAssessmentsRepository _assessmentsRepository;


    public ReviewerFeedbackService(IAssessmentsRepository assessmentsRepository)
    {
        _assessmentsRepository = assessmentsRepository;
    }

    public GetReviewerFeedbackResponse GetByEmployeeId(int employeeId)
    {
        var assessment = _assessmentsRepository.GetWithReviewersWithEmployees(employeeId, AssessmentState.Open)
                         ?? new Assessment { AssessmentReviewers = new List<AssessmentReviewer>() };

        return new GetReviewerFeedbackResponse
        {
            Feedbacks = assessment.AssessmentReviewers
                .Select(reviewers => reviewers)
                .Where(assessmentReviewer =>
                    assessmentReviewer.AssessmentReviewerState is AssessmentReviewerState.Declined
                        or AssessmentReviewerState.Reviewed)
                .Select(reviewer => new ReviewerFeedback
                {
                    Name = reviewer.Employee!.SurnameFirstName,
                    Feedback = reviewer.Feedback,
                    ImageSrc = Utilities.GetProfilePhoto(reviewer.Employee.Login),
                    AssessmentReviewerState = reviewer.AssessmentReviewerState,
                    AreasForImprovements = reviewer.AreasForImprovements
                })
                .ToList()
        };
    }
}
