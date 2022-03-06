namespace ReviewMe.Core.Services.QueryServices.ReviewersService;

public interface IReviewersService
{
    Task<GetAssessmentReviewersResponse> GetByEmployeeIdAsync(int employeeId);
}