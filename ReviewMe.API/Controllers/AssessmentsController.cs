using ReviewMe.Core.Services.CommandServices.AssessmentsService;

namespace ReviewMe.API.Controllers;

[Route("[controller]")]
[ApiController]
public class AssessmentsController : Controller
{
    private readonly IAssessmentsService _assessmentsService;

    public AssessmentsController(IAssessmentsService assessmentsService)
    {
        _assessmentsService = assessmentsService;
    }

    [Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpPost("employee/{employeeId:int}/[action]")]
    public void Open(int employeeId, OpenAssessmentRequest request)
        => _assessmentsService.OpenAssessment(employeeId, request);

    [Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpPost("employee/{employeeId:int}/[action]")]
    public void Update(int employeeId, UpdateAssessmentRequest request)
        => _assessmentsService.UpdateAssessment(employeeId, request);

    [Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpPost("employee/{employeeId:int}/[action]")]
    public void Close(int employeeId)
        => _assessmentsService.CloseAssessment(employeeId);

    [Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpDelete("employee/{employeeId:int}")]
    public void Delete(int employeeId)
        => _assessmentsService.DeleteAssessment(employeeId);

    [Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpPost("employee/{employeeId:int}/[action]")]
    public void AdditionalFeedback(int employeeId, SaveAdditionalFeedbackRequest request)
        => _assessmentsService.SaveAdditionalFeedback(employeeId, request);
}