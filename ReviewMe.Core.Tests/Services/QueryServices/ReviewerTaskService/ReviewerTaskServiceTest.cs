using ReviewMe.Core.Services.QueryServices.ReviewerTasksService;

namespace ReviewMe.Core.Tests.Services.QueryServices.ReviewerTaskService;

[TestFixture]
public class ReviewerTaskServiceTest
{
    [Test]
    public void Get()
    {
        //Arrange
        var assessment = new Assessment
        {
            Id = 1003,
            AssessmentDueDate = DateTimeOffset.UtcNow,
            PerformanceReviewDate = new DateTime(2022, 1, 1),
            AssessmentState = 0,
            EmployeeId = 191
        };

        var assessmentReviewer = new AssessmentReviewer
        {
            EmployeeId = 562,
            AssessmentId = 1003,
            Assessment = assessment,
            AreasForImprovements = "Hey what is?!"
        };

        var employee1 = new Employee
        {
            Id = 191,
            SurnameFirstName = "Jirka",
            Position = "Manager",
            Login = "jirik",
            AssessmentReviewers = new List<AssessmentReviewer> { assessmentReviewer }
        };

        assessment.Employee = employee1;

        var expectedResult = new GetReviewerTasksResponse
        {
            ReviewerTasks = new List<ReviewerTask>
            {
                new()
                {
                    AssessmentId = assessment.Id,
                    SurnameFirstName = assessment.Employee.SurnameFirstName,
                    Department = assessment.Employee.Department,
                    Position = assessment.Employee.Position,
                    ImageSrc = $"https://timur.domain.local:8080/persons/login/{assessment.Employee.Login}/photo",
                    AssessmentDueDate = assessment.AssessmentDueDate,
                    AreasForImprovements = "Hey what is?!"

                }
            }
        };

        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.UserNameWithoutDomain.Returns(employee1.Login);

        var employeeRepository = Substitute.For<IEmployeesRepository>();
        employeeRepository.GetByLogin(employee1.Login).Returns(employee1);
        employeeRepository.GetWithAllDetails(employee1.Id).Returns(employee1);

        var assessmentRepository = Substitute.For<IAssessmentReviewerRepository>();

        var testee =
            new Core.Services.QueryServices.ReviewerTasksService.ReviewerTaskService(employeeRepository, currentUserService, assessmentRepository);


        //Act
        var actualResult = testee.Get();

        //Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void Decline()
    {
        // Arrange
        var assessment = new Assessment
        {
            Id = 1003,
            AssessmentDueDate = DateTimeOffset.UtcNow,
            PerformanceReviewDate = new DateTime(2022, 1, 1),
            AssessmentState = 0,
            EmployeeId = 191
        };

        var assessmentReviewer = new AssessmentReviewer
        {
            EmployeeId = 562,
            AssessmentId = 1003,
            Assessment = assessment
        };

        var employee1 = new Employee
        {
            Id = 191,
            SurnameFirstName = "Jirka",
            Position = "Manager",
            Login = "jirik",
            AssessmentReviewers = new List<AssessmentReviewer> { assessmentReviewer }
        };

        var declineRequest = new DeclineRequest
        {
            Reason = "I do not want to review this person."
        };

        assessment.Employee = employee1;

        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.UserNameWithoutDomain.Returns(employee1.Login);

        var employeeRepository = Substitute.For<IEmployeesRepository>();
        employeeRepository.GetByLogin(employee1.Login).Returns(employee1);

        var assessmentReviewerRepository = Substitute.For<IAssessmentReviewerRepository>();
        assessmentReviewerRepository.Get(employee1.Id, assessment.Id).Returns(assessmentReviewer);

        var expectedResult = new AssessmentReviewer
        {
            Assessment = assessmentReviewer.Assessment,
            AssessmentId = assessmentReviewer.Assessment.Id,
            AssessmentReviewerState = AssessmentReviewerState.Declined,
            Feedback = declineRequest.Reason,
            EmployeeId = assessmentReviewer.EmployeeId
        };

        var testee = new Core.Services.QueryServices.ReviewerTasksService.ReviewerTaskService(employeeRepository, currentUserService, assessmentReviewerRepository);

        //Act
        testee.Decline(assessment.Id, declineRequest);

        //Assert
        assessmentReviewerRepository.Received(1).Update(Arg.Is<AssessmentReviewer>(actualResult => actualResult.IsEquivalentTo(expectedResult)));
    }

    [Test]
    public void Get_ById()
    {
        //Arrange
        var assessment = new Assessment
        {
            Id = 1003,
            AssessmentDueDate = DateTimeOffset.UtcNow,
            PerformanceReviewDate = new DateTime(2022, 1, 1),
            AssessmentState = 0,
            EmployeeId = 191
        };

        var assessmentReviewer = new AssessmentReviewer
        {
            EmployeeId = 562,
            AssessmentId = 1003,
            Assessment = assessment,
            AreasForImprovements = "IDK"
        };

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

        var employee1 = new Employee
        {
            Id = 191,
            SurnameFirstName = "Jirka",
            Position = "Manager",
            Login = "jirik",
            TeamLeaderLogin = teamLeaderLogin,
            AssessmentReviewers = new List<AssessmentReviewer> { assessmentReviewer }

        };

        assessment.Employee = employee1;

        var expectedResult = new GetReviewerTaskResponse
        {
            ReviewerTask = new()
            {
                AssessmentId = assessment.Id,
                SurnameFirstName = assessment.Employee.SurnameFirstName,
                Department = assessment.Employee.Department,
                Position = assessment.Employee.Position,
                ImageSrc = $"https://timur.domain.local:8080/persons/login/{assessment.Employee.Login}/photo",
                AssessmentDueDate = assessment.AssessmentDueDate,
                AreasForImprovements = "IDK",
                TeamLeaderName = teamLeader.SurnameFirstName
            }
        };

        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.UserNameWithoutDomain.Returns(employee1.Login);

        var employeeRepository = Substitute.For<IEmployeesRepository>();
        employeeRepository.GetByLogin(employee1.Login).Returns(employee1);
        employeeRepository.GetWithAllDetails(employee1.Id).Returns(employee1);
        employeeRepository.GetByLogin(teamLeaderLogin).Returns(teamLeader);

        var assessmentReviewerRepository = Substitute.For<IAssessmentReviewerRepository>();

        var testee =
            new Core.Services.QueryServices.ReviewerTasksService.ReviewerTaskService(employeeRepository, currentUserService, assessmentReviewerRepository);

        //Act
        var actualResult = testee.Get(assessment.Id);

        //Assert
        actualResult.Should().BeEquivalentTo(expectedResult);
    }

    [Test]
    public void Draft()
    {
        //Arrange
        var draftRequest = new DraftRequest
        {
            Feedback = "The best project manager ever!",
            AreasForImprovements = "Nothing"
        };

        var assessment = new Assessment
        {
            Id = 1003,
            AssessmentDueDate = DateTimeOffset.UtcNow,
            PerformanceReviewDate = new DateTime(2022, 1, 1),
            AssessmentState = 0,
            EmployeeId = 191
        };

        var assessmentReviewer = new AssessmentReviewer
        {
            EmployeeId = 562,
            AssessmentId = 1003,
            Assessment = assessment,
            AssessmentReviewerState = AssessmentReviewerState.Created,
            Feedback = ""
        };

        var employee1 = new Employee
        {
            Id = 191,
            SurnameFirstName = "Jirka",
            Position = "Manager",
            Login = "jirik",
            AssessmentReviewers = new List<AssessmentReviewer> { assessmentReviewer }
        };

        assessment.Employee = employee1;

        var expectedResult = new AssessmentReviewer
        {
            EmployeeId = 562,
            AssessmentId = 1003,
            Assessment = assessment,
            AssessmentReviewerState = AssessmentReviewerState.Drafted,
            Feedback = "The best project manager ever!",
            AreasForImprovements = "Nothing"
        };

        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.UserNameWithoutDomain.Returns(employee1.Login);

        var employeeRepository = Substitute.For<IEmployeesRepository>();
        employeeRepository.GetByLogin(employee1.Login).Returns(employee1);

        var assessmentReviewerRepository = Substitute.For<IAssessmentReviewerRepository>();
        assessmentReviewerRepository.Get(employee1.Id, assessment.Id).Returns(assessmentReviewer);

        var testee = new Core.Services.QueryServices.ReviewerTasksService.ReviewerTaskService(employeeRepository, currentUserService, assessmentReviewerRepository);

        //Act
        testee.Draft(assessment.Id, draftRequest);

        //Assert
        assessmentReviewerRepository.Received(1).Update(Arg.Is<AssessmentReviewer>(actualResult => actualResult.IsEquivalentTo(expectedResult)));
    }
    [Test]
    public void Submit()
    {
        //Arrange
        var submitRequest = new SubmitRequest
        {
            Feedback = "The best project manager ever!",
            AreasForImprovements = "Nothing"
        };

        var assessment = new Assessment
        {
            Id = 1003,
            AssessmentDueDate = DateTimeOffset.UtcNow,
            PerformanceReviewDate = new DateTime(2022, 1, 1),
            AssessmentState = 0,
            EmployeeId = 191
        };

        var assessmentReviewer = new AssessmentReviewer
        {
            EmployeeId = 562,
            AssessmentId = 1003,
            Assessment = assessment,
            AssessmentReviewerState = AssessmentReviewerState.Created,
            Feedback = ""
        };

        var employee1 = new Employee
        {
            Id = 191,
            SurnameFirstName = "Jirka",
            Position = "Manager",
            Login = "jirik",
            AssessmentReviewers = new List<AssessmentReviewer> { assessmentReviewer }
        };

        assessment.Employee = employee1;

        var expectedResult = new AssessmentReviewer
        {
            EmployeeId = 562,
            AssessmentId = 1003,
            Assessment = assessment,
            AssessmentReviewerState = AssessmentReviewerState.Reviewed,
            Feedback = "The best project manager ever!",
            AreasForImprovements = "Nothing"
        };

        var currentUserService = Substitute.For<ICurrentUserService>();
        currentUserService.UserNameWithoutDomain.Returns(employee1.Login);

        var employeeRepository = Substitute.For<IEmployeesRepository>();
        employeeRepository.GetByLogin(employee1.Login).Returns(employee1);

        var assessmentReviewerRepository = Substitute.For<IAssessmentReviewerRepository>();
        assessmentReviewerRepository.Get(employee1.Id, assessment.Id).Returns(assessmentReviewer);

        var testee =
            new Core.Services.QueryServices.ReviewerTasksService.ReviewerTaskService(employeeRepository, currentUserService, assessmentReviewerRepository);

        //Act
        testee.Submit(assessment.Id, submitRequest);

        //Assert
        assessmentReviewerRepository.Received(1).Update(Arg.Is<AssessmentReviewer>(actualResult => actualResult.IsEquivalentTo(expectedResult)));
    }
}