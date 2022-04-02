
namespace ReviewMe.Core.Services.QueryServices.ColleagueService;

public interface IColleagueService
{
    IReadOnlyCollection<ColleagueResponse> GetEmployeeColleaguesAsync(int timurId);
    IReadOnlyCollection<ProjectResponse> GetAllProjectsAsync();
}