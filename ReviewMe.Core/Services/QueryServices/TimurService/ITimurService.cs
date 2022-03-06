namespace ReviewMe.Core.Services.QueryServices.TimurService;

public interface ITimurService
{
    Task<IReadOnlyCollection<ColleagueResponse>> GetEmployeeColleaguesAsync(int timurId);
    Task<IReadOnlyCollection<ProjectResponse>> GetAllProjectsAsync();
}