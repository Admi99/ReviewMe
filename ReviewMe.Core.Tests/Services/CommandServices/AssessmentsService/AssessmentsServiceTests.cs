using ReviewMe.Core.Services.CommandServices.AssessmentsService;

namespace ReviewMe.Core.Tests.Services.CommandServices.AssessmentsService;

[TestFixture]
public class AssessmentsServiceTests
{
    [Test]
    public void OpenAssessment()
    {
        // Arrange

        var assessmentRepository = Substitute.For<IAssessmentsRepository>();
        var employeesRepository = Substitute.For<IEmployeesRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        var notifyAssessment = Substitute.For<IAssessmentsNotificationService>();

        var now = DateTimeOffset.Now;
        var employee1 = new Employee
        {
            Id = 191,
            SurnameFirstName = "Jirka",
            Position = "Manager",
            Login = "jirik"
        };

        var employeeId = 5;
        var assessmentDueDate = DateTimeOffset.Now.Date.AddDays(1);
        var performanceReviewDate = DateTimeOffset.Now.Date.AddDays(2);
        var request = new OpenAssessmentRequest
        {
            AssessmentDueDate = assessmentDueDate,
            PerformanceReviewDate = performanceReviewDate,
            Reviewers = new List<int>
            {
                153
            }
        };

        currentUserService.UserNameWithoutDomain.Returns("jirik");
        employeesRepository.GetByLogin("jirik").Returns(employee1);


        var expectedResult = new Assessment
        {
            PerformanceReviewDate = performanceReviewDate,
            AssessmentDueDate = assessmentDueDate,
            AssessmentState = AssessmentState.Open,
            EmployeeId = employeeId,
            CreatedByEmployeeId = employee1.Id,
            CreatedAt = now,
            AssessmentReviewers = new List<AssessmentReviewer>
            {
                new()
                {
                    EmployeeId = 153
                }
            }
        };

        dateTimeProvider.Now().Returns(now);

        var testee = new Core.Services.CommandServices.AssessmentsService.AssessmentsService(
            assessmentRepository,
            employeesRepository,
            currentUserService,
            dateTimeProvider,
            notifyAssessment);

        // Act
        testee.OpenAssessment(employeeId, request);

        // Assert
        assessmentRepository.Received(1).Add(Arg.Is<Assessment>(actualResult => actualResult.IsEquivalentTo(expectedResult)));
    }

    [Test]
    public void CloseAssessment()
    {
        // Arrange
        var employeeId = 5;

        var assessment = new Assessment
        {
            AssessmentState = AssessmentState.Open,
            EmployeeId = employeeId
        };

        var expectedResult = new Assessment
        {
            AssessmentState = AssessmentState.Closed,
            EmployeeId = employeeId
        };

        var assessmentRepository = Substitute.For<IAssessmentsRepository>();
        assessmentRepository.GetWithReviewers(employeeId, AssessmentState.Open).Returns(assessment);

        var employeesRepository = Substitute.For<IEmployeesRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        var notifyAssessment = Substitute.For<IAssessmentsNotificationService>();

        assessmentRepository.GetWithReviewers(employeeId, AssessmentState.Open).Returns(assessment);

        var testee = new Core.Services.CommandServices.AssessmentsService.AssessmentsService(
            assessmentRepository,
            employeesRepository,
            currentUserService,
            dateTimeProvider,
            notifyAssessment);

        // Act
        testee.CloseAssessment(employeeId);

        // Assert
        assessmentRepository.Received(1).Update(Arg.Is<Assessment>(actualResult => actualResult.IsEquivalentTo(expectedResult)));
    }

