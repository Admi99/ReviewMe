namespace ReviewMe.Core.Services.QueryServices.ReviewersService;

public class GetAssessmentReviewersResponse
{
    public Dictionary<string, IReadOnlyCollection<Reviewer>> AssessmentReviewers { get; set; }
    public GetAssessmentReviewersResponse()
    {
        AssessmentReviewers = new Dictionary<string, IReadOnlyCollection<Reviewer>>();
    }
}