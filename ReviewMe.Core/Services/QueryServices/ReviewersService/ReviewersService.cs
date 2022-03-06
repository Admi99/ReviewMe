using Microsoft.Extensions.Logging;
using ReviewMe.Core.Services.QueryServices.TimurService;

namespace ReviewMe.Core.Services.QueryServices.ReviewersService;

public class ReviewersService : IReviewersService
{
    private readonly IEmployeesRepository _employeesRepository;
    private readonly ITimurService _timurService;
    private readonly IAssessmentsRepository _assessmentsRepository;
    private readonly ILogger _logger;


    public ReviewersService(IEmployeesRepository employeesRepository,
        ITimurService timurService,
        IAssessmentsRepository assessmentsRepository, ILogger<ReviewersService> logger)
    {
        _employeesRepository = employeesRepository;
        _timurService = timurService;
        _assessmentsRepository = assessmentsRepository;
        _logger = logger;
    }

    public async Task<GetAssessmentReviewersResponse> GetByEmployeeIdAsync(int employeeId)
    {
        var employee = _employeesRepository.Get(employeeId);

        var assessment = _assessmentsRepository.GetWithReviewers(employeeId, AssessmentState.Open)
                         ?? new Assessment { AssessmentReviewers = new List<AssessmentReviewer>() };

        var assessmentReviewers = assessment.AssessmentReviewers
            .Select(reviewers => reviewers);

        var employeeColleagues = await _timurService.GetEmployeeColleaguesAsync(employee.TimurId);


        return new GetAssessmentReviewersResponse
        {
            AssessmentReviewers = employeeColleagues
                .Select(employeeColleague =>
                {
                    var employeeReviewer = _employeesRepository.GetByTimurId(employeeColleague.TimurId);
                    return (employeeColleague, employeeReviewer);
                })
                .Where(employeeColleagueAndReviewer =>
                {
                    if (employeeColleagueAndReviewer.employeeReviewer == null)
                    {
                        _logger.LogWarning("Unable to find '{@employeeColleagueAndReviewer.employeeColleague}' with TimurId '{@employeeColleagueAndReviewer.employeeColleague.TimurId}' in ReviewMe database, please check if SyncData in HR Service works correctly", employeeColleagueAndReviewer.employeeColleague.TimurId, employeeColleagueAndReviewer.employeeColleague);
                        return false;
                    }
                    return true;

                })
                .Select(
                    employeeColleagueAndReviewer =>
                        (employeeColleagueAndReviewer.employeeColleague.ProjectName, new Reviewer
                        {
                            IsSelected = IsReviewerSelected(assessmentReviewers, employeeColleagueAndReviewer.employeeReviewer!.Id),
                            Name = employeeColleagueAndReviewer.employeeReviewer.SurnameFirstName,
                            EmployeeId = employeeColleagueAndReviewer.employeeReviewer.Id,
                            ProjectId = employeeColleagueAndReviewer.employeeColleague.ProjectId,
                            IsProjectManager = IsManagerAsync(employeeColleagueAndReviewer.employeeReviewer.Id, employeeColleagueAndReviewer.employeeColleague.ProjectId).Result
                        }))

                .GroupBy(projectReviewers => projectReviewers.ProjectName, projectReviewers => projectReviewers.Item2)
                .ToDictionary(key => key.Key, value => value
                    .OrderByDescending(reviewer => reviewer.IsProjectManager)
                    .ThenBy(reviewer=>reviewer.Name)
                    .ToList() as IReadOnlyCollection<Reviewer>)
        };
    }

    private static bool IsReviewerSelected(IEnumerable<AssessmentReviewer> assessmentReviewers, int employeeId) 
        => assessmentReviewers.FirstOrDefault(reviewer =>
            reviewer.EmployeeId == employeeId) is not null;


    public async Task<bool> IsManagerAsync(int employeeId, int projectId)
    {
        var allProjects = await _timurService.GetAllProjectsAsync();
        var foundProject = allProjects.FirstOrDefault(projectResponse => projectResponse.Id == projectId);
        var employee = _employeesRepository.Get(employeeId);

        return foundProject != null && foundProject.ProjectManager.Id == employee.TimurId;
    }

}