    [Test]
    public void UpdateAssessmentTestCase()
    {
        // Arrange
        var employeeId = 5;
        var assessmentDueDate = DateTimeOffset.Now.Date.AddDays(1);
        var performanceReviewDate = DateTimeOffset.Now.Date.AddDays(2);

        var assessment = new Assessment
        {
            PerformanceReviewDate = performanceReviewDate,
            AssessmentDueDate = assessmentDueDate,
            AssessmentState = AssessmentState.Open,
            EmployeeId = employeeId,
            AssessmentReviewers = new List<AssessmentReviewer>
            {
                new()
                {
                    EmployeeId = 151
                }
            }
        };
        var newPerformanceReviewDate = DateTimeOffset.Now.Date.AddDays(3);
        var newAssessmentDueDate = DateTimeOffset.Now.Date.AddDays(4);

        var request = new UpdateAssessmentRequest
        {
            AssessmentDueDate = newAssessmentDueDate,
            PerformanceReviewDate = newPerformanceReviewDate,
            Reviewers = new List<int>
            {
                153
            }
        };
        var expectedResult = new Assessment
        {
            AssessmentDueDate = newPerformanceReviewDate,
            PerformanceReviewDate = newAssessmentDueDate,
            EmployeeId = employeeId,
            AssessmentReviewers = new List<AssessmentReviewer>
            {
                new()
                {
                    EmployeeId = 153
                }
            }
        };

        var assessmentRepository = Substitute.For<IAssessmentsRepository>();
        assessmentRepository.GetWithReviewers(employeeId, AssessmentState.Open).Returns(assessment);

        var employeesRepository = Substitute.For<IEmployeesRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        var notifyAssessment = Substitute.For<IAssessmentsNotificationService>();

        var testee = new Core.Services.CommandServices.AssessmentsService.AssessmentsService(assessmentRepository, employeesRepository, currentUserService, dateTimeProvider, notifyAssessment);

        // Act
        testee.UpdateAssessment(employeeId, request);

        // Assert
        assessmentRepository.Received(1);

        assessmentRepository.Received(1).Update(Arg.Do<Assessment>(actualResult => actualResult.IsEquivalentTo(expectedResult)));

    }


    [Test]
    public void UpdateAssessmentTestCase2()
    {
        // Arrange
        var employeeId = 5;
        var assessmentDueDate = DateTimeOffset.Now.Date.AddDays(1);
        var performanceReviewDate = DateTimeOffset.Now.Date.AddDays(2);

        var assessment = new Assessment
        {
            PerformanceReviewDate = performanceReviewDate,
            AssessmentDueDate = assessmentDueDate,
            AssessmentState = AssessmentState.Open,
            EmployeeId = employeeId,
            AssessmentReviewers = new List<AssessmentReviewer>
            {
                new()
                {
                    EmployeeId = 151
                }
            }
        };
        var newPerformanceReviewDate = DateTimeOffset.Now.Date.AddDays(3);
        var newAssessmentDueDate = DateTimeOffset.Now.Date.AddDays(4);

        var request = new UpdateAssessmentRequest
        {
            AssessmentDueDate = newAssessmentDueDate,
            PerformanceReviewDate = newPerformanceReviewDate,
            Reviewers = new List<int>
            {
                153,151
            }
        };

        var expectedResult = new Assessment
        {
            AssessmentDueDate = newAssessmentDueDate,
            PerformanceReviewDate = newPerformanceReviewDate,
            EmployeeId = employeeId,
            AssessmentReviewers = new List<AssessmentReviewer>
            {
                new()
                {
                    EmployeeId = 151
                },
                new()
                {
                EmployeeId = 153
                }
            }
        };

        var assessmentRepository = Substitute.For<IAssessmentsRepository>();
        assessmentRepository.GetWithReviewers(employeeId, AssessmentState.Open).Returns(assessment);

        var employeesRepository = Substitute.For<IEmployeesRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        var notifyAssessment = Substitute.For<IAssessmentsNotificationService>();

        var testee = new Core.Services.CommandServices.AssessmentsService.AssessmentsService(assessmentRepository, employeesRepository, currentUserService, dateTimeProvider, notifyAssessment);

        // Act
        testee.UpdateAssessment(employeeId, request);

        // Assert
        assessmentRepository.Received(1).Update(Arg.Do<Assessment>(actualResult => actualResult.IsEquivalentTo(expectedResult)));

    }

