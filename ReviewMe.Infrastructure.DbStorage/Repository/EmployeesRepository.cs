namespace ReviewMe.Infrastructure.DbStorage.Repository;

internal class EmployeesRepository : IEmployeesRepository
{
    private readonly ReviewMeDbContext _db;
    private readonly IMapper _mapper;

    public EmployeesRepository(ReviewMeDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public IReadOnlyCollection<Employee> Get()
        => _db.Employees
            .Include(employee => employee.Assessments)
            .OrderBy(employee => employee.SurnameFirstName)
            .ToList()
            .Map<IReadOnlyCollection<Employee>>(_mapper);

    public Employee Get(int id)
        => _db.Employees
            .Include(employee => employee.Assessments)
            .FirstOrDefault(employee => employee.Id == id)
            ?.Map<Employee>(_mapper) ?? throw new Exception($"Employee with id: {id} not found !");

    public Employee GetWithAllDetails(int id)
        => _db.Employees
            .Include(employee => employee.AssessmentReviewers)
            .ThenInclude(employee => employee.AssessmentDo)
            .ThenInclude(employee => employee!.EmployeeDo)
            .FirstOrDefault(employee => employee.Id == id)
            ?.Map<Employee>(_mapper) ?? throw new Exception($"Employee with id: {id} not found !");


    public Employee? GetByTimurId(int timurId) =>
        _db.Employees
            .FirstOrDefault(employee => employee.TimurId == timurId)
            .Map<Employee?>(_mapper);

    public Employee? GetByLogin(string? login)
        => _db.Employees
            .FirstOrDefault(employee => employee.Login == login)
            .Map<Employee?>(_mapper);
}