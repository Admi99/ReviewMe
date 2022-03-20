namespace ReviewMe.Core.Services.QueryServices.EmployeesService;

public class EmployeesService : IEmployeesService
{
    private readonly IEmployeesRepository _employeesRepository;

    public EmployeesService(IEmployeesRepository employeesRepository)
    {
        _employeesRepository = employeesRepository;
    }

    public IReadOnlyCollection<EmployeeResponse> Get()
        => _employeesRepository
            .Get()
            .Where(employee => employee.IsActive)
            .Select(ToEmployeeResponse)
            .ToList();

    public EmployeeResponse Get(int id)
    {
        var employee = _employeesRepository.Get(id);

        if (employee.IsActive)
            return ToEmployeeResponse(employee);

        throw new Exception("Error: Employee is not active!");
    }

    private EmployeeResponse ToEmployeeResponse(Employee employee)
    {
        var assessment = employee.Assessments.FirstOrDefault(c => c.AssessmentState == AssessmentState.Open);

        return new EmployeeResponse
        {
            Id = employee.Id,
            SurnameFirstName = employee.SurnameFirstName,
            Location = employee.Location,
            Department = employee.Department,
            Position = employee.Position,
            PerformanceReviewMonth = employee.PerformanceReviewMonth,
            HasOpenAssessment = assessment != null,
            AssessmentDueDate = assessment?.AssessmentDueDate ?? default,
            PerformanceReviewDate = assessment?.PerformanceReviewDate ?? default,
            ImageSrc = Utilities.GetProfilePhoto(employee.Login),
            TeamLeaderName  = _employeesRepository.GetByLogin(employee.TeamLeaderLogin)?.SurnameFirstName ?? string.Empty
        };

    }

}