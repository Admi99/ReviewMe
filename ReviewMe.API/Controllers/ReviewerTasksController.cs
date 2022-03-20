using ReviewMe.Core.Services.QueryServices.ReviewerTasksService;

namespace ReviewMe.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ReviewerTasksController : Controller
{
    private readonly IReviewerTaskService _reviewerTaskService;

    public ReviewerTasksController(IReviewerTaskService reviewerTaskService)
    {
        _reviewerTaskService = reviewerTaskService;
    }

    //[Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpGet]
    public GetReviewerTasksResponse Get()
        => _reviewerTaskService.Get();

    //[Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpGet("assessment/{assessmentId:int}")]
    public GetReviewerTaskResponse Get(int assessmentId)
        => _reviewerTaskService.Get(assessmentId);

    //[Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpPost("assessment/{assessmentId:int}/Decline")]
    public void Decline(int assessmentId, DeclineRequest request)
        => _reviewerTaskService.Decline(assessmentId, request);

    //[Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpPost("assessment/{assessmentId:int}/Draft")]
    public void Draft(int assessmentId, DraftRequest draftRequest)
        => _reviewerTaskService.Draft(assessmentId, draftRequest);

    //[Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpPost("assessment/{assessmentId:int}/Submit")]
    public void Submit(int assessmentId, SubmitRequest submitRequest)
        => _reviewerTaskService.Submit(assessmentId, submitRequest);

}