namespace ReviewMe.Core.Services.QueryServices.EmployeesService;

public interface IEmployeesService
{
    IReadOnlyCollection<EmployeeResponse> Get();
    EmployeeResponse Get(int id);
}