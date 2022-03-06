using ReviewMe.Core.Services.QueryServices.ReviewersFeedbackService;
using ReviewMe.Core.Services.QueryServices.ReviewersService;

namespace ReviewMe.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ReviewersController : Controller
{
    private readonly IReviewersService _reviewersService;
    private readonly IReviewerFeedbackService _reviewerFeedbackService;

    public ReviewersController(IReviewersService reviewersService, IReviewerFeedbackService reviewerFeedbackService)
    {
        _reviewersService = reviewersService;
        _reviewerFeedbackService = reviewerFeedbackService;
    }

    [Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpGet("employee/{employeeId:int}")]
    public async Task<GetAssessmentReviewersResponse> GetByEmployeeId(int employeeId)
        => await _reviewersService.GetByEmployeeIdAsync(employeeId);

    [Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpGet("Feedback/employee/{employeeId:int}")]
    public GetReviewerFeedbackResponse GetFeedBacksByEmployeeId(int employeeId)
        => _reviewerFeedbackService.GetByEmployeeId(employeeId);
}