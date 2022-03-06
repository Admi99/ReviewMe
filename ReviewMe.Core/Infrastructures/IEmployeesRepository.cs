namespace ReviewMe.Core.Infrastructures;

public interface IEmployeesRepository
{
    IReadOnlyCollection<Employee> Get();
    Employee Get(int id);
    Employee? GetByTimurId(int timurId);
    Employee GetWithAllDetails(int id);
    Employee? GetByLogin(string loginName);
}