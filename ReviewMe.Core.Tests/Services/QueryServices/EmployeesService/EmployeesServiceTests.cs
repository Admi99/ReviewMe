using ReviewMe.Core.Services.QueryServices.EmployeesService;

namespace ReviewMe.Core.Tests.Services.QueryServices.EmployeesService;

[TestFixture]
public class EmployeesServiceTests
{
    [Test]
    public void Get()
    {
        //Arrange
        var employee1 = "John Doe";
        var employee2 = "John Black";
        var login1 = "login1";
        var login2 = "johny2";

        var teamLeaderLogin = "Elvis";

        var teamLeader = new Employee
        {
            Id = 666,
            SurnameFirstName = "Elvis Presley",
            IsActive = true,
            Assessments = new List<Assessment>(),
            Login = teamLeaderLogin,
            TeamLeaderLogin = teamLeaderLogin
        };


        var employeeOne = new Employee
        {
            Id = 5,
            SurnameFirstName = employee1,
            IsActive = true,
            Assessments = new List<Assessment>(),
            Login = login1,
            TeamLeaderLogin = teamLeaderLogin
        };

        var employeeTwo = new Employee
        {
            Id = 6,
            SurnameFirstName = employee2,
            IsActive = true,
            Assessments = new List<Assessment>(),
            Login = login2,
            TeamLeaderLogin = teamLeaderLogin
        };

        var expectedEmployees = new List<Employee>
        {
            employeeOne,

            employeeTwo
        };

        var expectedResult = new List<EmployeeResponse>
        {
            new()
            {
                Id = 5,
                SurnameFirstName = employee1,
                ImageSrc = $"https://timur.domain.local:8080/persons/login/{login1}/photo",
                TeamLeaderName = teamLeader.SurnameFirstName
            },
            new()
            {
                Id = 6,
                SurnameFirstName = employee2,
                ImageSrc = $"https://timur.domain.local:8080/persons/login/{login2}/photo",
                TeamLeaderName = teamLeader.SurnameFirstName
            }
        };

        var employeeRepository = Substitute.For<IEmployeesRepository>();
        employeeRepository.Get().Returns(expectedEmployees);
        employeeRepository.GetByLogin(teamLeaderLogin).Returns(teamLeader);

        var testee = new Core.Services.QueryServices.EmployeesService.EmployeesService(employeeRepository);

        // Act
        var actualResult = testee.Get();

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void Get_Id()
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var login = "login";
        var employeeId = 1;

        var teamLeaderLogin = "Elvis";

        var LineManager = new Employee
        {
            Id = 666,
            SurnameFirstName = "Elvis Presley",
            IsActive = true,
            Assessments = new List<Assessment>(),
            Login = teamLeaderLogin,
            TeamLeaderLogin = teamLeaderLogin
        };


        var expectedEmployee = new Employee
        {
            Id = employeeId,
            SurnameFirstName = "SurnameFirstName",
            IsActive = true,
            Assessments = new[]
            {
                new Assessment
                {
                    AssessmentDueDate = now,
                    PerformanceReviewDate = now.AddDays(1),
                }
            },
            Login = login,
            TeamLeaderLogin = teamLeaderLogin
        };

        var expectedResult = new EmployeeResponse
        {
            Id = 1,
            SurnameFirstName = "SurnameFirstName",
            ImageSrc = $"https://timur.domain.local:8080/persons/login/{login}/photo",
            HasOpenAssessment = true,
            AdditionalFeedback = "AdditionalFeedback",
            AssessmentDueDate = now,
            PerformanceReviewDate = now.AddDays(1),
            TeamLeaderName = LineManager.SurnameFirstName
        };

        var employeeRepository = Substitute.For<IEmployeesRepository>();
        employeeRepository.Get(employeeId).Returns(expectedEmployee);
        employeeRepository.GetByLogin(teamLeaderLogin).Returns(LineManager);


        var testee = new Core.Services.QueryServices.EmployeesService.EmployeesService(employeeRepository);

        // Act
        var actualResult = testee.Get(employeeId);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }
}