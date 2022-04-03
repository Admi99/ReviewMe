using ReviewMe.Core.Services.QueryServices.EmployeesService;

namespace ReviewMe.API.Controllers;

[Route("[controller]")]
[ApiController]
public class EmployeesController : Controller
{
    private readonly IEmployeesService _employeesService;

    public EmployeesController(IEmployeesService employeesService)
    {
        _employeesService = employeesService;
    }

    [Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpGet]
    public IReadOnlyCollection<EmployeeResponse> Get()
        => _employeesService.Get();

    [Authorize(Policy = AuthorizationPolicies.Employee)]
    [HttpGet("{id:int}")]
    public EmployeeResponse Get(int id)
        => _employeesService.Get(id);
}