    [Test]
    public void UpdateAssessmentTestCase3()
    {
        // Arrange
        var employeeId = 5;
        var assessmentDueDate = DateTimeOffset.Now.Date.AddDays(1);
        var performanceReviewDate = DateTimeOffset.Now.Date.AddDays(2);

        var assessment = new Assessment
        {
            PerformanceReviewDate = performanceReviewDate,
            AssessmentDueDate = assessmentDueDate,
            AssessmentState = AssessmentState.Open,
            EmployeeId = employeeId,
            AssessmentReviewers = new List<AssessmentReviewer>
            {
                new()
                {
                    EmployeeId = 151,
                    Feedback = "FeedBack from Employee 151"
                },
                new()
                {
                EmployeeId = 153,
                Feedback = "FeedBack from Employee 153"
                }
            }
        };
        var newPerformanceReviewDate = DateTimeOffset.Now.Date.AddDays(3);
        var newAssessmentDueDate = DateTimeOffset.Now.Date.AddDays(4);

        var request = new UpdateAssessmentRequest
        {
            AssessmentDueDate = newAssessmentDueDate,
            PerformanceReviewDate = newPerformanceReviewDate,
            Reviewers = new List<int>
            {
                151
            }
        };

        var expectedResult = new Assessment
        {
            AssessmentDueDate = newAssessmentDueDate,
            PerformanceReviewDate = newPerformanceReviewDate,
            EmployeeId = employeeId,
            AssessmentReviewers = new List<AssessmentReviewer>
            {
                new()
                {
                    EmployeeId = 151,
                    Feedback = "FeedBack from Employee 151"
                },

            }
        };

        var assessmentRepository = Substitute.For<IAssessmentsRepository>();
        assessmentRepository.GetWithReviewers(employeeId, AssessmentState.Open).Returns(assessment);

        var employeesRepository = Substitute.For<IEmployeesRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        var notifyAssessment = Substitute.For<IAssessmentsNotificationService>();

        var testee = new Core.Services.CommandServices.AssessmentsService.AssessmentsService(
            assessmentRepository,
            employeesRepository,
            currentUserService,
            dateTimeProvider,
            notifyAssessment);

        // Act
        testee.UpdateAssessment(employeeId, request);

        // Assert
        assessmentRepository.Received(1).Update(Arg.Do<Assessment>(actualResult => actualResult.IsEquivalentTo(expectedResult)));

    }

    [Test]
    public void DeleteAssessment()
    {
        // Arrange
        var employeeId = 5;
        var assessment = new Assessment
        {
            AssessmentState = AssessmentState.Open,
            EmployeeId = employeeId
        };

        var expectedResult = new Assessment
        {
            AssessmentState = AssessmentState.Deleted,
            EmployeeId = employeeId
        };

        var assessmentRepository = Substitute.For<IAssessmentsRepository>();
        var employeesRepository = Substitute.For<IEmployeesRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        var notifyAssessment = Substitute.For<IAssessmentsNotificationService>();

        assessmentRepository.GetWithReviewers(employeeId, AssessmentState.Open).Returns(assessment);

        var testee = new Core.Services.CommandServices.AssessmentsService.AssessmentsService(
            assessmentRepository,
            employeesRepository,
            currentUserService,
            dateTimeProvider,
            notifyAssessment);

        // Act
        testee.DeleteAssessment(employeeId);

        // Assert
        assessmentRepository.Received(1).Update(Arg.Is<Assessment>(actualResult => actualResult.IsEquivalentTo(expectedResult)));
    }

    [Test]
    public void SaveAdditionalFeedback()
    {
        // Arrange
        var employeeId = 5;

        var assessment = new Assessment
        {
            Id = 1,
            AssessmentState = AssessmentState.Open,
            EmployeeId = employeeId
        };

        var expectedResult = new Assessment
        {
            EmployeeId = employeeId,
            AdditionalFeedback = "Additional feedback to LSR."
        };

        var request = new SaveAdditionalFeedbackRequest
        {
            Feedback = "Additional feedback to LSR"
        };

        var assessmentRepository = Substitute.For<IAssessmentsRepository>();
        assessmentRepository.GetWithReviewers(employeeId, AssessmentState.Open).Returns(assessment);

        var employeesRepository = Substitute.For<IEmployeesRepository>();
        var currentUserService = Substitute.For<ICurrentUserService>();
        var dateTimeProvider = Substitute.For<IDateTimeProvider>();
        var notifyAssessment = Substitute.For<IAssessmentsNotificationService>();

        var testee = new Core.Services.CommandServices.AssessmentsService.AssessmentsService(assessmentRepository, employeesRepository, currentUserService, dateTimeProvider, notifyAssessment);

        // Act
        testee.SaveAdditionalFeedback(employeeId, request);

        // Assert
        assessmentRepository.Received(1).Update(Arg.Do<Assessment>(actualResult => actualResult.Should().BeEquivalentTo(expectedResult)));
    }
}