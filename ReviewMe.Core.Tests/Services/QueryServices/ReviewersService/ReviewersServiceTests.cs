using Microsoft.Extensions.Logging;
using ReviewMe.Core.Services.QueryServices.ColleagueService;
using ReviewMe.Core.Services.QueryServices.ReviewersService;

namespace ReviewMe.Core.Tests.Services.QueryServices.ReviewersService;

[TestFixture]
public class ReviewersServiceTests
{
    [Test]
    public async Task GetByEmployeeIdAsync()
    {
        // Arrange
        var employeeId = 5;
        var employeeTimurId = 195;
        var employeeSurnameFirstName = "Doe John";

        var employeeColleagueId1 = 6;
        var employeeColleagueTimurId1 = 196;
        var employeeColleagueSurnameFirstName1 = "Brown John";
        var employeeColleaguePosition1 = "Developer";
        var employeeColleagueProjectName1 = "Avarda";

        var employeeColleagueId2 = 7;
        var employeeColleagueTimurId2 = 197;
        var employeeColleagueSurnameFirstName2 = "Red John";
        var employeeColleagueProjectName2 = "Interflex";

        var employee = new Employee
        {
            Id = employeeId,
            SurnameFirstName = employeeSurnameFirstName,
            TimurId = employeeTimurId,
        };
        var employeeColleague1 = new Employee
        {
            Id = employeeColleagueId1,
            SurnameFirstName = employeeColleagueSurnameFirstName1,
            TimurId = employeeColleagueTimurId1,
            Position = employeeColleaguePosition1
        };
        var employeeColleague2 = new Employee
        {
            Id = employeeColleagueId2,
            SurnameFirstName = employeeColleagueSurnameFirstName2,
            TimurId = employeeColleagueTimurId2,
        };

        var colleagues = new List<ColleagueResponse>
        {
            new()
            {
                TimurId = employeeColleagueTimurId1,
                Name = employeeColleagueSurnameFirstName1,
                ProjectName = employeeColleagueProjectName1,
                ProjectId = 10230
            },
            new()
            {
                TimurId = employeeColleagueTimurId2,
                Name = employeeColleagueSurnameFirstName2,
                ProjectName = employeeColleagueProjectName2,
                ProjectId = 10231
            }
        };

        var assessmentId = 2001;
        var assessmentReviewers = new List<AssessmentReviewer>
        {
            new()
            {
                Id = 1001,
                AssessmentId = assessmentId,
                EmployeeId = employeeColleagueId1
            },
            new()
            {
                Id = 1002,
                AssessmentId = assessmentId,
                EmployeeId = employeeColleagueId2
            }
        };
        var assessment = new Assessment
        {
            Id = assessmentId,
            AssessmentState = AssessmentState.Open,
            AssessmentReviewers = assessmentReviewers
        };

        var expectedResult = new GetAssessmentReviewersResponse
        {
            AssessmentReviewers = new Dictionary<string, IReadOnlyCollection<Reviewer>>
            {
                {
                    "Avarda", new List<Reviewer>
                    {
                        new()
                        {
                            EmployeeId = employeeColleagueId1,
                            IsSelected = true,
                            Name = employeeColleagueSurnameFirstName1,
                            ProjectId = 10230
                        }
                    }
                },
                {
                    "Interflex", new List<Reviewer>
                    {
                        new()
                        {
                            EmployeeId = employeeColleagueId2,
                            IsSelected = true,
                            Name = employeeColleagueSurnameFirstName2,
                            ProjectId = 10231
                        }
                    }
                }
            }
        };

        var employeeRepository = Substitute.For<IEmployeesRepository>();
        employeeRepository.Get(employeeId).Returns(employee);
        employeeRepository.GetByTimurId(employeeColleagueTimurId1).Returns(employeeColleague1);
        employeeRepository.GetByTimurId(employeeColleagueTimurId2).Returns(employeeColleague2);

        var timurService = Substitute.For<IColleagueService>();
        timurService.GetEmployeeColleaguesAsync(employeeTimurId).Returns(colleagues);

        var assessmentRepository = Substitute.For<IAssessmentsRepository>();
        assessmentRepository.Get(employeeId, AssessmentState.Open).Returns(assessment);
        assessmentRepository.GetWithReviewers(employeeId, AssessmentState.Open).Returns(assessment);

        var logger = Substitute.For<ILogger<Core.Services.QueryServices.ReviewersService.ReviewersService>>();
        var testee =
            new Core.Services.QueryServices.ReviewersService.ReviewersService(
                employeeRepository,
                timurService,
                assessmentRepository, logger);

        // Act
        var actualResult = await testee.GetByEmployeeIdAsync(employeeId);

        // Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }


    [Test]
    public void IsManagerAsyncTest()
    {
        // Arrange
        var projectManager = new ProjectManager
        {
            Id = 1,
            Name = "Lucky Luciano"
        };

        var project = new List<ProjectResponse>
        {
            new()
            {
                Id = 666,
                Name = "ReviewMe",
                ProjectManager = projectManager
            }
           
        };
        var employeeAl = new Employee
        {
            Id = 2,
            SurnameFirstName = "Alphonse Capone",
            TimurId = 2,
        };
        var employeeLucky = new Employee
        {
            Id = 5,
            SurnameFirstName = "Lucky Luciano",
            TimurId = 1,
        };
        

        var timurService = Substitute.For<IColleagueService>();
        var employeeRepository = Substitute.For<IEmployeesRepository>();
        var assessmentRepository = Substitute.For<IAssessmentsRepository>();
        var logger = Substitute.For<ILogger<Core.Services.QueryServices.ReviewersService.ReviewersService>>();

        timurService.GetAllProjectsAsync().Returns(project);

         var testee =
            new Core.Services.QueryServices.ReviewersService.ReviewersService(
                employeeRepository,
                timurService,
                assessmentRepository, logger);

        // Act
         employeeRepository.Get(employeeLucky.Id).Returns(employeeLucky);
        var empLucky = testee.IsManagerAsync(employeeLucky.Id, 666);

        // Assert
        empLucky.Should().BeTrue();


        // Act
        employeeRepository.Get(employeeAl.Id).Returns(employeeAl);
        var empAl =  testee.IsManagerAsync(employeeAl.Id, 666);

        // Assert
        empAl.Should().BeFalse();

    }

}